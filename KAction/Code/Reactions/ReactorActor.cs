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
        List<Collider2D> intTrig2Ds;
        List<Collision2D> intCol2Ds;
        
        public event OnDoAnything<ReactorActor, FPhysicsShape, FPhysicsShape> OnEnter, OnHit, OnExit, OnStopHit;
        public List<Collider> InteractedVolumes { get { return intTrigs; } }
        public List<Collision> InteractedCollisions { get { return intCols; } }
        public List<Collider2D> InteractedVolumes2D { get { return intTrig2Ds; } }
        public List<Collision2D> InteractedCollisions2D { get { return intCol2Ds; } }

        protected virtual void OnEnterVolume(ReactorActor rival, FPhysicsShape rivalShape, FPhysicsShape ownShape) { }
        protected virtual IEnumerator OnEnterVolumeAsync(ReactorActor rival, FPhysicsShape rivalShape, FPhysicsShape ownShape) { yield return null; }
        protected virtual void OnStartHitActor(ReactorActor rival, FPhysicsShape rivalShape, FPhysicsShape ownShape) { }
        protected virtual IEnumerator OnStartHitActorAsync(ReactorActor rival, FPhysicsShape rivalShape, FPhysicsShape ownShape) { yield return null; }

        protected virtual void OnExitVolume(ReactorActor rival, FPhysicsShape rivalShape, FPhysicsShape ownShape) { }
        protected virtual IEnumerator OnExitVolumeAsync(ReactorActor rival, FPhysicsShape rivalShape, FPhysicsShape ownShape) { yield return null; }
        protected virtual void OnStopHitActor(ReactorActor rival, FPhysicsShape rivalShape, FPhysicsShape ownShape) { }
        protected virtual IEnumerator OnStopHitActorAsync(ReactorActor rival, FPhysicsShape rivalShape, FPhysicsShape ownShape) { yield return null; }

        protected virtual bool SetInteractionValidity(ReactorActor rival, FPhysicsShape rivalShape, FPhysicsShape ownShape) { return false; }
        protected override void OnCleanup()
        {
            base.OnCleanup();
            tCount = cCount = 0;
            intTrigs = new List<Collider>();
            intCols = new List<Collision>();
            intTrig2Ds = new List<Collider2D>();
            intCol2Ds = new List<Collision2D>();
        }

        bool CheckValidityAgainstMode(int intCount, int maxCount)
        {
            if (mode == InteractionMode.Infinite) { return true; }
            else if (mode == InteractionMode.MultipleTime) { return intCount < maxCount; }
            else { return intCount == 0; }
        }

        internal void ProcessVolume(Collider rivalCollider, ReactorActor rivalActor, bool enter, FPhysicsShape rivalShape, FPhysicsShape ownShape)
        {
            if (!needVolumeExitEvent && enter == false) { return; }

            if (IsDead || HasDeathBeenStarted || !useVolume) { return; }
            if (SetInteractionValidity(rivalActor, rivalShape, ownShape) && CheckValidityAgainstMode(tCount, maxVolumeInteractionCount))
            {
                tCount++;
                if (intTrigs == null) { intTrigs = new List<Collider>(); }
                if (intTrigs.Contains(rivalCollider)) { intTrigs.Remove(rivalCollider); }
                if (enter)
                {
                    if (intTrigs == null) { intTrigs = new List<Collider>(); }
                    intTrigs.Add(rivalCollider);
                }
                if (intTrigs == null) { intTrigs = new List<Collider>(); }

                ProcessVolumeInternal(rivalActor, enter, rivalShape, ownShape);
            }
        }

        internal void ProcessSolid(Collision rivalCollision, ReactorActor rivalActor, bool isHit, FPhysicsShape rivalShape, FPhysicsShape ownShape)
        {
            if (!needHitStopEvent && isHit == false) { return; }
            if (IsDead || HasDeathBeenStarted || !useCollider) { return; }
            if (SetInteractionValidity(rivalActor, rivalShape, ownShape) && CheckValidityAgainstMode(cCount, maxCollisionInteractionCount))
            {
                cCount++;

                if (intCols == null) { intCols = new List<Collision>(); }
                if (intCols.Contains(rivalCollision)) { intCols.Remove(rivalCollision); }
                if (isHit)
                {
                    if (intCols == null) { intCols = new List<Collision>(); }
                    intCols.Add(rivalCollision);
                }
                if (intCols == null) { intCols = new List<Collision>(); }

                ProcessSolidInternal(rivalActor, isHit, rivalShape, ownShape);
            }
        }

        internal void ProcessVolume2D(Collider2D rivalCollider, ReactorActor rivalActor, bool enter, FPhysicsShape rivalShape, FPhysicsShape ownShape)
        {
            if (!needVolumeExitEvent && enter == false) { return; }
            if (IsDead || HasDeathBeenStarted || !useVolume) { return; }
            if (SetInteractionValidity(rivalActor, rivalShape, ownShape) && CheckValidityAgainstMode(tCount, maxVolumeInteractionCount))
            {
                tCount++;

                if (intTrig2Ds == null) { intTrig2Ds = new List<Collider2D>(); }
                if (intTrig2Ds.Contains(rivalCollider)) { intTrig2Ds.Remove(rivalCollider); }
                if (enter)
                {
                    if (intTrig2Ds == null) { intTrig2Ds = new List<Collider2D>(); }
                    intTrig2Ds.Add(rivalCollider);
                }
                if (intTrig2Ds == null) { intTrig2Ds = new List<Collider2D>(); }

                ProcessVolumeInternal(rivalActor, enter, rivalShape, ownShape);
            }
        }

        internal void ProcessSolid2D(Collision2D rivalCollision, ReactorActor rivalActor, bool isHit, FPhysicsShape rivalShape, FPhysicsShape ownShape)
        {
            if (!needHitStopEvent && isHit == false) { return; }
            if (IsDead || HasDeathBeenStarted || !useCollider) { return; }
            if (SetInteractionValidity(rivalActor, rivalShape, ownShape) && CheckValidityAgainstMode(cCount, maxCollisionInteractionCount))
            {
                cCount++;

                if (intCol2Ds == null) { intCol2Ds = new List<Collision2D>(); }
                if (intCol2Ds.Contains(rivalCollision)) { intCol2Ds.Remove(rivalCollision); }
                if (isHit)
                {
                    if (intCol2Ds == null) { intCol2Ds = new List<Collision2D>(); }
                    intCol2Ds.Add(rivalCollision);
                }
                if (intCol2Ds == null) { intCol2Ds = new List<Collision2D>(); }

                ProcessSolidInternal(rivalActor, isHit, rivalShape, ownShape);
            }
        }

        void ProcessSolidInternal(ReactorActor rival, bool isHit, FPhysicsShape rivalShape, FPhysicsShape ownShape)
        {
            if (isHit)
            {
                onHit?.Invoke();
                OnHit?.Invoke(rival, rivalShape, ownShape);
                OnStartHitActor(rival, rivalShape, ownShape);
                StartCoroutine(OnStartHitActorAsync(rival, rivalShape, ownShape));
            }
            else
            {
                onStopHit?.Invoke();
                OnStopHit?.Invoke(rival, rivalShape, ownShape);
                OnStopHitActor(rival,rivalShape, ownShape);
                StartCoroutine(OnStopHitActorAsync(rival, rivalShape, ownShape));
            }

            if (componentListDirty == false)
            {
                for (int i = 0; i < gameplayComponents.Count; i++)
                {
                    if (isHit)
                    {
                        gameplayComponents[i].OnActorHit(rival, rivalShape, ownShape);
                        StartCoroutine(gameplayComponents[i].OnActorHitAsync(rival, rivalShape, ownShape));
                    }
                    else
                    {
                        gameplayComponents[i].OnStopActorHit(rival, rivalShape, ownShape);
                        StartCoroutine(gameplayComponents[i].OnStopActorHitAsync(rival, rivalShape, ownShape));
                    }
                }
            }
        }

        void ProcessVolumeInternal(ReactorActor rival, bool enter, FPhysicsShape rivalShape, FPhysicsShape ownShape)
        {
            if (enter)
            {
                onEnter?.Invoke();
                OnEnter?.Invoke(rival, rivalShape, ownShape);
                OnEnterVolume(rival, rivalShape, ownShape);
                StartCoroutine(OnEnterVolumeAsync(rival, rivalShape, ownShape));
            }
            else
            {
                onExit?.Invoke();
                OnExit?.Invoke(rival, rivalShape, ownShape);
                OnExitVolume(rival, rivalShape, ownShape);
                StartCoroutine(OnExitVolumeAsync(rival, rivalShape, ownShape));
            }

            if (componentListDirty == false)
            {
                for (int i = 0; i < gameplayComponents.Count; i++)
                {
                    if (enter)
                    {
                        gameplayComponents[i].OnEnterActorVolume(rival, rivalShape, ownShape);
                        StartCoroutine(gameplayComponents[i].OnEnterActorVolumeAsync(rival, rivalShape, ownShape));
                    }
                    else
                    {
                        gameplayComponents[i].OnExitActorVolume(rival, rivalShape, ownShape);
                        StartCoroutine(gameplayComponents[i].OnExitActorVolumeAsync(rival, rivalShape, ownShape));
                    }
                }
            }
        }
    }
}