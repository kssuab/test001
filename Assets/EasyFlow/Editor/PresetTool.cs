using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace NAsoft_EasyFlow
{
    public partial class PresetTool : EditorWindow
    {
        static PresetTool window;
        public static EasyFlow selectedEasyflow = null;
        private static List<PresetData> presetList = null;

        [MenuItem("Window/NAsoft/EasyFlow/Preset Tool &%3", false, 3)]
        public static void OpenWindow()
        {
            OpenWindow(null);
        }
        public static void OpenWindow(EasyFlow easyflow)
        {
            window = (PresetTool)EditorWindow.GetWindow(typeof(PresetTool), false, "EF:PresetTool", true);
            window.autoRepaintOnSceneChange = true;
            window.minSize = new Vector2(440.0f, 145.0f);
            PresetTool.SetEasyflow(easyflow);
        }

        private void OnEnable()
        {
            Reset();
        }
        private void OnSelectionChange()
        {
            Reset();
        }
        private void OnFocus()
        {
            Reset();
        }
        private void OnLostFocus()
        {
        }
        private void OnDisable()
        {
        }

        private void OnProjectChange()
        {
        }

        private void Reset()
        {
            if (Selection.activeGameObject == null ||
               (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<EasyFlow>() == null))
                PresetTool.SetEasyflow(null);

            ResetPresetList();
            Repaint();
        }

        private void ResetPresetList()
        {
            UnityEngine.Object[] objList = Resources.LoadAll("Preset", typeof(PresetData));
            presetList = new List<PresetData>(objList.Length);
            foreach (PresetData presetData in objList)
                presetList.Add(presetData);
        }

        public static void SetEasyflow(EasyFlow easyflow)
        {
            selectedEasyflow = easyflow;

            if (window != null)
                window.Repaint();
        }

        private void OnGUI()
        {
            if (selectedEasyflow == null)
            {
                EditorGUILayout.LabelField(string.Format("EasyFlow Selected - [Not selected]"),
                                           new GUIStyle("BoldLabel"), GUILayout.ExpandWidth(true));
                return;
            }

            EditorGUILayout.LabelField(string.Format("EasyFlow Selected - [{0} : in {1} Cover]",
                                                     selectedEasyflow.gameObject.name, selectedEasyflow.coverList.Count),
                                       new GUIStyle("BoldLabel"), GUILayout.ExpandWidth(true));
            GUIPresetButton();
            CheckUndoRedoEvent();
        }

        private Vector2 scrollViewPos = Vector2.zero;
        private static GUIStyle buttonStyle = null;
        private void GUIPresetButton()
        {
            if (presetList == null || presetList.Count == 0)
                return;

            int xCount = (int)(Screen.width / 100.0f);
            if (xCount > presetList.Count)
                xCount = presetList.Count;
            float size = Screen.width / xCount;

            if (buttonStyle == null)
            {
                buttonStyle = new GUIStyle("Button");
                buttonStyle.margin = new RectOffset(0, 0, 0, 0);
                buttonStyle.padding = new RectOffset(3, 3, 3, 3);
                //buttonStyle.border = new RectOffset(0,0,0,0);
            }

            scrollViewPos = GUILayout.BeginScrollView(scrollViewPos, false, false, GUIStyle.none, GUIStyle.none);
            {
                int index = 0;
                GUIContent content = null;
                for (int i = 0; i < presetList.Count; ++i)
                {
                    GUILayout.BeginHorizontal();
                    {
                        for (int k = 0; k < xCount; ++k)
                        {
                            index = (i * xCount) + k;
                            if (index >= presetList.Count)
                                break;

                            PresetData presetData = presetList[index];
                            if (presetData.texture == null)
                                content = new GUIContent(presetData.name);
                            else
                                content = new GUIContent(presetData.texture);
                            if (GUILayout.Button(content, buttonStyle, GUILayout.Width(size), GUILayout.Height(size)))
                                OnBtn(presetData);
                        }
                    }
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndScrollView();
        }

        private void OnBtn(PresetData presetData)
        {
            if (presetData != null)
            {
                Undo.RecordObject(selectedEasyflow.saveData, "NAsoft_EasyFlow_Undo_Point");

                presetData.LoadToSaveData(ref selectedEasyflow.saveData);
                selectedEasyflow.Init();
                selectedEasyflow.Init();
                selectedEasyflow.InitTexture();
                EditorUtility.SetDirty(selectedEasyflow);
                EditorUtility.SetDirty(selectedEasyflow.saveData);
            }
        }

        private void CheckUndoRedoEvent()
        {
            if (Event.current.type == EventType.ValidateCommand)
            {
                if (Event.current.commandName.CompareTo("UndoRedoPerformed") == 0)
                {
                    if (selectedEasyflow != null)
                    {
                        selectedEasyflow.Init();
                    }
                }
            }
        }
    }
}