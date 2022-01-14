using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCController : MonoBehaviour
{

    [SerializeField] string[] dialogues;
    public int dialogueIndex = 0;

    void Start()
    {
    }

    public void ShowDialogue(){
        if(dialogueIndex > (dialogues.Length - 1)) dialogueIndex = (dialogues.Length - 1);
        print(name + ": " + dialogues[dialogueIndex]);
    }
}