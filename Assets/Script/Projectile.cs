using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector2 moveSpeed = new Vector2(5f,0);
    public int damage = 15;
    public Vector2 knockback = Vector2.zero;
    Rigidbody2D rb;
    public float TimeDestroy;
    private float timeDes;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = new Vector2(moveSpeed.x * transform.localScale.x, moveSpeed.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damgeable damageable = collision.GetComponent<Damgeable>();

        if (damageable != null)
        {
            Vector2 rotate = transform.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);
            // Hit 
            bool hitted = damageable.Hit(damage, rotate);

            if(hitted)
            {
                Destroy(gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        if (timeDes >= TimeDestroy)
            Destroy(gameObject);
        
        timeDes += Time.deltaTime;
    }
}
