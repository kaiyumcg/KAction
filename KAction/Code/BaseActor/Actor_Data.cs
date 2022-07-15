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
        [SerializeField, HideInInspector] internal Transform _transform;
        [SerializeField, HideInInspector] internal GameObject _gameobject;
        [SerializeField, HideInInspector] internal bool isRoot = true;
        [SerializeField, HideInInspector] internal GameLevel level;
        [SerializeField] internal List<GameplayComponent> gameplayComponents;
        [SerializeField] internal List<Actor> childActors;
        [SerializeField, HideInInspector] internal Actor owner = null;

        /// <summary>
        /// Actor Options for editor inspector
        /// </summary>
        [SerializeField] ActorType mType = ActorType.Player;
        [SerializeField] internal float life = 100f, timeScale = 1.0f;
        [SerializeField] internal List<GameplayTag> tags = null;
        [SerializeField] internal bool pauseResumeAffectsChildActors = true;
        [SerializeField] bool timeDilationAffectsChildActors = true;
        [SerializeField] internal ParticleSpawnDesc deathParticle = null, damageParticle = null,
           gainHealthParticle = null, rebornParticle = null;

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
            isSummoning = isShaking = isStillHanging = isSwinging = isBrownianDoing = isRubberBandFollowing =
            componentListDirty = childActorListDirty = canRecieveDamage = canGainHealth = healthOverflow = 
            gameplayRun =  isActorPaused = isDead =  deathStarted = false;
            initialLife = life; initialTimeScale = timeScale; canTick = true;
        }
    }
}