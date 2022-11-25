using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public class FVolume2DExit : FPhysicsShape
    {
        private void OnTriggerExit2D(Collider2D collision)
        {
            this.OnVolumeExit2D(collision);
        }
    }
}