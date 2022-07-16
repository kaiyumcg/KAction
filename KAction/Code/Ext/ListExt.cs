using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public static class ListExt
    {
        public static bool ContainsOPT<T>(this List<T> lst, T item)
        {
            bool contains = false;
            if (lst != null)
            {
                var len = lst.Count;
                if (len > 0)
                {
                    for(int i = 0; i < len;i++)
                    {
                        if(ReferenceEquals(lst[i], item))
                        {
                            contains = true;
                            break;
                        }
                    }
                }
            }
            return contains;
        }

        public static bool HasAtleastOneItemIn<T>(this List<T> lst, List<T> inlst, bool useRefOptimization = false)
        {
            bool hasIt = false;
            if (lst != null && inlst != null)
            {
                var tCount = lst.Count;
                if (tCount > 0)
                {
                    for (int i = 0; i < tCount; i++)
                    {
                        var t = lst[i];
                        if (useRefOptimization ? inlst.ContainsOPT(t) : inlst.Contains(t))
                        {
                            hasIt = true;
                            break;
                        }
                    }
                }
            }
            return hasIt;
        }

        public static bool HasAtleastOneItemIn<T>(this List<T> lst, List<T> inlst, int lCount, bool useRefOptimization = false)
        {
            bool hasIt = false;
            if (lst != null && lCount > 0 && inlst != null)
            {
                for (int i = 0; i < lCount; i++)
                {
                    var t = lst[i];
                    if (useRefOptimization ? inlst.ContainsOPT(t) : inlst.Contains(t))
                    {
                        hasIt = true;
                        break;
                    }
                }
            }
            return hasIt;
        }

        public static bool FoundAllIn<T>(this T[] lst, List<T> inlst, bool useRefOptimization = false)
        {
            bool found = false;
            if (lst != null && inlst != null)
            {
                var tCount = lst.Length;
                if (tCount > 0)
                {
                    int containNum = 0;
                    for (int i = 0; i < tCount; i++)
                    {
                        var t = lst[i];
                        if (useRefOptimization ? inlst.ContainsOPT(t) : inlst.Contains(t))
                        {
                            containNum++;
                        }
                    }

                    if (containNum == tCount)
                    {
                        found = true;
                    }
                }
            }
            return found;
        }

        public static List<T> RemoveAllNulls<T>(this List<T> lst) where T : class
        {
            List<T> result = new List<T>();
            if (lst != null)
            {
                var len = lst.Count;
                if (len > 0)
                {
                    for (int i = 0; i < len; i++)
                    {
                        var item = lst[i];
                        if (item == null) { continue; }
                        result.Add(item);
                    }
                }
            }
            return result;
        }

        public static List<T> RemoveAllIfLogicTrue<T>(this List<T> lst, ListDel1<T> pred) where T : class
        {
            List<T> result = new List<T>();
            if (lst != null)
            {
                var len = lst.Count;
                if (len > 0)
                {
                    for (int i = 0; i < len; i++)
                    {
                        var item = lst[i];
                        var isTrue = pred == null ? false : pred(item);
                        if (isTrue) { continue; }
                        result.Add(item);
                    }
                }
            }
            return result;
        }

        public delegate bool ListDel1<T>(T t);
    }
}