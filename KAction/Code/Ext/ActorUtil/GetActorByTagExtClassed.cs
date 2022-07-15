using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public static partial class ActorUtil
    {
        static T _GetActorByTagClassed<T>(bool isRoot, GameplayTag tag, bool useRefOptimization = false) where T : Actor
        {
            T result = null;
            var handle = ActorLevelModule.instance;
            var actors = isRoot ? handle.rootActors : handle.actors;
            var isDirty = handle.actorListDirty;
            if (actors != null && isDirty == false)
            {
                var len = actors.Count;
                if (len > 0)
                {
                    for (int i = 0; i < len; i++)
                    {
                        var actor = actors[i];
                        if (actor.tags == null) { continue; }
                        if (actor.GetType() == typeof(T) && (useRefOptimization ? actor.tags.ContainsOPT(tag) : actor.tags.Contains(tag)))
                        {
                            result = (T)actor;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        static List<T> _GetActorsByTagClassed<T>(bool isRoot, GameplayTag tag, bool useRefOptimization = false) where T : Actor
        {
            List<T> results = new List<T>();
            var handle = ActorLevelModule.instance;
            var actors = isRoot ? handle.rootActors : handle.actors;
            var isDirty = handle.actorListDirty;
            if (actors != null && isDirty == false)
            {
                var len = actors.Count;
                if (len > 0)
                {
                    for (int i = 0; i < len; i++)
                    {
                        var actor = actors[i];
                        if (actor.tags == null) { continue; }
                        if (actor.GetType() == typeof(T) && (useRefOptimization ? actor.tags.ContainsOPT(tag) : actor.tags.Contains(tag)))
                        {
                            results.Add((T)actor);
                        }
                    }
                }
            }
            return results;
        }

        static T _GetActorByTagsClassed<T>(bool isRoot, bool useRefOptimization = false, params GameplayTag[] a_tags) where T : Actor
        {
            T result = null;
            var handle = ActorLevelModule.instance;
            var actors = isRoot ? handle.rootActors : handle.actors;
            var isDirty = handle.actorListDirty;
            if (actors != null && isDirty == false)
            {
                var len = actors.Count;
                if (len > 0)
                {
                    for (int i = 0; i < len; i++)
                    {
                        var actor = actors[i];
                        if (actor.tags == null) { continue; }
                        if (actor.GetType() == typeof(T) && a_tags.FoundAllIn(actor.tags, useRefOptimization))
                        {
                            result = (T)actor;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        static List<T> _GetActorsByTagsClassed<T>(bool isRoot, bool useRefOptimization = false, params GameplayTag[] a_tags) where T : Actor
        {
            List<T> results = new List<T>();
            var handle = ActorLevelModule.instance;
            var actors = isRoot ? handle.rootActors : handle.actors;
            var isDirty = handle.actorListDirty;
            if (actors != null && isDirty == false)
            {
                var len = actors.Count;
                if (len > 0)
                {
                    for (int i = 0; i < len; i++)
                    {
                        var actor = actors[i];
                        if (actor.tags == null) { continue; }
                        if (actor.GetType() == typeof(T) && a_tags.FoundAllIn(actor.tags, useRefOptimization))
                        {
                            results.Add((T)actor);
                        }
                    }
                }
            }
            return results;
        }
    }
}