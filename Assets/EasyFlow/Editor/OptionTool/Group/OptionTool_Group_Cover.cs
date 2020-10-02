using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace NAsoft_EasyFlow
{
	public partial class OptionTool : EditorWindow
	{
		private bool toggleCover = false;

		public void GUIGroupCover()
		{
			toggleCover = GUILayout.Toggle(toggleCover, "Cover", styleToggle, GUILayout.Height(titleHeight));
			if (toggleCover)
			{
				GUILayout.Space(-5.0f);
				GUILayout.BeginVertical("HelpBox");
				{
					GUICoverCount();
					GUICoverDistance();
					GUILayout.Space(spaceInMenu*2.0f);
					GUICoverSize();
					GUILayout.Space(spaceInMenu);
					GUICoverDistanceZ();
				}
				GUILayout.EndVertical();
				GUILayout.Space(spaceInMenu);
			}
			GUILayout.Space(spaceInMenu);
		}


		private void GUICoverCount()
		{
			EditorGUI.BeginChangeCheck();
			
			GUISlider("Select - Cover Count", "Count", ref saveData.coverCount, 0, ref saveData.coverCountLimit);
			
			if (EditorGUI.EndChangeCheck())
				selectedEasyflow.ChangeCoverCount(saveData.coverCount);
		}

		private void GUICoverDistance()
		{
			GUITitle("Select - Cover Angle/Distance", "X", "Y", saveData.coverDistance.x, saveData.coverDistance.y);
			
			GUILayout.Space(2.0f);
			
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.BeginHorizontal();
			{
				GUILayout.Space(10.0f);
				GUILayout.Box("", GUILayout.Width(100.0f), GUILayout.Height(100.0f));
				Rect rect = GUILayoutUtility.GetLastRect();
				GUI.Box(rect, "", new GUIStyle("flow overlay box"));
				GUI.Box(new Rect(rect.x,rect.y,rect.width*0.5f,rect.height*0.5f), "");
				GUI.Box(new Rect(rect.center.x,rect.y,rect.width*0.5f,rect.height*0.5f), "");
				GUI.Box(new Rect(rect.center.x,rect.center.y,rect.width*0.5f,rect.height*0.5f), "");
				GUI.Box(new Rect(rect.x,rect.center.y,rect.width*0.5f,rect.height*0.5f), "");
				
				Matrix4x4 mat = GUI.matrix;
				GUIUtility.RotateAroundPivot(-saveData.coverAngle, rect.center);
				Rect clockRect = new Rect(rect.center.x-10.0f, rect.center.y-4.0f, 58.0f, 8.0f);
				GUIStyle style = new GUIStyle("flow varPin tooltip");
				style.fixedHeight = 8.0f;
				GUI.Box(clockRect, "", style);
				GUI.matrix = mat;
				
				if (Event.current.type == EventType.MouseDown ||
				   Event.current.type == EventType.MouseDrag)
				{
					if (rect.Contains(Event.current.mousePosition))
					{
						Vector2 gap = Event.current.mousePosition - rect.center;
						saveData.coverAngle = -Mathf.Atan2(gap.y, gap.x) * Mathf.Rad2Deg;
						GUI.changed = true;
						Repaint();
					}
				}
				EditorGUILayout.BeginVertical();
				{
					GUILayout.Space(10.0f);
					
					GUILayout.Label(string.Format("Angle: {0}", saveData.coverAngle));
					saveData.coverAngle = Mathf.Round(GUILayout.HorizontalSlider(saveData.coverAngle, -180.0f, 180.0f, GUILayout.ExpandWidth(true)));
					
					GUILayout.Space(10.0f);
					
					
					GUILayout.Label(string.Format("Distance XY: {0}", saveData.coverDistanceF));
					EditorGUILayout.BeginHorizontal();
					{
						saveData.coverDistanceF = Mathf.Round(GUILayout.HorizontalSlider(saveData.coverDistanceF, 0.0f, saveData.coverDistanceLimit, GUILayout.ExpandWidth(true)));
						string str = saveData.coverDistanceLimit.ToString();
						saveData.coverDistanceLimit = EditorGUILayout.FloatField(saveData.coverDistanceLimit, GUILayout.Width((str.Length+1) * 7.0f));
					}
					EditorGUILayout.EndHorizontal();
				}
				EditorGUILayout.EndVertical();
			}
			EditorGUILayout.EndHorizontal();
			
			// is changed
			if (EditorGUI.EndChangeCheck())
			{
				if (selectedEasyflow != null)
					selectedEasyflow.Init();
			}
		}

		private void GUICoverSize()
		{
			EditorGUI.BeginChangeCheck();
			
			GUISlider("Select - Cover Size", "X", "Y", ref saveData.coverSize, ref saveData.coverSizeLimit);
			
			if (EditorGUI.EndChangeCheck())
				selectedEasyflow.ChangeCoverSize(saveData.coverSize);
		}

		static GUIStyle coverDistanceZModeButtonStyle = null;
		static float coverDistanceZModeButtonXSize = 80.0f;
		static float coverDistanceZModeButtonYSize = 45.0f;
		private void GUICoverDistanceZ()
		{
			if (coverDistanceZModeButtonStyle == null)
			{
				coverDistanceZModeButtonStyle = new GUIStyle("Button");
				coverDistanceZModeButtonStyle.margin = new RectOffset(0,0,0,0);
				coverDistanceZModeButtonStyle.padding = new RectOffset(3,3,3,3);
				//coverDistanceZModeButtonStyle.border = new RectOffset(0,0,0,0);
			}

			EditorGUI.BeginChangeCheck();

			if (saveData.isPanelMove == BOOL.Yes)
				GUILayout.Label("Select - Cover Distance Z Mode (MoveTarget:Panel)", new GUIStyle("BoldLabel"));
			else
				GUILayout.Label("Select - Cover Distance Z Mode", new GUIStyle("BoldLabel"));

			coverDistanceZModeButtonXSize = (Screen.width-16.0f) / 5.0f - 1.0f;
			coverDistanceZModeButtonYSize = coverDistanceZModeButtonXSize / 1.9f;

			GUILayout.BeginHorizontal();
			{
				saveData.coverDistanceZMode = GUILayout.Toggle(saveData.coverDistanceZMode == COVER_DISTANCE_Z_MODE.Disable,
				                                               coverDistanceZModeTexutreList[0], coverDistanceZModeButtonStyle,
				                                               GUILayout.Width(coverDistanceZModeButtonXSize),
				                                               GUILayout.Height(coverDistanceZModeButtonYSize)) ? COVER_DISTANCE_Z_MODE.Disable : saveData.coverDistanceZMode;
				saveData.coverDistanceZMode = GUILayout.Toggle(saveData.coverDistanceZMode == COVER_DISTANCE_Z_MODE.Forward,
				                                               coverDistanceZModeTexutreList[1], coverDistanceZModeButtonStyle,
				                                               GUILayout.Width(coverDistanceZModeButtonXSize),
				                                               GUILayout.Height(coverDistanceZModeButtonYSize)) ? COVER_DISTANCE_Z_MODE.Forward : saveData.coverDistanceZMode;
				saveData.coverDistanceZMode = GUILayout.Toggle(saveData.coverDistanceZMode == COVER_DISTANCE_Z_MODE.Backward,
				                                               coverDistanceZModeTexutreList[2], coverDistanceZModeButtonStyle,
				                                               GUILayout.Width(coverDistanceZModeButtonXSize),
				                                               GUILayout.Height(coverDistanceZModeButtonYSize)) ? COVER_DISTANCE_Z_MODE.Backward : saveData.coverDistanceZMode;
				EditorGUI.BeginDisabledGroup(saveData.isPanelMove == BOOL.Yes);
				{
					saveData.coverDistanceZMode = GUILayout.Toggle(saveData.coverDistanceZMode == COVER_DISTANCE_Z_MODE.Far,
					                                               coverDistanceZModeTexutreList[3], coverDistanceZModeButtonStyle,
					                                               GUILayout.Width(coverDistanceZModeButtonXSize),
					                                               GUILayout.Height(coverDistanceZModeButtonYSize)) ? COVER_DISTANCE_Z_MODE.Far : saveData.coverDistanceZMode;
					saveData.coverDistanceZMode = GUILayout.Toggle(saveData.coverDistanceZMode == COVER_DISTANCE_Z_MODE.Near,
					                                               coverDistanceZModeTexutreList[4], coverDistanceZModeButtonStyle,
					                                               GUILayout.Width(coverDistanceZModeButtonXSize),
					                                               GUILayout.Height(coverDistanceZModeButtonYSize)) ? COVER_DISTANCE_Z_MODE.Near : saveData.coverDistanceZMode;
				}
				EditorGUI.EndDisabledGroup();
			}
			GUILayout.EndHorizontal();

			EditorGUI.BeginDisabledGroup(saveData.coverDistanceZMode == COVER_DISTANCE_Z_MODE.Disable);
				GUISlider("Select - Cover Distance Z", "Z", ref saveData.coverDistanceZ, 0.0f, ref saveData.coverDistanceZLimit);
			EditorGUI.EndDisabledGroup();

			if (EditorGUI.EndChangeCheck())
			{
				selectedEasyflow.Init();
				Repaint();
			}
		}
	}
}