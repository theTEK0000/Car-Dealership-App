namespace araba
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    namespace GaleriOtomasyon
    {
        // Basit araba modeli
        public class Araba
        {
            public int Id { get; set; }
            public string Plaka { get; set; } = string.Empty;
            public string Marka { get; set; } = string.Empty;
            public string Model { get; set; } = string.Empty;
            public int Yil { get; set; }
            public decimal GunlukUcret { get; set; }

            // Kiralama bilgileri
            public bool KiradaMi { get; set; }
            public string? KiraciAdi { get; set; }
            public DateTime? KiralamaBaslangic { get; set; }
            public int? KiralamaGunSayisi { get; set; }

            public DateTime? TahminiTeslim =>
                (KiralamaBaslangic.HasValue && KiralamaGunSayisi.HasValue)
                    ? KiralamaBaslangic.Value.AddDays(KiralamaGunSayisi.Value)
                    : (DateTime?)null;

            public override string ToString()
            {
                var durum = KiradaMi ? $"KİRADA → {KiraciAdi} | Başlangıç: {KiralamaBaslangic:dd.MM.yyyy} | Gün: {KiralamaGunSayisi}" : "GALERİDE";
                return $"#{Id,-3} {Plaka,-10} {Marka} {Model} {Yil} | {GunlukUcret,6:C0} | {durum}";
            }
        }

        public static class Ekran
        {
            public static void MenuYaz()
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
                Console.WriteLine();
                Console.Write("Seçiminiz: ");
            }

            public static void Baslik(string text)
            {
                Console.WriteLine();
                Console.WriteLine(new string('-', 70));
                Console.WriteLine(text);
                Console.WriteLine(new string('-', 70));
            }

            public static void ListeYaz(IEnumerable<Araba> arabalar)
            {
                foreach (var a in arabalar)
                    Console.WriteLine(a);
                if (!arabalar.Any()) Console.WriteLine("(kayıt yok)");
            }

            public static decimal SayiAlDecimal(string mesaj)
            {
                while (true)
                {
                    Console.Write(mesaj);
                    var g = Console.ReadLine();
                    if (decimal.TryParse(g, NumberStyles.Any, new CultureInfo("tr-TR"), out var d) && d >= 0)
                        return d;
                    Console.WriteLine("Hatalı giriş, tekrar deneyin.");
                }
            }

            public static int SayiAlInt(string mesaj, int min = int.MinValue, int max = int.MaxValue)
            {
                while (true)
                {
                    Console.Write(mesaj);
                    var g = Console.ReadLine();
                    if (int.TryParse(g, out var d) && d >= min && d <= max)
                        return d;
                    Console.WriteLine("Hatalı giriş, tekrar deneyin.");
                }
            }

            public static string MetinAl(string mesaj, bool bosOlabilir = false)
            {
                while (true)
                {
                    Console.Write(mesaj);
                    var g = (Console.ReadLine() ?? string.Empty).Trim();
                    if (bosOlabilir || !string.IsNullOrEmpty(g))
                        return g;
                    Console.WriteLine("Boş bırakılamaz, tekrar deneyin.");
                }
            }
        }

        public class Uygulama
        {
            private readonly List<Araba> _arabalar = new();
            private int _idSayaç = 1;
            private decimal _kasa = 0m;

            public Uygulama()
            {
                // Örnek veriler
                ArabaEkle(new Araba { Plaka = "34 ABC 123", Marka = "Renault", Model = "Clio", Yil = 2019, GunlukUcret = 900 });
                ArabaEkle(new Araba { Plaka = "06 XYZ 456", Marka = "Fiat", Model = "Egea", Yil = 2021, GunlukUcret = 1100 });
                ArabaEkle(new Araba { Plaka = "35 KLM 789", Marka = "Toyota", Model = "Corolla", Yil = 2020, GunlukUcret = 1300 });
            }

            public void Calistir()
            {
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                while (true)
                {
                    Ekran.MenuYaz();
                    var secim = (Console.ReadLine() ?? "").Trim().ToUpperInvariant();

                    switch (secim)
                    {
                        case "1":
                        case "K": Kirala(); break;
                        case "2":
                        case "T": TeslimAl(); break;
                        case "3":
                        case "R": KiradakileriListele(); break;
                        case "4":
                        case "M": GaleridekileriListele(); break;
                        case "5":
                        case "A": TumunuListele(); break;
                        case "6":
                        case "I": KiralamaIptali(); break;
                        case "7":
                        case "Y": YeniArabaEkle(); break;
                        case "8":
                        case "S": ArabaSil(); break;
                        case "9":
                        case "G": BilgileriGoster(); break;
                        case "0":
                        case "X": return; // Çıkış
                        default:
                            Console.WriteLine("Geçersiz seçim! Enter'a basın...");
                            Console.ReadLine();
                            break;
                    }
                    Console.WriteLine();
                }
            }

            private void Kirala()
            {
                Ekran.Baslik("ARABA KİRALAMA");
                GaleridekileriListele(false);
                var plaka = Ekran.MetinAl("Kiralanacak plaka: ").ToUpperInvariant();
                var araba = _arabalar.FirstOrDefault(a => a.Plaka.Equals(plaka, StringComparison.OrdinalIgnoreCase));
                if (araba == null) { Console.WriteLine("Araba bulunamadı."); return; }
                if (araba.KiradaMi) { Console.WriteLine("Seçilen araç zaten kirada."); return; }

                var ad = Ekran.MetinAl("Kiracı adı: ");
                var gun = Ekran.SayiAlInt("Gün sayısı: ", 1, 365);

                araba.KiradaMi = true;
                araba.KiraciAdi = ad;
                araba.KiralamaBaslangic = DateTime.Today;
                araba.KiralamaGunSayisi = gun;

                var tahsilat = araba.GunlukUcret * gun;
                _kasa += tahsilat;

                Console.WriteLine($"Onaylandı. Tahsilat: {tahsilat:C0}. Tahmini teslim: {araba.TahminiTeslim:dd.MM.yyyy}");
            }

            private void TeslimAl()
            {
                Ekran.Baslik("ARABA TESLİM ALMA");
                KiradakileriListele(false);
                var plaka = Ekran.MetinAl("Teslim alınacak plaka: ").ToUpperInvariant();
                var araba = _arabalar.FirstOrDefault(a => a.Plaka.Equals(plaka, StringComparison.OrdinalIgnoreCase));
                if (araba == null) { Console.WriteLine("Araba bulunamadı."); return; }
                if (!araba.KiradaMi) { Console.WriteLine("Bu araç kirada değil."); return; }

                // Gecikme ücreti (isteğe bağlı): geciken her gün günlük ücret alınır
                var gecikmeGun = 0;
                if (araba.TahminiTeslim.HasValue && DateTime.Today > araba.TahminiTeslim.Value)
                {
                    gecikmeGun = (DateTime.Today - araba.TahminiTeslim.Value).Days;
                }
                if (gecikmeGun > 0)
                {
                    var gecikmeUcreti = gecikmeGun * araba.GunlukUcret;
                    _kasa += gecikmeUcreti;
                    Console.WriteLine($"Gecikme gün sayısı: {gecikmeGun}, ek tahsilat: {gecikmeUcreti:C0}");
                }

                // Aracı iade al
                araba.KiradaMi = false;
                araba.KiraciAdi = null;
                araba.KiralamaBaslangic = null;
                araba.KiralamaGunSayisi = null;

                Console.WriteLine("Araç teslim alındı.");
            }

            private void KiradakileriListele(bool baslikYaz = true)
            {
                if (baslikYaz) Ekran.Baslik("KİRADAKİ ARABALAR");
                var liste = _arabalar.Where(a => a.KiradaMi).OrderBy(a => a.TahminiTeslim ?? DateTime.MaxValue);
                Ekran.ListeYaz(liste);
            }

            private void GaleridekileriListele(bool baslikYaz = true)
            {
                if (baslikYaz) Ekran.Baslik("GALERİDEKİ ARABALAR");
                var liste = _arabalar.Where(a => !a.KiradaMi).OrderBy(a => a.Marka).ThenBy(a => a.Model);
                Ekran.ListeYaz(liste);
            }

            private void TumunuListele()
            {
                Ekran.Baslik("TÜM ARABALAR");
                var liste = _arabalar.OrderBy(a => a.KiradaMi).ThenBy(a => a.Marka).ThenBy(a => a.Model);
                Ekran.ListeYaz(liste);
            }

            private void KiralamaIptali()
            {
                Ekran.Baslik("KİRALAMA İPTALİ (Ücret iadesiz)");
                KiradakileriListele(false);
                var plaka = Ekran.MetinAl("İptal edilecek plaka: ").ToUpperInvariant();
                var araba = _arabalar.FirstOrDefault(a => a.Plaka.Equals(plaka, StringComparison.OrdinalIgnoreCase));
                if (araba == null) { Console.WriteLine("Araba bulunamadı."); return; }
                if (!araba.KiradaMi) { Console.WriteLine("Bu araç kirada değil."); return; }

                // Basit senaryo: tahsilat iade edilmez, sadece kiralama sonlandırılır
                araba.KiradaMi = false;
                araba.KiraciAdi = null;
                araba.KiralamaBaslangic = null;
                araba.KiralamaGunSayisi = null;
                Console.WriteLine("Kiralama iptal edildi.");
            }

            private void YeniArabaEkle()
            {
                Ekran.Baslik("YENİ ARABA EKLE");
                var plaka = Ekran.MetinAl("Plaka: ").ToUpperInvariant();
                if (_arabalar.Any(a => a.Plaka.Equals(plaka, StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine("Bu plaka zaten kayıtlı!");
                    return;
                }
                var marka = Ekran.MetinAl("Marka: ");
                var model = Ekran.MetinAl("Model: ");
                var yil = Ekran.SayiAlInt("Yıl: ", 1980, DateTime.Today.Year + 1);
                var ucret = Ekran.SayiAlDecimal("Günlük ücret (TL): ");

                ArabaEkle(new Araba
                {
                    Plaka = plaka,
                    Marka = marka,
                    Model = model,
                    Yil = yil,
                    GunlukUcret = ucret
                });

                Console.WriteLine("Araç eklendi.");
            }

            private void ArabaSil()
            {
                Ekran.Baslik("ARABA SİL");
                TumunuListele();
                var plaka = Ekran.MetinAl("Silinecek plaka: ").ToUpperInvariant();
                var araba = _arabalar.FirstOrDefault(a => a.Plaka.Equals(plaka, StringComparison.OrdinalIgnoreCase));
                if (araba == null) { Console.WriteLine("Araba bulunamadı."); return; }
                if (araba.KiradaMi) { Console.WriteLine("Araç kiradayken silinemez!"); return; }

                _arabalar.Remove(araba);
                Console.WriteLine("Araç silindi.");
            }

            private void BilgileriGoster()
            {
                Ekran.Baslik("BİLGİLER");
                var toplam = _arabalar.Count;
                var kirada = _arabalar.Count(a => a.KiradaMi);
                var galeride = toplam - kirada;
                Console.WriteLine($"Toplam araç: {toplam}");
                Console.WriteLine($"Galeride: {galeride}");
                Console.WriteLine($"Kirada: {kirada}");
                Console.WriteLine($"Kasa (toplam tahsilat): {_kasa:C0}");
            }

            private void ArabaEkle(Araba a)
            {
                a.Id = _idSayaç++;
                _arabalar.Add(a);
            }
        }

        internal static class Program
        {
            private static void Main()
            {
                var app = new Uygulama();
                app.Calistir();
            }
        }
    }

}
