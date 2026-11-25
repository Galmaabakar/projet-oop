using System;
using System.Security.Cryptography;

namespace the_project
{
    // Hachage des mots de passe avec PBKDF2-SHA256 (integre a .NET).
    //
    // On n'enregistre JAMAIS le mot de passe lui-meme, seulement :
    //  - un "sel" (tuz) : des octets aleatoires, differents pour chaque compte ;
    //  - une "empreinte" (ozet) calculee a partir du mot de passe et du sel.
    // L'empreinte ne permet pas de retrouver le mot de passe. A la connexion,
    // on recalcule l'empreinte de ce que la personne a tape et on compare.
    public static class SifreGuvenligi
    {
        private const int TUZ_BOYUTU = 16;          // octets
        private const int OZET_BOYUTU = 32;         // octets
        private const int TEKRAR_SAYISI = 100000;   // ralentit les essais en serie

        public static void Olustur(string sifre, out string tuz, out string ozet)
        {
            byte[] tuzBaytlari = new byte[TUZ_BOYUTU];
            using (RandomNumberGenerator rastgele = RandomNumberGenerator.Create())
            {
                rastgele.GetBytes(tuzBaytlari);
            }
            tuz = Convert.ToBase64String(tuzBaytlari);
            ozet = Convert.ToBase64String(OzetHesapla(sifre, tuzBaytlari));
        }

        public static bool Dogrula(string sifre, string tuz, string ozet)
        {
            byte[] beklenen = Convert.FromBase64String(ozet);
            byte[] hesaplanan = OzetHesapla(sifre, Convert.FromBase64String(tuz));

            // On compare tous les octets, meme apres une difference,
            // pour que le temps de reponse ne donne pas d'indice.
            int fark = beklenen.Length ^ hesaplanan.Length;
            for (int i = 0; i < Math.Min(beklenen.Length, hesaplanan.Length); i++)
            {
                fark |= beklenen[i] ^ hesaplanan[i];
            }
            return fark == 0;
        }

        private static byte[] OzetHesapla(string sifre, byte[] tuz)
        {
            using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(sifre, tuz, TEKRAR_SAYISI, HashAlgorithmName.SHA256))
            {
                return pbkdf2.GetBytes(OZET_BOYUTU);
            }
        }
    }
}
