using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventorySlotPrefab; 
    public Inventory1 inventory; 
    public Transform inventoryPanel; 

    private void Start()
    {
        UpdateInventoryUI();
    }

    public void UpdateInventoryUI()
    {
        // Clear existing slots
        foreach (Transform child in inventoryPanel)
        {
            Destroy(child.gameObject);
        }

        // Create a new slot for each item in the inventory
        foreach (InventoryItem item in inventory.items)
        {
            GameObject slot = Instantiate(inventorySlotPrefab, inventoryPanel);
            slot.GetComponent<Image>().sprite = item.itemIcon;
        }
    }
}
