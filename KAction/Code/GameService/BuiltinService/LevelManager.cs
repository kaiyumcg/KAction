using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace GameplayFramework
{
    public sealed class LevelManager : GameService
    {
        [SerializeField] int levelStartNumAfterAllDone = 2;
        [SerializeField] bool loadRandomlyAfterDone = false;

        bool allInitDone = false;
        int curLevelNum = -1, nextLevelNum = -1, curRealLevelNum = -1, nextRealLevelNum = -1;
        public int CurrentLevelNumber { get { return curLevelNum; } }
        public int CurrentLevelNumberInBuildSetting { get { return curRealLevelNum; } }
        public int NextLevelNumber { get { return nextLevelNum; } }
        public int NextLevelNumberInBuildSetting { get { return nextRealLevelNum; } }
        public UnityEvent<int> OnStartUnityLevel, OnReloadUnityLevel, OnCompleteUnityLevel;

        protected internal override void OnInit()
        {
            base.OnInit();
            allInitDone = false;
            var firstScene = SceneManager.GetSceneByBuildIndex(0);
            var hasBootScene = firstScene.name == "boot";
            if (hasBootScene == false)
            {
                KLog.ThrowGameplaySDKException(GFType.GameService, 
                    "This game does not have any boot. Can not proceed further. Aborting....You will stuck in this blank scene. " +
                    "You must create a boot scene from 'Tools/Level Creation' window and add it to build setting window");
                return;
            }

            var curScene = SceneManager.GetActiveScene();
            if (curScene.name != "boot")
            {
                KLog.ThrowGameplaySDKException(GFType.GameService, 
                    "This service can only be used when started from boot scene. Misused Game service error!");
                return;
            }

            curLevelNum = PlayerPrefs.GetInt("curLevelNum", 1);
            var defaultRealLevelNum = 2;
            curRealLevelNum = PlayerPrefs.GetInt("curRealLevelNum", defaultRealLevelNum);
            nextLevelNum = PlayerPrefs.GetInt("nextLevelNum", 2);
            nextRealLevelNum = PlayerPrefs.GetInt("nextRealLevelNum", defaultRealLevelNum + 1);

            var sceneID = curRealLevelNum - 1;
            if (sceneID < 1)
            {
                sceneID = 1;
            }

            curRealLevelNum = sceneID + 1;
            nextRealLevelNum = curRealLevelNum + 1;
            CheckLevelOverFlow(curScene.buildIndex);
            PlayerPrefs.SetInt("curRealLevelNum", curRealLevelNum);
            GameServiceManager.OnReadyServiceManager.AddListener(() =>
            {
                allInitDone = true;
            });
            StartCoroutine(CheckForAllInit(sceneID));
        }

        IEnumerator CheckForAllInit(int sceneID)
        {
            while (allInitDone == false)
            {
                yield return null;
            }
            OnStartUnityLevel?.Invoke(curLevelNum);
            SceneManager.LoadScene(sceneID);
        }

        void CheckLevelOverFlow(int currentLevelIndex)
        {
            if (nextRealLevelNum > SceneManager.sceneCountInBuildSettings)
            {
                List<int> randLvNums = new List<int>();
                for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
                {
                    if (i == 0 || i == currentLevelIndex) { continue; }
                    randLvNums.Add(i);
                }
                var randID = UnityEngine.Random.Range(0, randLvNums.Count);
                var lvIndex = randLvNums.Count == 0 ? 1 : randLvNums[randID];
                nextRealLevelNum = loadRandomlyAfterDone ? lvIndex + 1 : levelStartNumAfterAllDone;
            }
        }

        public void LoadNextLevel()
        {
            OnCompleteUnityLevel?.Invoke(curLevelNum);
            var targetSceneID = nextRealLevelNum - 1;
            var curScene = SceneManager.GetActiveScene();
            curRealLevelNum = targetSceneID + 1;
            curLevelNum++;
            OnStartUnityLevel?.Invoke(curLevelNum);
            nextLevelNum = curLevelNum + 1;
            nextRealLevelNum = curRealLevelNum + 1;
            CheckLevelOverFlow(curScene.buildIndex);
            PlayerPrefs.SetInt("curLevelNum", curLevelNum);
            PlayerPrefs.SetInt("curRealLevelNum", curRealLevelNum);
            PlayerPrefs.SetInt("nextLevelNum", nextLevelNum);
            PlayerPrefs.SetInt("nextRealLevelNum", nextRealLevelNum);
            if (GameLevel.Instance != null)
            {
                GameLevel.Instance.OnLoadNextLevel();
            }
            SceneManager.LoadScene(targetSceneID);
        }

        public void LoadLevelByIndex(int buildIndex)
        {
            var ln = buildIndex + 1;
            var next_ln = ln + 1;
            if (next_ln > SceneManager.sceneCountInBuildSettings)
            {
                next_ln = levelStartNumAfterAllDone;
            }
            curLevelNum = ln;
            curRealLevelNum = ln;
            nextLevelNum = next_ln;
            nextRealLevelNum = next_ln;
            PlayerPrefs.SetInt("curLevelNum", ln);
            PlayerPrefs.SetInt("curRealLevelNum", ln);
            PlayerPrefs.SetInt("nextLevelNum", next_ln);
            PlayerPrefs.SetInt("nextRealLevelNum", next_ln);
            if (GameLevel.Instance != null)
            {
                GameLevel.Instance.OnLoadNextLevel();
            }
            SceneManager.LoadScene(buildIndex);
        }

        public void LoadLevelFromBuildIndex(string levelName)
        {
            var sceneCount = SceneManager.sceneCount;
            bool found = false;
            Scene foundScene = default;
            for (int i = 0; i < sceneCount; i++)
            {
                var sc = SceneManager.GetSceneAt(i);
                if (sc.name == levelName)
                {
                    found = true;
                    foundScene = sc;
                    break;
                }
            }

            if (found)
            {
                LoadLevelByIndex(foundScene.buildIndex);
            }
        }

        public void ReloadThisLevel()
        {
            OnReloadUnityLevel?.Invoke(curLevelNum);
            var curId = SceneManager.GetActiveScene().buildIndex;
            if (GameLevel.Instance != null)
            {
                GameLevel.Instance.OnReloadLevel();
            }
            SceneManager.LoadScene(curId);
        }

        //todo async loading level/streaming

        protected internal override void OnTick()
        {
            
        }
    }
}