using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Author: Md. Al Kaiyum(Rumman)
/// Email: kaiyumce06rumman@gmail.com
/// Placeable actor class that has some commonly used funtionalities
/// </summary>
namespace GameplayFramework
{
    public abstract class GameActor : MonoBehaviour
    {
        protected virtual void StartOrSpawnActor() 
        { 
            life = initialLife; 
            isDead = false;
            OnStartOrSpawn?.Invoke();
        }

        protected virtual void AwakeActor() { }
        protected virtual void UpdateActor(float dt, float fixedDt) { }
        protected virtual void UpdateActorPhysics(float dt, float fixedDt) { }
        protected virtual void OnEditorUpdate() { ReloadComponents(); }
        protected virtual void OnCleanupActor() { }

        public Transform _Transform { get { return _transform; } }
        public GameObject _GameObject { get { return _gameObject; } }

        [SerializeField] List<GameplayComponent> gameplayComponents;
        bool componentListDirty = false;
        Transform _transform;
        GameObject _gameObject;
        [SerializeField] float life = 100f;
        bool isDead = false;
        [SerializeField] float timeScale = 1.0f;
        float initialLife;
        GameManager gameMan_Core;

        public UnityEvent OnStartOrSpawn, OnDeath;
        public UnityEvent<float> OnDamage, OnGainHealth;
        public float FullLife { get { return initialLife; } }
        public float CurrentLife { get { return life; } }
        public float NormalizedLifeValue { get { return life / initialLife; } }
        public bool IsDead { get { return isDead; } }
        public float TimeScale { get { return timeScale; } set { timeScale = value; } }

        public T GetGameplayComponent<T>() where T : GameplayComponent
        {
            T result = null;
            if (gameplayComponents != null && gameplayComponents.Count > 0)
            {
                for (int i = 0; i < gameplayComponents.Count; i++)
                {
                    var gcom = gameplayComponents[i];
                    if (gcom.GetType() == typeof(T))
                    {
                        result = (T)gcom;
                        break;
                    }
                }
            }
            return result;
        }

        public List<T> GetGameplayComponents<T>() where T : GameplayComponent
        {
            List<T> result = new List<T>();
            if (gameplayComponents != null && gameplayComponents.Count > 0)
            {
                for (int i = 0; i < gameplayComponents.Count; i++)
                {
                    var gcom = gameplayComponents[i];
                    if (gcom.GetType() == typeof(T))
                    {
                        var com = (T)gcom;
                        if (result.Contains(com) == false)
                        {
                            result.Add(com);
                        }
                    }
                }
            }
            return result;
        }

        void ReloadComponents()
        {
            if (gameplayComponents == null) { gameplayComponents = new List<GameplayComponent>(); }
            GatherGameplayComponents(transform, ref gameplayComponents);
            gameplayComponents.RemoveAll((comp) => { return comp == null; });
            if (gameplayComponents == null) { gameplayComponents = new List<GameplayComponent>(); }

            void GatherGameplayComponents(Transform tr, ref List<GameplayComponent> compList)
            {
                var selfActor = tr.GetComponent<GameActor>();
                if (selfActor != null && selfActor != this)
                {
                    return;
                }

                var selfComps = tr.GetComponents<GameplayComponent>();
                if (selfComps != null && selfComps.Length > 0)
                {
                    for (int i = 0; i < selfComps.Length; i++)
                    {
                        var comp = selfComps[i];
                        if (comp == null) { continue; }
                        if (compList == null) { compList = new List<GameplayComponent>(); }
                        if (compList.Contains(comp) == false)
                        {
                            comp.SetOwner(this);
                            compList.Add(comp);
                        }
                    }
                }

                var count = tr.childCount;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        var chTr = tr.GetChild(i);
                        GatherGameplayComponents(chTr, ref compList);
                    }
                }
            }
        }

        public void DoDamage(float damage)
        {
            var dm = Mathf.Abs(damage);
            if (isDead) { return; }
            this.life -= dm;
            OnDamage?.Invoke(dm);
            if (this.life <= 0f)
            {
                isDead = true;
                OnDeath?.Invoke();
                _gameObject.SetActive(false);
            }
        }

        public void OneHitKill()
        {
            this.life = 0.0f;
            OnDamage?.Invoke(life);
            isDead = true;
            OnDeath?.Invoke();
            _gameObject.SetActive(false);
        }

        public void AddLife(float life)
        {
            var lf = Mathf.Abs(life);
            if (isDead) { return; }
            this.life += lf;
            OnGainHealth?.Invoke(lf);
        }

        public void AddGameplayComponent<T>() where T : GameplayComponent
        {
            componentListDirty = true;
            var _dyn_obj = new GameObject("_Gen_" + typeof(T) + "_CreatedAtT_" + Time.time + "_Level_" +
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            _dyn_obj.transform.SetParent(_transform);
            _dyn_obj.transform.localPosition = Vector3.zero;
            _dyn_obj.transform.localRotation = Quaternion.identity;
            _dyn_obj.transform.localScale = Vector3.one;
            this.AddGameplayComponent<T>(_dyn_obj);
            componentListDirty = false;
        }

        public void AddGameplayComponent<T>(GameObject objOnWhichToAdd) where T : GameplayComponent
        {
            componentListDirty = true;
            var comp = objOnWhichToAdd.AddComponent<T>();
            if (gameplayComponents.Contains(comp) == false)
            {
                comp.SetOwner(this);
                gameplayComponents.Add(comp);
            }
            componentListDirty = false;
        }

        private void OnValidate()
        {
            OnEditorUpdate();
        }

        private void Awake()
        {
            initialLife = life;
            _transform = transform;
            _gameObject = gameObject;
            ReloadComponents();
            AwakeActor();

            for (int i = 0; i < gameplayComponents.Count; i++)
            {
                gameplayComponents[i].AwakeComponent();
            }

            if (isDead)
            {
                OnDeath?.Invoke();
            }

            gameMan_Core = FindObjectOfType<GameManager>();
        }

        private void OnEnable()
        {
            StartOrSpawnActor();
            for (int i = 0; i < gameplayComponents.Count; i++)
            {
                gameplayComponents[i].OnStartOrSpawnActor();
            }
        }

        private void OnDisable()
        {
            OnCleanupActor();
            for (int i = 0; i < gameplayComponents.Count; i++)
            {
                gameplayComponents[i].OnCleanupComponent();
            }
        }

        void Update()
        {
            if (gameMan_Core.HasGameBeenStarted == false || gameMan_Core.HasGameBeenEnded) { return; }

            var dt = Time.deltaTime * timeScale;
            var fixedDt = Time.fixedDeltaTime;
            UpdateActor(dt, fixedDt);

            if (componentListDirty) { return; }
            for (int i = 0; i < gameplayComponents.Count; i++)
            {
                gameplayComponents[i].UpdateComponent(dt, fixedDt);
            }
        }

        private void FixedUpdate()
        {
            if (gameMan_Core.HasGameBeenStarted == false || gameMan_Core.HasGameBeenEnded) { return; }

            var dt = Time.deltaTime * timeScale;
            var fixedDt = Time.fixedDeltaTime;
            UpdateActorPhysics(dt, fixedDt);

            if (componentListDirty) { return; }
            for (int i = 0; i < gameplayComponents.Count; i++)
            {
                gameplayComponents[i].UpdateComponentPhysics(dt, fixedDt);
            }
        }
    }
}