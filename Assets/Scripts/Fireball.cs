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
        // ���������, ��� ���� ������
        if (player != null)
        {
            // ��������� ����������� � ����
            Vector3 direction = target.position - transform.position;
            // ��������� ���������� �� ����
            float distance = direction.magnitude;
            // ���� ���������� ������ ����, ���������� ������
            if (distance > 0)
            {
                // ����������� ����������� � ���������� ������
                Vector3 moveDirection = direction.normalized * speed * Time.deltaTime;
                transform.position += moveDirection;
                // ���� ������ ������ ����, �����, ��������, ���������� ���
                if (moveDirection.magnitude >= distance)
                {
                    transform.position = new Vector2(pos_x,pos_y); // ������������� ������� �� ����
                }
            }
        }
    }
}
