using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameplayFramework
{
    public abstract partial class Actor : MonoBehaviour
    {
        [SerializeField] UnityEvent onPause = null, onResume = null;
        public event OnDoAnything OnPauseEv, OnResumeEv;
        public void Pause()
        {
            if (isActorPaused || !gameplayRun || isDead || deathStarted) { return; }
            timeScale = 0.0f;
            isActorPaused = true;
            onPause?.Invoke();
            OnPauseEv?.Invoke();
            OnPause();

            if (pauseResumeAffectsChildActors)
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

        public void Resume()
        {
            if (isActorPaused == false || !gameplayRun || isDead || deathStarted) { return; }
            timeScale = 1.0f;
            isActorPaused = false;
            onResume?.Invoke();
            OnResumeEv?.Invoke();
            OnResume();

            if (pauseResumeAffectsChildActors)
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