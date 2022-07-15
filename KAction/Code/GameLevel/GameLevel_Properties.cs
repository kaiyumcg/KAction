using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public partial class GameLevel : MonoBehaviour
    {
        internal bool IsServer()
        {
#if KML_SUPPORT
            return false;//todo IsHost() or IsServer() from netcode for gameobject
#else
            return false; 
#endif
        }

        public static GameLevel Instance
        {
            get
            {
#if UNITY_EDITOR
                if (Application.isPlaying == false)
                {
                    if (instance == null)
                    {
                        instance = FindObjectOfType<GameLevel>();
                    }
                }
#endif
                return instance;
            }
        }
        public bool HasLevelGameplayBeenStarted { get { return lvGameplayStarted; } }
        public bool HasLevelGameplayBeenEnded { get { return lvGameplayEnded; } }
        public bool IsPlayingCutScene { get { return isPlayingCutScene; } }
        public bool IsPaused { get { return isPaused; } }
    }
}