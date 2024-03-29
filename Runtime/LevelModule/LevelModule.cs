using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public abstract class LevelModule : MonoBehaviour
    {
        /// <summary>
        /// baked into by editor tool
        /// </summary>
        [SerializeField, HideInInspector] GameLevel level;
#if UNITY_EDITOR
        public void SetEd_level(GameLevel level) { this.level = level; }
#endif
        protected internal virtual void OnInit() { }
        protected internal abstract void OnTick(float delta);
        protected internal abstract void OnPhysicsTick(float delta, float physxDelta);
        protected internal virtual void OnEndFrameTick(float delta) { }
        protected internal virtual void OnPause() { }
        protected internal virtual void OnResume() { }
        protected internal virtual void OnCustomTimeDilation(float slowDown) { }//todo time reverse?
        protected internal virtual void OnResetTimeDilation() { }
        protected internal virtual void OnStartGameplay() { }
        protected internal virtual void OnEndGameplay() { }
        protected GameLevel Level { get { return level; } }
    }
}