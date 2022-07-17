using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    internal enum PlayableState
    {
        UnPlayed = 0,
        Playing = 1,
        Paused = 2,
        Stopped = 3
    }
    internal delegate void TimelineCallbackEventHandler();
    internal delegate void TimelineCallbackEventHandler<in T>(T value);
}