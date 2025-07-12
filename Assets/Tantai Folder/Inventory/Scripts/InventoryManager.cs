using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static Item;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Inventory Data")]
    public List<Item> items = new List<Item>();
    public List<Inventory_Item> uiSlots = new List<Inventory_Item>();

    [SerializeField] private int selectedIndex = -1;

    [Header("Item Info UI")]
    private GameObject text_Object;
    private GameObject frame_Object;
    private TMP_Text nameText;
    private TMP_Text descriptionText;

    #region Unity Lifecycle
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

        if (text_Object != null)
            nameText = text_Object.transform.Find("NameText")?.GetComponent<TMP_Text>();

        if (text_Object != null)
            descriptionText = text_Object.transform.Find("DescriptionText")?.GetComponent<TMP_Text>();

        text_Object?.SetActive(false);
        frame_Object?.SetActive(false);
    }
    #endregion

    #region Public Methods
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
        if (!IsValidIndex(selectedIndex))
            return;

        Item itemToUse = items[selectedIndex];

        if (itemToUse.type == ItemType.Energy)
        {
            Player_Movement.Instance?.ReduceStress(itemToUse.recover);
        }

        RemoveSelectedItem();
    }

    public void RemoveSelectedItem()
    {
        if (!IsValidIndex(selectedIndex))
            return;

        items.RemoveAt(selectedIndex);
        selectedIndex = -1;

        DisplayItemInfo(null);
        UpdateUI();
    }

    public void SelectSlot(int index)
    {
        if (!IsValidIndex(index))
        {
            selectedIndex = -1;
            DisplayItemInfo(null);
        }
        else if (selectedIndex == index)
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

    public void ClearInventory()
    {
        items.Clear();
        selectedIndex = -1;
        DisplayItemInfo(null);
        UpdateUI();
    }

    public void UpdateUI()
    {
        for (int i = 0; i < uiSlots.Count; i++)
        {
            if (i < items.Count)
            {
                uiSlots[i].SetItem(items[i], i);
            }
            else
            {
                uiSlots[i].ClearSlot();
            }
        }

        UpdateSelectionVisuals();
    }

    public void DisplayItemInfo(Item item)
    {
        if (text_Object == null || frame_Object == null || nameText == null || descriptionText == null)
            return;

        bool hasItem = item != null;

        text_Object.SetActive(hasItem);
        frame_Object.SetActive(hasItem);

        if (hasItem)
        {
            nameText.text = $"{item.itemName} :";
            descriptionText.text = item.description;
        }
    }

    public bool HasItem(string itemName)
    {
        foreach (var item in items)
        {
            if (item.itemName == itemName)
                return true;
        }
        return false;
    }
    #endregion

    #region Helpers
    private void UpdateSelectionVisuals()
    {
        for (int i = 0; i < uiSlots.Count; i++)
        {
            uiSlots[i].SetSelected(i == selectedIndex);
        }
    }

    private bool IsValidIndex(int index)
    {
        return index >= 0 && index < items.Count;
    }
    #endregion
}
