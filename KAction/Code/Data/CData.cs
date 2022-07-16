using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public enum ErrorType { Exception = 0, Error = 1, CodeFailure = 2 }
    public enum ActionOnLog { DoNothing = 0, Stop = 1, Restart = 2 }
    [Serializable]
    public class ActorDataSet : FOrderedDictionary<string, ActorData> { }

    [System.Serializable]
    public abstract class ActorData
    {

        public virtual string ConvertToJson() { return null; }
    }

    public enum ActorForceEase
    {
        EaseIn = 0,
        EaseOut = 1,
        Cubic = 2,
        Linear = 3
    }

    public enum ActorType
    {
        Player = 0,
        NPC = 1, 
        EnvironmentalStatic = 2, 
        EnvironmentalDynamic = 3, 
        Other = 4
    }

    [System.Serializable]
    public class ParticleSpawnDesc
    {
        [SerializeField] Transform particle = null, holder = null;
        [SerializeField]
        Vector3 positionOffset = Vector3.zero,
            rotationOffset = Vector3.zero, scaleOffset = Vector3.zero;

        public void Spawn(Transform where)
        {
            var vfx = UnityEngine.Object.Instantiate(particle);
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

    public struct TimeReverseCapsule
    {

    }
}