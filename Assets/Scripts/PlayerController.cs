using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    // --- Movement variables ---
    [SerializeField]
    private float movementSpeed = 300;
    [SerializeField]
    private float jumpImpulse = 8;
    private Vector2 moveInput;
    private bool isJumpHeld = false;

    // --- Attack and Reload variables ---
    [SerializeField]
    private GameObject fireball;
    [SerializeField]
    private Transform FirePoint; // Point from which the fireball is fired
    [SerializeField]
    private float fireballDelay; // Delay before firing another fireball
    [SerializeField]
    private float reloadTimeFireballAtack; // Time between fireball attacks for animation
    private bool isSwordReloading = false;
    private bool isFireballReloading = false;
    private float reloadTime = 0.255f; // Reload time for sword attack

    // --- Health and State ---
    private bool isDead = false;
    private bool facingRight = true;
    private bool wasGrounded = true; // Flag to check if the player was grounded in the previous frame

    // --- Components ---
    Rigidbody2D rb;
    CollisionTouchCheck colTouchCheck;
    SpriteRenderer spr;
    Animator anim;
    GameObject attack_p; // GameObject for the attack point
    public LayerMask enemy;
    private Player playerComponent;
    private Color originalColor;
    [SerializeField]
    private PlayerAudioManager audioManager;
    [SerializeField]
    private GameObject deathPanel;

    // --- Initialize components ---
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        colTouchCheck = GetComponent<CollisionTouchCheck>();
        spr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        attack_p = GameObject.FindWithTag("Attack_p");
        playerComponent = GetComponent<Player>();
        originalColor = spr.color;
    }



    // --- Movement handling ---
    public void OnMove(InputAction.CallbackContext context)
    {
        if (isDead) return; // Ignore movement if the player is dead
        moveInput = context.ReadValue<Vector2>();
        anim.SetFloat("Speed",Mathf.Abs(moveInput.x));

        // Play movement sound if on ground
        if (Mathf.Abs(moveInput.x) > 0 && colTouchCheck.IsGrounded)
            audioManager.PlayMovementSound();
        else
            audioManager.StopMovementSound();

    }
    // --- Flip character direction ---
    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            rb.velocity = Vector2.zero; // Stop player movement if dead
            return; 
        }
        // Apply movement and update animation states
        rb.velocity = new Vector2(moveInput.x * movementSpeed * Time.fixedDeltaTime, rb.velocity.y);
        anim.SetFloat("ySpeed", rb.velocity.y);
        anim.SetBool("OnGround", colTouchCheck.IsGrounded);

        //if (!col_touch_check.IsGrounded && rb.velocity.y < -0.1f)
        //{
        //    audioManager.PlayFallingSound();
        //}
        //else
        //{
        //    audioManager.StopFallingSound();
        //}

        //if (!col_touch_check.IsGrounded && rb.velocity.y >= 0)
        //{
        //    audioManager.PlayFlyingSound();
        //}
        //else
        //{
        //    audioManager.StopFlyingSound();
        //}

        // Handle sounds on movement
        if (!wasGrounded && colTouchCheck.IsGrounded)
        {
            audioManager.PlayLandingSound();
        }

        if (!wasGrounded && colTouchCheck.IsGrounded && Mathf.Abs(moveInput.x) > 0)
        {
            audioManager.PlayMovementSound();
        }
        else if (!colTouchCheck.IsGrounded || Mathf.Abs(moveInput.x) == 0)
        {
            audioManager.StopMovementSound();
        }
        wasGrounded = colTouchCheck.IsGrounded;

        // Handle jump animations
        if (Mathf.Abs(rb.velocity.y)>0.1)
        {
            anim.SetTrigger("Jump");
        }

        // Flip character depending on movement direction
        if (moveInput.x < 0 && facingRight)
        {
            Flip();
        }
        else if (moveInput.x > 0 && !facingRight)
        {
            Flip();
        }

        // Jump handling
        if (colTouchCheck.IsGrounded && isJumpHeld)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
       
    }


    // --- Handle jumping inputs ---
    public void OnJump(InputAction.CallbackContext context)
    {
        if (isDead) return;
        if (context.started)
        {
            isJumpHeld = true;
            if (colTouchCheck.IsGrounded)
            {
                audioManager.PlayJumpSound();
                rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
            }
        }

        if (context.canceled)
        {
            isJumpHeld = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.3f);
        }
    }

    // --- Sword reload and attack ---
    IEnumerator Sword_Reload()
    {
        isSwordReloading = true;
        yield return new WaitForSeconds(reloadTime);
        isSwordReloading = false;
    }
    public void Sword_Attack(InputAction.CallbackContext context)
    {
        if (isDead) return;
        if (context.performed)
        {
            if (!isSwordReloading)
            {
                Collider2D[] damage = Physics2D.OverlapCircleAll(attack_p.transform.position, 2, enemy);
                foreach (Collider2D col in damage)
                {
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
                audioManager.PlaySwordAttackSound();
                StartCoroutine(Sword_Reload());
            }
        }
    }


    // --- Fireball reload and attack ---
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
        if (isDead) return;
        if (context.performed)
        {
            if (!isFireballReloading)
            {
                anim.SetTrigger("Fireball");
                audioManager.PlayFireballSound();
                StartCoroutine(FireballReload());
            }
        }
    }


    // --- Flash red on damage ---
    private IEnumerator FlashRed()
    {
        spr.color = new Color32(255, 105, 105, 255);
        yield return new WaitForSeconds(0.15f); // Delay
        spr.color = originalColor;
    }

    // --- Handle player taking damage ---
    public void TakeDamage(int damage)
    {
        if (isDead) return;
        playerComponent.Health -= damage;
        StartCoroutine(FlashRed());
        audioManager.PlayTakeDamageSound();
        if (playerComponent.Health <= 0)
        {
            // Player death logic
            isDead = true;
            FindObjectOfType<CoinManager>().ResetLevelCoins();

            // Align sprite position for death animation
            Transform spriteTransform = GetComponentInChildren<SpriteRenderer>().transform;
            spriteTransform.localPosition = new Vector3(spriteTransform.localPosition.x, -0.0422f, spriteTransform.localPosition.z);

            anim.SetTrigger("Die");

            deathPanel.SetActive(true);
        }
    }





}
