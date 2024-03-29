using KSaveDataMan;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Author: Md. Al Kaiyum(Rumman)
/// Email: kaiyumce06rumman@gmail.com
/// Player Actor class.
/// </summary>
namespace GameplayFramework
{
    public abstract class PlayerActor : Actor
    {
        [SerializeField] [HideInInspector] bool useDeviceStorageForScore = false;
        [SerializeField] [HideInInspector] int initialScore;
        [SerializeField] [HideInInspector] int currentScore;
        [SerializeField] [HideInInspector] UnityEvent<int> onUpdateScore;
        public OnDoAnything<int> OnUpdateScore;

        public abstract IPlayerController PlayerController { get; set; }
        public int Score { get { return currentScore; } }
        const string lastScoreIdentifier = "_Player_Last_Score_";
        //todo
        const string highScoreIdentifier = "_Player_High_Score_";
        //todo we can actually save a set of commonly used data, playerDataAPI?

        GameLevel gameMan;
        bool addedController = false;

        protected override void OnEndActor()
        {
            if (gameMan.HasLevelGameplayBeenStarted == false || gameMan.HasLevelGameplayBeenEnded)
            {
                base.OnEndActor();
            }
            else
            {
                if (PlayerController != null)
                {
                    PlayerController.OnEndController(this);
                }

                if (addedController)
                {
                    gameMan.OnLevelGameplayStartEv -= StartController;
                }
                base.OnEndActor();
            }
        }

        void StartController()
        {
            if (PlayerController != null)
            {
                PlayerController.OnStartController(this);
            }
        }

        protected override void OnStartActor()
        {
            if (onUpdateScore == null) { onUpdateScore = new UnityEvent<int>(); }
            base.OnStartActor();
            if (useDeviceStorageForScore)
            {
                /*
                var diskValue_lastScore = SaveDataManager.LoadInt(lastScoreIdentifier, -1);
                if (diskValue_lastScore < 0)
                {
                    currentScore = initialScore;
                    SaveDataManager.SaveInt(lastScoreIdentifier, currentScore);
                }
                else
                {
                    currentScore = diskValue_lastScore;
                }
                */
            }
            else
            {
                currentScore = initialScore;
            }

            gameMan = FindObjectOfType<GameLevel>();
            gameMan.OnLevelGameplayStartEv += StartController;
            addedController = true;
        }

        public void AddScore(int score)
        {
            var sc = Mathf.Abs(score);
            this.currentScore += sc;
            onUpdateScore?.Invoke(score);
            OnUpdateScore?.Invoke(score);

            if (useDeviceStorageForScore)
            {
                //SaveDataManager.SaveInt(lastScoreIdentifier, currentScore);
            }
        }

        protected override void UpdateActor(float dt, float fixedDt)
        {
            if (PlayerController != null)
            {
                PlayerController.ControlInUpdate(dt, fixedDt);
            }
        }

        protected override void UpdateActorPhysics(float dt, float fixedDt)
        {
            if (PlayerController != null)
            {
                PlayerController.ControlInPhysicsUpdate(dt, fixedDt);
            }
        }
    }
}