using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public class FVolume2D : FPhysicsShape
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            this.OnVolumeEnter2D(collision);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            this.OnVolumeExit2D(collision);
        }
    }
}