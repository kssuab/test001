using UnityEngine;
using UnityEditor;
using System.Collections;

namespace NAsoft_EasyFlow
{
	[CustomEditor(typeof(EasyFlow))]
	public class EasyFlow_CustomInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			if (GUILayout.Button("Open - PresetTool"))
				PresetTool.OpenWindow(target as EasyFlow);
			
			if (GUILayout.Button("Open - OptionTool"))
				OptionTool.OpenWindow(target as EasyFlow);

			//DrawDefaultInspector();
		}
		private void OnDisable()
		{
		}

		private void OnEnable()
		{
			switch (SettingData.GetInstance().linkMode)
			{
			case LINK_MODE.Link:
				if (Selection.activeGameObject != null)
				{
					EasyFlow easyflow = Selection.activeGameObject.GetComponent<EasyFlow>();
					OptionTool.SetEasyflow(easyflow);
					PresetTool.SetEasyflow(easyflow);
				}
				else
				{
					OptionTool.SetEasyflow(null);
					PresetTool.SetEasyflow(null);
				}
				break;
				
			case LINK_MODE.OpenPresetTool:
				if (Selection.activeGameObject != null)
				{
					EasyFlow easyflow = Selection.activeGameObject.GetComponent<EasyFlow>();
					if (easyflow != null)
						PresetTool.OpenWindow(easyflow);
				}
				break;

			case LINK_MODE.OpenOptionTool:
				if (Selection.activeGameObject != null)
				{
					EasyFlow easyflow = Selection.activeGameObject.GetComponent<EasyFlow>();
					if (easyflow != null)
						OptionTool.OpenWindow(easyflow);
				}
				break;
			}
		}

		private void OnDestroy()
		{
		}
	}
}