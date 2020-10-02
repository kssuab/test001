using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace NAsoft_EasyFlow
{
	public partial class OptionTool : EditorWindow
	{
		static bool toggleDepth = false;
		
		private void GUIGroupDepth()
		{
			toggleDepth = GUILayout.Toggle(toggleDepth, "Depth", styleToggle, GUILayout.Height(titleHeight));
			if (toggleDepth)
			{
				GUILayout.Space(-5.0f);
				GUILayout.BeginVertical("HelpBox");
				{
					//GUIPanelDepth();

					EditorGUI.BeginDisabledGroup(saveData.coverDistanceZMode != COVER_DISTANCE_Z_MODE.Disable);
					{
						GUICoverDepthRange();
						GUIDepthInfluenceRange();
						GUIDepthCurve();
					}
					EditorGUI.EndDisabledGroup();
				}
				GUILayout.EndVertical();
				GUILayout.Space(spaceInMenu);
			}
			GUILayout.Space(spaceInMenu);
		}


		private void GUIPanelDepth()
		{
			EditorGUI.BeginChangeCheck();
			
			GUISlider("Select - Panel Depth", "Depth", ref saveData.panelDepth, ref saveData.panelDepthLimit.x, ref saveData.panelDepthLimit.y);
			
			if (EditorGUI.EndChangeCheck())
			{
				if (selectedEasyflow != null)
					selectedEasyflow.Init();
			}
		}

		private void GUICoverDepthRange()
		{
			GUISlider("Select - Cover Depth Range", "Min", "Max", 0, ref saveData.coverDepth.x, ref saveData.coverDepth.y, ref saveData.coverDepthLimit.x, ref saveData.coverDepthLimit.y);
		}

		private void GUIDepthInfluenceRange()
		{
			EditorGUI.BeginChangeCheck();
			
			GUISlider("Select - Depth Influence Range", "Range", ref saveData.depthInfluenceRange, 0.0f, ref saveData.depthInfluenceRangeLimit);
			
			if (EditorGUI.EndChangeCheck())
			{
				if (selectedEasyflow != null)
					selectedEasyflow.Init();
			}
		}

		private void GUIDepthCurve()
		{
			GUICurve("Select - Depth Curve", ref saveData.depthCurve, 80.0f);
		}
	}
}