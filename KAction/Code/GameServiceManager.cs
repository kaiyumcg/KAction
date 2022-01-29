using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

namespace GameplayFramework
{
    public class GameServiceManager : MonoBehaviour
    {
        [Header("Execution order as well :)")]
        [SerializeField] internal List<GameService> services;
        [SerializeField] bool startAllParallel = true;
        static GameServiceManager instance;

        bool HasTheServiceOrTypeOfIt(GameService svc)
        {
            bool hasIt = false;
            if (services != null && services.Count > 0)
            {
                for(int i = 0; i < services.Count;i++)
                {
                    var s = services[i];
                    if (s == svc || s.GetType() == svc.GetType())
                    {
                        hasIt = true;
                        break;
                    }
                }
            }
            return hasIt;
        }

        internal void LoadRefIfReq()
        {
            if (services == null) { services = new List<GameService>(); }
            services.RemoveAll((svc) => { return svc == null; });
            if (services == null) { services = new List<GameService>(); }

            services.RemoveAll((svc) =>
            {
                bool matchForRemove = false;
                int mCount = 0;
                for (int i = 0; i < services.Count; i++)
                {
                    if (services[i] == svc || services[i].GetType() == svc.GetType())
                    {
                        mCount++;
                    }
                }
                if (mCount > 1) { matchForRemove = true; }
                return matchForRemove;
            });

            var allSvc = FindObjectsOfType<GameService>();
            if (allSvc != null && allSvc.Length > 0)
            {
                for (int i = 0; i < allSvc.Length; i++)
                {
                    var svc = allSvc[i];
                    if (svc == null) { continue; }
                    if (HasTheServiceOrTypeOfIt(svc) == false)
                    {
                        services.Add(svc);
                    }
                }
            }
        }

        private void Awake()
        {
            var curSceneName = SceneManager.GetActiveScene().name;
            if (curSceneName != "boot")
            {
                Debug.LogError("Game Service Manager should ideally exist on 'boot' level. " +
                    "Tthis is considered a fatal error by design." +
                    "You might have misused the framework to cause further problem(s) down the road. " +
                    "Use 'Tools/Level Creation' window to properly create boot level and other game levels.");
                return;
            }

            if (instance == null)
            {
                instance = this;
                LoadRefIfReq();
                if(services != null && services.Count > 0)
                {
                    for (int i = 0; i < services.Count; i++)
                    {
                        var svc = services[i];
                        if (svc == null) { continue; }
                        DontDestroyOnLoad(svc);
                    }
                }
                DontDestroyOnLoad(instance);
            }
            else
            {
                Debug.LogError("Multiple game service managers are found! " +
                    "This means you either loaded boot scene manually by yourself" +
                    " Or you misused the framework. " +
                    "Use 'Tools/Level Creation' window to properly create boot level and other game levels.");

                DestroyImmediate(instance);
                return;
            }
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "boot") { return; }
            var allSvc = FindObjectsOfType<GameService>();
            if (services == null) { services = new List<GameService>(); }
            for (int i = 0; i < allSvc.Length; i++)
            {
                var svc = allSvc[i];
                if (svc == null) { continue; }
                if (HasTheServiceOrTypeOfIt(svc) == false)
                {
                    Debug.LogWarning("You are trying to add a game service in normal Game Level. " +
                        "This is allowed but it is not a recommended practice. " +
                        "You should consider placeing this game service '" + svc.GetType().Name + "' in boot scene instead." +
                        " To Create a boot scene if you have not already, Please use 'Tools/Level Creation' window." +
                        " Additionally remember, we do not allow multiple instance of a Game Service type by design.");

                    DontDestroyOnLoad(svc);
                    svc.IsRunning = true;
                    svc.OnInit();
                    StartCoroutine(svc.OnInitAsync());
                    isListDirty = true;
                    services.Add(svc);
                }
                else
                {
                    Destroy(svc);
                }
            }
            isListDirty = false;
        }

        private IEnumerator Start()
        {
            isListDirty = false;
            if (services != null && services.Count > 0)
            {
                for (int i = 0; i < services.Count; i++)
                {
                    var svc = services[i];
                    if (svc == null) { continue; }
                    svc.IsRunning = true;
                    svc.OnInit();

                    if (startAllParallel || svc.WaitForThisInitialization == false)
                    {
                        StartCoroutine(svc.OnInitAsync());
                    }
                    else
                    {
                        yield return StartCoroutine(svc.OnInitAsync());
                    }   
                }
            }
        }

        public static T GetService<T>() where T : GameService
        {
            T result = null;
            if (instance != null && instance.services != null && instance.services.Count != 0) 
            {
                var services = instance.services;
                for (int i = 0; i < services.Count; i++)
                {
                    var svc = services[i];
                    if (svc == null) { continue; }
                    if (svc.GetType() == typeof(T))
                    {
                        result = (T)svc;
                        break;
                    }
                }
            }
            return result;
        }

        bool isListDirty = false;
        private void Update()
        {
            if (isListDirty) { return; }
            if (services != null && services.Count > 0)
            {
                for (int i = 0; i < services.Count; i++)
                {
                    var svc = services[i];
                    if (svc.IsRunning == false) { continue; }
                    svc.OnTick();
                }
            }
        }
    }
}