using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace RmsAuto.Common.Linq
{
    public static class DynamicSorting
    {
        class DynamicOrdering<T>
        {
            public bool Descending { get; set; }
            public Expression<Func<T, object>> SortExpression { get; set; }
        }

        /// <param name="sortExpression">Standard ORDER BY clause extended with "Method()" term for non-overloaded functons</param>
        /// <param name="sortExpressionArgs">All function arguments in same order as function names appear in sortExpression</param>
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> queryable,
            string sortExpression, params object[] sortExpressionArgs)
        {
            if (queryable == null)
                throw new ArgumentNullException("queryable");
            if (string.IsNullOrEmpty(sortExpression))
                return queryable;

            IOrderedQueryable<T> ret = null;
            foreach (var ordering in ParseSortExpression<T>(sortExpression, sortExpressionArgs))
            {
                if (ret == null)
                {
                    ret = !ordering.Descending ?
                        queryable.OrderBy(ordering.SortExpression) :
                        queryable.OrderByDescending(ordering.SortExpression);
                }
                else
                {
                    ret = !ordering.Descending ?
                     ret.ThenBy(ordering.SortExpression) :
                     ret.ThenByDescending(ordering.SortExpression);
                }

            }
            return ret;
        }

        private static IEnumerable<DynamicOrdering<T>> ParseSortExpression<T>(string sortExpression, object[] sortExpressionArgs)
        {
            foreach (var part in sortExpression.Split(',').Select(s => s.Trim()))
            {
                int spaceIndex = part.IndexOf('\x20');
                yield return new DynamicOrdering<T>
                {
                    SortExpression = CreateSortExpression<T>(spaceIndex == -1 ? part : part.Substring(0, spaceIndex),
                        ref sortExpressionArgs),
                    Descending = spaceIndex == -1 ?
                        false :
                        part.Substring(spaceIndex + 1).Equals("desc", StringComparison.OrdinalIgnoreCase)
                };
            }
        }

        private static Expression<Func<T, object>> CreateSortExpression<T>(string memberName, ref object[] sortExpressionArgs)
        {
            var paramEx = Expression.Parameter(typeof(T), "");
            Expression memberEx;
            if (memberName.EndsWith("()"))
            {
                memberName = memberName.Substring(0, memberName.Length - 2);

                MethodInfo mi = typeof(T).GetMethod(memberName);
                if (mi == null) throw new Exception(memberName + " method is not defined");

                ParameterInfo[] prms = mi.GetParameters();
                if (prms.Length < sortExpressionArgs.Length)
                    throw new ArgumentException("Insufficient number of arguments");

                object[] args = sortExpressionArgs.Take(prms.Length).ToArray();
                sortExpressionArgs = sortExpressionArgs.Skip(prms.Length).ToArray();

                Expression[] argsEx = prms.Select<ParameterInfo, Expression>(
                    pi => Expression.Constant(args[pi.Position], pi.ParameterType)).ToArray();
                memberEx = Expression.Call(paramEx, mi, argsEx);
            }
            else
            {
                memberEx = Expression.Property(paramEx, memberName);
            }
            var castEx = Expression.TypeAs(memberEx, typeof(object));
            return Expression.Lambda<Func<T, object>>(castEx, paramEx);
        }
    }
}
