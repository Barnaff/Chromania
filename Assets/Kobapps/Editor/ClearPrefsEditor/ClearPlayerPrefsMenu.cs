using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Kobapps
{
    public class ClearPlayerPrefsMenu
    {

        [MenuItem("Kobapps/Clear Playerprefs/Delete All PlayerPrefs")]
        public static void ClearPlayerPrefs()
        {
            if (EditorUtility.DisplayDialog("Delete all editor preferences?",
               "Are you sure you want to delete all the editor preferences?, \n this action cannot be undone.",
                "Yes",
               "No"))
                PlayerPrefs.DeleteAll();
        }

        [MenuItem("Kobapps/Clear Playerprefs/Delete All EditorPrefs")]
        public static void ClearEditorPrefs()
        {
            if (EditorUtility.DisplayDialog("Delete all editor preferences?",
               "Are you sure you want to delete all the editor preferences?, \n this action cannot be undone.",
                "Yes",
               "No"))
                EditorPrefs.DeleteAll();
        }
    }
}


