using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fireball_enemy : MonoBehaviour
{
    private GameObject player;

    private Transform trn;
    private Vector3 posi;
    private Vector3 direction;

    private float speed = 1f;
    private float pos_x;
    private float pos_y;

    private int damage = 1;
    // получение объектов, компонентов, получаем позицию игрока на моменте появления файербола
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        trn = player.GetComponent<Transform>();
        pos_x = player.transform.position.x;
        pos_y = player.transform.position.y;
        Vector3 posi = new Vector3(pos_x, pos_y,0);
        direction = (posi - transform.position) * speed * Time.deltaTime;
    }
    // летит в точку игрока(которая была определена)
    void FixedUpdate()
    {
        if (player != null)
        {
            transform.position += direction;
        }
    }
    // если попал в игрока, нанес урон
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}

