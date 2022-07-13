using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public static class ListExt
    {
        public static bool HasItemIn<T>(this List<T> lst, List<T> inlst)
        {
            bool hasIt = false;
            if (lst != null)
            {
                var tCount = lst.Count;
                if (tCount > 0)
                {
                    for (int i = 0; i < tCount; i++)
                    {
                        var t = lst[i];
                        if (inlst != null && inlst.Contains(t))
                        {
                            hasIt = true;
                            break;
                        }
                    }
                }
            }
            return hasIt;
        }

        public static bool HasItemIn<T>(this List<T> lst, List<T> inlst, int lCount)
        {
            bool hasIt = false;

            if (lst != null && lCount > 0)
            {
                for (int i = 0; i < lCount; i++)
                {
                    var t = lst[i];
                    if (inlst != null && inlst.Contains(t))
                    {
                        hasIt = true;
                        break;
                    }
                }
            }
            return hasIt;
        }
    }
}