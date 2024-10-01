using System.Collections.Generic;
using UnityEngine;

public class Inventory1 : MonoBehaviour
{
    public List<InventoryItem> items = new List<InventoryItem>();

    public void AddItem(InventoryItem item)
    {
        items.Add(item);
        Debug.Log(item.itemName + " added to inventory.");
    }

    public void RemoveItem(InventoryItem item)
    {
        items.Remove(item);
        Debug.Log(item.itemName + " removed from inventory.");
    }
}
