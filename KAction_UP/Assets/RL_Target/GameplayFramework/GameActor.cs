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
        protected virtual void OnStartOrSpawnActor() 
        { 
            life = initialLife; 
            isDead = false;
            beganDeath = false;
            onStartOrSpawn?.Invoke();
            OnStartOrSpawn?.Invoke();
        }

        protected virtual void AwakeActor() { }
        protected virtual IEnumerator StartActorAsync() { yield return null; }
        protected virtual IEnumerator OnBeginDeathAsync() { yield return null; }
        protected virtual void UpdateActor(float dt, float fixedDt) { }
        protected virtual void UpdateActorPhysics(float dt, float fixedDt) { }
        protected virtual void OnEditorUpdate() { ReloadComponents(); }
        protected virtual void OnCleanupActor() { }

        public Transform _Transform { get { return _transform; } }
        public GameObject _GameObject { get { return _gameObject; } }

        [SerializeField] List<GameplayComponent> gameplayComponents;
        [SerializeField] float life = 100f;
        [SerializeField] float timeScale = 1.0f;
        [SerializeField] UnityEvent onStartOrSpawn, onStartDeath, onDeath;
        [SerializeField] UnityEvent<float> onDamage, onGainHealth;
        public OnDoAnything OnStartOrSpawn, OnStartDeath, OnDeath;
        public UnityEvent<float> OnDamage, OnGainHealth;

        bool componentListDirty = false, isDead = false, beganDeath = false, gameplayRun = false;
        Transform _transform;
        GameObject _gameObject;
        float initialLife;
        GameManager gameMan_Core;
        GameActor owner = null;
        
        public float FullLife { get { return initialLife; } }
        public float CurrentLife { get { return life; } }
        public float NormalizedLifeValue { get { return life / initialLife; } }
        public bool IsDead { get { return isDead; } }
        public bool DeathHasBegan { get { return beganDeath; } }
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

        public void Damage(float damage)
        {
            if (isDead || beganDeath) { return; }
            var dm = Mathf.Abs(damage);
            this.life -= dm;
            onDamage?.Invoke(dm);
            OnDamage?.Invoke(dm);
            if (this.life <= 0f)
            {
                beganDeath = true;
                OnStartDeath?.Invoke();

                StartCoroutine(BeginDeathCOR());
            }
        }

        IEnumerator BeginDeathCOR()
        {
            yield return StartCoroutine(OnBeginDeathAsync());
            onDeath?.Invoke();
            _gameObject.SetActive(false);
        }

        public void Murder()
        {
            if (isDead || beganDeath) { return; }
            this.life = 0.0f;
            onDamage?.Invoke(life);
            OnDamage?.Invoke(life);
            beganDeath = true;
            OnStartDeath?.Invoke();

            StartCoroutine(BeginDeathCOR());
        }

        public void AddLife(float life)
        {
            if (isDead || beganDeath) { return; }
            var lf = Mathf.Abs(life);
            this.life += lf;
            onGainHealth?.Invoke(lf);
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

        void OnValidate()
        {
            OnEditorUpdate();
        }

        void Awake()
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

            gameMan_Core = FindObjectOfType<GameManager>();
            gameMan_Core.OnStartGameplay += StartGameplay;
            gameMan_Core.OnEndGameplay += EndGameplay;
        }

        void StartGameplay() { gameplayRun = true; }
        void EndGameplay() { gameplayRun = false; }

        void Start()
        {
            StartCoroutine(StartActorAsync());
        }

        void OnEnable()
        {
            OnStartOrSpawnActor();
            for (int i = 0; i < gameplayComponents.Count; i++)
            {
                gameplayComponents[i].OnStartOrSpawnActor();
            }
        }

        void OnDisable()
        {
            gameMan_Core.OnStartGameplay -= StartGameplay;
            gameMan_Core.OnEndGameplay -= EndGameplay;
            for (int i = 0; i < gameplayComponents.Count; i++)
            {
                gameplayComponents[i].OnCleanupComponent();
            }
            OnCleanupActor();
        }

        void Update()
        {
            if (!gameplayRun) { return; }

            var dt = Time.deltaTime * timeScale;
            var fixedDt = Time.fixedDeltaTime;
            UpdateActor(dt, fixedDt);

            if (componentListDirty) { return; }
            for (int i = 0; i < gameplayComponents.Count; i++)
            {
                gameplayComponents[i].UpdateComponent(dt, fixedDt);
            }
        }

        void FixedUpdate()
        {
            if (!gameplayRun) { return; }
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