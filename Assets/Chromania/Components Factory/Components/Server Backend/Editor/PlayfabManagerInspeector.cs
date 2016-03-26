using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayFabManager))]
public class PlayfabManagerInspeector : Editor {

    public override void OnInspectorGUI()
    {
        PlayFabManager costumEditor = (PlayFabManager)target;

        if (GUILayout.Button("Link Facebook"))
        {

        }

        base.DrawDefaultInspector();
    }
}
