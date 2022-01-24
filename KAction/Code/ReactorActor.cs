using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameplayFramework
{
    public abstract class ReactorActor : Actor
    {
        [SerializeField] InteractionMode mode = InteractionMode.Infinite;
        [SerializeField] int maxVolumeInteractionCount = 1, maxCollisionInteractionCount = 1;
        [SerializeField] bool useVolume = true, useCollider = true, needVolumeExitEvent = false, needHitStopEvent = false;
        [SerializeField] UnityEvent onEnter, onHit, onExit, onStopHit;

        int tCount = 0, cCount = 0;
        List<Collider> intTrigs;
        List<Collision> intCols;

        public event OnDoAnything<Actor> OnEnter, OnHit, OnExit, OnStopHit;
        public List<Collider> InteractedTrigger { get { return intTrigs; } }
        public List<Collision> InteractedCollision { get { return intCols; } }

        protected virtual void OnEnterVolume(Actor rival) { }
        protected virtual IEnumerator OnEnterVolumeAsync(Actor rival) { yield return null; }
        protected virtual void OnStartHitActor(Actor rival) { }
        protected virtual IEnumerator OnStartHitActorAsync(Actor rival) { yield return null; }

        protected virtual void OnExitVolume(Actor rival) { }
        protected virtual IEnumerator OnExitVolumeAsync(Actor rival) { yield return null; }
        protected virtual void OnStopHitActor(Actor rival) { }
        protected virtual IEnumerator OnStopHitActorAsync(Actor rival) { yield return null; }

        protected virtual bool SetInteractionValidity(Actor rival) { return false; }
        protected override void OnCleanup()
        {
            base.OnCleanup();
            tCount = cCount = 0;
            intTrigs = new List<Collider>();
            intCols = new List<Collision>();
        }

        bool CheckValidityAgainstMode(int intCount, int maxCount)
        {
            if (mode == InteractionMode.Infinite) { return true; }
            else if (mode == InteractionMode.MultipleTime) { return intCount < maxCount; }
            else { return intCount == 0; }
        }

        #region TriggerEvent
        private void OnTriggerEnter(Collider other)
        {
            ProcessVolume(other, true);
        }

        private void OnTriggerExit(Collider other)
        {
            ProcessVolume(other, false);
        }

        internal void ProcessVolume(Collider other, bool enter)
        {
            if (!needVolumeExitEvent && enter == false) { return; }

            if (IsDead || HasDeathBeenStarted || !useVolume) { return; }
            var rival = other.GetComponentInParent<Actor>();
            if (rival != null && SetInteractionValidity(rival) && CheckValidityAgainstMode(tCount, maxVolumeInteractionCount))
            {
                tCount++;
                if (intTrigs == null) { intTrigs = new List<Collider>(); }
                if (intTrigs.Contains(other)) { intTrigs.Remove(other); }
                if (enter)
                {
                    if (intTrigs == null) { intTrigs = new List<Collider>(); }
                    intTrigs.Add(other);
                }
                if (intTrigs == null) { intTrigs = new List<Collider>(); }

                ProcessVolumeInternal(rival, enter);
            }
        }

        #endregion

        #region CollisionEvent
        private void OnCollisionEnter(Collision other)
        {
            ProcessSolid(other, true);
        }

        private void OnCollisionExit(Collision other)
        {
            ProcessSolid(other, false);
        }

        internal void ProcessSolid(Collision other, bool isHit)
        {
            if (!needHitStopEvent && isHit == false) { return; }
            if (IsDead || HasDeathBeenStarted || !useCollider) { return; }
            var rival = other.collider.GetComponentInParent<Actor>();
            if (rival != null && SetInteractionValidity(rival) && CheckValidityAgainstMode(cCount, maxCollisionInteractionCount))
            {
                cCount++;

                if (intCols == null) { intCols = new List<Collision>(); }
                if (intCols.Contains(other)) { intCols.Remove(other); }
                if (isHit)
                {
                    if (intCols == null) { intCols = new List<Collision>(); }
                    intCols.Add(other);
                }
                if (intCols == null) { intCols = new List<Collision>(); }

                ProcessSolidInternal(rival, isHit);
            }
        }

        #endregion

        void ProcessSolidInternal(Actor rival, bool isHit)
        {
            if (isHit)
            {
                onHit?.Invoke();
                OnHit?.Invoke(rival);
                OnStartHitActor(rival);
                StartCoroutine(OnStartHitActorAsync(rival));
            }
            else
            {
                onStopHit?.Invoke();
                OnStopHit?.Invoke(rival);
                OnStopHitActor(rival);
                StartCoroutine(OnStopHitActorAsync(rival));
            }

            if (componentListDirty == false)
            {
                for (int i = 0; i < gameplayComponents.Count; i++)
                {
                    if (isHit)
                    {
                        gameplayComponents[i].OnActorHit(rival);
                        StartCoroutine(gameplayComponents[i].OnActorHitAsync(rival));
                    }
                    else
                    {
                        gameplayComponents[i].OnStopActorHit(rival);
                        StartCoroutine(gameplayComponents[i].OnStopActorHitAsync(rival));
                    }
                }
            }
        }

        void ProcessVolumeInternal(Actor rival, bool enter)
        {
            if (enter)
            {
                onEnter?.Invoke();
                OnEnter?.Invoke(rival);
                OnEnterVolume(rival);
                StartCoroutine(OnEnterVolumeAsync(rival));
            }
            else
            {
                onExit?.Invoke();
                OnExit?.Invoke(rival);
                OnExitVolume(rival);
                StartCoroutine(OnExitVolumeAsync(rival));
            }

            if (componentListDirty == false)
            {
                for (int i = 0; i < gameplayComponents.Count; i++)
                {
                    if (enter)
                    {
                        gameplayComponents[i].OnEnterActorVolume(rival);
                        StartCoroutine(gameplayComponents[i].OnEnterActorVolumeAsync(rival));
                    }
                    else
                    {
                        gameplayComponents[i].OnExitActorVolume(rival);
                        StartCoroutine(gameplayComponents[i].OnExitActorVolumeAsync(rival));
                    }
                }
            }
        }
    }
}