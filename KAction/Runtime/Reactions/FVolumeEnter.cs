using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public class FVolumeEnter : FPhysicsShape
    {
        private void OnTriggerEnter(Collider other)
        {
            this.OnVolumeEnter(other);
        }
    }
}