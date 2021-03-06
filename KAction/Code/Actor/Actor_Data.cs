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
        [SerializeField, HideInInspector] Transform _transform;
        [SerializeField, HideInInspector] GameObject _gameobject;
        [SerializeField, HideInInspector] bool isRoot = true;
        [SerializeField, HideInInspector] GameLevel level;
        [SerializeField] List<GameplayComponent> gameplayComponents;
        [SerializeField] List<Actor> childActors;
        [SerializeField, HideInInspector] Actor owner = null;

        /// <summary>
        /// Actor Options for editor inspector
        /// </summary>
        [SerializeField] ActorType mType = ActorType.Player;
        [SerializeField] float life = 100f, timeScale = 1.0f;
        [SerializeField] List<GameplayTag> tags = null;
        [SerializeField] bool pauseResumeAffectsChildActors = true;
        [SerializeField] bool timeDilationAffectsChildActors = true;

        /// <summary>
        /// Transient data for runtime
        /// </summary>
        internal ActorDataSet dataTable = null;
        internal bool isJumping = false, isHelixing = false, isTurning = false, isMoving = false,
            isThursting = false, isSwirling = false, isPoping = false, isSpringReleasing = false,
            isSummoning = false, isShaking = false, isStillHanging = false, isSwinging = false,
            isBrownianDoing = false, isRubberBandFollowing = false;
        
        internal bool componentListDirty = false, childActorListDirty = false,
            canRecieveDamage = false, canGainHealth = false, healthOverflow = false,
            canTick = true, gameplayRun = false, isActorPaused = false, 
            isDead = false, deathStarted = false;
        internal float prePausedTimeScale = 0.0f, initialLife = 0.0f, initialTimeScale = 1.0f;
        void InitData()
        {
            dataTable = new ActorDataSet();

            isJumping = isHelixing = isTurning = isMoving = isThursting = isSwirling = isPoping = isSpringReleasing =
            isSummoning = isShaking = isStillHanging = isSwinging = isBrownianDoing = isRubberBandFollowing = false;

            componentListDirty = childActorListDirty = canRecieveDamage = canGainHealth = healthOverflow = 
            gameplayRun =  isActorPaused = isDead =  deathStarted = false;
            initialLife = life; initialTimeScale = timeScale; canTick = true;

            if (onChangeActorData == null) { onChangeActorData = new UnityEvent<string, ActorData>(); }
            if (onDamage == null) { onDamage = new UnityEvent<float>(); }
            if (onGainHealth == null) { onGainHealth = new UnityEvent<float>(); }
            if (onDirectionalDamage == null) { onDirectionalDamage = new UnityEvent<float, Vector3>(); }
            if (onDirectionalGainHealth == null) { onDirectionalGainHealth = new UnityEvent<float, Vector3>(); }
            if (onStartDeath == null) { onStartDeath = new UnityEvent(); }
            if (onDeath == null) { onDeath = new UnityEvent(); }
            if (onPause == null) { onPause = new UnityEvent(); }
            if (onResume == null) { onResume = new UnityEvent(); }
        }
    }
}