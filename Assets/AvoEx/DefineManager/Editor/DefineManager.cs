using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

/* See the "http://avoex.com/avoex/default-license/" for the full license governing this code. */

namespace AvoEx
{
    public class DefineManager : EditorWindow
    {
        public enum COMPILER
        {
            START = 0,
            CSHARP = START,
            CSHARP_EDITOR,
            UNITY_SCRIPT,
            END = UNITY_SCRIPT,
            // 
            COUNT = END - START + 1,
        }

        const int COMPILER_COUNT = (int)COMPILER.COUNT;

        public class GlobalDefine
        {
            const string PATTERN = @"^\w+$";

            public string name { get; private set; }
            bool[] enableSettings = new bool[COMPILER_COUNT];

            public GlobalDefine(string defineName)
            {
                name = defineName;
            }

            public void Rename(string newName)
            {
                name = newName;
            }

            public bool IsEnabled(COMPILER compilerType)
            {
                uint compilerIndex = (uint)compilerType;
                if (compilerIndex < enableSettings.Length)
                    return enableSettings[compilerIndex];

                return false;
            }

            public void SetEnable(COMPILER compilerType, bool isEnable)
            {
                uint compilerIndex = (uint)compilerType;
                if (compilerIndex < enableSettings.Length)
                    enableSettings[compilerIndex] = isEnable;
            }

            public bool IsEnabledAll()
            {
                for (int i = 0; i < enableSettings.Length; ++i)
                {
                    if (enableSettings[i] == false)
                        return false;
                }
                return true;
            }

            public void SetEnableAll(bool isEnable)
            {
                for (int i = 0; i < enableSettings.Length; ++i)
                {
                    enableSettings[i] = isEnable;
                }
            }

            public static bool IsEquals(GlobalDefine a, GlobalDefine b)
            {
                if (a == null && b == null)
                    return true;

                if (a == null || b == null)
                    return false;

                if (a.name != b.name)
                {
                    return false;
                }

                for (int i = 0; i < a.enableSettings.Length; ++i)
                {
                    if (a.enableSettings[i] != b.enableSettings[i])
                        return false;
                }

                return true;
            }

            public static bool IsValidName(string name)
            {
                return Regex.IsMatch(name, PATTERN);
            }
        }

        public class GlobalDefineSetting
        {
            const string GROUP_NAME = @"DEFINE";
            const string PATTERN = @"[-/]d(?:efine)*:(?:;*(?<" + GROUP_NAME + @">\w+))+;*";
            static readonly string[] RSP_FILE_PATHs = new string[COMPILER_COUNT] { "Assets/smcs.rsp", "Assets/gmcs.rsp", "Assets/us.rsp" };
            const string SETTING_FILE_PATH = "Assets/DefineSettings.cs";
            const string RSP_DEFINE_OPTION = "-define:";
            const char DEFINE_DELIMITER = ';';

            static string RspFilePath(COMPILER compilerType)
            {
                uint compilerIndex = (uint)compilerType;
                if (compilerIndex < RSP_FILE_PATHs.Length)
                    return RSP_FILE_PATHs[compilerIndex];
                return "";
            }

            Dictionary<string, GlobalDefine> dicDefines = new Dictionary<string, GlobalDefine>();

            public ICollection<GlobalDefine> DefineCollection
            {
                get { return dicDefines.Values; }
            }

            public GlobalDefine GetData(string defineName)
            {
                if (string.IsNullOrEmpty(defineName))
                    return null;

                GlobalDefine defineData = null;
                dicDefines.TryGetValue(defineName, out defineData);
                if (defineData == null)
                {
                    defineData = new GlobalDefine(defineName);
                    dicDefines[defineName] = defineData;
                }

                return defineData;
            }

            public bool DelData(string defineName)
            {
                return dicDefines.Remove(defineName);
            }

            public bool IsDefined(string defineName)
            {
                return dicDefines.ContainsKey(defineName);
            }

            public void Rename(string oldName, string newName)
            {
                GlobalDefine defineData = null;
                dicDefines.TryGetValue(oldName, out defineData);
                if (defineData == null)
                    return;

                dicDefines.Remove(oldName);
                defineData.Rename(newName);
                dicDefines[defineData.name] = defineData;
            }

            public static bool IsEquals(GlobalDefineSetting a, GlobalDefineSetting b)
            {
                if (a.dicDefines.Count != b.dicDefines.Count)
                    return false;

                foreach (GlobalDefine aDefine in a.dicDefines.Values)
                {
                    if (aDefine == null)
                        continue;

                    GlobalDefine bDefine = null;
                    b.dicDefines.TryGetValue(aDefine.name, out bDefine);
                    if (bDefine == null || GlobalDefine.IsEquals(aDefine, bDefine) == false)
                        return false;
                }

                return true;
            }

            #region LOAD_SETTING
            public void LoadSettings()
            {
                dicDefines.Clear();
                LoadSettingFile();
                LoadRspFile(COMPILER.CSHARP);
                LoadRspFile(COMPILER.CSHARP_EDITOR);
                LoadRspFile(COMPILER.UNITY_SCRIPT);
            }

            void LoadSettingFile()
            {
                if (!File.Exists(SETTING_FILE_PATH))
                    return;

                string[] lines = File.ReadAllLines(SETTING_FILE_PATH);
                if (lines == null || lines.Length <= 0)
                    return;

                for (int i = 1; i < lines.Length; ++i)
                {
                    if (string.IsNullOrEmpty(lines[i]))
                        continue;
                    string defineText = lines[i].Replace("//", "");

                    string[] defines = defineText.Split(DEFINE_DELIMITER);
                    if (defines == null || defines.Length <= 0)
                        continue;

                    foreach (string define in defines)
                    {
                        if (string.IsNullOrEmpty(define))
                            continue;

                        GetData(define);
                    }
                }
            }

            void LoadRspFile(COMPILER compilerType)
            {
                string path = RspFilePath(compilerType);

                if (!File.Exists(path))
                    return;

                string rspOption = File.ReadAllText(path);

                MatchCollection defineMatchs = Regex.Matches(rspOption, PATTERN);
                foreach (Match match in defineMatchs)
                {
                    Group group = match.Groups[GROUP_NAME];
                    foreach (Capture cap in group.Captures)
                    {
                        GlobalDefine define = GetData(cap.Value);
                        if (define != null)
                            define.SetEnable(compilerType, true);
                    }
                }
            }
            #endregion LOAD_SETTING

            #region SAVE_SETTING
            public void SaveSettings()
            {
                SaveSettingFile();
                SaveRspFile(COMPILER.CSHARP);
                SaveRspFile(COMPILER.CSHARP_EDITOR);
                SaveRspFile(COMPILER.UNITY_SCRIPT);

                AssetDatabase.Refresh();
                AssetDatabase.ImportAsset(SETTING_FILE_PATH, ImportAssetOptions.ForceUpdate);
            }

            void SaveSettingFile()
            {
                string settingText = "// DO NOT EDIT OR DELETE THIS FILE. USED FOR 'AvoEx.DefineManager'." + System.Environment.NewLine + "//";
                foreach (GlobalDefine define in dicDefines.Values)
                {
                    settingText += define.name + DEFINE_DELIMITER;
                }

                using (StreamWriter writer = new StreamWriter(SETTING_FILE_PATH))
                {
                    writer.Write(settingText);
                }
            }

            void SaveRspFile(COMPILER compilerType)
            {
                string path = RspFilePath(compilerType);

                string rspOption = "";
                if (File.Exists(path))
                {
                    rspOption = File.ReadAllText(path);

                    MatchCollection defineMatchs = Regex.Matches(rspOption, PATTERN);
                    foreach (Match match in defineMatchs)
                    {
                        rspOption = rspOption.Replace(match.Value, "");
                    }
                }

                string appendDefine = "";
                foreach (GlobalDefine define in dicDefines.Values)
                {
                    if (define == null || define.IsEnabled(compilerType) == false)
                        continue;
                    appendDefine += define.name + DEFINE_DELIMITER;
                }

                if (string.IsNullOrEmpty(appendDefine) == false)
                    rspOption += RSP_DEFINE_OPTION + appendDefine;

                using (StreamWriter writer = new StreamWriter(path))
                {
                    writer.Write(rspOption);
                }
            }
            #endregion SAVE_SETTING
        }

        //
        const string DEFAULT_NAME = "NEW_DEFINE";
        const string STR_DEFINE_NAME_TEXT_FIELD = "EDITING_FIELD_NAME";
        const float DEFINE_LABEL_WIDTH = 80f;
        const float TOP_MENU_BUTTON_WIDTH = 250f;
        const float BUTTON_WIDTH = 65f;
        const float TOGGLE_WIDTH = 120f;
        const float DEFINE_LABEL_LONG_WIDTH = DEFINE_LABEL_WIDTH + BUTTON_WIDTH + BUTTON_WIDTH + 8f;

        GlobalDefineSetting definesSaved = new GlobalDefineSetting();
        GlobalDefineSetting definesEditing = new GlobalDefineSetting();
        List<string> listDeleteReserved = new List<string>();
        bool isInputProcessed = false;
        string newDefineName = DEFAULT_NAME;
        string selectedDefine = "";
        string editingDefine = "";
        string editingName = "";
        bool isEditingName = false;
        bool doRename = false;
        

        //
        Vector2 m_scrollDefines;

        [MenuItem("Edit/Project Settings/Define")]
        [MenuItem("Window/Define Manager")]
        [MenuItem("Tools/AvoEx/Define Manager")]
        public static void Open()
        {
            DefineManager window = EditorWindow.GetWindow<DefineManager>(false, "AvoEx.Defines", true);
            if (window != null)
            {
                window.position = new Rect(200, 200, 515, 300);
                window.minSize = new Vector2(515f, 200f);
            }
        }

        void OnGUI()
        {
            isInputProcessed = false;

            //
            OnGUITopMenu();
            OnGUIAddMenu();

            //
            m_scrollDefines = EditorGUILayout.BeginScrollView(m_scrollDefines);
            listDeleteReserved.Clear();
            int no = 0;
            foreach (GlobalDefine defineItem in definesEditing.DefineCollection)
            {
                OnGUIDefineItem(++no, defineItem);
            }
            foreach (string delName in listDeleteReserved)
            {
                definesEditing.DelData(delName);
            }
            if (doRename)
            {
                doRename = false;
                definesEditing.Rename(editingDefine, editingName);
                EndRename();
            }
            EditorGUILayout.EndScrollView();

            ProcessMouseOut();

            if (isInputProcessed)
                Repaint();
        }

        void OnGUITopMenu()
        {
            EditorGUILayout.BeginHorizontal();

            bool isChanged = !GlobalDefineSetting.IsEquals(definesSaved, definesEditing);

            EditorGUI.BeginDisabledGroup(!isChanged);
            if (isChanged)
            {
                GUI.backgroundColor = Color.red;
            }
            if (GUILayout.Button("Apply", GUILayout.Width(TOP_MENU_BUTTON_WIDTH)))
            {
                definesEditing.SaveSettings();
                definesSaved.LoadSettings();
            }
            if (isChanged)
            {
                GUI.backgroundColor = Color.green;
            }
            if (GUILayout.Button("Revert", GUILayout.Width(TOP_MENU_BUTTON_WIDTH)))
            {
                definesEditing.LoadSettings();
            }
            GUI.backgroundColor = Color.white;
            EditorGUI.EndDisabledGroup();
            
            EditorGUILayout.EndHorizontal();

            if (isChanged)
            {
                EditorGUILayout.HelpBox("You must press Apply button, to apply changes", MessageType.Warning);
            }
        }

        void OnGUIAddMenu()
        {
            EditorGUILayout.BeginHorizontal();

            newDefineName = EditorGUILayout.TextField(newDefineName, GUILayout.MinWidth(TOP_MENU_BUTTON_WIDTH));

            bool isInvalid = false;
            if (GlobalDefine.IsValidName(newDefineName) == false)
            {
                EditorGUILayout.HelpBox("invalid name", MessageType.Error);
                isInvalid = true;
            }
            else if (definesEditing.IsDefined(newDefineName))
            {
                EditorGUILayout.HelpBox("already exist define", MessageType.Error);
                isInvalid = true;
            }

            EditorGUI.BeginDisabledGroup(isInvalid);
            if (isInvalid == false)
            {
                GUI.backgroundColor = Color.cyan;
            }
            if (GUILayout.Button("Add Define", GUILayout.Width(TOGGLE_WIDTH)))
            {
                GlobalDefine newDefine = definesEditing.GetData(newDefineName);
                if (newDefine != null)
                    newDefine.SetEnableAll(true);
                GUI.FocusControl("");
                newDefineName = MakeNewDefineName();
            }
            GUI.backgroundColor = Color.white;
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndHorizontal();
        }

        void OnGUIDefineItem(int no, GlobalDefine defineItem)
        {
            if (defineItem == null)
                return;

            bool isSelected = IsSelectedDefine(defineItem);
            bool isEditing = IsEditingDefine(defineItem);
            bool isEnabledAll = defineItem.IsEnabledAll();

            GUI.backgroundColor = isSelected ? Color.yellow : Color.white;
            Rect rtItem = EditorGUILayout.BeginHorizontal("AS TextArea", GUILayout.MinHeight(25f));
            GUI.backgroundColor = Color.white;

            GUI.color = isEnabledAll ? Color.cyan : Color.black;
            EditorGUILayout.LabelField(no.ToString() + ".", GUILayout.Width(20f));
            GUI.color = Color.white;

            if (isEditing)
            {
                GUI.SetNextControlName(STR_DEFINE_NAME_TEXT_FIELD);
                editingName = EditorGUILayout.TextField(editingName, GUILayout.MinWidth(DEFINE_LABEL_LONG_WIDTH));
                if (isEditingName == false)
                {
                    isEditingName = true;
                    GUI.FocusControl(STR_DEFINE_NAME_TEXT_FIELD);
                }
            }
            else
            {
                GUIStyle style = isEnabledAll ? EditorStyles.whiteLabel : EditorStyles.miniLabel;

                if (isSelected)
                {
                    EditorGUILayout.LabelField(defineItem.name, style, GUILayout.MinWidth(DEFINE_LABEL_WIDTH));
                    if (GUILayout.Button("rename", GUILayout.Width(BUTTON_WIDTH)))
                    {
                        BeginRename(defineItem);
                        isEditingName = false;
                    }
                    if (GUILayout.Button("delete", GUILayout.Width(BUTTON_WIDTH)))
                    {
                        listDeleteReserved.Add(defineItem.name);
                    }
                }
                else
                {
                    EditorGUILayout.LabelField(defineItem.name, style, GUILayout.MinWidth(DEFINE_LABEL_LONG_WIDTH));
                }
            }

            GUI.backgroundColor = isEnabledAll ? Color.cyan : Color.white;
            bool newValue = GUILayout.Toggle(isEnabledAll, "All", EditorStyles.miniButtonLeft, GUILayout.Width(BUTTON_WIDTH));
            if (newValue != isEnabledAll)
            {
                defineItem.SetEnableAll(newValue);
                SetSelectedDefine(defineItem);
            }

            OnGUIDefineItemToggle(defineItem, COMPILER.CSHARP);
            OnGUIDefineItemToggle(defineItem, COMPILER.CSHARP_EDITOR);
            OnGUIDefineItemToggle(defineItem, COMPILER.UNITY_SCRIPT);

            EditorGUILayout.EndHorizontal();

            ProcessInput(rtItem, defineItem);
        }

        void OnGUIDefineItemToggle(GlobalDefine defineItem, COMPILER eType)
        {
            GUIStyle togStyle = EditorStyles.miniButtonMid;
            if (eType == COMPILER.END)
                togStyle = EditorStyles.miniButtonRight;

            string name = "Unity";
            if (eType == COMPILER.CSHARP)
                name = "C#";
            else if (eType == COMPILER.CSHARP_EDITOR)
                name = "C# editor";

            bool isEnabled = defineItem.IsEnabled(eType);
            GUI.backgroundColor = isEnabled ? Color.cyan : Color.white;
            bool newValue = GUILayout.Toggle(isEnabled, name, togStyle, GUILayout.Width(BUTTON_WIDTH));
            if (newValue == isEnabled)
                return;
            defineItem.SetEnable(eType, newValue);
            SetSelectedDefine(defineItem);
        }

        void OnEnable()
        {
            definesSaved.LoadSettings();
            definesEditing.LoadSettings();
            newDefineName = MakeNewDefineName();
        }

        string MakeNewDefineName()
        {
            string newName = DEFAULT_NAME;
            int index = 1;
            while (definesEditing.IsDefined(newName))
            {
                ++index;
                newName = DEFAULT_NAME + "_" + index.ToString();
            }
            return newName;
        }

        #region INPUT_PROCESS
        void SetSelectedDefine(GlobalDefine define)
        {
            if (define == null)
                selectedDefine = "";
            else
                selectedDefine = define.name;
            isInputProcessed = true;
        }

        bool IsSelectedDefine(GlobalDefine define)
        {
            if (define == null)
                return false;

            return selectedDefine == define.name;
        }

        bool IsEditingDefine(GlobalDefine define)
        {
            if (define == null)
                return false;

            return editingDefine == define.name;
        }

        void ProcessInput(Rect rtItem, GlobalDefine define)
        {
            if (isInputProcessed == true || define == null)
                return;

            if (ProcessRename(rtItem, define))
            {
                isInputProcessed = true;
            }
            else if (ProcessMouse(rtItem, define))
            {
                isInputProcessed = true;
            }
        }

        bool ProcessRename(Rect rtItem, GlobalDefine define)
        {
            if (define == null || define.name != editingDefine)
                return false;

            bool isInvalid = false;
            if (GlobalDefine.IsValidName(editingName) == false)
            {
                EditorGUILayout.HelpBox("invalid name", MessageType.Error);
                isInvalid = true;
            }
            else if (editingName != define.name && definesEditing.IsDefined(editingName))
            {
                EditorGUILayout.HelpBox("already exist define", MessageType.Error);
                isInvalid = true;
            }

            if ((Event.current.type == EventType.MouseDown && !UtilEvent.IsClicked(rtItem)) ||
                (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.Return))
            {
                if (isInvalid)
                    EndRename();
                else
                    doRename = true;

                Event.current.Use();
                return true;
            }

            return false;
        }

        void BeginRename(GlobalDefine define)
        {
            if (define == null)
                return;

            editingDefine = define.name;
            editingName = define.name;
        }

        void EndRename()
        {
            editingDefine = "";
            editingName = "";
            GUI.FocusControl("");
            //GUIUtility.keyboardControl = 0;
        }

        bool ProcessMouse(Rect rtItem, GlobalDefine define)
        {
            if (define == null || UtilEvent.IsClicked(rtItem) == false)
                return false;

            if (string.IsNullOrEmpty(editingDefine) == false)
            {
                EndRename();
            }
            if (IsSelectedDefine(define))
            {
                SetSelectedDefine(null);
            }
            else
            {
                SetSelectedDefine(define);
            }

            Event.current.Use();
            return true;
        }

        public void ProcessMouseOut()
        {
            if (isInputProcessed == true)
                return;

            if (Event.current.type == EventType.MouseDown)
            {
                newDefineName = MakeNewDefineName();
                EndRename();
                Event.current.Use();
            }
        }
        #endregion INPUT_PROCESS

    }

}