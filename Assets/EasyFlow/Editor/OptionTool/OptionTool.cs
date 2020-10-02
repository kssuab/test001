using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

namespace NAsoft_EasyFlow
{
    public partial class OptionTool : EditorWindow
    {
        private static bool isLock = false;
        private static float spaceInMenu = 3.0f;
        public static EasyFlow selectedEasyflow = null;
        private static SaveData saveData;
        private static OptionTool window = null;

        private static SerializedObject serializedObjectTextureList = null;
        private static SerializedProperty serializedPropertyTextureList = null;

        private static List<Texture2D> coverDistanceZModeTexutreList = null;

        private static Vector2 scrollPos = Vector2.zero;

        private void OnGUI()
        {
            CheckLockState();

            EditorGUI.BeginChangeCheck();

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUIStyle.none, GUIStyle.none);
            {
                GUISelectedEasyFlow();

                if (selectedEasyflow != null)
                {
                    if (saveData != null)
                    {
                        Undo.RecordObject(saveData, "NAsoft_EasyFlow_Undo_Point");
                        GUIGroup();
                    }
                }
            }
            EditorGUILayout.EndScrollView();

            if (EditorGUI.EndChangeCheck() == false)
            {
                if (saveData != null)
                    Undo.ClearUndo(saveData);
            }
            else
            {
                if (saveData != null)
                    EditorUtility.SetDirty(saveData);
            }

            CheckUndoRedoEvent();
        }

        private void CheckLockState()
        {
            if (isLock && selectedEasyflow == null)
                isLock = false;
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
                        selectedEasyflow.InitTexture();
                    }
                }
            }
        }

        [MenuItem("Window/NAsoft/EasyFlow/Option Tool &%2", false, 2)]
        public static void OpenWindow()
        {
            OpenWindow(null);
        }

        public static void OpenWindow(EasyFlow easyflow)
        {
            window = (OptionTool)EditorWindow.GetWindow(typeof(OptionTool), false, "EF:OptionTool", true);
            window.autoRepaintOnSceneChange = true;
            window.minSize = new Vector2(440.0f, 145.0f);
            OptionTool.SetEasyflow(easyflow);
        }

        private void OnEnable()
        {
            window = this;
            Reset();
        }

        private void OnSelectionChange()
        {
            window = this;
            Reset();
        }

        private void OnFocus()
        {
            window = this;
            Reset();
        }

        private void OnLostFocus()
        {
            if (saveData != null)
                EditorUtility.SetDirty(saveData);
        }

        private void OnDisable()
        {
            if (saveData != null)
                EditorUtility.SetDirty(saveData);
        }

        private void OnProjectChange()
        {
            Repaint();
        }

        private static void Reset()
        {
            if (Selection.activeGameObject == null ||
               (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<EasyFlow>() == null))
                OptionTool.SetEasyflow(null);

            Common.UpdateDefine();
        }

        public static void SetEasyflow(EasyFlow easyflow)
        {
            if (isLock)
                return;

            selectedEasyflow = easyflow;
            if (selectedEasyflow != null)
            {
                saveData = selectedEasyflow.saveData;
                ResetTextureList();
            }

            if (window != null)
                window.Repaint();
        }

        private static void ResetTextureList()
        {
            if (selectedEasyflow != null &&
               selectedEasyflow.saveData != null)
            {
                serializedObjectTextureList = new SerializedObject(selectedEasyflow.saveData);
                serializedPropertyTextureList = serializedObjectTextureList.FindProperty("textureList");
            }

            UnityEngine.Object[] objList = Resources.LoadAll("TextureCoverDistanceZMode", typeof(Texture2D));
            coverDistanceZModeTexutreList = new List<Texture2D>(objList.Length);
            foreach (Texture2D texture2D in objList)
                coverDistanceZModeTexutreList.Add(texture2D);
        }
    }
}