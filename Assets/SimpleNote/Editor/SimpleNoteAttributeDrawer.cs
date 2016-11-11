using UnityEngine;
using System.Collections;
using UnityEditor;

namespace DI.SimpleNote {
#if UNITY_EDITOR
	[CustomEditor(typeof(MonoBehaviour), true)]
	public class SimpleNoteAttributeDrawer : Editor
	{
		MonoBehaviour monoBehaviour;
		string title, note;

		void OnEnable()
		{
			monoBehaviour = (MonoBehaviour)target;
			if (!EditorPrefs.HasKey(monoBehaviour.GetInstanceID() + "-SimpleNote-Title"))
			{
				EditorPrefs.SetString(monoBehaviour.GetInstanceID() + "-SimpleNote-Title", "Title Here");
				EditorPrefs.SetString(monoBehaviour.GetInstanceID() + "-SimpleNote-Note", "Note Here");
			}

			title = EditorPrefs.GetString(monoBehaviour.GetInstanceID() + "-SimpleNote-Title", "Title Here");
			note = EditorPrefs.GetString(monoBehaviour.GetInstanceID() + "-SimpleNote-Note", "Note Here");
		}

		void OnDisable()
		{
			if (monoBehaviour == null)
			{
				EditorPrefs.DeleteKey(monoBehaviour.GetInstanceID() + "-SimpleNote-Title");
				EditorPrefs.DeleteKey(monoBehaviour.GetInstanceID() + "-SimpleNote-Note");
			}
		}

		public override void OnInspectorGUI()
		{
			SimpleNoteAttribute attribute = (SimpleNoteAttribute)PropertyAttribute.GetCustomAttribute(monoBehaviour.GetType(), typeof(SimpleNoteAttribute));
			if (attribute != null)
			{
				GUIStyle textField = new GUIStyle(EditorStyles.textField);
				textField.fontStyle = FontStyle.Bold;
				if (GUI.GetNameOfFocusedControl() != "Title" + monoBehaviour.gameObject.GetInstanceID())
					textField.normal = EditorStyles.label.normal;

				EditorGUILayout.BeginHorizontal();
				GUI.SetNextControlName("Title" + monoBehaviour.gameObject.GetInstanceID());
				title = EditorGUILayout.TextField(title, textField);
				if (GUI.GetNameOfFocusedControl() == "Title" + monoBehaviour.gameObject.GetInstanceID())
				{
					if (GUILayout.Button("Save", GUILayout.Height(15), GUILayout.Width(45)))
					{
						EditorPrefs.SetString(monoBehaviour.GetInstanceID() + "-SimpleNote-Title", title);
						GUI.FocusControl(null);
					}
				}
				EditorGUILayout.EndHorizontal();

				GUIStyle textArea = new GUIStyle(EditorStyles.textArea);
				textArea.richText = true;
				if (GUI.GetNameOfFocusedControl() != "Note" + monoBehaviour.gameObject.GetInstanceID())
					textArea.normal = EditorStyles.label.normal;

				EditorGUILayout.BeginHorizontal();

				GUI.SetNextControlName("Note" + monoBehaviour.gameObject.GetInstanceID());
				note = EditorGUILayout.TextArea(note, textArea);
				if (GUI.GetNameOfFocusedControl() == "Note" + monoBehaviour.gameObject.GetInstanceID())
				{
					if (GUILayout.Button("Save", GUILayout.Height(15), GUILayout.Width(45)))
					{
						EditorPrefs.SetString(monoBehaviour.GetInstanceID() + "-SimpleNote-Note", note);
						GUI.FocusControl(null);
					}
				}

				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Space();
				EditorGUI.DrawRect(GUILayoutUtility.GetLastRect(), SimpleNoteData.Instance.getBgColor1);
			}

			DrawDefaultInspector();
		}
	}

#endif
}
