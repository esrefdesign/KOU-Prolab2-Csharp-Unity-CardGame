using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarterButtonScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject starterButtonPrefab;
    public Transform parentTransform; 
    public Button mainButton;
    
    
    void Start()
    {
        mainButton.onClick.AddListener(spawnButton);
    }

    public void spawnButton()
    {
        GameObject starterButton = Instantiate(starterButtonPrefab, parentTransform);
    }

    
    // Update is called once per frame
    
}
