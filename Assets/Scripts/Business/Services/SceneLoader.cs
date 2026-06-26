using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Business.Services
{
    public class SceneLoader : Service
    {
        private AsyncOperation _loadingOperation;
        private Func<string, AsyncOperation> _loadingStarterFunc;
        private string _currentlyLoadingScene;

        public SceneLoader(Func<string, AsyncOperation> loadingStarterFunc)
        {
            _loadingStarterFunc = loadingStarterFunc;
        }

        public async void LoadScene(string sceneName)
        {
            if (_currentlyLoadingScene == sceneName) return;

            _currentlyLoadingScene = sceneName;
            _loadingOperation = _loadingStarterFunc(sceneName);

            await _loadingOperation;

            _currentlyLoadingScene = null;
            OnWorkFinished();
        }

        public void LoadScene(string sceneName, Action callback)
        {
            AddSubscriber(callback);
            LoadScene(sceneName);
        }

        public float GetCurrentLoadingProgress()
        {
            if (_loadingOperation == null) return 0f;
            if (_loadingOperation.isDone) return 100f;
            return _loadingOperation.progress;
        }
    }
}
