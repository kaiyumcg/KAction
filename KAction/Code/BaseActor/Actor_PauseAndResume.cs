using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameplayFramework
{
    public abstract partial class Actor : MonoBehaviour
    {
        [SerializeField] bool pauseResumeAffectsChildActors = true;
        public bool PauseResumeAffectChildActors { get { return pauseResumeAffectsChildActors; } set { pauseResumeAffectsChildActors = value; } }
        [SerializeField] UnityEvent onPause = null, onResume = null;
        public event OnDoAnything OnPauseEv, OnResumeEv;
        float prePausedTimeScale = 0.0f;
        public void Pause(bool ignoreChildControlToggle = false)
        {
            if (isActorPaused || !gameplayRun || isDead || deathStarted) { return; }
            prePausedTimeScale = timeScale;
            timeScale = 0.0f;
            isActorPaused = true;
            onPause?.Invoke();
            OnPauseEv?.Invoke();
            OnPause();

            if (pauseResumeAffectsChildActors && ignoreChildControlToggle == false)
            {
                if (childActorListDirty == false)
                {
                    for (int i = 0; i < childActors.Count; i++)
                    {
                        childActors[i].Pause();
                    }
                }
            }
        }

        public void Resume(bool ignoreChildControlToggle = false)
        {
            if (isActorPaused == false || !gameplayRun || isDead || deathStarted) { return; }
            timeScale = prePausedTimeScale;
            isActorPaused = false;
            onResume?.Invoke();
            OnResumeEv?.Invoke();
            OnResume();

            if (pauseResumeAffectsChildActors && ignoreChildControlToggle == false)
            {
                if (childActorListDirty == false)
                {
                    for (int i = 0; i < childActors.Count; i++)
                    {
                        childActors[i].Resume();
                    }
                }
            }
        }

        protected virtual void OnPause() { }
        protected virtual void OnResume() { }
    }
}