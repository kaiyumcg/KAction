using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public sealed partial class ActorLevelModule : LevelModule
    {
        static T _GetOrCloneActor<T>(T prefab, bool overrideTransform, bool overrideLocal, bool useEularForRotation,
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
                            freeItemAvailable = true;
                            freeActorObject = c;
                            freeActor = (T)c.clonedActor;
                            break;
                        }
                    }
                }
            }

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
            freeActorObject.clonedActor._gameobject.SetActive(true);
            var tr = freeActorObject.clonedActor._transform;
            tr.SetParent(null, true);
            if (overrideTransform)
            {
                if (overrideLocal) { tr.localPosition = overridePosition; }
                else { tr.position = overridePosition; }

                if (overrideLocal)
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
            freeActorObject.clonedActor.StopAllCoroutines();
            freeActorObject.clonedActor.StartActor();

            //pooled item list ta onusare reload korte hobe--ei time e Actor::PoolCreationTimeFree(Actor actor) ei method ta call korte hobe
            //jekhane jekhane actor list and related dictionary-map data ase, segulo patch kora
            return result;
        }

        static void _FreeActor<T>(T clonedActor) where T : Actor
        {
            //a call to this automatically includes it into pool
            //on death finally, it is called
            //if death is called with obliterate flag then it is not included into pool
            //rather it will finally unity destroy the actor

            var tr = clonedActor._transform;
            tr.SetParent(instance.transform, true)//transform ta cache kora keno na? LevelModule e etao bake data te include
                //then actor e baked data list?
                //dynamic weapon add/remove e 'ActorLevelModule.OnDestroyActorCompletely(Actor actor)' type jinis korte hobe
                //dynamic component add e ki korte hobe same jinis?

                
        }
    }
}