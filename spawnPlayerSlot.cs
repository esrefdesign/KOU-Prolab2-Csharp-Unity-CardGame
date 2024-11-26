using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System;
public class spawnPlayerSlot : MonoBehaviour
{
    
    public GameObject obusPrefab; // Prefab sablonunuz
    public GameObject ucakPrefab; // Prefab sablonunuz
    public GameObject fikrateynPrefab; // Prefab sablonunuz
    public GameObject otherPrefab ; // Prefab sablonunuz
     // Prefab sablonunuz
    public Transform parentTransform; // Prefablerin eklenecegi Canvas uzerindeki ebeveyn objesi
    public int numberOfElements = 6; // Kaç tane prefab oluşturulacak
    public float spacing = 50f; // Prefabler arasındaki boşluk miktarı
    List<Savas_Araclari> cardlist = new List<Savas_Araclari>();
    public Dictionary<string, GameObject> prefabDictionary;
    
    public Button logButton;      // Button referansı
    
    public TextMeshProUGUI level; // Reference to the title text
    
    
    public InputFieldLogger value = FindAnyObjectByType<InputFieldLogger>();

    
    public void Start()
    {
        logButton.onClick.AddListener(starterInput);
        
    }

    void starterInput(){
    

        
        
        GenerateTestCardList(value.inputValue);
        
        prefabDictionary = new Dictionary<string, GameObject>
        {
            { "Obüs", obusPrefab },
            { "Ucak", ucakPrefab },
            { "Firkateyn", fikrateynPrefab }
            // Diger isimlere karsilik gelen prefablar burada eklenebilir
        };
        SpawnPrefabs();
    }
    public void GenerateTestCardList(int input)
    {
        
        //kodun burasi liste dizisi olusturuyor ve kullaniciya rastgele nesneler vermemizi saglayacak
        
        List<Savas_Araclari>[] rastgeledagitim = new List<Savas_Araclari>[10];
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

        List<Savas_Araclari>[] rastgeledagitimpc = new List<Savas_Araclari>[10];
        for (int i = 0; i < rastgeledagitimpc.Length; i++)
        {
            rastgeledagitimpc[i] = new List<Savas_Araclari>();
            rastgeledagitimpc[i].Add(new Ucak(input));
            rastgeledagitimpc[i].Add(new Obus(input));
            rastgeledagitimpc[i].Add(new Fikrateyn(input));

            rastgeledagitimpc[i].Add(new Siha(input));
            rastgeledagitimpc[i].Add(new KFS(input));
            rastgeledagitimpc[i].Add(new Sida(input));
        }
        

        
        //baslangic kart destesini olusturur
        cardlist.Add(new Ucak(input));
        cardlist.Add(new Obus(input));
        cardlist.Add(new Fikrateyn(input));
        
        
        System.Random rand = new System.Random();
        cardlist.Add(rastgeledagitim[0][rand.Next(0, 3)]);
        cardlist.Add(rastgeledagitim[1][rand.Next(0, 3)]);
        cardlist.Add(rastgeledagitim[2][rand.Next(0, 3)]);

        
        //cardlist.Add(new Ucak(10));
        //cardlist.Add(new Ucak(10));
        //cardlist.Add(new Ucak(10));

    }

   public void SpawnPrefabs()
   {   
       int i=0;

       foreach (Savas_Araclari card in cardlist)
        {
            if (prefabDictionary.ContainsKey(card.AltSinif))
            {

                card.SeviyePuani=value.inputValue;
                level.text=card.SeviyePuani.ToString();
                // Prefabı al
                GameObject prefabToSpawn = prefabDictionary[card.AltSinif];

                // Prefabı spawnla
                GameObject spawnedCard = Instantiate(prefabToSpawn, parentTransform);

                // Pozisyonu ayarla
                
                RectTransform rectTransform = spawnedCard.GetComponent<RectTransform>();

                // Bir sonraki spawn pozisyonunu ayarla
                rectTransform.anchoredPosition = new Vector2(i*spacing, 0);
                i++;
            }
            else
            {
                Debug.LogWarning($"Prefab bulunamadi: {card.AltSinif}");
            }
        }
   }
}
