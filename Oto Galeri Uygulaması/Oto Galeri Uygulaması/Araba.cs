using System;
using System.Collections.Generic;
using System.Linq;

namespace Oto_Galeri_Uygulaması
{
    public class Araba
    {
        public string Plaka { get; set; }
        public string Marka { get; set; }
        public float KiralamaBedeli { get; set; }  
        public string AracTipi { get; set; }       
        public string Durum { get; set; }          

       
        public List<int> KiralamaSureleri = new List<int>();

       

        public Araba(string plaka, string marka, float kiralamaBedeli, string aracTipi)
        {
            this.Plaka = plaka;
            this.Marka = marka;
            this.KiralamaBedeli = kiralamaBedeli;
            this.AracTipi = aracTipi;
            this.Durum = "Galeride";
        }

        public int KiralanmaSayisi => KiralamaSureleri.Count;

        public int ToplamKiralanmaSuresi => KiralamaSureleri.Sum();
    }
}
