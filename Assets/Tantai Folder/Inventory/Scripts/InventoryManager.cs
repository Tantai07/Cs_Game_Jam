using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static Item;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public List<Item> items = new List<Item>();
    public List<Inventory_Item> uiSlots = new List<Inventory_Item>();

    [SerializeField]private int selectedIndex = -1;

    [Header("Item Info UI")]
    private GameObject text_Object;
    private GameObject frame_Object;
    private TMP_Text nameText;
    private TMP_Text descriptionText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        text_Object = GameObject.Find("Group_Text");
        frame_Object = GameObject.Find("Group_Frame");

        nameText = text_Object.transform.Find("NameText")?.GetComponent<TMP_Text>();
        descriptionText = text_Object.transform.Find("DescriptionText")?.GetComponent<TMP_Text>();
        text_Object.SetActive(false);
        frame_Object.SetActive(false);
    }

    public bool AddItem(Item item)
    {
        if (items.Count >= uiSlots.Count)
        {
            return false;
        }

        items.Add(item);
        UpdateUI();
        return true;
    }

    public void UseSelectedItem()
    {
        if (selectedIndex < 0 || selectedIndex >= items.Count)
        {
            return;
        }

        Item itemToUse = items[selectedIndex];


        if (itemToUse.type == ItemType.Energy)
        {

        }

        RemoveSelectedItem();
    }

    public void RemoveSelectedItem()
    {
        if (selectedIndex < 0 || selectedIndex >= items.Count)
        {
            return;
        }

        text_Object.SetActive(false);
        frame_Object.SetActive(false);
        items.RemoveAt(selectedIndex);

        selectedIndex = -1;

        UpdateUI();
    }


    public void SelectSlot(int index)
    {
        if (index < 0 || index >= items.Count)
        {
            selectedIndex = -1;
            DisplayItemInfo(null);
            UpdateSelectionVisuals();
            return;
        }

        if (selectedIndex == index)
        {
            selectedIndex = -1;
            DisplayItemInfo(null);
        }
        else
        {
            selectedIndex = index;
            DisplayItemInfo(items[index]);
        }

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
        DisplayItemInfo(null);
        UpdateUI();
    }

    public void DisplayItemInfo(Item item)
    {
        if (text_Object == null || frame_Object == null || nameText == null || descriptionText == null)
            return;

        if (item != null)
        {
            nameText.text = item.itemName + " :";
            descriptionText.text = item.description;
            text_Object.SetActive(true);
            frame_Object.SetActive(true);
        }
        else
        {
            text_Object.SetActive(false);
            frame_Object.SetActive(false);
        }
    }
}
