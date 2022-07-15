using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    /// <summary>
    /// A bunch of methods upon Actors and Root Actors in a game level.
    /// </summary>
    public static partial class ActorUtil
    {
        #region Pause-Resume
        public static void PauseRootActors() { _PauseAllActors(true); }
        public static void ResumeRootActors() { _ResumeAllActors(true); }
        public static void SetCustomTimeForRootActors(float timeScale) { _SetCustomTimeForAllActors(true, timeScale); }
        public static void PauseAllActors() { _PauseAllActors(false); }
        public static void ResumeAllActors() { _ResumeAllActors(false); }
        public static void SetCustomTimeForAllActors(float timeScale) { _SetCustomTimeForAllActors(false, timeScale); }
        #endregion

        #region GetActor
        public static T GetRootActorClassed<T>() where T : Actor { return _GetActorClassed<T>(true); }
        public static List<T> GetRootActorsClassed<T>() where T : Actor { return _GetActorsClassed<T>(true); }
        public static T GetRootActorByType<T>(ActorType aType) where T : Actor { return _GetActorByType<T>(true, aType); }
        public static List<T> GetRootActorsByType<T>(ActorType aType) where T : Actor { return _GetActorsByType<T>(true, aType); }
        public static Actor GetRootActor(ActorType aType) { return _GetActor(true, aType); }
        public static List<Actor> GetRootActors(ActorType aType) { return _GetActors(true, aType); }

        public static T GetActorClassed<T>() where T : Actor { return _GetActorClassed<T>(false); }
        public static List<T> GetActorsClassed<T>() where T : Actor { return _GetActorsClassed<T>(false); }
        public static T GetActorByType<T>(ActorType aType) where T : Actor { return _GetActorByType<T>(false, aType); }
        public static List<T> GetActorsByType<T>(ActorType aType) where T : Actor { return _GetActorsByType<T>(false, aType); }
        public static Actor GetActor(ActorType aType) { return _GetActor(false, aType); }
        public static List<Actor> GetActors(ActorType aType) { return _GetActors(false, aType); }
        #endregion

        #region GetActorClassedByTag
        public static T GetRootActorByTagClassed<T>(GameplayTag tag, bool useRefOptimization = false) where T : Actor 
        { 
            return _GetActorByTagClassed<T>(true, tag, useRefOptimization); 
        }

        public static List<T> GetRootActorsByTagClassed<T>(GameplayTag tag, bool useRefOptimization = false) where T : Actor 
        { 
            return _GetActorsByTagClassed<T>(true, tag, useRefOptimization); 
        }

        public static T GetRootActorByTagsClassed<T>(bool useRefOptimization = false, params GameplayTag[] a_tags) where T : Actor 
        { 
            return _GetActorByTagsClassed<T>(true, useRefOptimization, a_tags); 
        }

        public static List<T> GetRootActorsByTagsClassed<T>(bool useRefOptimization = false, params GameplayTag[] a_tags) where T : Actor 
        { 
            return _GetActorsByTagsClassed<T>(true, useRefOptimization, a_tags); 
        }

        public static T GetActorByTagClassed<T>(GameplayTag tag, bool useRefOptimization = false) where T : Actor 
        { 
            return _GetActorByTagClassed<T>(false, tag, useRefOptimization); 
        }

        public static List<T> GetActorsByTagClassed<T>(GameplayTag tag, bool useRefOptimization = false) where T : Actor 
        { 
            return _GetActorsByTagClassed<T>(false, tag, useRefOptimization); 
        }

        public static T GetActorByTagsClassed<T>(bool useRefOptimization = false, params GameplayTag[] a_tags) where T : Actor 
        { 
            return _GetActorByTagsClassed<T>(false, useRefOptimization, a_tags); 
        }

        public static List<T> GetActorsByTagsClassed<T>(bool useRefOptimization = false, params GameplayTag[] a_tags) where T : Actor 
        { 
            return _GetActorsByTagsClassed<T>(false, useRefOptimization, a_tags); 
        }
        #endregion

        #region GetActorByTag
        public static Actor GetRootActorByTag(GameplayTag tag, bool useRefOptimization = false)
        {
            return _GetActorByTag(true, tag, useRefOptimization);
        }

        public static List<Actor> GetRootActorsByTag(GameplayTag tag, bool useRefOptimization = false)
        {
            return _GetActorsByTag(true, tag, useRefOptimization);
        }

        public static Actor GetRootActorByTags(bool useRefOptimization = false, params GameplayTag[] a_tags)
        {
            return _GetActorByTags(true, useRefOptimization, a_tags);
        }

        public static List<Actor> GetRootActorsByTags(bool useRefOptimization = false, params GameplayTag[] a_tags)
        {
            return _GetActorsByTags(true, useRefOptimization, a_tags);
        }

        public static Actor GetActorByTag(GameplayTag tag, bool useRefOptimization = false)
        {
            return _GetActorByTag(false, tag, useRefOptimization);
        }

        public static List<Actor> GetActorsByTag(GameplayTag tag, bool useRefOptimization = false)
        {
            return _GetActorsByTag(false, tag, useRefOptimization);
        }

        public static Actor GetActorByTags(bool useRefOptimization = false, params GameplayTag[] a_tags)
        {
            return _GetActorByTags(false, useRefOptimization, a_tags);
        }

        public static List<Actor> GetActorsByTags(bool useRefOptimization = false, params GameplayTag[] a_tags)
        {
            return _GetActorsByTags(false, useRefOptimization, a_tags);
        }
        #endregion
    }
}