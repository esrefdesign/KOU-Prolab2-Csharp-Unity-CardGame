using System;
using UnityEngine;



    // Abstract Savaş Araçları Sınıfı
    abstract class Savas_Araclari 
    {
        public int SeviyePuani { get; set; }
        public abstract int Dayaniklilik { get; set; }
        public abstract int Vurus { get; set; }
        public abstract string Sinif { get; }
        public abstract string Avantaj { get; }


        public abstract string AltSinif { get; set; }
        //public abstract int KaraVurusAvantaji { get; }
        //public abstract int HavaVurusAvantaji { get; }
        //public abstract int DenizVurusAvantaji { get; }

        public abstract int VurusAvantaji { get; }

        public abstract bool Secilmis { get; set; }






        // Kart puanı göstermek için metod
        public void KartPuaniGoster()
        {
            Console.WriteLine($"Dayaniklilik: {Dayaniklilik}, Seviye Puani: {SeviyePuani}, Sınıf: {Sinif}");
        }

        // Abstract metod: Durum güncelleme
        public abstract void DurumGuncelle(int vurduguHasar);
    }

    // Abstract Hava Araçları Sınıfı
    abstract class Hava_Araclari : Savas_Araclari
    {
        public override string AltSinif { get; set; }
        public override int VurusAvantaji { get; }

        // Constructor
        protected Hava_Araclari(int seviyePuani)
        {
            SeviyePuani = seviyePuani;
        }
    }

    // Uçak Sınıfı
    class Ucak : Hava_Araclari
    {
        public override int Dayaniklilik { get; set; } = 20;
        public override int Vurus { get; set; } = 10;
        public override string Sinif => "Hava";
        public override string AltSinif { get; set; } = "Ucak";

        public override string Avantaj => "Kara";

        public override bool Secilmis { get; set; } = false;





        public override int VurusAvantaji { get; } = 10;
        //public override int HavaVurusAvantaji { get; } = 0;
        //public override int DenizVurusAvantaji { get; } = 0;


        // Constructor
        public Ucak(int seviyePuani) : base(seviyePuani) { }

        // Durum güncelleme
        public override void DurumGuncelle(int vurduguHasar)
        {
            Dayaniklilik -= vurduguHasar;
            SeviyePuani += vurduguHasar;
            Console.WriteLine($"{AltSinif} Durumu Güncellendi: Dayaniklilik = {Dayaniklilik}, SeviyePuani = {SeviyePuani}");
        }
    }

    // SİHA Sınıfı
    class Siha : Hava_Araclari
    {
        public override int Dayaniklilik { get; set; } = 15;
        public override int Vurus { get; set; } = 10;
        public override string Sinif => "Hava";
        public override string AltSinif { get; set; } = "Siha";
        public override string Avantaj => "Kara";
        public override bool Secilmis { get; set; } = false;

        public override int VurusAvantaji { get; } = 10;
        //public override int HavaVurusAvantaji { get; } = 0;
        //public override int DenizVurusAvantaji { get; } = 0;

        // Constructor
        public Siha(int seviyePuani) : base(seviyePuani) { }

        // Durum güncelleme
        public override void DurumGuncelle(int vurduguHasar)
        {
            Dayaniklilik -= vurduguHasar;
            SeviyePuani += vurduguHasar;
            Console.WriteLine($"{AltSinif} Durumu Güncellendi: Dayaniklilik = {Dayaniklilik}, SeviyePuani = {SeviyePuani}");
        }
    }

    // Abstract Kara Araçları Sınıfı
    abstract class Kara_Araclari : Savas_Araclari
    {
        public override string AltSinif { get; set; }
        public override int VurusAvantaji { get; }

        protected Kara_Araclari(int seviyePuani)
        {
            SeviyePuani = seviyePuani;
        }
    }

    // Obüs Sınıfı
    class Obus : Kara_Araclari
    {
        public override int Dayaniklilik { get; set; } = 20;
        public override int Vurus { get; set; } = 10;
        public override string Sinif => "Kara";
        public override string AltSinif => "Obüs";
        public override string Avantaj => "Deniz";
        public override bool Secilmis { get; set; } = false;

        public override int VurusAvantaji => 5;
        //public override int HavaVurusAvantaji => 0;
        //public override int KaraVurusAvantaji => 0;


        public Obus(int seviyePuani) : base(seviyePuani) { }

        // Durum güncelleme
        public override void DurumGuncelle(int vurduguHasar)
        {
            Dayaniklilik -= vurduguHasar;
            SeviyePuani += vurduguHasar;
            Console.WriteLine($"{AltSinif} Durumu Güncellendi: Dayaniklilik = {Dayaniklilik}, SeviyePuani = {SeviyePuani}");
        }
    }

    // KFS Sınıfı
    class KFS : Kara_Araclari
    {
        public override int Dayaniklilik { get; set; } = 10;
        public override int Vurus { get; set; } = 10;
        public override string Sinif => "Kara";
        public override string AltSinif => "KFS";
        public override string Avantaj => "Deniz";
        public override bool Secilmis { get; set; } = false;

        public override int VurusAvantaji => 10;
        //public override int HavaVurusAvantaji => 0;
        //public override int KaraVurusAvantaji => 0;

        public KFS(int seviyePuani) : base(seviyePuani) { }


        // Durum güncelleme
        public override void DurumGuncelle(int vurduguHasar)
        {
            Dayaniklilik -= vurduguHasar;
            SeviyePuani += vurduguHasar;
            Console.WriteLine($"{AltSinif} Durumu Güncellendi: Dayaniklilik = {Dayaniklilik}, SeviyePuani = {SeviyePuani}");
        }
    }

    // Abstract Deniz Araçları Sınıfı
    abstract class Deniz_Araclari : Savas_Araclari
    {
        public override string AltSinif { get; set; }
        public override int VurusAvantaji { get; }
        public override bool Secilmis { get; set; } = false;

        protected Deniz_Araclari(int seviyePuani)
        {
            SeviyePuani = seviyePuani;
        }
    }

    // Firkateyn Sınıfı
    class Fikrateyn : Deniz_Araclari
    {
        public override int Dayaniklilik { get; set; } = 25;
        public override int Vurus { get; set; } = 10;
        public override string Sinif => "Deniz";
        public override string AltSinif => "Firkateyn";
        public override string Avantaj => "Hava";

        public override int VurusAvantaji => 5;
        // public override int DenizVurusAvantaji => 0;
        //public override int KaraVurusAvantaji => 0;


        public Fikrateyn(int seviyePuani) : base(seviyePuani) { }


        // Durum güncelleme
        public override void DurumGuncelle(int vurduguHasar)
        {
            Dayaniklilik -= vurduguHasar;
            SeviyePuani += vurduguHasar;
            Console.WriteLine($"{AltSinif} Durumu Güncellendi: Dayaniklilik = {Dayaniklilik}, SeviyePuani = {SeviyePuani}");
        }
    }

    // SİDA Sınıfı
    class Sida : Deniz_Araclari
    {
        public override int Dayaniklilik { get; set; } = 15;
        public override int Vurus { get; set; } = 10;
        public override string Sinif => "Deniz";
        public override string AltSinif => "Sida";
        public override string Avantaj => "Hava";
        public override bool Secilmis { get; set; } = false;

        public override int VurusAvantaji => 10;
        //public override int DenizVurusAvantaji => 0;
        //public override int KaraVurusAvantaji => 0;

        public Sida(int seviyePuani) : base(seviyePuani) { }


        // Durum güncelleme
        public override void DurumGuncelle(int vurduguHasar)
        {
            Dayaniklilik -= vurduguHasar;
            SeviyePuani += vurduguHasar;
            Console.WriteLine($"{AltSinif} Durumu Güncellendi: Dayaniklilik = {Dayaniklilik}, SeviyePuani = {SeviyePuani}");
        }
    }

