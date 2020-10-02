using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace NAsoft_EasyFlow
{
	public partial class OptionTool : EditorWindow
	{
		static bool toggleRotate = false;
		
		private void GUIGroupRotate()
		{
			toggleRotate = GUILayout.Toggle(toggleRotate, "Near Effect - Rotate", styleToggle, GUILayout.Height(titleHeight));
			if (toggleRotate)
			{
				GUILayout.Space(-5.0f);
				GUILayout.BeginVertical("HelpBox");
				{
					GUIIsLookatCenter();
					GUIIsRotateOnAxis();
					GUILayout.Space(spaceInMenu);
					
					EditorGUI.BeginDisabledGroup(saveData.isLookatCenter == BOOL.No);
					{
						GUIRotateRate();
						GUIRotateInfluenceRange();
						GUIRotateCurve();
					}
					EditorGUI.EndDisabledGroup();
				}
				GUILayout.EndVertical();
				GUILayout.Space(spaceInMenu);
			}
			GUILayout.Space(spaceInMenu);
		}


		private void GUIIsLookatCenter()
		{
			EditorGUI.BeginChangeCheck();
			
			GUIYesNo("Select - Is Lookat Center?", ref saveData.isLookatCenter);
			
			if (EditorGUI.EndChangeCheck())
			{
				if (selectedEasyflow != null)
					selectedEasyflow.Init();
			}
		}

		private void GUIIsRotateOnAxis()
		{
			EditorGUI.BeginChangeCheck();
			
			GUIYesNo("Select - Is Rotate on Axis?", ref saveData.isRotateOnAxis);
			
			if (EditorGUI.EndChangeCheck())
			{
				if (selectedEasyflow != null)
					selectedEasyflow.Init();
			}
		}

		private void GUIRotateRate()
		{
			EditorGUI.BeginChangeCheck();
			
			GUISlider("Select - Rotate Rate", "Rate", ref saveData.rotateRate, 0.0f, ref saveData.rotateRateLimit);
			
			if (EditorGUI.EndChangeCheck())
			{
				if (selectedEasyflow != null)
					selectedEasyflow.Init();
			}
		}

		private void GUIRotateInfluenceRange()
		{
			EditorGUI.BeginChangeCheck();

			GUISlider("Select - Rotate Influence Range", "Range", ref saveData.rotateInfluenceRange, 0.0f, ref saveData.rotateInfluenceRangeLimit);
			
			if (EditorGUI.EndChangeCheck())
			{
				if (selectedEasyflow != null)
					selectedEasyflow.Init();
			}
		}

		private void GUIRotateCurve()
		{
			GUICurve("Select - Rotate Curve", ref saveData.rotateCurve, 80.0f);
		}
	}
}