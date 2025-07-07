using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Inventory_Item : MonoBehaviour, IPointerClickHandler
{
    public Image iconImage;
    public TMP_Text nameText;
    public TMP_Text typeText;
    public GameObject outlineObject;

    private int myIndex = -1;
    private Item currentItem;

    private void Start()
    {
        iconImage = GetComponent<Image>();
        iconImage.enabled = false;
    }

    public void SetItem(Item item, int index)
    {
        currentItem = item;
        myIndex = index;

        if (item != null)
        {
            iconImage.sprite = item.icon;
            iconImage.enabled = true;
        }
        else
        {
            ClearSlot();
        }
    }

    public void ClearSlot()
    {
        currentItem = null;
        myIndex = -1;

        iconImage.sprite = null;
        iconImage.enabled = false;
        SetSelected(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentItem != null && myIndex >= 0)
        {
            InventoryManager.Instance.SelectSlot(myIndex);
        }
    }

    public void SetSelected(bool selected)
    {
        if (outlineObject != null)
        {
            outlineObject.SetActive(selected);
        }
    }
}
