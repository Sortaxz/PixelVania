using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Rigidbody2D enemyRigidbody2D;
    [SerializeField] private float enemyMoveSpeed = 1f;


    void Start()
    {
        enemyRigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        enemyRigidbody2D.velocity = new Vector2(enemyMoveSpeed, 0);
    }


    void OnTriggerExit2D(Collider2D collision)
    {
        enemyMoveSpeed = -enemyMoveSpeed;
        FlipEnemyFacing();
    }

    private void FlipEnemyFacing()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(enemyRigidbody2D.velocity.x)), 1f);
    }

}
