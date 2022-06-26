using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework.Platforming
{
    public class Obstacle : ReactorActor
    {
        [Header("Default rotation setting: ")]
        [SerializeField] ObstacleRotateMode defaultRotationMode = ObstacleRotateMode.Rotate;
        [SerializeField] Vector3 rotateSpeed = Vector3.zero;
        [SerializeField] Transform centerForRotateAround = null;
        [SerializeField] float rotateAroundAngle = 360f;
        [SerializeField] Vector3 centerOffsetForAround = Vector3.zero, rotateAroundAxis = Vector3.zero;

        [Header("Damage setting for interacting actor: ")]
        [SerializeField] float damageAmount = 10f;
        [SerializeField] DamageMode obstacleDamageMode = DamageMode.Relative;
        [SerializeField] bool isLethal = true, useThisVolumeForDamage = true, useThisColliderForDamage = true;
        [SerializeField] List<PlatformObstacleSequence> sequencesForObstacle;
        [Space(5)]

        bool hasTransformCenter = false;
        Vector3 centerVec;

        protected override void OnStart()
        {
            base.OnStart();
            hasTransformCenter = centerForRotateAround != null;
            centerVec = _Transform.position + centerOffsetForAround;

            if (sequencesForObstacle != null && sequencesForObstacle.Count > 0)
            {
                foreach (var seq in sequencesForObstacle)
                {
                    InitSequence(seq);
                }
            }

            void InitSequence(PlatformObstacleSequence sequence)
            {
                var moves = sequence.ObstacleMoves;
                if (moves != null && moves.Count > 0)
                {
                    for (int i = 0; i < moves.Count; i++)
                    {
                        var obs = moves[i];
                        if (obs == null) { continue; }
                        obs.InitTask(this);
                    }
                }
            }
        }

        protected override IEnumerator OnStartAsync()
        {
            yield return StartCoroutine(base.OnStartAsync());
            if (sequencesForObstacle != null && sequencesForObstacle.Count > 0)
            {
                foreach (var seq in sequencesForObstacle)
                {
                    StartCoroutine(RunSequence(seq));
                }
            }
            IEnumerator RunSequence(PlatformObstacleSequence sequence)
            {
                var moves = sequence.ObstacleMoves;
                if (moves != null && moves.Count > 0)
                {
                    while (true)
                    {
                        if (sequence.Active)
                        {
                            for (int i = 0; i < moves.Count; i++)
                            {
                                var obs = moves[i];
                                if (obs == null || obs.Active == false || sequence.Active == false) { continue; }
                                yield return StartCoroutine(obs.ManipulateObstacle(this));
                            }

                            if (sequence.IsLooped == false)
                            {
                                break;
                            }
                        }

                        yield return null;
                    }
                }
            }
        }

        protected override void UpdateActor(float dt, float fixedDt)
        {
            if (defaultRotationMode == ObstacleRotateMode.Rotate)
            {
                _Transform.Rotate(rotateSpeed * dt, Space.Self);
            }
            else if (defaultRotationMode == ObstacleRotateMode.RotateAround)
            {
                var center = hasTransformCenter ? centerForRotateAround.position : centerVec;
                _Transform.RotateAround(center, rotateAroundAxis, rotateAroundAngle * dt);
            }
        }

        protected override IEnumerator OnStartHitActorAsync(ReactorActor rival, FPhysicsShape rivalShape, FPhysicsShape ownShape)
        {
            yield return StartCoroutine(base.OnStartHitActorAsync(rival, rivalShape, ownShape));
            if (useThisColliderForDamage) { DamageActor(rival); }
        }

        protected override IEnumerator OnEnterVolumeAsync(ReactorActor rival, FPhysicsShape rivalShape, FPhysicsShape ownShape)
        {
            Debug.Log("yap collider. this actor: " + gameObject.name + " rival: " + rival.gameObject.name);
            yield return StartCoroutine(base.OnEnterVolumeAsync(rival, rivalShape, ownShape));
            if (useThisVolumeForDamage) { DamageActor(rival); }
        }

        void DamageActor(Actor rival)
        {
            if (isLethal)
            {
                rival.Murder();
            }
            else
            {
                float dm = obstacleDamageMode == DamageMode.Relative ? damageAmount * rival.FullLife : damageAmount;
                rival.Damage(dm);
            }
        }

        protected override bool SetInteractionValidity(ReactorActor rival, FPhysicsShape rivalShape, FPhysicsShape ownShape)
        {
            return rival.IsPlayer;
        }

        protected override void OnEnterVolume(ReactorActor rival, FPhysicsShape rivalShape, FPhysicsShape ownShape)
        {
            base.OnEnterVolume(rival, rivalShape, ownShape);
            CallEventFunc(InteractionEventPhysics.EnterVolume, rival);
        }

        protected override void OnExitVolume(ReactorActor rival, FPhysicsShape rivalShape, FPhysicsShape ownShape)
        {
            base.OnExitVolume(rival, rivalShape, ownShape);
            CallEventFunc(InteractionEventPhysics.ExitVolume, rival);
        }

        protected override void OnStartHitActor(ReactorActor rival, FPhysicsShape rivalShape, FPhysicsShape ownShape)
        {
            base.OnStartHitActor(rival, rivalShape, ownShape);
            CallEventFunc(InteractionEventPhysics.HitEvent, rival);
        }

        protected override void OnStopHitActor(ReactorActor rival, FPhysicsShape rivalShape, FPhysicsShape ownShape)
        {
            base.OnStopHitActor(rival, rivalShape, ownShape);
            CallEventFunc(InteractionEventPhysics.StopHitEvent, rival);
        }

        void CallEventFunc(InteractionEventPhysics mode, Actor rival)
        {
            if (sequencesForObstacle != null && sequencesForObstacle.Count > 0)
            {
                for (int i = 0; i < sequencesForObstacle.Count; i++)
                {
                    var seq = sequencesForObstacle[i];
                    if (seq == null) { continue; }
                    InvokeEventFunc(seq);
                }
            }

            void InvokeEventFunc(PlatformObstacleSequence sequence)
            {
                var moves = sequence.ObstacleMoves;
                if (moves != null && moves.Count > 0)
                {
                    for (int i = 0; i < moves.Count; i++)
                    {
                        var obs = moves[i];
                        if (obs == null) { continue; }
                        if (mode == InteractionEventPhysics.EnterVolume)
                        {
                            obs.OnEnterVolume(rival);
                        }
                        else if (mode == InteractionEventPhysics.ExitVolume)
                        {
                            obs.OnExitVolume(rival);
                        }
                        else if (mode == InteractionEventPhysics.HitEvent)
                        {
                            obs.OnHit(rival);
                        }
                        else if (mode == InteractionEventPhysics.StopHitEvent)
                        {
                            obs.OnStopHit(rival);
                        }
                    }
                }
            }
        }
    }
}