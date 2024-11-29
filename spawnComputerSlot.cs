using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class spawnComputerSlot : MonoBehaviour
{   public InputFieldLogger inputFieldLogger;
    public Bilgisayar computer = new Bilgisayar(2, 0); 

    public List<Savas_Araclari> ComputerSelectedCard = new List<Savas_Araclari>();
    public GameObject prefab; // Prefab şablonunuz
    public Transform parentTransform;
    public Transform selectedTransform; // Prefablerin ekleneceği Canvas üzerindeki ebeveyn objesi
    public int numberOfElements = 6; // Kaç tane prefab oluşturulacak
    public Button logButton;
    public float spacing = 2f;
    public int baslangic;

    public List<Savas_Araclari>[] rastgeledagitimpc = new List<Savas_Araclari>[10]; 

    public void Start()
    {
        inputFieldLogger= FindAnyObjectByType<InputFieldLogger>();
        logButton.onClick.AddListener(SpawnPrefabs);
    }
    
    public void GenerateComputerCardList(int input)
    {
        

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

        computer.KartListesi.Add(new Ucak(input));
        computer.KartListesi.Add(new Obus(input));
        computer.KartListesi.Add(new Fikrateyn(input));

        System.Random rand = new System.Random();
        computer.KartListesi.Add(rastgeledagitimpc[0][rand.Next(0, 3)]);
        computer.KartListesi.Add(rastgeledagitimpc[1][rand.Next(0, 3)]);
        computer.KartListesi.Add(rastgeledagitimpc[2][rand.Next(0, 3)]);

        //ComputerSelectedCard.Add(new Ucak(10));
        //ComputerSelectedCard.Add(new Ucak(10));
        //ComputerSelectedCard.Add(new Ucak(10));

    }
    void SpawnPrefabs()
    {   

        baslangic = Convert.ToInt32(inputFieldLogger.inputValue);
        GenerateComputerCardList(baslangic);


        for (int i = 0; i < computer.KartListesi.Count-1; i++)
        {
            // Prefab'i oluştur
            GameObject newElement = Instantiate(prefab, parentTransform);

            // RectTransform üzerinden pozisyon ayarla
            RectTransform rectTransform = newElement.GetComponent<RectTransform>();
            

            if (rectTransform != null)
            {
                // Pozisyonu ayarla (yan yana dizim için)
                rectTransform.anchoredPosition = new Vector2(i*spacing, 0);
            }
        }
    }
}
