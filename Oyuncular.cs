using System;
using System.IO;

using System.Collections.Generic;
using UnityEngine;

    public class Oyuncular
    {

        System.Random Random = new System.Random();
        public int OyuncuID { get; set; }
        public string OyuncuAdi { get; set; }
        public int Skor { get; set; }
        public List<Savas_Araclari> KartListesi { get; set; }

        // Parametresiz constructor
        public Oyuncular()
        {
            OyuncuID = 0;
            OyuncuAdi = "Bilinmiyor";
            Skor = 0;
            KartListesi = new List<Savas_Araclari>();
        }

        // Parametreli constructor
        public Oyuncular(int oyuncuID, string oyuncuAdi, int skor)
        {
            OyuncuID = oyuncuID;
            OyuncuAdi = oyuncuAdi;
            Skor = skor;
            KartListesi = new List<Savas_Araclari>();
        }

        // Skor gösterme fonksiyonu
        public void SkorGoster(int artiş)
        {
            Skor += artiş;
            Debug.Log($"{OyuncuAdi} Skor: {Skor}");
            File.AppendAllText("similasyon.txt", $"{OyuncuAdi} Skor: {Skor}\n");


        }

        // Kart seç fonksiyonu (abstract gibi, alt sınıflar override edecek)
        public virtual Savas_Araclari KartSec()
        {
            return null;
        }
    }

    // Bilgisayar sınıfı
    public class Bilgisayar : Oyuncular
    {
        // Constructor
        public Bilgisayar(int oyuncuID, int skor) : base(oyuncuID, "Bilgisayar", skor) { }

        // Random kart seçme işlemi
        public override Savas_Araclari KartSec()
        {
            if (KartListesi.Count == 0)
            {
                Debug.Log("Bilgisayarin elinde kart kalmadi.");
                File.AppendAllText("similasyon.txt", $"Bilgisayarin elinde kart kalmadi.\n");

                
                
            }

           

            System.Random random = new System.Random();
            int index;

            bool hepsi_secili = true;

            foreach (var kart in KartListesi)
            {
                if (!kart.Secilmis)
                {
                    hepsi_secili = false;
                    break;
                }
            }
            if (hepsi_secili)
            {
                do
                {
                    index = random.Next(0, KartListesi.Count);
                } while (!(0 <= index && index < KartListesi.Count));
            }
            else
            {
                do
                {
                    index = random.Next(0, KartListesi.Count);
                } while (!(0 <= index && index < KartListesi.Count) || KartListesi[index].Secilmis);
            }


            Savas_Araclari secilenKart = KartListesi[index];

            if (!secilenKart.Secilmis)
            {
                secilenKart.Secilmis = true;
            }

            KartListesi.RemoveAt(index); // Kartı elden çıkar
            Debug.Log($"Bilgisayar {secilenKart.AltSinif}   kart secti.");
            //File.AppendAllText("similasyon.txt", $"Bilgisayar {secilenKart.AltSinif} kartini secti.\n");
             try
            {
                // Dosya oluştur ve metni yaz
                File.AppendAllText("similasyon.txt", $"Bilgisayar {secilenKart.AltSinif} kartini secti.\n");
                Debug.Log("Dosya başarıyla yazıldı: " + "similasyon.txt");
            }
            catch (IOException e)
            {
                Debug.LogError("Dosya yazılırken bir hata oluştu: " + e.Message);
            }

            
            return secilenKart;
        }
    }

    // Kullanıcı sınıfı
    public class Kullanici : Oyuncular
    {
        // Constructor
        public Kullanici(int oyuncuID, string oyuncuAdi, int skor) : base(oyuncuID, oyuncuAdi, skor) { }

        // Kart seçme işlemi (kullanıcıya seçenek sunar)
        public override Savas_Araclari KartSec()
        {
            if (KartListesi.Count == 0)
            {
                Debug.Log("Elinizde kart kalmadı.");
                File.AppendAllText("similasyon.txt", $"Elnizde kart kalmadi\n");

                return null;
            }

            Debug.Log("Kartlarınız:");
                File.AppendAllText("similasyon.txt", $"Kartlariniz\n");

            for (int i = 0; i < KartListesi.Count; i++)
            {
                Debug.Log($"{i + 1}: {KartListesi[i].AltSinif}, \nSinif: {KartListesi[i].Sinif}, \nDayaniklilik: {KartListesi[i].Dayaniklilik}, \nAvantaj: {KartListesi[i].Avantaj} {KartListesi[i].VurusAvantaji}\n");
                
            }

            Console.Write("Seçmek istediğiniz kartın numarasını girin: ");
            int secim;

            bool hepsi_secili = true;

            foreach (var kart in KartListesi)
            {
                if (!kart.Secilmis)
                {
                    hepsi_secili = false;
                    break;
                }
            }

            if (hepsi_secili)
            {
                while (!int.TryParse(Console.ReadLine(), out secim) || secim < 1 || secim > KartListesi.Count)
                {
                    Console.Write("Geçerli bir seçim yapın: ");
                }
            }
            else
            {
                while (!int.TryParse(Console.ReadLine(), out secim) || secim < 1 || secim > KartListesi.Count || KartListesi[secim - 1].Secilmis)
                {
                    Console.Write("Geçerli bir seçim yapın: ");
                    Console.Write("Daha once secmediginiz bir kart secin: ");

                }
            }

            Savas_Araclari secilenKart = KartListesi[secim - 1];

            if (!secilenKart.Secilmis)
            {
                secilenKart.Secilmis = true;
            }

            KartListesi.RemoveAt(secim - 1); // Kartı elden çıkar
            Debug.Log($"{OyuncuAdi}, {secilenKart.AltSinif} sınıfından bir kart seçti.");
                File.AppendAllText("similasyon.txt", $"{OyuncuAdi}, {secilenKart.AltSinif} kartini secti\n");

            return secilenKart;
        }
    }

