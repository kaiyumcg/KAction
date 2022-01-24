using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public class FCollider2D : MonoBehaviour
    {
        ReactorActor2D r_actor;
        Actor owningActor;
        private void Awake()
        {
            r_actor = GetComponentInParent<ReactorActor2D>();
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

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (IsValid(other.gameObject) == false) { return; }
            r_actor.ProcessVolume2D(other, true);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (IsValid(other.gameObject) == false) { return; }
            r_actor.ProcessVolume2D(other, false);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (IsValid(collision.gameObject) == false) { return; }
            r_actor.ProcessSolid2D(collision, true);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (IsValid(collision.gameObject) == false) { return; }
            r_actor.ProcessSolid2D(collision, false);
        }
    }
}