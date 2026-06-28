using Assets.Scripts.Business.Lists;
using Assets.Scripts.Business.Menus.MainMenu;
using Assets.Scripts.Business.SaveManagement;


//using Assets.Scripts.Business.Menus.MainMenu;
using Assets.Scripts.Business.Services;
using Assets.Scripts.Objects.Actors;
using Assets.Scripts.Objects.Fader;
using Assets.Scripts.Objects.Spawners;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Business
{
    public class GameManager : MonoBehaviour
    {
        private SceneLoader _sceneLoader = new SceneLoader((string sceneName) => SceneManager.LoadSceneAsync(sceneName));
        //private RoomSwitcher _roomSwitcher = new RoomSwitcher();
        private LevelSetUp _setUpper = new LevelSetUp();

        private string _currentSceneName;
        private Scene _currentScene;

        private float _globalVolumeModifier = 1f;
        private bool _isStartingNew = true;

        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        private void Start()
        {
            _setUpper.Init(
                //onDoorEnterAction: (GameObject currentRoom, GameObject targetRoom, ActorController initiator, GameObject door, Action onFaded) => {
                //    StartCoroutine(_roomSwitcher.SwitchTo(currentRoom, targetRoom, initiator, door, onFaded)); 
                //},
                playerDeathHandler: (GameObject player) => {
                    PlayerDeath(player); 
                },
                instantiateFunc: Instantiate,
                quitGame: () => { Debug.Log("QuiteButton Clicked"); StartCoroutine(SaveAndExit(LoadScene(SceneList.MainMenu, SetupMainMenu, true))); },
                proceedToNextLevel: (string level) => { SaveAndGoToNextLevel(level); }
            );
            if (_currentSceneName == null)
            {
                _currentSceneName = SceneList.MainMenu;
                _sceneLoader.LoadScene(_currentSceneName, SetupMainMenu);
            }
        }

        private IEnumerator SaveAndExit(IEnumerator SceneLoad)
        {
            var waitForFading = true;

            Fader.instance.FadeIn(() => waitForFading = false);

            while (waitForFading)
                yield return null;

            SaveManager.SaveGame(_setUpper.Player, _setUpper.Enemies, _currentSceneName);
            Debug.Log("Saved");
            StartCoroutine(SceneLoad);
            yield return null;
        }
        private IEnumerator SaveAndGoToNextLevel(string level)
        {
            var waitForFading = true;

            Fader.instance.FadeIn(() => waitForFading = false);

            while (waitForFading)
                yield return null;

            SaveManager.SaveGame(_setUpper.Player, _setUpper.Enemies, _currentSceneName);
            StartCoroutine(LoadScene(level));
        }

        private void SetupMainMenu()
        {
            _currentScene = SceneManager.GetActiveScene();
            var menuManager = _currentScene.GetRootGameObjects()[0].GetComponentInChildren<MainMenuManager>();
            
            menuManager.SetUp(new Action<float>[]
            {
                (float value) => {
                    if (SaveManager.HasSave) {
                        _isStartingNew = false;
                        StartCoroutine(LoadScene(SaveManager.GetLevel()));
                    }
                }, // StartButton Callback
                (float value) => {
                    _isStartingNew = true;
                    StartCoroutine(LoadScene(SceneList.Level_01));
                }, // VolumeAdjusting
            });
        }

        private IEnumerator LoadScene(string sceneName)
        {
            var waitForFading = true;

            Fader.instance.FadeIn(() => waitForFading = false);

            while (waitForFading)
                yield return null;

            _sceneLoader.LoadScene(sceneName);

            while (_sceneLoader.GetCurrentLoadingProgress() < 1f)
                yield return null;

            if (sceneName.Split("_")[0] == "Level") _setUpper.SetUp(SceneManager.GetActiveScene(), _globalVolumeModifier, _isStartingNew);
            if (_isStartingNew || !SaveManager.HasSave) SaveManager.SaveGame(_setUpper.Player, _setUpper.Enemies, _currentSceneName);

            waitForFading = true;
            Fader.instance.FadeOut(() => waitForFading = false);
            _currentSceneName = sceneName;

            while (waitForFading)
                yield return null;
        }
        private IEnumerator LoadScene(string sceneName, Action callback, bool? isFaded = false)
        {
            bool waitForFading;
            Debug.Log("isFaded.Value: " + isFaded.Value);
            if (!isFaded.Value)
            {
                waitForFading = true;

                Fader.instance.FadeIn(() => waitForFading = false);

                while (waitForFading)
                    yield return null;
            }
            _sceneLoader.LoadScene(sceneName, callback);

            while (_sceneLoader.GetCurrentLoadingProgress() < 1f) {
                yield return null;
            }

            if (sceneName.Split("_")[0] == "Level") _setUpper.SetUp(SceneManager.GetActiveScene(), _globalVolumeModifier, _isStartingNew);
            if (_isStartingNew || !SaveManager.HasSave) SaveManager.SaveGame(_setUpper.Player, _setUpper.Enemies, _currentSceneName);

            waitForFading = true;
            Fader.instance.FadeOut(() => waitForFading = false);

            while (waitForFading)
                yield return null;
        }

        private void PlayerDeath(GameObject player)
        {
            StartCoroutine(LoadScene(_currentSceneName));

            //var waitForFading = true;

            //Fader.instance.FadeIn(() => waitForFading = false);

            //while (waitForFading)
            //    yield return null;

            //player.GetComponent<ActorController>().Respawn();

            //yield return new WaitForSeconds(1.5f);
            //waitForFading = true;
            //Fader.instance.FadeOut(() => waitForFading = false);

            //while (waitForFading)
            //    yield return null;
        }
    }
}
