using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GameplayFramework
{
    [CustomEditor(typeof(PlayerActor), true)]
    [CanEditMultipleObjects]
    public class PlayerActorEditor : ActorEditor
    {
        SerializedProperty useDeviceStorageForScore_prop, initialScore_prop, currentScore_prop, onUpdateScore_prop;

        bool shouldShowPlayerActorData = false;
        PlayerActor playerActorObject;
        protected override void StartEditor()
        {
            base.StartEditor();
            playerActorObject = (PlayerActor)target;
            useDeviceStorageForScore_prop = serializedObject.FindProperty("useDeviceStorageForScore");
            initialScore_prop = serializedObject.FindProperty("initialScore");
            currentScore_prop = serializedObject.FindProperty("currentScore");
            onUpdateScore_prop = serializedObject.FindProperty("onUpdateScore");
        }

        protected override void UpdateEditor()
        {
            base.UpdateEditor();

            shouldShowPlayerActorData = EditorGUILayout.Foldout(shouldShowPlayerActorData, "Player Actor Setting");
            if (shouldShowPlayerActorData)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(useDeviceStorageForScore_prop);
                EditorGUILayout.PropertyField(initialScore_prop);
                EditorGUILayout.PropertyField(currentScore_prop);
                EditorGUILayout.PropertyField(onUpdateScore_prop);
                EditorGUI.indentLevel--;
            }
        }
    }
}