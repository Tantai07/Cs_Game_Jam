using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraPixelSnap : MonoBehaviour
{
    public float pixelsPerUnit = 16f; // ตั้งให้ตรงกับ Sprite
    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (cam.orthographic)
        {
            float unitsPerPixel = 1f / pixelsPerUnit;
            Vector3 pos = transform.position;

            pos.x = Mathf.Round(pos.x / unitsPerPixel) * unitsPerPixel;
            pos.y = Mathf.Round(pos.y / unitsPerPixel) * unitsPerPixel;

            transform.position = new Vector3(pos.x, pos.y, transform.position.z);
        }
    }
}
