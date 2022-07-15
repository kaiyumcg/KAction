using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public static partial class ActorUtil
    {
        static void _PauseAllActors(bool isRoot)
        {
            var handle = ActorLevelModule.instance;
            var actors = isRoot ? handle.rootActors : handle.actors;
            var isDirty = handle.actorListDirty;
            if (isDirty == false)
            {
                for (int i = 0; i < actors.Count; i++)
                {
                    actors[i].Pause(true);
                }
            }
        }

        static void _ResumeAllActors(bool isRoot)
        {
            var handle = ActorLevelModule.instance;
            var actors = isRoot ? handle.rootActors : handle.actors;
            var isDirty = handle.actorListDirty;
            if (isDirty == false)
            {
                for (int i = 0; i < actors.Count; i++)
                {
                    actors[i].Resume(true);
                }
            }
        }

        static void _SetCustomTimeForAllActors(bool isRoot, float timeScale)
        {
            var handle = ActorLevelModule.instance;
            var actors = isRoot ? handle.rootActors : handle.actors;
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