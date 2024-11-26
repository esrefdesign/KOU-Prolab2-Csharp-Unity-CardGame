using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;
public class computerCardScript : MonoBehaviour, IPointerClickHandler
{
    // Hedef pozisyonlar
    private Vector3[] targetPositions = new Vector3[3];
    private Vector3 originalPosition; // Kartın başlangıç pozisyonu
    private Vector3 originalScale; // Kartın başlangıç boyutu
    private static bool[] positionOccupied = new bool[3]; // Hedef pozisyonların doluluk durumunu takip eder
    private int assignedTargetIndex = -1; // Kartın atandığı hedef pozisyonun indexi (-1 başlangıç pozisyonunu temsil eder)


    
    void Start()
    {
        // Kartın başlangıç pozisyonunu ve boyutunu kaydet
        originalPosition = transform.position;
        originalScale = transform.localScale;
        
        int x = 380;
        int y = 370;

        // Üç hedef pozisyonu belirle ve aralarında 50 birimlik boşluk bırak
        float spacing = 80f;
        targetPositions[0] = new Vector3(x + spacing, y, 0);   // İlk hedef pozisyon
        targetPositions[1] = new Vector3(x + spacing * 2, y, 0); // İkinci hedef pozisyon
        targetPositions[2] = new Vector3(x + spacing * 3, y, 0); // Üçüncü hedef pozisyon
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (assignedTargetIndex == -1)
        {
            // Kart başlangıç pozisyonundaysa, sıradaki boş hedef pozisyonuna git ve büyüt
            for (int i = 0; i < targetPositions.Length; i++)
            {
                if (!positionOccupied[i])
                {
                    MoveToPositionAndScale(targetPositions[i], originalScale * 1.5f); // Hedef pozisyona giderken boyutu %20 büyüt
                    positionOccupied[i] = true;
                    assignedTargetIndex = i; // Kartın hangi hedef pozisyona atandığını kaydet
                    break;
                }
            }
        }
        else
        {
            // Kart bir hedef pozisyondaysa, başlangıç pozisyonuna geri dön ve eski boyuta küçült
            
    
            MoveToPositionAndScale(originalPosition, originalScale);
            

            positionOccupied[assignedTargetIndex] = false; // Hedef pozisyonu boş olarak işaretle
            assignedTargetIndex = -1; // Kartı başlangıç pozisyonuna at
        }
    }

    private void MoveToPositionAndScale(Vector3 newPosition, Vector3 newScale)
    {
        transform.position = newPosition;
        
        transform.localScale = newScale;
    }
}
