using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace NAsoft_EasyFlow
{
	public partial class OptionTool : EditorWindow
	{
		static bool toggleOther = false;
		
		private void GUIGroupOther()
		{
			toggleOther = GUILayout.Toggle(toggleOther, "Other", styleToggle, GUILayout.Height(titleHeight));
			if (toggleOther)
			{
				GUILayout.Space(-5.0f);
				GUILayout.BeginVertical("HelpBox");
				{
					GUISaveData();
					if (saveData != null)
					{
						GUISavePreset();
						GUILayout.Space(spaceInMenu);
						GUIBeginFlowIndex();
						//GUIIsMoveTarget();
					}
				}
				GUILayout.EndVertical();
			}
		}


		private void GUISaveData()
		{
			EditorGUI.BeginChangeCheck();
			
			GUILayout.Label("SaveData", "BoldLabel");
			saveData = (SaveData)EditorGUILayout.ObjectField(saveData, typeof(SaveData), false);
			
			if (EditorGUI.EndChangeCheck())
			{
				selectedEasyflow.saveData = saveData;
				selectedEasyflow.Init();
				ResetTextureList();
			}
		}
		
		private void GUISavePreset()
		{
			if (GUILayout.Button("Save To Preset", GUILayout.ExpandWidth(true)))
				PresetData.Save(saveData);
		}

		private void GUIBeginFlowIndex()
		{
			EditorGUI.BeginChangeCheck();
			
			GUISlider("Select - Begin Flow Index", "Index", ref saveData.beginFlowIndex, 0, (selectedEasyflow.coverList.Count-1));
			
			if (EditorGUI.EndChangeCheck())
			{
				if (selectedEasyflow != null)
				{
					selectedEasyflow.Init();
					selectedEasyflow.Init();
				}
			}
		}

		private void GUIIsMoveTarget()
		{
			EditorGUI.BeginChangeCheck();
			
			GUIYesNo("Select - Move Target", "Move Cover", "Move Panel", ref saveData.isPanelMove);
			
			if (EditorGUI.EndChangeCheck())
			{
				if (selectedEasyflow != null)
					selectedEasyflow.ChangeMoveTarget();
			}
		}
	}
}