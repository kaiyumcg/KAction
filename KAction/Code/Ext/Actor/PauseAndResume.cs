using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameplayFramework
{
    public static partial class ActorExt
    {
        static void _Pause(Actor actor, bool ignoreChildControlToggle = false)
        {
            if (actor.isActorPaused || !actor.gameplayRun || actor.isDead || actor.deathStarted) { return; }
            actor.prePausedTimeScale = actor.timeScale;
            actor.timeScale = 0.0f;
            actor.isActorPaused = true;
            actor.onPause?.Invoke();
            actor.OnPauseEv_Invoke();
            actor.OnPause();

            if (actor.pauseResumeAffectsChildActors && ignoreChildControlToggle == false)
            {
                if (actor.childActorListDirty == false)
                {
                    for (int i = 0; i < actor.childActors.Count; i++)
                    {
                        actor.childActors[i].Pause();
                    }
                }
            }
        }

        static void _Resume(Actor actor, bool ignoreChildControlToggle = false)
        {
            if (actor.isActorPaused == false || !actor.gameplayRun || actor.isDead || actor.deathStarted) { return; }
            actor.timeScale = actor.prePausedTimeScale;
            actor.isActorPaused = false;
            actor.onResume?.Invoke();
            actor.OnResumeEv_Invoke();
            actor.OnResume();

            if (actor.pauseResumeAffectsChildActors && ignoreChildControlToggle == false)
            {
                if (actor.childActorListDirty == false)
                {
                    for (int i = 0; i < actor.childActors.Count; i++)
                    {
                        actor.childActors[i].Resume();
                    }
                }
            }
        }
    }
}