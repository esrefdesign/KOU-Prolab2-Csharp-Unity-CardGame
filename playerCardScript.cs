using System.Collections.Generic;

using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using NUnit.Framework;

public class PlayerCardScript : MonoBehaviour
{
    
    public TMP_Text cardText,saglikText; // TextMeshPro için referans
   
    // Kart bilgilerini dinamik olarak ayarla
    public void SetCardInfo(string text,string saglik)
    {
        if (cardText != null)
        {
            cardText.text = text.ToString(); // Text'i güncelle
        }
         if (cardText != null)
        {
            saglikText.text = saglik; // Text'i güncelle
        }
    }
    void Start()
    {
        
    }
    
   


}
