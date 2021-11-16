using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace GameplayFramework
{
    [System.Serializable]
    public class ParticleSpawnDesc
    {
        [SerializeField] Transform particle = null, holder = null;
        [SerializeField]
        Vector3 positionOffset = Vector3.zero,
            rotationOffset = Vector3.zero, scaleOffset = Vector3.zero;

        public void Spawn(Transform where)
        {
            var vfx = Object.Instantiate(particle);
            if (holder != null)
            {
                vfx.SetParent(holder, false);
                vfx.transform.localPosition = positionOffset;
                vfx.transform.localRotation = Quaternion.Euler(rotationOffset);
                vfx.transform.localScale = scaleOffset;
            }
            else
            {
                vfx.transform.position = where.position + positionOffset;
                vfx.transform.rotation = Quaternion.Euler(rotationOffset);
                vfx.transform.localScale = scaleOffset;
            }
        }
    }

    public enum InteractionMode { OneTime = 0, MultipleTime = 1, Infinite = 3 }
}