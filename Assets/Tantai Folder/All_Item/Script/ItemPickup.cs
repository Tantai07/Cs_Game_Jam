using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class ItemPickup : MonoBehaviour
{
    public Item itemData;
    private bool isPlayerInRange = false;

    [SerializeField] private GameObject textPopupObject;

    private void Start()
    {
        if (textPopupObject != null)
            textPopupObject.SetActive(false);
    }

    private void Update()
    {
        if (isPlayerInRange && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (InventoryManager.Instance.AddItem(itemData))
            {
                Destroy(gameObject);
            }
            else
            {
                Player_Movement.Instance.Show_Inventory_Full();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (textPopupObject != null)
                textPopupObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (textPopupObject != null)
                textPopupObject.SetActive(false);
        }
    }
}
