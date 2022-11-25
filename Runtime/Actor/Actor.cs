using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameplayFramework
{
    public abstract partial class Actor : MonoBehaviour
    {
        //todo when actor or level module or actor level module or game level 's data should be patched in runtime? check it
        //and which and which data?

        //todo change parent-child of actor and set owner
        //todo patch map data for actor stream scripts(Actor, GameplayComponent, Level Module, Game Level etc)


        //todo data layer
        //todo visibility

        //todo editor tool to check prefab validity, referenced prefab must reside in asset folder and not in the scene
        //also editor tool make sure all scene actor and prefab actors have valid up to date baked data(on prebuild or on pre-post-compile etc?)

        internal void StartActorLifeCycle(bool firstTimePool, bool shouldMarkBusy)
        {
            InitData();
            if (shouldMarkBusy)
            {
                ActorLevelModule.instance.MarkBusy(this);
            }
            StopAllCoroutines();
            _gameobject.SetActive(true);
            isDead = deathStarted = isActorPaused = false;
            life = initialLife;
            timeScale = initialTimeScale;
            if (firstTimePool == false)
            {
                OnStartActor();
                for (int i = 0; i < gameplayComponents.Count; i++)
                {
                    gameplayComponents[i].OnStartActor();
                }
            }
            
            for (int i = 0; i < childActors.Count; i++)
            {
                childActors[i].StartActorLifeCycle(firstTimePool, shouldMarkBusy);
            }

            if (firstTimePool == false)
            {
                level.OnLevelGameplayStartEv += StartGameplay;
                level.onLevelGameplayEndEv += EndGameplay;
            }
        }

        internal void EndActorLifeCycle(bool firstTimePool, bool gameObjectDestroy)
        {
            StopAllCoroutines();
            deathStarted = true;
            life = 0.0f;
            isDead = true;
            _gameobject.SetActive(false);
            if (firstTimePool == false)
            {
                for (int i = 0; i < gameplayComponents.Count; i++)
                {
                    gameplayComponents[i].OnEndActor();
                }
            }
            
            for (int i = 0; i < childActors.Count; i++)
            {
                childActors[i].EndActorLifeCycle(firstTimePool : firstTimePool, gameObjectDestroy : gameObjectDestroy);
            }

            if(firstTimePool == false)
            {
                Call_OnDeath();
                OnEndActor();
                level.OnLevelGameplayStartEv -= StartGameplay;
                level.onLevelGameplayEndEv -= EndGameplay;
            }

            ActorLevelModule.instance.MarkFree(this);
            if (gameObjectDestroy && firstTimePool == false)
            {
                ActorLevelModule.instance.OnDestroyActorInstance(this);
                GameObject.Destroy(_gameobject);
            }
        }

        void StartGameplay() { gameplayRun = true; }
        void EndGameplay() { gameplayRun = false; }

        internal void Tick(float dt_from_unity, float fixedDt_from_unity)
        {
            if (!isRoot) { return; }
            var dt = dt_from_unity * timeScale;
            var fdt = fixedDt_from_unity * timeScale;
            TickActorInternal(dt, fdt, physx: false);
        }

        internal void TickPhysics(float dt_from_unity, float fixedDt_from_unity)
        {
            if (!isRoot) { return; }
            var dt = dt_from_unity * timeScale;
            var fdt = fixedDt_from_unity * timeScale;
            TickActorInternal(dt, fdt, physx: true);
        }

        void TickActorInternal(float dt, float fdt, bool physx)
        {
            if (!gameplayRun) { return; }
            if (canTick && isActorPaused == false && isDead == false && deathStarted == false)
            {
                if (physx)
                {
                    UpdateActorPhysics(dt, fdt);
                }
                else
                {
                    UpdateActor(dt, fdt);
                }

                if (componentListDirty == false)
                {
                    if (physx)
                    {
                        for (int i = 0; i < gameplayComponents.Count; i++)
                        {
                            var comp = gameplayComponents[i];
                            if (comp.canTick)
                            {
                                comp.UpdateComponentPhysics(dt, fdt);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < gameplayComponents.Count; i++)
                        {
                            var comp = gameplayComponents[i];
                            if (comp.canTick)
                            {
                                comp.UpdateComponent(dt, fdt);
                            }
                        }
                    }
                }
            }

            if (childActorListDirty == false)
            {
                var dt_from_unity = ActorLevelModule.RawDelta;
                var fdt_from_unity = ActorLevelModule.RawFixedDelta;
                for (int i = 0; i < childActors.Count; i++)
                {
                    var chActor = childActors[i];
                    var ch_dt = timeDilationAffectsChildActors ? dt : dt_from_unity * chActor.timeScale;
                    var ch_fdt = timeDilationAffectsChildActors ? fdt : fdt_from_unity * chActor.timeScale;
                    childActors[i].TickActorInternal(ch_dt, ch_fdt, physx);
                }
            }
        }
    }
}