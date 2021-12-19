using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public class ActorManager : GameSystem
    {
        [SerializeField] List<Actor> rootActors;

        bool actorListDirty = false;

        internal void ReloadRootActors()
        {
            
        }

        void Awake()
        {
            ReloadRootActors();
        }

        void Update()
        {
            if (actorListDirty) { return; }
            for (int i = 0; i < rootActors.Count; i++)
            {
                var dt = Time.deltaTime;
                var fDt = Time.fixedDeltaTime;
                rootActors[i].Tick(dt, fDt);
            }
        }

        void FixedUpdate()
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