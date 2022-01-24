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

        GameLevel level;
        internal void SetLevelManager(GameLevel level) { this.level = level; }
        protected GameLevel Level { get { return level; } }
    }
}