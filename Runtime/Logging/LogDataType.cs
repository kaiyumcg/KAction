using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameplayFramework
{
    public enum LogType 
    { 
        Normal = 0, 
        Warning = 1, 
        Error = 2, 
        ExceptionUnity = 3, 
        ExceptionUnityContexted = 4, 
        ExceptionCSharp = 5,
        FrameworkException = 6
    }

    public enum LogDataSize 
    { 
        Verbose = 0, 
        Optimal = 1, 
        Minimal = 2
    }

    public struct LogDataOptimal
    {
        public LogType logType;
        public string message;
        public string stackTrace;
        public bool isServer;
    }

    public struct LogDataMinimal
    {
        public LogType logType;
        public string message;
    }

    public struct LogDataVerbose
    {
        public LogType logType;
        public string message;
        public string timeStamp;
        public string unityLevelName;
        public string levelClassName;
        public int frameCount;
        public float gameTime;
        public string stackTrace;
        public bool isServer;
    }
}