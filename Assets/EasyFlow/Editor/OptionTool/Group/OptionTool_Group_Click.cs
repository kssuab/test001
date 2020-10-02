using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace NAsoft_EasyFlow
{
    public partial class OptionTool : EditorWindow
    {
        private bool toggleClick = false;
        private Vector2 clickScrollViewPos = Vector2.zero;
        private Vector2 callbackScrollViewPos = Vector2.zero;

        private static string[] clickModeStrs = { "None", "Move to", "Click center", "Click all", "Move to / Click Center", "Move to / Click all", "Custom" };

        private static GUIStyle onStyle = null;
        private static GUIStyle offStyle = null;
        private static GUIStyle onMiniStyle = null;
        private static GUIStyle offMiniStyle = null;
        private static GUIStyle nameStyle = null;

        public void GUIGroupClick()
        {
            UpdateClickStyle();

            toggleClick = GUILayout.Toggle(toggleClick, "Click", styleToggle, GUILayout.Height(titleHeight));
            if (toggleClick)
            {
                GUILayout.Space(-5.0f);
                GUILayout.BeginVertical("HelpBox");
                {
                    GUIClickMode();
                }
                GUILayout.EndVertical();
                GUILayout.Space(spaceInMenu);
            }
            GUILayout.Space(spaceInMenu);
        }

        private void UpdateClickStyle()
        {
            if (offStyle == null)
            {
                offStyle = new GUIStyle("Button");
                offStyle.margin = new RectOffset(0, 0, 0, 0);
                offStyle.padding = new RectOffset(0, 0, 0, 0);
                offStyle.fixedHeight = 16.0f;
            }
            if (onStyle == null)
            {
                onStyle = new GUIStyle("Button");
                onStyle.normal = offStyle.onActive;
                onStyle.margin = new RectOffset(0, 0, 0, 0);
                onStyle.padding = new RectOffset(0, 0, 0, 0);
                onStyle.fixedHeight = 16.0f;
            }

            if (offMiniStyle == null)
            {
                offMiniStyle = new GUIStyle("miniButton");
                offMiniStyle.margin = new RectOffset(0, 0, 0, 0);
                offMiniStyle.padding = new RectOffset(0, 0, 0, 0);
                offMiniStyle.fixedHeight = 14.0f;
            }
            if (onMiniStyle == null)
            {
                onMiniStyle = new GUIStyle("miniButton");
                onMiniStyle.normal = offMiniStyle.onActive;
                onMiniStyle.margin = new RectOffset(0, 0, 0, 0);
                onMiniStyle.padding = new RectOffset(0, 0, 0, 0);
                onMiniStyle.fixedHeight = 14.0f;
            }
            if (nameStyle == null)
            {
                nameStyle = new GUIStyle("TL LogicBar 1");
                nameStyle.margin = new RectOffset(0, 2, 1, 0);
                nameStyle.padding = new RectOffset(0, 0, 0, 0);
                nameStyle.alignment = TextAnchor.MiddleCenter;
                nameStyle.fixedHeight = 27.0f;
            }
        }

        private CLICK_MODE DrawClickModeGrid(CLICK_MODE clickMode, int index, int count, bool isMini)
        {
            CLICK_MODE newMode = clickMode;

            EditorGUILayout.BeginHorizontal();
            {
                for (int i = index; i < index + count; ++i)
                {
                    if (clickMode == (CLICK_MODE)i)
                        GUILayout.Button(clickModeStrs[i], isMini ? onMiniStyle : onStyle);
                    else
                    {
                        if (GUILayout.Button(clickModeStrs[i], isMini ? offMiniStyle : offStyle))
                            newMode = (CLICK_MODE)i;
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            return newMode;
        }

        private void GUIClickMode()
        {
            CLICK_MODE newMode = selectedEasyflow.saveData.clickMode;

            EditorGUI.BeginChangeCheck();
            {
                EditorGUILayout.LabelField("Select - Click Mode", new GUIStyle("BoldLabel"), GUILayout.ExpandWidth(true));

                newMode = DrawClickModeGrid(selectedEasyflow.saveData.clickMode, 0, 3, false);
                newMode = DrawClickModeGrid(newMode, 4, 3, false);
            }
            if (EditorGUI.EndChangeCheck())
            {
                if (newMode != selectedEasyflow.saveData.clickMode)
                {
                    selectedEasyflow.saveData.clickMode = newMode;
                    if (selectedEasyflow.saveData.clickMode != CLICK_MODE.Custom)
                    {
                        foreach (var v in selectedEasyflow.coverList)
                            v.SetClickMode(selectedEasyflow, selectedEasyflow.saveData.clickMode);
                    }
                }
            }

            if (selectedEasyflow.saveData.clickMode == CLICK_MODE.Custom)
            {
                GUILayout.Space(spaceInMenu);

                clickScrollViewPos = GUILayout.BeginScrollView(clickScrollViewPos, GUILayout.Height(200.0f));
                {
                    foreach (var v in selectedEasyflow.coverList)
                    {
                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Label(v.name, nameStyle, GUILayout.Width(76.0f));

                            GUILayout.BeginVertical();
                            {
                                newMode = DrawClickModeGrid(v.clickMode, 0, 3, true);
                                newMode = DrawClickModeGrid(newMode, 4, 2, true);
                                if (newMode != v.clickMode)
                                    v.SetClickMode(selectedEasyflow, newMode);
                            }
                            GUILayout.EndVertical();
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.Space(1.0f);
                    }
                }
                GUILayout.EndScrollView();
            }

            GUILayout.Space(spaceInMenu);

            if (selectedEasyflow.saveData.clickMode != CLICK_MODE.None)
            {
                EditorGUILayout.LabelField("Callback - OnClick", new GUIStyle("BoldLabel"), GUILayout.ExpandWidth(true));

                if (selectedEasyflow != null)
                {
                    float height = 18.0f * selectedEasyflow.callbackList.Count + 2.0f;
                    if (height > 100.0f)
                        height = 100.0f;
                    callbackScrollViewPos = EditorGUILayout.BeginScrollView(callbackScrollViewPos, GUILayout.Height(height));
                    {
                        foreach (var v in selectedEasyflow.callbackList)
                        {
                            Undo.RecordObject(selectedEasyflow, "NAsoft - Click Callback");
                            EditorGUILayout.BeginHorizontal(GUILayout.Height(16.0f));
                            {
                                EditorGUI.BeginChangeCheck();
                                {
                                    v.DrawTargetField(true, GUILayout.Height(16.0f));
                                    v.DrawMemberField();
                                }
                                if (EditorGUI.EndChangeCheck())
                                    EditorUtility.SetDirty(selectedEasyflow);

                                if (GUILayout.Button("X", GUILayout.Width(16.0f), GUILayout.Height(14.0f)))
                                {
                                    selectedEasyflow.callbackList.Remove(v);
                                    EditorGUILayout.EndHorizontal();
                                    break;
                                }
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                    EditorGUILayout.EndScrollView();
                }

                if (GUILayout.Button("Add new callback"))
                {
                    selectedEasyflow.callbackList.Add(new Connector(typeof(Cover), typeof(int), typeof(string)));
                }
            }
        }
    }
}