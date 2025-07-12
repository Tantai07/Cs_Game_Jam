using UnityEngine;

public class TargetBehavior : MonoBehaviour
{
    private RectTransform rt;
    private float lifetime;
    private Vector2 direction;
    private float angle;
    private Vector2 centerPoint;

    [Header("Linear Movement Settings")]
    public float linearSpeed = 450f;

    [Header("Circle Movement Settings")]
    public float circleRadius = 500f;
    public float circleRotateSpeed = 60f; // degrees per second
    public float startAngle = 0f;

    private enum BehaviorType { Idle, MoveLinear, MoveCircle }
    private BehaviorType behavior;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    public void Initialize(float life)
    {
        lifetime = life;

        if (lifetime <= 1.2f)
        {
            behavior = BehaviorType.Idle;
        }
        else if (lifetime <= 1.7f)
        {
            behavior = BehaviorType.MoveLinear;

            // Random direction: up/down/left/right
            Vector2[] dirs = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
            direction = dirs[Random.Range(0, dirs.Length)];
        }
        else
        {
            behavior = BehaviorType.MoveCircle;

            // ช่วงหมุนกว้างและช้าลง
            circleRadius = Random.Range(400f, 600f);
            circleRotateSpeed = Random.Range(30f, 90f); // หมุนช้า
            startAngle = Random.Range(0f, 360f);
            angle = startAngle;

            // เก็บจุดเริ่มต้นเป็นศูนย์กลางการหมุน
            centerPoint = rt.anchoredPosition;
        }
    }

    private void Update()
    {
        switch (behavior)
        {
            case BehaviorType.MoveLinear:
                rt.anchoredPosition += direction * linearSpeed * Time.deltaTime;
                break;

            case BehaviorType.MoveCircle:
                angle += circleRotateSpeed * Time.deltaTime;
                float rad = angle * Mathf.Deg2Rad;

                Vector2 offset = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * circleRadius;
                rt.anchoredPosition = centerPoint + offset;
                break;
        }
    }
}
