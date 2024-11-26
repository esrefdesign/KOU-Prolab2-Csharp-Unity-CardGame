using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class InputFieldLogger : MonoBehaviour
{   
    
    public TMP_InputField inputField; // InputField referansı
    public Button logButton;      // Button referansı
    public spawnPlayerSlot spawnPlayerSlotScript;
    public int inputValue ;
   
    public void Start()
    {   
        // Butona tıklama olayını dinler
        logButton.onClick.AddListener(LogInputValue);
    }
    
    public void LogInputValue()
    {
        
        spawnPlayerSlotScript.SpawnPrefabs(inputValue.ToString(),spawnPlayerSlotScript.saglik.ToString());
        
       
        inputValue = Convert.ToInt32(inputField.text);
    
        Destroy(inputField.gameObject);
        Destroy(logButton.gameObject);

        
    }
   
}
