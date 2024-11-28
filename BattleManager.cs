using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public GameObject obusPrefab, ucakPrefab, fikrateynPrefab, kfsPrefab, sidaPrefab, sihaPrefab,computerPrefab;

    private Vector3[] targetPositions;
    private static bool[] positionOccupied;
    public Transform parentTransform; // Prefablerin ekleneceği Canvas üzerindeki ebeveyn objesi
    public float spacing = 150f; // Prefabler arası boşluk
    // Savaş başlatma fonksiyonu, selectedCards parametresi alır
    public List<Savas_Araclari> selectedCards;
    public List<Savas_Araclari> cardList;
    public spawnPlayerSlot playerSlot;  // Kartları yöneten sınıf

    private Transform PlayerParent;
    private Transform ComputerParent;

    public Dictionary<string, GameObject> prefabDictionary;


    

    public void InitializeLists(List<Savas_Araclari> selectedCards,List<Savas_Araclari> cardList,Transform playerTransform,Transform computerTransform)
    {
        this.selectedCards = selectedCards;
        this.cardList=cardList;
        this.PlayerParent = playerTransform;
        this.ComputerParent = computerTransform;
        playerSlot = FindFirstObjectByType<spawnPlayerSlot>();
        InitializeTargetPositions();
        prefabDictionary = new Dictionary<string, GameObject>
        {
            { "Obüs", obusPrefab },
            { "Ucak", ucakPrefab },
            { "Firkateyn", fikrateynPrefab },
            { "KFS", kfsPrefab },
            { "Sida", sidaPrefab },
            { "Siha", sihaPrefab }
        };
        StartBattle();
    }

    private IEnumerator RespawnWithDelay(float delay)
    {
    // Belirtilen süre kadar bekle
    yield return new WaitForSeconds(delay);

    // Kartları tekrar spawnla
    RespawnUpdatedCards();
    RespawnComputerCards();
    }

    private void InitializeTargetPositions()
    {
        int maxPositions = 3; // Hedef pozisyon sayısı
        targetPositions = new Vector3[maxPositions];
        positionOccupied = new bool[maxPositions];

        int startX = 70;
        int startY = 200;
        float positionSpacing = 80f;

        for (int i = 0; i < maxPositions; i++)
        {
            targetPositions[i] = new Vector3(startX + i * positionSpacing, startY, 0);
        }
    }

    public void StartBattle()
    {
        // Burada savaş işlemleri başlatılır
        Debug.Log($"Savaş başladı! Seçilen Kartlar:");

        UpdateCardList();


        ClearCardSlots();
        StartCoroutine(RespawnWithDelay(1.2f));
        
    }

    public void UpdateCardList()
    {
        foreach (var card in selectedCards){
            cardList.Remove(card);
            Debug.Log("basariyla silinen kart: "+card.AltSinif);
        }

    }

    private void ClearCardSlots()
    {
        // PlayerCardSlot altındaki tüm çocuk objeleri yok et
        foreach (Transform child in PlayerParent) // `parentTransform` sizin PlayerCardSlot canvas'ınızın Transform'u
        {
            Destroy(child.gameObject); // Her prefabın GameObject'ini yok et
        }
        foreach (Transform child in ComputerParent) // `parentTransform` sizin PlayerCardSlot canvas'ınızın Transform'u
        {
            Destroy(child.gameObject); // Her prefabın GameObject'ini yok et
        }
    }

    private void RespawnUpdatedCards()
    {
        // Kartları güncellenen listeye göre oluştur
        int index = 0;

        foreach (Savas_Araclari card in cardList)
        {
            if (prefabDictionary.ContainsKey(card.AltSinif))
            {
                // Prefabı al ve spawnla
                GameObject prefabToSpawn = prefabDictionary[card.AltSinif];
                GameObject spawnedCard = Instantiate(prefabToSpawn, PlayerParent);

                // Pozisyon ayarı
                RectTransform rectTransform = spawnedCard.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(index * spacing, 0);

                // Kartın seçilmesi için CardSelectHandler ekle
                CardEvent cardHandler = spawnedCard.AddComponent<CardEvent>();
                cardHandler.Initialize(targetPositions, positionOccupied, card, this); // Gerekirse son parametre `null`

                // Kart bilgisi gönder
                PlayerCardScript cardScript = spawnedCard.GetComponent<PlayerCardScript>();
                if (cardScript != null)
                {
                    cardScript.SetCardInfo(card.SeviyePuani.ToString(), card.Dayaniklilik.ToString());
                }

                index++;
            }
            else
            {
                Debug.LogWarning($"Prefab bulunamadı: {card.AltSinif}");
            }

        
        }
    }

    private void RespawnComputerCards()
{
    int index = 0;
    foreach (Savas_Araclari card in cardList)
    {
        GameObject prefabToSpawn = computerPrefab ; // Computer kartı için sabit prefab
        GameObject spawnedCard = Instantiate(prefabToSpawn, ComputerParent);

        RectTransform rectTransform = spawnedCard.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(index * spacing, 0);

        index++;
    }
}

    public void AddSelectedCard(Savas_Araclari card)
    {
        selectedCards.Add(card);
    }

    public void RemoveSelectedCard(Savas_Araclari card)
    {
        selectedCards.Remove(card);
    }
}
