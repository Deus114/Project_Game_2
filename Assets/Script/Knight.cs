using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirection))]

public class Knight : MonoBehaviour
{
    public float walkSpeed = 3f;
    Rigidbody2D rb;
    TouchingDirection touchingDirection;
    public enum WalkableDirection { Left, Right}
    public DetectZone attackZone;
    private Animator animator;
    public DetectZone cliffDetection;

    private WalkableDirection _walkDirection;
    private Vector2 WalkDirectionVector = Vector2.right;
    public bool _hasTarget = false;
    public float walkStopRate = 0.05f;

    Damgeable damgeable;

    public bool HasTarget { get { return _hasTarget; } 
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationString.hasTarget, value); 
        }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationString.canMove);
        }
    }

    public WalkableDirection WalkDirection { 
        get { return _walkDirection; } 
        set { 
            if (_walkDirection != value) 
            {
                // Direction flipped
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
                if(value == WalkableDirection.Right)
                {
                    WalkDirectionVector = Vector2.left;
                }
                else if (value == WalkableDirection.Left)
                {
                    WalkDirectionVector = Vector2.right;
                }
            }
            
            _walkDirection = value; }
    }

    public float AttackCD { get
        {
            return animator.GetFloat(AnimationString.AttackCD);
        }
        private set
        {
            animator.SetFloat(AnimationString.AttackCD, MathF.Max(value,0));
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirection = GetComponent<TouchingDirection>();  
        animator = GetComponent<Animator>();    
        damgeable = GetComponent<Damgeable>();
    }

    // Update is called once per frame
    void Update()
    {
        HasTarget = attackZone.detectedCollider.Count > 0;
        if(AttackCD > 0)
            AttackCD -= Time.deltaTime;
    }

    private void FixedUpdate()
    {

        if(touchingDirection.IsOnWall && touchingDirection.IsGrounded)
        {
            FlipDiretion();
        }
        if (!damgeable.LockVelocity)
        {
            if (CanMove && touchingDirection.IsGrounded)
                rb.velocity = new Vector2(walkSpeed * WalkDirectionVector.x, rb.velocity.y);
            else
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
        }
        
    }

    private void FlipDiretion()
    {
        if(WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }
        else if (WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    public void OnNoCliffDetected()
    {
        if(touchingDirection.IsGrounded)
        {
            FlipDiretion();
        }
    }
}
