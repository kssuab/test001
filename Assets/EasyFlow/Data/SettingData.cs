using UnityEngine;
using System;
using System.IO;
using System.Collections;

namespace NAsoft_EasyFlow
{
	public enum LINK_MODE
	{
		Link = 0,
		OpenPresetTool,
		OpenOptionTool
	}

	[Serializable]
	public class SettingData : ScriptableObject
	{
		[SerializeField]
		public LINK_MODE linkMode;
		public int defaultCoverCount;
		public int defaultCoverCountLimit;

		public SettingData()
		{
			Init();
		}

		private void Init()
		{
			linkMode = LINK_MODE.Link;
			defaultCoverCount = 10;
			defaultCoverCountLimit = 100;
		}

		[NonSerialized]
		private static SettingData instance;
		public static SettingData GetInstance()
		{
			if (instance == null)
			{
// Create File in Editor
#if UNITY_EDITOR
				// File Exists(true) : Load file
				if (File.Exists(PathEF.settingDataFilePath))
				{
					//instance = Resources.Load<SettingData>("SettingData");
                    instance = (SettingData)Resources.Load("SettingData", typeof(SettingData));
                }
				// File Exists(false) : Create file
				else
				{
					// Directory Exists(false) : Create Directory
					if (Directory.Exists(PathEF.directoryPath) == false)
						Directory.CreateDirectory(PathEF.directoryPath);

					// Create SettingData
					instance = ScriptableObject.CreateInstance<SettingData>();
					UnityEditor.AssetDatabase.CreateAsset(instance, PathEF.settingDataAssetPath);
					UnityEditor.AssetDatabase.SaveAssets();
					UnityEditor.AssetDatabase.Refresh();
				}

// Load File in Other Version
#else
                instance = (SettingData)Resources.Load("PathEF.settingDataName", typeof(SettingData));
#endif
			}
			return instance;
		}
	}
}