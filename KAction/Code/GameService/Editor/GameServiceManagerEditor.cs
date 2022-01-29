using UnityEngine;
using UnityEditor;

namespace GameplayFramework
{
    [CustomEditor(typeof(GameServiceManager))]
    [CanEditMultipleObjects]
    public class GameServiceManagerEditor : Editor
    {
        GameServiceManager man;
        void OnEnable()
        {
            man = (GameServiceManager)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefaultInspector();
            man.LoadRefIfReq();
            var allSvcInScene = FindObjectsOfType<GameService>();
            if (allSvcInScene.Length > 0 && allSvcInScene.Length != man.services.Count)
            {
                EditorGUILayout.HelpBox("There are multiple game services in the scene. " +
                    "This is not allowed. Duplicates will not be added to execution list. ", MessageType.Warning);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}