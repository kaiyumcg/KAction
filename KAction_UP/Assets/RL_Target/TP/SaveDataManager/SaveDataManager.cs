using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Md. Al Kaiyum(Rumman)
/// Email: kaiyumce06rumman@gmail.com
/// A small library designed to save small game data into disk via unity's player pref class or custom way.
/// Custom class saving/loading has not been implemented yet.
/// Using cloud technilogy, we can also save data into cloud, though it is not implemented yet. Just planned.
/// </summary>
namespace KSaveDataMan
{
    public static class SaveDataManager
    {
        public static void SaveInt(string identifier, int defaultValue = -1)
        {
            PlayerPrefs.SetInt(identifier, defaultValue);
        }

        public static int LoadInt(string identifier, int defaultIfNotExist = -1)
        {
            return PlayerPrefs.GetInt(identifier, defaultIfNotExist);
        }

        public static void SaveFloat(string identifier, float defaultValue = -1)
        {
            PlayerPrefs.SetFloat(identifier, defaultValue);
        }

        public static float LoadFloat(string identifier, float defaultIfNotExist = -1)
        {
            return PlayerPrefs.GetFloat(identifier, defaultIfNotExist);
        }

        public static void SaveString(string identifier, string defaultValue = "")
        {
            PlayerPrefs.SetString(identifier, defaultValue);
        }

        public static string LoadString(string identifier, string defaultIfNotExist = "")
        {
            return PlayerPrefs.GetString(identifier, defaultIfNotExist);
        }

        public static void SaveClassLocal<T>(string identifier, T defaultValue = null) where T : class
        {
            //custom data saving scheme
            //In application dir or persistent data dir according to runtime platform
            //adopt encryption/decryption if necessary
            throw new System.NotImplementedException();
        }

        public static T LoadClassLocal<T>(string identifier, T defaultIfNotExist = null) where T : class
        {
            //custom data loading scheme
            //In application dir or persistent data dir according to runtime platform
            //adopt encryption/decryption if necessary
            throw new System.NotImplementedException();
        }

        public static void SaveClassOnline<T>(string credentialAndIdentifier, System.Action OnComplete, T defaultValue = null) where T : class
        {
            //custom data saving scheme
            //In cloud using possibly object storage(IBM?)
            //use www class or similar unity API to communicate
            //adopt encryption/decryption if necessary
            throw new System.NotImplementedException();
        }

        public static void LoadClassOnline<T>(string credentialAndIdentifier, System.Action<T> OnComplete, T defaultIfNotExist = null) where T : class
        {
            //custom data loading scheme
            //In cloud using possibly object storage(IBM?)
            //use www class or similar unity API to communicate
            //adopt encryption/decryption if necessary
            throw new System.NotImplementedException();
        }
    }
}