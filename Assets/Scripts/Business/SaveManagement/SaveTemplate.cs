namespace Assets.Scripts.Business.SaveManagement
{
    [System.Serializable]
    public class SaveTemplate
    {
        public float[] playerPosition;
        public int playerState;
        public int playerLookDirection;
        public float playerDamage;
        public int[] playerInventory;

        public string currentLevel;

        public int[] aliveEnemies;
        public float[] enemyPosition;
        public int[] enemyLookDirection;

        public SaveTemplate(float[] playerPosition, int playerState, int playerLookDirection, float playerDamage,
            int[] playerInventory, string currentLevel, int[] aliveEnemies, float[] enemyPosition, int[] enemyLookDirection)
        {
            this.playerPosition = playerPosition;
            this.playerState = playerState;
            this.playerLookDirection = playerLookDirection;
            this.playerDamage = playerDamage;
            this.playerInventory = playerInventory;
            this.currentLevel = currentLevel;
            this.aliveEnemies = aliveEnemies;
            this.enemyPosition = enemyPosition;
            this.enemyLookDirection = enemyLookDirection;
        }
    }
}
