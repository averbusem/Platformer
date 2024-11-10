using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private GameObject player;
    private float speed = 1f;
    private int damage = 1;
    private float pos_x;
    private float pos_y;
    Transform trn;
    Vector3 posi;
    Vector3 direction;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        trn = player.GetComponent<Transform>();
        pos_x = player.transform.position.x;
        pos_y = player.transform.position.y;
        Vector3 posi = new Vector3(pos_x, pos_y,0);
        direction = (posi - transform.position) * speed * Time.deltaTime;
    }
    void FixedUpdate()
    {
        if (player != null)
        {
            transform.position += direction;
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}

