using System.Text.RegularExpressions;

namespace OtoGaleri
{
    public static class Plaka
    {
        // İl kodu 01-81, ardından Türkiye'deki geçerli harf/rakam düzenleri:
        // 34 A 1234, 34 A 12345, 34 AB 123, 34 AB 1234, 34 ABC 12, 34 ABC 123
        private static readonly Regex Desen = new Regex(
            @"^(0[1-9]|[1-7][0-9]|8[01])([A-Z]\d{4,5}|[A-Z]{2}\d{3,4}|[A-Z]{3}\d{2,3})$");

        // Boşlukları atar, büyük harfe çevirir: "34 ab 123" -> "34AB123"
        public static string Normalize(string? plaka) =>
            new string((plaka ?? "").Where(c => !char.IsWhiteSpace(c)).ToArray()).ToUpperInvariant();

        public static bool GecerliMi(string? plaka) => Desen.IsMatch(Normalize(plaka));
    }
}
