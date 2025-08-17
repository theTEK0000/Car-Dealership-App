namespace araba1
{
    using System;
    using System.Collections.Generic;

  
    class Araba
    {
        public string Plaka;
        public string Marka;
        public string Model;
        public int Yil;
        public decimal GunlukUcret;

        public bool Kirada;
        public string Kiraci;
        public DateTime BaslangicTarihi;
        public int GunSayisi;
    }

    class Program
    {
        static List<Araba> arabalar = new List<Araba>();
        static decimal kasa = 0;

        static void Main()
        {
            // Örnek veriler
            arabalar.Add(new Araba { Plaka = "34 ABC 123", Marka = "Renault", Model = "Clio", Yil = 2019, GunlukUcret = 900 });
            arabalar.Add(new Araba { Plaka = "06 XYZ 456", Marka = "Fiat", Model = "Egea", Yil = 2021, GunlukUcret = 1100 });
            arabalar.Add(new Araba { Plaka = "35 KLM 789", Marka = "Toyota", Model = "Corolla", Yil = 2020, GunlukUcret = 1300 });

            while (true)
            {
                Console.WriteLine("Galeri Otomasyon");
                Console.WriteLine("1-  Araba Kirala (K)");
                Console.WriteLine("2-  Araba Teslim Al (T)");
                Console.WriteLine("3-  Kiradaki Arabaları Listele (R)");
                Console.WriteLine("4-  Galerideki Arabaları Listele (M)");
                Console.WriteLine("5-  Tüm Arabaları Listele (A)");
                Console.WriteLine("6-  Kiralama İptali (I)");
                Console.WriteLine("7-  Araba Ekle (Y)");
                Console.WriteLine("8-  Araba Sil (S)");
                Console.WriteLine("9-  Bilgileri Göster (G)");
                Console.WriteLine("0-  Çıkış (X)");
                Console.Write("Seçiminiz: ");
                var secim = (Console.ReadLine() ?? "").ToUpper();

                if (secim == "0" || secim == "X") break;
                if (secim == "1" || secim == "K") Kirala();
                else if (secim == "2" || secim == "T") TeslimAl();
                else if (secim == "3" || secim == "R") KiradaListele();
                else if (secim == "4" || secim == "M") GalerideListele();
                else if (secim == "5" || secim == "A") TumunuListele();
                else if (secim == "6" || secim == "I") KiralamaIptali();
                else if (secim == "7" || secim == "Y") ArabaEkle();
                else if (secim == "8" || secim == "S") ArabaSil();
                else if (secim == "9" || secim == "G") BilgileriGoster();

                Console.WriteLine();
            }
        }

        static void Kirala()
        {
            Console.Write("Kiralanacak plaka: ");
            string plaka = Console.ReadLine().ToUpper();
            Araba a = ArabaBul(plaka);
            if (a == null)
            {
                Console.WriteLine("Araba bulunamadı");
                return;
            }
            if (a.Kirada)
            {
                Console.WriteLine("Bu araç zaten kirada");
                return;
            }
            Console.Write("Kiracı adı: ");
            a.Kiraci = Console.ReadLine();
            Console.Write("Gün sayısı: ");
            a.GunSayisi = int.Parse(Console.ReadLine());
            a.BaslangicTarihi = DateTime.Today;
            a.Kirada = true;

            decimal tutar = a.GunlukUcret * a.GunSayisi;
            kasa += tutar;
            Console.WriteLine("Kiralama tamam. Tahsilat: " + tutar + " TL");
        }

        static void TeslimAl()
        {
            Console.Write("Teslim alınacak plaka: ");
            string plaka = Console.ReadLine().ToUpper();
            Araba a = ArabaBul(plaka);
            if (a == null)
            {
                Console.WriteLine("Araba bulunamadı");
                return;
            }
            if (!a.Kirada)
            {
                Console.WriteLine("Bu araç kirada değil");
                return;
            }

            // Basit: gecikme varsa her gün için günlük ücret al
            DateTime tahminiTeslim = a.BaslangicTarihi.AddDays(a.GunSayisi);
            if (DateTime.Today > tahminiTeslim)
            {
                int gecikmeGun = (DateTime.Today - tahminiTeslim).Days;
                decimal ek = gecikmeGun * a.GunlukUcret;
                kasa += ek;
                Console.WriteLine("Gecikme: " + gecikmeGun + " gün, ek tahsilat: " + ek + " TL");
            }

            a.Kirada = false;
            a.Kiraci = null;
            a.GunSayisi = 0;
            a.BaslangicTarihi = DateTime.MinValue;
            Console.WriteLine("Araç teslim alındı");
        }

        static void KiradaListele()
        {
            Console.WriteLine("Kiradaki Arabalar");
            bool varMi = false;
            foreach (var a in arabalar)
            {
                if (a.Kirada)
                {
                    varMi = true;
                    Console.WriteLine(a.Plaka + " " + a.Marka + " " + a.Model + " | Kiracı: " + a.Kiraci);
                }
            }
            if (!varMi) Console.WriteLine("(yok)");
        }

        static void GalerideListele()
        {
            Console.WriteLine("Galerideki Arabalar");
            bool varMi = false;
            foreach (var a in arabalar)
            {
                if (!a.Kirada)
                {
                    varMi = true;
                    Console.WriteLine(a.Plaka + " " + a.Marka + " " + a.Model + " | Günlük: " + a.GunlukUcret + " TL");
                }
            }
            if (!varMi) Console.WriteLine("(yok)");
        }

        static void TumunuListele()
        {
            Console.WriteLine("Tüm Arabalar");
            if (arabalar.Count == 0) Console.WriteLine("(yok)");
            for (int i = 0; i < arabalar.Count; i++)
            {
                var a = arabalar[i];
                string durum = a.Kirada ? "KİRADA" : "GALERİDE";
                Console.WriteLine((i + 1) + ") " + a.Plaka + " " + a.Marka + " " + a.Model + " " + a.Yil + " | " + durum);
            }
        }

        static void KiralamaIptali()
        {
            Console.Write("İptal edilecek plaka: ");
            string plaka = Console.ReadLine().ToUpper();
            Araba a = ArabaBul(plaka);
            if (a == null) { Console.WriteLine("Araba bulunamadı"); return; }
            if (!a.Kirada) { Console.WriteLine("Bu araç kirada değil"); return; }
            a.Kirada = false;
            a.Kiraci = null;
            a.GunSayisi = 0;
            a.BaslangicTarihi = DateTime.MinValue;
            Console.WriteLine("Kiralama iptal edildi");
        }

        static void ArabaEkle()
        {
            Console.Write("Plaka: ");
            string plaka = Console.ReadLine().ToUpper();
            if (ArabaBul(plaka) != null)
            {
                Console.WriteLine("Bu plaka zaten var");
                return;
            }
            Console.Write("Marka: ");
            string marka = Console.ReadLine();
            Console.Write("Model: ");
            string model = Console.ReadLine();
            Console.Write("Yıl: ");
            int yil = int.Parse(Console.ReadLine());
            Console.Write("Günlük Ücret: ");
            decimal ucret = decimal.Parse(Console.ReadLine());

            arabalar.Add(new Araba
            {
                Plaka = plaka,
                Marka = marka,
                Model = model,
                Yil = yil,
                GunlukUcret = ucret
            });
            Console.WriteLine("Araç eklendi");
        }

        static void ArabaSil()
        {
            Console.Write("Silinecek plaka: ");
            string plaka = Console.ReadLine().ToUpper();
            Araba a = ArabaBul(plaka);
            if (a == null) { Console.WriteLine("Araba bulunamadı"); return; }
            if (a.Kirada) { Console.WriteLine("Araç kiradayken silinemez"); return; }
            arabalar.Remove(a);
            Console.WriteLine("Araç silindi");
        }

        static void BilgileriGoster()
        {
            int toplam = arabalar.Count;
            int kirada = 0;
            for (int i = 0; i < arabalar.Count; i++)
            {
                if (arabalar[i].Kirada) kirada++;
            }
            int galeride = toplam - kirada;
            Console.WriteLine("Toplam araç: " + toplam);
            Console.WriteLine("Galeride: " + galeride);
            Console.WriteLine("Kirada: " + kirada);
            Console.WriteLine("Kasa: " + kasa + " TL");
        }

        static Araba ArabaBul(string plaka)
        {
            for (int i = 0; i < arabalar.Count; i++)
            {
                if (arabalar[i].Plaka.ToUpper() == plaka) return arabalar[i];
            }
            return null;
        }
    }

}
