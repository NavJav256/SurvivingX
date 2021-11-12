using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    float attackSpeed = 3f;
    float attackTimer;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.setMaxHealth(maxHealth);
    }

    private void Update()
    {
        if(takingDamage)
        {
            if(attackTimer >= attackSpeed)
            {
                TakeDamage(10);
                attackTimer = 0;
            }
            attackTimer += Time.deltaTime;
        }
        if (currentHealth <= 0) 
        {
            SceneManager.LoadScene("EndScreen");
        }
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.setHealth(currentHealth);
    }
}
