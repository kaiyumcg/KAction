using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public class FVolume : FPhysicsShape
    {
        private void OnTriggerEnter(Collider other)
        {
            this.OnVolumeEnter(other);
        }

        private void OnTriggerExit(Collider other)
        {
            this.OnVolumeExit(other);
        }
    }
}