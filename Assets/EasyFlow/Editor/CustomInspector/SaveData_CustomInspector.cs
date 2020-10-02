using UnityEngine;
using UnityEditor;
using System.Collections;

namespace NAsoft_EasyFlow
{
    [CustomEditor(typeof(SaveData))]
    public class SaveData_CustomInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        }
    }
}