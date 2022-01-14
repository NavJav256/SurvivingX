using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    public static float gameSensitivity = 1f;
    public static float aimSensitivity = 0.5f;

    public static bool invertYAxis;

    public static float enemyChaseSpeed;
    public static float viewRadius;
    public static float damageRadius;
    public static float enemySpeed;
    public static float damage;

    //Player stats kinda variables

    public static int playerHealth;
    public static int playerSpeed;
    public static int playerDamage;
    public static float playerHealthRegenRate;
    public static float playerStaminaRegenRate;
    public static float playerHManaRegenRate;
    public static float enemyHealth;
    public static int enemyDamage;
    public static flaot enemySpawnRate;
}
