using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : MonoBehaviour
{
    public DetectZone detectZone;
    public List<Transform> wayPoints;

    public float flySpeed = 3f;

    public Collider2D deathColl;
    Animator animator;
    Rigidbody2D rb;
    Damgeable damgeable;
    Transform nextWayPoint;
    int wayPointNum = 0;

    public bool _hasTarget = false;
    public float waypointReachDistance = 0.1f;

    public bool HasTarget
    {
        get { return _hasTarget; }
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

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        damgeable = GetComponent<Damgeable>();
    }

    // Start is called before the first frame update
    void Start()
    {
        nextWayPoint = wayPoints[wayPointNum];
    }

    // Update is called once per frame
    void Update()
    {
        HasTarget = detectZone.detectedCollider.Count > 0;
    }

    private void FixedUpdate()
    {
        if (damgeable.IsAlive)
        {
            if (CanMove)
            {
                Fly();
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
        else
        {
            rb.gravityScale = 2f;
            rb.velocity = new Vector2(0,rb.velocity.y);
        }
    }

    private void Fly()
    {
        // Fly to next waypoint
        Vector2 directiontoWayPoint = (nextWayPoint.position - transform.position).normalized;

        rb.velocity = new Vector2(directiontoWayPoint.x * flySpeed, directiontoWayPoint.y * flySpeed);
        updateDirection();

        // Check if we have reach the waypoint already
        float distance = Vector2.Distance(nextWayPoint.position, transform.position);

        // Check if need to switch waypoint
        if(distance <= waypointReachDistance)
        {
            wayPointNum++;
            if(wayPointNum >= wayPoints.Count)
            {
                wayPointNum = 0;
            }
            nextWayPoint = wayPoints[wayPointNum];
        }
    }

    private void updateDirection()
    {
        Vector3 localscale = transform.localScale;
        if(transform.localScale.x > 0)
        {   
            if(rb.velocity.x < 0)
            //flip direction
            transform.localScale = new Vector3(-1*localscale.x, localscale.y, localscale.z);    
        }

        else
        {
            if ( rb.velocity.x > 0)
            {
                //flip direction
                transform.localScale = new Vector3(-1 * localscale.x, localscale.y, localscale.z);
            }
        }
    }
}
