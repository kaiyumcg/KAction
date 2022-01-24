using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public abstract class GameSystem : MonoBehaviour
    {
        protected internal virtual void InitSystem() { }
        protected internal virtual IEnumerator InitSystemAsync() { yield return null; }
        protected internal virtual void UpdateSystem() { }

        GameManager gameMan;
        internal void SetGameManager(GameManager gameMan) { this.gameMan = gameMan; }
        protected GameManager GameMan { get { return gameMan; } }
    }
}