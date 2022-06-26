using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public enum GFType
    {
        FirstContact = 0,
        GameService = 1,
        GameLevel = 2,
        GameModule = 3,
        ActorModule = 4,
        Actor = 5,
        AI = 6,
        Controller = 7,
        PlayerController = 8,
        AIController = 9,
        Reaction = 10,
        NetCode = 11,
        Cinematic = 12,
        Video = 13,
        UI = 14
    }

    public class GameplayFrameworkException : System.Exception
    {
        public GameplayFrameworkException(GFType errorType, string customMessage)
        {
#if KLOG_SUPPORT
            KLog.AddLogToGameLevel("Gameplay Framework Error. Type: " + errorType + " and custom message: " + customMessage, LogType.FrameworkException);
#endif        
        }

        public override string HelpLink { get => "git repo link"; set => base.HelpLink = value; }
    }
}