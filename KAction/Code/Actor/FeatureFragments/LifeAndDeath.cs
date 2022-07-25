using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameplayFramework
{
    public abstract partial class Actor : MonoBehaviour
    {
        public void Born() { _Born(false); }
        void _Born(bool initTime)
        {
            if (initTime == false)
            {
                if (!isDead || !gameplayRun || deathStarted) { return; }
            }
            StopAllCoroutines();//todo script theke born call korle pool data r flag changed hoye not-free hobe!
            _GameObject.SetActive(true);
            isDead = deathStarted = isActorPaused = false;
            life = initialLife;
            timeScale = initialTimeScale;
            OnStart();
        }

        public void Murder(bool obliterate = false, bool sync = false)
        {
            if (isDead || deathStarted || !canRecieveDamage || !gameplayRun) { return; }
            _DeathProc(obliterate, sync);
        }

        void _DeathProc(bool destroyCompletely, bool sync)
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
                Call_OnDeath();
                isDead = true;
                _GameObject.SetActive(false);

                if (destroyCompletely)
                {
                    ActorLevelModule.instance.OnDestroyCallUnity(this);
                    GameObject.Destroy(_GameObject);
                }
                else
                {
                    ActorLevelModule._FreeActor(this);
                }
            }
        }

        public void Damage(float damage)
        {
            if (isDead || deathStarted || !canRecieveDamage || !gameplayRun) { return; }
            var dm = Mathf.Abs(damage);
            life -= dm;
            Call_OnDamage(dm);
            if (life <= 0.0f)
            {
                _DeathProc(false, false);
            }
        }

        public void DamageFromDirection(float damage, Vector3 direction)
        {
            if (isDead || deathStarted || !canRecieveDamage || !gameplayRun) { return; }
            var dm = Mathf.Abs(damage);
            life -= dm;
            Call_DirectionalDamage(dm, direction);
            if (life <= 0.0f)
            {
                _DeathProc(false, false);
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