using UnityEngine;
using UnityEngine.EventSystems;
using static Item;

public class ClickToAddItem : MonoBehaviour, IPointerClickHandler
{
    [Header("Item Settings")]
    public string itemName = "Potion";
    public string description = "Restore 50 HP";
    public ItemType itemType = ItemType.Energy;
    public Sprite itemIcon;

    public void OnPointerClick(PointerEventData eventData)
    {
        Item newItem = new Item
        {
            itemName = itemName,
            description = description,
            type = itemType,
            icon = itemIcon
        };

        InventoryManager.Instance.AddItem(newItem);
        Debug.Log("Added item: " + itemName);
    }
}
