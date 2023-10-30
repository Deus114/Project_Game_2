using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider health;   
    public TMP_Text healthBarText;

    Damgeable playerDamgeable;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerDamgeable = player.GetComponent<Damgeable>();
    }

    // Start is called before the first frame update
    void Start()
    {
        health.value = currentPercent(playerDamgeable.Health, playerDamgeable.MaxHealth);
        healthBarText.text = "HP: " + playerDamgeable.Health + "/" + playerDamgeable.MaxHealth;
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
        if(newHealth <= 0) newHealth = 0;
        health.value = currentPercent(newHealth, maxHealth);
        healthBarText.text = "HP: " + newHealth + "/" + maxHealth;
    }

    private float currentPercent(float health, float maxHealth)
    {
        return health / maxHealth;
    }
}
