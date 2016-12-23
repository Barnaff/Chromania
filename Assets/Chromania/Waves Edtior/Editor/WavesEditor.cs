using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;

public class WavesEditor : ExtendedEditorWindow
{
    // textures
    [SerializeField]
    public Texture2D _leftBottomTexture;
    [SerializeField]
    public Texture2D _leftTopTexture;
    [SerializeField]
    public Texture2D _rightBottomTexture;
    [SerializeField]
    public Texture2D _rightTopTexture;
    [SerializeField]
    public Texture2D _randomTexture;
    [SerializeField]
    public Texture2D _arrowTexture;

    // waves and sequances
    [SerializeField]
    public SequanceDataObject _currentSequance;
    [SerializeField]
    public WaveDataObject _currentWave;
    [SerializeField]
    public SpawnedItemDataObject _currentSpwanItem;


    // consts
    private const string TEXTURES_PATH = "Assets/Chromania/Waves Edtior/Textures/";
    private const string WAVES_DATA_PATH = "Assets/Resources/WavesData.asset";

    // Waves list
    [SerializeField]
    private ReorderableList _sequancesList;

    [SerializeField]
    private List<SequanceDataObject> _sequances;

    private Vector2 _sequancesListScrollPosition = Vector2.zero;

    [SerializeField]
    private WavesData _wavesData;


    [MenuItem("Chromania/Waves Editor")]
    public static void WavesEditorWindow()
    {
        EditorWindow window = EditorWindow.GetWindow<WavesEditor>();
        window.titleContent = new GUIContent("Chromania Waves Editor");
    }

    void OnEnable()
    {
        LoadTextures();
        LoadWavesdata();
    }

    void OnGUI()
    {
        base.OnGUI();


       // EditorGUI.DrawRect(new Rect(0, 0, position.width, position.height), new Color(0,0.1f,0.3f));

        EditorGUILayout.BeginVertical();
        {


            // preview area
            Rect previewAreaRect = new Rect(0, 0, position.width * 0.7f, position.height * 0.5f);

            GUILayout.BeginArea(previewAreaRect);
            {
                this.DrawPreviewPanel(previewAreaRect);
            }
            GUILayout.EndArea();

            // darw buttons panel

            Rect drawButtonRect = new Rect(0, position.height * 0.5f, position.width * 0.7f, 32);

            GUILayout.BeginArea(drawButtonRect);
            {
                DrawButtons(drawButtonRect);
            }
            GUILayout.EndArea();

            // right side bar

            Rect wavesListRect = new Rect(position.width * 0.7f, 0, position.width * 0.3f, position.height);
            GUILayout.BeginArea(wavesListRect);
            {
                DrawSequancesList(wavesListRect);
            }
            GUILayout.EndArea();

            // properties panel 

            Rect proeprtiesRect = new Rect(0, position.height * 0.5f + 32, position.width * 0.7f, position.height - 32 - position.height * 0.5f);
            GUILayout.BeginArea(proeprtiesRect);
            {
                this.DrawProperties(proeprtiesRect);
            }
            GUILayout.EndArea();

        }
        EditorGUILayout.EndVertical();

        if (_wavesData != null)
        {
            EditorUtility.SetDirty(_wavesData);
            
        }
    }

    #region Draw Panels

    private void DrawButtons(Rect rect)
    {
        Color panelColor = Color.green;
        EditorGUI.DrawRect(new Rect(0,0, rect.width , rect.height), panelColor);

        EditorGUILayout.BeginHorizontal();
        {
            panelColor.a = 0.5f;
            GUI.backgroundColor = panelColor;
            if (GUILayout.Button("Play Current ▶", GUILayout.Height(rect.height)))
            {
                PlayTestCurrentWave();
            }

            if (GUILayout.Button("Play Selected ▶", GUILayout.Height(rect.height)))
            {
                PlayTestSelecte();
            }

            GUI.backgroundColor = Color.white;
        }
        EditorGUILayout.EndHorizontal();
    }

    private void DrawSequancesList(Rect rect)
    {
        //Color panelColor = new Color(0.2f, 0.2f, 0.4f); ;
       // EditorGUI.DrawRect(new Rect(0, 0, rect.width, rect.height), panelColor);


        EditorGUILayout.BeginVertical();
        {
            if (GUILayout.Button("Load From File"))
            {
                LoadWavesFromFile();
            }

            if (GUILayout.Button("Save"))
            {

            }

            if (GUILayout.Button("Merge JSON file"))
            {
                MergeJsonFile();
            }

            _wavesData = (WavesData)EditorGUILayout.ObjectField(_wavesData, typeof(WavesData));

            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Select All"))
                {
                    foreach (SequanceDataObject sequance in _sequances)
                    {
                        sequance.Selected = true;
                    }
                }

                if (GUILayout.Button("Deselect All"))
                {
                    foreach (SequanceDataObject sequance in _sequances)
                    {
                        sequance.Selected = false;
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            _sequancesListScrollPosition = EditorGUILayout.BeginScrollView(_sequancesListScrollPosition);
            {
                if (_sequances == null)
                {
                    _sequances = new List<SequanceDataObject>();
                }

                //panelColor.a = 0.5f;
                //GUI.backgroundColor = panelColor;
                if (_sequancesList == null)
                {
                    _sequancesList = new ReorderableList(_sequances, typeof(SequanceDataObject), true, false, true, true);

                    _sequancesList.drawElementCallback = (Rect cellRect, int index, bool isActive, bool isFocused) => {

                        if (_sequances != null && _sequances[index] != null && _sequances[index].Identifier != null)
                        {
                            GUI.Label(cellRect, "[" + _sequances[index].MinLevel + "-" + _sequances[index].MaxLevel + "] " +  _sequances[index].Identifier);

                            _sequances[index].Selected = GUI.Toggle(new Rect(cellRect.width - 20, cellRect.y, 20, 20), _sequances[index].Selected, "");
                        }
                        
                    };

                    _sequancesList.onSelectCallback = (list) =>
                    {
                        _currentSequance = _sequances[list.index];
                        _currentWave = null;
                        _currentSpwanItem = null;
                    };

                    _sequancesList.onAddCallback = (list) =>
                    {
                        int newWaveId = _sequances.Count;
                        SequanceDataObject newSequance = new SequanceDataObject();
                        newSequance.Identifier = "sequance." + newWaveId.ToString();
                        _sequances.Add(newSequance);
                    };

                }

                if (_sequancesList != null)
                {
                    _sequancesList.DoLayoutList();
                }

                GUI.backgroundColor = Color.white;
            }
            EditorGUILayout.EndScrollView();
        }
        EditorGUILayout.EndVertical();

        

       
    }

    #endregion


    #region Mouse Events

    protected override void OnMouseDown(MouseButton button, Vector2 position)
    {
        base.OnMouseDown(button, position);
        this.MouseDown(position);
        Repaint();
    }

    protected override void OnMouseDrag(MouseButton button, Vector2 position, Vector2 delta)
    {
        base.OnMouseDrag(button, position, delta);
        this.MouseDrag(position);
        Repaint();
    }

    protected override void OnMouseMove(Vector2 position, Vector2 delta)
    {
        base.OnMouseMove(position, delta);
    }

    protected override void OnMouseUp(MouseButton button, Vector2 position)
    {
        base.OnMouseUp(button, position);
        this.MouseUp(position);
        Repaint();
    }

    #endregion


    #region Utils

    private void AddNewSpwanItem(eSpwanedColorType spwanItemType)
    {
        if (_currentSequance != null && _currentWave != null)
        {
            SpawnedItemDataObject newSpwanItem = new SpawnedItemDataObject();
            newSpwanItem.SpwanedColor = spwanItemType;
            newSpwanItem.XPosition = 0;
            newSpwanItem.ForceVector = new Vector2(0, 100);
            _currentWave.SpawnedItems.Add(newSpwanItem);
        }
    }

    private void LoadWavesFromFile()
    {
        LoadWavesdata();
        return;
    }

    private void LoadTextures()
    {
        _leftBottomTexture = AssetDatabase.LoadAssetAtPath(TEXTURES_PATH + "icon_bottom_left.png", typeof(Texture2D)) as Texture2D;
        _leftTopTexture = AssetDatabase.LoadAssetAtPath(TEXTURES_PATH + "icon_top_left.png", typeof(Texture2D)) as Texture2D;
        _rightBottomTexture = AssetDatabase.LoadAssetAtPath(TEXTURES_PATH + "icon_bottom_right.png", typeof(Texture2D)) as Texture2D;
        _rightTopTexture = AssetDatabase.LoadAssetAtPath(TEXTURES_PATH + "icon_top_right.png", typeof(Texture2D)) as Texture2D;
        _randomTexture = AssetDatabase.LoadAssetAtPath(TEXTURES_PATH + "icon_random.png", typeof(Texture2D)) as Texture2D;
        _arrowTexture = AssetDatabase.LoadAssetAtPath(TEXTURES_PATH + "arrow.png", typeof(Texture2D)) as Texture2D;
    }

    private void LoadWavesdata()
    {
        if (AssetDatabase.LoadAssetAtPath(WAVES_DATA_PATH, typeof(WavesData)) != null)
        {
            _wavesData = AssetDatabase.LoadAssetAtPath(WAVES_DATA_PATH, typeof(WavesData)) as WavesData;
            _sequances = _wavesData.SequancesList;
        }
        else
        {
            Debug.LogError("ERROR - Could not find WavesData file");
        }
    }

    private void PlayTestCurrentWave()
    {

        EditorApplication.OpenScene("Assets/Chromania/Scenes/GameplayScene.unity");
        EditorApplication.isPlaying = true;

        SpwanerController spwanerController = GameObject.FindObjectOfType<SpwanerController>();
        if (spwanerController != null && _currentSequance != null)
        {
            List<SequanceDataObject> sequances = new List<SequanceDataObject>();
            sequances.Add(_currentSequance);
            spwanerController.PlaySequanceForEditMode(sequances);
        }
    }

    private void PlayTestSelecte()
    {
        EditorApplication.playmodeStateChanged += HandlePlayTestSelected;
    
        EditorApplication.OpenScene("Assets/Chromania/Scenes/GameplayScene.unity");
        EditorApplication.isPlaying = true;

      
    }

    private void HandlePlayTestSelected()
    {
        SpwanerController spwanerController = GameObject.FindObjectOfType<SpwanerController>();
        if (spwanerController != null)
        {
            List<SequanceDataObject> sequances = new List<SequanceDataObject>();
            foreach (SequanceDataObject sequance in _sequances)
            {
                if (sequance.Selected)
                {
                    sequances.Add(sequance);
                }
            }

           

            spwanerController.OnSequancesLoaded += () =>
            {
                Debug.Log(">>>>>>>>>>>>>>>>>> override sequances for editor");
                spwanerController.PlaySequanceForEditMode(sequances);
            };
            Debug.Log("override sequances for editor");
        }
    }

    private void MergeJsonFile()
    {
        TextAsset textAsset = Resources.Load("ChromaniaWavesData") as TextAsset;

        JSONObject j = new JSONObject(textAsset.text);
        JSONObject waves = j["wavesArray"];
        List<WaveDefenition> JSONWaves = new List<WaveDefenition>();
        for (int i = 0; i < waves.Count; i++)
        {
            JSONObject waveData = waves[i];
            WaveDefenition wave = new WaveDefenition(waveData);
            JSONWaves.Add(wave);
        }


        foreach (WaveDefenition JSONWave in JSONWaves)
        {
            SequanceDataObject newSequance = new SequanceDataObject();

            newSequance.Identifier = "sequance." + JSONWave.ID;
            newSequance.MinLevel = JSONWave.MinLevel;
            newSequance.MaxLevel = JSONWave.MaxLevel;
            newSequance.GameMode = JSONWave.GameMode;
            newSequance.LevelModifier = JSONWave.LevelModier;
            newSequance.Enabled = true;

            if (JSONWave.StartDelay > 0)
            {
                WaveDataObject delayWave = new WaveDataObject();
                delayWave.Delay = JSONWave.StartDelay;
                delayWave.Enabled = true;
                newSequance.Waves.Add(delayWave);
            }

            foreach (SequanceDefenition JSONSequance in JSONWave.SequanceList)
            {
                if (JSONSequance.StartInterval > 0)
                {
                    WaveDataObject delayWave = new WaveDataObject();
                    delayWave.Delay = JSONSequance.StartInterval;
                    delayWave.Enabled = true;
                    newSequance.Waves.Add(delayWave);
                }

                WaveDataObject newWave = new WaveDataObject();
                
                foreach (SpwanedItemDefenition JSONSpwanItem in JSONSequance.SpwanItemList)
                {
                    SpawnedItemDataObject newSpwanItem = new SpawnedItemDataObject();

                    newSpwanItem.SpwanedColor = JSONSpwanItem.SpwanedColor;
                    newSpwanItem.ForceVector = JSONSpwanItem.ForceVector;
                    newSpwanItem.XPosition = JSONSpwanItem.XPosition;

                    newWave.SpawnedItems.Add(newSpwanItem);
                }

                if (newWave.SpawnedItems.Count > 0)
                {
                    newSequance.Waves.Add(newWave);
                }

                if (JSONSequance.EndInterval > 0)
                {
                    WaveDataObject delayWave = new WaveDataObject();
                    delayWave.Delay = JSONSequance.EndInterval;
                    delayWave.Enabled = true;
                    newSequance.Waves.Add(delayWave);
                }
            }

            if (JSONWave.EndDelay > 0)
            {
                WaveDataObject delayWave = new WaveDataObject();
                delayWave.Delay = JSONWave.EndDelay;
                delayWave.Enabled = true;
                newSequance.Waves.Add(delayWave);
            }

            _sequances.Add(newSequance);
        }
    }

    #endregion
}
