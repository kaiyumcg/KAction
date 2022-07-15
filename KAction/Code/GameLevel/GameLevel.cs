using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

namespace GameplayFramework
{
    public partial class GameLevel : MonoBehaviour
    {
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
                KLog.PrintError("You can not pause the game while cutscene is playing. Wait for it to finish. Ignored!");
                return; 
            } 
            isPaused = true;
            ActorUtil.PauseAllActors();
        }

        public void ResumeGame() 
        {
            if (isPlayingCutScene)
            {
                KLog.PrintError("You can not resume a paused game while cutscene is playing. Wait for it to finish. Ignored!");
                return;
            }
            isPaused = false;
            ActorUtil.ResumeAllActors();
        }

        private void OnDisable()
        {
            UploadLogsIfReq();
        }

        void UploadLogsIfReq()
        {
#if USE_CLOUD_LOG
            string endPoint = logUploadEndPoint;            
            if (string.IsNullOrEmpty(endPoint) || string.IsNullOrWhiteSpace(endPoint) ||
                endPoint.Contains("https://") == false)
            {
                Debug.LogError("Log Upload to Cloud is enabled but the end point is not defined properly. " +
                    "End point must not be null or empty or whitespace and must be valid URL. Define a valid endpoint in inspector.");
            }
            CloudLogUploader.UploadLog(gameplayLogs_min, gameplayLogs_optimal,
            gameplayLogs_verbose, runtimeCloudLogSize, endPoint);
            gameplayLogs_min = null;
            gameplayLogs_optimal = null;
            gameplayLogs_verbose = null;
#endif
        }

        public void EndLevelGameplay(string nextLevelName = "")
        {
            OnLevelGameplayEnd();
            onLevelGameplayEnd?.Invoke();
            onLevelGameplayEndEv?.Invoke();
            lvGameplayEnded = true;
            StartCoroutine(Ender());
            IEnumerator Ender()
            {
                if (endCutScene != null)
                {
                    yield return StartCoroutine(OnEndScriptCutScene());
                    if (director != null && endCutScene != null)
                    {
                        PlayCutScene(endCutScene, director, null, null);
                    }

                    UploadLogsIfReq();
                    OnLoadNextLevel();
                    var svc = GameServiceManager.GetService<LevelManager>();
                    if (svc != null)
                    {
                        if (string.IsNullOrEmpty(nextLevelName))
                        {
                            svc.LoadNextLevel();
                        }
                        else
                        {
                            svc.LoadLevelFromBuildIndex(nextLevelName);
                        }
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

        //instead of Awake, we will use when server or client or host is started--callback
        private void Awake()
        {
            instance = this;
            StartCoroutine(EntryPoint());
        }
        
        IEnumerator EntryPoint()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                DestroyImmediate(gameObject);
                yield break;
            }

            if ((startCutScene != null || endCutScene != null) && director != null)
            {
                director.playableAsset = null;
                director.playOnAwake = false;
            }

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

            if (startCutScene != null)
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

            if (customLogicForLevelCompletion)
            {
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
        }

        void Update()
        {
            if (isPlayingCutScene || lvGameplayStarted == false || lvGameplayEnded || isPaused) { return; }
            for (int i = 0; i < levelModules.Count; i++)
            {
                levelModules[i].OnTick();
            }
            OnTick();
        }

        void FixedUpdate()
        {
            if (isPlayingCutScene || lvGameplayStarted == false || lvGameplayEnded || isPaused) { return; }
            for (int i = 0; i < levelModules.Count; i++)
            {
                levelModules[i].OnPhysxTick();
            }
            OnPhysxTick();
        }
    }
}