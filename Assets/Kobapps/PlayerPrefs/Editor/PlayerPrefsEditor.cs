using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

namespace Kobapps.Editor
{
    public class PlayerPrefsEditor : EditorWindow
    {

        private const float LIST_ITEM_HEIGHT = 30.0f;
        private Dictionary<string, PlayerPrefsUtil.PlayerPrefsUtilItemType> _allKeys;
        private Vector2 _scrollPosition = Vector2.zero;

        private string errorMessage = "Click Edit to change item";

        private string _currentEditKey = null;
        private string _currentEditValue;
        private PlayerPrefsUtil.PlayerPrefsUtilItemType _currentEditType;

        private float _panelWidth = 0;

        private bool _isAddingNewKey = false;

        [MenuItem("Kobapps/Player Prefs Editor")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(PlayerPrefsEditor));
        }

        void OnGUI()
        {

            int numberOfItems = _allKeys.Count + 1;
            float scrollViewHeight = (LIST_ITEM_HEIGHT * numberOfItems);
            float scrollViewFrameHeight = position.height;
            float scrollViewOriginY = 0.0f;
            bool shouldScroll = true;
            errorMessage = "Click Edit to change item";
            if (!string.IsNullOrEmpty(errorMessage))
            {
                scrollViewFrameHeight -= 50;
                EditorGUILayout.HelpBox(errorMessage, MessageType.Info);
            }

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Reload Keys"))
            {
                PlayerPrefsUtil.ReloadAllKeys();
                LoadPlayerPrefs();
            }

            if (GUILayout.Button("Clear Cache"))
            {
                PlayerPrefsUtil.ClearCache();
            }
            GUI.color = Color.red;
            if (GUILayout.Button("Delete PlayerPrefs"))
            {
                PlayerPrefsUtil.DeleteAll();
            }
            GUI.color = Color.white;

            if (_isAddingNewKey)
            {
                GUILayout.EndHorizontal();
                GUI.color = Color.cyan;
                GUILayout.BeginHorizontal("Box");

                _currentEditKey = EditorGUILayout.TextField(_currentEditKey);
                _currentEditType = (PlayerPrefsUtil.PlayerPrefsUtilItemType)EditorGUILayout.EnumPopup(_currentEditType, GUILayout.Width(_panelWidth * 0.1f));
                _currentEditValue = EditorGUILayout.TextField(_currentEditValue);
                GUI.color = Color.green;
                if (GUILayout.Button("Save"))
                {
                    switch (_currentEditType)
                    {
                        case PlayerPrefsUtil.PlayerPrefsUtilItemType.Int:
                            {
                                PlayerPrefsUtil.SetInt(_currentEditKey, int.Parse(_currentEditValue));
                                break;
                            }
                        case PlayerPrefsUtil.PlayerPrefsUtilItemType.Float:
                            {
                                PlayerPrefsUtil.SetFloat(_currentEditKey, float.Parse(_currentEditValue));
                                break;
                            }
                        case PlayerPrefsUtil.PlayerPrefsUtilItemType.String:
                            {
                                PlayerPrefsUtil.SetString(_currentEditKey, _currentEditValue);
                                break;
                            }
                        case PlayerPrefsUtil.PlayerPrefsUtilItemType.Bool:
                            {
                                PlayerPrefsUtil.SetBool(_currentEditKey, bool.Parse(_currentEditValue));
                                break;
                            }
                        case PlayerPrefsUtil.PlayerPrefsUtilItemType.Obj:
                            {
                                PlayerPrefsUtil.SetObject(_currentEditKey, (object)_currentEditValue);
                                break;
                            }
                    }

                    _isAddingNewKey = false;
                    _currentEditKey = null;
                    _currentEditValue = null;
                    LoadPlayerPrefs();
                }
                GUI.color = Color.red;
                if (GUILayout.Button("X"))
                {
                    _isAddingNewKey = false;
                }
                GUI.color = Color.white;

                GUILayout.EndHorizontal();
            }
            else
            {
                if (GUILayout.Button("Add New Key"))
                {
                    _isAddingNewKey = true;
                    _currentEditKey = "Key";
                    _currentEditValue = "Value";
                }
                GUILayout.EndHorizontal();
            }

            if (scrollViewHeight < scrollViewFrameHeight)
            {
                scrollViewHeight = scrollViewFrameHeight;
                shouldScroll = false;
                _panelWidth = position.size.x - 5.0f; ;
            }
            else
            {
                _panelWidth = position.size.x - 20.0f;
            }
            _scrollPosition = GUI.BeginScrollView(new Rect(0, scrollViewOriginY, position.width, scrollViewFrameHeight), _scrollPosition, new Rect(0, 0, 0, scrollViewHeight), false, shouldScroll);

            int indexCount = 0;

            List<string> keyList = new List<string>();

            foreach (string key in _allKeys.Keys)
            {
                keyList.Add(key);
            }

            foreach (string key in keyList)
            {
                KeyItemPanel(key, _allKeys[key], indexCount);
                indexCount++;
            }

            GUI.EndScrollView();
        }

        void OnEnable()
        {
            _allKeys = new Dictionary<string, PlayerPrefsUtil.PlayerPrefsUtilItemType>();
            LoadPlayerPrefs();
        }

        void OnDisable()
        {
            _allKeys = null;
            SavedPlayerPrefs();
        }

        private void LoadPlayerPrefs()
        {
            _allKeys = PlayerPrefsUtil.GetAllKeys();
        }

        private void SavedPlayerPrefs()
        {
            PlayerPrefsUtil.Save();
        }

        private void KeyItemPanel(string keyName, PlayerPrefsUtil.PlayerPrefsUtilItemType valueType, int index)
        {

            string keyValue = "";

            if (PlayerPrefsUtil.HasKey(keyName))
            {
                keyValue = PlayerPrefsUtil.GetString(keyName);

                if (string.IsNullOrEmpty(keyValue))
                {
                    keyValue = PlayerPrefsUtil.GetInt(keyName).ToString();
                }
                if (string.IsNullOrEmpty(keyValue))
                {
                    keyValue = PlayerPrefsUtil.GetFloat(keyName).ToString();
                }
            }

            bool isEditable = (keyName == _currentEditKey);

            if (isEditable)
            {
                GUI.backgroundColor = Color.cyan;
                EditorGUILayout.BeginHorizontal("Box");
                EditorGUILayout.LabelField(keyName, GUILayout.Width(_panelWidth * 0.23f));
                _currentEditType = (PlayerPrefsUtil.PlayerPrefsUtilItemType)EditorGUILayout.EnumPopup(_currentEditType, GUILayout.Width(_panelWidth * 0.1f));
                _currentEditValue = EditorGUILayout.TextField(_currentEditValue, GUILayout.Width(_panelWidth * 0.5f));
                GUI.backgroundColor = Color.red;

                if (GUILayout.Button("X", GUILayout.Width(_panelWidth * 0.05f)))
                {
                    RemoveField(keyName);
                    _currentEditKey = null;
                    _currentEditValue = null;
                }
                GUI.backgroundColor = Color.green;
                if (GUILayout.Button("V", GUILayout.Width(_panelWidth * 0.05f)))
                {
                    SetKey(_currentEditKey, _currentEditType, _currentEditValue);
                    _currentEditKey = null;
                    _currentEditValue = null;
                }
                GUI.backgroundColor = Color.white;


            }
            else
            {
                EditorGUILayout.BeginHorizontal("Box");
                EditorGUILayout.LabelField(keyName, GUILayout.Width(_panelWidth * 0.35f));
                EditorGUILayout.LabelField("(" + valueType.ToString() + ")", GUILayout.Width(_panelWidth * 0.1f));
                EditorGUILayout.LabelField(keyValue, GUILayout.Width(_panelWidth * 0.35f));

                if (GUILayout.Button("Edit", GUILayout.Width(_panelWidth * 0.15f)))
                {
                    _isAddingNewKey = false;
                    _currentEditKey = keyName;
                    _currentEditType = valueType;
                    _currentEditValue = keyValue;
                }

            }

            EditorGUILayout.EndHorizontal();

        }


        private void SetKey(string key, PlayerPrefsUtil.PlayerPrefsUtilItemType valueType, string keyValue)
        {

            switch (valueType)
            {
                case PlayerPrefsUtil.PlayerPrefsUtilItemType.Int:
                    {
                        PlayerPrefsUtil.SetInt(key, int.Parse(keyValue));
                        break;
                    }
                case PlayerPrefsUtil.PlayerPrefsUtilItemType.Float:
                    {
                        PlayerPrefsUtil.SetFloat(key, float.Parse(keyValue));
                        break;
                    }
                case PlayerPrefsUtil.PlayerPrefsUtilItemType.String:
                    {
                        PlayerPrefsUtil.SetString(key, keyValue);
                        break;
                    }
                case PlayerPrefsUtil.PlayerPrefsUtilItemType.Bool:
                    {
                        PlayerPrefsUtil.SetBool(key, bool.Parse(keyValue));
                        break;
                    }
                case PlayerPrefsUtil.PlayerPrefsUtilItemType.Obj:
                    {
                        PlayerPrefsUtil.SetObject(key, keyValue);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        private void RemoveField(string fieldName)
        {
            PlayerPrefsUtil.DeleteKey(fieldName);
        }
    }
}
