using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


using UnityEditor;
using Microsoft.Unity.VisualStudio.Editor;
using System;

public class BattleManager : MonoBehaviour
{
    public GameObject obusPrefab, ucakPrefab, fikrateynPrefab, kfsPrefab, sidaPrefab, sihaPrefab,computerPrefab;

    public List <Savas_Araclari> randomCards = new List<Savas_Araclari>();
    public List <Savas_Araclari> randomCardspc = new List<Savas_Araclari>();


    public List <Savas_Araclari> randomCards2 = new List<Savas_Araclari>();
    public List <Savas_Araclari> randomCardspc2 = new List<Savas_Araclari>();

    private Vector3[] targetPositions;
    private static bool[] positionOccupied;
    public Transform parentTransform; // Prefablerin ekleneceği Canvas üzerindeki ebeveyn objesi
    public float spacing = 150f; // Prefabler arası boşluk
    // Savaş başlatma fonksiyonu, selectedCards parametresi alır
    public List<Savas_Araclari> playerSelectedCard;
    public List<Savas_Araclari> computerSelectedCard;
    public Kullanici playerCardList;
    public Bilgisayar computerCardList;

    public spawnPlayerSlot playerSlot; 
    public spawnComputerSlot computerSlot; 
   
    public int adim = 0;

     // Kartları yöneten sınıf

    public Transform ComputerSelecteds;
    private Transform PlayerParent;
    private Transform ComputerParent;

    public Dictionary<string, GameObject> prefabDictionary;
    

    public void QuitGame()
{
    Debug.Log("Oyun kapatılıyor...");
    #if UNITY_EDITOR
        EditorApplication.isPlaying = false; // Editor'da oyunu durdur
    #else
        Application.Quit(); // Build edilmiş oyunda uygulamayı kapat
    #endif
    }
    public void InitializeLists(List<Savas_Araclari> playerSelectedCard,List<Savas_Araclari> computerSelectedCard,Kullanici player,Bilgisayar computer,Transform playerTransform,Transform computerTransform,Transform Selectedtransform)
    {
        this.computerSelectedCard=computerSelectedCard;
        this.playerSelectedCard = playerSelectedCard;
        this.playerCardList=player;
        this.PlayerParent = playerTransform;
        this.ComputerParent = computerTransform;
        this.computerCardList=computer;
        this.ComputerSelecteds=Selectedtransform;
        
        playerSlot = FindAnyObjectByType<spawnPlayerSlot>();
        computerSlot= FindFirstObjectByType<spawnComputerSlot>();

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
    UpdateCardList();
    
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
    private IEnumerator SpawnComputerSelectedWithDelay(float delay)
    {
    // Gecikme

        spawnComputerSelected();
        encounter(playerSelectedCard, computerSelectedCard);

        yield return new WaitForSeconds(delay);
        
        StartCoroutine(RespawnWithDelay(2f));
        ClearCardSlots();
        

    // Kart oluşturma işlemi
    }
    public void spawnComputerSelected()
    {
        // Kartları güncellenen listeye göre oluştur
        int index = 0;

        foreach (Savas_Araclari card in computerSelectedCard)
        {
            if (prefabDictionary.ContainsKey(card.AltSinif))
            {
                // Prefabı al ve spawnla

                card.isSelected=false;
                GameObject prefabToSpawn = prefabDictionary[card.AltSinif];
                GameObject spawnedCard = Instantiate(prefabToSpawn, ComputerSelecteds);

                // Pozisyon ayarı
                RectTransform rectTransform = spawnedCard.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(index, 0);
                rectTransform.localScale = new Vector3(1.5f, 1.5f, 1f);
                // Kartın seçilmesi için CardSelectHandler ekle
                
                // Kart bilgisi gönder
                PlayerCardScript cardScript = spawnedCard.GetComponent<PlayerCardScript>();
                if (cardScript != null)
                {
                    cardScript.SetCardInfo(card.SeviyePuani.ToString(), card.Dayaniklilik.ToString());
                }

                index++;
            }
           

        
        }
    }
    
    public void StartBattle()
    {

        if(adim == 5) {
           
            
            QuitGame();
        }
        // Burada savaş işlemleri başlatılır
        Debug.Log($"Savaş başladı! Seçilen Kartlar:");
        if((adim < 5) || (computerCardList.KartListesi.Count > 0) || (playerCardList.KartListesi.Count > 0)) {

            computerSelectedCard.Add(computerCardList.KartSec());
            computerSelectedCard.Add(computerCardList.KartSec());
            computerSelectedCard.Add(computerCardList.KartSec());
        }
        
        StartCoroutine(SpawnComputerSelectedWithDelay(2f));
    
        
        
        Debug.Log($"{adim}");

        adim++;

        
    }

    public void encounter(List<Savas_Araclari> selectedCards, List<Savas_Araclari> computerSelectedCard)
    {
        System.Random random = new System.Random();
        for (int i = 0; i < computerSelectedCard.Count; i++)
        {
            if (computerSelectedCard[i].Avantaj == selectedCards[i].Sinif) 
            { 
                selectedCards[i].DurumGuncelle(computerSelectedCard[i].Vurus + computerSelectedCard[i].VurusAvantaji);
                ////Debug.Log();
            }
            else
            {
                selectedCards[i].DurumGuncelle(computerSelectedCard[i].Vurus);
                //Debug.Log();

            }

            if (selectedCards[i].Avantaj == computerSelectedCard[i].Sinif)
            {
                computerSelectedCard[i].DurumGuncelle(selectedCards[i].Vurus + selectedCards[i].VurusAvantaji);
                //Debug.Log();

            }
            else
            {
                computerSelectedCard[i].DurumGuncelle(selectedCards[i].Vurus);
                //Debug.Log();

            }

            if (selectedCards[i].Dayaniklilik > 0)
            {
                //playerCardList.KartListesi.Add(selectedCards[i]);
            }
            else if(selectedCards[i].Dayaniklilik <= 0)
            {
                Debug.Log(selectedCards[i].AltSinif + " karti yenildi.");
                if (selectedCards[i].SeviyePuani <= 10)
                {
                    computerCardList.SkorGoster(10);
                    //Debug.Log();

                }
                else
                {
                    computerCardList.SkorGoster(selectedCards[i].SeviyePuani);
                    //Debug.Log();

                }
            }
            if (computerSelectedCard[i].Dayaniklilik > 0)
            {
                computerCardList.KartListesi.Add(computerSelectedCard[i]);
            }
            else if (computerSelectedCard[i].Dayaniklilik <= 0)
            {
                Debug.Log(computerSelectedCard[i].AltSinif + " karti yenildi.");

                if (computerSelectedCard[i].SeviyePuani <= 10)
                {
                    playerCardList.SkorGoster(10);
                    //Debug.Log();

                }
                else
                {
                    playerCardList.SkorGoster(computerSelectedCard[i].SeviyePuani);
                    //Debug.Log();
                }
            }
        }

        /*if(playerCardList.Skor < 20)
        {
            playerCardList.KartListesi.Add(playerSlot.rastgeledagitim[adim + 2][random.Next(0, 3)]);
            if (playerCardList.KartListesi.Count == 2)
            {
                playerCardList.KartListesi.Add(playerSlot.rastgeledagitim[adim + 3][random.Next(0, 3)]);

            }
        }
        else
        {
            playerCardList.KartListesi.Add(playerSlot.rastgeledagitim[adim + 2][random.Next(0, 6)]);
            if (playerCardList.KartListesi.Count == 2)
            {
                playerCardList.KartListesi.Add(playerSlot.rastgeledagitim[adim + 3][random.Next(0, 6)]);

            }
        }


        if(playerCardList.KartListesi.Count < 1 && computerCardList.KartListesi.Count < 1)
        {
            Debug.Log("kart bitti.\n");
            //break;
        }

        if (computerCardList.Skor < 20)
        {
            computerCardList.KartListesi.Add(computerSlot.rastgeledagitimpc[adim + 2][random.Next(0, 3)]);
            if (computerCardList.KartListesi.Count == 2)
            {
                computerCardList.KartListesi.Add(computerSlot.rastgeledagitimpc[adim + 3][random.Next(0, 3)]);
            }
        }
        else
        {
            computerCardList.KartListesi.Add(computerSlot.rastgeledagitimpc[adim + 2][random.Next(0, 6)]);
            if (computerCardList.KartListesi.Count == 2)
            {
                computerCardList.KartListesi.Add(computerSlot.rastgeledagitimpc[adim + 3][random.Next(0, 6)]);
            }
        }
        */
        //random icin listeler olusturulur

        
            
        
        

        randomCards.Add(new Ucak(playerCardList.KartListesi[0].SeviyePuani));
        randomCards.Add(new Obus(playerCardList.KartListesi[0].SeviyePuani));
        randomCards.Add(new Fikrateyn(playerCardList.KartListesi[0].SeviyePuani));

        randomCards.Add(new Siha(playerCardList.KartListesi[0].SeviyePuani));
        randomCards.Add(new KFS(playerCardList.KartListesi[0].SeviyePuani));
        randomCards.Add(new Sida(playerCardList.KartListesi[0].SeviyePuani));


        randomCardspc.Add(new Ucak(playerCardList.KartListesi[0].SeviyePuani));
        randomCardspc.Add(new Obus(playerCardList.KartListesi[0].SeviyePuani));
        randomCardspc.Add(new Fikrateyn(playerCardList.KartListesi[0].SeviyePuani));

        randomCardspc.Add(new Siha(playerCardList.KartListesi[0].SeviyePuani));
        randomCardspc.Add(new KFS(playerCardList.KartListesi[0].SeviyePuani));
        randomCardspc.Add(new Sida(playerCardList.KartListesi[0].SeviyePuani));

        //iki kart almak isterse

        

        randomCards2.Add(new Ucak(playerCardList.KartListesi[0].SeviyePuani));
        randomCards2.Add(new Obus(playerCardList.KartListesi[0].SeviyePuani));
        randomCards2.Add(new Fikrateyn(playerCardList.KartListesi[0].SeviyePuani));

        randomCards2.Add(new Siha(playerCardList.KartListesi[0].SeviyePuani));
        randomCards2.Add(new KFS(playerCardList.KartListesi[0].SeviyePuani));
        randomCards2.Add(new Sida(playerCardList.KartListesi[0].SeviyePuani));


        randomCardspc2.Add(new Ucak(playerCardList.KartListesi[0].SeviyePuani));
        randomCardspc2.Add(new Obus(playerCardList.KartListesi[0].SeviyePuani));
        randomCardspc2.Add(new Fikrateyn(playerCardList.KartListesi[0].SeviyePuani));

        randomCardspc2.Add(new Siha(playerCardList.KartListesi[0].SeviyePuani));
        randomCardspc2.Add(new KFS(playerCardList.KartListesi[0].SeviyePuani));
        randomCardspc2.Add(new Sida(playerCardList.KartListesi[0].SeviyePuani));

        if(playerCardList.Skor < 20)
        {
            playerCardList.KartListesi.Add(randomCards[random.Next(0, 2)]);
            if (playerCardList.KartListesi.Count == 2)
            {
                playerCardList.KartListesi.Add(randomCards2[random.Next(0, 2)]);

            }
        }
        else
        {
            playerCardList.KartListesi.Add(randomCards[random.Next(0, 5)]);
            if (playerCardList.KartListesi.Count == 2)
            {
                playerCardList.KartListesi.Add(randomCards2[random.Next(0, 5)]);

            }
        }


        if(playerCardList.KartListesi.Count < 1 || computerCardList.KartListesi.Count < 1)
        {
            Debug.Log("kart bitti.\n");
            //break;
            QuitGame();
        }

        if (computerCardList.Skor < 20)
        {
            computerCardList.KartListesi.Add(randomCardspc[random.Next(0, 2)]);
            if (computerCardList.KartListesi.Count == 2)
            {
                computerCardList.KartListesi.Add(randomCardspc2[random.Next(0, 2)]);
            }
        }
        else
        {
            computerCardList.KartListesi.Add(randomCardspc[random.Next(0, 5)]);
            if (computerCardList.KartListesi.Count == 2)
            {
                computerCardList.KartListesi.Add(randomCardspc2[random.Next(0, 5)]);
            }
        }
        

        randomCards.Clear();
        randomCardspc.Clear();
        randomCards2.Clear();
        randomCardspc2.Clear();



        selectedCards.Clear();
        computerSelectedCard.Clear();
        //File.WriteAllText("similasyon.txt", "başarıyla yazıldı");

        
    }

    public void UpdateCardList()
    {
        foreach (var card in playerSelectedCard){

            card.isSelected=false;
            
            playerCardList.KartListesi.Remove(card);
            
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
        foreach (Transform child in ComputerSelecteds) // `parentTransform` sizin PlayerCardSlot canvas'ınızın Transform'u
        {
            Destroy(child.gameObject); // Her prefabın GameObject'ini yok et
        }
    }

    private void RespawnUpdatedCards()
    {
        // Kartları güncellenen listeye göre oluştur
        int index = 0;

        foreach (Savas_Araclari card in playerCardList.KartListesi)
        {
            if (prefabDictionary.ContainsKey(card.AltSinif)&&card.Dayaniklilik>0)
            {
                // Prefabı al ve spawnla

                card.isSelected=false;
                GameObject prefabToSpawn = prefabDictionary[card.AltSinif];
                GameObject spawnedCard = Instantiate(prefabToSpawn, PlayerParent);

                // Pozisyon ayarı
                RectTransform rectTransform = spawnedCard.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(index * spacing, 0);

                // Kartın seçilmesi için CardSelectHandler ekle
                CardEvent cardHandler = spawnedCard.AddComponent<CardEvent>();
                cardHandler.Initialize(targetPositions, positionOccupied, card, this,playerSelectedCard); // Gerekirse son parametre `null`

               

                // Kart bilgisi gönder
                PlayerCardScript cardScript = spawnedCard.GetComponent<PlayerCardScript>();
                if (cardScript != null)
                {
                    cardScript.SetCardInfo(card.SeviyePuani.ToString(), card.Dayaniklilik.ToString());
                }

                index++;
            }
            

        
        }
    }

    private void RespawnComputerCards()
{
    int index = 0;
    foreach (Savas_Araclari card in computerCardList.KartListesi)
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
        playerSelectedCard.Add(card);
    }

    public void RemoveSelectedCard(Savas_Araclari card)
    {
        playerSelectedCard.Remove(card);
    }
}
