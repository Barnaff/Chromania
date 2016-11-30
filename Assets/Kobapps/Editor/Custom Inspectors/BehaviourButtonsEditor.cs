using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Kobapps
{
    public class BehaviourButtonsHelper
    {
        private static object[] emptyParamList = new object[0];

        private IList<MethodInfo> methods = new List<MethodInfo>();
        private Object targetObject;

        public void Init(Object targetObject)
        {
            this.targetObject = targetObject;
            methods = targetObject.GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(m =>
             m.GetCustomAttributes(typeof(ButtonAttribute), false).Length == 1 &&
             m.GetParameters().Length == 0 &&
             !m.ContainsGenericParameters
            ).ToList();
        }

        public void DrawButtons()
        {
            if (methods.Count > 0)
            {
                EditorGUILayout.HelpBox("Click to execute methods!", MessageType.None);
                ShowMethodButtons();
            }
        }

        private void ShowMethodButtons()
        {
            foreach (MethodInfo method in methods)
            {
                string buttonText = ObjectNames.NicifyVariableName(method.Name);
                if (GUILayout.Button(buttonText))
                {
                    method.Invoke(targetObject, emptyParamList);
                }
            }
        }
    }


    [CanEditMultipleObjects]
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class BehaviourButtonsEditor : UnityEditor.Editor
    {
        private BehaviourButtonsHelper helper = new BehaviourButtonsHelper();

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            helper.DrawButtons();
        }

        private void OnEnable()
        {
            helper.Init(target);
        }
    }


}
