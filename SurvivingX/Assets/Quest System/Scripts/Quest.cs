using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Quest
{
    public bool isActive;
    public string title;
    public string description;
    public GameObject reward;
    public Sprite rewardSprite;
    public int rewardAmount;
}
