using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Events;

namespace GameplayFramework
{
    public sealed class GameServiceManager : MonoBehaviour
    {
        [Header("Execution order as well :)")]
        [SerializeField] List<GameService> services;
#if UNITY_EDITOR
        public void SetEd_services(List<GameService> services) { this.services = services; }
#endif

        [SerializeField] bool startAllParallel = true;
        static GameServiceManager instance;
        public UnityEvent onReadyServiceManager;
        public UnityEvent<int, int, GameService> onInitializedAGameService;
        public static UnityEvent OnReadyServiceManager { get { return instance.onReadyServiceManager; } }
        public static UnityEvent<int, int, GameService> OnInitializedAGameService { get { return instance.onInitializedAGameService; } }
        void InitData()
        {
            if (onReadyServiceManager == null) { onReadyServiceManager = new UnityEvent(); }
            if (onInitializedAGameService == null) { onInitializedAGameService = new UnityEvent<int, int, GameService>(); }
        }

        private void Awake()
        {
            var curSceneName = SceneManager.GetActiveScene().name;
            if (curSceneName != "boot")
            {
                Debug.LogError("Game Service Manager should ideally exist on 'boot' level. " +
                    "This is considered a fatal error by design." +
                    "You might have misused the framework to cause further problem(s) down the road. " +
                    "Use 'Tools/Level Creation' window to properly create boot level and other game levels.");
                return;
            }

            if (instance == null)
            {
                instance = this;
                InitData();
                if(services != null && services.Count > 0)
                {
                    for (int i = 0; i < services.Count; i++)
                    {
                        var svc = services[i];
                        if (svc == null) { continue; }
                        if (svc.transform.parent != null)
                        {
                            svc.transform.SetParent(null, true);
                        }
                        DontDestroyOnLoad(svc.gameObject);
                    }
                }
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Debug.LogError("Multiple game service managers are found! " +
                    "This means you either loaded boot scene manually by yourself" +
                    " Or you misused the framework. ");
                DestroyImmediate(gameObject);
                return;
            }
        }

        private IEnumerator Start()
        {
            isListDirty = false;
            if (services != null && services.Count > 0)
            {
                for (int i = 0; i < services.Count; i++)
                {
                    var svc = services[i];
                    svc.IsRunning = true;
                    svc.OnInit();

                    if (!startAllParallel && svc.WaitForThisInitialization)
                    {
                        yield return StartCoroutine(svc.OnInitAsync());
                    }
                    else
                    {
                        StartCoroutine(svc.OnInitAsync());
                    }
                    onInitializedAGameService?.Invoke(i + 1, services.Count, svc);
                }
            }
            onReadyServiceManager?.Invoke();
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

        internal static void DoLogAction(ErrorType errorType)
        {
            if (instance != null && instance.services != null)
            {
                var services = instance.services;
                var len = services.Count;
                if (len > 0 && instance.isListDirty == false)
                {
                    for (int i = 0; i < services.Count; i++)
                    {
                        var svc = services[i];
                        if (svc == null) { continue; }
                        svc.DoLogAction(errorType);
                    }
                }
            }
        }

        public static T AddService<T>() where T : GameService
        {
            T spawnedService = null;
            instance.isListDirty = true;
            instance.services = instance.services.RemoveAllIfLogicTrue((svc) => { return svc.GetType() == typeof(T); });
            var g = new GameObject("_spawned_" + typeof(T));
            spawnedService = g.AddComponent<T>();
            DontDestroyOnLoad(spawnedService.gameObject);
            instance.services.Add(spawnedService);
            instance.isListDirty = false;
            return spawnedService;
        }

        public static void RemoveService<T>() where T : GameService
        {
            RemoveService(typeof(T));
        }

        public static void RemoveService(Type serviceType)
        {
            instance.isListDirty = true;
            GameService toRemoveSvc = null;
            for (int i = 0; i < instance.services.Count; i++)
            {
                var svc = instance.services[i];
                if (svc == null) { continue; }
                if(svc.GetType() == serviceType)
                {
                    toRemoveSvc = svc;
                    break;
                }
            }
            instance.services.Remove(toRemoveSvc);
            instance.services = instance.services.RemoveAllNulls();
            instance.isListDirty = false;
        }

        bool isListDirty = false;
        private void Update()
        {
            if (isListDirty) { return; }
            for (int i = 0; i < services.Count; i++)
            {
                var svc = services[i];
                if (svc.IsRunning == false) { continue; }
                svc.OnTick();
            }
        }
    }
}