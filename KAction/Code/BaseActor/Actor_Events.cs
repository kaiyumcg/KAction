using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameplayFramework
{
    public abstract partial class Actor : MonoBehaviour
    {
        [SerializeField] UnityEvent onStartOrSpawn = null;
        [SerializeField] internal UnityEvent onPause = null, onResume = null, onStartDeath = null, onDeath = null, onReborn = null;
        [SerializeField] internal UnityEvent<float> onDamage = null, onGainHealth = null;
        [SerializeField] internal UnityEvent<float, Vector3> onDirectionalDamage = null, onDirectionalGainHealth = null;

        public event OnDoAnything2 OnChangeActorData;
        public event OnDoAnything<float> OnDamageEv, OnGainHealthEv;
        public event OnDoAnything<float, Vector3> OnDirectionalDamageEv, OnDirectionalGainHealthEv;
        public event OnDoAnything OnDeathEv, OnStartDeathEv, OnRebornEv, OnPauseEv, OnResumeEv, OnStartOrSpawnEv;

        internal void OnDamageEv_Invoke(float damage) { OnDamageEv?.Invoke(damage); }
        internal void OnGainHealthEv_Invoke(float healthGained) { OnGainHealthEv?.Invoke(healthGained); }
        internal void OnDirectionalDamageEv_Invoke(float damage, Vector3 direction) { OnDirectionalDamageEv?.Invoke(damage, direction); }
        internal void OnDirectionalGainHealthEv_Invoke(float healthGained, Vector3 direction) { OnDirectionalGainHealthEv?.Invoke(healthGained, direction); }
        internal void OnDeathEv_Invoke() { OnDeathEv?.Invoke(); }
        internal void OnStartDeathEv_Invoke() { OnStartDeathEv?.Invoke(); }
        internal void OnRebornEv_Invoke() { OnRebornEv?.Invoke(); }
        internal void OnPauseEv_Invoke() { OnPauseEv?.Invoke(); }
        internal void OnResumeEv_Invoke() { OnResumeEv?.Invoke(); }
    }
}