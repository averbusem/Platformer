using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float movement_speed = 300;
    private bool isSwordReloading= false;
    [SerializeField] private float reloadTime = 2f;

    Rigidbody2D rb;
    CollisionTouchCheck col_touch_check;
    SpriteRenderer spr;
    Animator anim;
    GameObject attack_p;
    public LayerMask enemy;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col_touch_check = GetComponent<CollisionTouchCheck>();
        spr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        attack_p = GameObject.FindWithTag("Attack_p");
    }

    Vector2 move_input; // ось движения
    private bool isJumpHeld = false; // удерживается ли кнопка прыжка

    public void OnMove(InputAction.CallbackContext context)
    {
        move_input = context.ReadValue<Vector2>();
        anim.SetFloat("Speed",Mathf.Abs(move_input.x));
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(move_input.x * movement_speed * Time.fixedDeltaTime, rb.velocity.y);
        anim.SetFloat("ySpeed", rb.velocity.y);
        anim.SetBool("OnGround", col_touch_check.IsGrounded);
        if(Mathf.Abs(rb.velocity.y)>0.1)
        {
            anim.SetTrigger("Jump");
        }
        if (move_input.x<0)
        {
            spr.flipX = true;
        }
        if (move_input.x > 0)
        {
            spr.flipX = false;
        }
        // Если кнопка прыжка удерживается и персонаж касается земли, совершаем прыжок
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
            // Если отпустили кнопку, уменьшаем скорость прыжка
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.3f);
        }
    }
    IEnumerator Sword_Reload()
    {
        isSwordReloading = true;
        yield return new WaitForSeconds(reloadTime);
        isSwordReloading = false;
    }
    public void Sword_Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!isSwordReloading)
            {
                Collider2D[] damage = Physics2D.OverlapCircleAll(attack_p.transform.position, 2, enemy);
                foreach (Collider2D col in damage)
                {
                    Debug.Log(col + " Damaged");
                    col.GetComponent<Enemy>().Death();
                }
                anim.SetTrigger("Sword");
                StartCoroutine(Sword_Reload());
            }
        }
    }
}
