using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameplayFramework
{
    public abstract partial class Actor : MonoBehaviour
    {
        /// <summary>
        /// Preloaded data by editor tools
        /// </summary>
        [SerializeField] List<GameplayComponent> gameplayComponents;
        [SerializeField, HideInInspector] Transform _transform;
        [SerializeField, HideInInspector] GameObject _gameobject;
        [SerializeField, HideInInspector] bool isRoot = true;
        [SerializeField, HideInInspector] GameLevel level;
        [SerializeField, HideInInspector] List<Actor> childActors;
        [SerializeField, HideInInspector] Actor owner = null;

        /// <summary>
        /// Actor Options for editor inspector
        /// </summary>
        [SerializeField] ActorType mType = ActorType.Player;
        [SerializeField] float life = 100f, timeScale = 1.0f;
        [SerializeField] List<GameplayTag> tags = null;

        /// <summary>
        /// Property data
        /// </summary>
        bool canTick = false, canRecieveDamage = false, canGainHealth = false,
            healthOverflow = false, pauseResumeAffectsChildActors = true, timeDilationAffectsChildActors = true;

        /// <summary>
        /// Transient data for runtime
        /// </summary>
        [System.NonSerialized, HideInInspector]
        bool isJumping = false, isHelixing = false, isTurning = false, isMoving = false,
            isThursting = false, isSwirling = false, isPoping = false, isSpringReleasing = false,
            isSummoning = false, isShaking = false, isStillHanging = false, isSwinging = false,
            isBrownianDoing = false, isRubberBandFollowing = false;

        bool componentListDirty = false, childActorListDirty = false, gameplayRun = false, 
            isActorPaused = false, deathStarted = false, isDead = false;
        float prePausedTimeScale = 1.0f, initialLife = 100.0f, initialTimeScale = 1.0f;
        NullChecker nuller = null;

        void InitData()
        {
            isJumping = isHelixing = isTurning = isMoving = isThursting = isSwirling = isPoping = isSpringReleasing =
            isSummoning = isShaking = isStillHanging = isSwinging = isBrownianDoing = isRubberBandFollowing = false;

            componentListDirty = childActorListDirty = isActorPaused = isDead = deathStarted = false;
            gameplayRun = level.HasLevelGameplayBeenStarted && !level.HasLevelGameplayBeenEnded;
            if (nuller == null)
            {
                nuller = new NullChecker();
                initialLife = life;
                initialTimeScale = timeScale;
            }

            if (onChangeActorData == null) { onChangeActorData = new UnityEvent<string, ActorData>(); }
            else { onChangeActorData.RemoveAllListeners(); }
            if (onDamage == null) { onDamage = new UnityEvent<float>(); }
            else { onDamage.RemoveAllListeners(); }
            if (onGainHealth == null) { onGainHealth = new UnityEvent<float>(); }
            else { onGainHealth.RemoveAllListeners(); }
            if (onDirectionalDamage == null) { onDirectionalDamage = new UnityEvent<float, Vector3>(); }
            else { onDirectionalDamage.RemoveAllListeners(); }
            if (onDirectionalGainHealth == null) { onDirectionalGainHealth = new UnityEvent<float, Vector3>(); }
            else { onDirectionalGainHealth.RemoveAllListeners(); }
            if (onStartDeath == null) { onStartDeath = new UnityEvent(); }
            else { onStartDeath.RemoveAllListeners(); }
            if (onDeath == null) { onDeath = new UnityEvent(); }
            else { onDeath.RemoveAllListeners(); }
            if (onPause == null) { onPause = new UnityEvent(); }
            else { onPause.RemoveAllListeners(); }
            if (onResume == null) { onResume = new UnityEvent(); }
            else { onResume.RemoveAllListeners(); }
        }
    }
}