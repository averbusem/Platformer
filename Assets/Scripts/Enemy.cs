using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float speed = 2.0f;
    private float dist = 2.5f;
    private 
    float pos_x;
    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer spt;
    GameObject player;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pos_x = transform.position.x;
        anim = GetComponent<Animator>();
        spt = GetComponent<SpriteRenderer>();
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector2.Distance(player.transform.position, transform.position) < 2)
        {
            Attack();
            StartCoroutine(Reload());
        }
        else
        {
            if (Mathf.Abs(transform.position.x - pos_x) < dist)
            {
                anim.SetFloat("Speed", Mathf.Abs(speed));
                transform.Translate(Vector2.right * speed * Time.fixedDeltaTime);
            }
            else
            {
                spt.flipX = !spt.flipX;
                anim.SetFloat("Speed", Mathf.Abs(speed));
                speed *= -1;
                transform.Translate(Vector2.right * speed * Time.fixedDeltaTime);
            }
        }
    }
    IEnumerator Reload()
    {
        Debug.Log("Yes");
        yield return new WaitForSeconds(5);
    }    
    private void Attack()
    {
        anim.Play("Attack");
    }
}
