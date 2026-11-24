using System.Collections.Generic;
using System.Linq;

namespace the_project
{
    // Registre des comptes : un numero appartient a un seul type
    // d'utilisateur et a son propre mot de passe.
    public static class KullaniciDeposu
    {
        private class Hesap
        {
            public string No { get; }
            public string Sifre { get; }
            public string Tip { get; }

            public Hesap(string no, string sifre, string tip)
            {
                No = no;
                Sifre = sifre;
                Tip = tip;
            }
        }

        private static readonly List<Hesap> hesaplar = new List<Hesap>
        {
            new Hesap("1001", "12345678", ogrenci.TIP),
            new Hesap("1002", "87654321", ogrenci.TIP),
            new Hesap("2001", "1234567", personel_kutuphaneclass.TIP),
            new Hesap("3001", "123456", reyonclass.TIP),
            new Hesap("4001", "12345", personel_aletclass.TIP)
        };

        // Un numero doit etre non vide et ne contenir que des chiffres.
        public static bool NumaraGecerliMi(string no)
        {
            return !string.IsNullOrEmpty(no) && no.All(char.IsDigit);
        }

        public static bool Dogrula(string no, string sifre, string tip)
        {
            if (!NumaraGecerliMi(no)) return false;
            Hesap hesap = hesaplar.FirstOrDefault(h => h.No == no);
            return hesap != null && hesap.Tip == tip && hesap.Sifre == sifre;
        }
    }
}
