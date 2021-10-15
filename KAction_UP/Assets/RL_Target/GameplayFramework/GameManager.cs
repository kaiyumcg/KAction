using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Author: Md. Al Kaiyum(Rumman)
/// Email: kaiyumce06rumman@gmail.com
/// Game Manager to controll the game systems
/// </summary>
namespace GameplayFramework
{
    public abstract class GameManager : MonoBehaviour
    {
        [SerializeField] List<GameSystem> allSystems = new List<GameSystem>();
        [SerializeField] bool asyncWaitForInit = false;
        protected virtual void OnGameStart() { }
        protected abstract bool WhenGameStarts();
        protected abstract bool WhenGameEnds();
        bool gameStarted = false, gameEnded = false;
        public bool HasGameBeenStarted { get { return gameStarted; } }
        public bool HasGameBeenEnded { get { return gameEnded; } }
        protected virtual void AwakeGameManager() { }
        protected virtual void InitAllGameSystems() { }
        protected virtual void UpdateGameManager() { }
        public UnityEvent OnAwakeGameManager, OnInitAllGameSystems, OnStartGameplay, OnEndGame;

        void ReloadSysData()
        {
            if (allSystems == null) { allSystems = new List<GameSystem>(); }
            var allsys = FindObjectsOfType<GameSystem>();
            if (allsys != null && allsys.Length > 0)
            {
                for (int i = 0; i < allsys.Length; i++)
                {
                    var sys = allsys[i];
                    if (sys == null) { continue; }
                    if (allSystems.Contains(sys) == false)
                    {
                        allSystems.Add(sys);
                    }
                }
            }

            allSystems.RemoveAll((sys) => { return sys == null; });
            if (allSystems == null) { allSystems = new List<GameSystem>(); }
        }

        private void OnValidate()
        {
            ReloadSysData();
        }

        public T GetManager<T>() where T : GameSystem
        {
            T result = null;
            if (allSystems != null && allSystems.Count > 0)
            {
                for (int i = 0; i < allSystems.Count; i++)
                {
                    var sys = allSystems[i];
                    if (sys == null) { continue; }
                    if (sys.GetType() == typeof(T))
                    {
                        result = (T)sys;
                        break;
                    }
                }
            }

            return result;
        }

        // Start is called before the first frame update
        IEnumerator Start()
        {
            ReloadSysData();
            AwakeGameManager();
            OnAwakeGameManager?.Invoke();
            if (allSystems.Count > 0)
            {
                for (int i = 0; i < allSystems.Count; i++)
                {
                    var sys = allSystems[i];
                    if (sys == null)
                    {
                        throw new System.Exception("By architectural design we do not allow any null system(per frame UnityEngine.Object ==, != check) " +
                            "in the list for performance implication. Please assign valid system(s) in the list!");
                    }
                    sys.SetGameManager(this);
                    sys.InitSystem();

                    if (asyncWaitForInit)
                    {
                        yield return StartCoroutine(sys.InitSystemAsync());
                    }
                    else
                    {
                        StartCoroutine(sys.InitSystemAsync());
                    }
                }
            }

            InitAllGameSystems();
            OnInitAllGameSystems?.Invoke();

            while (true)
            {
                if (WhenGameStarts())
                {
                    break;
                }

                yield return null;
            }

            OnGameStart();
            OnStartGameplay?.Invoke();
            gameStarted = true;

            while (true)
            {
                if (WhenGameEnds())
                {
                    break;
                }
                yield return null;
            }
            OnEndGame?.Invoke();
            gameEnded = true;
        }

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < allSystems.Count; i++)
            {
                var sys = allSystems[i];
                sys.UpdateSystem();
            }
            UpdateGameManager();
        }
    }
}