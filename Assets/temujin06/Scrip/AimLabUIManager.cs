using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class AimLabUIManager : MonoBehaviour
{
    public static AimLabUIManager Instance;

    [Header("UI Elements")]
    [SerializeField] private GameObject aimLabPanel;
    [SerializeField] private RectTransform targetArea;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private Button closeButton;

    [Header("Target Settings")]
    [SerializeField] private GameObject targetPrefab;
    [SerializeField] private float targetSize = 40f;
    [SerializeField] private float spawnInterval = 0.7f;
    [SerializeField] private float targetLifetime = 2f;

    [Header("Game Settings")]
    [SerializeField] private float gameDuration = 30f;
    [SerializeField] private int winScore = 100;

    private float timeLeft;
    private int score;
    private bool isRunning;
    private readonly List<GameObject> activeTargets = new List<GameObject>();
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    private void Start()
    {
        aimLabPanel.SetActive(false);
        closeButton.onClick.AddListener(CloseMiniGame);
    }

    private void Update()
    {
        if (!isRunning) return;

        timeLeft -= Time.deltaTime;
        timerText.text = $"Time: {timeLeft:0.0}";

        if (timeLeft <= 0 || score >= winScore)
        {
            Player_Movement.Instance.Aimlab = true;
            EndGame();
        }
    }

    public void StartMiniGame()
    {
        aimLabPanel.SetActive(true);
        ResetGameState();

        InvokeRepeating(nameof(SpawnTarget), 0f, spawnInterval);
    }

    private void EndGame()
    {
        if (!isRunning) return;

        isRunning = false;
        CancelInvoke(nameof(SpawnTarget));
        ClearTargets();

        statusText.text = score >= winScore ? "Win" : "Failed";
    }

    private void CloseMiniGame()
    {
        EndGame();
        aimLabPanel.SetActive(false);
    }

    private void ResetGameState()
    {
        score = 0;
        timeLeft = gameDuration;
        isRunning = true;
        statusText.text = string.Empty;
        UpdateScoreText();
        ClearTargets();
    }

    private void SpawnTarget()
    {
        if (!isRunning || targetPrefab == null || targetArea == null) return;

        Vector2 areaSize = targetArea.rect.size;

        float x = Random.Range(targetSize / 2f, areaSize.x - targetSize / 2f);
        float y = Random.Range(targetSize / 2f, areaSize.y - targetSize / 2f);

        GameObject target = Instantiate(targetPrefab, targetArea);
        RectTransform rt = target.GetComponent<RectTransform>();

        rt.sizeDelta = new Vector2(targetSize, targetSize);
        rt.anchorMin = new Vector2(0, 0);
        rt.anchorMax = new Vector2(0, 0);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = new Vector2(x, y);

        float randomLifetime = Random.Range(0.6f, 3f);

        TargetBehavior behavior = target.GetComponent<TargetBehavior>();
        if (behavior != null)
        {
            behavior.Initialize(randomLifetime);
        }

        Button btn = target.GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(() =>
            {
                score += 10;
                UpdateScoreText();
                RemoveTarget(target);
            });
        }

        activeTargets.Add(target);
        Destroy(target, randomLifetime);
    }

    private void RemoveTarget(GameObject target)
    {
        if (activeTargets.Contains(target))
            activeTargets.Remove(target);

        if (target != null)
            Destroy(target);
    }

    private void ClearTargets()
    {
        foreach (var target in activeTargets)
        {
            if (target != null) Destroy(target);
        }
        activeTargets.Clear();
    }

    private void UpdateScoreText()
    {
        scoreText.text = $"Score: {score} / {winScore}";
    }
}
