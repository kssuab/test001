using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace NAsoft_EasyFlow
{
    public partial class CreateTool : EditorWindow
    {
        private void OnCreateBtn(COVER_MODE coverMode)
        {
            GameObject root = CreateRoot(coverMode);
            GameObject camera = CreateCamera(coverMode, root);
            GameObject panel = CreatePanel(coverMode, root, camera);
            EasyFlow easyflow = CreateEasyFlow(coverMode, root, panel);
            CreateCover(coverMode, easyflow, panel);
            OptionTool.OpenWindow(easyflow);
        }

        private GameObject CreateRoot(COVER_MODE coverMode)
        {
            GameObject root = null;
            switch (coverMode)
            {
                case COVER_MODE.NGUI:
                    root = CreateRoot_NGUI();
                    break;

                case COVER_MODE.UGUI:
                    root = CreateRoot_UGUI();
                    break;

                case COVER_MODE.Quad:
                    root = CreateRoot_UnityTexture();
                    break;
            }

            root.name = string.Format("EasyFlow : {0}", root.GetInstanceID());
            Selection.activeObject = root;

            return root;
        }
        private GameObject CreateRoot_NGUI()
        {
#if NGUI_USED
            UIRoot uiroot = NGUITools.AddChild<UIRoot>(null);
            GameObject root = uiroot.gameObject;
            return root;
#else
			return null;
#endif
        }
        private GameObject CreateRoot_UGUI()
        {
            GameObject root = new GameObject("Root");
            return root;
        }
        private GameObject CreateRoot_UnityTexture()
        {
            GameObject root = new GameObject("Root");
            return root;
        }


        private GameObject CreateCamera(COVER_MODE coverMode, GameObject root)
        {
            GameObject camera = null;
            switch (coverMode)
            {
                case COVER_MODE.NGUI: camera = CreateCamera_NGUI(root); break;
                case COVER_MODE.UGUI: camera = CreateCamera_UGUI(root); break;
                case COVER_MODE.Quad: camera = CreateCamera_UnityTexture(root); break;
            }

            return camera;
        }
        private GameObject CreateCamera_NGUI(GameObject root)
        {
#if NGUI_USED
            UICamera camera = NGUITools.AddChild<UICamera>(root);
            camera.transform.localPosition = new Vector3(0.0f, 0.0f, -300.0f);
            return camera.gameObject;
#else
			return null;
#endif
        }
        private GameObject CreateCamera_UGUI(GameObject root)
        {
            GameObject camera = new GameObject("Camera");
            camera.AddComponent<Camera>();
            camera.transform.localPosition = new Vector3(0.0f, 0.0f, -300.0f);
            camera.transform.parent = root.transform;
            return camera.gameObject;
        }
        private GameObject CreateCamera_UnityTexture(GameObject root)
        {
            GameObject camera = new GameObject("Camera");
            camera.AddComponent<Camera>();
            camera.transform.localPosition = new Vector3(0.0f, 0.0f, -300.0f);
            camera.transform.parent = root.transform;
            return camera.gameObject;
        }

        private GameObject CreatePanel(COVER_MODE coverMode, GameObject root, GameObject camera)
        {
            GameObject panel = null;
            switch (coverMode)
            {
                case COVER_MODE.NGUI: panel = CreatePanel_NGUI(); break;
                case COVER_MODE.UGUI: panel = CreatePanel_UGUI(camera.GetComponent<Camera>()); break;
                case COVER_MODE.Quad: panel = CreatePanel_UnityTexture(); break;
            }

            panel.name = "Panel";
            panel.transform.parent = root.transform;
            return panel;
        }

        private GameObject CreatePanel_NGUI()
        {
#if NGUI_USED
            UIPanel uiPanel = NGUITools.AddChild<UIPanel>(null);
            return uiPanel.gameObject;
#else
			return null;
#endif
        }
        private GameObject CreatePanel_UGUI(Camera camera)
        {
#if UGUI_USED
            GameObject panel = new GameObject();
            panel.AddComponent<RectTransform>();

            Canvas canvas = panel.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = camera;

            panel.AddComponent<CanvasGroup>();

            return panel;
#else
            return null;
#endif
        }
        private GameObject CreatePanel_UnityTexture()
        {
            GameObject panel = new GameObject();
            return panel;
        }

        private EasyFlow CreateEasyFlow(COVER_MODE coverMode, GameObject root, GameObject panel)
        {
            EasyFlow easyflow = null;
            switch (coverMode)
            {
                case COVER_MODE.NGUI: easyflow = root.AddComponent<EasyFlow_NGUI>(); break;
                case COVER_MODE.UGUI: easyflow = root.AddComponent<EasyFlow_UGUI>(); break;
                case COVER_MODE.Quad: easyflow = root.AddComponent<EasyFlow_Quad>(); break;
            }

            easyflow.panel = panel.transform;
            easyflow.easyflowCamera = root.GetComponentInChildren<Camera>();
            CreateSaveData(coverMode, easyflow);
            return easyflow;
        }

        private void CreateCover(COVER_MODE coverMode, EasyFlow easyflow, GameObject panel)
        {
            easyflow.saveData.coverMode = coverMode;
            easyflow.ChangeCoverCount(SettingData.GetInstance().defaultCoverCount);
        }

        private void CreateSaveData(COVER_MODE coverMode, EasyFlow easyflow)
        {
            // Create SaveData instance
            easyflow.saveData = ScriptableObject.CreateInstance<SaveData>();
            easyflow.saveData.coverMode = coverMode;
            easyflow.saveData.coverCount = SettingData.GetInstance().defaultCoverCount;

            // Load Sample Textures ("Assets/EasyFlow/Resources/TextureSample")
            UnityEngine.Object[] objList = Resources.LoadAll("TextureSample", typeof(Texture));
            if (objList != null)
            {
                easyflow.saveData.textureList = new List<Texture>(objList.Length);
                foreach (Texture texture in objList)
                    easyflow.saveData.textureList.Add(texture);
            }

            string directoryPath = string.Format("{0}/SaveData", PathEF.assetPath);

            // Directory Exists(false) : Create Directory
            if (Directory.Exists(directoryPath) == false)
                Directory.CreateDirectory(directoryPath);

            // Save SaveData file
            UnityEditor.AssetDatabase.CreateAsset(easyflow.saveData, string.Format("{0}/SaveData_{1}.asset", directoryPath, easyflow.GetInstanceID()));
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
        }
    }
}