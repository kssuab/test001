using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace NAsoft_EasyFlow
{
	public partial class OptionTool : EditorWindow
	{
		static bool toggleAlpha = false;
		
		private void GUIGroupAlpha()
		{
			toggleAlpha = GUILayout.Toggle(toggleAlpha, "Near Effect - Alpha", styleToggle, GUILayout.Height(titleHeight));
			if (toggleAlpha)
			{
				GUILayout.Space(-5.0f);
				GUILayout.BeginVertical("HelpBox");
				{
					GUIAlphaRate();
					GUIAlphaInfluenceRange();
					GUIAlphaCurve();
				}
				GUILayout.EndVertical();
				GUILayout.Space(spaceInMenu);
			}
			GUILayout.Space(spaceInMenu);
		}


		private void GUIAlphaRate()
		{
			EditorGUI.BeginChangeCheck();
			
			GUISlider("Select - Alpha Rate", "Rate", 2, ref saveData.alphaRate, 0.0f, ref saveData.alphaRateLimit);
			
			if (EditorGUI.EndChangeCheck())
			{
				if (selectedEasyflow != null)
					selectedEasyflow.Init();
			}
		}

		private void GUIAlphaInfluenceRange()
		{
			EditorGUI.BeginChangeCheck();
			
			GUISlider("Select - Alpha Influence Range", "Range", ref saveData.alphaInfluenceRange, 0.0f, ref saveData.alphaInfluenceRangeLimit);
			
			if (EditorGUI.EndChangeCheck())
			{
				if (selectedEasyflow != null)
					selectedEasyflow.Init();
			}
		}

		private void GUIAlphaCurve()
		{
			GUICurve("Select - Alpha Curve", ref saveData.alphaCurve, 80.0f);
		}
	}
}