using UnityEngine;

namespace Kobapps
{
    public class SceneLoaderutil 
    {
        public static void LoadSceneAsync(string scenename, System.Action sceneLoadedAction)
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
                CreateSceneLoaderController().LoadSceneAsync(scenename, sceneLoadedAction);
            }
        }

        public static void LoadSceneAsync(int sceneBuildIndex, System.Action sceneLoadedAction)
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
                CreateSceneLoaderController().LoadSceneAsync(sceneBuildIndex, sceneLoadedAction);
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
        private AsyncOperation _asyncOperation;

        private System.Action _sceneLoadedAction;

        public void LoadSceneAsync(string sceneName, System.Action sceneLoadedAction)
        {
            _sceneLoadedAction = sceneLoadedAction;
            _asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        }

        public void LoadSceneAsync(int sceneBuildIndex, System.Action sceneLoadedAction)
        {
            _sceneLoadedAction = sceneLoadedAction;
            _asyncOperation =  UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneBuildIndex);
        }

        void Update()
        {
            if (_asyncOperation != null && _asyncOperation.isDone)
            {
                SceneLoaded();
            }
        }

        private void SceneLoaded()
        {
            if (_sceneLoadedAction != null)
            {
                _sceneLoadedAction();
            }
            Destroy(this.gameObject);
        }
    }
}
