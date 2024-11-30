using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.InputSystem;

public class InputFieldLogger : MonoBehaviour
{   
    
    public InputField inputField,UserInput; // InputField referansı
    public Button logButton;      // Button referansı
    public int inputValue=2 ;
    
    public string UserName;
   

    public void Start()
    {   
        // Butona tıklama olayını dinler
        logButton.onClick.AddListener(LogInputValue);
    }
    
    public void LogInputValue()
    {

        Destroy(inputField.gameObject);
        Destroy(logButton.gameObject);
        Destroy(UserInput.gameObject);
        
        
    }
   
}
