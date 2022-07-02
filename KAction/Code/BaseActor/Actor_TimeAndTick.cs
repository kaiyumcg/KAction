using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public abstract partial class Actor : MonoBehaviour
    {
        [SerializeField] float timeScale = 1.0f;
        [SerializeField] bool pauseResumeAffectsChildActors = false, customTimeDilationAffectsChildActors = false;

        public bool PauseResumeAffectChildActors { get { return pauseResumeAffectsChildActors; } set { pauseResumeAffectsChildActors = value; } }
        public bool CustomTimeDilationAffectChildActors { get { return customTimeDilationAffectsChildActors; } set { customTimeDilationAffectsChildActors = value; } }
        public float TimeScale
        {
            get
            {
                return timeScale;
            }
            set
            {
                timeScale = value;
                if (customTimeDilationAffectsChildActors)
                {
                    if (childActorListDirty == false)
                    {
                        if (childActors != null && childActors.Count > 0)
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
        }

        #region TickFunc
        //for actor manager
        internal void Tick(float dt_from_unity, float fixedDt_from_unity)
        {
            if (!gameplayRun || isDead || deathStarted || !isRoot) { return; }
            dt_from_unity *= timeScale;
            fixedDt_from_unity *= timeScale;
            TickActorInternal(dt_from_unity, fixedDt_from_unity);
        }

        private protected void TickActorInternal(float dt_from_unity, float fixedDt_from_unity)
        {
            UpdateActor(dt_from_unity, fixedDt_from_unity);
            if (childActorListDirty == false)
            {
                for (int i = 0; i < childActors.Count; i++)
                {
                    childActors[i].TickActorInternal(dt_from_unity, fixedDt_from_unity);
                }
            }

            if (componentListDirty) { return; }
            for (int i = 0; i < gameplayComponents.Count; i++)
            {
                gameplayComponents[i].UpdateComponent(dt_from_unity, fixedDt_from_unity);
            }
        }

        #endregion

        #region PhysicsTickFunc
        internal void TickPhysics(float dt_from_unity, float fixedDt_from_unity)
        {
            if (!gameplayRun || isDead || deathStarted || !isRoot) { return; }
            dt_from_unity *= timeScale;
            fixedDt_from_unity *= timeScale;
            TickPhysicsInternal(dt_from_unity, fixedDt_from_unity);
        }

        private protected void TickPhysicsInternal(float dt, float fixedDt)
        {
            UpdateActorPhysics(dt, fixedDt);

            if (childActorListDirty == false)
            {
                for (int i = 0; i < childActors.Count; i++)
                {
                    childActors[i].TickPhysicsInternal(dt, fixedDt);//so child dead hoileo ki child run korbe? eta fix korte hobe!
                }
            }

            if (componentListDirty) { return; }
            for (int i = 0; i < gameplayComponents.Count; i++)
            {
                gameplayComponents[i].UpdateComponentPhysics(dt, fixedDt);
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
    }
}