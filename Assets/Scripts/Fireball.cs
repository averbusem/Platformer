using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private GameObject player;
    private float speed = 0.01f;
    private float pos_x;
    private float pos_y;
    Transform trn;
    Vector2 posi;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        trn = player.GetComponent<Transform>();
        pos_x = player.transform.position.x;
        pos_y = player.transform.position.y;
        Vector2 posi = new Vector2(pos_x, pos_y);
    }
    void Update()
    {
        // Проверяем, что цель задана
        if (player != null)
        {
            // Вычисляем направление к цели
            Vector2 direction = posi - transform.position;
            // Вычисляем расстояние до цели
            float distance = direction.magnitude;
            // Если расстояние больше нуля, перемещаем объект
            if (distance > 0)
            {
                // Нормализуем направление и перемещаем объект
                Vector3 moveDirection = direction.normalized * speed * Time.deltaTime;
                transform.position += moveDirection;
                // Если объект достиг цели, можно, например, остановить его
                if (moveDirection.magnitude >= distance)
                {
                    transform.position = Vector2(pos_x,pos_y); // Устанавливаем позицию на цель
                }
            }
        }
    }
}
