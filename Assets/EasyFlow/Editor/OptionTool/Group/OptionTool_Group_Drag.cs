using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace NAsoft_EasyFlow
{
    public partial class OptionTool : EditorWindow
    {
        private static bool toggleDrag = false;

        private void GUIGroupDrag()
        {
            toggleDrag = GUILayout.Toggle(toggleDrag, "Drag", styleToggle, GUILayout.Height(titleHeight));
            if (toggleDrag)
            {
                GUILayout.Space(-5.0f);
                GUILayout.BeginVertical("HelpBox");
                {
                    GUIDragMode();
                    GUILayout.Space(spaceInMenu);
                    GUIDragPower();
                    GUIIsInverseDrag();
                    //					GUIIsDragOnAxis();
                }
                GUILayout.EndVertical();
                GUILayout.Space(spaceInMenu);
            }
            GUILayout.Space(spaceInMenu);
        }

        private void GUIDragMode()
        {
            GUITitle("Selected - Drag Mode", "Mode", saveData.dragMode);

            // bug - Mode: On Collider - Loss data(after build)
            saveData.dragMode = (DRAG_MODE)GUILayout.SelectionGrid((int)saveData.dragMode,
                                                                     new string[] { "OnScreen(Ratio)", "OnCollider" }, 2, GUILayout.ExpandWidth(true));
            /*saveData.dragMode = DRAG_MODE.OnScreen;
			GUILayout.BeginHorizontal();
			{
				GUILayout.Toggle(true, "      On Screen(Ratio)", new GUIStyle("Button"), GUILayout.ExpandWidth(true));
				EditorGUI.BeginDisabledGroup(true);
				{
					GUILayout.Toggle(false, "On Collider(Camera)", new GUIStyle("Button"), GUILayout.ExpandWidth(true));
				}
				EditorGUI.EndDisabledGroup();
			}
			GUILayout.EndHorizontal();
			*/

            if (saveData.dragMode == DRAG_MODE.OnScreen)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Label("X"); saveData.dragRect.x = EditorGUILayout.FloatField(saveData.dragRect.x, GUILayout.ExpandWidth(true));
                    GUILayout.Space(25.0f);
                    GUILayout.Label("Y"); saveData.dragRect.y = EditorGUILayout.FloatField(saveData.dragRect.y, GUILayout.ExpandWidth(true));
                    GUILayout.Space(25.0f);
                    GUILayout.Label("W"); saveData.dragRect.width = EditorGUILayout.FloatField(saveData.dragRect.width, GUILayout.ExpandWidth(true));
                    GUILayout.Space(25.0f);
                    GUILayout.Label("H"); saveData.dragRect.height = EditorGUILayout.FloatField(saveData.dragRect.height, GUILayout.ExpandWidth(true));
                }
                EditorGUILayout.EndHorizontal();
            }
            if (saveData.dragMode == DRAG_MODE.OnCollider)
            {
                var width = Screen.width / 7.0f;
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Camera", GUILayout.Width(width));
                    selectedEasyflow.dragCamera = (Camera)EditorGUILayout.ObjectField(selectedEasyflow.dragCamera, typeof(Camera), true);

                    GUILayout.Space(2.0f);

                    GUILayout.Label("Collider", GUILayout.Width(width));
                    selectedEasyflow.dragCollider = (Collider)EditorGUILayout.ObjectField(selectedEasyflow.dragCollider, typeof(Collider), true);
                }
                EditorGUILayout.EndHorizontal();
                if (EditorGUI.EndChangeCheck())
                {
                    EditorUtility.SetDirty(saveData);
                }
            }
        }

        private void GUIDragPower()
        {
            GUISlider("Select - Drag Power", "Power", 1, ref saveData.dragPower, 0.0f, ref saveData.dragPowerLimit);
        }

        private void GUIIsInverseDrag()
        {
            GUIYesNo("Select - Is Inverse Drag?", ref saveData.isInverseDrag);
        }

        private void GUIIsDragOnAxis()
        {
            GUIYesNo("Select - Is Drag on Axis?", ref saveData.isDragOnAxis);
        }
    }
}