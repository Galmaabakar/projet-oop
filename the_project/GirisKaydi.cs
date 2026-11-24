using System;

namespace the_project
{
    // Une connexion reussie (affichee dans l'ecran yonetici).
    public class GirisKaydi
    {
        public string No { get; }
        public string AdSoyad { get; }
        public string Tip { get; }
        public DateTime Zaman { get; }

        public GirisKaydi(gecis kullanici)
        {
            No = kullanici.no;
            AdSoyad = kullanici.ad + " " + kullanici.soyad;
            Tip = kullanici.kim;
            Zaman = DateTime.Now;
        }

        public override string ToString()
        {
            return $"{Zaman:HH:mm} {No} {AdSoyad} ({Tip})";
        }
    }
}
