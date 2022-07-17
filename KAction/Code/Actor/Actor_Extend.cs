using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public abstract partial class Actor : MonoBehaviour
    {
        protected internal virtual void OnDamage(float damageAmount) { }
        protected internal virtual void OnDirectionalDamage(float damageAmount, Vector3 damageDir) { }
        protected internal virtual void OnGainHealth(float addedHealth) { }
        protected internal virtual void OnDirectionalGainHealth(float addedHealth, Vector3 damageDir) { }
        protected internal virtual void OnStartDeath() { }
        protected internal virtual void OnDeath() { }
        protected internal virtual void OnReborn() { }
        protected internal virtual IEnumerator OnStartDeathAsync() { yield break; }

        protected virtual void UpdateActor(float dt, float fixedDt) { }
        protected virtual void UpdateActorPhysics(float dt, float fixedDt) { }

        protected internal virtual void OnPause() { }
        protected internal virtual void OnResume() { }

        protected internal virtual IEnumerator OnStartAsync() { yield break; }

        protected internal virtual void OnStart() { }
        protected internal virtual void OnCleanupActor() { }
#if UNITY_EDITOR
        protected internal virtual void OnEditorUpdate() { }//called from editor inspector custom code. todo
#endif
    }
}