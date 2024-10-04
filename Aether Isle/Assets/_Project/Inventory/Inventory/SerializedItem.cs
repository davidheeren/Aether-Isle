namespace Inventory
{
    [System.Serializable]
    public struct SerializedItem
    {
        public int count;
        public int databaseIndex;
        public int inventoryIndex;

        public SerializedItem(int count, int databaseIndex, int inventoryIndex)
        {
            this.count = count;
            this.databaseIndex = databaseIndex;
            this.inventoryIndex = inventoryIndex;
        }
    }
}
