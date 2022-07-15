using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public partial class GameLevel : MonoBehaviour
    {
        protected virtual void OnLevelStart() { }
        protected virtual void OnLevelGameplayStart() { }
        protected virtual void OnLevelGameplayEnd() { }

        protected internal virtual void OnLoadNextLevel() { }
        protected internal virtual void OnReloadLevel() { }
        protected internal virtual void OnStartLoadingNextLevel(AsyncOperation asyncOpHandle) { }//streaming thingy

        protected virtual void OnTick() { }
        protected virtual void OnPhysxTick() { }
        protected virtual bool WhenLevelGameplayAutoEnd() { return false; }
        protected virtual IEnumerator OnStartScriptCutScene() { yield break; }
        protected virtual IEnumerator OnEndScriptCutScene() { yield break; }
    }
}