using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using NUnit.Framework;

public class PlayerCardScript : MonoBehaviour
{
    
    
    private Vector3[] targetPositions = new Vector3[3];
    private Vector3 originalPosition; // Kartın başlangıç pozisyonu
    private Vector3 originalScale; // Kartın başlangıç boyutu
    public spawnPlayerSlot card;
    public bool isSelected ;

    private CardSelectHandler cardSelect;
    
     // Reference to the description text

    public TMP_Text cardText,saglikText; // TextMeshPro için referans
   
    // Kart bilgilerini dinamik olarak ayarla
    public void SetCardInfo(string text,string saglik)
    {
        if (cardText != null)
        {
            cardText.text = text; // Text'i güncelle
        }
        if (saglikText!=null)
        {
            saglikText.text=saglik;
        }
    }
    void Start()
    {
    }
    
   


}
