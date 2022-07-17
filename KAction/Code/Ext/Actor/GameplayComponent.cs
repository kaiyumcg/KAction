using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public static partial class ActorExt
    {
        static T _GetGameplayComponent<T>(Actor actor) where T : GameplayComponent
        {
            T result = null;
            var gameplayComponents = actor.gameplayComponents;
            var componentListDirty = actor.componentListDirty;
            if (gameplayComponents != null && componentListDirty == false)
            {
                var len = gameplayComponents.Count;
                if (len > 0)
                {
                    for (int i = 0; i < len; i++)
                    {
                        var comp = gameplayComponents[i];
                        if (comp.GetType() == typeof(T))
                        {
                            result = (T)comp;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        static List<T> _GetGameplayComponents<T>(Actor actor) where T : GameplayComponent
        {
            List<T> results = new List<T>();
            var gameplayComponents = actor.gameplayComponents;
            var componentListDirty = actor.componentListDirty;
            if (gameplayComponents != null && componentListDirty == false)
            {
                var len = gameplayComponents.Count;
                if (len > 0)
                {
                    for (int i = 0; i < len; i++)
                    {
                        var comp = gameplayComponents[i];
                        if (comp.GetType() == typeof(T))
                        {
                            results.Add((T)comp);
                        }
                    }
                }
            }
            return results;
        }

        static T _GetGameplayComponentByTag<T>(Actor actor, GameplayTag tag, bool useRefOptimization = false) where T : GameplayComponent
        {
            T result = null;
            var gameplayComponents = actor.gameplayComponents;
            var componentListDirty = actor.componentListDirty;
            if (gameplayComponents != null && componentListDirty == false)
            {
                var len = gameplayComponents.Count;
                if (len > 0)
                {
                    for (int i = 0; i < len; i++)
                    {
                        var comp = gameplayComponents[i];
                        if (comp.componentTag == null) { continue; }
                        if (comp.GetType() == typeof(T) && (useRefOptimization ? ReferenceEquals(comp.componentTag, tag) : comp.componentTag == tag))
                        {
                            result = (T)comp;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        static List<T> _GetGameplayComponentsByTag<T>(Actor actor, GameplayTag tag, bool useRefOptimization = false) where T : GameplayComponent
        {
            List<T> results = new List<T>();
            var gameplayComponents = actor.gameplayComponents;
            var componentListDirty = actor.componentListDirty;
            if (gameplayComponents != null && componentListDirty == false)
            {
                var len = gameplayComponents.Count;
                if (len > 0)
                {
                    for (int i = 0; i < len; i++)
                    {
                        var comp = gameplayComponents[i];
                        if (comp.componentTag == null) { continue; }
                        if (comp.GetType() == typeof(T) && (useRefOptimization ? ReferenceEquals(comp.componentTag, tag) : comp.componentTag == tag))
                        {
                            results.Add((T)comp);
                        }
                    }
                }
            }
            return results;
        }

        static T _AddActorComponent<T>(Actor actor) where T : GameplayComponent
        {
            actor.componentListDirty = true;
            var ownerObject = actor._Transform;
            var holderObject = new GameObject("_GEN_" + typeof(T));
            var holderTr = holderObject.transform;
            holderTr.SetParent(ownerObject);
            holderTr.localPosition = Vector3.zero;
            holderTr.localEulerAngles = Vector3.zero;
            holderTr.localScale = Vector3.one;
            T t = holderObject.AddComponent<T>();
            t.isDynamic = true;
            actor.gameplayComponents.Add(t);
            actor.componentListDirty = false;
            return t;
        }

        static void _RemoveActorComponent<T>(Actor actor, T t) where T : GameplayComponent
        {
            if (t.isDynamic == false)
            {
                KLog.PrintError("Only Actor Component can be removed!");
                return;
            }
            actor.componentListDirty = true;
            if (actor.gameplayComponents.Contains(t)) { actor.gameplayComponents.Remove(t); }
            Actor.Destroy(t.gameObject);
            actor.componentListDirty = false;
        }
    }
}