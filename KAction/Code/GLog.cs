using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public static class GLog
    {
        public static void Print(string msg, bool useColor = false, Color color = default)
        {
#if GLOG
            if (GameLevel.Instance == null || GameLevel.Instance.DebugMessage == false) { return; }
            if (useColor)
            {
                var toPrint = string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(color.r * 255f),
                    (byte)(color.g * 255f), (byte)(color.b * 255f), msg);
                Debug.Log(toPrint);
            }
            else
            {
                Debug.Log(msg);
            }
#endif
        }

        public static void PrintError(string msg)
        {
#if GLOG
            if (GameLevel.Instance == null || GameLevel.Instance.DebugMessage == false) { return; }
            Debug.LogError(msg);
#endif
        }
    }
}