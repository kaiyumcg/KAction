using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public class FCollider2DStart : FPhysicsShape
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            this.OnCollisionStart2D(collision);
        }
    }
}