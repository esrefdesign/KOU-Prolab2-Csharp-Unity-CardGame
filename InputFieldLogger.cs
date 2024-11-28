using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class InputFieldLogger : MonoBehaviour
{   
    
    public InputField inputField; // InputField referansı
    public Button logButton;      // Button referansı
    public int inputValue ;
   
    public void Start()
    {   
        // Butona tıklama olayını dinler
        logButton.onClick.AddListener(LogInputValue);
    }
    
    public void LogInputValue()
    {
        
        inputValue = Convert.ToInt32(inputField.text);
        
        Destroy(inputField.gameObject);
        Destroy(logButton.gameObject);
        
        
    }
   
}
