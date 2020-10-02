using UnityEngine;
using UnityEditor;
using System.Collections;

namespace NAsoft_EasyFlow
{
	[CustomEditor(typeof(PresetData))]
	public class PresetData_CustomInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			PresetData presetData = serializedObject.targetObject as PresetData;
			if(presetData == null)
				return;


			EditorGUI.BeginChangeCheck();

			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField("PresetTool Texture", GUILayout.Width(110.0f));
				presetData.texture = EditorGUILayout.ObjectField(presetData.texture, typeof(Texture2D), false) as Texture2D;
			}
			EditorGUILayout.EndHorizontal();

			if(EditorGUI.EndChangeCheck())
				EditorUtility.SetDirty(presetData);

			float size = ((Screen.width > Screen.height) ? Screen.height : Screen.width) - 30.0f;
			EditorGUILayout.LabelField(new GUIContent(presetData.texture), GUILayout.Width(size), GUILayout.Height(size));
		}
	}
}