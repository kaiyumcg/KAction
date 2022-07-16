using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace GameplayFramework
{
    public partial class GameLevel : MonoBehaviour
    {
        /// <summary>
        /// These are baked into by editor tools
        /// </summary>
        [SerializeField] List<LevelModule> levelModules = new List<LevelModule>();
        [SerializeField, HideInInspector] PlayableDirector director;
#if UNITY_EDITOR
        public void SetEd_levelModules(List<LevelModule> levelModules) { this.levelModules = levelModules; }
        public void SetEd_director(PlayableDirector director) { this.director = director; }
#endif
        [SerializeField, HideInInspector]
        ActionOnLog whenError = ActionOnLog.DoNothing,
            whenException = ActionOnLog.DoNothing, whenCodeFailure = ActionOnLog.DoNothing;

        [Header("Level Start and End Setting")]
        [SerializeField] bool customLogicForLevelCompletion = false;
        [SerializeField] PlayableAsset startCutScene = null, endCutScene = null;

        [Header("Klog Config for this level")]
        [SerializeField] internal LogDataSize runtimeCloudLogSize = LogDataSize.Optimal;
        [SerializeField, Multiline] string logUploadEndPoint = "";

        [HideInInspector] internal List<LogDataMinimal> gameplayLogs_min;
        [HideInInspector] internal List<LogDataVerbose> gameplayLogs_verbose;
        [HideInInspector] internal List<LogDataOptimal> gameplayLogs_optimal;

        bool lvGameplayStarted = false, lvGameplayEnded = false, isPlayingCutScene = false, isPaused = false, stopped = false;
        void InitData()
        {
            lvGameplayStarted = lvGameplayEnded = isPlayingCutScene = isPaused = stopped = false;
            gameplayLogs_min = null;
            gameplayLogs_verbose = null;
            gameplayLogs_optimal = null;
        }
        static GameLevel instance = null;
    }
}