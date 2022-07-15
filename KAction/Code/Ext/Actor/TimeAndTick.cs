using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public static partial class ActorAPI
    {
        static Coroutine _Wait(Actor actor, float amountInScaledTime, System.Action OnComplete)
        {
            return actor.StartCoroutine(Waiter(amountInScaledTime, OnComplete));
            IEnumerator Waiter(float amount_wt, System.Action OnComplete)
            {
                yield return new WaitForSeconds(amount_wt);
                OnComplete?.Invoke();
            }
        }

        static void _SetTickForGameplayComponents(Actor actor, bool tick)
        {
            var gameplayComponents = actor.gameplayComponents;
            if (gameplayComponents != null && gameplayComponents.Count > 0)
            {
                for (int i = 0; i < gameplayComponents.Count; i++)
                {
                    var comp = gameplayComponents[i];
                    if (comp == null) { continue; }
                    comp.canTick = tick;
                }
            }
        }

        static void _SetTick(Actor actor, bool tick)
        {
            var childActors = actor.childActors;
            _SetTickForGameplayComponents(actor, tick);
            for (int i = 0; i < childActors.Count; i++)
            {
                var chActor = childActors[i];
                chActor.SetTick(tick);
            }
        }
    }
}