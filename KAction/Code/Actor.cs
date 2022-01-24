using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameplayFramework
{
    public abstract class Actor : MonoBehaviour
    {
        Rigidbody rgd;
        Rigidbody2D rgd2D;
        Transform tr;
        GameObject gobj;
        bool gameplayRun = false, isDead = false, deathStarted = false,
            isRoot = true, childActorListDirty = false, isActorPaused = false;
        private protected bool componentListDirty = false;
        float initialLife = 0.0f, initialTimeScale = 1.0f;
        GameManager gameMan_Core;
        Actor owner = null;

        //Record if actor rigidbody is kinematic or not
        //Record collider tree enable state
        //In reverse time, per actor 
        //Per Transform Tree per transform set-->parent transform data, position-rotation-local scale data
        //Per collider-->disable collider
        //Set-->rigidbody position-velocity(in kinematic mode)
        //Get current TimeRevCapsule struct/class data-->call reverse()-->delete current-->Set current for next frame
        //At the end of process-->reset colliders enable state-->reset rigidbody kinematic mode

        [SerializeField] bool isPlayer = false;
        [SerializeField] List<GameplayTag> tags = null;
        [SerializeField] private protected List<GameplayComponent> gameplayComponents;
        [SerializeField] internal List<Actor> childActors;
        [SerializeField] float life = 100f;
        [SerializeField] ParticleSpawnDesc deathParticle = null, damageParticle = null,
           gainHealthParticle = null, rebornParticle = null;
        [SerializeField] bool canRecieveDamage = false, canGainHealth = false, healthOverflow = false;
        [SerializeField] UnityEvent onStartOrSpawn = null, onStartDeath = null, onDeath = null, onReborn = null,
            onPause = null, onResume = null;
        [SerializeField] UnityEvent<float> onDamage = null, onGainHealth = null;
        [SerializeField] UnityEvent<float, Vector3> onDirectionalDamage = null, onDirectionalGainHealth = null;
        [SerializeField] float timeScale = 1.0f;
        [SerializeField] bool pauseResumeAffectsChildActors = false, customTimeDilationAffectsChildActors = false;

        protected virtual IEnumerator OnStartAsync() { yield return null; }
        protected virtual IEnumerator OnStartDeathAsync() { yield return null; }
        protected virtual void OnStartOnce() { }
        protected virtual void OnStart() { }
        protected virtual void OnCleanup() { }
        protected virtual void OnDamage(float damageAmount) { }
        protected virtual void OnDirectionalDamage(float damageAmount, Vector3 damageDir) { }
        protected virtual void OnGainHealth(float addedHealth) { }
        protected virtual void OnDirectionalGainHealth(float addedHealth, Vector3 damageDir) { }
        protected virtual void OnStartDeath() { }
        protected virtual void OnDeath() { }
        protected virtual void OnReborn() { }
        protected virtual void OnPause() { }
        protected virtual void OnResume() { }
#if UNITY_EDITOR
        protected virtual void OnEditorUpdate() { }
#endif
        protected virtual void UpdateActor(float dt, float fixedDt) { }
        protected virtual void UpdateActorPhysics(float dt, float fixedDt) { }
        public Rigidbody PhysicsBody { get { return rgd; } }
        public Rigidbody2D PhysicsBody2D { get { return rgd2D; } }
        public Transform _Transform { get { return tr; } }
        public GameObject _GameObject { get { return gobj; } }
        public bool IsDead { get { return isDead; } }
        public bool HasDeathBeenStarted { get { return deathStarted; } }
        public float FullLife { get { return initialLife; } }
        public float NormalizedLife { get { return life / initialLife; } }
        public float CurrentLife { get { return life; } }
        public float TimeScale 
        { 
            get 
            { 
                return timeScale; 
            } 
            set 
            { 
                timeScale = value;
                if (customTimeDilationAffectsChildActors)
                {
                    if (childActorListDirty == false)
                    {
                        if(childActors != null && childActors.Count > 0)
                        {
                            for (int i = 0; i < childActors.Count; i++)
                            {
                                var ch = childActors[i];
                                if (ch == null) { continue; }
                                ch.TimeScale = value;
                            }
                        }
                    }
                }
            } 
        }
        public event OnDoAnything OnStartOrSpawnEv, OnDeathEv, OnStartDeathEv, OnRebornEv, OnPauseEv, OnResumeEv;
        public event OnDoAnything<float> OnDamageEv, OnGainHealthEv;
        public event OnDoAnything<float, Vector3> OnDirectionalDamageEv, OnDirectionalGainHealthEv;
        public bool IsPlayer { get { return isPlayer; } }
        public bool ShouldGameplayRun { get { return gameplayRun; } }
        public bool PauseResumeAffectChildActors { get { return pauseResumeAffectsChildActors; } set { pauseResumeAffectsChildActors = value; } }
        public bool CustomTimeDilationAffectChildActors { get { return customTimeDilationAffectsChildActors; } set { customTimeDilationAffectsChildActors = value; } }

        #region GameplayComponents Add/Remove/Reference
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

        internal void ReloadComponents()
        {
            if (gameplayComponents == null) { gameplayComponents = new List<GameplayComponent>(); }
            GatherGameplayComponents(transform, ref gameplayComponents);
            gameplayComponents.RemoveAll((comp) => { return comp == null; });
            if (gameplayComponents == null) { gameplayComponents = new List<GameplayComponent>(); }
            var lst = new List<GameplayComponent>();
            if (gameplayComponents.Count > 0)
            {
                for (int i = 0; i < gameplayComponents.Count; i++)
                {
                    var comp = gameplayComponents[i];
                    if (lst.Contains(comp) == false) { lst.Add(comp); }
                }
            }
            gameplayComponents = lst;

            void GatherGameplayComponents(Transform tr, ref List<GameplayComponent> compList)
            {
                var selfActor = tr.GetComponent<Actor>();
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

        public void AddGameplayComponent<T>() where T : GameplayComponent
        {
            componentListDirty = true;
            var _dyn_obj = new GameObject("_Gen_" + typeof(T) + "_CreatedAtT_" + Time.time + "_Level_" +
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            _dyn_obj.transform.SetParent(tr);
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
        #endregion

        #region ChildActors Add/Remove/Reference
        internal void ReloadChildActors()
        {
            
        }

        #endregion

#if UNITY_EDITOR
        void OnValidate()
        {
            OnEditorUpdate();
        }
#endif

        internal void AwakeActor()
        {
            initialTimeScale = timeScale;
            initialLife = life;
            tr = transform;
            gobj = gameObject;
            owner = GetComponentInParent<Actor>();
            isRoot = owner == null;
            ReloadComponents();
            ReloadChildActors();

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
            gameMan_Core = FindObjectOfType<GameManager>();
            gameMan_Core.OnStartGameplay += StartGameplay;
            gameMan_Core.OnEndGameplay += EndGameplay;
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

        #region ActorCore

        protected virtual void OnStartOrSpawnActor()
        {
            isDead = deathStarted = isActorPaused = false;
            life = initialLife;
            timeScale = initialTimeScale;
            onStartOrSpawn?.Invoke();
            OnStartOrSpawnEv?.Invoke();
        }

        public void Reborn()
        {
            if (!isDead || !gameplayRun || deathStarted) { return; }
            StopAllCoroutines();
            gameObject.SetActive(true);
            Born();
            onReborn?.Invoke();
            OnRebornEv?.Invoke();
            OnReborn();
            rebornParticle.Spawn(tr);
        }

        void Born()
        {
            isDead = deathStarted = isActorPaused = false;
            life = initialLife;
            timeScale = initialTimeScale;
            OnStart();
            StartCoroutine(OnStartAsync());
        }

        public bool IsSimilarTo(Actor actor)
        {
            bool found = false;
            if (tags.Count > 0)
            {
                for (int i = 0; i < tags.Count; i++)
                {
                    var t = tags[i];
                    if (actor.tags.Contains(t))
                    {
                        found = true;
                        break;
                    }
                }
            }
            return found;
        }

        public void AddUniqueTag(GameplayTag tag)
        {
            if (tags == null) { tags = new List<GameplayTag>(); }
            if (tag == null) { return; }
            if (tags.Contains(tag) == false) { tags.Add(tag); }
        }

        public void Murder()
        {
            if (isDead || deathStarted || !canRecieveDamage || !gameplayRun) { return; }
            DeathProc();
        }

        void DeathProc()
        {
            deathStarted = true;
            life = 0.0f;
            StartCoroutine(DeathProcAsync());
            IEnumerator DeathProcAsync()
            {
                deathParticle.Spawn(tr);
                onStartDeath?.Invoke();
                OnStartDeathEv?.Invoke();
                OnStartDeath();
                yield return StartCoroutine(OnStartDeathAsync());
                onDeath?.Invoke();
                OnDeathEv?.Invoke();
                OnDeath();
                isDead = true;
                gobj.SetActive(false);
            }
        }

        public void Damage(float damage)
        {
            if (isDead || deathStarted || !canRecieveDamage || !gameplayRun) { return; }
            var dm = Mathf.Abs(damage);
            life -= dm;
            onDamage?.Invoke(dm);
            OnDamageEv?.Invoke(dm);
            OnDamage(dm);
            damageParticle.Spawn(tr);
            if (life <= 0.0f)
            {
                DeathProc();
            }
        }

        public void DamageFromDirection(float damage, Vector3 direction)
        {
            if (isDead || deathStarted || !canRecieveDamage || !gameplayRun) { return; }
            var dm = Mathf.Abs(damage);
            life -= dm;

            onDirectionalDamage?.Invoke(dm, direction);
            OnDirectionalDamageEv?.Invoke(dm, direction);
            OnDirectionalDamage(dm, direction);
            damageParticle.Spawn(tr);
            if (life <= 0.0f)
            {
                DeathProc();
            }


        }

        public void AddHealth(float health)
        {
            if (isDead || deathStarted || !canGainHealth || !gameplayRun) { return; }
            var hl = Mathf.Abs(health);
            life += hl;
            if (life > initialLife && !healthOverflow) { life = initialLife; }
            onGainHealth?.Invoke(health);
            OnGainHealthEv?.Invoke(health);
            OnGainHealth(hl);
            gainHealthParticle.Spawn(tr);
        }

        public void AddDirectionalHealth(float health, Vector3 direction)
        {
            if (isDead || deathStarted || !canGainHealth || !gameplayRun) { return; }
            var hl = Mathf.Abs(health);
            life += hl;
            if (life > initialLife && !healthOverflow) { life = initialLife; }
            onDirectionalGainHealth?.Invoke(health, direction);
            OnDirectionalGainHealthEv?.Invoke(health, direction);
            OnDirectionalGainHealth(hl, direction);
            gainHealthParticle.Spawn(tr);
        }

        public void Pause()
        {
            if (isActorPaused || !gameplayRun || isDead || deathStarted) { return; }
            timeScale = 0.0f;
            isActorPaused = true;
            onPause?.Invoke();
            OnPauseEv?.Invoke();
            OnPause();

            if (pauseResumeAffectsChildActors)
            {
                if (childActorListDirty == false)
                {
                    for (int i = 0; i < childActors.Count; i++)
                    {
                        childActors[i].Pause();
                    }
                }
            }
        }

        public void Resume()
        {
            if (isActorPaused == false || !gameplayRun || isDead || deathStarted) { return; }
            timeScale = 1.0f;
            isActorPaused = false;
            onResume?.Invoke();
            OnResumeEv?.Invoke();
            OnResume();

            if (pauseResumeAffectsChildActors)
            {
                if (childActorListDirty == false)
                {
                    for (int i = 0; i < childActors.Count; i++)
                    {
                        childActors[i].Resume();
                    }
                }
            }
        }

        #endregion

        void OnDisable()
        {
            gameMan_Core.OnStartGameplay -= StartGameplay;
            gameMan_Core.OnEndGameplay -= EndGameplay;
            for (int i = 0; i < gameplayComponents.Count; i++)
            {
                gameplayComponents[i].OnCleanupComponent();
            }
            StopAllCoroutines();
            OnCleanup();
        }

        #region TickFunc
        //for actor manager
        internal void Tick(float dt_from_unity, float fixedDt_from_unity)
        {
            if (!gameplayRun || isDead || deathStarted || !isRoot) { return; }
            dt_from_unity *= timeScale;
            fixedDt_from_unity *= timeScale;
            TickActorInternal(dt_from_unity, fixedDt_from_unity);
        }

        private protected void TickActorInternal(float dt_from_unity, float fixedDt_from_unity)
        {
            UpdateActor(dt_from_unity, fixedDt_from_unity);
            if (childActorListDirty == false)
            {
                for (int i = 0; i < childActors.Count; i++)
                {
                    childActors[i].TickActorInternal(dt_from_unity, fixedDt_from_unity);
                }
            }

            if (componentListDirty) { return; }
            for (int i = 0; i < gameplayComponents.Count; i++)
            {
                gameplayComponents[i].UpdateComponent(dt_from_unity, fixedDt_from_unity);
            }
        }

        #endregion

        #region PhysicsTickFunc
        internal void TickPhysics(float dt_from_unity, float fixedDt_from_unity)
        {
            if (!gameplayRun || isDead || deathStarted || !isRoot) { return; }
            dt_from_unity *= timeScale;
            fixedDt_from_unity *= timeScale;
            TickPhysicsInternal(dt_from_unity, fixedDt_from_unity);
        }

        private protected void TickPhysicsInternal(float dt, float fixedDt)
        {
            UpdateActorPhysics(dt, fixedDt);

            if (childActorListDirty == false)
            {
                for (int i = 0; i < childActors.Count; i++)
                {
                    childActors[i].TickPhysicsInternal(dt, fixedDt);//so child dead hoileo ki child run korbe? eta fix korte hobe!
                }
            }

            if (componentListDirty) { return; }
            for (int i = 0; i < gameplayComponents.Count; i++)
            {
                gameplayComponents[i].UpdateComponentPhysics(dt, fixedDt);
            }
        }
        #endregion

        public Coroutine Wait(float amountInScaledTime, System.Action OnComplete)
        {
            return StartCoroutine(Waiter(amountInScaledTime, OnComplete));

            IEnumerator Waiter(float amount_wt, System.Action OnComplete)
            {
                yield return new WaitForSeconds(amount_wt);
                OnComplete?.Invoke();
            }
        }

        #region Shake
        bool shaking = false;

        IEnumerator shakeGameObjectCOR(GameObject objectToShake, float totalShakeDuration, float decreasePoint, bool objectIs2D = false)
        {
            if (decreasePoint >= totalShakeDuration)
            {
                yield break; //Exit!
            }

            //Get Original Pos and rot
            Transform objTransform = objectToShake.transform;
            Vector3 defaultPos = objTransform.position;
            Quaternion defaultRot = objTransform.rotation;

            float counter = 0f;

            //Shake Speed
            const float speed = 0.1f;

            //Angle Rotation(Optional)
            const float angleRot = 4;

            //Do the actual shaking
            while (counter < totalShakeDuration)
            {
                counter += Time.deltaTime;
                float decreaseSpeed = speed;
                float decreaseAngle = angleRot;

                //Shake GameObject
                if (objectIs2D)
                {
                    //Don't Translate the Z Axis if 2D Object
                    Vector3 tempPos = defaultPos + UnityEngine.Random.insideUnitSphere * decreaseSpeed;
                    tempPos.z = defaultPos.z;
                    objTransform.position = tempPos;

                    //Only Rotate the Z axis if 2D
                    objTransform.rotation = defaultRot * Quaternion.AngleAxis(UnityEngine.Random.Range(-angleRot, angleRot), new Vector3(0f, 0f, 1f));
                }
                else
                {
                    objTransform.position = defaultPos + UnityEngine.Random.insideUnitSphere * decreaseSpeed;
                    objTransform.rotation = defaultRot * Quaternion.AngleAxis(UnityEngine.Random.Range(-angleRot, angleRot), new Vector3(1f, 1f, 1f));
                }
                yield return null;


                //Check if we have reached the decreasePoint then start decreasing  decreaseSpeed value
                if (counter >= decreasePoint)
                {
                    //Reset counter to 0 
                    counter = 0f;
                    while (counter <= decreasePoint)
                    {
                        counter += Time.deltaTime;
                        decreaseSpeed = Mathf.Lerp(speed, 0, counter / decreasePoint);
                        decreaseAngle = Mathf.Lerp(angleRot, 0, counter / decreasePoint);
                        //Shake GameObject
                        if (objectIs2D)
                        {
                            //Don't Translate the Z Axis if 2D Object
                            Vector3 tempPos = defaultPos + UnityEngine.Random.insideUnitSphere * decreaseSpeed;
                            tempPos.z = defaultPos.z;
                            objTransform.position = tempPos;

                            //Only Rotate the Z axis if 2D
                            objTransform.rotation = defaultRot * Quaternion.AngleAxis(UnityEngine.Random.Range(-decreaseAngle, decreaseAngle), new Vector3(0f, 0f, 1f));
                        }
                        else
                        {
                            objTransform.position = defaultPos + UnityEngine.Random.insideUnitSphere * decreaseSpeed;
                            objTransform.rotation = defaultRot * Quaternion.AngleAxis(UnityEngine.Random.Range(-decreaseAngle, decreaseAngle), new Vector3(1f, 1f, 1f));
                        }
                        yield return null;
                    }

                    //Break from the outer loop
                    break;
                }
            }
            objTransform.position = defaultPos; //Reset to original postion
            objTransform.rotation = defaultRot;//Reset to original rotation

            shaking = false; //So that we can call this function next time
        }

        //rigidbody counterpart?
        public void Shake(float shakeDuration, float decreasePoint)
        {
            if (shaking)
            {
                return;
            }
            shaking = true;
            StartCoroutine(shakeGameObjectCOR(gameObject, shakeDuration, decreasePoint, rgd2D != null));
        }
        #endregion
    }
}