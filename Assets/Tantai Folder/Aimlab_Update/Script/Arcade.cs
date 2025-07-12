using UnityEngine;
using UnityEngine.InputSystem;

public class Arcade : MonoBehaviour
{
    private bool isPlayerInRange = false;
    private bool isPlay = false; // เพิ่มตัวแปรสถานะ

    [SerializeField] private GameObject textPopupObject;

    private void Start()
    {
        if (textPopupObject != null)
            textPopupObject.SetActive(false);
    }

    private void Update()
    {
        if (isPlayerInRange && !isPlay && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            AimLabUIManager.Instance.StartMiniGame();
            isPlay = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;

            if (textPopupObject != null && !isPlay)
                textPopupObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            isPlay = false;

            if (textPopupObject != null)
                textPopupObject.SetActive(false);
        }
    }

    // เรียกฟังก์ชันนี้จาก AimLabUIManager เมื่อเกมจบ
    public void OnMiniGameEnd()
    {
        isPlay = false;

        if (isPlayerInRange && textPopupObject != null)
            textPopupObject.SetActive(true);
    }
}
