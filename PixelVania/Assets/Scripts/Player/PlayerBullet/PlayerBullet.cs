using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private Rigidbody2D playerBulletRigidbody2D;
    PlayerMovement player;
    [SerializeField] private float playerBulletSpeed = 1f;
    private float xSpeed;
    void Start()
    {
        playerBulletRigidbody2D = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
        xSpeed = player.transform.localScale.x * playerBulletSpeed;
    }


    void Update()
    {
        playerBulletRigidbody2D.velocity = new Vector2(xSpeed, 0f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }
        Destroy(gameObject);
    }

}
