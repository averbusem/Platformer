using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float movement_speed = 300;
    private bool isSwordReloading= false;
    private float reloadTime = 0.255f;
    private bool isDead = false;


    Rigidbody2D rb;
    CollisionTouchCheck col_touch_check;
    SpriteRenderer spr;
    Animator anim;
    GameObject attack_p;
    public LayerMask enemy;
    private Player playerComponent;
    private Color originalColor;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col_touch_check = GetComponent<CollisionTouchCheck>();
        spr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        attack_p = GameObject.FindWithTag("Attack_p");
        playerComponent = GetComponent<Player>();
        originalColor = spr.color;
    }

    Vector2 move_input; // пїЅпїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ
    private bool isJumpHeld = false; // пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ пїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅ

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
        // пїЅпїЅпїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ пїЅ пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ пїЅпїЅпїЅпїЅпїЅ, пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅ
        if (col_touch_check.IsGrounded && isJumpHeld)
        {
            rb.velocity = new Vector2(rb.velocity.x, jump_impulse);
        }
        //Debug.Log("пїЅ 18 пїЅ пїЅпїЅпїЅпїЅпїЅпїЅ");
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
            // пїЅпїЅпїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅ, пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅ
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
                    //Debug.Log(col + " Damaged");
                    col.GetComponent<Enemy>().TakeDamage(1);
                }
                anim.SetTrigger("Sword");
                StartCoroutine(Sword_Reload());
            }
        }
    }

    // Корутин для временного окрашивания спрайта в красный цвет
    private IEnumerator FlashRed()
    {
        spr.color = new Color32(255, 105, 105, 255);
        yield return new WaitForSeconds(0.15f); // Задержка
        spr.color = originalColor;
    }
    public void TakeDamage(int damage) // параметр damage, тк игра может иметь различные источники урона, которые наносят разное количество урона (например, слабая атака — 1, сильная атака — 2)
    {
        if (isDead) return;

        playerComponent.Health -= damage;
        StartCoroutine(FlashRed());

        if (playerComponent.Health <= 0)
        {
            // Логика смерти игрока
            isDead = true;
            anim.SetTrigger("Die");
        }
    }

    


}
