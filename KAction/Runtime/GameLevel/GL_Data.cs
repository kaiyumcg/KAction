using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace GameplayFramework
{
    public partial class GameLevel : MonoBehaviour
    {
        [SerializeField] List<LevelModule> levelModules;//BAKE
        [SerializeField, HideInInspector] PlayableDirector director;//BAKE
        [SerializeField, HideInInspector] ActionOnLog whenError = ActionOnLog.DoNothing,
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
        float delta, fixedDelta;
        void InitData()
        {
            lvGameplayStarted = lvGameplayEnded = isPlayingCutScene = isPaused = stopped = false;
            gameplayLogs_min = null;
            gameplayLogs_verbose = null;
            gameplayLogs_optimal = null;
            if (onLevelStart == null) { onLevelStart = new UnityEngine.Events.UnityEvent(); }
            if (onLevelGameplayStart == null) { onLevelGameplayStart = new UnityEngine.Events.UnityEvent(); }
            if (onLevelGameplayEnd == null) { onLevelGameplayEnd = new UnityEngine.Events.UnityEvent(); }
        }
        static GameLevel instance = null;
    }
}