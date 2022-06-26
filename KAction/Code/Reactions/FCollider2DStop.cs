using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public class FCollider2DStop : FPhysicsShape
    {
        private void OnCollisionExit2D(Collision2D collision)
        {
            this.OnCollisionStop2D(collision);
        }
    }
}