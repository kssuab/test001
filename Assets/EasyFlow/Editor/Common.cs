using UnityEngine;
using UnityEditor;
using System.Collections;

namespace NAsoft_EasyFlow
{
    public class Common : ScriptableObject
    {
        public static bool IsNGUIUsed()
        {
#if NGUI_USED
            return true;
#else
            return false;
#endif
        }

        public static bool IsUnity5_0orHigher()
        {
#if UNITY_5_0_or_Higher
            return true;
#else
            return false;
#endif
        }

        public static void GetUnityVersion(out int first, out int second)
        {
            string version = Application.unityVersion;
            string[] splitStr = version.Split('.');
            first = int.Parse(splitStr[0]);
            second = int.Parse(splitStr[1]);
        }

        [InitializeOnLoadMethod]
        public static void UpdateDefine()
        {
            UpdateDefineUnityVerstion();
            UpdateDefineNGUI();
            UpdateDefineUGUI();
        }

        private static void UpdateDefineUnityVerstion()
        {
            int first, second;
            GetUnityVersion(out first, out second);

            if (first >= 2017)
            {
                ReplaceString(true, "UNITY_2017_or_Higher");
                ReplaceString(true, "UNITY_5_0_or_Higher");
            }
            if (first == 5)
            {
                ReplaceString(true, "UNITY_5_0_or_Higher");
            }
        }

        private static void UpdateDefineNGUI()
        {
            ReplaceString(IsNGUIImport(), "NGUI_USED");
        }

        private static void UpdateDefineUGUI()
        {
            ReplaceString(IsUGUIImport(), "UGUI_USED");
        }

        private static void ReplaceString(bool isOkay, string replaceStr)
        {
            BuildTargetGroup currentTarget = EditorUserBuildSettings.selectedBuildTargetGroup;
            string str = PlayerSettings.GetScriptingDefineSymbolsForGroup(currentTarget);

            if (isOkay)
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(currentTarget, AppendString(str, replaceStr));
            }
            else
            {
                str = PlayerSettings.GetScriptingDefineSymbolsForGroup(currentTarget);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(currentTarget, RemoveString(str, replaceStr));
            }
        }

        private static string AppendString(string originStr, string appendStr)
        {
            if (originStr.Length == 0)
                return appendStr;
            else
            {
                if (originStr.Contains(appendStr) == false)
                    return string.Format("{0};{1}", originStr, appendStr);
                else
                    return originStr;
            }
        }

        private static string RemoveString(string originStr, string removeStr)
        {
            if (originStr.Length > 0)
            {
                if (originStr.Contains(removeStr))
                {
                    int index = originStr.IndexOf(removeStr);
                    return originStr.Remove(index, removeStr.Length);
                }
            }
            return originStr;
        }

        private static bool IsNGUIImport()
        {
            // Check files : NGUIHelp.cs, NGUITools.cs
            System.Type type1 = System.Type.GetType("NGUIHelp");
            System.Type type2 = System.Type.GetType("NGUITools");
            return ((type1 != null) || (type2 != null));
        }

        private static bool IsUGUIImport()
        {
#if UNITY_5_0_or_Higher
            return true;
#else
            return false;
#endif
        }
    }
}