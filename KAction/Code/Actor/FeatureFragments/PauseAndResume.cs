using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameplayFramework
{
    public abstract partial class Actor : MonoBehaviour
    {
        public void Pause(bool ignoreChildOverride = false)
        {
            if (isActorPaused || !gameplayRun || isDead || deathStarted) { return; }
            prePausedTimeScale = timeScale;
            timeScale = 0.0f;
            isActorPaused = true;
            Call_OnPause();

            if (pauseResumeAffectsChildActors && ignoreChildOverride == false)
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

        public void Resume(bool ignoreChildOverride = false)
        {
            if (isActorPaused == false || !gameplayRun || isDead || deathStarted) { return; }
            timeScale = prePausedTimeScale;
            isActorPaused = false;
            Call_OnResume();

            if (pauseResumeAffectsChildActors && ignoreChildOverride == false)
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
    }
}