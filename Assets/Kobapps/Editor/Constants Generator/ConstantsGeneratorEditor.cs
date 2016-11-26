using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using UnityEditorInternal;
using System.IO;
using System.Text.RegularExpressions;

namespace Kobapps
{
    public class ConstantsGeneratorEditor : EditorWindow
    {

        #region Private Properties

        [SerializeField]
        private ConstantsGeneratorSettings _settings = null;

        private const string CONSTANTS_SETTINGS_ASSET_LABEL = "ConstantsGeneratorSettings";

        private const string CONSTANTS_FILE_NAME = "GeneratedConstants";

        private Vector2 _scrollPosition = Vector2.zero;

        private Dictionary<ConstantCategory, ReorderableList> _listsCache = new Dictionary<ConstantCategory, ReorderableList>();

        private ConstantCategory _categoryToDelete = null;

        private List<object> _errors;

        #endregion

        [MenuItem("Kobapps/Constants Generator")]
        public static void DisplayConstantsGeneratorWindow()
        {
            EditorWindow window = EditorWindow.GetWindow<ConstantsGeneratorEditor>();
            window.titleContent = new GUIContent("Constants Generator Editor");
        }

        #region Initialization

        void OnEnable()
        {
            _settings = ConstantsGeneratorEditor.LoadSettings();
        }

        #endregion


        #region GUI

        void OnGUI()
        {
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
            {
                if (Settings == null)
                {
                    if (GUILayout.Button("INSTALL", GUILayout.Height(100)))
                    {
                        ConstantsGeneratorEditor.GenerateSettingsAsset();
                    }
                }
                else
                {
                    DrawHeader();

                    EditorGUILayout.Space();

                    if (Settings.Categories != null)
                    {
                        for (int i = 0; i < Settings.Categories.Count; i++)
                        {
                            ConstantCategory category = Settings.Categories[i];
                            DrawCategory(category);
                        }
                    }
                    else
                    {
                        Settings.Categories = new List<ConstantCategory>();
                    }

                }
            }
            GUILayout.EndScrollView();

            if (_categoryToDelete != null && Settings != null && Settings.Categories != null && Settings.Categories.Contains(_categoryToDelete))
            {
                Settings.Categories.Remove(_categoryToDelete);
            }
        }

        private void DrawHeader()
        {
            EditorGUILayout.BeginVertical("Box");
            {

                EditorGUILayout.BeginVertical("Box");
                {
                    EditorGUILayout.LabelField("Auto Generate:");

                    EditorGUILayout.BeginHorizontal();
                    {
                        Settings.GenerateSortingLayers = EditorGUILayout.Toggle("Sorting Layers", Settings.GenerateSortingLayers);
                        Settings.GenerateLayers = EditorGUILayout.Toggle("Layers", Settings.GenerateLayers);

                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    {
                        Settings.GenerateTags = EditorGUILayout.Toggle("Tags", Settings.GenerateTags);
                        Settings.GenerateScenes = EditorGUILayout.Toggle("Scenes", Settings.GenerateScenes);

                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    {
                        Settings.GenerateBundleVersion = EditorGUILayout.Toggle("Bundle Version", Settings.GenerateBundleVersion);
                        Settings.GenerateBundleIdentifier = EditorGUILayout.Toggle("Bundle Identifer", Settings.GenerateBundleIdentifier);

                    }
                    EditorGUILayout.EndHorizontal();

                }
                EditorGUILayout.EndVertical();


                EditorGUILayout.BeginHorizontal("Box");
                {

                    if (GUILayout.Button("Generate Code", GUILayout.Height(40)))
                    {
                        if (!CheckForErrors())
                        {
                            EditorUtility.SetDirty(_settings);

                            AssetDatabase.SaveAssets();
                            AssetDatabase.Refresh();

                            ConstantsGeneratorEditor.GenerateConstantsCode(Settings);


                        }
                    }

                    if (GUILayout.Button("New Category", GUILayout.Height(40)))
                    {
                        ConstantCategory newCategory = new ConstantCategory();
                        newCategory.Name = "Unamed Category";
                        newCategory.Constants = new List<ConstantValuePair>();
                        newCategory.IsOpned = true;

                        Settings.Categories.Add(newCategory);
                    }

                    if (GUILayout.Button("Select Asset", GUILayout.Height(40)))
                    {
                        string[] assetsGuids = AssetDatabase.FindAssets("l:" + CONSTANTS_SETTINGS_ASSET_LABEL);
                        string assetPath = AssetDatabase.GUIDToAssetPath(assetsGuids[0]);
                        Object settingsAsset = AssetDatabase.LoadMainAssetAtPath(assetPath);

                        Selection.activeObject = settingsAsset;
                    }

                    if (GUILayout.Button("Select Output", GUILayout.Height(40)))
                    {
                        string[] assetsGuids = AssetDatabase.FindAssets(CONSTANTS_FILE_NAME);
                        string assetPath = AssetDatabase.GUIDToAssetPath(assetsGuids[0]);
                        Object generatedConstantsScript = AssetDatabase.LoadMainAssetAtPath(assetPath);

                        Selection.activeObject = generatedConstantsScript;
                    }

                }
                EditorGUILayout.EndHorizontal();

            }
            EditorGUILayout.EndVertical();
        }

        private void DrawCategory(ConstantCategory category)
        {
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.BeginHorizontal("Box");
                {
                    if (category.IsOpned)
                    {
                        category.Name = EditorGUILayout.TextField(category.Name, GUILayout.Width(150));
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("X", GUILayout.Width(25)))
                        {
                            _categoryToDelete = category;
                        }
                    }
                    else
                    {
                        EditorGUILayout.LabelField(category.Name);
                    }


                    if (GUILayout.Button(category.IsOpned ? "▲" : "▼", GUILayout.Width(25)))
                    {
                        category.IsOpned = !category.IsOpned;
                    }
                }
                EditorGUILayout.EndHorizontal();


                if (_errors != null && _errors.Contains((object)category))
                {
                    EditorGUILayout.HelpBox("Category '" + category.Name + "' already exists", MessageType.Error);
                }

                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.FlexibleSpace();

                    EditorGUILayout.BeginVertical(GUILayout.Width(position.width * 0.9f));
                    {

                        if (category.IsOpned)
                        {
                            ReorderableList list = ListForCategory(category);
                            //serializedObject.Update();
                            if (list != null)
                            {
                                list.DoLayoutList();
                            }
                            //serializedObject.ApplyModifiedProperties();
                        }
                    }
                    EditorGUILayout.EndVertical();

                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();

        }

        #endregion


        #region Private

        private ReorderableList ListForCategory(ConstantCategory category)
        {
            if (_listsCache.ContainsKey(category) && _listsCache[category] != null)
            {
                return _listsCache[category];
            }

            ReorderableList list = new ReorderableList(category.Constants, typeof(ConstantValuePair));

            list.drawElementCallback = (rect, index, active, focused) => {
                ConstantValuePair element = category.Constants[index];
                if (_errors != null && _errors.Contains((object)element))
                {
                    GUI.color = Color.red;
                }
                element.Key = EditorGUI.TextField(new Rect(rect.x, rect.y, rect.width * 0.3f, EditorGUIUtility.singleLineHeight), element.Key);
                element.ValueType = (eConstantValueType)EditorGUI.EnumPopup(new Rect(rect.x + rect.width * 0.32f, rect.y, rect.width * 0.12f, EditorGUIUtility.singleLineHeight), element.ValueType);

                Rect valueRect = new Rect(rect.x + rect.width * 0.46f, rect.y, rect.width * 0.5f, EditorGUIUtility.singleLineHeight);
                switch (element.ValueType)
                {
                    case eConstantValueType.String:
                        {
                            element.StringValue = EditorGUI.TextField(valueRect, element.StringValue);
                            break;
                        }
                    case eConstantValueType.Int:
                        {
                            element.IntValue = EditorGUI.IntField(valueRect, element.IntValue);
                            break;
                        }
                    case eConstantValueType.Float:
                        {
                            element.FloatValue = EditorGUI.FloatField(valueRect, element.FloatValue);
                            break;
                        }
                    case eConstantValueType.Color:
                        {
                            element.ColorValue = EditorGUI.ColorField(valueRect, element.ColorValue);
                            break;
                        }
                    case eConstantValueType.Vector3:
                        {
                            element.Vector3Value = EditorGUI.Vector3Field(valueRect, "", element.Vector3Value);
                            break;
                        }
                    case eConstantValueType.Vector2:
                        {
                            element.Vector2Value = EditorGUI.Vector2Field(valueRect, "", element.Vector2Value);
                            break;
                        }
                }

                GUI.color = Color.white;
            };

            list.drawHeaderCallback = rect => {
                EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width * 0.4f, EditorGUIUtility.singleLineHeight), "Key");
                EditorGUI.LabelField(new Rect(rect.x + rect.width * 0.42f, rect.y, rect.width * 0.5f, EditorGUIUtility.singleLineHeight), "Value");
            };

            list.onChangedCallback = changedList =>
            {
                EditorUtility.SetDirty(_settings);
            };

            _listsCache.Add(category, list);

            return list;
        }

        private ConstantsGeneratorSettings Settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = LoadSettings();
                }
                return _settings;
            }
        }

        private static ConstantsGeneratorSettings LoadSettings()
        {
            string[] assetsGuids = AssetDatabase.FindAssets("l: " + CONSTANTS_SETTINGS_ASSET_LABEL);

            foreach (string guid in assetsGuids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                ConstantsGeneratorSettings constantGeneratorSettings = (ConstantsGeneratorSettings)AssetDatabase.LoadAssetAtPath(assetPath, typeof(ConstantsGeneratorSettings));
                if (constantGeneratorSettings != null)
                {
                    return constantGeneratorSettings;
                }
            }
            return null;
        }

        private static void GenerateSettingsAsset()
        {
            ConstantsGeneratorSettings asset = ScriptableObject.CreateInstance<ConstantsGeneratorSettings>();
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path == "")
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != "")
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }

            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + CONSTANTS_SETTINGS_ASSET_LABEL + ".asset");

            AssetDatabase.CreateAsset(asset, assetPathAndName);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;

            string[] assetLabels = new string[] { CONSTANTS_SETTINGS_ASSET_LABEL };
            AssetDatabase.SetLabels(asset, assetLabels);
        }

        private bool CheckForErrors()
        {
            _errors = new List<object>();

            foreach (ConstantCategory category in Settings.Categories)
            {
                foreach (ConstantValuePair constant in category.Constants)
                {
                    if (constant.Key == category.Name || string.IsNullOrEmpty(constant.Key))
                    {
                        _errors.Add(constant);
                    }

                }

                foreach (ConstantCategory otherCategory in Settings.Categories)
                {
                    if (otherCategory != category && otherCategory.Name == category.Name)
                    {
                        _errors.Add(category);
                    }
                }
            }



            return (_errors.Count > 0);
        }

        #endregion


        #region Code Generation

        public static void GenerateConstantsCode(ConstantsGeneratorSettings settings)
        {
            string filePath = string.Empty;
            foreach (var file in Directory.GetFiles(Application.dataPath, "*.cs", SearchOption.AllDirectories))
            {
                if (Path.GetFileNameWithoutExtension(file) == CONSTANTS_FILE_NAME)
                {
                    filePath = file;
                    break;
                }
            }
            if (string.IsNullOrEmpty(filePath))
            {
                string directory = EditorUtility.OpenFolderPanel("Choose location for " + CONSTANTS_FILE_NAME + ".cs", Application.dataPath, "");
                if (string.IsNullOrEmpty(directory))
                {
                    return;
                }

                filePath = Path.Combine(directory, CONSTANTS_FILE_NAME + ".cs");
            }

            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine("// This file is auto-generated. Modifications are not saved.");
                writer.WriteLine("using UnityEngine;");
                writer.WriteLine();
                writer.WriteLine("namespace " + CONSTANTS_FILE_NAME);
                writer.WriteLine("{");

                foreach (ConstantCategory category in settings.Categories)
                {

                    writer.WriteLine("    /// <summary>");
                    writer.WriteLine("    ///Category '{0}'.", category.Name);
                    writer.WriteLine("    /// </summary>");
                    string safeCategoryName = MakeSafeForCode(category.Name);
                    writer.WriteLine("    public static class {0}", safeCategoryName);
                    if (category.Name != safeCategoryName)
                    {
                        category.Name = safeCategoryName;
                    }
                    writer.WriteLine("    {");
                    foreach (ConstantValuePair constanValuePair in category.Constants)
                    {
                        string safeForCodeKey = MakeSafeForCode(constanValuePair.Key);
                        writer.WriteLine("        /// <summary>");
                        writer.WriteLine("        /// {0} Value for key '{1}'.", constanValuePair.ValueType.ToString(), safeForCodeKey);
                        writer.WriteLine("        /// </summary>");

                        switch (constanValuePair.ValueType)
                        {
                            case eConstantValueType.String:
                                {
                                    writer.WriteLine("        public const string {0} = \"{1}\";", safeForCodeKey, constanValuePair.StringValue);
                                    break;
                                }
                            case eConstantValueType.Int:
                                {
                                    writer.WriteLine("        public const int {0} = {1};", safeForCodeKey, constanValuePair.IntValue);
                                    break;
                                }
                            case eConstantValueType.Float:
                                {
                                    writer.WriteLine("        public const float {0} = {1}f;", safeForCodeKey, constanValuePair.FloatValue);
                                    break;
                                }
                            case eConstantValueType.Color:
                                {
                                    writer.WriteLine("        public static Color {0} = new Color({1}f,{2}f,{3}f,{4}f);", safeForCodeKey,
                                        constanValuePair.ColorValue.r, constanValuePair.ColorValue.g,
                                        constanValuePair.ColorValue.b, constanValuePair.ColorValue.a);

                                    break;
                                }
                            case eConstantValueType.Vector3:
                                {
                                    writer.WriteLine("        public static Vector3 {0} = new Vector3({1}f,{2}f,{3}f);", safeForCodeKey,
                                        constanValuePair.Vector3Value.x, constanValuePair.Vector3Value.y, constanValuePair.Vector3Value.z);
                                    break;
                                }
                            case eConstantValueType.Vector2:
                                {
                                    writer.WriteLine("        public static Vector2 {0} = new Vector2({1}f,{2}f);", safeForCodeKey,
                                        constanValuePair.Vector3Value.x, constanValuePair.Vector3Value.y);
                                    break;
                                }
                        }



                        if (safeForCodeKey != constanValuePair.Key)
                        {
                            constanValuePair.Key = safeForCodeKey;
                        }
                    }
                    writer.WriteLine("    }");
                    writer.WriteLine();
                }


                if (settings.GenerateTags)
                {
                    writer.WriteLine("    public static class Tags");
                    writer.WriteLine("    {");
                    foreach (var tag in UnityEditorInternal.InternalEditorUtility.tags)
                    {
                        writer.WriteLine("        /// <summary>");
                        writer.WriteLine("        /// Name of tag '{0}'.", tag);
                        writer.WriteLine("        /// </summary>");
                        writer.WriteLine("        public const string {0} = \"{1}\";", MakeSafeForCode(tag), tag);
                    }
                    writer.WriteLine("    }");
                    writer.WriteLine();
                }

                if (settings.GenerateLayers)
                {
                    writer.WriteLine("    public static class Layers");
                    writer.WriteLine("    {");
                    for (int i = 0; i < 32; i++)
                    {
                        string layer = UnityEditorInternal.InternalEditorUtility.GetLayerName(i);
                        if (!string.IsNullOrEmpty(layer))
                        {
                            writer.WriteLine("        /// <summary>");
                            writer.WriteLine("        /// Index of layer '{0}'.", layer);
                            writer.WriteLine("        /// </summary>");
                            writer.WriteLine("        public const int {0} = {1};", MakeSafeForCode(layer), i);
                        }
                    }
                    writer.WriteLine();
                    for (int i = 0; i < 32; i++)
                    {
                        string layer = UnityEditorInternal.InternalEditorUtility.GetLayerName(i);
                        if (!string.IsNullOrEmpty(layer))
                        {
                            writer.WriteLine("        /// <summary>");
                            writer.WriteLine("        /// Bitmask of layer '{0}'.", layer);
                            writer.WriteLine("        /// </summary>");
                            writer.WriteLine("        public const int {0}Mask = 1 << {1};", MakeSafeForCode(layer), i);
                        }
                    }
                    writer.WriteLine("    }");
                    writer.WriteLine();
                }

                if (settings.GenerateScenes)
                {
                    writer.WriteLine("    public static class Scenes");
                    writer.WriteLine("    {");
                    int sceneIndex = 0;
                    foreach (var scene in EditorBuildSettings.scenes)
                    {
                        if (!scene.enabled)
                        {
                            continue;
                        }

                        var sceneName = Path.GetFileNameWithoutExtension(scene.path);

                        writer.WriteLine("        /// <summary>");
                        writer.WriteLine("        /// ID of scene '{0}'.", sceneName);
                        writer.WriteLine("        /// </summary>");
                        writer.WriteLine("        public const int {0} = {1};", MakeSafeForCode(sceneName), sceneIndex);

                        sceneIndex++;
                    }

                    sceneIndex = 0;
                    foreach (var scene in EditorBuildSettings.scenes)
                    {
                        if (!scene.enabled)
                        {
                            continue;
                        }

                        var sceneName = Path.GetFileNameWithoutExtension(scene.path);

                        writer.WriteLine("        /// <summary>");
                        writer.WriteLine("        /// Name of scene '{0}'.", sceneName);
                        writer.WriteLine("        /// </summary>");
                        writer.WriteLine("        public const string {0} = \"{1}\";", MakeSafeForCode(sceneName) + "Name", sceneName);

                        sceneIndex++;
                    }

                    writer.WriteLine("    }");
                    writer.WriteLine();
                }

                if (settings.GenerateSortingLayers)
                {
                    writer.WriteLine("    public static class SortingLayers");
                    writer.WriteLine("    {");
                    foreach (var layer in SortingLayer.layers)
                    {
                        writer.WriteLine("        /// <summary>");
                        writer.WriteLine("        /// ID of sorting layer '{0}'.", layer.name);
                        writer.WriteLine("        /// </summary>");
                        writer.WriteLine("        public const int {0} = {1};", MakeSafeForCode(layer.name), layer.id);
                    }
                    writer.WriteLine("    }");
                    writer.WriteLine();
                }


                writer.WriteLine("    public static class PlayerSettings");
                writer.WriteLine("    {");

                if (settings.GenerateBundleIdentifier)
                {
                    writer.WriteLine("        /// <summary>");
                    writer.WriteLine("        /// Bundle Identifier");
                    writer.WriteLine("        /// </summary>");
                    writer.WriteLine("        public const string BundleIdentifier = \"{0}\";", PlayerSettings.bundleIdentifier);
                }

                if (settings.GenerateBundleVersion)
                {
                    writer.WriteLine("        /// <summary>");
                    writer.WriteLine("        /// Bundle Version");
                    writer.WriteLine("        /// </summary>");
                    writer.WriteLine("        public const string BundleVersion = \"{0}\";", PlayerSettings.bundleVersion);
                }

                writer.WriteLine("    }");
                writer.WriteLine();


                writer.WriteLine("}");
                writer.WriteLine();
            }

            AssetDatabase.Refresh();

        }

        private static string MakeSafeForCode(string str)
        {
            str = Regex.Replace(str, "[^a-zA-Z0-9_]", "_", RegexOptions.Compiled);
            if (char.IsDigit(str[0]))
            {
                str = "_" + str;
            }
            return str;
        }

        #endregion
    }

}
