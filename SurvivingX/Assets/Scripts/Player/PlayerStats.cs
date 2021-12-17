using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{

    PlayerController playerController;

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

    [Header("Bars")]
    [SerializeField]
    HealthBar healthBar;
    [SerializeField]
    HungerBar hungerBar;
    [SerializeField]
    StaminaBar staminaBar;

    public bool takingDamage = false;
    float attackSpeed = 1.5f;
    float attackTimer;
    float hungerRate = 3.5f;
    float hungerTimer;
    float staminaTiredRate = 0.5f;
    float staminaRechargeRate = 0.2f;
    float staminaTimer;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        currentHealth = maxHealth;
        healthBar.setMaxHealth(maxHealth);
        currentHunger = maxHunger;
        hungerBar.setMaxHunger(maxHunger);
        currentStamina = maxStamina;
        staminaBar.setMaxStamina(maxStamina);
    }

    private void Update()
    {
        if(playerController.isSprinting)
        {
            if(currentStamina <= 0)
            {
                playerController.isSprinting = false;
            }
            GetTired(3);
        }
        else
        {
            if(currentStamina >= maxStamina)
            {
                currentStamina = maxStamina;
            }
            rechargeStamina();
        }
        if(hungerTimer >= hungerRate)
        {
            if(currentHunger == 0)
            {
                TakeDamage(4);
            } else
            {
                GetHungry(5);
            }
            hungerTimer = 0;
        }
        hungerTimer += Time.deltaTime;
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

    private void GetHungry(int hunger)
    {
        currentHunger -= hunger;
        hungerBar.setHunger(currentHunger);
    }

    private void GetTired(int stamina)
    {
        if(staminaTimer >= staminaTiredRate)
        {
            currentStamina -= stamina;
            staminaBar.setStamina(currentStamina);
            staminaTimer = 0;
        }
        staminaTimer += Time.deltaTime;
    }

    private void rechargeStamina()
    {
        if(staminaTimer >= staminaRechargeRate)
        {
            currentStamina += 2;
            staminaBar.setStamina(currentStamina);
            staminaTimer = 0;
        }
        staminaTimer += Time.deltaTime;
    }
}
