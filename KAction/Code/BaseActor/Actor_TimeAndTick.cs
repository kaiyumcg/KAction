using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public abstract partial class Actor : MonoBehaviour
    {
        [SerializeField] internal float timeScale = 1.0f;
        [SerializeField] bool timeDilationAffectsChildActors = true;
        
        public bool TimeDilationAffectChildActors { get { return timeDilationAffectsChildActors; } set { timeDilationAffectsChildActors = value; } }
        public float TimeScale
        {
            get
            {
                return timeScale;
            }
            set
            {
                timeScale = value;
                if (timeDilationAffectsChildActors)
                {
                    if (childActorListDirty == false)
                    {
                        for (int i = 0; i < childActors.Count; i++)
                        {
                            var ch = childActors[i];
                            if (ch == null) { continue; }
                            ch.TimeScale = value;
                        }
                    }
                }
            }
        }

        #region TickFunc
        internal void Tick(float dt_from_unity, float fixedDt_from_unity)
        {
            if (!isRoot) { return; }
            var dt = dt_from_unity * timeScale;
            var fdt = fixedDt_from_unity * timeScale;
            TickActorInternal(dt, fdt);
        }

        void TickActorInternal(float dt, float fdt)
        {
            if (!gameplayRun || isDead || deathStarted) { return; }
            UpdateActor(dt, fdt);
            if (componentListDirty == false)
            {
                for (int i = 0; i < gameplayComponents.Count; i++)
                {
                    gameplayComponents[i].UpdateComponent(dt, fdt);
                }
            }

            if (childActorListDirty == false)
            {
                var dt_from_unity = ActorLevelModule.RawDelta;
                var fdt_from_unity = ActorLevelModule.RawFixedDelta;
                for (int i = 0; i < childActors.Count; i++)
                {
                    var chActor = childActors[i];
                    var ch_dt = timeDilationAffectsChildActors ? dt : dt_from_unity * chActor.timeScale;
                    var ch_fdt = timeDilationAffectsChildActors ? fdt : fdt_from_unity * chActor.timeScale;
                    childActors[i].TickActorInternal(ch_dt, ch_fdt);
                }
            }
        }

        #endregion

        #region PhysicsTickFunc
        internal void TickPhysics(float dt_from_unity, float fixedDt_from_unity)
        {
            if (!isRoot) { return; }
            var dt = dt_from_unity * timeScale;
            var fdt = fixedDt_from_unity * timeScale;
            TickPhysicsInternal(dt, fdt);
        }

        void TickPhysicsInternal(float dt, float fdt)
        {
            if (!gameplayRun || isDead || deathStarted) { return; }
            UpdateActorPhysics(dt, fdt);
            if (componentListDirty == false)
            {
                for (int i = 0; i < gameplayComponents.Count; i++)
                {
                    gameplayComponents[i].UpdateComponentPhysics(dt, fdt);
                }
            }

            if (childActorListDirty == false)
            {
                var dt_from_unity = ActorLevelModule.RawDelta;
                var fdt_from_unity = ActorLevelModule.RawFixedDelta;
                for (int i = 0; i < childActors.Count; i++)
                {
                    var chActor = childActors[i];
                    var ch_dt = timeDilationAffectsChildActors ? dt : dt_from_unity * chActor.timeScale;
                    var ch_fdt = timeDilationAffectsChildActors ? fdt : fdt_from_unity * chActor.timeScale;
                    childActors[i].TickPhysicsInternal(ch_dt, ch_fdt);
                }
            }
        }
        #endregion

        protected virtual void UpdateActor(float dt, float fixedDt) { }
        protected virtual void UpdateActorPhysics(float dt, float fixedDt) { }

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

        public void SetTick(bool tick)
        {
            SetTickForGameplayComponents(tick);
            for (int i = 0; i < childActors.Count; i++)
            {
                var chActor = childActors[i];
                chActor.SetTick(tick);
            }
        }
    }
}