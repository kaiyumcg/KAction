using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

namespace GameplayFramework
{
    public static class KLog
    {
        static void AddLogToGameLevel(string msg, LogType logType)
        {
            var size = GameLevel.Instance.RuntimeCloudLogSize;
            if (size == LogDataSize.Minimal)
            {
                var lData = GetCurrentMinimalLogData(msg, logType);
                if (GameLevel.Instance.gameplayLogs_min == null) { GameLevel.Instance.gameplayLogs_min = new List<LogDataMinimal>(); }
                GameLevel.Instance.gameplayLogs_min.Add(lData);
            }
            else if (size == LogDataSize.Optimal)
            {
                var lData = GetCurrentOptimalLogData(msg, logType);
                if (GameLevel.Instance.gameplayLogs_optimal == null) { GameLevel.Instance.gameplayLogs_optimal = new List<LogDataOptimal>(); }
                GameLevel.Instance.gameplayLogs_optimal.Add(lData);
            }
            else if (size == LogDataSize.Verbose)
            {
                var lData = GetCurrentVerboseLogData(msg, logType);
                if (GameLevel.Instance.gameplayLogs_verbose == null) { GameLevel.Instance.gameplayLogs_verbose = new List<LogDataVerbose>(); }
                GameLevel.Instance.gameplayLogs_verbose.Add(lData);
            }

            static LogDataMinimal GetCurrentMinimalLogData(string msg, LogType logType)
            {
                var lData = new LogDataMinimal
                {
                    logType = logType,
                    message = msg
                };
                return lData;
            }

            static LogDataOptimal GetCurrentOptimalLogData(string msg, LogType logType)
            {
                var lData = new LogDataOptimal
                {
                    isServer = GameLevel.Instance.IsServer(),
                    logType = logType,
                    message = msg,
                    stackTrace = Environment.StackTrace
                };
                return lData;
            }

            static LogDataVerbose GetCurrentVerboseLogData(string msg, LogType logType)
            {
                var lData = new LogDataVerbose
                {
                    frameCount = Time.frameCount,
                    gameTime = Time.time,
                    isServer = GameLevel.Instance.IsServer(),
                    levelClassName = GameLevel.Instance.GetType().Name,
                    logType = logType,
                    message = msg,
                    stackTrace = Environment.StackTrace,
                    timeStamp = DateTime.Now.ToString(),
                    unityLevelName = SceneManager.GetActiveScene().name
                };
                return lData;
            }
        }

        public static void Print(string msg, bool useColor = false, Color color = default)
        {
#if KLOG_SUPPORT
            if (GameLevel.Instance == null) 
            {
                Debug.LogError("Usage of KLog outside GameLevel is prohibited!");
                return; 
            }
            string ltxt = msg;
            if (useColor)
            {
                ltxt = string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(color.r * 255f),
                    (byte)(color.g * 255f), (byte)(color.b * 255f), msg);
                Debug.Log(ltxt);
            }
            else
            {
                Debug.Log(ltxt);
            }
            AddLogToGameLevel(msg, LogType.Normal);
#endif
        }

        public static void PrintError(string msg)
        {
#if KLOG_SUPPORT
            if (GameLevel.Instance == null) 
            {
                Debug.LogError("Usage of KLog outside GameLevel is prohibited!");
                return; 
            }
            Debug.LogError(msg);
            AddLogToGameLevel(msg, LogType.Error);
#endif            
        }

        public static void PrintWarning(string msg)
        {
#if KLOG_SUPPORT
            if (GameLevel.Instance == null) 
            {
                Debug.LogError("Usage of KLog outside GameLevel is prohibited!");
                return; 
            }
            Debug.LogWarning(msg);
            AddLogToGameLevel(msg, LogType.Warning);
#endif            
        }

        public static void PrintException(Exception exception)
        {
#if KLOG_SUPPORT
            if (GameLevel.Instance == null) 
            {
                Debug.LogError("Usage of KLog outside GameLevel is prohibited!");
                return; 
            }
            Debug.LogException(exception);
            AddLogToGameLevel(exception.Message, LogType.ExceptionUnity);
#endif            
        }

        public static void PrintException(Exception exception, UnityEngine.Object objectContext)
        {
#if KLOG_SUPPORT
            if (GameLevel.Instance == null) 
            {
                Debug.LogError("Usage of KLog outside GameLevel is prohibited!");
                return; 
            }
            Debug.LogException(exception, objectContext);
            AddLogToGameLevel(exception.Message + " on unity object: " + objectContext.name, LogType.ExceptionUnityContexted);
#endif            
        }

        public static void Check(System.Action OnCheckCode)
        {
#if KLOG_SUPPORT
            if (GameLevel.Instance == null) 
            {
                Debug.LogError("Usage of KLog outside GameLevel is prohibited!");
                return; 
            }
            try
            {
                OnCheckCode?.Invoke();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                AddLogToGameLevel(ex.Message, LogType.ExceptionCSharp);
            }
#else
            OnCheckCode?.Invoke();
#endif
        }
    }
}