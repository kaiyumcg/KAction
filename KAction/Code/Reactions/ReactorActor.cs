using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameplayFramework
{
    public abstract class ReactorActor : Actor
    {
        [SerializeField] int maxVolumeInteractionCount = 1, maxCollisionInteractionCount = 1;
        [SerializeField] UnityEvent<ReactorActor, FPhysicsShape, FPhysicsShape> onEnter, onHit, onExit, onStopHit;

        Rigidbody rgd;
        Rigidbody2D rgd2D;
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
        public Rigidbody PhysicsBody { get { return rgd; } }
        public Rigidbody2D PhysicsBody2D { get { return rgd2D; } }

        protected virtual void OnEnterVolume(ReactorActor rival, FPhysicsShape rivalShape, FPhysicsShape ownShape) { }
        protected virtual IEnumerator OnEnterVolumeAsync(ReactorActor rival, FPhysicsShape rivalShape, FPhysicsShape ownShape) { yield break; }
        protected virtual void OnStartHitActor(ReactorActor rival, FPhysicsShape rivalShape, FPhysicsShape ownShape) { }
        protected virtual IEnumerator OnStartHitActorAsync(ReactorActor rival, FPhysicsShape rivalShape, FPhysicsShape ownShape) { yield break; }

        protected virtual void OnExitVolume(ReactorActor rival, FPhysicsShape rivalShape, FPhysicsShape ownShape) { }
        protected virtual IEnumerator OnExitVolumeAsync(ReactorActor rival, FPhysicsShape rivalShape, FPhysicsShape ownShape) { yield break; }
        protected virtual void OnStopHitActor(ReactorActor rival, FPhysicsShape rivalShape, FPhysicsShape ownShape) { }
        protected virtual IEnumerator OnStopHitActorAsync(ReactorActor rival, FPhysicsShape rivalShape, FPhysicsShape ownShape) { yield break; }

        protected virtual bool SetInteractionValidity(ReactorActor rival, FPhysicsShape rivalShape, FPhysicsShape ownShape) { return false; }

        protected override void OnEndActor()
        {
            base.OnEndActor();
            tCount = cCount = 0;
            intTrigs = new List<Collider>();
            intCols = new List<Collision>();
            intTrig2Ds = new List<Collider2D>();
            intCol2Ds = new List<Collision2D>();
        }

        protected override void OnStartActor()
        {
            if (onEnter == null) { onEnter = new UnityEvent<ReactorActor, FPhysicsShape, FPhysicsShape>(); }
            if (onHit == null) { onHit = new UnityEvent<ReactorActor, FPhysicsShape, FPhysicsShape>(); }
            if (onExit == null) { onExit = new UnityEvent<ReactorActor, FPhysicsShape, FPhysicsShape>(); }
            if (onStopHit == null) { onStopHit = new UnityEvent<ReactorActor, FPhysicsShape, FPhysicsShape>(); }
            base.OnStartActor();
            rgd = ActorLevelModule.instance.ReactorBodies[this];
            rgd2D = ActorLevelModule.instance.ReactorBodies2D[this];
        }

        internal void ProcessVolume(Collider rivalCollider3D, Collider2D rivalCollider2D, 
            ReactorActor rivalActor, bool enter, FPhysicsShape rivalShape, FPhysicsShape ownShape)
        {
            if (IsDead || HasDeathBeenStarted || SetInteractionValidity(rivalActor, rivalShape, ownShape) == false) { return; }
            tCount++;
            if (ReferenceEquals(rivalCollider3D, null))
            {
                if (intTrig2Ds == null) { intTrig2Ds = new List<Collider2D>(); }
                if (intTrig2Ds.Contains(rivalCollider2D)) { intTrig2Ds.Remove(rivalCollider2D); }
            }
            else
            {
                if (intTrigs == null) { intTrigs = new List<Collider>(); }
                if (intTrigs.Contains(rivalCollider3D)) { intTrigs.Remove(rivalCollider3D); }
            }

            if (enter)
            {
                if (ReferenceEquals(rivalCollider3D, null))
                {
                    if (intTrig2Ds == null) { intTrig2Ds = new List<Collider2D>(); }
                    intTrig2Ds.Add(rivalCollider2D);
                }
                else
                {
                    if (intTrigs == null) { intTrigs = new List<Collider>(); }
                    intTrigs.Add(rivalCollider3D);
                }
            }

            if (ReferenceEquals(rivalCollider3D, null))
            {
                if (intTrig2Ds == null) { intTrig2Ds = new List<Collider2D>(); }
            }
            else
            {
                if (intTrigs == null) { intTrigs = new List<Collider>(); }
            }
            ProcessVolumeInternal(rivalActor, enter, rivalShape, ownShape);

            void ProcessVolumeInternal(ReactorActor rival, bool enter, FPhysicsShape rivalShape, FPhysicsShape ownShape)
            {
                if (enter)
                {
                    onEnter?.Invoke(rival, rivalShape, ownShape);
                    OnEnter?.Invoke(rival, rivalShape, ownShape);
                    OnEnterVolume(rival, rivalShape, ownShape);
                    StartCoroutine(OnEnterVolumeAsync(rival, rivalShape, ownShape));
                }
                else
                {
                    onExit?.Invoke(rival, rivalShape, ownShape);
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

        internal void ProcessSolid(Collision rivalCollision3D, Collision2D rivalCollision2D, 
            ReactorActor rivalActor, bool isHit, FPhysicsShape rivalShape, FPhysicsShape ownShape)
        {
            if (IsDead || HasDeathBeenStarted || SetInteractionValidity(rivalActor, rivalShape, ownShape) == false) { return; }
            cCount++;
            if (ReferenceEquals(rivalCollision3D, null))
            {
                if (intCol2Ds == null) { intCol2Ds = new List<Collision2D>(); }
                if (intCol2Ds.Contains(rivalCollision2D)) { intCol2Ds.Remove(rivalCollision2D); }
            }
            else
            {
                if (intCols == null) { intCols = new List<Collision>(); }
                if (intCols.Contains(rivalCollision3D)) { intCols.Remove(rivalCollision3D); }
            }

            if (isHit)
            {
                if (ReferenceEquals(rivalCollision3D, null))
                {
                    if (intCol2Ds == null) { intCol2Ds = new List<Collision2D>(); }
                    intCol2Ds.Add(rivalCollision2D);
                }
                else
                {
                    if (intCols == null) { intCols = new List<Collision>(); }
                    intCols.Add(rivalCollision3D);
                }

            }

            if (ReferenceEquals(rivalCollision3D, null))
            {
                if (intCol2Ds == null) { intCol2Ds = new List<Collision2D>(); }
            }
            else
            {
                if (intCols == null) { intCols = new List<Collision>(); }
            }
            ProcessSolidInternal(rivalActor, isHit, rivalShape, ownShape);

            void ProcessSolidInternal(ReactorActor rival, bool isHit, FPhysicsShape rivalShape, FPhysicsShape ownShape)
            {
                if (isHit)
                {
                    onHit?.Invoke(rival, rivalShape, ownShape);
                    OnHit?.Invoke(rival, rivalShape, ownShape);
                    OnStartHitActor(rival, rivalShape, ownShape);
                    StartCoroutine(OnStartHitActorAsync(rival, rivalShape, ownShape));
                }
                else
                {
                    onStopHit?.Invoke(rival, rivalShape, ownShape);
                    OnStopHit?.Invoke(rival, rivalShape, ownShape);
                    OnStopHitActor(rival, rivalShape, ownShape);
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
        }
    }
}