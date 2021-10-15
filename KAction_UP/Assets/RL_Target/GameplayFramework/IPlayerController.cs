using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Md. Al Kaiyum(Rumman)
/// Email: kaiyumce06rumman@gmail.com
/// Player Controller Interface.
/// </summary>
namespace GameplayFramework
{
    public interface IPlayerController
    {
        Rigidbody PlayerRigidbody { get; }
        Rigidbody2D PlayerRigidbody2D { get; }
        Transform PlayerTransform { get; }
        Animator PlayerAnimator { get; }
        void OnStartController(PlayerActor player);
        void OnEndController(PlayerActor player);

        void ControlInUpdate(float dt, float fixedDt);
        void ControlInPhysicsUpdate(float dt, float fixedDt);
    }
}