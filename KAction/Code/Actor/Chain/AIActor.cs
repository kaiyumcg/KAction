using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Author: Md. Al Kaiyum(Rumman)
/// Email: kaiyumce06rumman@gmail.com
/// AI Actor class
/// </summary>
namespace GameplayFramework
{
    public abstract class AIActor : Actor
    {
        public abstract IAIController AI_Controller { get; set; }
        GameLevel gameMan;
        bool addedController = false;

        protected internal override void OnEndActor()
        {
            if (gameMan.HasLevelGameplayBeenStarted == false || gameMan.HasLevelGameplayBeenEnded)
            {
                base.OnEndActor();
            }
            else
            {
                if (AI_Controller != null)
                {
                    AI_Controller.OnEndController(this);
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
            if (AI_Controller != null)
            {
                AI_Controller.OnStartController(this);
            }
        }

        protected internal override void OnStartActor()
        {
            base.OnStartActor();
            gameMan = FindObjectOfType<GameLevel>();
            gameMan.OnLevelGameplayStartEv += StartController;
            addedController = true;
        }

        protected override void UpdateActor(float dt, float fixedDt)
        {
            if (AI_Controller != null)
            {
                AI_Controller.ControlInUpdate(dt, fixedDt);
            }
        }

        protected override void UpdateActorPhysics(float dt, float fixedDt)
        {
            if (AI_Controller != null)
            {
                AI_Controller.ControlInPhysicsUpdate(dt, fixedDt);
            }
        }
    }
}