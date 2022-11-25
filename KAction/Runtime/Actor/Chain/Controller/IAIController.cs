using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Author: Md. Al Kaiyum(Rumman)
/// Email: kaiyumce06rumman@gmail.com
/// AI Controller Interface.
/// </summary>
namespace GameplayFramework
{
    public interface IAIController
    {
        Rigidbody AIRigidbody { get; }
        Rigidbody2D AIRigidbody2D { get; }
        Transform AITransform { get; }
        Animator AIAnimator { get; }
        NavMeshAgent AIAgent { get; }

        void OnStartController(AIActor ai);
        void OnEndController(AIActor ai);
        void ControlInUpdate(float dt, float fixedDt);
        void ControlInPhysicsUpdate(float dt, float fixedDt);
    }
}