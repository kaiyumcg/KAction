using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public sealed class ActorLevelModule : LevelModule
    {
        /// <summary>
        /// these are baked into by baking tools upon save operation of a level, during build process and upon play mode.
        /// in case of actor component or gameplay component or actor reparenting or cloning actors for first time or destroying them,
        /// these data might change. Audit and think!
        /// </summary>
        [SerializeField] internal List<Actor> rootActors;
        [SerializeField, HideInInspector] internal List<Actor> actors;
        [SerializeField, HideInInspector] FOrderedDictionary<ReactorActor, Rigidbody> reactorBodies;
        [SerializeField, HideInInspector] FOrderedDictionary<ReactorActor, Rigidbody2D> reactorBodies2D;
        [SerializeField, HideInInspector] FOrderedDictionary<Collider, Actor> actorColliders;
        [SerializeField, HideInInspector] FOrderedDictionary<Collider, ReactorActor> reactorColliders;
        [SerializeField, HideInInspector] FOrderedDictionary<Collider2D, Actor> actorColliders2D;
        [SerializeField, HideInInspector] FOrderedDictionary<Collider2D, ReactorActor> reactorColliders2D;
        [SerializeField, HideInInspector] FOrderedDictionary<FPhysicsShape, Actor> actorShapes;
        [SerializeField, HideInInspector] FOrderedDictionary<FPhysicsShape, ReactorActor> reactorShapes;
        [SerializeField, HideInInspector] FOrderedDictionary<Collider, FPhysicsShape> shapes;
        [SerializeField, HideInInspector] FOrderedDictionary<Collider2D, FPhysicsShape> shapes2D;
        [SerializeField, HideInInspector] FOrderedDictionary<FPhysicsShape, Collider> shapes_RV;
        [SerializeField, HideInInspector] FOrderedDictionary<FPhysicsShape, Collider2D> shapes2D_RV;

#if UNITY_EDITOR
        public void SetEd_rootActors(List<Actor> rootActors) { this.rootActors = rootActors; }
        public void SetEd_actors(List<Actor> actors) { this.actors = actors; }
        public void SetEd_reactorBodies(FOrderedDictionary<ReactorActor, Rigidbody> reactorBodies) { this.reactorBodies = reactorBodies; }
        public void SetEd_reactorBodies2D(FOrderedDictionary<ReactorActor, Rigidbody2D> reactorBodies2D) { this.reactorBodies2D = reactorBodies2D; }
        public void SetEd_actorColliders(FOrderedDictionary<Collider, Actor> actorColliders) { this.actorColliders = actorColliders; }
        public void SetEd_reactorColliders(FOrderedDictionary<Collider, ReactorActor> reactorColliders) { this.reactorColliders = reactorColliders; }
        public void SetEd_actorColliders2D(FOrderedDictionary<Collider2D, Actor> actorColliders2D) { this.actorColliders2D = actorColliders2D; }
        public void SetEd_reactorColliders2D(FOrderedDictionary<Collider2D, ReactorActor> reactorColliders2D) { this.reactorColliders2D = reactorColliders2D; }
        public void SetEd_actorShapes(FOrderedDictionary<FPhysicsShape, Actor> actorShapes) { this.actorShapes = actorShapes; }
        public void SetEd_reactorShapes(FOrderedDictionary<FPhysicsShape, ReactorActor> reactorShapes) { this.reactorShapes = reactorShapes; }
        public void SetEd_shapes(FOrderedDictionary<Collider, FPhysicsShape> shapes) { this.shapes = shapes; }
        public void SetEd_shapes2D(FOrderedDictionary<Collider2D, FPhysicsShape> shapes2D) { this.shapes2D = shapes2D; }
        public void SetEd_shapes_RV(FOrderedDictionary<FPhysicsShape, Collider> shapes_RV) { this.shapes_RV = shapes_RV; }
        public void SetEd_shapes2D_RV(FOrderedDictionary<FPhysicsShape, Collider2D> shapes2D_RV) { this.shapes2D_RV = shapes2D_RV; }
#endif

        internal bool actorListDirty = false, rootActorListDirty = false;

        internal static float RawDelta, RawFixedDelta;

        internal static ActorLevelModule instance;
        public static ActorLevelModule Instance { get { return instance; } }
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
            base.OnInit();
            instance = this;
            if (rootActorListDirty)
            {
                KLog.ThrowGameplaySDKException(GFType.ActorModule, 
                    "Root actor list can never be dirty at Init() of Actor Level Module! BUG!!");
            }
            for (int i = 0; i < rootActors.Count; i++)
            {
                rootActors[i].AwakeActor();
            }
        }

        protected internal override void OnTick()
        {
            if (rootActorListDirty) { return; }
            RawDelta = Time.deltaTime;
            RawFixedDelta = Time.fixedDeltaTime;
            for (int i = 0; i < rootActors.Count; i++)
            {
                rootActors[i].Tick(RawDelta, RawFixedDelta);
            }
        }

        protected internal override void OnPhysxTick()
        {
            if (rootActorListDirty) { return; }
            for (int i = 0; i < rootActors.Count; i++)
            {
                rootActors[i].TickPhysics(RawDelta, RawFixedDelta);
            }
        }

        public T GetOrCloneActor<T>(T prefab) where T : Actor
        {
            return null;
        }

        public void FreeActor<T>(T clonedActor) where T : Actor
        {
            
        }
    }
}