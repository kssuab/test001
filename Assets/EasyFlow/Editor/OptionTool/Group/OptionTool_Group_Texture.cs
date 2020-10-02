using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace NAsoft_EasyFlow
{
    public partial class OptionTool : EditorWindow
    {
        private static bool toggleTexture = false;

        private void GUIGroupTexture()
        {
            toggleTexture = GUILayout.Toggle(toggleTexture, "Texture", styleToggle, GUILayout.Height(titleHeight));
            if (toggleTexture)
            {
                GUILayout.Space(-5.0f);
                GUILayout.BeginVertical("HelpBox");
                {
                    //GUI_Texture_Apply_Button();
                    EditorGUI.BeginChangeCheck();

                    GUITextureMode();
                    GUITextureList();

                    if (EditorGUI.EndChangeCheck())
                    {
                        if (selectedEasyflow != null)
                            selectedEasyflow.InitTexture();
                    }
                }
                GUILayout.EndVertical();
                GUILayout.Space(spaceInMenu);
            }
            GUILayout.Space(spaceInMenu);
        }

        private void GUITextureApplyButton()
        {
            if (GUILayout.Button("Apply : Change Cover Texture"))
                OnTextureChange();
        }

        private void GUITextureMode()
        {
            EditorGUILayout.LabelField("Select - Texture Mode", new GUIStyle("BoldLabel"), GUILayout.ExpandWidth(true));
            selectedEasyflow.saveData.textureMode = (TEXTURE_MODE)GUILayout.SelectionGrid((int)selectedEasyflow.saveData.textureMode,
                                                                                              new string[] { "Once", "Loop", "Random" }, 3, GUILayout.ExpandWidth(true));
        }

        private void GUITextureList()
        {
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(serializedPropertyTextureList, new GUIContent("Texture List"), true);

            if (EditorGUI.EndChangeCheck())
            {
                if (serializedObjectTextureList != null)
                {
                    serializedObjectTextureList.ApplyModifiedProperties();
#if UNITY_2017_or_Higher
                    serializedObjectTextureList.UpdateIfRequiredOrScript();
#else
                    serializedObjectTextureList.UpdateIfDirtyOrScript();
#endif
                }
            }
        }

        private void OnTextureChange()
        {
            selectedEasyflow.InitTexture();
        }
    }
}