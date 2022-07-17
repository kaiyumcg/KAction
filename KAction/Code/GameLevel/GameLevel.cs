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
        internal void DoLogAction(ErrorType errorType)
        {
            ActionOnLog action = ActionOnLog.DoNothing;
            if (errorType == ErrorType.Exception)
            {
                action = whenException;
            }
            else if (errorType == ErrorType.Error)
            {
                action = whenError;
            }
            else if (errorType == ErrorType.CodeFailure)
            {
                action = whenCodeFailure;
            }
            if (action == ActionOnLog.DoNothing || action == ActionOnLog.Restart) { return; }
            else if (action == ActionOnLog.Stop) { stopped = true; StopAllCoroutines(); }
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
            if (stopped) { KLog.Print("Can not pause game since the level is stopped due to error or exception or code failure!"); return; }
            if (isPlayingCutScene) 
            {
                KLog.ThrowGameplaySDKException(GFType.GameLevel, 
                    "You can not pause the game while cutscene is playing. Wait for it to finish. Ignored!");
                return; 
            } 
            isPaused = true;
            ActorLevelModule.PauseAllActors();
            for (int i = 0; i < levelModules.Count; i++)
            {
                levelModules[i].OnPause();
            }
        }

        public void SetCustomTimeDilation(float factor)
        {
            if (stopped) { KLog.Print("Can not set time dilation since the level is stopped due to error or exception or code failure!"); return; }
            if (isPlayingCutScene)
            {
                KLog.ThrowGameplaySDKException(GFType.GameLevel, 
                    "You can not set time dilation while cutscene is playing. Wait for it to finish. Ignored!");
                return;
            }
            isPaused = true;
            ActorLevelModule.SetCustomTimeForAllActors(factor);
            for (int i = 0; i < levelModules.Count; i++)
            {
                levelModules[i].OnCustomTimeDilation(factor);
            }
        }

        public void ResetTimeDilation()
        {
            if (stopped) { KLog.Print("Can not reset time dilation since the level is stopped due to error or exception or code failure!"); return; }
            if (isPlayingCutScene)
            {
                KLog.ThrowGameplaySDKException(GFType.GameLevel, 
                    "You can not reset time dilation while cutscene is playing. Wait for it to finish. Ignored!");
                return;
            }
            isPaused = true;
            ActorLevelModule.SetCustomTimeForAllActors(1.0f);
            for (int i = 0; i < levelModules.Count; i++)
            {
                levelModules[i].OnResetTimeDilation();
            }
        }

        public void ResumeGame() 
        {
            if (stopped) { KLog.Print("Can not set resume game since the level is stopped due to error or exception or code failure!"); return; }
            if (isPlayingCutScene)
            {
                KLog.ThrowGameplaySDKException(GFType.GameLevel, 
                    "You can not resume a paused game while cutscene is playing. Wait for it to finish. Ignored!");
                return;
            }
            isPaused = false;
            ActorLevelModule.ResumeAllActors();
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
            if (stopped) { KLog.Print("Can not end game since the level is stopped due to error or exception or code failure!"); return; }
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
            if (stopped) { KLog.Print("Can not play cutscene since the level is stopped due to error or exception or code failure!"); return; }
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
            //if user wish to go to playmode from directly a opened scene, redirect from boot scene!
#if UNITY_EDITOR
            if (GameServiceManager.Healthy == false)
            {
                StopAllCoroutines();
                LevelManager.editorModeOpenedScene = SceneManager.GetActiveScene();
                LevelManager.editorModeFlagSet = 1;
                SceneManager.LoadScene(0);
                return;
            }
#endif
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
            if (isPlayingCutScene || lvGameplayStarted == false || lvGameplayEnded || isPaused || stopped) { return; }
            OnTick();
            for (int i = 0; i < levelModules.Count; i++)
            {
                levelModules[i].OnTick();
            }
        }

#if !GF_DISABLE_PHYSICS
        void FixedUpdate()
        {
            if (isPlayingCutScene || lvGameplayStarted == false || lvGameplayEnded || isPaused || stopped) { return; }
            OnPhysxTick();
            for (int i = 0; i < levelModules.Count; i++)
            {
                levelModules[i].OnPhysicsTick();
            }
        }
#endif
    }
}