using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class AimLabUIManager : MonoBehaviour
{
    public GameObject aimLabPanel;
    public GameObject targetPrefab;
    public RectTransform targetArea;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI statusText;
    public Button startButton;
    public Button closeButton;

    public float gameDuration = 30f;
    private float timeLeft;
    private int score = 0;
    private bool isRunning = false;
    private List<GameObject> activeTargets = new List<GameObject>();

    void Start()
    {
        aimLabPanel.SetActive(false);
        startButton.onClick.AddListener(StartMiniGame);
        closeButton.onClick.AddListener(CloseMiniGame);
    }

    void Update()
    {
        if (!isRunning) return;

        timeLeft -= Time.deltaTime;
        timerText.text = "Time: " + timeLeft.ToString("0.0");

        if (timeLeft <= 0 || score >= 100)
        {
            EndGame();
        }
    }

    public void StartMiniGame()
    {
        aimLabPanel.SetActive(true);
        statusText.text = "";
        score = 0;
        timeLeft = gameDuration;
        isRunning = true;
        UpdateScoreText();
        CancelInvoke();
        InvokeRepeating("SpawnTarget", 0f, 0.7f);
    }

    void EndGame()
    {
        isRunning = false;
        CancelInvoke();
        ClearTargets();

        if (score >= 100)
        {
            statusText.text = "Win!";
        }
        else
        {
            statusText.text = "Failed";
        }
    }

    public void CloseMiniGame()
    {
        EndGame();
        aimLabPanel.SetActive(false);
    }

    void SpawnTarget()
    {
        if (!isRunning) return;

        Vector2 areaSize = targetArea.rect.size;
        float targetWidth = 40f;
        float targetHeight = 40f;

        float x = Random.Range(targetWidth / 2f, areaSize.x - targetWidth / 2f);
        float y = Random.Range(targetHeight / 2f, areaSize.y - targetHeight / 2f);

        GameObject target = Instantiate(targetPrefab, targetArea);
        RectTransform rt = target.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(x, y);

        Button btn = target.GetComponent<Button>();
        btn.onClick.AddListener(() => {
            score += 10;
            UpdateScoreText();
            Destroy(target);
            activeTargets.Remove(target); // ลบออกจาก list
        });

        activeTargets.Add(target);

        // 👉 เป้าจะหายไปเองภายใน 2 วินาทีถ้าไม่โดนยิง
        Destroy(target, 2f);
    }




    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score + " / 100";
    }

    void ClearTargets()
    {
        foreach (var t in activeTargets)
        {
            if (t != null) Destroy(t);
        }
        activeTargets.Clear();
    }
}
