namespace OtoGaleri
{
    public class Galeri
    {
        private readonly List<Araba> arabalar = new List<Araba>();

        public IReadOnlyList<Araba> Arabalar => arabalar;

        public int ToplamAracSayisi => arabalar.Count;

        public int KiradakiAracSayisi => arabalar.Count(a => a.Durum == AracDurumu.Kirada);

        public int GaleridekiAracSayisi => arabalar.Count(a => a.Durum == AracDurumu.Galeride);

        public int ToplamKiralamaSuresi => arabalar.Sum(a => a.ToplamKiralanmaSuresi);

        public int ToplamKiralamaAdeti => arabalar.Sum(a => a.KiralanmaSayisi);

        public decimal Ciro => arabalar.Sum(a => a.Ciro);

        public Araba? Bul(string? plaka)
        {
            string p = Plaka.Normalize(plaka);
            return arabalar.FirstOrDefault(a => a.Plaka == p);
        }

        public bool ArabaEkle(string plaka, string marka, decimal gunlukBedel, string aracTipi, out string mesaj)
        {
            string p = Plaka.Normalize(plaka);
            if (!Plaka.GecerliMi(p)) { mesaj = "Geçersiz plaka."; return false; }
            if (string.IsNullOrWhiteSpace(marka)) { mesaj = "Marka boş olamaz."; return false; }
            if (gunlukBedel <= 0) { mesaj = "Kiralama bedeli 0'dan büyük olmalı."; return false; }
            if (Bul(p) != null) { mesaj = "Bu plaka zaten kayıtlı."; return false; }

            arabalar.Add(new Araba(p, marka.Trim(), gunlukBedel, aracTipi));
            mesaj = "Araba eklendi.";
            return true;
        }

        public bool ArabaKirala(string plaka, int sureSaat, out string mesaj)
        {
            var a = Bul(plaka);
            if (a == null) { mesaj = "Bu plakada araba yok."; return false; }
            if (a.Durum == AracDurumu.Kirada) { mesaj = "Bu araç zaten kirada."; return false; }
            if (sureSaat <= 0) { mesaj = "Süre 0'dan büyük olmalı."; return false; }

            a.Durum = AracDurumu.Kirada;
            a.KiralamaSureleri.Add(sureSaat);
            mesaj = $"{a.Plaka} plakalı araç {sureSaat} saatliğine kiralandı.";
            return true;
        }

        public bool ArabaTeslimAl(string plaka, out string mesaj)
        {
            var a = Bul(plaka);
            if (a == null) { mesaj = "Araba bulunamadı."; return false; }
            if (a.Durum != AracDurumu.Kirada) { mesaj = "Bu araç kirada değil."; return false; }

            a.Durum = AracDurumu.Galeride;
            mesaj = "Araba galeride beklemeye alındı.";
            return true;
        }

        // Yalnızca devam eden kiralama iptal edilir; teslim alınmış kiralamalar ciroda kalır.
        public bool KiralamaIptal(string plaka, out string mesaj)
        {
            var a = Bul(plaka);
            if (a == null) { mesaj = "Araba bulunamadı."; return false; }
            if (a.Durum != AracDurumu.Kirada) { mesaj = "Bu araç şu an kirada değil."; return false; }

            a.KiralamaSureleri.RemoveAt(a.KiralamaSureleri.Count - 1);
            a.Durum = AracDurumu.Galeride;
            mesaj = "Kiralama iptal edildi.";
            return true;
        }

        public bool ArabaSil(string plaka, out string mesaj)
        {
            var a = Bul(plaka);
            if (a == null) { mesaj = "Araba bulunamadı."; return false; }
            if (a.Durum == AracDurumu.Kirada) { mesaj = "Araba kiradayken silinemez."; return false; }

            arabalar.Remove(a);
            mesaj = "Araba silindi.";
            return true;
        }
    }
}
