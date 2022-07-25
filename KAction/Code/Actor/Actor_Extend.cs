using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public abstract partial class Actor : MonoBehaviour
    {
        protected virtual void OnDamage(float damageAmount) { }
        protected virtual void OnDirectionalDamage(float damageAmount, Vector3 damageDir) { }
        protected virtual void OnGainHealth(float addedHealth) { }
        protected virtual void OnDirectionalGainHealth(float addedHealth, Vector3 damageDir) { }
        protected virtual void OnStartDeath() { }
        protected virtual void OnDeath() { }
        protected virtual void OnPause() { }
        protected virtual void OnResume() { }
        protected virtual void OnChangeDataLayer(string key, ActorData data) { }

        protected internal virtual IEnumerator OnStartDeathAsync() { yield break; }
        protected internal virtual void OnStart() { }
        protected internal virtual void OnCleanupActor() { }

        protected virtual void UpdateActor(float dt, float fixedDt) { }
        protected virtual void UpdateActorPhysics(float dt, float fixedDt) { }
        
#if UNITY_EDITOR
        public virtual void OnEditorUpdate() { }//called from editor inspector custom code. todo
        public virtual void OnPreBuild() { }//called before the build process then it marks this actor and the containing scene dirty and auto save.
#endif
    }
}