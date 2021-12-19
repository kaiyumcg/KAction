using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework.Platforming
{
    public interface IPlatformObstacleTask
    {
        bool Active { get; set; }
        IEnumerator ManipulateObstacle(Obstacle obstacle);

        void InitTask(Obstacle obstacle);

        void OnHit(Actor rival);
        void OnStopHit(Actor rival);

        void OnEnterVolume(Actor rival);
        void OnExitVolume(Actor rival);
    }
}