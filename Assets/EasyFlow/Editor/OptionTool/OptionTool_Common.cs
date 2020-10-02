using UnityEngine;
using UnityEditor;
using System.Collections;

namespace NAsoft_EasyFlow
{
	public partial class OptionTool : EditorWindow
	{
		private void GUITitle(string title, string valueName, object value)
		{
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField(title, new GUIStyle("BoldLabel"), GUILayout.ExpandWidth(true));
				EditorGUILayout.LabelField(string.Format("{0}: {1}", valueName, value));
			}
			EditorGUILayout.EndHorizontal();
		}
		
		private void GUITitle(string title, string valueName1, string valueName2, object value1, object value2)
		{
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField(title, new GUIStyle("BoldLabel"), GUILayout.ExpandWidth(true));
				EditorGUILayout.LabelField(string.Format("{0}: {1}  {2}: {3}", valueName1, value1, valueName2, value2), GUILayout.ExpandWidth(true));
			}
			EditorGUILayout.EndHorizontal();
		}
		
		private void GUITitle(string title, string valueName1, string valueName2, string valueName3, object value1, object value2, object value3)
		{
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField(title, new GUIStyle("BoldLabel"), GUILayout.ExpandWidth(true));
				EditorGUILayout.LabelField(string.Format("{0}: {1}  {2}: {3}  {4}: {5}", valueName1, value1, valueName2, value2, valueName3, value3), GUILayout.ExpandWidth(true));
			}
			EditorGUILayout.EndHorizontal();
		}




		private void GUIYesNo(string title, ref BOOL isBool)
		{
			GUIYesNo(title, "No", "Yes", ref isBool);
		}

		private void GUIYesNo(string title, string s1, string s2, ref BOOL isBool)
		{
			EditorGUILayout.LabelField(title, new GUIStyle("BoldLabel"), GUILayout.ExpandWidth(true));
			isBool = (BOOL)GUILayout.SelectionGrid((int)isBool, new string[]{s1,s2}, 2, GUILayout.ExpandWidth(true));
		}



		// Horizontal Slider - Limit Field : Left(x), Right(x)
		private void GUISlider(string title, string valueName, int round, ref float value, float leftLimit, float rightLimit)
		{
			GUITitle(title, valueName, value);
			
			float roundF = 1.0f;
			for (int i=0; i<round; ++i)
				roundF *= 10.0f;
			
			EditorGUILayout.BeginHorizontal();
			{
				value = GUILayout.HorizontalSlider(value, leftLimit, rightLimit, GUILayout.ExpandWidth(true));
				value = Mathf.Round(roundF * value) / roundF;
			}
			EditorGUILayout.EndHorizontal();
		}
		// Horizontal Slider - Limit Field : Left(x), Right(x)
		private void GUISlider(string title, string valueName, ref float value, float leftLimit, float rightLimit)
		{
			GUITitle(title, valueName, value);
			
			value = (int)GUILayout.HorizontalSlider((int)value, leftLimit, rightLimit, GUILayout.ExpandWidth(true));
		}

		// Int Slider - Limit Field : Left(x), Right(o)
		private void GUISlider(string title, string valueName, ref int value, int leftLimit, ref int rightLimit)
		{
			GUITitle(title, valueName, value);

			EditorGUILayout.BeginHorizontal();
			{
				string str = string.Empty;

				value = (int)GUILayout.HorizontalSlider((float)value, (float)leftLimit, (float)rightLimit, GUILayout.ExpandWidth(true));
				
				str = rightLimit.ToString();
				rightLimit = EditorGUILayout.IntField(rightLimit, GUILayout.Width((str.Length+1) * 7.0f));
			}
			EditorGUILayout.EndHorizontal();
		}

		// Horizontal Slider - Limit Field : Left(x), Right(o)
		private void GUISlider(string title, string valueName, int round, ref float value, float leftLimit, ref float rightLimit)
		{
			GUITitle(title, valueName, value);
			
			float roundF = 1.0f;
			for (int i=0; i<round; ++i)
				roundF *= 10.0f;
			
			EditorGUILayout.BeginHorizontal();
			{
				string str = string.Empty;
				
				value = GUILayout.HorizontalSlider(value, leftLimit, rightLimit, GUILayout.ExpandWidth(true));
				value = Mathf.Round(roundF * value) / roundF;
				
				str = rightLimit.ToString();
				rightLimit = EditorGUILayout.FloatField(rightLimit, GUILayout.Width((str.Length+1) * 7.0f));
				rightLimit = Mathf.Round(roundF * rightLimit) / roundF;
			}
			EditorGUILayout.EndHorizontal();
		}
		// Horizontal Slider - Limit Field : Left(x), Right(o)
		private void GUISlider(string title, string valueName, ref float value, float leftLimit, ref float rightLimit)
		{
			GUITitle(title, valueName, value);
			
			EditorGUILayout.BeginHorizontal();
			{
				string str = string.Empty;
				
				value = (int)GUILayout.HorizontalSlider((int)value, leftLimit, rightLimit, GUILayout.ExpandWidth(true));
				
				str = rightLimit.ToString();
				rightLimit = (int)EditorGUILayout.FloatField((int)rightLimit, GUILayout.Width((str.Length+1) * 7.0f));
			}
			EditorGUILayout.EndHorizontal();
		}

		// Horizontal Slider - Limit Field : Left(o), Right(o)
		private void GUISlider(string title, string valueName, int round, ref float value, ref float leftLimit, ref float rightLimit)
		{
			GUITitle(title, valueName, value);
			
			float roundF = 1.0f;
			for (int i=0; i<round; ++i)
				roundF *= 10.0f;
			
			EditorGUILayout.BeginHorizontal();
			{
				string str = leftLimit.ToString();
				leftLimit = EditorGUILayout.FloatField(leftLimit, GUILayout.Width((str.Length+1) * 7.0f));
				leftLimit = Mathf.Round(roundF * leftLimit) / roundF;
				
				value = GUILayout.HorizontalSlider(value, leftLimit, rightLimit, GUILayout.ExpandWidth(true));
				value = Mathf.Round(roundF * value) / roundF;
				
				str = rightLimit.ToString();
				rightLimit = EditorGUILayout.FloatField(rightLimit, GUILayout.Width((str.Length+1) * 7.0f));
				rightLimit = Mathf.Round(roundF * rightLimit) / roundF;
			}
			EditorGUILayout.EndHorizontal();
		}
		// Horizontal Slider - Limit Field : Left(o), Right(o)
		private void GUISlider(string title, string valueName, ref float value, ref float leftLimit, ref float rightLimit)
		{
			GUITitle(title, valueName, value);
			
			EditorGUILayout.BeginHorizontal();
			{
				string str = leftLimit.ToString();
				leftLimit = (int)EditorGUILayout.FloatField((int)leftLimit, GUILayout.Width((str.Length+1) * 7.0f));
				
				value = (int)GUILayout.HorizontalSlider((int)value, leftLimit, rightLimit, GUILayout.ExpandWidth(true));
				
				str = rightLimit.ToString();
				rightLimit = (int)EditorGUILayout.FloatField((int)rightLimit, GUILayout.Width((str.Length+1) * 7.0f));
			}
			EditorGUILayout.EndHorizontal();
		}
		
		// Horizontal Slider : 2 Line - Limit Field : Left(x), Right(o)
		private void GUISlider(string title, string valueName1, string valueName2, int round, ref Vector2 value, ref Vector2 limit)
		{
			GUITitle(title, valueName1, valueName2, value.x, value.y);
			
			float roundF = 1.0f;
			for (int i=0; i<round; ++i)
				roundF *= 10.0f;
			
			
			string str = string.Empty;
			EditorGUILayout.BeginHorizontal();
			{
				GUILayout.Label(valueName1, "BoldLabel", GUILayout.ExpandWidth(false));
				
				value.x = GUILayout.HorizontalSlider(value.x, 0.0f, limit.x, GUILayout.ExpandWidth(true));
				value.x = Mathf.Round(roundF * value.x) / roundF;
				
				str = limit.x.ToString();
				limit.x = EditorGUILayout.FloatField(limit.x, GUILayout.Width((str.Length+1) * 7.0f));
				limit.x = Mathf.Round(roundF * limit.x) / roundF;
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			{
				GUILayout.Label(valueName2, "BoldLabel", GUILayout.ExpandWidth(false));
				
				value.y = GUILayout.HorizontalSlider(value.y, 0.0f, limit.y, GUILayout.ExpandWidth(true));
				value.y = Mathf.Round(roundF * value.y) / roundF;
				
				str = limit.y.ToString();
				limit.y = EditorGUILayout.FloatField(limit.y, GUILayout.Width((str.Length+1) * 7.0f));
				limit.y = Mathf.Round(roundF * limit.y) / roundF;
			}
			EditorGUILayout.EndHorizontal();
		}
		// Horizontal Slider : 2 Line - Limit Field : Left(x), Right(o)
		private void GUISlider(string title, string valueName1, string valueName2, ref Vector2 value, ref Vector2 limit)
		{
			GUITitle(title, valueName1, valueName2, value.x, value.y);
			
			string str = string.Empty;
			EditorGUILayout.BeginHorizontal();
			{
				GUILayout.Label(valueName1, "BoldLabel", GUILayout.ExpandWidth(false));
				
				value.x = (int)GUILayout.HorizontalSlider((int)value.x, 0.0f, limit.x, GUILayout.ExpandWidth(true));
				
				str = limit.x.ToString();
				limit.x = (int)EditorGUILayout.FloatField((int)limit.x, GUILayout.Width((str.Length+1) * 7.0f));
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			{
				GUILayout.Label(valueName2, "BoldLabel", GUILayout.ExpandWidth(false));
				
				value.y = (int)GUILayout.HorizontalSlider((int)value.y, 0.0f, limit.y, GUILayout.ExpandWidth(true));
				
				str = limit.y.ToString();
				limit.y = (int)EditorGUILayout.FloatField((int)limit.y, GUILayout.Width((str.Length+1) * 7.0f));
			}
			EditorGUILayout.EndHorizontal();
		}

		// Horizontal Slider : 3 Line - Limit Field : Left(o), Right(o)
		private void GUISlider(string title, string valueName1, string valueName2, string valueName3, int round, ref Vector3 value, ref Vector3 limit)
		{
			GUITitle(title, valueName1, valueName2, valueName3, value.x, value.y, value.z); 
			
			float roundF = 1.0f;
			for (int i=0; i<round; ++i)
				roundF *= 10.0f;
			
			string str = string.Empty;
			EditorGUILayout.BeginHorizontal();
			{
				GUILayout.Label(valueName1, "BoldLabel", GUILayout.ExpandWidth(false));
				
				value.x = GUILayout.HorizontalSlider(value.x, -limit.x, limit.x, GUILayout.ExpandWidth(true));
				value.x = Mathf.Round(roundF * value.x) / roundF;
				
				str = limit.x.ToString();
				limit.x = EditorGUILayout.FloatField(limit.x, GUILayout.Width((str.Length+1) * 7.0f));
				limit.x = Mathf.Round(roundF * limit.x) / roundF;
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			{
				GUILayout.Label(valueName2, "BoldLabel", GUILayout.ExpandWidth(false));
				
				value.y = GUILayout.HorizontalSlider(value.y, -limit.y, limit.y, GUILayout.ExpandWidth(true));
				value.y = Mathf.Round(roundF * value.y) / roundF;
				
				str = limit.y.ToString();
				limit.y = EditorGUILayout.FloatField(limit.y, GUILayout.Width((str.Length+1) * 7.0f));
				limit.y = Mathf.Round(roundF * limit.y) / roundF;
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			{
				GUILayout.Label(valueName3, "BoldLabel", GUILayout.ExpandWidth(false));
				
				value.z = GUILayout.HorizontalSlider(value.z, -limit.z, limit.z, GUILayout.ExpandWidth(true));
				value.z = Mathf.Round(roundF * value.z) / roundF;
				
				str = limit.z.ToString();
				limit.z = EditorGUILayout.FloatField(limit.z, GUILayout.Width((str.Length+1) * 7.0f));
				limit.z = Mathf.Round(roundF * limit.z) / roundF;
			}
			EditorGUILayout.EndHorizontal();
		}
		// Horizontal Slider : 3 Line - Limit Field : Left(o), Right(o)
		private void GUISlider(string title, string valueName1, string valueName2, string valueName3, ref Vector3 value, ref Vector3 limit)
		{
			GUITitle(title, valueName1, valueName2, valueName3, value.x, value.y, value.z); 
			
			string str = string.Empty;
			EditorGUILayout.BeginHorizontal();
			{
				GUILayout.Label(valueName1, "BoldLabel", GUILayout.ExpandWidth(false));
				
				value.x = GUILayout.HorizontalSlider((int)value.x, -limit.x, limit.x, GUILayout.ExpandWidth(true));
				
				str = limit.x.ToString();
				limit.x = (int)EditorGUILayout.FloatField((int)limit.x, GUILayout.Width((str.Length+1) * 7.0f));
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			{
				GUILayout.Label(valueName2, "BoldLabel", GUILayout.ExpandWidth(false));
				
				value.y = (int)GUILayout.HorizontalSlider((int)value.y, -limit.y, limit.y, GUILayout.ExpandWidth(true));
				
				str = limit.y.ToString();
				limit.y = (int)EditorGUILayout.FloatField((int)limit.y, GUILayout.Width((str.Length+1) * 7.0f));
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			{
				GUILayout.Label(valueName3, "BoldLabel", GUILayout.ExpandWidth(false));
				
				value.z = (int)GUILayout.HorizontalSlider((int)value.z, -limit.z, limit.z, GUILayout.ExpandWidth(true));
				
				str = limit.z.ToString();
				limit.z = (int)EditorGUILayout.FloatField((int)limit.z, GUILayout.Width((str.Length+1) * 7.0f));
			}
			EditorGUILayout.EndHorizontal();
		}
		
		// Min Max Slider
		private void GUISlider(string title, string valueName1, string valueName2, int round, ref float value1, ref float value2, ref float limit1, ref float limit2)
		{
			GUITitle(title, valueName1, valueName2, value1, value2);
			
			float roundF = 1.0f;
			for (int i=0; i<round; ++i)
				roundF *= 10.0f;
			
			EditorGUILayout.BeginHorizontal();
			{
				string str = limit1.ToString();
				limit1 = EditorGUILayout.FloatField(limit1, GUILayout.Width((str.Length+1) * 7.0f));
				limit1 = Mathf.Round(roundF * limit1) / roundF;
				
				EditorGUILayout.MinMaxSlider(ref value1, ref value2, limit1, limit2, GUILayout.ExpandWidth(true));
				value1 = Mathf.Round(roundF * value1) / roundF;
				value2 = Mathf.Round(roundF * value2) / roundF;
				
				str = limit2.ToString();
				limit2 = EditorGUILayout.FloatField(limit2, GUILayout.Width((str.Length+1) * 7.0f));
				limit2 = Mathf.Round(roundF * limit2) / roundF;
			}
			EditorGUILayout.EndHorizontal();
		}

		
		
		private void GUICurve(string title, ref AnimationCurve curve, float height)
		{
			EditorGUILayout.LabelField(title, new GUIStyle("BoldLabel"), GUILayout.ExpandWidth(true));
			
			if (curve != null)
			{
				EditorGUI.BeginChangeCheck();

				curve = EditorGUILayout.CurveField(curve, GUILayout.Height(height));
				
				if (EditorGUI.EndChangeCheck())
				{
					if (selectedEasyflow != null)
						selectedEasyflow.Init();
				}
			}
		}
	}
}