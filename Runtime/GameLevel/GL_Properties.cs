using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public partial class GameLevel : MonoBehaviour
    {
        public static GameLevel Instance
        {
            get
            {
                return instance;
            }
        }
        public bool HasLevelGameplayBeenStarted { get { return lvGameplayStarted; } }
        public bool HasLevelGameplayBeenEnded { get { return lvGameplayEnded; } }
        public bool IsPlayingCutScene { get { return isPlayingCutScene; } }
        public bool IsPaused { get { return isPaused; } }
    }
}