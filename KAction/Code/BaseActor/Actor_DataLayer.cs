using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public abstract partial class Actor : MonoBehaviour
    {
        internal Dictionary<string, ActorData> dataTable = null;
        public event OnDoActorDataFunc OnChangeActorData;

        public T GetData<T>(string key) where T : ActorData
        {
            throw new System.NotImplementedException();
        }

        public void SetData<T>(string key, T data) where T : ActorData
        {
            throw new System.NotImplementedException();
            //invoke 'OnChangeActorData'
        }

        //fetch data, convert to json, write with saveData Manager
        public void WriteDataToDisk<T>(string key, string devUniqueName) where T : ActorData
        {
            throw new System.NotImplementedException();
        }

        public void LoadDataFromDisk<T>(string key, string devUniqueName) where T : ActorData
        {
            throw new System.NotImplementedException();
        }

        public void ClearDataTable()
        {
            throw new System.NotImplementedException();
        }
    }
}