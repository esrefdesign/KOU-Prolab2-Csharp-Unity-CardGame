using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using NUnit.Framework;

public class PlayerCardScript : MonoBehaviour, IPointerClickHandler
{
    
    private Vector3[] targetPositions = new Vector3[3];
    private Vector3 originalPosition; // Kartın başlangıç pozisyonu
    private Vector3 originalScale; // Kartın başlangıç boyutu
    private static bool[] positionOccupied = new bool[3]; // Hedef pozisyonların doluluk durumunu takip eder
    private int assignedTargetIndex = -1; // Kartın atandığı hedef pozisyonun indexi (-1 başlangıç pozisyonunu temsil eder)
    private bool isSelected;
    // Seçilen kartlar listesi
    private float scaleMultiplier = 1.2f; 
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
        
        originalScale = transform.localScale;

        AddEventTrigger();

        // Kartın başlangıç pozisyonunu ve boyutunu kaydet
        originalPosition = transform.position;
        
        int x = 380;
        int y = 180;

        // Üç hedef pozisyonu belirle ve aralarında 50 birimlik boşluk bırak
        float spacing = 80f;
        targetPositions[0] = new Vector3(x+ spacing, y, 0);   // İlk hedef pozisyon
        targetPositions[1] = new Vector3(x + spacing * 2, y, 0); // İkinci hedef pozisyon
        targetPositions[2] = new Vector3(x + spacing * 3, y, 0); // Üçüncü hedef pozisyon
    }
private void AddEventTrigger()
    {
        // EventTrigger bileşeni ekle
        EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();

        // PointerEnter olayı
        EventTrigger.Entry pointerEnter = new EventTrigger.Entry();
        pointerEnter.eventID = EventTriggerType.PointerEnter;
        pointerEnter.callback.AddListener((data) => OnPointerEnter());
        eventTrigger.triggers.Add(pointerEnter);

        // PointerExit olayı
        EventTrigger.Entry pointerExit = new EventTrigger.Entry();
        pointerExit.eventID = EventTriggerType.PointerExit;
        pointerExit.callback.AddListener((data) => OnPointerExit());
        eventTrigger.triggers.Add(pointerExit);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (assignedTargetIndex == -1)
        {
           isSelected= true;
            // Kart başlangıç pozisyonundaysa, sıradaki boş hedef pozisyonuna git ve büyüt
            for (int i = 0; i < targetPositions.Length; i++)
            {
                if (!positionOccupied[i])
                {
                    MoveToPositionAndScale(targetPositions[i], originalScale * 1.6f); // Hedef pozisyona giderken boyutu %20 büyüt
                    positionOccupied[i] = true;
                    assignedTargetIndex = i; // Kartın hangi hedef pozisyona atandığını kaydet
                    break;
                }
            }
        }
        else
        {
           isSelected= false;
       
            // Kart bir hedef pozisyondaysa, başlangıç pozisyonuna geri dön ve eski boyuta küçült
            

            MoveToPositionAndScale(originalPosition, originalScale);
            

            positionOccupied[assignedTargetIndex] = false; // Hedef pozisyonu boş olarak işaretle
            assignedTargetIndex = -1; // Kartı başlangıç pozisyonuna at
        }
    }

    public void OnPointerEnter()
    {
        // İmleç kartın üzerine geldiğinde boyutunu büyüt
         if (isSelected) return;

        // İmleç kartın üzerine geldiğinde boyutunu büyüt
        transform.localScale = originalScale * scaleMultiplier;
    }

    public void OnPointerExit()
    {
        // İmleç kartın üzerinden ayrıldığında boyutunu eski haline getir
        if (isSelected) return;

        // İmleç kartın üzerinden ayrıldığında boyutunu eski haline getir
        transform.localScale = originalScale;
    }

    private void MoveToPositionAndScale(Vector3 newPosition, Vector3 newScale)
    {
        transform.position = newPosition;
        
        transform.localScale = newScale;
    }

}
