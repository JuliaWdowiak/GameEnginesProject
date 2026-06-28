namespace Assets.Scripts.Objects.Actors.Player
{
    public class Inventory
    {
        public int[] Storage = new int[4];

        public void StoreItem(int itemID)
        {
            for (int i = 0; i < Storage.Length; i++)
            {
                if (Storage[i] == 0)
                {
                    Storage[i] = itemID;
                    return;
                }
            }
        }

        public int UnstoreItem(int storageCell)
        {
            int itemId = Storage[storageCell];
            Storage[storageCell] = 0;
            return itemId;
        }
    }
}
