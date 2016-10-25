using UnityEngine;
using System.Collections;

namespace Kobapps
{
    public class FPSDisplay : MonoBehaviour
    {
        [SerializeField]
        private bool _enabled = true;

        [SerializeField]
        private bool _dontDestoryOnLoad = false;

        float timeCount = 0.0f;

        void Start()
        {
            if (_dontDestoryOnLoad)
            {
                DontDestroyOnLoad(this.gameObject);
            }
        }

        void Update()
        {
            timeCount += (Time.deltaTime - timeCount) * 0.1f;
        }

        void OnGUI()
        {
            if (_enabled)
            {
                int w = Screen.width, h = Screen.height;
                GUIStyle style = new GUIStyle();
                Rect rect = new Rect(0, Screen.height - (h * 2 / 25), w, h * 2 / 25);
                style.alignment = TextAnchor.UpperLeft;
                style.fontSize = h * 2 / 25;
                style.normal.textColor = Color.white;
                float msec = timeCount * 1000.0f;
                float fps = 1.0f / timeCount;
                string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
                GUI.Label(rect, text, style);
            }
        }
    }
}