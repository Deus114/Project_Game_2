using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Wizard_healthbar : MonoBehaviour
{
    [SerializeField] GameObject Win;
    public Slider health;
    public TMP_Text healthBarText;
    private float delay = 2f;

    Damgeable playerDamgeable;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Wizard");
        playerDamgeable = player.GetComponent<Damgeable>();
    }

    // Start is called before the first frame update
    void Start()
    {
        health.value = currentPercent(playerDamgeable.Health, playerDamgeable.MaxHealth);
        healthBarText.text = "BOSS HP: " + playerDamgeable.Health + "/" + playerDamgeable.MaxHealth;
    }

    private void OnEnable()
    {
        playerDamgeable.healthChange.AddListener(OnHealthChange);
    }

    private void OnDisable()
    {
        playerDamgeable.healthChange.RemoveListener(OnHealthChange);
    }

    private void OnHealthChange(int newHealth, int maxHealth)
    {
        if (newHealth <= 0) newHealth = 0;
        health.value = currentPercent(newHealth, maxHealth);
        healthBarText.text = "BOSS HP: " + newHealth + "/" + maxHealth;
    }

    private float currentPercent(float health, float maxHealth)
    {
        return health / maxHealth;
    }

    private void FixedUpdate()
    {
        if (playerDamgeable.Health <= 0)
        {
            if(delay <= 0)
            {
                Win.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                delay -= Time.deltaTime;
            }
        }
    }
}

