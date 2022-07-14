using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public static partial class ActorUtil
    {
        public static T GetActorClassed<T>() where T : Actor
        {
            T result = null;
            var handle = ActorLevelModule.instance;
            var actors = handle.actors;
            var isDirty = handle.actorListDirty;
            if(actors != null && isDirty == false)
            {
                var len = actors.Count;
                if (len > 0)
                {
                    for (int i = 0; i < len; i++)
                    {
                        var actor = actors[i];
                        if (actor.GetType() == typeof(T))
                        {
                            result = (T)actor;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        public static List<T> GetActorsClassed<T>() where T : Actor
        {
            List<T> results = new List<T>();
            var handle = ActorLevelModule.instance;
            var actors = handle.actors;
            var isDirty = handle.actorListDirty;
            if (actors != null && isDirty == false)
            {
                var len = actors.Count;
                if (len > 0)
                {
                    for (int i = 0; i < len; i++)
                    {
                        var actor = actors[i];
                        if (actor.GetType() == typeof(T))
                        {
                            results.Add((T)actor);
                        }
                    }
                }
            }
            return results;
        }

        public static T GetActorByType<T>(ActorType aType) where T : Actor
        {
            T result = null;
            var handle = ActorLevelModule.instance;
            var actors = handle.actors;
            var isDirty = handle.actorListDirty;
            if (actors != null && isDirty == false)
            {
                var len = actors.Count;
                if (len > 0)
                {
                    for (int i = 0; i < len; i++)
                    {
                        var actor = actors[i];
                        if (actor.GetType() == typeof(T) && actor.ActorType == aType)
                        {
                            result = (T)actor;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        public static List<T> GetActorsByType<T>(ActorType aType) where T : Actor
        {
            List<T> results = new List<T>();
            var handle = ActorLevelModule.instance;
            var actors = handle.actors;
            var isDirty = handle.actorListDirty;
            if (actors != null && isDirty == false)
            {
                var len = actors.Count;
                if (len > 0)
                {
                    for (int i = 0; i < len; i++)
                    {
                        var actor = actors[i];
                        if (actor.GetType() == typeof(T) && actor.ActorType == aType)
                        {
                            results.Add((T)actor);
                        }
                    }
                }
            }
            return results;
        }

        public static Actor GetActor(ActorType aType)
        {
            Actor result = null;
            var handle = ActorLevelModule.instance;
            var actors = handle.actors;
            var isDirty = handle.actorListDirty;
            if (actors != null && isDirty == false)
            {
                var len = actors.Count;
                if (len > 0)
                {
                    for (int i = 0; i < len; i++)
                    {
                        var actor = actors[i];
                        if (actor.ActorType == aType)
                        {
                            result = actor;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        public static List<Actor> GetActors(ActorType aType)
        {
            List<Actor> results = new List<Actor>();
            var handle = ActorLevelModule.instance;
            var actors = handle.actors;
            var isDirty = handle.actorListDirty;
            if (actors != null && isDirty == false)
            {
                var len = actors.Count;
                if (len > 0)
                {
                    for (int i = 0; i < len; i++)
                    {
                        var actor = actors[i];
                        if (actor.ActorType == aType)
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