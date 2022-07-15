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
        [SerializeField] internal List<LevelModule> levelModules = new List<LevelModule>();
        [SerializeField, HideInInspector] internal PlayableDirector director;

        [Header("Level Start and End Setting")]
        [SerializeField] bool customLogicForLevelCompletion = false;
        //[SerializeField] bool useStartCutScene = false, useEndCutScene = false;
        [SerializeField] PlayableAsset startCutScene = null, endCutScene = null;

        [Header("Klog Config for this level")]
        [SerializeField] internal LogDataSize runtimeCloudLogSize = LogDataSize.Optimal;
        [SerializeField, Multiline] string logUploadEndPoint = "";

        [HideInInspector] internal List<LogDataMinimal> gameplayLogs_min;
        [HideInInspector] internal List<LogDataVerbose> gameplayLogs_verbose;
        [HideInInspector] internal List<LogDataOptimal> gameplayLogs_optimal;

        bool lvGameplayStarted = false, lvGameplayEnded = false, isPlayingCutScene = false, isPaused = false;
        static GameLevel instance = null;
    }
}