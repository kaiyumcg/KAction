using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//all events in game service, game service manager, game level, level module, actor level module and actor should be null checked and then 'new'ed.
//Singleton removal--some class's singleton should only be internal-check it
//if user loads a game level without boot scene(for testing in editor), then editor conditional compilation will
//                  ---redirect into boot scene, set to load scene as the one user were in, and then loads it.

namespace GameplayFramework
{
    public abstract class GameService : MonoBehaviour
    {
        [SerializeField, HideInInspector, Multiline] string serviceAbout = "", serviceDescription = "";
        [SerializeField, HideInInspector] bool waitForThisInitialization = false, useDelay = false;
        [SerializeField, HideInInspector] float delayAmount = 2f;
        [SerializeField, HideInInspector] ActionOnLog whenError = ActionOnLog.DoNothing,
            whenException = ActionOnLog.DoNothing, whenCodeFailure = ActionOnLog.DoNothing;
        bool isRunning = true;
        protected internal abstract void OnTick();
        protected internal virtual void OnInit() { }
        protected internal virtual IEnumerator OnInitAsync() 
        {
            if (useDelay)
            {
                yield return new WaitForSeconds(delayAmount);
            }
            else
            {
                yield break;
            }
        }
        protected virtual void OnRestart() { }
        protected virtual void OnStop() { }
        protected virtual void OnStartManual() { }
        
        public bool IsRunning { get { return isRunning; } internal set { isRunning = value; } }
        internal bool WaitForThisInitialization { get { return waitForThisInitialization; } }
        public string AboutService { get { return serviceAbout; } }
        public string ServiceDescription { get { return serviceDescription; } }

        internal void DoLogAction(ErrorType errorType)
        {
            ActionOnLog action = ActionOnLog.DoNothing;
            if (errorType == ErrorType.Exception)
            {
                action = whenException;
            }
            else if (errorType == ErrorType.Error)
            {
                action = whenError;
            }
            else if (errorType == ErrorType.CodeFailure)
            {
                action = whenCodeFailure;
            }
            if (action == ActionOnLog.DoNothing) { return; }
            if (action == ActionOnLog.Restart) { RestartService(); }
            else if (action == ActionOnLog.Stop) { StopService(); }
        }

        public void StopService()
        {
            isRunning = false;
            StopAllCoroutines();
            OnStop();
        }

        public void StartServiceIfStopped()
        {
            if (isRunning) { return; }
            isRunning = true;
            OnInit();
            StartCoroutine(OnInitAsync());
            OnStartManual();
        }

        public void RestartService()
        {
            isRunning = true;
            StopAllCoroutines();
            OnInit();
            StartCoroutine(OnInitAsync());
            OnRestart();
        }

        public void RemoveService()
        {
            StopService();
            GameServiceManager.RemoveService(this.GetType());
        }
    }
}