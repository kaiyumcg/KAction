using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public abstract partial class Actor : MonoBehaviour
    {
        public T GetGameplayComponent<T>() where T : GameplayComponent
        {
            T result = null;
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

        public List<T> GetGameplayComponents<T>() where T : GameplayComponent
        {
            List<T> results = new List<T>();
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

        public T GetGameplayComponentByTag<T>(GameplayTag tag, bool useRefOptimization = false) where T : GameplayComponent
        {
            T result = null;
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

        public List<T> GetGameplayComponentsByTag<T>(GameplayTag tag, bool useRefOptimization = false) where T : GameplayComponent
        {
            List<T> results = new List<T>();
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

        public T AddActorComponent<T>() where T : GameplayComponent
        {
            componentListDirty = true;
            var ownerObject = _transform;
            var holderObject = new GameObject("_GEN_" + typeof(T));
            var holderTr = holderObject.transform;
            holderTr.SetParent(ownerObject);
            holderTr.localPosition = Vector3.zero;
            holderTr.localEulerAngles = Vector3.zero;
            holderTr.localScale = Vector3.one;
            T t = holderObject.AddComponent<T>();
            t.isDynamic = true;
            t.dynamicFlag = new NullChecker();
            gameplayComponents.Add(t);
            componentListDirty = false;
            return t;
        }

        public void RemoveActorComponent<T>(T t) where T : GameplayComponent
        {
#if GF_DEBUG
            if (t.isDynamic == false && t.dynamicFlag == null)
            {
                KLog.PrintError("Only Actor Component can be removed!");
                return;
            }
#endif
            componentListDirty = true;
            if (gameplayComponents.Contains(t)) { gameplayComponents.Remove(t); }
            Actor.Destroy(t.gameObject);
            componentListDirty = false;
        }
    }
}