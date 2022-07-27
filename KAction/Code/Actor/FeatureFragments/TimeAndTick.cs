using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public abstract partial class Actor : MonoBehaviour
    {
        public Coroutine Wait(float amountInScaledTime, System.Action OnComplete)
        {
            return StartCoroutine(Waiter(amountInScaledTime, OnComplete));
            IEnumerator Waiter(float amount_wt, System.Action OnComplete)
            {
                yield return new WaitForSeconds(amount_wt);
                OnComplete?.Invoke();
            }
        }

        public void SetTickForGameplayComponents(bool tick)
        {
            if (componentListDirty) { return; }
            for (int i = 0; i < gameplayComponents.Count; i++)
            {
                var comp = gameplayComponents[i];
                if (comp == null) { continue; }
                comp.canTick = tick;
            }
        }

        public void SetTick(bool tick)
        {
            canTick = tick;
            SetTickForGameplayComponents(tick);
            if (childActorListDirty) { return; }
            for (int i = 0; i < childActors.Count; i++)
            {
                var chActor = childActors[i];
                chActor.SetTick(tick);
            }
        }
    }
}