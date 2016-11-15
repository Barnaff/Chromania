using UnityEngine;
using MovementEffects;
using System.Collections.Generic;

namespace Kobapps
{
    public enum eSceneTransition
    {
        None,
        FadeOut,
        FadeOutFadeIn,
    }

    public class SceneLoaderutil 
    {
        public static void LoadSceneAsync(string scenename, System.Action sceneLoadedAction, eSceneTransition sceneTransition = eSceneTransition.None)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == scenename)
            {
                if (sceneLoadedAction != null)
                {
                    sceneLoadedAction();
                }
            }
            else
            {
                CreateSceneLoaderController().LoadSceneAsync(scenename, sceneLoadedAction, sceneTransition);
            }
        }

        public static void LoadSceneAsync(int sceneBuildIndex, System.Action sceneLoadedAction, eSceneTransition sceneTransition = eSceneTransition.None)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == sceneBuildIndex)
            {
                if (sceneLoadedAction != null)
                {
                    sceneLoadedAction();
                }
            }
            else
            {
                CreateSceneLoaderController().LoadSceneAsync(sceneBuildIndex, sceneLoadedAction, sceneTransition);
            }  
        }

        private static SceneLoaderController CreateSceneLoaderController()
        {
            GameObject sceneLoaderContainer = new GameObject();
            sceneLoaderContainer.name = "Scene Loader";
            SceneLoaderController sceneLoaderController = sceneLoaderContainer.AddComponent<SceneLoaderController>();
            GameObject.DontDestroyOnLoad(sceneLoaderContainer);
            return sceneLoaderController;
        }
    }


    internal class SceneLoaderController : MonoBehaviour
    {

        private float _fadeValue = 0;
        private Texture2D _overlayTexture = null;
        private const float _fadeSpeed = 1f;
        private bool _displayOverlay = false;
        private int _fadeDirection = 1;

        public void LoadSceneAsync(string sceneName, System.Action sceneLoadedAction, eSceneTransition sceneTransition)
        {
            int sceneBuildIndex = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName).buildIndex;
            Timing.RunCoroutine(LoadSceenCorutine(sceneBuildIndex, sceneTransition, sceneLoadedAction));
        }

        public void LoadSceneAsync(int sceneBuildIndex, System.Action sceneLoadedAction, eSceneTransition sceneTransition)
        {
            Timing.RunCoroutine(LoadSceenCorutine(sceneBuildIndex, sceneTransition, sceneLoadedAction));
        }

        private IEnumerator<float> LoadSceenCorutine(int sceneBuildIndex, eSceneTransition sceneTransition, System.Action completionAction)
        {
            sceneTransition = eSceneTransition.FadeOutFadeIn;

            switch (sceneTransition)
            {
                case eSceneTransition.FadeOut:
                case eSceneTransition.FadeOutFadeIn:
                    {
                        CreateOverlayTexture();
                        _fadeValue = 0;
                        _displayOverlay = true;
                        _fadeDirection = 1;
                        while (_fadeValue < 1f)
                        {
                            yield return 0f;
                        }

                        break;
                    }
            }

            AsyncOperation sceneLoadOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneBuildIndex);

            while (!sceneLoadOperation.isDone)
            {
                yield return 0f;
            }

            if (completionAction != null)
            {
                completionAction();
            }

            switch (sceneTransition)
            {
                case eSceneTransition.FadeOutFadeIn:
                    {
                        CreateOverlayTexture();
                        _fadeValue = 1;
                        _displayOverlay = true;
                        _fadeDirection = -1;
                        while (_fadeValue > 0f)
                        {
                            yield return 0f;
                        }

                        break;
                    }
            }

            Destroy(this.gameObject);

        }

        private void CreateOverlayTexture()
        {
            if (_overlayTexture == null)
            {
                _overlayTexture = new Texture2D(1, 1);
                _overlayTexture.SetPixel(0, 0, Color.black);
            }
        }

        void OnGUI()
        {
            if (_displayOverlay)
            {
                _fadeValue += _fadeSpeed * _fadeDirection * Time.deltaTime;
                GUI.color = new Color(0, 0, 0, _fadeValue); ;
                GUI.depth = -1000 ;
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _overlayTexture);
            }
        }

    }
}
