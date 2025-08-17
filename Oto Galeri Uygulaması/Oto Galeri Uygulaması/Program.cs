using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.Intrinsics.Arm;

namespace Oto_Galeri_Uygulaması
{
    internal class Program
    {
        static Galeri OtoGaleri = new Galeri();

        static void Main(string[] args)
        {
            // Örnek veriler
            OtoGaleri.Arabalar.Add(new Araba("07ASD123", "Renault", 900, "Hatchback"));
            OtoGaleri.Arabalar.Add(new Araba("07QWE123", "Fiat", 1100, "Sedan"));
            OtoGaleri.Arabalar.Add(new Araba("07ZXC123", "Toyota", 1300, "SUV"));

            Uygulama();
        }

        static void Uygulama()
        {
            Menu();
            while (true)
            {
                string secim = SecimAl();
                switch (secim)
                {
                    case "1":
                    case "K": ArabaKirala(); break;
                    case "2":
                    case "T": ArabaTeslimAl(); break;
                    case "3":
                    case "R": KiradakiArabalar(); break;
                    case "4":
                    case "M": GaleridekiArabalar(); break;
                    case "5":
                    case "A": TumArabalar(); break;
                    case "6":
                    case "I": KiralamaIptali(); break;
                    case "7":
                    case "Y": ArabaEkle(); break;
                    case "8":
                    case "S": ArabaSil(); break;
                    case "9":
                    case "G": GaleriBilgileri(); break;
                    case "X": Console.WriteLine("Programdan çıkılıyor..."); return;
                }
                Console.WriteLine();
                Menu();
            }
        }

        static string SecimAl()
        {
            string karakterler = "123456789KTRMAIYSGX";
            int sayac = 0;
            while (true)
            {
                sayac++;
                Console.Write("Seçiminiz: ");
                string giris = (Console.ReadLine() ?? "").ToUpper();
                if (karakterler.Contains(giris)) return giris;
                if (sayac == 10)
                {
                    Console.WriteLine("Araç bulunamadı.");
                    Environment.Exit(0);
                }
                Console.WriteLine("Hatalı Giriş Yapıldı\n");
            }
        }

        static void Menu()
        {
            Console.WriteLine("Galeri Otomasyon");
            Console.WriteLine("1- Araba Kirala (K)");
            Console.WriteLine("2- Araba Teslim Al (T)");
            Console.WriteLine("3- Kiradaki Arabaları Listele (R)");
            Console.WriteLine("4- Galerideki Arabaları Listele (M)");
            Console.WriteLine("5- Tüm Arabaları Listele (A)");
            Console.WriteLine("6- Kiralama İptali (I)");
            Console.WriteLine("7- Araba Ekle (Y)");
            Console.WriteLine("8- Araba Sil (S)");
            Console.WriteLine("9- Bilgileri Göster (G)");
            Console.WriteLine("X- Çıkış\n");
        }

        static void ArabaKirala()
        {
            Console.WriteLine("\n-Araba Kirala-\n");
            string plaka = PlakaAlKirala();
            Console.Write("Kiralanma süresi (saat): ");
            int sure;
            if (!int.TryParse(Console.ReadLine(), out sure))
            {
                Console.WriteLine("Geçersiz süre."); return;
            }
            if (OtoGaleri.ArabaKirala(plaka, sure, out string mesaj))
                Console.WriteLine(mesaj);
            else
                Console.WriteLine(mesaj);
        }

        static void ArabaTeslimAl()
        {
            Console.Write("Teslim edilecek arabanın plakası: ");
            string plaka = (Console.ReadLine() ?? "").ToUpper();
            if (OtoGaleri.ArabaTeslimAl(plaka, out string mesaj))
                Console.WriteLine(mesaj);
            else
                Console.WriteLine(mesaj);
        }

        static void KiradakiArabalar()
        {
            Console.WriteLine("\n-Kiradaki Arabalar-\n");
            bool varMi = false;
            foreach (var a in OtoGaleri.Arabalar)
            {
                if (a.Durum == "Kirada")
                {
                    varMi = true;
                    Console.WriteLine($"{a.Plaka}  {a.Marka}  {a.KiralamaBedeli} TL/gün  {a.AracTipi}  (K. Sayısı: {a.KiralanmaSayisi})");
                }
            }
            if (!varMi) Console.WriteLine("(kirada araç yok)");
        }

        static void GaleridekiArabalar()
        {
            Console.WriteLine("-Galerideki Arabalar-");
            bool varMi = false;
            foreach (var a in OtoGaleri.Arabalar)
            {
                if (a.Durum != "Kirada")
                {
                    varMi = true;
                    Console.WriteLine($"{a.Plaka}  {a.Marka}  {a.KiralamaBedeli} TL/gün  {a.AracTipi}");
                }
            }
            if (!varMi) Console.WriteLine("(galeride araç yok)");
        }

        static void TumArabalar()
        {
            Console.WriteLine("\nTüm Arabalar\n");
            Console.WriteLine("Plaka       Marka     K.Bedeli   Araba Tipi   K.Sayısı   Durum");
            Console.WriteLine("----------------------------------------------------------------");
            if (OtoGaleri.Arabalar.Count == 0) { Console.WriteLine("(Kayıt yok)"); return; }

            foreach (var a in OtoGaleri.Arabalar)
            {
                Console.WriteLine(
                    a.Plaka.PadRight(12) + " " +
                    a.Marka.PadRight(9) + " " +
                    a.KiralamaBedeli.ToString().PadRight(9) + " " +
                    a.AracTipi.PadRight(11) + " " +
                    a.KiralanmaSayisi.ToString().PadRight(9) + " " +
                    a.Durum
                );
            }
            Console.WriteLine();
        }

        static void KiralamaIptali()
        {
            Console.WriteLine("\n-Kiralama İptali-\n");
            Console.Write("Kiralaması iptal edilecek arabanın plakası: ");
            string plaka = (Console.ReadLine() ?? "").ToUpper();
            if (OtoGaleri.KiralamaIptal(plaka, out string mesaj))
                Console.WriteLine(mesaj);
            else
                Console.WriteLine(mesaj);
        }

        static void ArabaEkle()
        {
            Console.WriteLine("\n-Araba Ekle-\n");
            string plaka = PlakaAlEkle();
            Console.Write("Marka: ");
            string marka = Console.ReadLine();
            float bedel = KiralamaBedeli();
            int secim = ArabaTipi();
            string aractipi = AracTipiYazdır(secim);
            if (OtoGaleri.ArabaEkle(plaka, marka, bedel, aractipi, out string mesaj))
                Console.WriteLine(mesaj);
            else
                Console.WriteLine(mesaj);
        }

        static void ArabaSil()
        {
            Console.WriteLine("\n-Araba Sil-\n");
            Console.Write("Silmek istediğiniz arabanın plakası: ");
            string plaka = (Console.ReadLine() ?? "").ToUpper();
            if (OtoGaleri.ArabaSil(plaka, out string mesaj))
                Console.WriteLine(mesaj);
            else
                Console.WriteLine(mesaj);
        }

        static void GaleriBilgileri()
        {
            int toplam = OtoGaleri.ToplamAracSayisi;
            int kirada = OtoGaleri.KiradakiAracSayisi;
            int galeride = OtoGaleri.GaleridekiAracSayisi;

            Console.WriteLine("\n-Galeri Bilgileri-\n");
            Console.WriteLine("Toplam araba sayısı: " + toplam);
            Console.WriteLine("Kiradaki araba sayısı: " + kirada);
            Console.WriteLine("Bekleyen (galeride) araba sayısı: " + galeride);
            Console.WriteLine("Toplam araba kiralama süresi (saat): " + OtoGaleri.ToplamAracKiralamaSuresi());
            Console.WriteLine("Toplam araba kiralama adedi: " + OtoGaleri.ToplamAracKiralamaAdeti);
            Console.WriteLine("Ciro: " + OtoGaleri.Ciro + " TL");
        }

        
        static int ArabaTipi()
        {
            int secim;
            while (true)
            {
                Console.WriteLine("Araç Tipi:");
                Console.WriteLine("SUV için 1");
                Console.WriteLine("Hatchback için 2");
                Console.WriteLine("Sedan için 3");
                Console.Write("Araba Tipi: ");
                string giris = Console.ReadLine();
                if (int.TryParse(giris, out secim) && secim >= 1 && secim <= 3) return secim;
                Console.WriteLine("Giriş tanımlanamadı. Tekrar deneyin.");
            }
        }

        static string AracTipiYazdır(int sayi)
        {
            switch (sayi)
            {
                case 1: return "SUV";
                case 2: return "Hatchback";
                case 3: return "Sedan";
                default: return "Bilinmeyen Araç Tipi";
            }
        }

        static float KiralamaBedeli()
        {
            float sayi;
            while (true)
            {
                Console.Write("Kiralama Bedeli (TL/gün): ");
                string giris = Console.ReadLine();
                if (float.TryParse(giris, out sayi) && sayi > 0) return sayi;
                Console.WriteLine("Giriş tanımlanamadı. Tekrar deneyin.");
            }
        }

        static string PlakaAlKirala()
        {
            while (true)
            {
                Console.Write("Kiralanacak arabanın plakası: ");
                string plaka = (Console.ReadLine() ?? "").ToUpper();
                if (plaka == "X") return SecimAl();
                if (PlakaFormatKontrol(plaka)) return plaka;
                Console.WriteLine("Bu şekilde plaka girişi yapamazsınız. Tekrar deneyin.");
            }
        }

        static string PlakaAlEkle()
        {
            while (true)
            {
                Console.Write("Plaka: ");
                string plaka = (Console.ReadLine() ?? "").ToUpper();
                if (plaka == "X") return SecimAl();
                if (PlakaFormatKontrol(plaka)) return plaka;
                Console.WriteLine("Bu şekilde plaka girişi yapamazsınız. Tekrar deneyin.");
            }
        }

        static bool PlakaFormatKontrol(string plaka)
        {
            if (string.IsNullOrEmpty(plaka)) return false;
            if (plaka.Length != 8) return false;
            if (!char.IsDigit(plaka[0]) || !char.IsDigit(plaka[1])) return false;
            if (!char.IsLetter(plaka[2]) || !char.IsLetter(plaka[3]) || !char.IsLetter(plaka[4])) return false;
            if (!char.IsDigit(plaka[5]) || !char.IsDigit(plaka[6]) || !char.IsDigit(plaka[7])) return false;
            return true;
        }
    }
}
