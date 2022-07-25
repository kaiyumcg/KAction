using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameplayFramework
{
    public abstract partial class Actor : MonoBehaviour
    {
        [SerializeField] UnityEvent<string, ActorData> onChangeActorData = null;
        [SerializeField] UnityEvent<float> onDamage = null, onGainHealth = null;
        [SerializeField] UnityEvent<float, Vector3> onDirectionalDamage = null, onDirectionalGainHealth = null;
        [SerializeField] UnityEvent onStartDeath = null, onDeath = null, onPause = null, onResume = null;

        public event OnDoAnything2 OnChangeActorDataEv;
        public event OnDoAnything<float> OnDamageEv, OnGainHealthEv;
        public event OnDoAnything<float, Vector3> OnDirectionalDamageEv, OnDirectionalGainHealthEv;
        public event OnDoAnything OnStartDeathEv, OnDeathEv, OnPauseEv, OnResumeEv;

        internal void Call_OnChangeActorData(string key, ActorData data) 
        { 
            OnChangeActorDataEv?.Invoke(key, data);
            onChangeActorData?.Invoke(key, data);
            OnChangeDataLayer(key, data);
        }

        internal void Call_OnDamage(float damage) 
        { 
            OnDamageEv?.Invoke(damage);
            onDamage?.Invoke(damage);
            OnDamage(damage);
        }

        internal void Call_OnGainHealth(float healthGained) 
        { 
            OnGainHealthEv?.Invoke(healthGained);
            onGainHealth?.Invoke(healthGained);
            OnGainHealth(healthGained);
        }

        internal void Call_DirectionalDamage(float damage, Vector3 direction) 
        { 
            OnDirectionalDamageEv?.Invoke(damage, direction);
            onDirectionalDamage?.Invoke(damage, direction);
            OnDirectionalDamage(damage, direction);
        }

        internal void Call_OnDirectionalGainHealth(float healthGained, Vector3 direction) 
        { 
            OnDirectionalGainHealthEv?.Invoke(healthGained, direction);
            onDirectionalGainHealth?.Invoke(healthGained, direction);
            OnDirectionalGainHealth(healthGained, direction);
        }

        internal void Call_OnStartDeath()
        {
            OnStartDeathEv?.Invoke();
            onStartDeath?.Invoke();
            OnStartDeath();
        }

        internal void Call_OnDeath() 
        { 
            OnDeathEv?.Invoke();
            onDeath?.Invoke();
            OnDeath();
        }

        internal void Call_OnPause() 
        { 
            OnPauseEv?.Invoke();
            onPause?.Invoke();
            OnPause();
        }

        internal void Call_OnResume() 
        { 
            OnResumeEv?.Invoke();
            onResume?.Invoke();
            OnResume();
        }
    }
}