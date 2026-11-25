using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Sqlite;

namespace the_project
{
    // Les comptes sont dans la table "hesaplar" de la base SQLite.
    // Un numero appartient a un seul type d'utilisateur ; on ne garde
    // que le sel et l'empreinte du mot de passe (voir SifreGuvenligi).
    public static class KullaniciDeposu
    {
        public const int EN_AZ_SIFRE_UZUNLUGU = 5;

        // Un numero doit etre non vide et ne contenir que des chiffres.
        public static bool NumaraGecerliMi(string no)
        {
            return !string.IsNullOrEmpty(no) && no.All(char.IsDigit);
        }

        public static bool Dogrula(string no, string sifre, string tip)
        {
            if (!NumaraGecerliMi(no) || string.IsNullOrEmpty(sifre)) return false;

            using (SqliteConnection baglanti = Veritabani.BaglantiAc())
            using (SqliteCommand komut = Veritabani.Komut(baglanti,
                "SELECT tip, sifre_tuzu, sifre_ozeti FROM hesaplar WHERE no = $no"))
            {
                // Toujours des parametres ($no), jamais de concatenation :
                // cela protege contre l'injection SQL.
                komut.Parameters.AddWithValue("$no", no);
                using (SqliteDataReader okuyucu = komut.ExecuteReader())
                {
                    if (!okuyucu.Read()) return false;   // numero inconnu
                    return okuyucu.GetString(0) == tip
                        && SifreGuvenligi.Dogrula(sifre, okuyucu.GetString(1), okuyucu.GetString(2));
                }
            }
        }

        public static bool HesapVarMi(string no)
        {
            using (SqliteConnection baglanti = Veritabani.BaglantiAc())
            using (SqliteCommand komut = Veritabani.Komut(baglanti, "SELECT COUNT(*) FROM hesaplar WHERE no = $no"))
            {
                komut.Parameters.AddWithValue("$no", no);
                return (long)komut.ExecuteScalar() > 0;
            }
        }

        // Liste "numero - type" pour l'ecran yonetici (sans les mots de passe).
        public static List<string> Hesaplar()
        {
            List<string> liste = new List<string>();
            using (SqliteConnection baglanti = Veritabani.BaglantiAc())
            using (SqliteCommand komut = Veritabani.Komut(baglanti, "SELECT no, tip FROM hesaplar ORDER BY no"))
            using (SqliteDataReader okuyucu = komut.ExecuteReader())
            {
                while (okuyucu.Read())
                {
                    liste.Add(okuyucu.GetString(0) + " - " + okuyucu.GetString(1));
                }
            }
            return liste;
        }

        public static bool HesapEkle(string no, string tip, string sifre, out string mesaj)
        {
            no = (no ?? "").Trim();
            if (!NumaraGecerliMi(no))
            {
                mesaj = "numara sadece rakamlardan olusmalidir";
                return false;
            }
            if (!gecis.Tipler.Contains(tip))
            {
                mesaj = "bir hesap tipi seciniz";
                return false;
            }
            if (!SifreGecerliMi(sifre, out mesaj)) return false;
            if (HesapVarMi(no))
            {
                mesaj = $"{no} numarasi zaten kullaniliyor";
                return false;
            }

            string tuz, ozet;
            SifreGuvenligi.Olustur(sifre, out tuz, out ozet);
            using (SqliteConnection baglanti = Veritabani.BaglantiAc())
            using (SqliteCommand komut = Veritabani.Komut(baglanti,
                "INSERT INTO hesaplar (no, tip, sifre_tuzu, sifre_ozeti) VALUES ($no, $tip, $tuz, $ozet)"))
            {
                komut.Parameters.AddWithValue("$no", no);
                komut.Parameters.AddWithValue("$tip", tip);
                komut.Parameters.AddWithValue("$tuz", tuz);
                komut.Parameters.AddWithValue("$ozet", ozet);
                komut.ExecuteNonQuery();
            }
            mesaj = $"{no} numarali {tip} hesabi olusturuldu";
            return true;
        }

        public static bool SifreDegistir(string no, string yeniSifre, out string mesaj)
        {
            no = (no ?? "").Trim();
            if (!SifreGecerliMi(yeniSifre, out mesaj)) return false;

            // nouveau sel a chaque changement de mot de passe
            string tuz, ozet;
            SifreGuvenligi.Olustur(yeniSifre, out tuz, out ozet);
            using (SqliteConnection baglanti = Veritabani.BaglantiAc())
            using (SqliteCommand komut = Veritabani.Komut(baglanti,
                "UPDATE hesaplar SET sifre_tuzu = $tuz, sifre_ozeti = $ozet WHERE no = $no"))
            {
                komut.Parameters.AddWithValue("$no", no);
                komut.Parameters.AddWithValue("$tuz", tuz);
                komut.Parameters.AddWithValue("$ozet", ozet);
                if (komut.ExecuteNonQuery() == 0)
                {
                    mesaj = $"{no} numarali hesap bulunamadi";
                    return false;
                }
            }
            mesaj = $"{no} numarali hesabin sifresi degistirildi";
            return true;
        }

        private static bool SifreGecerliMi(string sifre, out string mesaj)
        {
            if (string.IsNullOrEmpty(sifre) || sifre.Length < EN_AZ_SIFRE_UZUNLUGU)
            {
                mesaj = $"sifre en az {EN_AZ_SIFRE_UZUNLUGU} karakter olmali";
                return false;
            }
            mesaj = "";
            return true;
        }
    }
}
