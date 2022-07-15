using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public static class DSUtil
    {
        internal static bool IsEqual<T>(T t1, T t2, bool valueType, bool useOptimization)
        {
            if (valueType) { return t1.Equals(t2); }
            else 
            {
                if (useOptimization)
                {
                    return ReferenceEquals(t1, t2);
                }
                else
                {
                    if (t1 == null && t2 == null)
                    {
                        return true;
                    }
                    else if ((t1 == null && t2 != null) || (t1 != null && t2 == null))
                    {
                        return false;
                    }
                    else
                    {
                        return t1.Equals(t2);
                    }
                }
            }
        }

        public static bool IsEqual<T>(T t1, T t2, bool useOptimization) where T : class
        {
            var useOpt = false;
#if !Turn_Off_RefEqual_Optmization
            useOpt = useOptimization;       
#endif
            if (useOpt)
            {
                return ReferenceEquals(t1, t2);
            }
            else
            {
                return t1 == t2;
            }
        }

        internal static bool IsNull<T>(T t, bool valueType, bool useOptimization)
        {
            if (valueType) { return false; }
            else 
            {
                if (useOptimization)
                {
                    return ReferenceEquals(t, null);
                }
                else
                {
                    return t == null;
                }
            }
        }

        public static bool IsNull<T>(T t, bool useOptimization) where T : class
        {
            var useOpt = false;
#if !Turn_Off_RefEqual_Optmization
            useOpt = useOptimization;       
#endif
            if (useOpt)
            {
                return ReferenceEquals(t, null);
            }
            else
            {
                return t == null;
            }
        }
    }
}