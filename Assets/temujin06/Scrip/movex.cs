using UnityEngine;
using UnityEngine.InputSystem;

public class movex : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        Vector2 moveInput = Vector2.zero;

        if (Keyboard.current.wKey.isPressed)
            moveInput.y += 1;
        if (Keyboard.current.sKey.isPressed)
            moveInput.y -= 1;
        if (Keyboard.current.dKey.isPressed)
            moveInput.x += 1;
        if (Keyboard.current.aKey.isPressed)
            moveInput.x -= 1;

        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y);
        transform.Translate(movement.normalized * speed * Time.deltaTime);
    }
}
