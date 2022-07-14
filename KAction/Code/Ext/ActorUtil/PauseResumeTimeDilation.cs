using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public static partial class ActorUtil
    {
        public static void PauseRootActors()
        {
            var handle = ActorLevelModule.instance;
            var actors = handle.rootActors;
            var isDirty = handle.actorListDirty;
            if (isDirty == false)
            {
                for (int i = 0; i < actors.Count; i++)
                {
                    actors[i].Pause();
                }
            }
        }

        public static void ResumeRootActors()
        {
            var handle = ActorLevelModule.instance;
            var actors = handle.rootActors;
            var isDirty = handle.actorListDirty;
            if (isDirty == false)
            {
                for (int i = 0; i < actors.Count; i++)
                {
                    actors[i].Resume();
                }
            }
        }

        public static void SetCustomTimeForRootActors(float timeScale)
        {
            var handle = ActorLevelModule.instance;
            var actors = handle.rootActors;
            var isDirty = handle.actorListDirty;
            if (isDirty == false)
            {
                for (int i = 0; i < actors.Count; i++)
                {
                    actors[i].TimeScale = timeScale;
                }
            }
        }

        public static void PauseAllActors()
        {
            var handle = ActorLevelModule.instance;
            var actors = handle.actors;
            var isDirty = handle.actorListDirty;
            if (isDirty == false)
            {
                for (int i = 0; i < actors.Count; i++)
                {
                    actors[i].Pause(true);
                }
            }
        }

        public static void ResumeAllActors()
        {
            var handle = ActorLevelModule.instance;
            var actors = handle.actors;
            var isDirty = handle.actorListDirty;
            if (isDirty == false)
            {
                for (int i = 0; i < actors.Count; i++)
                {
                    actors[i].Resume(true);
                }
            }
        }

        public static void SetCustomTimeForAllActors(float timeScale)
        {
            var handle = ActorLevelModule.instance;
            var actors = handle.actors;
            var isDirty = handle.actorListDirty;
            if (isDirty == false)
            {
                for (int i = 0; i < actors.Count; i++)
                {
                    actors[i].timeScale = timeScale;
                }
            }
        }
    }
}