using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;

namespace GameplayFramework
{
    internal class SettingWindow : EditorWindow
    {
        bool multiPlayerSupport = false, loggingSupport = false, cloudLogSupport = false;
        string helpMsg;
        const string multiplayerDef = "KML_SUPPORT";
        const string loggingDef = "KLOG_SUPPORT";
        const string cloudLogDef = "USE_CLOUD_LOG";
        [MenuItem("Tool/Game Setting/Open Setting Window")]
        static void Init()
        {
            SettingWindow window = (SettingWindow)EditorWindow.GetWindow(typeof(SettingWindow));
            window.multiPlayerSupport = EditorPrefs.GetBool("_KF_Multiplayer_Support", false);
            window.loggingSupport = EditorPrefs.GetBool("_KF_Log_Support", false);
            window.cloudLogSupport = EditorPrefs.GetBool("_KF_CloudLog_Support", false);
            window.titleContent = new GUIContent("Game Setting");
            window.helpMsg = "To use cloud log, you must define API end point by either setting it in your GameLevel class inspector's Logging section Or" +
                " overriding GameLevel's 'OnDefineCloudLogUploadAPIEndPoint()' method";
            window.Show();
        }

        void OnGUI()
        {
            GUILayout.Label("Project Settings", EditorStyles.boldLabel);
            multiPlayerSupport = EditorGUILayout.Toggle("MultiPlayer Support? ", multiPlayerSupport);
            loggingSupport = EditorGUILayout.Toggle("Logging Support? ", loggingSupport);
            EditorGUILayout.HelpBox(helpMsg, MessageType.Info);
            cloudLogSupport = EditorGUILayout.Toggle("Cloud Log Support? ", cloudLogSupport);

            if (GUILayout.Button("Apply"))
            {
                ApplySetting();
            }
        }

        void ApplySetting()
        {
            EditorPrefs.SetBool("_KF_Multiplayer_Support", multiPlayerSupport);
            EditorPrefs.SetBool("_KF_Log_Support", loggingSupport);
            EditorPrefs.SetBool("_KF_CloudLog_Support", cloudLogSupport);

            var symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            var syms = Regex.Split(symbols, ";");
            List<string> targetSymbols = new List<string>();
            targetSymbols.AddRange(syms);
            targetSymbols.RemoveAll((str) => { return string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str) || str == ";"; });
            ProcessScriptingDefine(multiPlayerSupport, ref targetSymbols, multiplayerDef);
            ProcessScriptingDefine(loggingSupport, ref targetSymbols, loggingDef);
            ProcessScriptingDefine(cloudLogSupport, ref targetSymbols, cloudLogDef);

            var targetStr = "";
            if (targetSymbols != null && targetSymbols.Count > 0)
            {
                for (int i = 0; i < targetSymbols.Count; i++)
                {
                    if (i == targetSymbols.Count - 1)
                    {
                        targetStr += targetSymbols[i];
                    }
                    else
                    {
                        targetStr += targetSymbols[i] + ";";
                    }
                }
            }
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, targetStr);
        }

        void ProcessScriptingDefine(bool hasIt, ref List<string> defines, string featureDefine)
        {
            if (hasIt)
            {
                if (defines.Contains(featureDefine) == false)
                {
                    defines.Add(featureDefine);
                }
            }
            else
            {
                if (defines.Contains(featureDefine))
                {
                    defines.Remove(featureDefine);
                }
            }
        }

        private void OnDestroy()
        {
            ApplySetting();
        }
    }
}