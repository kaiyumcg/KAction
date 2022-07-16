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
                KLog.ThrowGameplaySDKException(GFType.GameLevel, 
                    "You can not pause the game while cutscene is playing. Wait for it to finish. Ignored!");
                return; 
            } 
            isPaused = true;
            ActorUtil.PauseAllActors();
            for (int i = 0; i < levelModules.Count; i++)
            {
                levelModules[i].OnPause();
            }
        }

        public void SetCustomTimeDilation(float factor)
        {
            if (isPlayingCutScene)
            {
                KLog.ThrowGameplaySDKException(GFType.GameLevel, 
                    "You can not set time dilation while cutscene is playing. Wait for it to finish. Ignored!");
                return;
            }
            isPaused = true;
            ActorUtil.SetCustomTimeForAllActors(factor);
            for (int i = 0; i < levelModules.Count; i++)
            {
                levelModules[i].OnCustomTimeDilation(factor);
            }
        }

        public void ResetTimeDilation()
        {
            if (isPlayingCutScene)
            {
                KLog.ThrowGameplaySDKException(GFType.GameLevel, 
                    "You can not reset time dilation while cutscene is playing. Wait for it to finish. Ignored!");
                return;
            }
            isPaused = true;
            ActorUtil.SetCustomTimeForAllActors(1.0f);
            for (int i = 0; i < levelModules.Count; i++)
            {
                levelModules[i].OnResetTimeDilation();
            }
        }

        public void ResumeGame() 
        {
            if (isPlayingCutScene)
            {
                KLog.ThrowGameplaySDKException(GFType.GameLevel, 
                    "You can not resume a paused game while cutscene is playing. Wait for it to finish. Ignored!");
                return;
            }
            isPaused = false;
            ActorUtil.ResumeAllActors();
            for (int i = 0; i < levelModules.Count; i++)
            {
                levelModules[i].OnResume();
            }
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
                KLog.ThrowGameplaySDKException(GFType.GameLevel, 
                    "Log Upload to Cloud is enabled but the end point is not defined properly. " +
                    "End point must not be null or empty or whitespace and must be valid URL. Define a valid endpoint in inspector.");
            }
            CloudLogUploader.UploadLog(gameplayLogs_min, gameplayLogs_optimal,
            gameplayLogs_verbose, runtimeCloudLogSize, endPoint);
            gameplayLogs_min = null;
            gameplayLogs_optimal = null;
            gameplayLogs_verbose = null;
#endif
        }

        public void EndLevelGameplay(int nextLevelIndex = -1)
        {
            OnLevelGameplayEnd();
            onLevelGameplayEnd?.Invoke();
            onLevelGameplayEndEv?.Invoke();
            lvGameplayEnded = true;
            for (int i = 0; i < levelModules.Count; i++)
            {
                var sys = levelModules[i];
                if (sys == null)
                {
                    KLog.ThrowGameplaySDKException(GFType.GameLevel, "By design there can not be any null module. Module initialization error.");
                }
                sys.OnEndGameplay();
            }

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
                    var svc = GameServiceManager.GetService<LevelManager>();
                    if (svc != null)
                    {
                        if (nextLevelIndex < 0)
                        {
                            svc.LoadNextLevel();
                        }
                        else
                        {
                            svc.LoadLevelByIndex(nextLevelIndex);
                        }
                    }
                    UploadLogsIfReq();
                    OnLoadNextLevel();
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
            InitData();
            StartCoroutine(LevelEntryPoint());
        }
        
        IEnumerator LevelEntryPoint()
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
                        KLog.ThrowGameplaySDKException(GFType.GameLevel, "By design there can not be any null module. Module initialization error.");
                    }
                    sys.OnInit();
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

            for (int i = 0; i < levelModules.Count; i++)
            {
                var sys = levelModules[i];
                if (sys == null)
                {
                    KLog.ThrowGameplaySDKException(GFType.GameLevel, "By design there can not be any null module. Module initialization error.");
                }
                sys.OnStartGameplay();
            }

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
            OnTick();
            for (int i = 0; i < levelModules.Count; i++)
            {
                levelModules[i].OnTick();
            }
        }

        void FixedUpdate()
        {
            if (isPlayingCutScene || lvGameplayStarted == false || lvGameplayEnded || isPaused) { return; }
            OnPhysxTick();
            for (int i = 0; i < levelModules.Count; i++)
            {
                levelModules[i].OnPhysxTick();
            }
        }
    }
}