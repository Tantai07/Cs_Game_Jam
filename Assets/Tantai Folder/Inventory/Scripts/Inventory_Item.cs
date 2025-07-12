using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory_Item : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image iconImage;
    public Image outlineObject;

    private int myIndex = -1;
    private Item currentItem;

    private void Start()
    {
        outlineObject = transform.parent.GetComponent<Image>();
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentItem != null && myIndex >= 0)
        {
            InventoryManager.Instance.SelectSlot(myIndex);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryManager.Instance.SelectSlot(-1);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (myIndex < 0 || currentItem == null)
            return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (currentItem.itemName == "Duck") return;
            InventoryManager.Instance.UseSelectedItem();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (currentItem.itemName == "Duck") return;

            InventoryManager.Instance.RemoveSelectedItem();
        }
    }

    public void SetSelected(bool selected)
    {
        if (selected)
        {
            outlineObject.color = Color.white;
        }
        else
        {
            outlineObject.color = new Color32(255, 215, 0, 255);
        }
    }
}
