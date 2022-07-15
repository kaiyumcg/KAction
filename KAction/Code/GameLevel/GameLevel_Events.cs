using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameplayFramework
{
    public partial class GameLevel : MonoBehaviour
    {
        public event OnDoAnything OnLevelStartEv, OnLevelGameplayStartEv, onLevelGameplayEndEv;
        [SerializeField] UnityEvent onLevelStart, onLevelGameplayStart, onLevelGameplayEnd;
    }
}