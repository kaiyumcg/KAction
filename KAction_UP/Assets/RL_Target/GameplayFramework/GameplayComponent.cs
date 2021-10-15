using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Md. Al Kaiyum(Rumman)
/// Email: kaiyumce06rumman@gmail.com
/// Gameplay Component Interface.
/// </summary>
namespace GameplayFramework
{
    public abstract class GameplayComponent : MonoBehaviour
    {
        protected internal virtual void OnStartOrSpawnActor() { }
        protected internal virtual void AwakeComponent() { }
        protected internal virtual void UpdateComponent(float dt, float fixedDt) { }
        protected internal virtual void UpdateComponentPhysics(float dt, float fixedDt) { }
        protected virtual void OnEditorUpdate() { }
        protected internal virtual void OnCleanupComponent() { }

        internal void SetOwner(GameActor actor)
        {
            owner = actor;
        }

        [SerializeField] [HideInInspector] GameActor owner;
        public GameActor Owner { get { return owner; } }

        private void OnValidate()
        {
            OnEditorUpdate();
        }
    }
}