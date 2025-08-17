using System;
using System.Collections.Generic;
using System.Linq;

namespace Oto_Galeri_Uygulaması
{
    internal class Galeri
    {
        public List<Araba> Arabalar = new List<Araba>();

        public Galeri() { }

        public int ToplamAracSayisi => Arabalar.Count;

        public int KiradakiAracSayisi => Arabalar.Count(a => a.Durum == "Kirada");

        public int GaleridekiAracSayisi => Arabalar.Count(a => a.Durum != "Kirada");

        public int ToplamAracKiralamaSuresi()
        {
            int toplam = 0;
            foreach (Araba item in Arabalar)
                toplam += item.ToplamKiralanmaSuresi;
            return toplam;
        }

        public int ToplamAracKiralamaAdeti => Arabalar.Sum(a => a.KiralanmaSayisi);

       
        public int Ciro
        {
            get
            {
                int toplam = 0;
                foreach (var a in Arabalar)
                {
                    foreach (var saat in a.KiralamaSureleri)
                    {
                        int gun = Math.Max(1, (int)Math.Ceiling(saat / 24.0));
                        toplam += (int)Math.Round(gun * a.KiralamaBedeli);
                    }
                }
                return toplam;
            }
        }

        public bool ArabaKirala(string plaka, int sureSaat, out string mesaj)
        {
            var a = Arabalar.FirstOrDefault(x => (x.Plaka ?? "").ToUpper() == (plaka ?? "").ToUpper());
            if (a == null) { mesaj = "Bu plakada araba yok."; return false; }
            if (a.Durum == "Kirada") { mesaj = "Bu araç zaten kirada."; return false; }
            if (sureSaat <= 0) { mesaj = "Süre 0'dan büyük olmalı."; return false; }

            a.Durum = "Kirada";
            a.KiralamaSureleri.Add(sureSaat);
            mesaj = $"{plaka} plakalı araç {sureSaat} saatliğine kiralandı.";
            return true;
        }

        public bool ArabaTeslimAl(string plaka, out string mesaj)
        {
            var a = Arabalar.FirstOrDefault(x => (x.Plaka ?? "").ToUpper() == (plaka ?? "").ToUpper());
            if (a == null) { mesaj = "Araba bulunamadı."; return false; }
            if (a.Durum != "Kirada") { mesaj = "Bu araç kirada değil."; return false; }

            a.Durum = "Galeride";
            mesaj = "Araba galeride beklemeye alındı.";
            return true;
        }

        public bool KiralamaIptal(string plaka, out string mesaj)
        {
            var a = Arabalar.FirstOrDefault(x => (x.Plaka ?? "").ToUpper() == (plaka ?? "").ToUpper());
            if (a == null) { mesaj = "Araba bulunamadı."; return false; }
            if (a.KiralamaSureleri.Count == 0) { mesaj = "Bu araç için kiralama kaydı yok."; return false; }

            
            a.KiralamaSureleri.RemoveAt(a.KiralamaSureleri.Count - 1);
            a.Durum = "Galeride";
            mesaj = "Son kiralama iptal edildi.";
            return true;
        }

        public bool ArabaEkle(string plaka, string marka, float kiralamaBedeli, string aTipi, out string mesaj)
        {
            if (Arabalar.Any(x => (x.Plaka ?? "").ToUpper() == (plaka ?? "").ToUpper()))
            {
                mesaj = "Bu plaka zaten kayıtlı.";
                return false;
            }
            var a = new Araba(plaka, marka, kiralamaBedeli, aTipi);
            Arabalar.Add(a);
            mesaj = "Araba eklendi.";
            return true;
        }

        public bool ArabaSil(string plaka, out string mesaj)
        {
            var a = Arabalar.FirstOrDefault(x => (x.Plaka ?? "").ToUpper() == (plaka ?? "").ToUpper());
            if (a == null) { mesaj = "Araba bulunamadı."; return false; }
            if (a.Durum == "Kirada") { mesaj = "Araba kiradayken silinemez."; return false; }
            Arabalar.Remove(a);
            mesaj = "Araba silindi.";
            return true;
        }
    }
}
