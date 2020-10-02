using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;

namespace NAsoft_EasyFlow
{
    public partial class CreateTool : EditorWindow
    {
        static CreateTool window;

        static COVER_MODE coverMode;

        [MenuItem("Window/NAsoft/EasyFlow/Create Tool &%1", false, 1)]
        public static void OpenWindow()
        {
            window = (CreateTool)EditorWindow.GetWindow(typeof(CreateTool), false, "EF:CreateTool", true);
            window.autoRepaintOnSceneChange = true;
            window.minSize = new Vector2(600.0f, 145.0f);
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
            Repaint();
        }

        private void Reset()
        {
            Common.UpdateDefine();

            coverMode = COVER_MODE.Disabled;

            Repaint();
        }

    }
}