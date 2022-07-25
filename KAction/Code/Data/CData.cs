using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public enum ErrorType { Exception = 0, Error = 1, CodeFailure = 2 }
    public enum ActionOnLog { DoNothing = 0, Stop = 1, Restart = 2 }

    [Serializable]
    public class PooledActor
    {
        [SerializeField] Actor prefab;
        [SerializeField] int preloadCount = 10;
        public Actor Prefab { get { return prefab; } }
        public int PreloadCount { get { return preloadCount; } }
    }

    public class ClonedActor
    {
        internal Actor sourcePrefab;
        internal Actor clonedActor;
        internal bool free;
    }

    [Serializable]
    public class ActorDataSet : FOrderedDictionary<string, ActorData> 
    {
        //todo methods for read/write device IO
        //todo so that the door was opened and now it will also be opened
    }

    [System.Serializable]
    public abstract class ActorData
    {

        public virtual string ConvertToJson() { return null; }
    }

    public enum ActorForceEase
    {
        EaseIn = 0,
        EaseOut = 1,
        Cubic = 2,
        Linear = 3
    }

    public enum ActorType
    {
        Player = 0,
        NPC = 1, 
        EnvironmentalStatic = 2, 
        EnvironmentalDynamic = 3, 
        Other = 4
    }

    public struct TimeReverseCapsule
    {

    }
}