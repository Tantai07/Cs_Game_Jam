using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

public class PlayTextInteractiveSmooth : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public string sceneToLoad = "GameScene";
    public float targetScale = 1.2f;
    public float scaleSpeed = 5f;
    public float fadeSpeed = 5f;

    private TextMeshProUGUI text;
    private Vector3 originalScale;
    private Color originalColor;
    private Vector3 currentScaleTarget;
    private Color currentColorTarget;
    private bool isPressed = false;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        originalScale = transform.localScale;
        currentScaleTarget = originalScale;

        originalColor = text.color;
        currentColorTarget = originalColor;
    }

    void Update()
    {
        // Smooth scale
        transform.localScale = Vector3.Lerp(transform.localScale, currentScaleTarget, Time.deltaTime * scaleSpeed);

        // Smooth fade
        text.color = Color.Lerp(text.color, currentColorTarget, Time.deltaTime * fadeSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        currentScaleTarget = originalScale * targetScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        currentScaleTarget = originalScale;
        currentColorTarget = originalColor;
        isPressed = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        currentColorTarget = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isPressed)
        {
            currentScaleTarget = originalScale;
            currentColorTarget = originalColor;
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
