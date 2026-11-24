using System.Windows.Forms;

namespace the_project
{
    // L'administrateur se connecte comme les autres utilisateurs
    // (numero + mot de passe) avant de voir l'ecran yonetici.
    public class yoneticiclass : gecis
    {
        public const string TIP = "yonetici";

        public yoneticiclass(string no, string ad, string soyad) : base(no, ad, soyad)
        {
        }

        public override string kim => TIP;

        public override Form EkranOlustur()
        {
            return new yonetici();
        }
    }
}
