using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardEvent : MonoBehaviour
{
    private Vector3 originalPosition; // Kartın başlangıç pozisyonu
    private Vector3 originalScale;    // Kartın başlangıç boyutu
    private Vector3[] targetPositions; // Hedef pozisyonlar
    private static bool[] positionOccupied; // Hedef pozisyonların doluluk durumu
    public int assignedTargetIndex = -1; // Kartın atandığı hedef pozisyon
    private float scaleMultiplier = 1.2f; 

    public Savas_Araclari associatedCard; // Bu kart ile ilişkili Savas_Araclari nesnesi
    private BattleManager battleManager;  // BattleManager referansı

    // Initialize method updated to accept BattleManager reference
    public void Initialize(Vector3[] targetPositions, bool[] positionOccupied, Savas_Araclari associatedCard, BattleManager battleManager,List<Savas_Araclari> selectedCards)
    {
        this.targetPositions = targetPositions;
        CardEvent.positionOccupied = positionOccupied;
        this.associatedCard = associatedCard;
        this.battleManager = battleManager;

        // Kartın başlangıç pozisyonunu ve boyutunu kaydet
        originalPosition = transform.localPosition;
        originalScale = transform.localScale;
        AddEventTrigger();
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

        EventTrigger.Entry pointerClick = new EventTrigger.Entry();
        pointerClick.eventID = EventTriggerType.PointerClick;
        pointerClick.callback.AddListener((data) => OnPointerClick()); // PointerEventData otomatik atanacak
        eventTrigger.triggers.Add(pointerClick);
    }

    public void OnPointerEnter()
    {
        // İmleç kartın üzerine geldiğinde boyutunu büyüt
        if (associatedCard.isSelected) return;

        transform.localScale = originalScale * scaleMultiplier;
    }

    public void OnPointerExit()
    {
        // İmleç kartın üzerinden ayrıldığında boyutunu eski haline getir
        if (associatedCard.isSelected) return;

        transform.localScale = originalScale;
    }

    public void OnPointerClick()
    {
        if (battleManager.playerCardList.KartListesi.Contains(associatedCard))
        {
            // Kart zaten seçilmişse, pozisyonunu kontrol et
            if (IsAtTargetPosition())
            {
                // Hedef pozisyondaysa, eski pozisyona geri dön
                MoveToPositionAndScale(originalPosition, originalScale);
                associatedCard.isSelected = false;
                battleManager.RemoveSelectedCard(associatedCard);

                // Hedef pozisyonu boşalt
                if (assignedTargetIndex != -1)
                {
                    positionOccupied[assignedTargetIndex] = false;
                    assignedTargetIndex = -1;
                }
            }
            else
            {
                
                associatedCard.isSelected = true;// Orijinal pozisyondaysa, hedef pozisyona git
                MoveToNextAvailablePosition();
                battleManager.AddSelectedCard(associatedCard);
            }
        }
        else if (battleManager.playerSelectedCard.Count < 3)
        {
            // Kart seçilmemiş ve 3'ten az kart seçilmişse, hedef pozisyona git
            associatedCard.isSelected = true;
            battleManager.AddSelectedCard(associatedCard);
            MoveToNextAvailablePosition();
        }
    }


    private bool IsAtTargetPosition()
    {
        // Kartın şu anki pozisyonu hedef pozisyonlarıyla karşılaştırılır
        foreach (Vector3 targetPosition in targetPositions)
        {
            if (Vector3.Distance(transform.localPosition, targetPosition) < 0.01f)
            {
                return true;
            }
        }
        return false;
    }

    private void MoveToNextAvailablePosition()
    {
        for (int i = 0; i < targetPositions.Length; i++)
        {
            if (!positionOccupied[i])
            {
                MoveToPositionAndScale(targetPositions[i], originalScale * 1.6f);
                positionOccupied[i] = true;
                assignedTargetIndex = i; // Hedef pozisyonu kartla ilişkilendir
                break;
            }
        }
    }

    private void MoveToPositionAndScale(Vector3 newPosition, Vector3 newScale)
    {
        transform.localPosition = newPosition;
        transform.localScale = newScale;  // Boyutu sıfırlayın
    }
}
