using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public class FVolumeExit : FPhysicsShape
    {
        private void OnTriggerExit(Collider other)
        {
            this.OnVolumeExit(other);
        }
    }
}