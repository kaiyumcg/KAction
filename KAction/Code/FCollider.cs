using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public class FCollider : MonoBehaviour
    {
        ReactorActor r_actor;
        Actor owningActor;
        private void Awake()
        {
            r_actor = GetComponentInParent<ReactorActor>();
            owningActor = r_actor;
        }

        bool IsValid(GameObject other)
        {
            var valid = true;
            if (r_actor == null) { valid = false; }
            var ra = other.GetComponentInParent<Actor>();
            if (ra == null) { valid = false; }
            if (ra != null && ra == owningActor) { valid = false; }
            return valid;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (IsValid(other.gameObject) == false) { return; }
            r_actor.ProcessVolume(other, true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (IsValid(other.gameObject) == false) { return; }
            r_actor.ProcessVolume(other, false);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (IsValid(collision.gameObject) == false) { return; }
            r_actor.ProcessSolid(collision, true);
        }

        private void OnCollisionExit(Collision collision)
        {
            if (IsValid(collision.gameObject) == false) { return; }
            r_actor.ProcessSolid(collision, false);
        }
    }
}