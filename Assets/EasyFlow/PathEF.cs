using UnityEngine;
using System.Collections;

namespace NAsoft_EasyFlow
{
	public static class PathEF
	{
		public static string directoryPath = Application.dataPath + "/EasyFlow/Resources";
		public static string assetPath = "Assets/EasyFlow/Resources";

		public static string settingDataName = "SettingData";
		public static string settingDataFilePath = string.Format("{0}/{1}.asset", directoryPath, settingDataName);
		public static string settingDataAssetPath = string.Format("{0}/{1}.asset", assetPath, settingDataName);

		static PathEF()
		{
		}
	}
}