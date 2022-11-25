using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public sealed partial class ActorLevelModule : LevelModule
    {
        #region BakeFields
        /// <summary>
        /// these are baked into by baking tools upon save operation of a level, during build process and upon play mode.
        /// in case of actor component or gameplay component or actor reparenting or cloning actors for first time or destroying them,
        /// these data might change. Audit and think!
        /// </summary>
        [SerializeField] List<Actor> rootActors;//actor add/remove || child-parent relation change
        [SerializeField] List<PooledActor> pooledItems;

        [SerializeField, HideInInspector] List<Actor> actors;//actor add/remove
        [NonSerialized, HideInInspector] List<ClonedActor> clonedData;//actor remove
        [SerializeField, HideInInspector] FOrderedDictionary<ReactorActor, Rigidbody> reactorBodies;//reactor actor add/remove
        [SerializeField, HideInInspector] FOrderedDictionary<ReactorActor, Rigidbody2D> reactorBodies2D;//reactor actor add/remove
        [SerializeField, HideInInspector] FOrderedDictionary<Collider, Actor> actorColliders;//reactor actor add/remove
        [SerializeField, HideInInspector] FOrderedDictionary<Collider, ReactorActor> reactorColliders;//reactor actor add/remove
        [SerializeField, HideInInspector] FOrderedDictionary<Collider2D, Actor> actorColliders2D;//reactor actor add/remove
        [SerializeField, HideInInspector] FOrderedDictionary<Collider2D, ReactorActor> reactorColliders2D;//reactor actor add/remove
        [SerializeField, HideInInspector] FOrderedDictionary<FPhysicsShape, Actor> actorShapes;//reactor actor add/remove
        [SerializeField, HideInInspector] FOrderedDictionary<FPhysicsShape, ReactorActor> reactorShapes;//reactor actor add/remove
        [SerializeField, HideInInspector] FOrderedDictionary<Collider, FPhysicsShape> shapes;//reactor actor add/remove
        [SerializeField, HideInInspector] FOrderedDictionary<Collider2D, FPhysicsShape> shapes2D;//reactor actor add/remove
        [SerializeField, HideInInspector] FOrderedDictionary<FPhysicsShape, Collider> shapes_RV;//reactor actor add/remove
        [SerializeField, HideInInspector] FOrderedDictionary<FPhysicsShape, Collider2D> shapes2D_RV;//reactor actor add/remove
        #endregion

        bool actorListDirty = false, rootActorListDirty = false;
        internal static float RawDelta, RawFixedDelta;

        internal static ActorLevelModule instance;
        internal FOrderedDictionary<ReactorActor, Rigidbody> ReactorBodies { get { return reactorBodies; } }
        internal FOrderedDictionary<ReactorActor, Rigidbody2D> ReactorBodies2D { get { return reactorBodies2D; } }
        internal FOrderedDictionary<Collider, Actor> ActorColliders { get { return actorColliders; } }
        internal FOrderedDictionary<Collider, ReactorActor> ReactorColliders { get { return reactorColliders; } }
        internal FOrderedDictionary<Collider2D, Actor> ActorColliders2D { get { return actorColliders2D; } }
        internal FOrderedDictionary<Collider2D, ReactorActor> ReactorColliders2D { get { return reactorColliders2D; } }
        internal FOrderedDictionary<FPhysicsShape, Actor> ActorShapes { get { return actorShapes; } }
        internal FOrderedDictionary<FPhysicsShape, ReactorActor> ReactorShapes { get { return reactorShapes; } }
        internal FOrderedDictionary<Collider, FPhysicsShape> Shapes { get { return shapes; } }
        internal FOrderedDictionary<Collider2D, FPhysicsShape> Shapes2D { get { return shapes2D; } }
        internal FOrderedDictionary<FPhysicsShape, Collider> Shapes_RV { get { return shapes_RV; } }
        internal FOrderedDictionary<FPhysicsShape, Collider2D> Shapes2D_RV { get { return shapes2D_RV; } }

        protected internal override void OnInit()
        {
            actorListDirty = rootActorListDirty = false;
            base.OnInit();
            instance = this;
            var tr = transform;
            if (pooledItems != null && pooledItems.Count > 0)
            {
                for (int i = 0; i < pooledItems.Count; i++)
                {
                    var pitem = pooledItems[i];
                    if (pitem == null) { continue; }
                    LoadActorPrefabForPool(pitem.Prefab, pitem.PreloadCount);
                }
            }

            for (int i = 0; i < rootActors.Count; i++)
            {
                rootActors[i].StartActorLifeCycle(firstTimePool: false, shouldMarkBusy: false);
            }

            void LoadActorPrefabForPool(Actor prefab, int count)
            {
                for (int i = 0; i < count; i++)
                {
                    var clonedGameobject = Instantiate(prefab.gameObject) as GameObject;
                    clonedGameobject.transform.SetParent(tr, true);

                    var clonedActor = clonedGameobject.GetComponent<Actor>();
                    clonedActor.StartActorLifeCycle(firstTimePool: true, shouldMarkBusy: false);
                    clonedActor.EndActorLifeCycle(firstTimePool: true, gameObjectDestroy: false);

                    var clonedActorData = new ClonedActor { clonedActor = clonedActor, free = false, sourcePrefab = prefab };
                    if (clonedData == null) { clonedData = new List<ClonedActor>(); }
                    clonedData.Add(clonedActorData);
                }
            }
        }

        protected internal override void OnTick(float delta)
        {
            if (rootActorListDirty) { return; }
            RawDelta = Time.deltaTime;
            RawFixedDelta = Time.fixedDeltaTime;
            for (int i = 0; i < rootActors.Count; i++)
            {
                rootActors[i].Tick(RawDelta, RawFixedDelta);
            }
        }

        protected internal override void OnPhysicsTick(float delta, float physxDelta)
        {
            if (rootActorListDirty) { return; }
            for (int i = 0; i < rootActors.Count; i++)
            {
                rootActors[i].TickPhysics(RawDelta, RawFixedDelta);
            }
        }

        //todo when busy, the actor's root is null, otherwise child of this module for better scene organization
        internal void MarkBusy<T>(T actorInScene) where T : Actor
        {
            //set in pool as NOT-free
        }

        internal void MarkFree<T>(T actorInScene) where T : Actor
        {
            //set in pool as free
        }

        public static void RemoveFreeActors()
        {
            instance.actorListDirty = true;
            instance.rootActorListDirty = true;
            var lst = instance.clonedData;
            var toRemove = new List<ClonedActor>();
            if (lst != null && lst.Count > 0)
            {
                for(int i = 0;i < lst.Count ;i++)
                {
                    var item = lst[i];
                    if (item == null || item.free == false) { continue; }
                    toRemove.Add(item);
                }
            }

            for (int i = 0; i < toRemove.Count; i++)
            {
                var item = toRemove[i];
                if (lst != null)
                {
                    lst.Remove(item);
                }
                instance.OnDestroyActorInstance(item.clonedActor);//is it necessary to do it for each of them?
                Destroy(item.clonedActor.gameObject);
            }

            //remove all free entry from "instance.clonedData"

            //destroy those actor gameobjects
            instance.actorListDirty = false;
            instance.rootActorListDirty = false;
        }

        internal void OnDestroyActorInstance<T>(T actor) where T : Actor
        {
            
        }

        
    }
}