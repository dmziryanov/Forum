using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Collections;

namespace System.Linq
{
    public static class LinqExtensions
    {
        public static IEnumerable<T> Each<T>(this IEnumerable<T> enu, Action<T> action)
        {
            if (action != null)
            {
                foreach (T item in enu)
                {
                    action(item);
                }
            }
            return enu;
        }

        public static IEnumerable<IEnumerable<T>> Slice<T>(this IEnumerable<T> enu, int num)
        {
            return enu.Slice(num, false);
        }

        public static IEnumerable<IEnumerable<T>> Slice<T>(this IEnumerable<T> enu, int num, bool over)
        {
            if (num <= 0)
            {
                throw new ArgumentException("Num >= 0");
            }

            int cCount = enu.Count();

            if (num > cCount && !over)
            {
                throw new ArgumentException("Num <= IEnumerable<T>.Count()");
            }

            int sliceSize = (int)Math.Ceiling((double)cCount / (double)num);

            List<IEnumerable<T>> toRet = new List<IEnumerable<T>>(num);

            for (int i = 0; i < num; i++)
            {
                toRet.Add(enu.Skip(i * sliceSize).Take(sliceSize).ToList());
            }

            return toRet;
        }
    }
}
