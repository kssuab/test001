using System;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

public class DebugCodeLocation
{

	static string LoggerName = "KyowonLogger";


	[OnOpenAsset(0)]
	static bool OnOpenAsset(int instance, int line)
	{
		string name = EditorUtility.InstanceIDToObject(instance).name;
		if (name != LoggerName) return false;

		 

		string stack_trace = GetStackTrace();
		if (!string.IsNullOrEmpty(stack_trace))
		{
			Match matches = Regex.Match(stack_trace, @"\(at(.+)\)", RegexOptions.IgnoreCase);
			Match matchesCompile = Regex.Match(stack_trace, @"Assets.+\)", RegexOptions.IgnoreCase);

			if (matches.Success)
			{
				matches = matches.NextMatch(); // Raise another layer up to enter;
				if (matches.Success)
				{
					string pathline;
					pathline = matches.Groups[1].Value;
					pathline = pathline.Replace(" ", "");

					int split_index = pathline.LastIndexOf(":");
					string path = pathline.Substring(0, split_index);
					line = Convert.ToInt32(pathline.Substring(split_index + 1));
					string fullpath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("Assets"));
					fullpath = fullpath + path;
					UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(fullpath, line, 0);
				}
				else
				{
					Debug.LogError("DebugCodeLocation OnOpenAsset, Error StackTrace");
				}
			}
			else if (matchesCompile.Success)
				{
					if (matchesCompile.Success)
					{
						string pathline;
						pathline = matchesCompile.Groups[0].Value;
						pathline = pathline.Replace(" ", "");
						int split_index = pathline.LastIndexOf("(");
					string path = pathline.Substring(0, split_index);
					line = Convert.ToInt32(pathline.Split('(')[1].Split(',')[0]);
					UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(path, line, 0);
					}
					
				}
			else
			{
				Debug.LogError("DebugCodeLocation OnOpenAsset, Error StackTrace");
			}

			return true;
		}
		return false;
	}

	static string GetStackTrace()
	{
		var assembly_unity_editor = Assembly.GetAssembly(typeof(EditorWindow));
		if (assembly_unity_editor == null) return null;

		var type_console_window = assembly_unity_editor.GetType("UnityEditor.ConsoleWindow");
		if (type_console_window == null) return null;
		var field_console_window = type_console_window.GetField("ms_ConsoleWindow", BindingFlags.Static | BindingFlags.NonPublic);
		if (field_console_window == null) return null;
		var instance_console_window = field_console_window.GetValue(null);
		if (instance_console_window == null) return null;

		if ((object)EditorWindow.focusedWindow == instance_console_window)
		{
			var type_list_view_state = assembly_unity_editor.GetType("UnityEditor.ListViewState");
			if (type_list_view_state == null) return null;

			var field_list_view = type_console_window.GetField("m_ListView", BindingFlags.Instance | BindingFlags.NonPublic);
			if (field_list_view == null) return null;

			var value_list_view = field_list_view.GetValue(instance_console_window);
			if (value_list_view == null) return null;

			var field_active_text = type_console_window.GetField("m_ActiveText", BindingFlags.Instance | BindingFlags.NonPublic);
			if (field_active_text == null) return null;

			string value_active_text = field_active_text.GetValue(instance_console_window).ToString();
			return value_active_text;
		}

		return null;
	}
}