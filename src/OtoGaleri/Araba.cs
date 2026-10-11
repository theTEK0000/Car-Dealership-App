namespace OtoGaleri
{
    public enum AracDurumu
    {
        Galeride,
        Kirada
    }

    public class Araba
    {
        public string Plaka { get; }
        public string Marka { get; }
        public decimal GunlukBedel { get; }
        public string AracTipi { get; }
        public AracDurumu Durum { get; set; } = AracDurumu.Galeride;

        // Her kiralamanın süresi (saat)
        public List<int> KiralamaSureleri { get; } = new List<int>();

        public Araba(string plaka, string marka, decimal gunlukBedel, string aracTipi)
        {
            Plaka = plaka;
            Marka = marka;
            GunlukBedel = gunlukBedel;
            AracTipi = aracTipi;
        }

        public int KiralanmaSayisi => KiralamaSureleri.Count;

        public int ToplamKiralanmaSuresi => KiralamaSureleri.Sum();

        // Başlamış her gün tam gün sayılır (25 saat = 2 gün)
        public static int GunSayisi(int saat) => Math.Max(1, (int)Math.Ceiling(saat / 24.0));

        public decimal Ciro => KiralamaSureleri.Sum(saat => GunSayisi(saat) * GunlukBedel);
    }
}
