using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace NAsoft_EasyFlow
{
    public partial class CreateTool : EditorWindow
    {
        private void OnGUI()
        {
            GUISelectCreateMode();
            GUICreateButton();
        }

        private void GUISelectCreateMode()
        {
            GUILayout.BeginVertical("HelpBox");
            {
                EditorGUILayout.LabelField("Select - EasyFlow Mode", new GUIStyle("BoldLabel"), GUILayout.ExpandWidth(true));

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUI.BeginDisabledGroup(!Common.IsNGUIUsed());
                    {
                        if (GUILayout.Toggle(coverMode == COVER_MODE.NGUI, "NGUI - UITexture\nRequires NGUI(not free)", "Button", GUILayout.ExpandWidth(true), GUILayout.Height(50.0f)))
                            coverMode = COVER_MODE.NGUI;
                    }
                    EditorGUI.EndDisabledGroup();

                    EditorGUI.BeginDisabledGroup(!Common.IsUnity5_0orHigher());
                    {
                        if (GUILayout.Toggle(coverMode == COVER_MODE.Quad, "Unity - Texture(Quad)", "Button", GUILayout.ExpandWidth(true), GUILayout.Height(50.0f)))
                            coverMode = COVER_MODE.Quad;
                    }
                    EditorGUI.EndDisabledGroup();

                    EditorGUI.BeginDisabledGroup(!Common.IsUnity5_0orHigher());
                    {
                        if (GUILayout.Toggle(coverMode == COVER_MODE.UGUI, "UGUI", "Button", GUILayout.ExpandWidth(true), GUILayout.Height(50.0f)))
                            coverMode = COVER_MODE.UGUI;
                    }
                    EditorGUI.EndDisabledGroup();
                }
                EditorGUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
            GUILayout.Space(5.0f);
        }

        private void GUICreateButton()
        {
            EditorGUI.BeginDisabledGroup(coverMode == COVER_MODE.Disabled);
            {
                if (GUILayout.Button(string.Format("Create EasyFlow : {0}", coverMode), "LargeButton", GUILayout.ExpandWidth(true), GUILayout.Height(50.0f)))
                    OnCreateBtn(coverMode);
            }
            EditorGUI.EndDisabledGroup();
        }
    }
}