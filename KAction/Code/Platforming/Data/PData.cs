using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using DG.Tweening;

namespace GameplayFramework.Platforming
{
    public enum DamageMode { AbsoluteAmount, Relative }
    public enum ObstacleRotateMode { RotateAround = 0, Rotate = 1, NoRotation = 3 }
    internal enum InteractionEventPhysics { EnterVolume = 0, ExitVolume = 1, HitEvent = 2, StopHitEvent = 3 }

    [System.Serializable]
    public class PlatformObstacleSequence
    {
        [SerializeField] bool active = true;
        public bool Active { get { return active; } }
        [SerializeReference] [SerializeReferenceButton] List<IPlatformObstacleTask> obstacleMoves;
        public List<IPlatformObstacleTask> ObstacleMoves { get { return obstacleMoves; } }
        [SerializeField] bool loop = true;
        public bool IsLooped { get { return loop; } }
    }

    [System.Serializable]
    public class DelayForObstacle
    {
        [SerializeField] float amount;
        [SerializeField] bool use;
        public float Amount { get { return amount; } }
        public bool Use { get { return use; } }
    }

    public enum TransformTaskObstacleMode { Translate = 0, Rotation = 1, Scale = 2 }

    [System.Serializable]
    public class TranformTask_Obstacle : IPlatformObstacleTask
    {
        [SerializeField] bool isActive = true;
        [SerializeField] DelayForObstacle startDelay;
        [SerializeField] TransformTaskObstacleMode mode;
        [SerializeField] Transform targetTransform;
        [SerializeField] Vector3 targetOffset;
        [SerializeField] float withinTime = 1.2f;
        [SerializeField] DelayForObstacle endDelay;

        Vector3 initPos, initRot, initScale;

        bool IPlatformObstacleTask.Active { get => isActive; set => isActive = value; }

        void IPlatformObstacleTask.InitTask(Obstacle obstacle)
        {
            initPos = obstacle._Transform.position;
            initRot = obstacle._Transform.eulerAngles;
            initScale = obstacle._Transform.localScale;
        }

        IEnumerator IPlatformObstacleTask.ManipulateObstacle(Obstacle obstacle)
        {
            if (startDelay != null && startDelay.Use)
            {
                yield return new WaitForSeconds(startDelay.Amount);
            }

            var completed = false;

            if (targetTransform != null)
            {
                if (mode == TransformTaskObstacleMode.Translate)
                {
                    //obstacle._Transform.DOMove(targetTransform.position, withinTime).OnComplete(() =>
                    //{
                    //    completed = true;
                    //});
                }
                else if (mode == TransformTaskObstacleMode.Rotation)
                {
                    //obstacle._Transform.DORotate(targetTransform.eulerAngles, withinTime).OnComplete(() =>
                    //{
                    //    completed = true;
                    //});
                }
                else
                {
                    //obstacle._Transform.DOScale(targetTransform.localScale, withinTime).OnComplete(() =>
                    //{
                    //    completed = true;
                    //});
                }
            }
            else
            {
                if (mode == TransformTaskObstacleMode.Translate)
                {
                    var target = initPos + targetOffset;
                    //obstacle._Transform.DOMove(target, withinTime).OnComplete(() =>
                    //{
                    //    completed = true;
                    //});
                }
                else if (mode == TransformTaskObstacleMode.Rotation)
                {
                    var target = initRot + targetOffset;
                    //obstacle._Transform.DORotate(target, withinTime).OnComplete(() =>
                    //{
                    //    completed = true;
                    //});
                }
                else
                {
                    var target = initScale + targetOffset;
                    //obstacle._Transform.DOScale(target, withinTime).OnComplete(() =>
                    //{
                    //    completed = true;
                    //});
                }

            }


            while (completed == false) { yield return null; }

            if (endDelay != null && endDelay.Use)
            {
                yield return new WaitForSeconds(endDelay.Amount);
            }
        }

        void IPlatformObstacleTask.OnEnterVolume(Actor actor)
        {

        }

        void IPlatformObstacleTask.OnExitVolume(Actor actor)
        {

        }

        void IPlatformObstacleTask.OnHit(Actor actor)
        {

        }

        void IPlatformObstacleTask.OnStopHit(Actor actor)
        {

        }
    }
}