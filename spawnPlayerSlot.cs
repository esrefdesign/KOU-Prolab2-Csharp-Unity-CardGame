using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;
using System;
using System.IO;


public class spawnPlayerSlot : MonoBehaviour
{
    public GameObject obusPrefab, ucakPrefab, fikrateynPrefab, kfsPrefab, sidaPrefab, sihaPrefab;

    private Vector3[] targetPositions;
    private static bool[] positionOccupied;
    public Transform parentTransform; // Prefablerin ekleneceği Canvas üzerindeki ebeveyn objesi
    public float spacing = 150f; // Prefabler arası boşluk

    public  List<Savas_Araclari>[] rastgeledagitim = new List<Savas_Araclari>[10];

    public Kullanici ben;

    public int adim = 1;

    //public List<Savas_Araclari> cardList= new List<Savas_Araclari>();
    public List<Savas_Araclari> selectedCards = new List<Savas_Araclari>();

    public Button logButton; // Button referansı
    
    public InputFieldLogger inputFieldLogger;
    public Dictionary<string, GameObject> prefabDictionary;
    public int baslangic;
    public string UserName;
    private CardSelectHandler cardSelectHandler;
    public InputField inputField,UserInput; // InputField referansı


   
    
    private void Start()
    {
        
        logButton.onClick.AddListener(StarterInput);
    }

    private void StarterInput()
    {
        if (int.TryParse(inputField.text, out int value))
        {
                baslangic = value; // Değeri kaydet
                Debug.Log("Kullanıcının girdiği değer: " + baslangic);
        }
        else
        {
                Debug.LogWarning("Lütfen geçerli bir sayı girin!");
        }
       
        
        UserName=UserInput.text;
        
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
        for (int i = 0; i < rastgeledagitim.Length; i++)
        {
            rastgeledagitim[i] = new List<Savas_Araclari>();
            rastgeledagitim[i].Add(new Ucak(input));
            rastgeledagitim[i].Add(new Obus(input));
            rastgeledagitim[i].Add(new Fikrateyn(input));

            rastgeledagitim[i].Add(new Siha(input));
            rastgeledagitim[i].Add(new KFS(input));
            rastgeledagitim[i].Add(new Sida(input));
        }

        //aynisini pc icin yapalim

        

        ben = new Kullanici(1,UserName,0);

        //baslangic kart destesini olusturur
        ben.KartListesi.Add(new Ucak(input));
        ben.KartListesi.Add(new Obus(input));
        ben.KartListesi.Add(new Fikrateyn(input));


        System.Random rand = new System.Random();
        ben.KartListesi.Add(rastgeledagitim[0][rand.Next(0, 3)]);
        ben.KartListesi.Add(rastgeledagitim[1][rand.Next(0, 3)]);
        ben.KartListesi.Add(rastgeledagitim[2][rand.Next(0, 3)]);
    }

    public void SpawnPrefabs(int baslangic)
    {
        int index = 0;

        foreach (Savas_Araclari card in ben.KartListesi)
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
        if(adim == 1){

            File.WriteAllText("similasyon.txt", $"Kart seçildi: {card.AltSinif}");
        }
        else{
            File.AppendAllText("similasyon.txt", $"Kart seçildi: {card.AltSinif}");
            
        }
        adim++;
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
