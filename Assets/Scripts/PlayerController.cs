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
    private bool facingRight = true;

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

    Vector2 move_input;
    private bool isJumpHeld = false; 

    public void OnMove(InputAction.CallbackContext context)
    {
        move_input = context.ReadValue<Vector2>();
        anim.SetFloat("Speed",Mathf.Abs(move_input.x));
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
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
        if (move_input.x < 0 && facingRight)
        {
            Flip();
        }
        else if (move_input.x > 0 && !facingRight)
        {
            Flip();
        }
        
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
                    if (col.gameObject.CompareTag("Goblin"))
                    {
                        col.GetComponent<Enemy>().TakeDamage(1);
                    }
                    if (col.gameObject.CompareTag("Flight"))
                    {
                        Debug.Log("Flight");
                        col.GetComponent<EnemyFlight>().TakeDamage(1);
                    }
                }
                anim.SetTrigger("Sword");
                StartCoroutine(Sword_Reload());
            }
        }
    }


    private bool isFireballReloading = false; 
    public GameObject fireball;
    public Transform FirePoint;
    public float fireballDelay;
    public float reloadTimeFireballAtack;
    IEnumerator FireballReload()
    {
        isFireballReloading = true;

        yield return new WaitForSeconds(fireballDelay);

        // Determine the rotation of the fireball depending on the direction of the character
        Quaternion rotation = facingRight ? FirePoint.rotation : Quaternion.Euler(0, 180, 0);
        Instantiate(fireball, FirePoint.position, rotation);

        yield return new WaitForSeconds(reloadTimeFireballAtack);
        isFireballReloading = false;
    }
    public void FireballAtack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!isFireballReloading)
            {
                Debug.Log("fireball");
                anim.SetTrigger("Fireball");
                StartCoroutine(FireballReload());
            }
        }
    }



    private IEnumerator FlashRed()
    {
        spr.color = new Color32(255, 105, 105, 255);
        yield return new WaitForSeconds(0.15f); // Delay
        spr.color = originalColor;
    }
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        playerComponent.Health -= damage;
        StartCoroutine(FlashRed());

        if (playerComponent.Health <= 0)
        {
            // The logic of the player's death
            isDead = true;

            // Setting the sprite's y-coordinates to a fixed value (align the sprite to the lower boundary of the collider)
            Transform spriteTransform = GetComponentInChildren<SpriteRenderer>().transform;
            spriteTransform.localPosition = new Vector3(spriteTransform.localPosition.x, -0.0422f, spriteTransform.localPosition.z);

            anim.SetTrigger("Die");
        }
    }





}
