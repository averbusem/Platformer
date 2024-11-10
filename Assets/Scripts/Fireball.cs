using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed; 
    public float lifetime;    
    public float distance;    
    public int damage;        
    public LayerMask isSolid; 

    private float timer; // Timer for tracking the life time of a fireball
    private void Start()
    {
        timer = lifetime;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);
        }

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.right, distance, isSolid);
        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag("Goblin"))
            {
                hitInfo.collider.GetComponent<Enemy>().TakeDamage(damage);
            }
            Destroy(gameObject);
        }

        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
}
