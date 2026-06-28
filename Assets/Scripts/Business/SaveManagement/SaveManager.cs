using Assets.Scripts.Business;
using Assets.Scripts.Business.SaveManagement;
using Assets.Scripts.Objects.Actors;
using Assets.Scripts.Objects.Actors.Enemy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

namespace Assets.Scripts.Business.SaveManagement
{
    public static class SaveManager
    {
        private static SaveTemplate Save = null;
        public static bool HasSave { get; private set; } = false;
        public static void SaveGame(ActorController player, List<GameObject> Enemies, string level)
        {
            HasSave = true;

            Debug.Log(player);

            float[] playerPosition = new float[3];
            playerPosition[0] = player.transform.position.x;
            playerPosition[1] = player.transform.position.y;
            playerPosition[2] = player.transform.position.z;

            int playerState = (int)player.StateMachine.CurrentState;
            int playerLookDirection = (int)player.StateMachine.CurrentDirection;
            float playerDamage = player.CurrentDamage;
            int[] playerInventory = player.ActorInventory.Storage;

            string currentLevel = level;

            int[] aliveEnemies = new int[Enemies.Count];
            float[] enemyPosition = new float[Enemies.Count*3];
            int[] enemyLookDirection = new int[Enemies.Count];

            for (int i = 0; i < Enemies.Count; i++)
            {
                if (Enemies[i] != null)
                {
                    aliveEnemies[i] = Enemies[i].GetComponent<EnemyLogic>().ID;
                    int ind = i * 3;
                    enemyPosition[ind] = Enemies[i].transform.position.x;
                    enemyPosition[ind+1] = Enemies[i].transform.position.y;
                    enemyPosition[ind+2] = Enemies[i].transform.position.z;
                    enemyLookDirection[i] = (int)Enemies[i].GetComponent<ActorController>().StateMachine.CurrentDirection;
                }
                else
                {
                    aliveEnemies[i] = -1;
                    int ind = i * 3;
                    enemyPosition[ind] = -1;
                    enemyPosition[ind + 1] = -1;
                    enemyPosition[ind + 2] = -1;
                }
            }

            Save = new SaveTemplate(playerPosition, playerState, playerLookDirection, playerDamage, playerInventory, currentLevel, aliveEnemies, enemyPosition, enemyLookDirection);
            GenerateSaveFile();
        }

        private static void GenerateSaveFile()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/Last.save";
            Debug.Log(path);
            FileStream stream = new FileStream(path, FileMode.Create);

            formatter.Serialize(stream, Save);
            stream.Close();
        }

        public static bool CheckForSave()
        {
            string path = Application.persistentDataPath + "/Last.save";
            if (File.Exists(path))
            {
                Debug.Log("Save Exists");
                HasSave = true;

                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream (path, FileMode.Open);

                Save = formatter.Deserialize(stream) as SaveTemplate;
                stream.Close();

                return true;
            }
            Debug.Log("Save Not Exists");
            return false;
        }

        public static int[] GetEnemiesIDs() { return Save.aliveEnemies; }
        public static float[] GetEnemiesPositions() { return Save.enemyPosition; }
        public static int[] GetEnemiesLookDirection() { return Save.enemyLookDirection; }

        public static float[] GetPlayerPosition() { return Save.playerPosition; }
        public static LookDirection GetPlayerLookDirection() { return (LookDirection)Save.playerLookDirection; }
        public static ActorState GetPlayerState() { return (ActorState)Save.playerState; }
        public static float GetPlayerDamage() { return Save.playerDamage; }
        public static int[] GetPlayerInventory() { return Save.playerInventory; }

        public static string GetLevel() { return Save.currentLevel; }
    }
}