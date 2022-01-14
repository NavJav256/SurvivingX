using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestGiver : MonoBehaviour
{
    [SerializeField]
    public Quest quest;
    public PlayerStats player;

    public GameObject questWindow;
    public Text titleText;
    public Text descriptionText;
    public Image reward;
    public Text rewardAmount;

    public void OpenQuestWindow()
    {
        questWindow.SetActive(true);
        titleText.text = quest.title;
        descriptionText.text = quest.description;
        reward.sprite = quest.rewardSprite;
        rewardAmount.text = "x" + quest.rewardAmount.ToString();
    }

    public void AcceptQuest()
    {
        questWindow.SetActive(false);
        quest.isActive = true;
        player.quests.Add(quest);
    }
}
