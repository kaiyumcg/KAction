using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GameplayFramework
{
    [CustomEditor(typeof(GameActor), true)]
    [CanEditMultipleObjects]
    public class GameActorEd : Editor
    {
        SerializedProperty gameplayComponents_prop, life_prop, timeScale_prop, onStartOrSpawn_prop,
            onStartDeath_prop, onDeath_prop, onDamage_prop, onGainHealth_prop;

        bool shouldShowActorData = false;
        GameActor actorObject;
        protected virtual void StartEditor() { }
        protected virtual void UpdateEditor() { }
        void OnEnable()
        {
            actorObject = (GameActor)target;
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
                    actorObject.ReloadComponents();
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

            actorObject.OnEditorUpdate();
            UpdateEditor();
            DrawDefaultInspector();
            serializedObject.ApplyModifiedProperties();
        }
    }
}