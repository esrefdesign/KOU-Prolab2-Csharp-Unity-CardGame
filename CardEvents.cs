using UnityEngine;
using UnityEngine.EventSystems;

public class CardEvent : MonoBehaviour, IPointerClickHandler
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
    public void Initialize(Vector3[] targetPositions, bool[] positionOccupied, Savas_Araclari associatedCard, BattleManager battleManager)
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (battleManager.selectedCards.Contains(associatedCard))
        {
            // Kart zaten seçilmişse, pozisyonunu kontrol et
            if (IsAtTargetPosition())
            {
                // Hedef pozisyondaysa, eski pozisyona geri dön
                MoveToPositionAndScale(originalPosition, originalScale);
                associatedCard.isSelected = false;
                battleManager.RemoveSelectedCard(associatedCard);
            }
            else
            {
                // Orijinal pozisyondaysa, hedef pozisyona git
                MoveToNextAvailablePosition();
            }
        }
        else if (battleManager.selectedCards.Count < 3)
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
            if (Vector3.Distance(transform.localPosition, targetPosition) < 0.1f)
            {
                return true;
            }
        }
        return false;
    }

    private void MoveToNextAvailablePosition()
    {
        // Hedef pozisyonları arasında boş bir yer bulun
        for (int i = 0; i < targetPositions.Length; i++)
        {
            if (!positionOccupied[i])
            {
                MoveToPositionAndScale(targetPositions[i], originalScale * 1.6f);
                positionOccupied[i] = true;
                assignedTargetIndex = i;
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
