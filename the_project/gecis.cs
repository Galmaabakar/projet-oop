using System;
using System.Windows.Forms;

namespace the_project
{
    // Classe de base de tous les utilisateurs (ogrenci, personels et yonetici).
    // Tout ce qui est commun (numero, nom, connexion) est ici ;
    // chaque sous-classe ne definit que son type (kim) et son ecran.
    public abstract class gecis : giris
    {
        public const string GIRIS_BASARILI = "giris basari ile tamamlandi";
        public const string GIRIS_HATALI = "hatali giris";

        public string no { get; }
        public string ad { get; set; }
        public string soyad { get; set; }

        // Le type d'utilisateur : chaque sous-classe donne le sien.
        public abstract string kim { get; }

        public bool GirisBasarili { get; private set; }
        public string girisoldu => GirisBasarili ? GIRIS_BASARILI : GIRIS_HATALI;

        protected gecis(string no, string ad, string soyad)
        {
            this.no = (no ?? "").Trim();
            this.ad = (ad ?? "").Trim();
            this.soyad = (soyad ?? "").Trim();
        }

        // Verifie le numero, le mot de passe et le type dans le registre des comptes.
        public bool GirisYap(string sifre)
        {
            GirisBasarili = KullaniciDeposu.Dogrula(no, sifre, kim);
            return GirisBasarili;
        }

        // Polymorphisme : chaque type d'utilisateur ouvre son propre ecran.
        public abstract Form EkranOlustur();

        public override string ToString()
        {
            return $"{no} nolu, Tip: {kim}, {ad} adli, {soyad} soyadli";
        }

        // Liste des types proposes dans l'ecran de connexion.
        public static readonly string[] Tipler =
        {
            ogrenci.TIP,
            personel_kutuphaneclass.TIP,
            personel_aletclass.TIP,
            reyonclass.TIP,
            yoneticiclass.TIP
        };

        // Fabrique : cree le bon objet a partir du type choisi.
        public static gecis Olustur(string kim, string no, string ad, string soyad)
        {
            switch (kim)
            {
                case ogrenci.TIP: return new ogrenci(no, ad, soyad);
                case personel_kutuphaneclass.TIP: return new personel_kutuphaneclass(no, ad, soyad);
                case personel_aletclass.TIP: return new personel_aletclass(no, ad, soyad);
                case reyonclass.TIP: return new reyonclass(no, ad, soyad);
                case yoneticiclass.TIP: return new yoneticiclass(no, ad, soyad);
                default: throw new ArgumentException("bilinmeyen kullanici tipi: " + kim);
            }
        }
    }
}
