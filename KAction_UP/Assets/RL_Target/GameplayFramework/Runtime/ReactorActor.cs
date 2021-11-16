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
        List<Collider2D> intTrig2Ds;
        List<Collision> intCols;
        List<Collision2D> intCol2Ds;

        public event OnDoAnything<Actor> OnEnter, OnHit, OnExit, OnStopHit;
        public List<Collider> InteractedTrigger { get { return intTrigs; } }
        public List<Collider2D> InteractedTrigger2D { get { return intTrig2Ds; } }
        public List<Collision> InteractedCollision { get { return intCols; } }
        public List<Collision2D> InteractedCollision2D { get { return intCol2Ds; } }

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
            intTrig2Ds = new List<Collider2D>();
            intCols = new List<Collision>();
            intCol2Ds = new List<Collision2D>();
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

            if (Is2D || IsDead || HasDeathBeenStarted || !useVolume) { return; }
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

        #region Trigger2DEvent
        private void OnTriggerEnter2D(Collider2D other)
        {
            ProcessVolume2D(other, true);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            ProcessVolume2D(other, false);
        }

        internal void ProcessVolume2D(Collider2D other, bool enter)
        {
            if (!needVolumeExitEvent && enter == false) { return; }
            if (!Is2D || IsDead || HasDeathBeenStarted || !useVolume) { return; }
            var rival = other.GetComponentInParent<Actor>();
            if (rival != null && SetInteractionValidity(rival) && CheckValidityAgainstMode(tCount, maxVolumeInteractionCount))
            {
                tCount++;

                if (intTrig2Ds == null) { intTrig2Ds = new List<Collider2D>(); }
                if (intTrig2Ds.Contains(other)) { intTrig2Ds.Remove(other); }
                if (enter)
                {
                    if (intTrig2Ds == null) { intTrig2Ds = new List<Collider2D>(); }
                    intTrig2Ds.Add(other);
                }
                if (intTrig2Ds == null) { intTrig2Ds = new List<Collider2D>(); }

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
            if (Is2D || IsDead || HasDeathBeenStarted || !useCollider) { return; }
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

        #region Collision2DEvent
        private void OnCollisionEnter2D(Collision2D collision)
        {
            ProcessSolid2D(collision, true);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            ProcessSolid2D(collision, false);
        }

        internal void ProcessSolid2D(Collision2D other, bool isHit)
        {
            if (!needHitStopEvent && isHit == false) { return; }
            if (!Is2D || IsDead || HasDeathBeenStarted || !useCollider) { return; }
            var rival = other.collider.GetComponentInParent<Actor>();
            if (rival != null && SetInteractionValidity(rival) && CheckValidityAgainstMode(cCount, maxCollisionInteractionCount))
            {
                cCount++;

                if (intCol2Ds == null) { intCol2Ds = new List<Collision2D>(); }
                if (intCol2Ds.Contains(other)) { intCol2Ds.Remove(other); }
                if (isHit)
                {
                    if (intCol2Ds == null) { intCol2Ds = new List<Collision2D>(); }
                    intCol2Ds.Add(other);
                }
                if (intCol2Ds == null) { intCol2Ds = new List<Collision2D>(); }

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