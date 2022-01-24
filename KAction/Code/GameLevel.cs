using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace GameplayFramework
{
    public abstract class GameLevel : MonoBehaviour
    {
        protected virtual void OnLevelStart() { }
        protected virtual void OnLevelGameplayStart() { }
        protected virtual void OnTick() { }
        protected virtual void OnPhysxTick() { }
        protected virtual bool WhenLevelGameplayAutoEnd() { return false; }

        protected virtual IEnumerator OnStartScriptCutScene() { yield return null; }
        protected virtual IEnumerator OnEndScriptCutScene() { yield return null; }

        [SerializeField] bool useStartCutScene = false, useEndCutScene = false;
        [SerializeField] PlayableAsset startCutScene = null, endCutScene = null;
        [SerializeField] List<LevelModule> levelModules = new List<LevelModule>();
        [SerializeField] bool debugMessage = false;
        internal bool DebugMessage { get { return debugMessage; } }

        static GameLevel instance = null;
        public static GameLevel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<GameLevel>();
                }
                return instance;
            }
        }
        
        bool lvGameplayStarted = false, lvGameplayEnded = false, isPlayingCutScene = false, isPaused = false;
        PlayableDirector director;

        public bool HasLevelGameplayBeenStarted { get { return lvGameplayStarted; } }
        public bool HasLevelGameplayBeenEnded { get { return lvGameplayEnded; } }
        public bool IsPlayingCutScene { get { return isPlayingCutScene; } }
        public bool IsPaused { get { return isPaused; } }

        [SerializeField] UnityEvent onLevelStart, onLevelGameplayStart, onLevelGameplayEnd;
        public event OnDoAnything OnLevelStartEv, OnLevelGameplayStartEv, onLevelGameplayEndEv;

        //Time slow, reverse etc should be handled by here
        //Load next level(), LoadFakeLevel(), LoadRealLevel(), level streaming etc should also be handled by here
        //Finding actors by player or not, tags, type etc handling
        //Level progression, checkpoints, level config data(coins, exp, health etc, door unlock state) update,
        //put it simple things are related to a whole level handling
        //
        //other todo which are related to a whole level
        //todo for that we need actor manager which handles execution of all actors
        //we should have a default inspector editor UI to create default game systems whenever user adds a game manager or a button to do that!

        void ReloadSysData()
        {
            director = GetComponent<PlayableDirector>();
            if (useStartCutScene || useEndCutScene)
            {
                if (director == null)
                {
                    director = gameObject.AddComponent<PlayableDirector>();
                }
                director.playableAsset = null;
                director.playOnAwake = false;
            }
            if (levelModules == null) { levelModules = new List<LevelModule>(); }
            var allsys = FindObjectsOfType<LevelModule>();
            if (allsys != null && allsys.Length > 0)
            {
                for (int i = 0; i < allsys.Length; i++)
                {
                    var sys = allsys[i];
                    if (sys == null) { continue; }
                    if (levelModules.Contains(sys) == false)
                    {
                        levelModules.Add(sys);
                    }
                }
            }

            levelModules.RemoveAll((sys) => { return sys == null; });
            if (levelModules == null) { levelModules = new List<LevelModule>(); }
        }

        private void OnValidate()
        {
            ReloadSysData();
        }

        public T GetLevelModule<T>() where T : LevelModule
        {
            T result = null;
            if (levelModules != null && levelModules.Count > 0)
            {
                for (int i = 0; i < levelModules.Count; i++)
                {
                    var mod = levelModules[i];
                    if (mod == null) { continue; }
                    if (mod.GetType() == typeof(T))
                    {
                        result = (T)mod;
                        break;
                    }
                }
            }

            return result;
        }

        public void PauseGame() 
        { 
            if (isPlayingCutScene) 
            {
                GLog.PrintError("You can not pause the game while cutscene is playing. Wait for it to finish. Ignored!");
                return; 
            } 
            isPaused = true; 
        }

        public void ResumeGame() 
        {
            if (isPlayingCutScene)
            {
                GLog.PrintError("You can not resume a paused game while cutscene is playing. Wait for it to finish. Ignored!");
                return;
            }
            isPaused = false; 
        }

        public void EndLevelGameplay()
        {
            onLevelGameplayEnd?.Invoke();
            onLevelGameplayEndEv?.Invoke();
            lvGameplayEnded = true;
            StartCoroutine(Ender());
            IEnumerator Ender()
            {
                if (useEndCutScene)
                {
                    yield return StartCoroutine(OnEndScriptCutScene());
                    if (director != null && endCutScene != null)
                    {
                        PlayCutScene(endCutScene, director, null, null);
                    }
                }
            }
        }

        public void PlayCutScene(PlayableAsset cutScene, PlayableDirector director, IEnumerator scriptCutScene, System.Action OnComplete)
        {
            StartCoroutine(CutScener());
            IEnumerator CutScener()
            {
                isPlayingCutScene = true;
                if (scriptCutScene != null)
                {
                    yield return StartCoroutine(scriptCutScene);
                }

                PlayableDirector dir = director;
                if (director == null) { dir = this.director; }
                this.director.Play(cutScene);
                var hnd = TimelineCallbackHandler.Create(dir, () =>
                {
                    OnComplete?.Invoke();
                    isPlayingCutScene = false;
                });
            }
        }

        // Start is called before the first frame update
        IEnumerator Start()
        {
            ReloadSysData();
            OnLevelStart();
            onLevelStart?.Invoke();
            OnLevelStartEv?.Invoke();
            if (levelModules.Count > 0)
            {
                for (int i = 0; i < levelModules.Count; i++)
                {
                    var sys = levelModules[i];
                    if (sys == null)
                    {
                        throw new System.Exception("By design there can not be any null module. Module initialization error.");
                    }
                    sys.SetLevelManager(this);
                    sys.OnInit();
                    StartCoroutine(sys.OnInitAsync());
                }
            }

            if (useStartCutScene)
            {
                isPlayingCutScene = true;
                yield return StartCoroutine(OnStartScriptCutScene());
                if (director != null && startCutScene != null)
                {
                    PlayCutScene(startCutScene, director, null, null);
                }
            }

            OnLevelGameplayStart();
            onLevelGameplayStart?.Invoke();
            OnLevelGameplayStartEv?.Invoke();
            lvGameplayStarted = true;
            isPlayingCutScene = false;

            while (true)
            {
                if (WhenLevelGameplayAutoEnd())
                {
                    EndLevelGameplay();
                    break;
                }
                yield return null;
            }
        }

        void Update()
        {
            if (isPlayingCutScene || lvGameplayStarted == false || lvGameplayEnded || isPaused) { return; }
            for (int i = 0; i < levelModules.Count; i++)
            {
                var sys = levelModules[i];
                sys.OnTick();
            }
            OnTick();
        }

        void FixedUpdate()
        {
            if (isPlayingCutScene || lvGameplayStarted == false || lvGameplayEnded || isPaused) { return; }
            for (int i = 0; i < levelModules.Count; i++)
            {
                var sys = levelModules[i];
                sys.OnPhysxTick();
            }
            OnPhysxTick();
        }
    }
}