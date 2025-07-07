using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public List<Item> items = new List<Item>();
    public List<Inventory_Item> uiSlots = new List<Inventory_Item>();

    private int selectedIndex = -1;

    private void Awake()
    {
        Instance = this;
    }

    public void AddItem(Item item)
    {
        if (items.Count >= uiSlots.Count)
        {
            Debug.LogWarning("Inventory is full");
            return;
        }

        items.Add(item);
        UpdateUI();
    }

    public void SelectSlot(int index)
    {
        if (index < 0 || index >= items.Count)
        {
            Debug.LogWarning("Invalid slot index");
            return;
        }

        selectedIndex = index;
        UpdateSelectionVisuals();
    }

    private void UpdateSelectionVisuals()
    {
        for (int i = 0; i < uiSlots.Count; i++)
        {
            uiSlots[i].SetSelected(i == selectedIndex);
        }
    }

    public void UpdateUI()
    {
        for (int i = 0; i < uiSlots.Count; i++)
        {
            if (i < items.Count)
                uiSlots[i].SetItem(items[i], i);
            else
                uiSlots[i].ClearSlot();
        }

        UpdateSelectionVisuals();
    }

    public void ClearInventory()
    {
        items.Clear();
        selectedIndex = -1;
        UpdateUI();
    }
}
