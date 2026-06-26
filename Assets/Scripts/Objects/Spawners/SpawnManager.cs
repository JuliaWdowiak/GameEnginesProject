using Assets.Scripts.Business.Lists;
using Assets.Scripts.Objects.Actors;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Objects.Spawners
{
    public class SpawnManager : MonoBehaviour
    {
        private List<Vector3> _monsterSpawners = new();
        private List<SpawnPoint> _playerSpawners = new();
        private SpawnPoint _fixedSpawnPoint = null;
        private SpawnPoint _initial = null;

        private void Start()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).tag == Tags.PlayerSpawnPoint)
                {
                    var spawnPoint = transform.GetChild(i).GetComponent<SpawnPoint>();

                    if (spawnPoint.IsInitial)_initial = spawnPoint;
                    _playerSpawners.Add(spawnPoint);
                    _playerSpawners[_playerSpawners.Count-1].SetActivationAction((SpawnPoint point) => { _fixedSpawnPoint = point; });
                }
            }
        }

        //public void SpawnPlayer(GameObject player)
        //{
        //    if (_initial != null)
        //    {
        //        player.transform.position = _initial.Position;
        //        _initial.TargetRoom.SetActive(true);
        //        player.GetComponent<ActorController>().SetCurrentRoom(_initial.TargetRoom);
        //        _initial = null;
        //    }
        //    else if (_fixedSpawnPoint != null)
        //    {
        //        player.transform.position = _fixedSpawnPoint.Position;
        //        _fixedSpawnPoint.TargetRoom.SetActive(true);
        //        player.GetComponent<ActorController>().SetCurrentRoom(_fixedSpawnPoint.TargetRoom);
        //    }
        //    else
        //    {
        //        int index = (int)Math.Floor(UnityEngine.Random.value * _playerSpawners.Count);
        //        player.transform.position = _playerSpawners[index].Position;
        //        _playerSpawners[index].TargetRoom.SetActive(true);
        //        player.GetComponent<ActorController>().SetCurrentRoom(_playerSpawners[index].TargetRoom);
        //    }
        //    player.GetComponent<ActorController>().OnSpawn();
        //}
    }
}
