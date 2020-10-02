using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace NAsoft_EasyFlow
{
	public partial class OptionTool : EditorWindow
	{
		static bool toggleScale = false;
		
		private void GUIGroupScale()
		{
			toggleScale = GUILayout.Toggle(toggleScale, "Near Effect - Scale", styleToggle, GUILayout.Height(titleHeight));
			if (toggleScale)
			{
				GUILayout.Space(-5.0f);
				GUILayout.BeginVertical("HelpBox");
				{
					GUIScaleRate();
					GUIScaleInfluenceRange();
					GUIScaleCurve();
				}
				GUILayout.EndVertical();
				GUILayout.Space(spaceInMenu);
			}
			GUILayout.Space(spaceInMenu);
		}


		private void GUIScaleRate()
		{
			EditorGUI.BeginChangeCheck();
			
			GUISlider("Select - Scale Rate", "Rate", 2, ref saveData.scaleRate, ref saveData.scaleRateLimit.x, ref saveData.scaleRateLimit.y);
			
			if (EditorGUI.EndChangeCheck())
			{
				if (selectedEasyflow != null)
					selectedEasyflow.Init();
			}
		}

		private void GUIScaleInfluenceRange()
		{
			EditorGUI.BeginChangeCheck();
			
			GUISlider("Select - Scale Influence Range", "Range", ref saveData.scaleInfluenceRange, 0.0f, ref saveData.scaleInfluenceRangeLimit);
			
			if (EditorGUI.EndChangeCheck())
			{
				if (selectedEasyflow != null)
					selectedEasyflow.Init();
			}
		}

		private void GUIScaleCurve()
		{
			GUICurve("Select - Scale Curve", ref saveData.scaleCurve, 80.0f);
		}
	}
}