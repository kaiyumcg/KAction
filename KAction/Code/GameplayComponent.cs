using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        protected internal virtual void OnEnterActorVolume(ReactorActor rival, FPhysicsShape rivalShape, FPhysicsShape ownShape) { }
        protected internal virtual IEnumerator OnEnterActorVolumeAsync(ReactorActor rival, FPhysicsShape rivalShape, FPhysicsShape ownShape) { yield return null; }
        protected internal virtual void OnActorHit(ReactorActor rival, FPhysicsShape rivalShape, FPhysicsShape ownShape) { }
        protected internal virtual IEnumerator OnActorHitAsync(ReactorActor rival, FPhysicsShape rivalShape, FPhysicsShape ownShape) { yield return null; }

        protected internal virtual void OnExitActorVolume(ReactorActor rival, FPhysicsShape rivalShape, FPhysicsShape ownShape) { }
        protected internal virtual IEnumerator OnExitActorVolumeAsync(ReactorActor rival, FPhysicsShape rivalShape, FPhysicsShape ownShape) { yield return null; }
        protected internal virtual void OnStopActorHit(ReactorActor rival, FPhysicsShape rivalShape, FPhysicsShape ownShape) { }
        protected internal virtual IEnumerator OnStopActorHitAsync(ReactorActor rival, FPhysicsShape rivalShape, FPhysicsShape ownShape) { yield return null; }
        [SerializeField] List<GameplayTag> componentTags;

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