using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarterScript : MonoBehaviour
{
    public  PlayerCardScript playerScript;
    public Button StartButton;
    private spawnPlayerSlot spawnPlayerSlotScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartButton.onClick.AddListener(findChosenCards);
    }

    // Update is called once per frame
    public void findChosenCards()
    {
        spawnPlayerSlotScript.cardlist[0].isSelected=false;
    }
}
