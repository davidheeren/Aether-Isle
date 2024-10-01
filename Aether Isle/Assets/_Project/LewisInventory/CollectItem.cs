using UnityEngine;

public class CollectItem : MonoBehaviour
{
    public Inventory1 inventory;
    public InventoryItem itemToCollect;

    public void OnCollect()
    {
        inventory.AddItem(itemToCollect);
        FindObjectOfType<InventoryUI>().UpdateInventoryUI();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        OnCollect();
        Destroy(gameObject);
    }
}
