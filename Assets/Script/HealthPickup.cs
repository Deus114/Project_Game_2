using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healthRestore = 15;
    public Vector3 spinRotationSpeed = new Vector3(0, 180, 0);
    AudioSource heal;

    private void Awake()
    {
        heal = GetComponent<AudioSource>(); 
    }

    private void Update()
    {
        transform.eulerAngles += spinRotationSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damgeable damgeable = collision.GetComponent<Damgeable>();

        if(damgeable)
        {
            bool healed = damgeable.Heal(healthRestore);

            if (healed)
            {
                if (heal)
                    AudioSource.PlayClipAtPoint(heal.clip, gameObject.transform.position, heal.volume);
                Destroy(gameObject);
            }
        }
    }
}
