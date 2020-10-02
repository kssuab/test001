using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace NAsoft_EasyFlow
{
	public partial class OptionTool : EditorWindow
	{
		static bool togglePosition = false;
		
		private void GUIGroupPosition()
		{
			EditorGUI.BeginDisabledGroup(saveData.isPanelMove == BOOL.Yes);
			{
				togglePosition = GUILayout.Toggle(togglePosition, saveData.isPanelMove == BOOL.Yes
				                                  ? "Near Effect - Position (Move Panel Mode - in Other Tab)" : "Near Effect - Position",
				                                  styleToggle, GUILayout.Height(titleHeight));
			}
			EditorGUI.EndDisabledGroup();

			if (saveData.isPanelMove != BOOL.Yes)
				if (togglePosition)
			{
				GUILayout.Space(-5.0f);
				GUILayout.BeginVertical("HelpBox");
				{
					GUIPositionRate();
					GUIPositionInfluenceRange();
					GUIPositionCurve();
				}
				GUILayout.EndVertical();
				GUILayout.Space(spaceInMenu);
			}
			
			GUILayout.Space(spaceInMenu);
		}


		private void GUIPositionRate()
		{
			EditorGUI.BeginChangeCheck();
			
			GUISlider("Select - Position Rate", "Rate", 1, ref saveData.positionRate, 0.0f, ref saveData.positionRateLimit);
			
			if (EditorGUI.EndChangeCheck())
			{
				if (selectedEasyflow != null)
					selectedEasyflow.Init();
			}
		}

		private void GUIPositionInfluenceRange()
		{
			EditorGUI.BeginChangeCheck();
			
			GUISlider("Select - Position Influence Range", "Range", ref saveData.positionInfluenceRange, 0.0f, ref saveData.positionInfluenceRangeLimit);
			
			if (EditorGUI.EndChangeCheck())
			{
				if (selectedEasyflow != null)
					selectedEasyflow.Init();
			}
		}

		private void GUIPositionCurve()
		{
			GUICurve("Select - Position Curve", ref saveData.positionCurve, 80.0f);
		}
	}
}