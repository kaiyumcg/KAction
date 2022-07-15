using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public static partial class ActorUtil
    {
        static Actor _GetActorByTag(bool isRoot, GameplayTag tag, bool useRefOptimization = false)
        {
            Actor result = null;
            var handle = ActorLevelModule.instance;
            var actors = isRoot ? handle.rootActors : handle.actors;
            var isDirty = isRoot ? handle.rootActorListDirty : handle.actorListDirty;
            if (actors != null && isDirty == false)
            {
                var len = actors.Count;
                if (len > 0)
                {
                    for (int i = 0; i < len; i++)
                    {
                        var actor = actors[i];
                        if (actor.tags == null) { continue; }
                        if (useRefOptimization ? actor.tags.ContainsOPT(tag) : actor.tags.Contains(tag))
                        {
                            result = actor;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        static List<Actor> _GetActorsByTag(bool isRoot, GameplayTag tag, bool useRefOptimization = false)
        {
            List<Actor> results = new List<Actor>();
            var handle = ActorLevelModule.instance;
            var actors = isRoot ? handle.rootActors : handle.actors;
            var isDirty = isRoot ? handle.rootActorListDirty : handle.actorListDirty;
            if (actors != null && isDirty == false)
            {
                var len = actors.Count;
                if (len > 0)
                {
                    for (int i = 0; i < len; i++)
                    {
                        var actor = actors[i];
                        if (actor.tags == null) { continue; }
                        if (useRefOptimization ? actor.tags.ContainsOPT(tag) : actor.tags.Contains(tag))
                        {
                            results.Add(actor);
                        }
                    }
                }
            }
            return results;
        }

        static Actor _GetActorByTags(bool isRoot, bool useRefOptimization = false, params GameplayTag[] a_tags)
        {
            Actor result = null;
            var handle = ActorLevelModule.instance;
            var actors = isRoot ? handle.rootActors : handle.actors;
            var isDirty = isRoot ? handle.rootActorListDirty : handle.actorListDirty;
            if (actors != null && isDirty == false)
            {
                var len = actors.Count;
                if (len > 0)
                {
                    for (int i = 0; i < len; i++)
                    {
                        var actor = actors[i];
                        if (actor.tags == null) { continue; }
                        if (a_tags.FoundAllIn(actor.tags, useRefOptimization))
                        {
                            result = actor;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        static List<Actor> _GetActorsByTags(bool isRoot, bool useRefOptimization = false, params GameplayTag[] a_tags)
        {
            List<Actor> results = new List<Actor>();
            var handle = ActorLevelModule.instance;
            var actors = isRoot ? handle.rootActors : handle.actors;
            var isDirty = isRoot ? handle.rootActorListDirty : handle.actorListDirty;
            if (actors != null && isDirty == false)
            {
                var len = actors.Count;
                if (len > 0)
                {
                    for (int i = 0; i < len; i++)
                    {
                        var actor = actors[i];
                        if (actor.tags == null) { continue; }
                        if (a_tags.FoundAllIn(actor.tags, useRefOptimization))
                        {
                            results.Add(actor);
                        }
                    }
                }
            }
            return results;
        }
    }
}