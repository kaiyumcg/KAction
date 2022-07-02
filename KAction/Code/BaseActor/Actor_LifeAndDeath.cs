using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameplayFramework
{
    public abstract partial class Actor : MonoBehaviour
    {
        [SerializeField] ParticleSpawnDesc deathParticle = null, damageParticle = null,
           gainHealthParticle = null, rebornParticle = null;
        [SerializeField] bool canRecieveDamage = false, canGainHealth = false, healthOverflow = false;
        [SerializeField] UnityEvent onStartDeath = null, onDeath = null, onReborn = null;
        [SerializeField] float life = 100f;
        bool isDead = false, deathStarted = false;
        float initialLife = 0.0f, initialTimeScale = 1.0f;
        [SerializeField] UnityEvent<float> onDamage = null, onGainHealth = null;
        [SerializeField] UnityEvent<float, Vector3> onDirectionalDamage = null, onDirectionalGainHealth = null;

        public bool IsDead { get { return isDead; } }
        public bool HasDeathBeenStarted { get { return deathStarted; } }
        public float FullLife { get { return initialLife; } }
        public float NormalizedLife { get { return life / initialLife; } }
        public float CurrentLife { get { return life; } }

        public event OnDoAnything<float> OnDamageEv, OnGainHealthEv;
        public event OnDoAnything<float, Vector3> OnDirectionalDamageEv, OnDirectionalGainHealthEv;
        public event OnDoAnything OnDeathEv, OnStartDeathEv, OnRebornEv;

        protected virtual void OnDamage(float damageAmount) { }
        protected virtual void OnDirectionalDamage(float damageAmount, Vector3 damageDir) { }
        protected virtual void OnGainHealth(float addedHealth) { }
        protected virtual void OnDirectionalGainHealth(float addedHealth, Vector3 damageDir) { }
        protected virtual void OnStartDeath() { }
        protected virtual void OnDeath() { }
        protected virtual void OnReborn() { }
        protected virtual IEnumerator OnStartDeathAsync() { yield break; }

        public void Reborn()
        {
            if (!isDead || !gameplayRun || deathStarted) { return; }
            StopAllCoroutines();
            gameObject.SetActive(true);
            Born();
            onReborn?.Invoke();
            OnRebornEv?.Invoke();
            OnReborn();
            rebornParticle.Spawn(tr);
        }

        void Born()
        {
            isDead = deathStarted = isActorPaused = false;
            life = initialLife;
            timeScale = initialTimeScale;
            OnStart();
            StartCoroutine(OnStartAsync());
        }

        public void Murder()
        {
            if (isDead || deathStarted || !canRecieveDamage || !gameplayRun) { return; }
            DeathProc();
        }

        void DeathProc()
        {
            deathStarted = true;
            life = 0.0f;
            StartCoroutine(DeathProcAsync());
            IEnumerator DeathProcAsync()
            {
                deathParticle.Spawn(tr);
                onStartDeath?.Invoke();
                OnStartDeathEv?.Invoke();
                OnStartDeath();
                yield return StartCoroutine(OnStartDeathAsync());
                onDeath?.Invoke();
                OnDeathEv?.Invoke();
                OnDeath();
                isDead = true;
                gobj.SetActive(false);
            }
        }

        public void Damage(float damage)
        {
            if (isDead || deathStarted || !canRecieveDamage || !gameplayRun) { return; }
            var dm = Mathf.Abs(damage);
            life -= dm;
            onDamage?.Invoke(dm);
            OnDamageEv?.Invoke(dm);
            OnDamage(dm);
            damageParticle.Spawn(tr);
            if (life <= 0.0f)
            {
                DeathProc();
            }
        }

        public void DamageFromDirection(float damage, Vector3 direction)
        {
            if (isDead || deathStarted || !canRecieveDamage || !gameplayRun) { return; }
            var dm = Mathf.Abs(damage);
            life -= dm;
            onDirectionalDamage?.Invoke(dm, direction);
            OnDirectionalDamageEv?.Invoke(dm, direction);
            OnDirectionalDamage(dm, direction);
            damageParticle.Spawn(tr);
            if (life <= 0.0f)
            {
                DeathProc();
            }
        }

        public void AddHealth(float health)
        {
            if (isDead || deathStarted || !canGainHealth || !gameplayRun) { return; }
            var hl = Mathf.Abs(health);
            life += hl;
            if (life > initialLife && !healthOverflow) { life = initialLife; }
            onGainHealth?.Invoke(health);
            OnGainHealthEv?.Invoke(health);
            OnGainHealth(hl);
            gainHealthParticle.Spawn(tr);
        }

        public void AddDirectionalHealth(float health, Vector3 direction)
        {
            if (isDead || deathStarted || !canGainHealth || !gameplayRun) { return; }
            var hl = Mathf.Abs(health);
            life += hl;
            if (life > initialLife && !healthOverflow) { life = initialLife; }
            onDirectionalGainHealth?.Invoke(health, direction);
            OnDirectionalGainHealthEv?.Invoke(health, direction);
            OnDirectionalGainHealth(hl, direction);
            gainHealthParticle.Spawn(tr);
        }
    }
}