using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace GameplayFramework
{
    internal class CloudLogUploadManager : MonoBehaviour
    {
        internal static void StartUploadLogs(string logStr, string API_End_Point)
        {
            if (string.IsNullOrEmpty(logStr) || string.IsNullOrWhiteSpace(logStr)) { return; }

            var uploaderGObj = new GameObject("_LogUploader_");
            var uploader = uploaderGObj.AddComponent<CloudLogUploadManager>();
            DontDestroyOnLoad(uploader.gameObject);
            uploader.StartCoroutine(Upload());
            IEnumerator Upload()
            {
                byte[] logBytes = System.Text.Encoding.UTF8.GetBytes(logStr);
                UnityWebRequest www = UnityWebRequest.Put(API_End_Point, logBytes);
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Could not upload logs into cloud. Error: " + www.error);
                }
                else
                {
                    DestroyImmediate(uploader.gameObject);
                }
            }
        }
    }
}