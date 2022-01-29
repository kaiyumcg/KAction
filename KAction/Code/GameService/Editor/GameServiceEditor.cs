using UnityEngine;
using UnityEditor;

namespace GameplayFramework
{
    [CustomEditor(typeof(GameService), true)]
    [CanEditMultipleObjects]
    public class GameServiceEditor : Editor
    {
        SerializedProperty serviceAbout_prop, serviceDescription_prop, waitForThisInitialization_prop, useDelay_prop,
            delayAmount_prop, whenError_prop, whenException_prop, whenCodeFailure_prop;
        bool overrideDefault = false;
        GameService service;
        protected GameService Service { get { return service; } }
        protected virtual void StartEditor() { }
        protected virtual void UpdateEditor() { }
        void OnEnable()
        {
            service = (GameService)target;
            serviceAbout_prop = serializedObject.FindProperty("serviceAbout");
            serviceDescription_prop = serializedObject.FindProperty("serviceDescription");
            waitForThisInitialization_prop = serializedObject.FindProperty("waitForThisInitialization");
            useDelay_prop = serializedObject.FindProperty("useDelay");
            delayAmount_prop = serializedObject.FindProperty("delayAmount");
            whenError_prop = serializedObject.FindProperty("whenError");
            whenException_prop = serializedObject.FindProperty("whenException");
            whenCodeFailure_prop = serializedObject.FindProperty("whenCodeFailure");
            StartEditor();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.LabelField("This is a Game Service", EditorStyles.boldLabel);
            DrawDefaultInspector();
            overrideDefault = EditorGUILayout.Toggle("Edit Default", overrideDefault);
            if (overrideDefault)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(serviceAbout_prop);
                EditorGUILayout.PropertyField(serviceDescription_prop);
                EditorGUILayout.PropertyField(waitForThisInitialization_prop);
                EditorGUILayout.PropertyField(useDelay_prop);
                EditorGUILayout.PropertyField(delayAmount_prop);
                GUILayout.Space(10);
                EditorGUILayout.LabelField("What to do in case of following. (Only valid when logging enabled)");
                EditorGUILayout.PropertyField(whenError_prop);
                EditorGUILayout.PropertyField(whenException_prop);
                EditorGUILayout.PropertyField(whenCodeFailure_prop);
                EditorGUI.indentLevel--;
            }
            UpdateEditor();
            serializedObject.ApplyModifiedProperties();
        }
    }
}