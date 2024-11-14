using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpikesDamage : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1;            
    [SerializeField] private float damageCooldown = 2.0f;     
    private bool playerOnSpikes = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerOnSpikes = true;
            StartCoroutine(ApplyPeriodicDamage(collision.gameObject));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerOnSpikes = false;
        }
    }

    private IEnumerator ApplyPeriodicDamage(GameObject player)
    {
        while (playerOnSpikes)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(damageAmount);
            }
            yield return new WaitForSeconds(damageCooldown);
        }
    }
}


