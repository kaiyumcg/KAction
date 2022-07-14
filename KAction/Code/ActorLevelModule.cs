using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public sealed class ActorLevelModule : LevelModule
    {
        [SerializeField] internal List<Actor> actors, rootActors;
        internal bool actorListDirty = false;

        internal static float RawDelta, RawFixedDelta;

        internal static ActorLevelModule instance;
        public static ActorLevelModule Instance { get { return instance; } }

        Dictionary<ReactorActor, Rigidbody> reactorBodies;
        internal Dictionary<ReactorActor, Rigidbody> ReactorBodies { get { return reactorBodies; } }

        Dictionary<ReactorActor, Rigidbody2D> reactorBodies2D;
        internal Dictionary<ReactorActor, Rigidbody2D> ReactorBodies2D { get { return reactorBodies2D; } }

        Dictionary<Collider, Actor> actorColliders;
        internal Dictionary<Collider, Actor> ActorColliders { get { return actorColliders; } }
        Dictionary<Collider, ReactorActor> reactorColliders;
        internal Dictionary<Collider, ReactorActor> ReactorColliders { get { return reactorColliders; } }
        Dictionary<Collider2D, Actor> actorColliders2D;
        internal Dictionary<Collider2D, Actor> ActorColliders2D { get { return actorColliders2D; } }
        Dictionary<Collider2D, ReactorActor> reactorColliders2D;
        internal Dictionary<Collider2D, ReactorActor> ReactorColliders2D { get { return reactorColliders2D; } }

        Dictionary<FPhysicsShape, Actor> actorShapes;
        internal Dictionary<FPhysicsShape, Actor> ActorShapes { get { return actorShapes; } }
        Dictionary<FPhysicsShape, ReactorActor> reactorShapes;
        internal Dictionary<FPhysicsShape, ReactorActor> ReactorShapes { get { return reactorShapes; } }

        Dictionary<Collider, FPhysicsShape> shapes;
        internal Dictionary<Collider, FPhysicsShape> Shapes { get { return shapes; } }
        Dictionary<Collider2D, FPhysicsShape> shapes2D;
        internal Dictionary<Collider2D, FPhysicsShape> Shapes2D { get { return shapes2D; } }

        Dictionary<FPhysicsShape, Collider> shapes_RV;
        internal Dictionary<FPhysicsShape, Collider> Shapes_RV { get { return shapes_RV; } }
        Dictionary<FPhysicsShape, Collider2D> shapes2D_RV;
        internal Dictionary<FPhysicsShape, Collider2D> Shapes2D_RV { get { return shapes2D_RV; } }

        protected internal override void OnInit()
        {
            base.OnInit();
            instance = this;
        }

        protected internal override void OnTick()
        {
            if (actorListDirty) { return; }
            RawDelta = Time.deltaTime;
            RawFixedDelta = Time.fixedDeltaTime;
            for (int i = 0; i < rootActors.Count; i++)
            {
                rootActors[i].Tick(RawDelta, RawFixedDelta);
            }
        }

        protected internal override void OnPhysxTick()
        {
            if (actorListDirty) { return; }
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