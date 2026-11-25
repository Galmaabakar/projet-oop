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
            : this(kullanici.no, kullanici.ad + " " + kullanici.soyad, kullanici.kim, DateTime.Now)
        {
        }

        // utilise au chargement depuis la base
        public GirisKaydi(string no, string adSoyad, string tip, DateTime zaman)
        {
            No = no;
            AdSoyad = adSoyad;
            Tip = tip;
            Zaman = zaman;
        }

        public override string ToString()
        {
            return $"{Zaman:dd.MM HH:mm} {No} {AdSoyad} ({Tip})";
        }
    }
}
