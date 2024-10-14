namespace Inventory
{
    [System.Serializable]
    public struct SerializedItem
    {
        public string id;
        public int count;
        public int inventoryIndex;

        public SerializedItem(string id, int count, int inventoryIndex)
        {
            this.id = id;
            this.count = count;
            this.inventoryIndex = inventoryIndex;
        }
    }
}
