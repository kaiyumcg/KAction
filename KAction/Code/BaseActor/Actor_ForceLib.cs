using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public abstract partial class Actor : MonoBehaviour
    {
        internal bool isJumping = false, isHelixing = false, isTurning = false, isMoving = false,
            isThursting = false, isSwirling = false, isPoping = false, isSpringReleasing = false,
            isSummoning = false, isShaking = false, isStillHanging = false, isSwinging = false,
            isBrownianDoing = false, isRubberBandFollowing = false;
    }
}