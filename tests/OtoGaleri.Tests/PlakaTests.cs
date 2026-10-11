using Xunit;

namespace OtoGaleri.Tests
{
    public class PlakaTests
    {
        [Theory]
        [InlineData("34 A 1234")]
        [InlineData("34 A 12345")]
        [InlineData("06 AB 123")]
        [InlineData("06 AB 1234")]
        [InlineData("07 ABC 12")]
        [InlineData("07abc123")]
        [InlineData("81 ZZ 999")]
        public void GecerliPlakalar(string plaka) => Assert.True(Plaka.GecerliMi(plaka));

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("00 ABC 123")] // il kodu 01-81 arası olmalı
        [InlineData("82 ABC 123")]
        [InlineData("34 ABCD 12")]
        [InlineData("34 123")]
        [InlineData("ABC 34 123")]
        public void GecersizPlakalar(string? plaka) => Assert.False(Plaka.GecerliMi(plaka));

        [Fact]
        public void Normalize_BosluklariAtarVeBuyukHarfYapar() =>
            Assert.Equal("34AB123", Plaka.Normalize(" 34 ab 123 "));
    }
}
