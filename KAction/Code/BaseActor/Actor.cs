using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameplayFramework
{
    public abstract partial class Actor : MonoBehaviour
    {
        internal void AwakeActor()
        {
            InitData();
            this.Born();
            OnStartOnce();
            for (int i = 0; i < gameplayComponents.Count; i++)
            {
                gameplayComponents[i].StartComponentOnce();
            }
            level.OnLevelGameplayStartEv += StartGameplay;
            level.onLevelGameplayEndEv += EndGameplay;
        }

        void StartGameplay() { gameplayRun = true; }
        void EndGameplay() { gameplayRun = false; }

        void OnEnable()
        {
            OnAppearActor();
            for (int i = 0; i < gameplayComponents.Count; i++)
            {
                gameplayComponents[i].OnAppearActor();
            }
        }

        protected virtual void OnAppearActor()
        {
            isDead = deathStarted = isActorPaused = false;
            life = initialLife;
            timeScale = initialTimeScale;
            onStartOrSpawn?.Invoke();
            OnStartOrSpawnEv?.Invoke();
        }

        void OnDisable()
        {
            for (int i = 0; i < gameplayComponents.Count; i++)
            {
                gameplayComponents[i].OnCleanupComponent();
            }
            OnCleanupActor();
            StopAllCoroutines();
            level.OnLevelGameplayStartEv -= StartGameplay;
            level.onLevelGameplayEndEv -= EndGameplay;
        }

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
                            gameplayComponents[i].UpdateComponentPhysics(dt, fdt);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < gameplayComponents.Count; i++)
                        {
                            gameplayComponents[i].UpdateComponent(dt, fdt);
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