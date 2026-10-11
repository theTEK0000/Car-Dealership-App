namespace OtoGaleri
{
    internal class Program
    {
        static readonly Galeri OtoGaleri = new Galeri();

        static readonly string[] AracTipleri = { "SUV", "Hatchback", "Sedan" };

        static void Main()
        {
            // Örnek veriler
            OtoGaleri.ArabaEkle("07ASD123", "Renault", 900, "Hatchback", out _);
            OtoGaleri.ArabaEkle("07QWE123", "Fiat", 1100, "Sedan", out _);
            OtoGaleri.ArabaEkle("07ZXC123", "Toyota", 1300, "SUV", out _);

            while (true)
            {
                Menu();
                switch (SecimAl())
                {
                    case "1": case "K": ArabaKirala(); break;
                    case "2": case "T": ArabaTeslimAl(); break;
                    case "3": case "R": Listele("Kiradaki Arabalar", a => a.Durum == AracDurumu.Kirada); break;
                    case "4": case "M": Listele("Galerideki Arabalar", a => a.Durum == AracDurumu.Galeride); break;
                    case "5": case "A": Listele("Tüm Arabalar", a => true); break;
                    case "6": case "I": KiralamaIptali(); break;
                    case "7": case "Y": ArabaEkle(); break;
                    case "8": case "S": ArabaSil(); break;
                    case "9": case "G": GaleriBilgileri(); break;
                    case "X":
                        Console.WriteLine("Programdan çıkılıyor...");
                        return;
                }
                Console.WriteLine();
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

        static string SecimAl()
        {
            string[] gecerli = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "K", "T", "R", "M", "A", "I", "Y", "S", "G", "X" };
            while (true)
            {
                Console.Write("Seçiminiz: ");
                string giris = Oku().ToUpperInvariant();
                if (gecerli.Contains(giris)) return giris;
                Console.WriteLine("Hatalı giriş yapıldı.\n");
            }
        }

        static void ArabaKirala()
        {
            Console.WriteLine("\n-Araba Kirala-  (vazgeçmek için X)\n");
            string? plaka = PlakaAl("Kiralanacak arabanın plakası: ");
            if (plaka == null) return;

            Console.Write("Kiralanma süresi (saat): ");
            if (!int.TryParse(Oku(), out int sure))
            {
                Console.WriteLine("Geçersiz süre.");
                return;
            }
            OtoGaleri.ArabaKirala(plaka, sure, out string mesaj);
            Console.WriteLine(mesaj);
        }

        static void ArabaTeslimAl()
        {
            Console.Write("Teslim edilecek arabanın plakası: ");
            OtoGaleri.ArabaTeslimAl(Oku(), out string mesaj);
            Console.WriteLine(mesaj);
        }

        static void KiralamaIptali()
        {
            Console.WriteLine("\n-Kiralama İptali-\n");
            Console.Write("Kiralaması iptal edilecek arabanın plakası: ");
            OtoGaleri.KiralamaIptal(Oku(), out string mesaj);
            Console.WriteLine(mesaj);
        }

        static void ArabaSil()
        {
            Console.WriteLine("\n-Araba Sil-\n");
            Console.Write("Silmek istediğiniz arabanın plakası: ");
            OtoGaleri.ArabaSil(Oku(), out string mesaj);
            Console.WriteLine(mesaj);
        }

        static void ArabaEkle()
        {
            Console.WriteLine("\n-Araba Ekle-  (vazgeçmek için X)\n");
            string? plaka = PlakaAl("Plaka: ");
            if (plaka == null) return;

            Console.Write("Marka: ");
            string marka = Oku();
            decimal bedel = GunlukBedelAl();
            string aracTipi = AracTipiAl();

            OtoGaleri.ArabaEkle(plaka, marka, bedel, aracTipi, out string mesaj);
            Console.WriteLine(mesaj);
        }

        static void Listele(string baslik, Func<Araba, bool> filtre)
        {
            Console.WriteLine($"\n-{baslik}-\n");
            var liste = OtoGaleri.Arabalar.Where(filtre).ToList();
            if (liste.Count == 0)
            {
                Console.WriteLine("(kayıt yok)");
                return;
            }

            Console.WriteLine($"{"Plaka",-10} {"Marka",-10} {"TL/gün",9} {"Tip",-10} {"K.Sayısı",8}  Durum");
            Console.WriteLine(new string('-', 62));
            foreach (var a in liste)
                Console.WriteLine($"{a.Plaka,-10} {a.Marka,-10} {a.GunlukBedel,9:N0} {a.AracTipi,-10} {a.KiralanmaSayisi,8}  {a.Durum}");
        }

        static void GaleriBilgileri()
        {
            Console.WriteLine("\n-Galeri Bilgileri-\n");
            Console.WriteLine($"Toplam araba sayısı: {OtoGaleri.ToplamAracSayisi}");
            Console.WriteLine($"Kiradaki araba sayısı: {OtoGaleri.KiradakiAracSayisi}");
            Console.WriteLine($"Galerideki araba sayısı: {OtoGaleri.GaleridekiAracSayisi}");
            Console.WriteLine($"Toplam kiralama süresi (saat): {OtoGaleri.ToplamKiralamaSuresi}");
            Console.WriteLine($"Toplam kiralama adedi: {OtoGaleri.ToplamKiralamaAdeti}");
            Console.WriteLine($"Ciro: {OtoGaleri.Ciro:N0} TL");
        }

        // Geçerli bir plaka döner; kullanıcı X girerse null döner.
        static string? PlakaAl(string soru)
        {
            while (true)
            {
                Console.Write(soru);
                string giris = Oku();
                if (giris.Equals("X", StringComparison.OrdinalIgnoreCase)) return null;
                if (Plaka.GecerliMi(giris)) return Plaka.Normalize(giris);
                Console.WriteLine("Geçersiz plaka (ör. 34 ABC 123). Tekrar deneyin.");
            }
        }

        static decimal GunlukBedelAl()
        {
            while (true)
            {
                Console.Write("Kiralama bedeli (TL/gün): ");
                if (decimal.TryParse(Oku(), out decimal bedel) && bedel > 0) return bedel;
                Console.WriteLine("Giriş tanımlanamadı. Tekrar deneyin.");
            }
        }

        static string AracTipiAl()
        {
            while (true)
            {
                Console.WriteLine("Araç tipi:");
                for (int i = 0; i < AracTipleri.Length; i++)
                    Console.WriteLine($"{i + 1}- {AracTipleri[i]}");
                Console.Write("Seçim: ");
                if (int.TryParse(Oku(), out int secim) && secim >= 1 && secim <= AracTipleri.Length)
                    return AracTipleri[secim - 1];
                Console.WriteLine("Giriş tanımlanamadı. Tekrar deneyin.");
            }
        }

        // Girdi kapanırsa (Ctrl+Z / Ctrl+D) programı düzgünce kapatır.
        static string Oku()
        {
            string? satir = Console.ReadLine();
            if (satir == null) Environment.Exit(0);
            return satir.Trim();
        }
    }
}
