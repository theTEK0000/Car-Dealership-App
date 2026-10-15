using Xunit;

namespace OtoGaleri.Tests
{
    public class GaleriTests
    {
        private static Galeri OrnekGaleri()
        {
            var g = new Galeri();
            g.ArabaEkle("07ASD123", "Renault", 900, "Hatchback", out _);
            g.ArabaEkle("07QWE123", "Fiat", 1100, "Sedan", out _);
            return g;
        }

        [Fact]
        public void AyniPlakaIkinciKezEklenemez()
        {
            var g = OrnekGaleri();
            Assert.False(g.ArabaEkle("07 asd 123", "Opel", 800, "Sedan", out _));
            Assert.Equal(2, g.ToplamAracSayisi);
        }

        [Fact]
        public void GecersizPlakaEklenemez()
        {
            var g = OrnekGaleri();
            Assert.False(g.ArabaEkle("ABC", "Opel", 800, "Sedan", out _));
        }

        [Fact]
        public void KiralananAracKiradaGorunur()
        {
            var g = OrnekGaleri();
            Assert.True(g.ArabaKirala("07asd123", 10, out _));
            Assert.Equal(1, g.KiradakiAracSayisi);
            Assert.Equal(1, g.GaleridekiAracSayisi);
        }

        [Fact]
        public void KiradakiAracTekrarKiralanamaz()
        {
            var g = OrnekGaleri();
            g.ArabaKirala("07ASD123", 10, out _);
            Assert.False(g.ArabaKirala("07ASD123", 5, out _));
        }

        [Theory]
        [InlineData(1, 900)]   // 1 saat = 1 gün
        [InlineData(24, 900)]  // 24 saat = 1 gün
        [InlineData(25, 1800)] // başlamış gün tam sayılır
        [InlineData(72, 2700)]
        public void CiroBaslamisGunuTamSayar(int saat, decimal beklenen)
        {
            var g = OrnekGaleri();
            g.ArabaKirala("07ASD123", saat, out _);
            Assert.Equal(beklenen, g.Ciro);
        }

        [Fact]
        public void TeslimAlinanKiralamaCirodaKalir()
        {
            var g = OrnekGaleri();
            g.ArabaKirala("07ASD123", 30, out _);
            g.ArabaTeslimAl("07ASD123", out _);

            Assert.False(g.KiralamaIptal("07ASD123", out _));
            Assert.Equal(1800, g.Ciro);
            Assert.Equal(1, g.ToplamKiralamaAdeti);
        }

        [Fact]
        public void DevamEdenKiralamaIptalEdilir()
        {
            var g = OrnekGaleri();
            g.ArabaKirala("07ASD123", 30, out _);

            Assert.True(g.KiralamaIptal("07ASD123", out _));
            Assert.Equal(0, g.Ciro);
            Assert.Equal(AracDurumu.Galeride, g.Bul("07ASD123")!.Durum);
        }

        [Fact]
        public void KiradakiAracSilinemez()
        {
            var g = OrnekGaleri();
            g.ArabaKirala("07ASD123", 5, out _);
            Assert.False(g.ArabaSil("07ASD123", out _));
            Assert.True(g.ArabaSil("07QWE123", out _));
            Assert.Equal(1, g.ToplamAracSayisi);
        }
    }
}
