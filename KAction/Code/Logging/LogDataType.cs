using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameplayFramework
{
    public enum LogType { Normal, Warning, Error, ExceptionUnity, ExceptionUnityContexted, ExceptionCSharp }
    public enum LogDataSize { Verbose, Optimal, Minimal }

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