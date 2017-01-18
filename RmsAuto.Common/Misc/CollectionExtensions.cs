using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Web;

namespace RmsAuto.Common.Misc
{
    public static class CollectionExtensions
    {
        public static string ToWwwQueryString(this NameValueCollection collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            var stringBuilder = new StringBuilder();
            for(int i =0; i<collection.Count; ++i)
            {
                if (stringBuilder.Length > 0)
					stringBuilder.Append( '&' ); 
				string key = collection.GetKey( i );
				foreach( string value in collection.GetValues( i ) )
				{
					stringBuilder.Append( key + "=" + HttpUtility.UrlEncode( value ) );
				}
            }
            return stringBuilder.ToString();
        }

        public static string GetAndRemove(this NameValueCollection collection, string key)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (key == null)
                throw new ArgumentNullException("key");

            string value = collection[key];
            if (value != null)
                collection.Remove(key);
            return value;
        }
    }
}

namespace System.Collections.Specialized
{
    public static class NameValueCollectionExtension
    {
        public static T TryGet<T>(this NameValueCollection collection, string keyValue, T defValue)
        {
            if (collection == null)
            {
                return defValue;
            }

            try
            {
                var val = collection[keyValue];
                if (val == null)
                {
                    return defValue;
                }
                T retVal = (T)Convert.ChangeType(val, typeof(T));
                return retVal.Equals(default(T)) ? defValue : retVal;
            }
            catch (InvalidCastException)
            {
                return defValue;
            }
            catch (FormatException)
            {
                return defValue;
            }
        }
        public static T TryGet<T>(this NameValueCollection collection, string keyValue)
        {
            return TryGet<T>(collection, keyValue, default(T));
        }

        public static T Get<T>(this NameValueCollection collection, string keyValue)
        {
            if (collection == null)
            {
                throw new ArgumentException("Collection is null");
            }

            var val = collection[keyValue];
            if (val == null)
            {
                throw new ArgumentException("Collection contains no item with given key");
            }
            return (T)Convert.ChangeType(val, typeof(T));
        }
    }
}

namespace System.Web.UI
{
    public static class AttributesCollectionExt
    {
        public static void Each(this System.Web.UI.AttributeCollection col, Action<KeyValuePair<string, string>> act)
        {
            if (act != null)
            {
                foreach (string key in col.Keys)
                {
                    act(new KeyValuePair<string, string>(key, col[key]));
                }
            }
        }
    }
}

namespace System.Collections
{
    public static class IEnumerableExt
    {
        public static IEnumerable Each<T>(this IEnumerable enu, Action<T> action)
        {
            if (action != null && action.Method != null)
            {
                foreach (T item in enu)
                {
                    action(item);
                }
            }
            return enu;
        }
    }
}

namespace System.Collections.Generic
{
    public static class ListExt
    {
        /// <summary>
        /// Вычитание слиянием. Оба списка должны быть упорядочены, это важно.
        /// Дает результат O(N) вместо O(N^2) в случае вложенных циклов.
        /// </summary>
        /// <typeparam name="T">Тип, поддерживающий сравнение</typeparam>
        /// <param name="from">Откуда вычитаем</param>
        /// <param name="what">Что вычитаем</param>
        /// <returns>Результат вычитания</returns>
        public static List<T> Subtract<T>(this List<T> from, List<T> what) where T : IComparable<T>
        {
            if (from == null)
            {
                throw new ArgumentNullException("from");
            }
            if (what == null)
            {
                throw new ArgumentNullException("what");
            }
            if (what.Count == 0 || from.Count == 0)
            {
                return from;
            }

            int fromLastIdx = from.Count;
            int whatLastIdx = what.Count;
            List<T> result = new List<T>();
            int idx1 = 0, idx2 = 0;

            while (idx1 < fromLastIdx && idx2 < whatLastIdx)
            {
                if (from[idx1].CompareTo(what[idx2]) < 0)
                {
                    result.Add(from[idx1++]);
                    continue;
                }
                else if (what[idx2].CompareTo(from[idx1]) < 0)
                {
                    idx2++;
                    continue;
                }
                idx1++;
                idx2++;
            }

            while (idx1 < fromLastIdx)
            {
                result.Add(from[idx1++]);
            }

            return result;
        }

        /// <summary>
        /// Вычитание слиянием. Оба списка должны быть упорядочены, это важно.
        /// Дает результат O(N) вместо O(N^2) в случае вложенных циклов.        /// </summary>
        /// <typeparam name="T">Тип списковых элементов</typeparam>
        /// <param name="from">Откуда вычитаем</param>
        /// <param name="what">Что вычитаем</param>
        /// <param name="comparer">IComparer для T</param>
        /// <returns>Результат вычитания</returns>
        public static List<T> Subtract<T>(this List<T> from, List<T> what, IComparer<T> comparer)
        {
            if (from == null)
            {
                throw new ArgumentNullException("from");
            }
            if (what == null)
            {
                throw new ArgumentNullException("what");
            }
            if (what.Count == 0 || from.Count == 0)
            {
                return from;
            }

            int fromLastIdx = from.Count;
            int whatLastIdx = what.Count;
            List<T> result = new List<T>();
            int idx1 = 0, idx2 = 0;

            while (idx1 < fromLastIdx && idx2 < whatLastIdx)
            {
                if (comparer.Compare(from[idx1], what[idx2]) < 0)
                {
                    result.Add(from[idx1++]);
                    continue;
                }
                else if (comparer.Compare(what[idx2], from[idx1]) < 0)
                {
                    idx2++;
                    continue;
                }
                idx1++;
                idx2++;
            }

            while (idx1 < fromLastIdx)
            {
                result.Add(from[idx1++]);
            }

            return result;
        }

        /// <summary>
        /// Объединение (join) слиянием. Оба списка должны быть упорядочены, это важно.
        /// Дает результат O(N) вместо O(N^2) в случае вложенных циклов.
        /// </summary>
        /// <typeparam name="T">Тип, поддерживающий сравнение</typeparam>
        /// <param name="from">Откуда вычитаем</param>
        /// <param name="what">Что вычитаем</param>
        /// <returns>Результат вычитания</returns>
        public static List<T> IntersectSorted<T>(this List<T> from, List<T> what, bool takeFromWhatList) where T : IComparable<T>
        {
            if (from == null)
            {
                throw new ArgumentNullException("from");
            }
            if (what == null)
            {
                throw new ArgumentNullException("what");
            }
            if (from.Count == 0 || what.Count == 0)
            {
                return new List<T>();
            }

            int fromLastIdx = from.Count;
            int whatLastIdx = what.Count;
            List<T> result = new List<T>();
            int idx1 = 0, idx2 = 0;

            while (idx1 < fromLastIdx && idx2 < whatLastIdx)
            {
                if (from[idx1].CompareTo(what[idx2]) < 0)
                {
                    idx1++;
                    continue;
                }
                else if (what[idx2].CompareTo(from[idx1]) < 0)
                {
                    idx2++;
                    continue;
                }

                if (takeFromWhatList)
                {
                    result.Add(what[idx2++]);
                    idx1++;
                }
                else
                {
                    result.Add(from[idx1++]);
                    idx2++;
                }
            }

            return result;
        }

        /// <summary>
        /// Объединение (join) слиянием. Оба списка должны быть упорядочены, это важно.
        /// Дает результат O(N) вместо O(N^2) в случае вложенных циклов.
        /// </summary>
        /// <typeparam name="T">Тип элементов списка</typeparam>
        /// <param name="from">Откуда вычитаем</param>
        /// <param name="what">Что вычитаем</param>
        /// <param name="comparer">Компарер для T</param>
        /// <returns>Результат вычитания</returns>
        public static List<T> IntersectSorted<T>(this List<T> from, List<T> what, IComparer<T> comparer, bool takeFromWhatList)
        {
            if (from == null)
            {
                throw new ArgumentNullException("from");
            }
            if (what == null)
            {
                throw new ArgumentNullException("what");
            }
            if (from.Count == 0 || what.Count == 0)
            {
                return new List<T>();
            }

            int fromLastIdx = from.Count;
            int whatLastIdx = what.Count;
            List<T> result = new List<T>();
            int idx1 = 0, idx2 = 0;

            while (idx1 < fromLastIdx && idx2 < whatLastIdx)
            {
                if (comparer.Compare(from[idx1], what[idx2]) < 0)
                {
                    idx1++;
                    continue;
                }
                else if (comparer.Compare(what[idx2], from[idx1]) < 0)
                {
                    idx2++;
                    continue;
                }

                if (takeFromWhatList)
                {
                    result.Add(what[idx2++]);
                    idx1++;
                }
                else
                {
                    result.Add(from[idx1++]);
                    idx2++;
                }
            }

            return result;
        }

        /// <summary>
        /// Слияние + дистинкт. Оба списка должны быть упорядочены, это важно.
        /// Опять же за счет упорядочивания преимущество O(N) вместо O(N^2)
        /// </summary>
        /// <typeparam name="T">Тип, поддерживающий сравнение</typeparam>
        /// <param name="from">Первый список</param>
        /// <param name="what">Второй список</param>
        /// <returns>Слитый резалт</returns>
        public static List<T> UnionAndDistinct<T>(this List<T> first, List<T> second) where T : IComparable<T>
        {
            if (first == null)
            {
                throw new ArgumentNullException("from");
            }
            if (second == null)
            {
                throw new ArgumentNullException("what");
            }
            if (second.Count == 0)
            {
                return first;
            }

            int fromLastIdx = first.Count;
            int whatLastIdx = second.Count;
            List<T> result = new List<T>();
            int idx1 = 0, idx2 = 0;

            while (idx1 < fromLastIdx && idx2 < whatLastIdx)
            {
                if (first[idx1].CompareTo(second[idx2]) < 0)
                {
                    result.Add(first[idx1++]);
                    continue;
                }
                else if (second[idx2].CompareTo(first[idx1]) < 0)
                {
                    result.Add(second[idx2++]);
                    continue;
                }
                result.Add(first[idx1++]);
                idx2++;
            }

            while (idx1 < fromLastIdx)
            {
                result.Add(first[idx1++]);
            }
            while (idx2 < whatLastIdx)
            {
                result.Add(second[idx2++]);
            }

            return result;
        }

        /// <summary>
        /// Слияние + дистинкт. Оба списка должны быть упорядочены, это важно.
        /// Опять же за счет упорядочивания преимущество O(N) вместо O(N^2)
        /// </summary>
        /// <typeparam name="T">Тип элементов списка</typeparam>
        /// <param name="from">Первый список</param>
        /// <param name="what">Второй список</param>
        /// <param name="comparer">Компарер для T</param>
        /// <returns>Слитый резалт</returns>
        public static List<T> UnionAndDistinct<T>(this List<T> first, List<T> second, IComparer<T> comparer)
        {
            if (first == null)
            {
                throw new ArgumentNullException("from");
            }
            if (second == null)
            {
                throw new ArgumentNullException("what");
            }
            if (second.Count == 0)
            {
                return first;
            }

            int fromLastIdx = first.Count;
            int whatLastIdx = second.Count;
            List<T> result = new List<T>();
            int idx1 = 0, idx2 = 0;

            while (idx1 < fromLastIdx && idx2 < whatLastIdx)
            {
                if (comparer.Compare(first[idx1], second[idx2]) < 0)
                {
                    result.Add(first[idx1++]);
                    continue;
                }
                else if (comparer.Compare(second[idx2], first[idx1]) < 0)
                {
                    result.Add(second[idx2++]);
                    continue;
                }
                result.Add(first[idx1++]);
                idx2++;
            }

            while (idx1 < fromLastIdx)
            {
                result.Add(first[idx1++]);
            }
            while (idx2 < whatLastIdx)
            {
                result.Add(second[idx2++]);
            }

            return result;
        }

        /// <summary>
        /// Аналогично методу IntersectSorted, но позволяет сохранить одинаковые члены из первого списка.
        /// Например если в первом списке { 1, 1 }, а второй { 1 }. IntersectSorted даст { 1 }, а данный метод - { 1, 1 }
        /// </summary>
        public static List<T> CheckItemsInSecondList<T>(this List<T> from, List<T> what, IComparer<T> comparer)
        {
            if (from == null)
            {
                throw new ArgumentNullException("from");
            }
            if (what == null)
            {
                throw new ArgumentNullException("what");
            }
            if (from.Count == 0 || what.Count == 0)
            {
                return new List<T>();
            }

            int fromLastIdx = from.Count;
            int whatLastIdx = what.Count;
            List<T> result = new List<T>();
            int idx1 = 0, idx2 = 0;

            while (idx1 < fromLastIdx && idx2 < whatLastIdx)
            {
                if (comparer.Compare(from[idx1], what[idx2]) < 0)
                {
                    idx1++;
                    continue;
                }
                else if (comparer.Compare(what[idx2], from[idx1]) < 0)
                {
                    idx2++;
                    continue;
                }
                result.Add(from[idx1++]);
                //idx2++;
            }

            return result;
        }

        /// <summary>
        /// Аналогично методу IntersectSorted, но позволяет сохранить одинаковые члены из первого списка. 
        /// Например если в первом списке { 1, 1 }, а второй { 1 }. IntersectSorted даст { 1 }, а данный метод - { 1, 1 }
        /// </summary>
        public static List<T> CheckItemsInSecondList<T>(this List<T> from, List<T> what) where T : IComparable<T>
        {
            if (from == null)
            {
                throw new ArgumentNullException("from");
            }
            if (what == null)
            {
                throw new ArgumentNullException("what");
            }
            if (from.Count == 0 || what.Count == 0)
            {
                return new List<T>();
            }

            int fromLastIdx = from.Count;
            int whatLastIdx = what.Count;
            List<T> result = new List<T>();
            int idx1 = 0, idx2 = 0;

            while (idx1 < fromLastIdx && idx2 < whatLastIdx)
            {
                if (from[idx1].CompareTo(what[idx2]) < 0)
                {
                    idx1++;
                    continue;
                }
                else if (what[idx2].CompareTo(from[idx1]) < 0)
                {
                    idx2++;
                    continue;
                }
                result.Add(from[idx1++]);
                //idx2++;
            }

            return result;
        }
    }
}
