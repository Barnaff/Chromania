﻿using UnityEngine;
using System.Collections;
using UnityEditor;

public class PlayModeEditor : EditorWindow {

    [SerializeField]
    private int _selectedSceneIndex = 0;

    [SerializeField]
    private bool _enteredPlayMode = false;

    [SerializeField]
    private bool _overrideOpeningScene;

    [MenuItem("Tools/Editor Play Mode")]
    public static void DisplayPlayModeWindow()
    {
        EditorWindow.GetWindow<PlayModeEditor>();

    }
	// Use this for initialization
	void OnEnable () {
        EditorApplication.playmodeStateChanged += HandleOnPlayModeChanged;

    }
	

    private void HandleOnPlayModeChanged()
    {
        Debug.Log("editor mode changed");
        // This method is run whenever the playmode state is changed.
        if (EditorApplication.isPlaying)
        {
            // do stuff when the editor is paused.
            Debug.Log("is playing");

            if (!_enteredPlayMode && _overrideOpeningScene)
            {
                EditorApplication.LoadLevelInPlayMode(EditorBuildSettings.scenes[_selectedSceneIndex].path);
                _enteredPlayMode = true;
            }
           
        }
        else
        {
            _enteredPlayMode = false;
        }

        
    }

    void OnGUI()
    {
        EditorGUILayout.BeginVertical("Box");

        GUI.skin.button.fontSize = 10;
        _overrideOpeningScene = GUILayout.Toggle(_overrideOpeningScene, "Override Opening Scene", "Button");


        EditorGUILayout.BeginFadeGroup(_overrideOpeningScene ? 1.0f : 0.0f);

        string[] names = GetSceneNames();
        _selectedSceneIndex = EditorGUILayout.Popup(_selectedSceneIndex, names);

        EditorGUILayout.EndFadeGroup();

        EditorGUILayout.BeginHorizontal("Box");
        GUI.skin.button.fontSize = 20;
        if( GUILayout.Button("►", GUILayout.Width(50), GUILayout.Height(50)))
        {
            EditorApplication.isPlaying = true;
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }

    private string[] GetSceneNames()
    {
        EditorBuildSettingsScene[] scenesList = EditorBuildSettings.scenes;
        string[] names = new string[scenesList.Length];
        for (int i=0; i< scenesList.Length; i++)
        {
            names[i] = System.IO.Path.GetFileNameWithoutExtension(scenesList[i].path);
        }
        return names;
    }
}
