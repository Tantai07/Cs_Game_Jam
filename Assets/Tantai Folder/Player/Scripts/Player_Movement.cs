using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;
using Unity.Cinemachine;
using System.Collections;

public class Player_Movement : MonoBehaviour
{
    public static Player_Movement Instance;

    [Header("Movement")]
    [SerializeField] private float move_Speed = 5f;
    private Rigidbody2D rb;
    private Vector2 move_Input;

    public bool canMove = true;

    [Space(5)]
    [Header("Stat")]
    [Range(0, 100)]
    public float stress;
    public float maxStress = 100f;
    public float minStress = 0f;

    [Space(5)]
    [Header("UI")]
    private GameObject group;
    public TextMeshProUGUI stressText;
    public TextMeshProUGUI missionText;

    [Space(5)]
    [Header("Post Processing")]
    public Volume globalVolume;
    private Vignette vignette;
    private ColorAdjustments colorAdjust;

    [Space(5)]
    [Header("Mission")]
    public int friendTarget = 3;
    private int friendFound = 0;
    private bool missionCompleted = false;

    [Space(5)]
    [Header("Cinemachine")]
    public CinemachineCamera cine_Cam;

    [Space(5)]
    [Header("Transition")]
    public GameObject Group_Transition;
    [SerializeField] Animator anim_Transition;
    public float delay = 1.5f;

    // ** เพิ่ม Animator ตัวนี้ **
    private Animator animator;

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
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();  // ** เพิ่มบรรทัดนี้ **

        group = GameObject.Find("UI_Player");

        stressText = group.transform.Find("Text_Stress")?.GetComponent<TextMeshProUGUI>();
        missionText = group.transform.Find("Text_Mission")?.GetComponent<TextMeshProUGUI>();
        globalVolume = GameObject.Find("Global Volume")?.GetComponent<Volume>();

        if (globalVolume.profile.TryGet(out vignette))
            vignette.intensity.overrideState = true;

        if (globalVolume.profile.TryGet(out colorAdjust))
            colorAdjust.postExposure.overrideState = true;

        cine_Cam = FindObjectOfType<CinemachineCamera>();
        cine_Cam.Follow = transform;

        UpdateStressUI();
        UpdateStressVisual();
        UpdateMissionUI();

        Group_Transition = GameObject.Find("Group_Transition");
        anim_Transition = Group_Transition.GetComponentInChildren<Animator>();

        anim_Transition.SetTrigger("Start_Scene");
        StartCoroutine(FinishSceneTransition());
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            rb.velocity = move_Input.normalized * move_Speed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void LateUpdate()
    {
        UpdateAnimation(move_Input);
    }

    private void UpdateAnimation(Vector2 move)
    {
        float speed = move.sqrMagnitude;

        if (speed > 0.01f)
        {
            // ถ้าเดินแนวนอนเยอะกว่าแนวตั้ง
            if (Mathf.Abs(move.x) > Mathf.Abs(move.y))
            {
                animator.SetFloat("MoveX", Mathf.Sign(move.x)); // +1 หรือ -1
                animator.SetFloat("MoveY", 0f);
            }
            else
            {
                animator.SetFloat("MoveX", 0f);
                animator.SetFloat("MoveY", Mathf.Sign(move.y)); // +1 หรือ -1
            }
        }
        else
        {
            animator.SetFloat("MoveX", 0f);
            animator.SetFloat("MoveY", 0f);
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        move_Input = context.ReadValue<Vector2>();
    }

    // ---------- Stress ----------
    public void AddStress(float amount)
    {
        float oldStress = stress;
        stress = Mathf.Clamp(stress + amount, minStress, maxStress);

        if (stress != oldStress)
        {
            UpdateStressUI();
            UpdateStressVisual();
        }
    }

    public void ReduceStress(float amount)
    {
        float oldStress = stress;
        stress = Mathf.Clamp(stress - amount, minStress, maxStress);

        if (stress != oldStress)
        {
            UpdateStressUI();
            UpdateStressVisual();
        }
    }

    private void UpdateStressVisual()
    {
        if (vignette != null)
            vignette.intensity.value = Mathf.Lerp(0.17f, 0.8f, stress / maxStress);

        if (colorAdjust != null)
            colorAdjust.postExposure.value = Mathf.Lerp(0f, -1f, stress / maxStress);
    }

    private void UpdateStressUI()
    {
        if (stressText != null)
            stressText.text = $"Stress: {stress:0}/100";
    }

    // ---------- Mission ----------
    public void FindFriend()
    {
        if (missionCompleted) return;

        friendFound++;

        if (friendFound >= friendTarget)
        {
            missionCompleted = true;
        }

        UpdateMissionUI();
    }
    private void UpdateMissionUI()
    {
        if (missionText != null)
        {
            if (missionCompleted)
                missionText.text = "Mission: Complete";
            else
                missionText.text = $"Friends: ({friendFound}/{friendTarget})";
        }
    }
    private IEnumerator FinishSceneTransition()
    {
        yield return new WaitForSeconds(delay);
        anim_Transition.SetTrigger("End");
    }
}
