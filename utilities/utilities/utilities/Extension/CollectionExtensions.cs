///
/// <author>lufty.abdillah@gmail.com</author>
/// <summary>
/// Toyota .Net Development Kit
/// Copyright (c) Toyota Motor Manufacturing Indonesia, All Right Reserved.
/// </summary>
///
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;

namespace Toyota.Common.Utilities
{
    public static class CollectionExtensions
    {
        public static bool IsNullOrSizeLessThan<T>(this ICollection<T> list, int size)
        {
            return (list == null) || (list.Count < size);
        }
        public static bool IsNullOrEmpty<T>(this ICollection<T> list)
        {
            return (list == null) || (list.Count == 0);
        }
        public static void IterateByAction<T>(this ICollection<T> list, Action<T> action)
        {
            if (!IsNullOrEmpty<T>(list))
            {
                foreach (T listItem in list)
                {
                    action.Invoke(listItem);
                }
            }
        }
        public static void IterateByAction<T>(this ICollection<T> list, Predicate<T> action)
        {
            if (!IsNullOrEmpty<T>(list))
            {
                bool keepLoop;
                foreach (T listItem in list)
                {
                    keepLoop = action.Invoke(listItem);
                    if (!keepLoop)
                    {
                        break;
                    }
                }
            }
        }
        public static bool IsElementExists<T>(this ICollection<T> list, Func<T, bool> function)
        {
            bool found = false;
            if (!list.IsNullOrEmpty())
            {                
                foreach (T listItem in list)
                {
                    found = function.Invoke(listItem);
                    if (found)
                    {
                        break;
                    }
                }
            }
            return found;
        }
        public static T FindElement<T>(this ICollection<T> list, Predicate<T> criteria)
        {
            bool found = false;
            if (!list.IsNullOrEmpty())
            {   
                foreach (T listItem in list)
                {
                    found = criteria.Invoke(listItem);
                    if (found)
                    {
                        return listItem;
                    }
                }
            }
            return default(T);
        }
        public static ICollection<T> Merge<T>(this ICollection<T> target, ICollection<T> source)
        {
            if (!target.IsNull() && !source.IsNullOrEmpty())
            {
                foreach (T t in source)
                {
                    target.Add(t);
                }
            }
            return target;
        }
        public static ICollection<T> Merge<T>(this ICollection<T> target, T[] source)
        {
            if (!target.IsNull() && !source.IsNullOrEmpty())
            {
                foreach (T t in source)
                {
                    target.Add(t);
                }
            }
            return target;
        }        
        public static ICollection<T> Extend<T>(this ICollection<T> target, Action<ICollection<T>> action)
        {
            if (!target.IsNull())
            {
                action.Invoke(target);
            }
            return target;
        }
        public static IList<T> ExtendAsList<T>(this ICollection<T> target, Action<ICollection<T>> action)
        {
            if (!target.IsNull())
            {
                action.Invoke(target);
            }
            return new List<T>(target);
        }
        public static void AddIfConditionAchieved<T>(this ICollection<T> target, T obj, Predicate<T> action)
        {
            if (!target.IsNull() && !obj.IsNull())
            {
                bool passed = false;
                if (action != null)
                {
                    passed = action.Invoke(obj);
                }
                if (passed)
                {
                    target.Add(obj);
                }
            }            
        }
        public static void AddIfAllowed<T>(this ICollection<T> target, T obj, bool flag)
        {
            if(flag && !target.IsNull() && !obj.IsNull()) 
            {
                target.Add(obj);
            }
        }
        public static void AddIfNotNull<T>(this ICollection<T> target, T obj)
        {
            if (!target.IsNull() && !obj.IsNull())
            {
                target.Add(obj);
            }
        }
        public static void AddIfStringIsNotNullOrEmpty<T>(this ICollection<T> target, T obj, string str)
        {
            if (!target.IsNull() && !obj.IsNull() && !str.IsNullOrEmpty())
            {
                target.Add(obj);
            }
        }
        public static ICollection<T> IteratedAddition<T>(this ICollection<T> target, int startIndex, int size, Func<int, T> action) 
        {
            if (!target.IsNull() && !action.IsNull())
            {
                for (int i = startIndex; i < size; i++)
                {
                    target.Add(action.Invoke(i));
                }
            }
            return target;
        }
        public static ICollection<T> IteratedAddition<T>(this ICollection<T> target, int size, Func<int, T> action)
        {
            return target.IteratedAddition(0, size, action);
        }
        public static void FindAgainst<T>(this ICollection<T> target, ICollection<T> other, Func<T, T, bool> criteria, Action<T, T> foundAction)
        {
            if (!target.IsNullOrEmpty() && !other.IsNullOrEmpty())
            {
                bool passed;
                foreach (T t in target)
                {
                    passed = false;
                    foreach (T o in other)
                    {
                        passed = criteria.Invoke(t, o);
                        if (passed)
                        {
                            foundAction(t, o);
                            break;
                        }
                    }

                    if (passed)
                    {
                        break;
                    }
                }
            }
        }

        public static long Count(this IEnumerable target)
        {
            long count = 0;
            if (!target.IsNull())
            {
                IEnumerator enumerator = target.GetEnumerator();
                if (!enumerator.IsNull())
                {
                    while (enumerator.MoveNext())
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public static bool IsNullOrEmpty<T>(this T[] array)
        {
            return (array == null) || (array.Length == 0);
        }
        public static IDictionary<T, U> Merge<T, U>(this IDictionary<T, U> target, IDictionary<T, U> source)
        {
            if (!target.IsNull() && !source.IsNullOrEmpty())
            {
                foreach (T key in source.Keys)
                {
                    target.Add(key, source[key]);
                }
            }
            return target;
        }
        public static IDictionary<T, U> Extend<T, U>(this IDictionary<T, U> target, Action<IDictionary<T, U>> action)
        {
            if (action != null)
            {
                action.Invoke(target);
            }
            return target;
        }

        public static void EnumerateAsKeyValuePair(this IEnumerable<object> list, string keyField, string valueField, Action<object, object> action) 
        {
            if (list != null)
            {
                PropertyInfo key, value;
                Type type;
                foreach (object obj in list)
                {
                    type = obj.GetType();
                    key = type.GetProperty(keyField);
                    value = type.GetProperty(valueField);
                    if (!key.IsNull())
                    {
                        action.Invoke(key.GetValue(obj, null), (value != null) ? value.GetValue(obj, null) : null);
                    }
                }
            }
        }
        public static void EnumerateAsKeyValuePair(this IEnumerable<object> list, Action<object, object> action)
        {
            EnumerateAsKeyValuePair(list, "Key", "Value", action);
        }
    }
}
