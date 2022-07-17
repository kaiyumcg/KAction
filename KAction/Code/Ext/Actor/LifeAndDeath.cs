using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameplayFramework
{
    public static partial class ActorExt
    {
        static void _Reborn(Actor actor)
        {
            if (!actor.isDead || !actor.gameplayRun || actor.deathStarted) { return; }
            actor.StopAllCoroutines();
            actor._GameObject.SetActive(true);
            _Born(actor);
            actor.onReborn?.Invoke();
            actor.OnRebornEv_Invoke();
            actor.OnReborn();
            actor.rebornParticle.Spawn(actor._Transform);
        }

        static void _Born(Actor actor)
        {
            actor.isDead = actor.deathStarted = actor.isActorPaused = false;
            actor.life = actor.initialLife;
            actor.timeScale = actor.initialTimeScale;
            actor.OnStart();
            actor.StartCoroutine(actor.OnStartAsync());
        }

        static void _Murder(Actor actor, bool obliterate)
        {
            if (actor.isDead || actor.deathStarted || !actor.canRecieveDamage || !actor.gameplayRun) { return; }
            _DeathProc(actor, obliterate);
        }

        static void _DeathProc(Actor actor, bool destroyCompletely)
        {
            actor.deathStarted = true;
            actor.life = 0.0f;
            actor.StartCoroutine(DeathProcAsync());
            IEnumerator DeathProcAsync()
            {
                actor.deathParticle.Spawn(actor._Transform);
                actor.onStartDeath?.Invoke();
                actor.OnStartDeathEv_Invoke();
                actor.OnStartDeath();
                yield return actor.StartCoroutine(actor.OnStartDeathAsync());
                actor.onDeath?.Invoke();
                actor.OnDeathEv_Invoke();
                actor.OnDeath();
                actor.isDead = true;
                actor._GameObject.SetActive(false);

                if (destroyCompletely)
                {
                    ActorLevelModule.instance.OnDestroyActorCompletely(actor);
                    GameObject.Destroy(actor._GameObject);
                }
                else
                {
                    ActorLevelModule.FreeActor(actor);
                }
            }
        }

        static void _Damage(Actor actor, float damage)
        {
            if (actor.isDead || actor.deathStarted || !actor.canRecieveDamage || !actor.gameplayRun) { return; }
            var dm = Mathf.Abs(damage);
            actor.life -= dm;
            actor.onDamage?.Invoke(dm);
            actor.OnDamageEv_Invoke(dm);
            actor.OnDamage(dm);
            actor.damageParticle.Spawn(actor._Transform);
            if (actor.life <= 0.0f)
            {
                _DeathProc(actor, false);
            }
        }

        static void _DamageFromDirection(Actor actor, float damage, Vector3 direction)
        {
            if (actor.isDead || actor.deathStarted || !actor.canRecieveDamage || !actor.gameplayRun) { return; }
            var dm = Mathf.Abs(damage);
            actor.life -= dm;
            actor.onDirectionalDamage?.Invoke(dm, direction);
            actor.OnDirectionalDamageEv_Invoke(dm, direction);
            actor.OnDirectionalDamage(dm, direction);
            actor.damageParticle.Spawn(actor._Transform);
            if (actor.life <= 0.0f)
            {
                _DeathProc(actor, false);
            }
        }

        static void _AddHealth(Actor actor, float health)
        {
            if (actor.isDead || actor.deathStarted || !actor.canGainHealth || !actor.gameplayRun) { return; }
            var hl = Mathf.Abs(health);
            actor.life += hl;
            if (actor.life > actor.initialLife && !actor.healthOverflow) { actor.life = actor.initialLife; }
            actor.onGainHealth?.Invoke(health);
            actor.OnGainHealthEv_Invoke(health);
            actor.OnGainHealth(hl);
            actor.gainHealthParticle.Spawn(actor._Transform);
        }

        static void _AddDirectionalHealth(Actor actor, float health, Vector3 direction)
        {
            if (actor.isDead || actor.deathStarted || !actor.canGainHealth || !actor.gameplayRun) { return; }
            var hl = Mathf.Abs(health);
            actor.life += hl;
            if (actor.life > actor.initialLife && !actor.healthOverflow) { actor.life = actor.initialLife; }
            actor.onDirectionalGainHealth?.Invoke(health, direction);
            actor.OnDirectionalGainHealthEv_Invoke(health, direction);
            actor.OnDirectionalGainHealth(hl, direction);
            actor.gainHealthParticle.Spawn(actor._Transform);
        }
    }
}