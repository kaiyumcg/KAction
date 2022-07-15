using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public static partial class ActorUtil
    {
        static T _GetActorClassed<T>(bool isRoot) where T : Actor
        {
            T result = null;
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

        static List<T> _GetActorsClassed<T>(bool isRoot) where T : Actor
        {
            List<T> results = new List<T>();
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
                        if (actor.GetType() == typeof(T))
                        {
                            results.Add((T)actor);
                        }
                    }
                }
            }
            return results;
        }

        static T _GetActorByType<T>(bool isRoot, ActorType aType) where T : Actor
        {
            T result = null;
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

        static List<T> _GetActorsByType<T>(bool isRoot, ActorType aType) where T : Actor
        {
            List<T> results = new List<T>();
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
                        if (actor.GetType() == typeof(T) && actor.ActorType == aType)
                        {
                            results.Add((T)actor);
                        }
                    }
                }
            }
            return results;
        }

        static Actor _GetActor(bool isRoot, ActorType aType)
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

        static List<Actor> _GetActors(bool isRoot, ActorType aType)
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