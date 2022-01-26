using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameplayFramework
{
    internal static class CloudLogUploader
    {
        internal static void UploadLog(
            List<LogDataMinimal> minLogs,
            List<LogDataOptimal> optimalLogs,
            List<LogDataVerbose> verboseLogs, 
            LogDataSize size, string API_EndPoint)
        {
            var targetString = "";
            if (size == LogDataSize.Minimal) { targetString = GetLogString(minLogs); }
            else if (size == LogDataSize.Optimal) { targetString = GetLogString(optimalLogs); }
            else if (size == LogDataSize.Verbose) { targetString = GetLogString(verboseLogs); }
            CloudLogUploadManager.StartUploadLogs(targetString, API_EndPoint);
        }

        static string GetLogString(List<LogDataMinimal> logs)
        {
            string result = "";
            if (logs != null && logs.Count > 0)
            {
                for (int i = 0; i < logs.Count; i++)
                {
                    var log = logs[i];
                    result += "Type-->" + log.logType + " Message-->" + log.message + Environment.NewLine;
                }
            }
            return result;
        }

        static string GetLogString(List<LogDataOptimal> logs)
        {
            string result = "";
            if (logs != null && logs.Count > 0)
            {
                for (int i = 0; i < logs.Count; i++)
                {
                    var log = logs[i];
                    result += "Type-->" + log.logType + " Message-->" + log.message
                        + " Is Server-->" + log.isServer + " Stacktrace-->" + log.stackTrace
                        + Environment.NewLine;
                }
            }
            return result;
        }

        static string GetLogString(List<LogDataVerbose> logs)
        {
            string result = "";
            if (logs != null && logs.Count > 0)
            {
                for (int i = 0; i < logs.Count; i++)
                {
                    var log = logs[i];
                    result += "Type-->" + log.logType + " Message-->" + log.message
                        + " Is Server-->" + log.isServer + " Stacktrace-->" + log.stackTrace
                        + " Level Class Name-->" + log.levelClassName + " Unity Level Name-->" + log.unityLevelName
                        + " FrameCount-->" + log.frameCount + " GameTime-->" + log.gameTime
                        + " TimeStamp-->" + log.timeStamp
                        + Environment.NewLine;
                }
            }
            return result;
        }
    }
}