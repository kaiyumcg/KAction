using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public sealed class FCollider : FPhysicsShape
    {
        private void OnCollisionEnter(Collision collision)
        {
            this.OnCollisionStart(collision);
        }

        private void OnCollisionExit(Collision collision)
        {
            this.OnCollisionStop(collision);
        }
    }
}