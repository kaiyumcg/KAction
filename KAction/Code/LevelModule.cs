using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public abstract class LevelModule : MonoBehaviour
    {
        protected internal virtual void OnInit() { }
        protected internal virtual IEnumerator OnInitAsync() { yield return null; }
        protected internal abstract void OnTick();
        protected internal abstract void OnPhysxTick();

        //below are yet to be called from GameLevel. TODO. NOT yet implemented
        protected internal virtual void OnPause() { }
        protected internal virtual void OnResume() { }
        protected internal virtual void OnSlowDown(float slowDown) { }
        protected internal virtual void OnStartGameplay() { }
        protected internal virtual void OnEndGameplay() { }
        //what else need to be called from GameLevel? TODO


        GameLevel level;
        internal void SetLevelManager(GameLevel level) { this.level = level; }
        protected GameLevel Level { get { return level; } }
    }
}