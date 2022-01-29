using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    internal enum ActionOnService { DoNothing = 0, Stop = 1, Restart = 2 }

    public abstract class GameService : MonoBehaviour
    {
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
                yield return null;
            }
        }
        protected virtual void OnRestart() { }
        protected virtual void OnStop() { }
        protected virtual void OnStartManual() { }

        [SerializeField] bool waitForThisInitialization = false;
        [SerializeField] bool useDelay = false;
        [SerializeField] float delayAmount = 2f;
        [Header("What to do")]
        [SerializeField] ActionOnService whenError = ActionOnService.DoNothing;
        [SerializeField] ActionOnService whenException = ActionOnService.DoNothing, whenCodeFailure = ActionOnService.DoNothing;
        bool isRunning = true;
        public bool IsRunning { get { return isRunning; } internal set { isRunning = value; } }
        internal bool WaitForThisInitialization { get { return waitForThisInitialization; } }

        #region Logging
        protected void Check(System.Action Code)
        {
            if (GameLevel.Instance == null)
            {
                try
                {
                    Code?.Invoke();
                }
                catch (System.Exception ex)
                {
                    Debug.Log("Code execution failure in service. Error Messag: " + ex.Message);
                    DoAction(whenCodeFailure);
                }
            }
            else
            {
                KLog.Check(Code);
            }
        }

        protected void PrintLog(string message, Color color = default)
        {
            if (GameLevel.Instance == null)
            {
                string ltxt = message;
                if (color != default)
                {
                    ltxt = string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color> Service log: ", (byte)(color.r * 255f),
                        (byte)(color.g * 255f), (byte)(color.b * 255f), message);
                    Debug.Log(ltxt);
                }
                else
                {
                    Debug.Log("Service log: " + ltxt);
                }
            }
            else
            {
                KLog.Print("Service log: " + message, color);
            }
        }

        protected void PrintError(string message)
        {
            if (GameLevel.Instance == null)
            {
                Debug.LogError("Service Error: " + message);
            }
            else
            {
                KLog.PrintError("Service Error: " + message);
            }
            DoAction(whenError);
        }

        protected void PrintWarning(string message)
        {
            if (GameLevel.Instance == null)
            {
                Debug.LogWarning("Service warning: " + message);
            }
            else
            {
                KLog.PrintWarning("Service warning: " + message);
            }
        }

        protected void PrintException(Exception exception)
        {
            exception.Source += Environment.NewLine + " At Service";
            if (GameLevel.Instance == null)
            {
                Debug.LogException(exception);
            }
            else
            {
                KLog.PrintException(exception);
            }
            DoAction(whenException);
        }

        protected void PrintException(Exception exception, UnityEngine.Object context)
        {
            exception.Source += Environment.NewLine + " At Service";
            if (GameLevel.Instance == null)
            {
                Debug.LogException(exception, context);
            }
            else
            {
                KLog.PrintException(exception, context);
            }
            DoAction(whenException);
        }
        #endregion

        void DoAction(ActionOnService action)
        {
            if (action == ActionOnService.DoNothing) { return; }
            if (action == ActionOnService.Restart) { RestartService(); }
            else if (action == ActionOnService.Stop) { StopService(); }
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
    }
}