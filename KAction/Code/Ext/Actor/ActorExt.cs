using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public static partial class ActorExt
    {
        public static Coroutine Wait(this Actor actor, float amountInScaledTime, System.Action OnComplete) 
        {
            return _Wait(actor, amountInScaledTime, OnComplete);
        }
        public static void SetTickForGameplayComponents(this Actor actor, bool tick) 
        {
            _SetTickForGameplayComponents(actor, tick);
        }
        public static void SetTick(this Actor actor, bool tick) 
        {
            _SetTick(actor, tick);
        }
        public static bool HasAnyTaggedRelation(this Actor actor, Actor otherActor, bool useRefOptimization = false) 
        {
            return _HasAnyTaggedRelation(actor, otherActor, useRefOptimization);
        }
        public static bool IsTaggedDescendantOf(this Actor actor, Actor otherActor, bool useRefOptimization = false) 
        {
            return _IsTaggedDescendantOf(actor, otherActor, useRefOptimization);
        }
        public static bool IsTaggedAncestorOf(this Actor actor, Actor otherActor, bool useRefOptimization = false) 
        {
            return _IsTaggedAncestorOf(actor, otherActor, useRefOptimization);
        }
        public static bool HasCommonTaggedAncestor(this Actor actor, Actor otherActor, bool useRefOptimization = false) 
        {
            return _HasCommonTaggedAncestor(actor, otherActor, useRefOptimization);
        }
        public static bool HasAtleastOneTagOf(this Actor actor, Actor otherActor, bool useRefOptimization = false) 
        {
            return _HasAtleastOneTagOf(actor, otherActor, useRefOptimization);
        }
        public static void AddUniqueTag(this Actor actor, GameplayTag tag, bool useRefOptimization = false) 
        {
            _AddUniqueTag(actor, tag, useRefOptimization);
        }
        public static void Pause(this Actor actor, bool ignoreChildControlToggle = false) { _Pause(actor, ignoreChildControlToggle); }
        public static void Resume(this Actor actor, bool ignoreChildControlToggle = false) { _Resume(actor, ignoreChildControlToggle); }

        public static void Born(this Actor actor) { _Born(actor); }
        public static void Reborn(this Actor actor) { _Reborn(actor); }
        public static void Murder(this Actor actor, bool obliterate = false) { _Murder(actor, obliterate); }
        public static void Damage(this Actor actor, float damage) { _Damage(actor, damage); }
        public static void DamageFromDirection(this Actor actor, float damage, Vector3 direction) { _DamageFromDirection(actor, damage, direction); }
        public static void AddHealth(this Actor actor, float health) { _AddHealth(actor, health); }
        public static void AddDirectionalHealth(this Actor actor, float health, Vector3 direction) { _AddDirectionalHealth(actor, health, direction); }

        public static T GetGameplayComponent<T>(this Actor actor) where T : GameplayComponent { return _GetGameplayComponent<T>(actor); }
        public static List<T> GetGameplayComponents<T>(this Actor actor) where T : GameplayComponent { return _GetGameplayComponents<T>(actor); }
        public static T GetGameplayComponentByTag<T>(this Actor actor, GameplayTag tag, bool useRefOptimization = false) where T : GameplayComponent 
        {
            return _GetGameplayComponentByTag<T>(actor, tag, useRefOptimization);
        }
        public static List<T> GetGameplayComponentsByTag<T>(this Actor actor, GameplayTag tag, bool useRefOptimization = false) where T : GameplayComponent
        {
            return _GetGameplayComponentsByTag<T>(actor, tag, useRefOptimization);
        }
        public static T AddActorComponent<T>(this Actor actor) where T : GameplayComponent
        {
            return _AddActorComponent<T>(actor);
        }
        public static void RemoveActorComponent<T>(this Actor actor, T t) where T : GameplayComponent
        {
            _RemoveActorComponent<T>(actor, t);
        }

        public static T GetData<T>(this Actor actor, string key) where T : ActorData { throw new System.NotImplementedException(); }
        public static void SetData<T>(this Actor actor, string key, T data) where T : ActorData { }
        public static void WriteDataToDisk<T>(this Actor actor, string key, string devUniqueName) where T : ActorData { }
        public static void LoadDataFromDisk<T>(this Actor actor, string key, string devUniqueName) where T : ActorData { }
        public static void ClearDataTable(this Actor actor) { }

        //todo visibility
        //todo parent child actors
        //todo data layer
    }
}