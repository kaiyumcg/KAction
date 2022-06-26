using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public class FColliderStart : FPhysicsShape
    {
        private void OnCollisionEnter(Collision collision)
        {
            this.OnCollisionStart(collision);
        }
    }
}