using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateCoord : MonoBehaviour
{
    public Transform player;
    Text txt;

    void Start()
    {
        txt = gameObject.GetComponent<Text>();
    }

    void Update()
    {
        txt.text = "(" + (int) player.position.x + "," + (int) player.position.z + ")";
    }
}
