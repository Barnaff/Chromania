using UnityEngine;
using System.Collections;
using UnityEditor;

public class PlayModeEditor : EditorWindow {

    [MenuItem("Tools/Editor Play Mode")]
    public static void DisplayPlayModeWindow()
    {
        EditorWindow.GetWindow<PlayModeEditor>();

    }
	// Use this for initialization
	void OnEnable () {
        EditorApplication.playmodeStateChanged += HandleOnPlayModeChanged;

    }
	
	// Update is called once per frame
	void Update () {
	
	}


    private void HandleOnPlayModeChanged()
    {
        Debug.Log("editor mode changed");
        // This method is run whenever the playmode state is changed.
        if (EditorApplication.isPaused)
        {
            // do stuff when the editor is paused.
        }
    }
}
