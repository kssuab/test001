using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace NAsoft_EasyFlow
{
    [Serializable]
    public partial class Connector
    {
        private class Info
        {
            public UnityEngine.Object target;
            public MethodInfo methodInfo;

            public Info(UnityEngine.Object target, MethodInfo mi)
            {
                this.target = target;
                this.methodInfo = mi;
            }
        }

        [SerializeField]
        private UnityEngine.Object _target;

        public UnityEngine.Object target
        {
            get
            {
                return _target;
            }
            set
            {
                if (value == null)
                    _target = null;
                else if (value is GameObject)
                    _target = value;
                else if (value is Component)
                {
                    var c = value as Component;
                    _target = c.gameObject;
                }
                else
                    _target = null;
            }
        }

        public int selectedIndex;
        private UnityEngine.Object realTarget;

        [NonSerialized]
        private MethodInfo _methodInfo;

        private MethodInfo methodInfo
        {
            get
            {
                if (_methodInfo == null)
                    UpdateMethodInfo();
                return _methodInfo;
            }
            set { _methodInfo = value; }
        }

        private System.Type[] _types;

        private System.Type[] types
        {
            get
            {
                if (_types == null &&
                    typeNames != null)
                {
                    _types = new System.Type[typeNames.Length];
                    for (int i = 0; i < typeNames.Length; ++i)
                        _types[i] = System.Type.GetType(typeNames[i]);
                }
                return _types;
            }
            set
            {
                _types = value;
                typeNames = new string[_types.Length];
                for (int i = 0; i < _types.Length; ++i)
                    typeNames[i] = _types[i].ToString();
            }
        }

        [SerializeField]
        private string[] typeNames;

#if UNITY_EDITOR
        public string selectedString;
#endif

        public Connector(params System.Type[] types)
        {
            target = null;
            selectedIndex = 0;

            methodInfo = null;

            this.types = types;

#if UNITY_EDITOR
            selectedString = "Not selected";
#endif
        }

        public void Invoke(params object[] parameters)
        {
            if (methodInfo == null ||
                realTarget == null)
                return;

            methodInfo.Invoke(realTarget, parameters);
        }

        public System.Type GetFirstParameterType()
        {
            if (methodInfo == null)
                return null;

            var pis = methodInfo.GetParameters();
            if (pis == null ||
                pis.Length < 1)
                return null;

            return pis[0].ParameterType;
        }

        public string[] GetNames()
        {
            if (target == null)
                return null;

            List<Info> infoList = GetMethods(target);
            if (infoList == null)
                return null;

            string[] strs = new string[infoList.Count + 1];
            strs[0] = "Not selected";
            for (int i = 0; i < infoList.Count; ++i)
                strs[i + 1] = GetName(infoList[i]);

            return strs;
        }

        public void UpdateSelectedString()
        {
#if UNITY_EDITOR
            string[] names = GetNames();
            if (names != null &&
                names.Length > selectedIndex)
                selectedString = names[selectedIndex];
#endif
        }

        public void UpdateMethodInfo()
        {
            Info info = FindInfo();
            if (info != null)
            {
                realTarget = info.target;
                methodInfo = info.methodInfo;
            }
            else
                methodInfo = null;
        }

        private Info FindInfo()
        {
            if (target == null ||
                selectedIndex == 0)
                return null;

            int index = selectedIndex - 1;

            List<Info> infoList = GetMethods(target);
            if (infoList != null &&
                infoList.Count > index)
                return infoList[index];

            return null;
        }

        private List<Info> GetMethods(UnityEngine.Object obj)
        {
            MethodInfo[] mis = obj.GetType().GetMethods(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance);
            ParameterInfo[] pis = null;
            List<Info> infoList = new List<Info>();
            foreach (var mi in mis)
            {
                // Pass the Obsolete property
                var attributes = mi.GetCustomAttributes(typeof(ObsoleteAttribute), false);
                if (attributes != null &&
                    attributes.Length > 0)
                    continue;

                // If you did not select a specific type, just add
                if (types == null ||
                    types.Length <= 0)
                {
                    infoList.Add(new Info(obj, mi));
                    continue;
                }

                // If you select a specific type, add only those that match the type
                pis = mi.GetParameters();
                if (pis.Length >= 1)
                {
                    foreach (var v in types)
                    {
                        if (v != pis[0].ParameterType)
                            continue;

                        infoList.Add(new Info(obj, mi));
                        break;
                    }
                }
            }

            if (obj is GameObject)
            {
                Component[] childs = (obj as GameObject).GetComponents<Component>();
                foreach (var v in childs)
                    infoList.AddRange(GetMethods(v));
            }

            return infoList;
        }

        private static string GetName(Info info)
        {
            StringBuilder sb = new StringBuilder();
            foreach (ParameterInfo parameter in info.methodInfo.GetParameters())
                sb.AppendFormat("{0}, ", parameter.ParameterType.Name.ToString());
            if (sb.Length > 0)
                sb.Remove(sb.Length - 2, 2);

            return string.Format("{0}/{1} {2}({3})", info.target.GetType().Name, info.methodInfo.ReturnType.Name, info.methodInfo.Name, sb.ToString());
        }
    }
}