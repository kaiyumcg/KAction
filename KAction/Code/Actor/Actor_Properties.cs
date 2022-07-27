using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public abstract partial class Actor : MonoBehaviour
    {
        public ActorType ActorType { get { return mType; } }
        public Transform _Transform { get { return _transform; } }
        public GameObject _GameObject { get { return _gameobject; } }

        public bool CanTick { get { return canTick; } set { canTick = value; } }
        public bool CanRecieveDamage { get { return canRecieveDamage; } set { canRecieveDamage = value; } }
        public bool CanGainHealth { get { return canGainHealth; } set { canGainHealth = value; } }
        public bool HealthOverflow { get { return healthOverflow; } set { healthOverflow = value; } }
        public float FullLife { get { return initialLife; } }
        public float NormalizedLife { get { return life / initialLife; } }
        public float CurrentLife { get { return life; } }
        public bool HasDeathBeenStarted { get { return deathStarted; } }
        public bool IsDead { get { return isDead; } }
        public bool PauseResumeAffectChildActors { get { return pauseResumeAffectsChildActors; } set { pauseResumeAffectsChildActors = value; } }
        public bool TimeDilationAffectChildActors { get { return timeDilationAffectsChildActors; } set { timeDilationAffectsChildActors = value; } }
        public float TimeScale
        {
            get
            {
                return timeScale;
            }
            set
            {
                timeScale = value;
                if (timeDilationAffectsChildActors && childActorListDirty == false)
                {
                    for (int i = 0; i < childActors.Count; i++)
                    {
                        var ch = childActors[i];
                        ch.TimeScale = value;
                    }
                }
            }
        }
    }
}