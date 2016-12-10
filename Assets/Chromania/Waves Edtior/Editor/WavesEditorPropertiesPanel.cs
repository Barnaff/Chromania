using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;

public static class WavesEditorPropertiesPanel  {

    private enum eWaveEditorTabTab
    {
        Sequance,
        Wave,
        Item,
    }

    private static eWaveEditorTabTab _selectedTab;

    [SerializeField]
    private static ReorderableList _selectedWaveList;
    private static Vector2 _waveListScrollPosition = Vector2.zero;
    private static SequanceDataObject SelectedSequance = null;

    [SerializeField]
    private static ReorderableList _spawnedItemsList;
    private static Vector2 _spawnedItemsListScrollPosition = Vector2.zero;
    private static WaveDataObject _selectedWave = null;

    public static void DrawProperties(this WavesEditor wavesEditor, Rect rect)
    {
        EditorGUI.DrawRect(new Rect(0, 0, rect.width, rect.height), Color.yellow);

        GUILayout.BeginArea(new Rect(0, 0, rect.width * 0.1f, rect.height));
        {
            GUILayout.BeginArea(new Rect(5, 0, rect.width * 0.1f, rect.height));

            EditorGUILayout.BeginVertical();
            {
                if (GUILayout.Toggle(_selectedTab == eWaveEditorTabTab.Sequance, "Sequance", "Button", GUILayout.Height(rect.height * 0.3f)))
                {
                    _selectedTab = eWaveEditorTabTab.Sequance;
                }

                if (GUILayout.Toggle(_selectedTab == eWaveEditorTabTab.Wave, "Wave", "Button", GUILayout.Height(rect.height * 0.3f)))
                {
                    _selectedTab = eWaveEditorTabTab.Wave;
                }

                if (GUILayout.Toggle(_selectedTab == eWaveEditorTabTab.Item, "Item", "Button", GUILayout.Height(rect.height * 0.3f)))
                {
                    _selectedTab = eWaveEditorTabTab.Item;
                }
            }
            EditorGUILayout.EndVertical();

            GUILayout.EndArea();
        }
        GUILayout.EndArea();


        GUILayout.BeginArea(new Rect(rect.width * 0.1f, 0, rect.width  - rect.width * 0.1f, rect.height));
        {
            switch (_selectedTab)
            {
                case eWaveEditorTabTab.Sequance:
                    {
                        if (wavesEditor._currentSequance == null)
                        {
                            break;
                        }
                        DrawSequancePanel(wavesEditor);
                        break;
                    }
                case eWaveEditorTabTab.Wave:
                    {
                        if (wavesEditor._currentWave == null)
                        {
                            _selectedTab = eWaveEditorTabTab.Sequance;
                            break;
                        }
                        DrawWavePanel(wavesEditor);
                        break;
                    }
                case eWaveEditorTabTab.Item:
                    {
                        if (wavesEditor._currentSpwanItem == null)
                        {
                            _selectedTab = eWaveEditorTabTab.Wave;
                            break;
                        }
                        DrawItemPanel(wavesEditor);
                        break;
                    }
            }
        }
        GUILayout.EndArea();
    }

    private static void DrawSequancePanel(WavesEditor wavesEditor)
    {
        if (wavesEditor._currentSequance != null)
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.BeginVertical();
                {
                    wavesEditor._currentSequance.Enabled = EditorGUILayout.Toggle("Enabled", wavesEditor._currentSequance.Enabled);
                    wavesEditor._currentSequance.Identifier = EditorGUILayout.TextField("Wave ID", wavesEditor._currentSequance.Identifier);
                    wavesEditor._currentSequance.LevelModifier = EditorGUILayout.FloatField("Level Modifier", wavesEditor._currentSequance.LevelModifier);
                    wavesEditor._currentSequance.MaxLevel = EditorGUILayout.IntField("Max Level", wavesEditor._currentSequance.MaxLevel);
                    wavesEditor._currentSequance.MinLevel = EditorGUILayout.IntField("Min Level", wavesEditor._currentSequance.MinLevel);
                    wavesEditor._currentSequance.GameMode = (eGameplayMode)EditorGUILayout.EnumPopup("Game Mode", wavesEditor._currentSequance.GameMode);
                }
                EditorGUILayout.EndVertical();

                DrawWaveList(wavesEditor);
            }
            EditorGUILayout.EndHorizontal();
        }
       
    }

    private static void DrawWavePanel(WavesEditor wavesEditor)
    {
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.BeginVertical();
            {
                wavesEditor._currentWave.Enabled = EditorGUILayout.Toggle("Enabled", wavesEditor._currentWave.Enabled);
                wavesEditor._currentWave.Delay = EditorGUILayout.FloatField("Delay", wavesEditor._currentWave.Delay);
              
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginHorizontal();
            {
                DrawSpwanedItemsList(wavesEditor);

                DrawWaveList(wavesEditor);
            }
            EditorGUILayout.EndHorizontal();

        }
        EditorGUILayout.EndHorizontal();
       
    }

    private static void DrawItemPanel(WavesEditor wavesEditor)
    {
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.BeginVertical();
            {
                if (wavesEditor._currentSpwanItem != null)
                {
                    wavesEditor._currentSpwanItem.SpwanedColor = (eSpwanedColorType)EditorGUILayout.EnumPopup("Spwaned Color", wavesEditor._currentSpwanItem.SpwanedColor);
                    wavesEditor._currentSpwanItem.XPosition = EditorGUILayout.FloatField("X Position", wavesEditor._currentSpwanItem.XPosition);
                    wavesEditor._currentSpwanItem.ForceVector = EditorGUILayout.Vector2Field("Force Vector", wavesEditor._currentSpwanItem.ForceVector);
                }
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginHorizontal();
            {
                DrawSpwanedItemsList(wavesEditor);

                DrawWaveList(wavesEditor);
            }
            EditorGUILayout.EndHorizontal();

        }
        EditorGUILayout.EndHorizontal();
    }

    private static void DrawWaveList(WavesEditor wavesEditor)
    {
        if (wavesEditor._currentSequance != null)
        {
            _waveListScrollPosition = EditorGUILayout.BeginScrollView(_waveListScrollPosition);
            {
                if (wavesEditor._currentSequance != SelectedSequance)
                {
                    _selectedWaveList = new ReorderableList(wavesEditor._currentSequance.Waves, typeof(SequanceDefenition), true, false, true, true);

                    _selectedWaveList.drawElementCallback = (Rect cellRect, int index, bool isActive, bool isFocused) => {

                        for (int i=0; i < wavesEditor._currentSequance.Waves[index].SpawnedItems.Count; i++)
                        {
                            GUI.DrawTexture(new Rect(cellRect.x + (i * 16), cellRect.y, 16, 16), TeaxtureForSpwanItem(wavesEditor._currentSequance.Waves[index].SpawnedItems[i].SpwanedColor, wavesEditor));
                        }
                        if (wavesEditor._currentSequance.Waves[index].Delay > 0)
                        {
                            EditorGUI.LabelField(new Rect(cellRect.x + wavesEditor._currentSequance.Waves[index].SpawnedItems.Count * 16, cellRect.y, 100,16) ,"Delay " + wavesEditor._currentSequance.Waves[index].Delay);
                        }
                    };


                    _selectedWaveList.onSelectCallback = (list) =>
                    {
                        wavesEditor._currentWave = wavesEditor._currentSequance.Waves[list.index];
                        _selectedTab = eWaveEditorTabTab.Wave;
                    };

                    _selectedWaveList.onAddCallback = (list) =>
                    {
                        if (wavesEditor._currentSequance != null)
                        {
                            WaveDataObject newWave = new WaveDataObject();
                            wavesEditor._currentSequance.Waves.Add(newWave);
                            wavesEditor._currentWave = newWave;
                            _selectedWave = newWave;
                            list.index = list.count - 1;
                        }
                    };

                    SelectedSequance = wavesEditor._currentSequance;
                }


                _selectedWaveList.DoLayoutList();

            }
            EditorGUILayout.EndScrollView();
        }
       
    }

    private static void DrawSpwanedItemsList(WavesEditor wavesEditor)
    {
        if (wavesEditor._currentWave != null)
        {
            _spawnedItemsListScrollPosition = EditorGUILayout.BeginScrollView(_spawnedItemsListScrollPosition);
            {
                if (wavesEditor._currentWave != _selectedWave)
                {
                    _spawnedItemsList = new ReorderableList(wavesEditor._currentWave.SpawnedItems, typeof(SpawnedItemDataObject), true, false, true, true);

                    _spawnedItemsList.drawElementCallback = (Rect cellRect, int index, bool isActive, bool isFocused) => {

                       // GUI.Label(cellRect, "item " + index.ToString());

                        if (wavesEditor._currentWave != null && wavesEditor._currentWave.SpawnedItems != null && wavesEditor._currentWave.SpawnedItems.Count > index)
                        {
                           GUI.DrawTexture(new Rect(cellRect.x, cellRect.y, 16, 16), TeaxtureForSpwanItem(wavesEditor._currentWave.SpawnedItems[index].SpwanedColor, wavesEditor));
                        }
                        
                    };

                    _spawnedItemsList.onSelectCallback = (list) =>
                    {
                        wavesEditor._currentSpwanItem = wavesEditor._currentWave.SpawnedItems[list.index];
                        _selectedTab = eWaveEditorTabTab.Item;
                    };

                    _spawnedItemsList.onAddCallback = (list) =>
                    {
                        SpawnedItemDataObject newItem = new SpawnedItemDataObject();
                        newItem.ForceVector = new Vector2(0, 144);
                        wavesEditor._currentWave.SpawnedItems.Add(newItem);
                    };

                    _selectedWave = wavesEditor._currentWave;
                }


                _spawnedItemsList.DoLayoutList();

            }
            EditorGUILayout.EndScrollView();
        }
       
    }

    private static Texture2D TeaxtureForSpwanItem(eSpwanedColorType spwanedItemColorType, WavesEditor wavesEditor)
    {
        Texture2D texture = null;
        switch (spwanedItemColorType)
        {
            case eSpwanedColorType.BottomLeft:
                {
                    texture = wavesEditor._leftBottomTexture;
                    break;
                }
            case eSpwanedColorType.BottomRight:
                {
                    texture = wavesEditor._rightBottomTexture;
                    break;
                }
            case eSpwanedColorType.RandomCorner:
                {
                    texture = wavesEditor._randomTexture;
                    break;
                }
            case eSpwanedColorType.TopLeft:
                {
                    texture = wavesEditor._leftTopTexture;
                    break;
                }
            case eSpwanedColorType.TopRight:
                {
                    texture = wavesEditor._rightTopTexture;
                    break;
                }
        }

        return texture;
    }
}
