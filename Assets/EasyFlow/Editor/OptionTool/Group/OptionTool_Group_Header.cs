using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace NAsoft_EasyFlow
{
    public partial class OptionTool : EditorWindow
    {
        private static bool toggleAll = false;

        private void GUISelectedEasyFlow()
        {
            if (selectedEasyflow == null)
                EditorGUILayout.LabelField(string.Format("EasyFlow Selected - [Not selected]"), new GUIStyle("BoldLabel"), GUILayout.ExpandWidth(true));
            else
                EditorGUILayout.LabelField(string.Format("EasyFlow Selected - [{0} : in {1} Cover]",
                                                         selectedEasyflow.gameObject.name, selectedEasyflow.coverList.Count),
                                           new GUIStyle("BoldLabel"), GUILayout.ExpandWidth(true));
        }

        private void GUIGroupLock()
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Is Lock Selected EasyFlow?", new GUIStyle("BoldLabel"), GUILayout.ExpandWidth(true));

                EditorGUI.BeginChangeCheck();

                isLock = GUILayout.Toggle(isLock, isLock ? "Locked" : "Unlocked", new GUIStyle("Button"), GUILayout.ExpandWidth(true));

                if (EditorGUI.EndChangeCheck())
                    Reset();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void GUIGroupAll()
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("All Open / All Close", new GUIStyle("BoldLabel"), GUILayout.ExpandWidth(true));

                EditorGUI.BeginChangeCheck();

                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Open", GUILayout.ExpandWidth(true)))
                        toggleAll = true;
                    if (GUILayout.Button("Close", GUILayout.ExpandWidth(true)))
                        toggleAll = false;
                }
                GUILayout.EndHorizontal();

                if (EditorGUI.EndChangeCheck())
                {
                    toggleCover = toggleAll;
                    toggleClick = toggleAll;
                    toggleTexture = toggleAll;
                    toggleDrag = toggleAll;
                    toggleEffectAfterDrag = toggleAll;
                    togglePosition = toggleAll;
                    toggleRotate = toggleAll;
                    toggleScale = toggleAll;
                    toggleAlpha = toggleAll;
                    toggleDepth = toggleAll;
                    toggleOther = toggleAll;
                }
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(spaceInMenu);
        }
    }
}