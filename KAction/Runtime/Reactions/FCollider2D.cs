using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public sealed class FCollider2D : FPhysicsShape
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            this.OnCollisionStart2D(collision);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            this.OnCollisionStop2D(collision);
        }
    }
}