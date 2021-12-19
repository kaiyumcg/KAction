using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GameplayFramework
{
    [CustomEditor(typeof(ReactorActor), true)]
    [CanEditMultipleObjects]
    public class ReactorActorEditor : ActorEditor
    {
        SerializedProperty mode_prop, maxVolumeInteractionCount_prop, maxCollisionInteractionCount_prop,
            useVolume_prop, needVolumeExitEvent_prop, useCollider_prop, needHitStopEvent_prop, onEnter_prop, onHit_prop, onExit_prop, onStopHit_prop;

        bool shouldShowReactorData = false, showReactionEvents = false;
        ReactorActor reactor_Object;
        protected override void StartEditor()
        {
            base.StartEditor();
            reactor_Object = (ReactorActor)target;
            mode_prop = serializedObject.FindProperty("mode");
            maxVolumeInteractionCount_prop = serializedObject.FindProperty("maxVolumeInteractionCount");
            maxCollisionInteractionCount_prop = serializedObject.FindProperty("maxCollisionInteractionCount");
            useVolume_prop = serializedObject.FindProperty("useVolume");
            useCollider_prop = serializedObject.FindProperty("useCollider");
            onEnter_prop = serializedObject.FindProperty("onEnter");
            onHit_prop = serializedObject.FindProperty("onHit");

            needVolumeExitEvent_prop = serializedObject.FindProperty("needVolumeExitEvent");
            needHitStopEvent_prop = serializedObject.FindProperty("needHitStopEvent");
            onExit_prop = serializedObject.FindProperty("onExit");
            onStopHit_prop = serializedObject.FindProperty("onStopHit");
        }

        void DrawWarningIfInvalid()
        {
            var rgd = reactor_Object.GetComponent<Rigidbody>();
            var rgd2D = reactor_Object.GetComponent<Rigidbody2D>();
            var col = reactor_Object.GetComInActor<Collider>();
            var col2D = reactor_Object.GetComInActor<Collider2D>();

            var str = "";
            var valid = true;

            if (rgd == null && rgd2D == null)
            {
                str = "This actor(reactor) must have rigidbody attached to it!";
                valid = false;
            }
            else if (col == null && col2D == null)
            {
                str = "This actor(reactor) must have collider(s) attached to it/children that belongs to this actor!";
                valid = false;
            }

            if (valid == false)
            {
                EditorGUILayout.HelpBox(str, MessageType.Error);
            }
        }

        protected override void UpdateEditor()
        {
            base.UpdateEditor();
            shouldShowReactorData = EditorGUILayout.Foldout(shouldShowReactorData, "Reaction Setting");
            if (shouldShowReactorData)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(mode_prop);
                EditorGUILayout.PropertyField(maxVolumeInteractionCount_prop);
                EditorGUILayout.PropertyField(maxCollisionInteractionCount_prop);
                EditorGUILayout.PropertyField(useVolume_prop);
                EditorGUILayout.PropertyField(useCollider_prop);
                EditorGUILayout.PropertyField(needVolumeExitEvent_prop);
                EditorGUILayout.PropertyField(needHitStopEvent_prop);

                showReactionEvents = EditorGUILayout.Foldout(showReactionEvents, "Events");
                if (showReactionEvents)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(onEnter_prop);
                    EditorGUILayout.PropertyField(onHit_prop);
                    EditorGUILayout.PropertyField(onExit_prop);
                    EditorGUILayout.PropertyField(onStopHit_prop);
                    EditorGUI.indentLevel--;
                }
                EditorGUI.indentLevel--;
            }
            DrawWarningIfInvalid();
        }
    }
}