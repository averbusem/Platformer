using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlight : MonoBehaviour
{
    private float speed = 2.0f;
    private float dist = 2.5f;
    private bool reload=false;
    private bool isDead=false;
    private int health = 2;
    float pos_x;
    private Vector2 dir = Vector2.right;
    private Rigidbody2D rb;
    Animator anim;
    SpriteRenderer spt;
    GameObject player;
    GameObject attack_gp;
    public LayerMask player_layer;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pos_x = transform.position.x;
        anim = GetComponent<Animator>();
        spt = GetComponent<SpriteRenderer>();
        player = GameObject.FindWithTag("Player");
        attack_gp = GameObject.FindWithTag("Attack_gp");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isDead)
        {
            if (Vector2.Distance(player.transform.position, transform.position) < 2.5f)
            {
                speed = 0;
                //anim.SetFloat("Speed", Mathf.Abs(speed));
                if (!reload)
                {
                    //Attack1();
                }
            }
            else
            {
                speed = 2.0f;
                if (Mathf.Abs(transform.position.x - pos_x) < dist)
                {
                    //anim.SetFloat("Speed", Mathf.Abs(speed));
                    transform.Translate(dir * speed * Time.fixedDeltaTime);
                }
                else
                {
                    spt.flipX = !spt.flipX;
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
    public void TakeDamage(int damage) // параметр damage, тк игра может иметь различные источники урона, которые наносят разное количество урона (например, слабая атака — 1, сильная атака — 2)
    {
        health-=damage;

        if (health <= 0)
        {
            Death();
        }
    }
    public void Death()
    {
        anim.SetTrigger("Death");
        isDead = true;
        rb.simulated = false;
    }
}
