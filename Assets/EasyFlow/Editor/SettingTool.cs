using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

namespace NAsoft_EasyFlow
{
	public class SettingTool : EditorWindow
	{
		SettingData settingData;

		[MenuItem("Window/NAsoft/EasyFlow/Setting Tool &%4", false, 4)]
		public static void OpenWindow()
		{
			SettingTool window = (SettingTool)EditorWindow.GetWindow(typeof(SettingTool), false, "EF:SettingTool", true);
			window.autoRepaintOnSceneChange = true;
			window.minSize = new Vector2(440.0f, 145.0f);
			window.settingData = SettingData.GetInstance();
		}
		
		private void OnEnable()
		{
			settingData = SettingData.GetInstance();
		}
		private void OnLostFocus()
		{
			EditorUtility.SetDirty(this);
		}
		private void OnDisable()
		{
			EditorUtility.SetDirty(this);
		}
		private void OnSelectionChange()
		{
			settingData = SettingData.GetInstance();
		}

		private void OnGUI()
		{
			EditorGUI.BeginChangeCheck();

			GUILinkMode();
			GUIDefaultCoverCount();

			if (EditorGUI.EndChangeCheck())
				EditorUtility.SetDirty(settingData);
		}


		private void GUILinkMode()
        {
            GUILayout.BeginVertical("HelpBox");
            {
                EditorGUILayout.LabelField("Select - Link Mode", new GUIStyle("BoldLabel"), GUILayout.ExpandWidth(true));
                settingData.linkMode = (LINK_MODE)GUILayout.SelectionGrid((int)settingData.linkMode,
                                                                            new string[] { "Link", "Open-PresetTool", "Open-OptionTool" }, 3, GUILayout.Height(50.0f));
            }
            GUILayout.EndVertical();
            GUILayout.Space(10.0f);
        }

		private void GUIDefaultCoverCount()
        {
            GUILayout.BeginVertical("HelpBox");
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Select - Default Cover Count", new GUIStyle("BoldLabel"), GUILayout.ExpandWidth(true));
                    EditorGUILayout.LabelField(string.Format("Count {0}", settingData.defaultCoverCount), GUILayout.ExpandWidth(true));
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    settingData.defaultCoverCount = (int)GUILayout.HorizontalSlider((float)settingData.defaultCoverCount, (float)0, (float)settingData.defaultCoverCountLimit, GUILayout.ExpandWidth(true));

                    string str = settingData.defaultCoverCountLimit.ToString();
                    settingData.defaultCoverCountLimit = EditorGUILayout.IntField(settingData.defaultCoverCountLimit, GUILayout.Width((str.Length + 1) * 7.0f));
                }
                EditorGUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }
	}
}