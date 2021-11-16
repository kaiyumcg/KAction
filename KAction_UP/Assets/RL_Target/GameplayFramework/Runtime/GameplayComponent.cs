using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Md. Al Kaiyum(Rumman)
/// Email: kaiyumce06rumman@gmail.com
/// Gameplay Component Interface.
/// </summary>
namespace GameplayFramework
{
    public abstract class GameplayComponent : MonoBehaviour
    {
        protected internal virtual void OnStartOrSpawnActor() { }
        protected internal virtual void StartComponent() { }
        protected internal virtual void UpdateComponent(float dt, float fixedDt) { }
        protected internal virtual void UpdateComponentPhysics(float dt, float fixedDt) { }
#if UNITY_EDITOR
        protected virtual void OnEditorUpdate() { }
#endif
        protected internal virtual void OnCleanupComponent() { }

        protected internal virtual void OnEnterActorVolume(Actor rival) { }
        protected internal virtual IEnumerator OnEnterActorVolumeAsync(Actor rival) { yield return null; }
        protected internal virtual void OnActorHit(Actor rival) { }
        protected internal virtual IEnumerator OnActorHitAsync(Actor rival) { yield return null; }

        protected internal virtual void OnExitActorVolume(Actor rival) { }
        protected internal virtual IEnumerator OnExitActorVolumeAsync(Actor rival) { yield return null; }
        protected internal virtual void OnStopActorHit(Actor rival) { }
        protected internal virtual IEnumerator OnStopActorHitAsync(Actor rival) { yield return null; }

        internal void SetOwner(Actor actor)
        {
            owner = actor;
        }

        [SerializeField] [HideInInspector] Actor owner;
        public Actor Owner { get { return owner; } }

#if UNITY_EDITOR
        private void OnValidate()
        {
            OnEditorUpdate();
        }
#endif
    }
}