using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlight : MonoBehaviour
{
    public GameObject fb;
    public LayerMask player_layer;

    [SerializeField] private FlightAudioController audioManager;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spt;
    private Transform trn;
    private GameObject player;
    private GameObject attack_gp;

    private Vector2 dir = Vector2.right;
    private float speed = 2.0f;
    private float dist = 2.5f;
    private float pos_x;
    private float attackRange = 5f;
    private int health = 2;

    private bool reload = false;
    private bool isDead = false;
    private bool isFacingRight = true;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pos_x = transform.position.x;
        anim = GetComponent<Animator>();
        spt = GetComponent<SpriteRenderer>();
        player = GameObject.FindWithTag("Player");
        attack_gp = GameObject.FindWithTag("Attack_gp");
        trn=GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isDead)                                                            // ���� ����� �� ����������� ���������� �� �������, ������ �������
        {
            if (Vector2.Distance(player.transform.position, transform.position) < attackRange)
            {
                audioManager.StopFlyingSound();
                isFacingPlayer();
                speed = 0;
                //anim.SetFloat("Speed", Mathf.Abs(speed));
                if (!reload)
                {
                    Attack1();
                }
            }
            else                                                  // ���� �� ����� ������ �� ������ �����������
            {
                ContinueWalking();
                audioManager.PlayFlyingSound();
                speed = 2.0f;
                if (Mathf.Abs(transform.position.x - pos_x) < dist)
                {
                    //anim.SetFloat("Speed", Mathf.Abs(speed));
                    transform.Translate(dir * speed * Time.fixedDeltaTime);
                }
                else
                {
                    spt.flipX = !spt.flipX;
                    isFacingRight = !isFacingRight;
                    //anim.SetFloat("Speed", Mathf.Abs(speed));
                    if(dir==Vector2.left)
                    {
                        dir=Vector2.right;
                    }
                    else
                    {
                        dir = Vector2.left;
                    }
                    transform.Translate(dir * speed * Time.fixedDeltaTime);
                }
            }
        }
    }
    IEnumerator Reload()
    {
        reload = true;
        yield return new WaitForSeconds(5);
        reload = false;
    }    
   
    private void Attack1()
    {
        Instantiate(fb, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.Euler(0,0,0));
        audioManager.PlayFireballSound();
        StartCoroutine(Reload());
    }
    /*
    private void Attack2() 
    {
        Collider2D[] damage = Physics2D.OverlapCircleAll(attack_gp.transform.position, 3, player_layer);
        foreach (Collider2D col in damage)
        {
            Debug.Log("Shot");
            col.GetComponent<PlayerController>().TakeDamage(1);
        }
    }
    */
    public void TakeDamage(int damage) // �������� damage, �� ���� ����� ����� ��������� ��������� �����, ������� ������� ������ ���������� ����� (��������, ������ ����� � 1, ������� ����� � 2)
    {
        health-=damage;
        audioManager.PlayTakeDamageSound();
        if (health <= 0)
        {
            Death();
        }
    }
    public void Death()
    {
        anim.SetBool("Death",true);
        isDead = true;
        rb.gravityScale = 1.0f;
    }
    public void Troop()
    {
        rb.simulated = false;
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground") && isDead)
        {
            anim.SetFloat("Troop", 1);
        }
    }
    private void isFacingPlayer()
    {
        if(player.transform.position.x>transform.position.x && !isFacingRight) 
        {
            spt.flipX = !spt.flipX;
            isFacingRight = !isFacingRight;
        }
        if (player.transform.position.x < transform.position.x && isFacingRight)
        {
            spt.flipX = !spt.flipX;
            isFacingRight = !isFacingRight;
        }
    }
    public bool IsDead()
    {
        return isDead;
    }
    private void ContinueWalking()
    {
        if (dir == Vector2.left && isFacingRight)
        {
            isFacingRight = false;
            spt.flipX = !spt.flipX;
        }
        if (dir == Vector2.right && !isFacingRight)
        {
            isFacingRight = true;
            spt.flipX = !spt.flipX;
        }
    }
}
