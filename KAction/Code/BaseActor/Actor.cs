using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameplayFramework
{
    public abstract partial class Actor : MonoBehaviour
    {
        [SerializeField] ActorType mType = ActorType.Player;
        public ActorType ActorType { get { return mType; } }
        [System.NonSerialized, HideInInspector] internal bool canTick = true;
        public bool CanTick { get { return canTick; } set { canTick = value; } }
        Transform tr;
        GameObject gobj;
        bool gameplayRun = false, isRoot = true, childActorListDirty = false, isActorPaused = false;
        
        GameLevel gameMan_Core;
        Actor owner = null;
        
        
        [SerializeField] internal List<Actor> childActors;

        [SerializeField] UnityEvent onStartOrSpawn = null;

        protected virtual IEnumerator OnStartAsync() { yield break; }
        
        protected virtual void OnStartOnce() { }
        protected virtual void OnStart() { }
        protected virtual void OnCleanup() { }
#if UNITY_EDITOR
        protected virtual void OnEditorUpdate() { }//called from editor inspector custom code. todo
#endif
        
        public Transform _Transform { get { return tr; } }
        public GameObject _GameObject { get { return gobj; } }
        public event OnDoAnything OnStartOrSpawnEv;
        public bool ShouldGameplayRun { get { return gameplayRun; } }

        internal void AwakeActor()
        {
            initialTimeScale = timeScale;
            initialLife = life;
            tr = transform;
            gobj = gameObject;
            owner = GetComponentInParent<Actor>();
            isRoot = owner == null;

            if (tags != null && tags.Count > 0)
            {
                tags.RemoveAll((t) => { return t == null; });
            }
            if (tags == null) { tags = new List<GameplayTag>(); }

            Born();
            OnStartOnce();
            for (int i = 0; i < gameplayComponents.Count; i++)
            {
                gameplayComponents[i].StartComponent();
            }
            gameMan_Core = FindObjectOfType<GameLevel>();
            gameMan_Core.OnLevelGameplayStartEv += StartGameplay;
            gameMan_Core.onLevelGameplayEndEv += EndGameplay;
        }

        void StartGameplay() { gameplayRun = true; }
        void EndGameplay() { gameplayRun = false; }

        void OnEnable()
        {
            OnStartOrSpawnActor();
            for (int i = 0; i < gameplayComponents.Count; i++)
            {
                gameplayComponents[i].OnStartOrSpawnActor();
            }
        }

        protected virtual void OnStartOrSpawnActor()
        {
            isDead = deathStarted = isActorPaused = false;
            life = initialLife;
            timeScale = initialTimeScale;
            onStartOrSpawn?.Invoke();
            OnStartOrSpawnEv?.Invoke();
        }

        void OnDisable()
        {
            gameMan_Core.OnLevelGameplayStartEv -= StartGameplay;
            gameMan_Core.onLevelGameplayEndEv -= EndGameplay;
            for (int i = 0; i < gameplayComponents.Count; i++)
            {
                gameplayComponents[i].OnCleanupComponent();
            }
            StopAllCoroutines();
            OnCleanup();
        }
    }
}