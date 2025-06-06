using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameObject playerBulletPrefab;
    [SerializeField] private Transform playerGunTransform;
    [SerializeField] private Transform playerBulletParentTransform;
    private Vector2 moveInput;
    private Rigidbody2D rigidbody2d;
    private Animator animator;
    private PolygonCollider2D bodyCollider2D;
    private BoxCollider2D myFeetCollider2D;
    [SerializeField] private Vector2 deathKick = new Vector2(10f,10f);
     
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private float climbSpeed = 10f;
    [SerializeField] private float gravityScaleAtStart;

    private bool isAlive = true;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bodyCollider2D = GetComponent<PolygonCollider2D>();
        myFeetCollider2D = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = rigidbody2d.gravityScale;
    }

    void Update()
    {
        if (!isAlive) return;

        Run();
        FlipeSprite();
        ClimbLadder();
        Die();
    }



    private void FlipeSprite()
    {


        if (PlayerMovementControl('X'))
        {
            transform.localScale = new Vector2(Mathf.Sign(rigidbody2d.velocity.x), 1f);
        }
    }

    private void Run()
    {


        Vector2 playerVeleocity = new Vector2(moveInput.x * runSpeed, rigidbody2d.velocity.y);
        rigidbody2d.velocity = playerVeleocity;



        animator.SetBool("isRunning", PlayerMovementControl('X'));

    }

    private void OnMove(InputValue value)
    {
        if (!isAlive) return;
        moveInput = value.Get<Vector2>();
    }

    private void OnFire(InputValue inputValue)
    {
        if (!isAlive) return;
        Instantiate(playerBulletPrefab,playerGunTransform.position,Quaternion.identity,playerBulletParentTransform);
    }

    public void OnJump(InputValue inputValue)
    {
        if (!isAlive) return;

        if (!myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"))) return;
        if (inputValue.isPressed)
        {
            rigidbody2d.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    private void ClimbLadder()
    {
        if (!myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            rigidbody2d.gravityScale = gravityScaleAtStart;
            animator.SetBool("isClimbing", false);
            return;
        }


        Vector2 climbVeleocity = new Vector2(rigidbody2d.velocity.x, moveInput.y * climbSpeed);
        rigidbody2d.velocity = climbVeleocity;
        rigidbody2d.gravityScale = 0;

        animator.SetBool("isClimbing", PlayerMovementControl('Y'));
    }

    public bool PlayerMovementControl(char axis)
    {
        bool playerHasHorizontalSpeed = false;
        switch (axis)
        {
            case 'X':
                playerHasHorizontalSpeed = Mathf.Abs(rigidbody2d.velocity.x) > Mathf.Epsilon;
                break;
            case 'Y':
                playerHasHorizontalSpeed = Mathf.Abs(rigidbody2d.velocity.y) > Mathf.Epsilon;
                break;
        }

        return playerHasHorizontalSpeed;
    }



    private void Die()
    {
        if (bodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemies","Hazards")))
        {
            isAlive = false;
            animator.SetTrigger("Dyring");
            rigidbody2d.velocity = deathKick;
        }
        
    }


}
