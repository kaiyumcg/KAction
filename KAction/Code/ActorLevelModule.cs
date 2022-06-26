using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public sealed class ActorLevelModule : LevelModule
    {
        [SerializeField] List<Actor> rootActors;

        bool actorListDirty = false;

        static ActorLevelModule instance;
        public static ActorLevelModule Instance { get { return instance; } }

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

        internal void ReloadRootActors()
        {
            
        }

        void Awake()
        {
            ReloadRootActors();
        }

        protected internal override void OnTick()
        {
            if (actorListDirty) { return; }
            for (int i = 0; i < rootActors.Count; i++)
            {
                var dt = Time.deltaTime;
                var fDt = Time.fixedDeltaTime;
                rootActors[i].Tick(dt, fDt);
            }
        }

        protected internal override void OnPhysxTick()
        {
            if (actorListDirty) { return; }
            for (int i = 0; i < rootActors.Count; i++)
            {
                var dt = Time.deltaTime;
                var fDt = Time.fixedDeltaTime;
                rootActors[i].TickPhysics(dt, fDt);
            }
        }

        public void PauseActors()
        {
            
        }

        public void ResumeActors()
        {
            
        }

        public void SetCustomTime(float timeScale)
        {
            
        }

        public T GetActor<T>() where T : Actor
        {
            return null;
        }

        public List<T> GetActors<T>() where T : Actor
        {
            return null;
        }

        public T GetPlayerActor<T>() where T : Actor
        {
            return null;
        }

        public List<T> GetPlayerActors<T>() where T : Actor
        {
            return null;
        }

        public T GetActorByTag<T>(GameplayTag tag) where T : Actor
        {
            return null;
        }

        public List<T> GetActorsByTag<T>(GameplayTag tag) where T : Actor
        {
            return null;
        }

        public T GetActorByTag<T>(string a_tag, bool inFileName = false) where T : Actor
        {
            return null;
        }

        public List<T> GetActorsByTag<T>(string a_tag, bool inFileName = false) where T : Actor
        {
            return null;
        }

        public T GetActorByTag<T>(List<GameplayTag> a_tags) where T : Actor
        {
            return null;
        }

        public List<T> GetActorsByTag<T>(List<GameplayTag> a_tags) where T : Actor
        {
            return null;
        }

        public T GetActorByTag<T>(List<string> a_tags, bool inFileName = false) where T : Actor
        {
            return null;
        }

        public List<T> GetActorsByTag<T>(List<string> a_tags, bool inFileName = false) where T : Actor
        {
            return null;
        }

        public List<T> GetActorsFor<T>(System.Predicate<Actor> condition) where T : Actor
        {
            return null;
        }

        public T GetActorFor<T>(System.Predicate<Actor> condition) where T : Actor
        {
            return null;
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