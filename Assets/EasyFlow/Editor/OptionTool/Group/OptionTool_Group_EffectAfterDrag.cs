using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace NAsoft_EasyFlow
{
	public partial class OptionTool : EditorWindow
	{
		static bool toggleEffectAfterDrag = false;

		private void GUIGroupEffectAfterDrag()
		{
			toggleEffectAfterDrag = GUILayout.Toggle(toggleEffectAfterDrag, "Effect After Drag", styleToggle, GUILayout.Height(titleHeight));
			if (toggleEffectAfterDrag)
			{
				GUILayout.Space(-5.0f);
				GUILayout.BeginVertical("HelpBox");
				{
					GUIIsMoveToNearCover();
					GUIIsEffectAfterDrag();
					EditorGUI.BeginDisabledGroup(saveData.isEffectAfterDrag == BOOL.No);
					{
						GUIEffectAfterDragTime();
						GUIEffectAfterDragCurve();
					}
					EditorGUI.EndDisabledGroup();
				}
				GUILayout.EndVertical();
				GUILayout.Space(spaceInMenu);
			}
			GUILayout.Space(spaceInMenu);
		}


		private void GUIIsMoveToNearCover()
		{
			GUIYesNo("Select - Is Move to Near Cover?", ref saveData.isMoveToNearCover);
		}

		private void GUIIsEffectAfterDrag()
		{
			GUIYesNo("Select - Is Play Effect After Drag?", ref saveData.isEffectAfterDrag);
		}

		private void GUIEffectAfterDragTime()
		{
			GUISlider("Select - Effect After Drag Time", "Time", 1, ref saveData.effectAfterDragTime, 0.0f, ref saveData.effectAfterDragTimeLimit);
		}

		private void GUIEffectAfterDragCurve()
		{
			GUICurve("Select - Effect After Drag Curve", ref saveData.effectAfterDragCurve, 80.0f);
		}
	}
}