using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public sealed partial class ActorLevelModule : LevelModule
    {
        public static T Spawn<T>(T prefab) where T : Actor
        {
            return _GetOrCloneActor<T>(prefab, modifyTransform: false, false, false,
                Vector3.zero, Vector3.zero, Quaternion.identity, Vector3.zero);
        }

        public static T SpawnAtTransform<T>(T prefab, Vector3 position, Quaternion rotation, Vector3 localScale, bool local = false)
            where T : Actor
        {
            return _GetOrCloneActor<T>(prefab, modifyTransform: true, local, useEularForRotation: false, 
                position, Vector3.zero, rotation, localScale);
        }

        public static T SpawnAtTransform<T>(T prefab, Vector3 position, Vector3 rotation, Vector3 localScale, bool local = false)
            where T : Actor
        {
            return _GetOrCloneActor<T>(prefab, modifyTransform: true, local, useEularForRotation: false, 
                position, rotation, Quaternion.identity, localScale);
        }

        public static T SpawnByType<T>() where T : Actor
        {
            var pr = _GetOne<T>();
            return pr == null ? null : _GetOrCloneActor<T>(pr, modifyTransform: false, false, false,
                Vector3.zero, Vector3.zero, Quaternion.identity, Vector3.zero);
        }

        public static T SpawnByTypeAtTransform<T>(Vector3 position, Quaternion rotation, Vector3 localScale, bool local = false)
            where T : Actor
        {
            var pr = _GetOne<T>();
            return pr == null ? null : _GetOrCloneActor<T>(pr, modifyTransform: true, local, useEularForRotation: false, 
                position, Vector3.zero, rotation, localScale);
        }

        public static T SpawnByTypeAtTransform<T>(Vector3 position, Vector3 rotation, Vector3 localScale, bool local = false)
            where T : Actor
        {
            var pr = _GetOne<T>();
            return pr == null ? null : _GetOrCloneActor<T>(pr, modifyTransform: true, local, useEularForRotation: false, 
                position, rotation, Quaternion.identity, localScale);
        }

        static T _GetOne<T>() where T : Actor
        {
            T prefab = null;
            var clonedList = instance.clonedData;
            if (clonedList != null)
            {
                var len = clonedList.Count;
                if (len > 0)
                {
                    for (int i = 0; i < len; i++)
                    {
                        var c = clonedList[i];
                        if (c == null) { continue; }
                        if (c.sourcePrefab.GetType() == typeof(T))
                        {
                            prefab = (T)c.sourcePrefab;
                            break;
                        }
                    }
                }
            }
#if GF_DEBUG
            if (prefab == null)
            {
                KLog.ThrowGameplaySDKException(GFType.ActorModule,
                    "Could not get a actor by type from the pool list. " +
                    "It is most likely you have not added any such typed prefab into the pool list of actor level module. " +
                    "Since debug mode is turned on, you are getting this exception.");
            }
#endif
            return prefab;
        }

        static T _GetOrCloneActor<T>(T prefab, bool modifyTransform, bool localSpace, bool useEularForRotation,
            Vector3 overridePosition, Vector3 overrideRotationEular, 
            Quaternion overrideRotation, Vector3 overrideScale) where T : Actor
        {
            var freeItemAvailable = false;
            T freeActor = null;
            ClonedActor freeActorObject = null;
            var clonedList = instance.clonedData;
            if (clonedList != null)
            {
                var len = clonedList.Count;
                if (len > 0)
                {
                    for (int i = 0; i < len; i++)
                    {
                        var c = clonedList[i];
                        if (c == null || c.free == false) { continue; }
                        if (c.sourcePrefab == prefab)
                        {
                            prefab = (T)c.sourcePrefab;
                            freeItemAvailable = true;
                            freeActorObject = c;
                            freeActor = (T)c.clonedActor;
                            break;
                        }
                    }
                }
            }

#if GF_DEBUG
            if (instance.actors.Contains(prefab))
            {
                KLog.ThrowGameplaySDKException(GFType.ActorModule,
                    "You are trying to clone or get from pool list an actor that already exists in the scene! " +
                    "This is not supported by design. You should always clone or get from pool actors that are prefab instead. " +
                    "Since debug mode is turned on, you are getting this exception.");
            }
#endif

            if (!freeItemAvailable)
            {
                var cloned = Instantiate(prefab.gameObject) as GameObject;
                freeActor = cloned.GetComponent<T>();
                freeActorObject = new ClonedActor { clonedActor = freeActor, free = false, sourcePrefab = prefab };
                if (clonedList == null) { clonedList = new List<ClonedActor>(); }
                clonedList.Add(freeActorObject);
            }

            T result = freeActor;
            freeActorObject.free = false;
            freeActorObject.clonedActor._GameObject.SetActive(true);
            var tr = freeActorObject.clonedActor._Transform;
            tr.SetParent(null, true);
            if (modifyTransform)
            {
                if (localSpace) { tr.localPosition = overridePosition; }
                else { tr.position = overridePosition; }

                if (localSpace)
                {
                    if (useEularForRotation)
                    {
                        tr.localEulerAngles = overrideRotationEular;
                    }
                    else
                    {
                        tr.localRotation = overrideRotation;
                    }
                }
                else
                {
                    if (useEularForRotation)
                    {
                        tr.eulerAngles = overrideRotationEular;
                    }
                    else
                    {
                        tr.rotation = overrideRotation;
                    }
                }
                tr.localScale = overrideScale;
            }
            freeActorObject.clonedActor.StartActorLifeCycle(firstTimePool : false);
            return result;
        }
    }
}