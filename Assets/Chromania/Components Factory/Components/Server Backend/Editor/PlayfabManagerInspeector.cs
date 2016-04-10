using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayFabManager))]
public class PlayfabManagerInspeector : Editor {

    private eGameMode _selectedGameMode;

    private int _score;

    public override void OnInspectorGUI()
    {
        PlayFabManager costumEditor = (PlayFabManager)target;

        if (GUILayout.Button("Link Facebook"))
        {

        }

        GUILayout.BeginHorizontal("Box");

        _score = EditorGUILayout.IntField(_score);

        _selectedGameMode = (eGameMode)EditorGUILayout.EnumPopup(_selectedGameMode);

        if (GUILayout.Button("Send Score"))
        {
            costumEditor.PostScore(_selectedGameMode, _score, null, () =>
            {
                Debug.Log("Posted score");
            });
        }

        GUILayout.EndHorizontal();

        base.DrawDefaultInspector();
    }
}
