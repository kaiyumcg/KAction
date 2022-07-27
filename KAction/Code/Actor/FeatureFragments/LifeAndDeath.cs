using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameplayFramework
{
    public abstract partial class Actor : MonoBehaviour
    {
        public void Reborn() 
        {
            if (!isDead || !gameplayRun || deathStarted) { return; }
            StartActorLifeCycle(firstTimePool : false);
        }

        public void Murder(bool obliterate = false, bool asyncDeathScriptSupport = true, bool childDeathOneAfterAnother = false, OnDoAnything OnComplete = null)
        {
            if (isDead || deathStarted || !canRecieveDamage || !gameplayRun) { return; }
            _DeathProc(obliterate, !asyncDeathScriptSupport, childDeathOneAfterAnother, OnComplete);
        }

        void _DeathProc(bool destroyCompletely, bool sync, bool childDeathOneAfterAnother, OnDoAnything OnComplete)
        {
            deathStarted = true;
            life = 0.0f;
            StartCoroutine(DeathProcAsync());
            IEnumerator DeathProcAsync()
            {
                Call_OnStartDeath();
                if (!sync)
                {
                    yield return StartCoroutine(OnStartDeathAsync());
                }

                int totalChildCount = childActors.Count;
                int childDeathCount = 0;
                if (childDeathOneAfterAnother)
                {
                    for (int i = 0; i < totalChildCount; i++)
                    {
                        var childDead = false;
                        childActors[i].Murder(destroyCompletely, sync, true, () => { childDead = true; });
                        while (!childDead) { yield return null; }
                    }
                }
                else
                {
                    for (int i = 0; i < totalChildCount; i++)
                    {
                        childActors[i].Murder(destroyCompletely, sync, false, () => { childDeathCount++; });
                    }
                    while (childDeathCount < totalChildCount) { yield return null; }
                }

                OnComplete?.Invoke();
                EndActorLifeCycle(destroyCompletely, firstTimePool: false);
            }
        }

        public void Damage(float damage, bool asyncDeathScriptSupport = true, 
            bool incaseOfMurderChildsDieOneByOne = false, bool destroyGameObjectIncaseOfDeath = false)
        {
            if (isDead || deathStarted || !canRecieveDamage || !gameplayRun) { return; }
            var dm = Mathf.Abs(damage);
            life -= dm;
            Call_OnDamage(dm);
            if (life <= 0.0f)
            {
                _DeathProc(destroyGameObjectIncaseOfDeath, !asyncDeathScriptSupport, incaseOfMurderChildsDieOneByOne, null);
            }
        }

        public void DamageFromDirection(float damage, Vector3 direction, bool asyncDeathScriptSupport = true,
            bool incaseOfMurderChildsDieOneByOne = false, bool destroyGameObjectIncaseOfDeath = false)
        {
            if (isDead || deathStarted || !canRecieveDamage || !gameplayRun) { return; }
            var dm = Mathf.Abs(damage);
            life -= dm;
            Call_DirectionalDamage(dm, direction);
            if (life <= 0.0f)
            {
                _DeathProc(destroyGameObjectIncaseOfDeath, !asyncDeathScriptSupport, incaseOfMurderChildsDieOneByOne, null);
            }
        }

        public void AddHealth(float health)
        {
            if (isDead || deathStarted || !canGainHealth || !gameplayRun) { return; }
            var hl = Mathf.Abs(health);
            life += hl;
            if (life > initialLife && !healthOverflow) { life = initialLife; }
            Call_OnGainHealth(health);
        }

        public void AddDirectionalHealth(float health, Vector3 direction)
        {
            if (isDead || deathStarted || !canGainHealth || !gameplayRun) { return; }
            var hl = Mathf.Abs(health);
            life += hl;
            if (life > initialLife && !healthOverflow) { life = initialLife; }
            Call_OnDirectionalGainHealth(health, direction);
        }
    }
}