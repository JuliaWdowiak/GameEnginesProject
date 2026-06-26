using Assets.Scripts.Business.Lists;
using Assets.Scripts.Business.Menus.MainMenu;

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
                playerDeathHandler: (SpawnManager manager, GameObject player) => {
                    StartCoroutine(PlayerDeath(manager, player)); 
                },
                instantiateFunc: Instantiate,
                quitGame: () => { StartCoroutine(LoadScene(SceneList.MainMenu, SetupMainMenu)); }
            );
            if (_currentSceneName == null)
            {
                _currentSceneName = SceneList.MainMenu;
                _sceneLoader.LoadScene(_currentSceneName, SetupMainMenu);
            }
        }

        private void SetupMainMenu()
        {
            _currentScene = SceneManager.GetActiveScene();
            var menuManager = _currentScene.GetRootGameObjects()[0].GetComponentInChildren<MainMenuManager>();
            
            menuManager.SetUp(new Action<float>[]
            {
                (float value) => {
                    StartCoroutine(LoadScene(SceneList.Level_01));
                }, // StartButton Callback
                (float value) => {
                    _globalVolumeModifier = value;
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

            if (sceneName.Split("_")[0] == "Level") _setUpper.SetUp(SceneManager.GetActiveScene(), _globalVolumeModifier);

            waitForFading = true;
            Fader.instance.FadeOut(() => waitForFading = false);

            while (waitForFading)
                yield return null;
        }
        private IEnumerator LoadScene(string sceneName, Action callback)
        {
            var waitForFading = true;

            Fader.instance.FadeIn(() => waitForFading = false);

            while (waitForFading)
                yield return null;

            _sceneLoader.LoadScene(sceneName, callback);

            while (_sceneLoader.GetCurrentLoadingProgress() < 1f)
                yield return null;

            if (sceneName.Split("_")[0] == "Level") _setUpper.SetUp(SceneManager.GetActiveScene(), _globalVolumeModifier);

            waitForFading = true;
            Fader.instance.FadeOut(() => waitForFading = false);

            while (waitForFading)
                yield return null;
        }

        private IEnumerator PlayerDeath(SpawnManager manager, GameObject player)
        {
            var waitForFading = true;

            Fader.instance.FadeIn(() => waitForFading = false);

            while (waitForFading)
                yield return null;

            //player.GetComponent<ActorController>().CurrentRoom.SetActive(false);
            //manager.SpawnPlayer(player);
            //player.GetComponent<ActorController>().Restore();
            //player.GetComponent<ActorController>().CurrentRoom.SetActive(true);

            yield return new WaitForSeconds(1.5f);
            waitForFading = true;
            Fader.instance.FadeOut(() => waitForFading = false);

            while (waitForFading)
                yield return null;
        }
    }
}
