using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float movement_speed = 300;

    Rigidbody2D rb;
    CollisionTouchCheck col_touch_check;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col_touch_check = GetComponent<CollisionTouchCheck>();
    }

    Vector2 move_input; // ��� ��������
    private bool isJumpHeld = false; // ������������ �� ������ ������

    public void OnMove(InputAction.CallbackContext context)
    {
        move_input = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(move_input.x * movement_speed * Time.fixedDeltaTime, rb.velocity.y);

        // ���� ������ ������ ������������ � �������� �������� �����, ��������� ������
        if (col_touch_check.IsGrounded && isJumpHeld)
        {
            rb.velocity = new Vector2(rb.velocity.x, jump_impulse);
        }
    }

    [SerializeField]
    float jump_impulse = 8;

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isJumpHeld = true;
            if (col_touch_check.IsGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jump_impulse);
            }
        }

        if (context.canceled)
        {
            isJumpHeld = false;
            // ���� ��������� ������, ��������� �������� ������
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.3f);
        }
    }
}
