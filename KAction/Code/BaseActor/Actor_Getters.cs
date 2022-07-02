using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public abstract partial class Actor : MonoBehaviour
    {
        public T GetGameplayComponent<T>() where T : GameplayComponent
        {
            throw new System.NotImplementedException();
        }

        public List<T> GetGameplayComponents<T>() where T : GameplayComponent
        {
            throw new System.NotImplementedException();
        }

        public void AddGameplayComponent<T>() where T : GameplayComponent
        {
            componentListDirty = true;
            throw new System.NotImplementedException();
            componentListDirty = false;
        }

        public void AddGameplayComponent<T>(GameObject objOnWhichToAdd) where T : GameplayComponent
        {
            componentListDirty = true;
            throw new System.NotImplementedException();
            componentListDirty = false;
        }
    }
}