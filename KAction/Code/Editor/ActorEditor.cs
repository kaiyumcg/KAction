using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GameplayFramework
{
    [CustomEditor(typeof(Actor), true)]
    [CanEditMultipleObjects]
    public class ActorEditor : Editor
    {
        SerializedProperty gameplayComponents_prop, life_prop, timeScale_prop, onStartOrSpawn_prop,
            onStartDeath_prop, onDeath_prop, onDamage_prop, onGainHealth_prop;

        bool shouldShowActorData = false;
        Actor actorObject;
        protected virtual void StartEditor() { }
        protected virtual void UpdateEditor() { }
        void OnEnable()
        {
            actorObject = (Actor)target;
            gameplayComponents_prop = serializedObject.FindProperty("gameplayComponents");
            life_prop = serializedObject.FindProperty("life");
            timeScale_prop = serializedObject.FindProperty("timeScale");
            onStartOrSpawn_prop = serializedObject.FindProperty("onStartOrSpawn");

            onStartDeath_prop = serializedObject.FindProperty("onStartDeath");
            onDeath_prop = serializedObject.FindProperty("onDeath");
            onDamage_prop = serializedObject.FindProperty("onDamage");
            onGainHealth_prop = serializedObject.FindProperty("onGainHealth");
            StartEditor();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            shouldShowActorData = EditorGUILayout.Foldout(shouldShowActorData, "Actor Setting");
            if (shouldShowActorData)
            {
                EditorGUI.indentLevel++;
                if (GUILayout.Button("Fetch components"))
                {
                    
                }
                EditorGUILayout.PropertyField(gameplayComponents_prop);
                EditorGUILayout.PropertyField(life_prop);
                EditorGUILayout.PropertyField(timeScale_prop);
                EditorGUILayout.PropertyField(onStartOrSpawn_prop);
                EditorGUILayout.PropertyField(onStartDeath_prop);
                EditorGUILayout.PropertyField(onDeath_prop);
                EditorGUILayout.PropertyField(onDamage_prop);
                EditorGUILayout.PropertyField(onGainHealth_prop);
                EditorGUI.indentLevel--;
            }
            UpdateEditor();
            DrawDefaultInspector();
            serializedObject.ApplyModifiedProperties();
        }

        //New Code

        /*
        SerializedProperty isPlayer_prop, tags_prop, life_prop, deathParticle_prop, damageParticle_prop, gainHealthParticle_prop, rebornParticle_prop, 
        canRecieveDamage_prop, canGainHealth_prop, healthOverflow_prop, onStartDeath_prop, onDeath_prop,
        onDamage_prop, onGainHealth_prop, onReborn_prop, timeScale_prop;

    bool shouldShowActorData = false, showParticleOptions = false, showActorEvents = false;
    Actor actorObject;
    protected virtual void StartEditor() { }
    protected virtual void UpdateEditor() { }
    void OnEnable()
    {
        actorObject = (Actor)target;

        isPlayer_prop = serializedObject.FindProperty("isPlayer");
        tags_prop = serializedObject.FindProperty("tags");
        life_prop = serializedObject.FindProperty("life");

        deathParticle_prop = serializedObject.FindProperty("deathParticle");
        damageParticle_prop = serializedObject.FindProperty("damageParticle");
        gainHealthParticle_prop = serializedObject.FindProperty("gainHealthParticle");
        rebornParticle_prop = serializedObject.FindProperty("rebornParticle");
        canRecieveDamage_prop = serializedObject.FindProperty("canRecieveDamage");
        canGainHealth_prop = serializedObject.FindProperty("canGainHealth");
        healthOverflow_prop = serializedObject.FindProperty("healthOverflow");
        onStartDeath_prop = serializedObject.FindProperty("onStartDeath");
        onDeath_prop = serializedObject.FindProperty("onDeath");
        onDamage_prop = serializedObject.FindProperty("onDamage");
        onGainHealth_prop = serializedObject.FindProperty("onGainHealth");
        onReborn_prop = serializedObject.FindProperty("onReborn");
        timeScale_prop = serializedObject.FindProperty("timeScale");
        StartEditor();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        //DrawDefaultInspector();
        DrawPropertiesExcluding(serializedObject, new string[] { "isPlayer", "tags", "life",
            "deathParticle", "damageParticle", "gainHealthParticle", "rebornParticle", "canRecieveDamage", "canGainHealth",
            "healthOverflow", "onStartDeath", "onDeath", "onDamage", "onGainHealth", "onReborn", "timeScale" });

        shouldShowActorData = EditorGUILayout.Foldout(shouldShowActorData, "Actor Setting");
        if (shouldShowActorData)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(isPlayer_prop);
            EditorGUILayout.PropertyField(tags_prop);
            EditorGUILayout.PropertyField(life_prop);
            EditorGUILayout.PropertyField(canRecieveDamage_prop);
            EditorGUILayout.PropertyField(canGainHealth_prop);
            EditorGUILayout.PropertyField(healthOverflow_prop);
            EditorGUILayout.PropertyField(timeScale_prop);

            showParticleOptions = EditorGUILayout.Foldout(showParticleOptions, "Particle Options");
            if (showParticleOptions)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(deathParticle_prop);
                EditorGUILayout.PropertyField(damageParticle_prop);
                EditorGUILayout.PropertyField(gainHealthParticle_prop);
                EditorGUILayout.PropertyField(rebornParticle_prop);
                EditorGUI.indentLevel--;
            }

            showActorEvents = EditorGUILayout.Foldout(showActorEvents, "Events");
            if (showActorEvents)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(onStartDeath_prop);
                EditorGUILayout.PropertyField(onDeath_prop);
                EditorGUILayout.PropertyField(onDamage_prop);
                EditorGUILayout.PropertyField(onGainHealth_prop);
                EditorGUILayout.PropertyField(onReborn_prop);
                EditorGUI.indentLevel--;
            }
            
            EditorGUI.indentLevel--;
        }

        actorObject.OnEditorUpdate();
        UpdateEditor();
        serializedObject.ApplyModifiedProperties();
    }

        */
    }
}