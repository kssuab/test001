using System;
using System.Collections;

#if UNITY_EDITOR

using UnityEditor;

#endif

using UnityEngine;

namespace NAsoft_EasyFlow
{
    public static class ConnectorGUI
    {
#if UNITY_EDITOR

        public static UnityEngine.Object DrawTargetField(this Connector connector, bool allowSceneObjects, params GUILayoutOption[] options)
        {
            connector.target = EditorGUILayout.ObjectField(connector.target, typeof(UnityEngine.Object), allowSceneObjects, options);
            return connector.target;
        }

        public static void DrawMemberField(this Connector connector, params GUILayoutOption[] options)
        {
            DrawMemberField(connector, new GUIStyle("MiniPullDown"), options);
        }

        public static void DrawMemberField(this Connector connector, GUIStyle style, params GUILayoutOption[] options)
        {
            if (connector.target == null)
                connector.selectedString = "Not Selected";

            DrawPopup(connector.selectedString, connector.GetNames, connector.selectedIndex, connector, OnMemberSelected, style, options);
        }

        private static void OnMemberSelected(int index, object obj)
        {
            Connector connector = obj as Connector;
            connector.selectedIndex = index;
            connector.UpdateSelectedString();
        }

        private static void DrawPopup(string title, Func<string[]> stringFunc, int index,
            object reference, Action<int, object> callback,
            GUIStyle style, params GUILayoutOption[] options)
        {
            Rect rt = GUILayoutUtility.GetRect(10.0f, 1000.0f, 16.0f, 16.0f, style, options);

            if (GUI.enabled)
            {
                if (GUI.Button(rt, title, style))
                {
                    if (stringFunc != null)
                    {
                        string[] menuList = stringFunc.Invoke();
                        if (menuList != null)
                        {
                            Connector connector = reference as Connector;
                            connector.UpdateMethodInfo();

                            GenericMenu menu = new GenericMenu();
                            for (int i = 0; i < menuList.Length; ++i)
                            {
                                menu.AddItem(new GUIContent(menuList[i]), (index == i), CallbackSelectPopup,
                                    new { _index = i, _ref = reference, _callback = callback });
                            }
                            menu.DropDown(rt);
                            return;
                        }
                    }

                    Debug.Log("Connector - GetNames is nothing");
                }
            }
            else
            {
                GUI.Label(rt, title, style);
            }
        }

        private static void CallbackSelectPopup(object userData)
        {
            int index = (int)userData.GetType().GetProperty("_index").GetValue(userData, null);
            object reference = userData.GetType().GetProperty("_ref").GetValue(userData, null);
            Action<int, object> callback = (Action<int, object>)userData.GetType().GetProperty("_callback").GetValue(userData, null);
            callback.Invoke(index, reference);
        }

#endif
    }
}