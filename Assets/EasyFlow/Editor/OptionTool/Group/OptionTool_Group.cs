using UnityEngine;
using UnityEditor;
using System.Collections;

namespace NAsoft_EasyFlow
{
	public partial class OptionTool : EditorWindow
	{
		static GUIStyle styleToggle = null;
		static float titleHeight = 20.0f;

		private void GUIGroup()
		{
			InitGroupStyle();

			GUILayout.BeginVertical("HelpBox");
			GUIGroupLock();

			if (saveData != null)
			{
				GUIGroupAll();
				GUILayout.EndVertical();

				GUIGroupCover();
                GUIGroupClick();
				GUIGroupTexture();
				GUIGroupDrag();
				GUIGroupEffectAfterDrag();
				GUIGroupPosition();
				GUIGroupRotate();
				GUIGroupScale();
				GUIGroupAlpha();
				GUIGroupDepth();
				GUIGroupOther();
			}
			else
			{
				GUISaveData();
				GUILayout.EndVertical();
			}
		}

		private void InitGroupStyle()
		{
			if (styleToggle == null)
			{
				styleToggle = new GUIStyle("sv_label_3");
				styleToggle.fixedHeight = titleHeight;
				styleToggle.alignment = TextAnchor.MiddleCenter;
				styleToggle.fontSize = 12;
			}
		}
	}
}