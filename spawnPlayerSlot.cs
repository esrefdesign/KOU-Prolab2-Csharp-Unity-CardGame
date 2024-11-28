using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEditor.U2D.Aseprite;

public class spawnPlayerSlot : MonoBehaviour
{
    public GameObject obusPrefab, ucakPrefab, fikrateynPrefab, kfsPrefab, sidaPrefab, sihaPrefab;

    private Vector3[] targetPositions;
    private static bool[] positionOccupied;
    public Transform parentTransform; // Prefablerin ekleneceği Canvas üzerindeki ebeveyn objesi
    public float spacing = 150f; // Prefabler arası boşluk
    public List<Savas_Araclari> cardlist = new List<Savas_Araclari>();
    public List<Savas_Araclari> selectedCards = new List<Savas_Araclari>();

    public Button logButton; // Button referansı
    public InputFieldLogger inputFieldLogger;

    public Dictionary<string, GameObject> prefabDictionary;
    public int baslangic;
    public PlayerCardScript playerCard;
    private CardSelectHandler cardSelectHandler;
    private void Start()
    {
        logButton.onClick.AddListener(StarterInput);
    }

    private void StarterInput()
    {
        baslangic = Convert.ToInt32(inputFieldLogger.inputValue);
        
        // Kart listesini oluştur
        GenerateTestCardList(baslangic);

        // Prefab Dictionary oluştur
        prefabDictionary = new Dictionary<string, GameObject>
        {
            { "Obüs", obusPrefab },
            { "Ucak", ucakPrefab },
            { "Firkateyn", fikrateynPrefab },
            { "KFS", kfsPrefab },
            { "Sida", sidaPrefab },
            { "Siha", sihaPrefab }
        };

        // Hedef pozisyonları ve doluluk durumunu başlat
        InitializeTargetPositions();

        // Prefableri spawnla
        SpawnPrefabs(baslangic);
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

    public void GenerateTestCardList(int input)
    {
        // Kart listesi oluştur
        cardlist.Add(new Ucak(input));
        cardlist.Add(new Obus(input));
        cardlist.Add(new Fikrateyn(input));
        cardlist.Add(new Siha(input));
        cardlist.Add(new KFS(input));
        cardlist.Add(new Sida(input));
    }

    public void SpawnPrefabs(int baslangic)
    {
        int index = 0;

        foreach (Savas_Araclari card in cardlist)
        {
            if (prefabDictionary.ContainsKey(card.AltSinif))
            {
                // Prefabı al ve spawnla
                GameObject prefabToSpawn = prefabDictionary[card.AltSinif];
                GameObject spawnedCard = Instantiate(prefabToSpawn, parentTransform);

                // Pozisyon ayarı
                RectTransform rectTransform = spawnedCard.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(index*spacing, 0);

                // Kartın seçilmesi için CardSelectHandler ekle
                CardSelectHandler cardHandler = spawnedCard.AddComponent<CardSelectHandler>();
                cardHandler.Initialize(targetPositions, positionOccupied,card,this);

                // Kart bilgisi gönder
                PlayerCardScript cardScript = spawnedCard.GetComponent<PlayerCardScript>();
                if (cardScript != null)
                {
                    cardScript.SetCardInfo(baslangic.ToString(), card.Dayaniklilik.ToString());
                }
                
                index++;
            }
            else
            {
                Debug.LogWarning($"Prefab bulunamadı: {card.AltSinif}");
            }
        }
    }

    public void AddSelectedCard(Savas_Araclari card)
    {
        selectedCards.Add(card);
        Debug.Log($"Kart seçildi: {card.AltSinif}");
    }

    public void RemoveSelectedCard(Savas_Araclari card)
    {
         selectedCards.Remove(card);
        // Kartın seçim durumunu sıfırla
        card.isSelected = false;

         // Slotların doluluk durumunu güncelle
        positionOccupied[cardSelectHandler.assignedTargetIndex] = false;
    }
}
