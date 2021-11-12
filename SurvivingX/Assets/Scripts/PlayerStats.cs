using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    [Header("Variables")]
    [SerializeField]
    int maxHealth = 100;
    [SerializeField]
    int currentHealth;
    [SerializeField]
    int maxHunger = 100;
    [SerializeField]
    int currentHunger;
    [SerializeField]
    int maxStamina = 100;
    [SerializeField]
    int currentStamina;
    [SerializeField]
    int ringCount;

    [SerializeField]
    HealthBar healthBar;

    public bool takingDamage = false;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.setMaxHealth(maxHealth);
    }

    private void Update()
    {
        if(takingDamage)
        {
            TakeDamage(10);
        }
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.setHealth(currentHealth);
    }
}
