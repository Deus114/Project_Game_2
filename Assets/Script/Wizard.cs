using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb;
    public Transform player;
    public DetectZone targetZone;
    public DetectZone CanAttack;

    public bool isFlipped = false;
    public bool _hasTarget = false;
    public bool HasTarget
    {
        get { return _hasTarget; }
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationString.hasTarget, value);
        }
    }

    public bool _attack = false;
    public bool Attack
    {
        get { return _hasTarget; }
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationString.canAttack, value);
        }
    }

    public float AttackCD
    {
        get
        {
            return animator.GetFloat(AnimationString.AttackCD);
        }
        private set
        {
            animator.SetFloat(AnimationString.AttackCD, MathF.Max(value, 0));
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HasTarget = targetZone.detectedCollider.Count > 0;
        Attack = CanAttack.detectedCollider.Count > 0;
        if (AttackCD > 0)
            AttackCD -= Time.deltaTime;
    }

    public void lookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= 1f;

        if (transform.position.x < player.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x > player.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }
}
