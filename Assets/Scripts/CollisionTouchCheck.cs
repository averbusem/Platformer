using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTouchCheck : MonoBehaviour
{
    bool _isGrouded;
    CapsuleCollider2D collision;
    public bool IsGrounded
    {
        get
        {
            return _isGrouded;
        }
        set
        {
            _isGrouded = value;
        }
    }

    private void Awake()
    {
        collision = GetComponent<CapsuleCollider2D>();
    }
    [SerializeField]
    ContactFilter2D ground_filter;
    RaycastHit2D[] grounds_hits = new RaycastHit2D[5];
    float ground_check_distance = 0.05f;
    private void FixedUpdate()
    {
        IsGrounded = collision.Cast(Vector2.down, ground_filter, grounds_hits, ground_check_distance) > 0;
        Debug.Log("IsGrounded: " + IsGrounded);
    }
}
