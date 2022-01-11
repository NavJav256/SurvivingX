using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{

    StarterAssets.ThirdPersonController playerController;
    ShooterController shooterController;

    [Header("Variables")]
    public int maxHealth = 100;
    public int currentHealth;
    public int maxHunger = 100;
    public int currentHunger;
    public int maxStamina = 100;
    public int currentStamina;
    public int maxMana = 100;
    public int currentMana;
    public int maxEXP = 100;
    public int currentEXP;
    
    [Header("Bars")]
    [SerializeField]
    HealthBar healthBar;
    [SerializeField]
    HungerBar hungerBar;
    [SerializeField]
    StaminaBar staminaBar;
    [SerializeField]
    ManaBar manaBar;
    [SerializeField]
    ExpBar expBar;

    public bool takingDamage = false;
    public bool canShoot = true;
    public bool shot = false;
    public bool gain = false;
    float attackSpeed = 1.5f;
    float attackTimer;
    float hungerRate = 3.5f;
    float hungerTimer;
    float staminaTiredRate = 0.5f;
    float staminaRechargeRate = 0.2f;
    float staminaTimer;
    float manaTimer;
    float manaRechargeRate = 0.3f;

    private void Start()
    {
        playerController = GetComponent<StarterAssets.ThirdPersonController>();
        shooterController = GetComponent<ShooterController>();
        currentHealth = maxHealth;
        healthBar.setMaxHealth(maxHealth);
        currentHunger = maxHunger;
        hungerBar.setMaxHunger(maxHunger);
        currentStamina = maxStamina;
        staminaBar.setMaxStamina(maxStamina);
        currentMana = maxMana;
        manaBar.setMaxMana(maxMana);
        currentEXP = 0;
        expBar.setMaxEXP(maxEXP);
    }

    private void Update()
    {
        if(playerController.isSprinting)
        {
            if(currentStamina <= 0) playerController.canSprint = false;
            GetTired(3);
        }
        else
        {
            if(currentStamina >= maxStamina) currentStamina = maxStamina;
            RechargeStamina();
            playerController.canSprint = true;
        }

        if(hungerTimer >= hungerRate)
        {
            if(currentHunger <= 0) TakeDamage(4);
            else GetHungry(5);
            hungerTimer = 0;
        }
        hungerTimer += Time.deltaTime;

        if (currentMana <= 0) canShoot = false;
        else if (currentMana >= 25) canShoot = true;
        if (shot)
        {
            Shooting(5);
            GainEXP(20);
            shot = false;
        } else
        {
            if (currentMana >= maxMana) currentMana = maxMana;
            RechargeMana();
        }

        if(takingDamage)
        {
            if(attackTimer >= attackSpeed)
            {
                TakeDamage(10);
                attackTimer = 0;
            }
            attackTimer += Time.deltaTime;
        }
        if (currentHealth <= 0) SceneManager.LoadScene("EndScreen");

        if (gain)
        {
            
            gain = false;
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

    private void RechargeStamina()
    {
        if(staminaTimer >= staminaRechargeRate)
        {
            currentStamina += 2;
            staminaBar.setStamina(currentStamina);
            staminaTimer = 0;
        }
        staminaTimer += Time.deltaTime;
    }

    private void Shooting(int mana)
    {
        currentMana -= mana;
        manaBar.setMana(currentMana);
    }

    private void RechargeMana()
    {
        if (manaTimer >= manaRechargeRate)
        {
            currentMana += 3;
            manaBar.setMana(currentMana);
            manaTimer = 0;
        }
        manaTimer += Time.deltaTime;
    }

    private void GainEXP(int xp)
    {
        currentEXP += xp;
        if (currentEXP >= maxEXP)
        {
            int remainder = currentEXP - maxEXP;
            maxEXP *= 2;
            expBar.setEXP(remainder);
            expBar.setMaxEXP(maxEXP);
        }
    }
}
