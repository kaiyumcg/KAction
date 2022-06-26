using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public class FColliderStop : FPhysicsShape
    {
        private void OnCollisionExit(Collision collision)
        {
            this.OnCollisionStop(collision);
        }
    }
}