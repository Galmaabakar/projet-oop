using System;
using System.IO;
using Microsoft.Data.Sqlite;

namespace the_project
{
    // Base de donnees SQLite : un seul fichier (veritabani.db) a cote du .exe.
    // Il est cree automatiquement au premier lancement.
    // Pour repartir de zero, il suffit de supprimer ce fichier.
    public static class Veritabani
    {
        public const string DOSYA_ADI = "veritabani.db";

        // Modifiable (par exemple pour les tests).
        public static string DosyaYolu { get; set; } =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DOSYA_ADI);

        public static SqliteConnection BaglantiAc()
        {
            SqliteConnection baglanti = new SqliteConnection("Data Source=" + DosyaYolu);
            baglanti.Open();
            // SQLite ne verifie les cles etrangeres que si on le demande
            using (SqliteCommand komut = Komut(baglanti, "PRAGMA foreign_keys = ON;"))
            {
                komut.ExecuteNonQuery();
            }
            return baglanti;
        }

        // Cree une commande SQL. Le parametre islem (transaction) est
        // obligatoire si une transaction est en cours sur la connexion.
        public static SqliteCommand Komut(SqliteConnection baglanti, string sql, SqliteTransaction islem = null)
        {
            SqliteCommand komut = baglanti.CreateCommand();
            komut.CommandText = sql;
            komut.Transaction = islem;
            return komut;
        }

        private const string TABLOLAR = @"
CREATE TABLE IF NOT EXISTS hesaplar (
    no          TEXT PRIMARY KEY,
    tip         TEXT NOT NULL,
    sifre_tuzu  TEXT NOT NULL,
    sifre_ozeti TEXT NOT NULL
);
CREATE TABLE IF NOT EXISTS girisler (
    id       INTEGER PRIMARY KEY AUTOINCREMENT,
    no       TEXT NOT NULL,
    ad_soyad TEXT NOT NULL,
    tip      TEXT NOT NULL,
    zaman    TEXT NOT NULL
);
CREATE TABLE IF NOT EXISTS dersler (
    id    INTEGER PRIMARY KEY AUTOINCREMENT,
    ad    TEXT NOT NULL,
    no    TEXT NOT NULL,
    kredi INTEGER NOT NULL CHECK (kredi > 0)
);
CREATE TABLE IF NOT EXISTS ders_kayitlari (
    id               INTEGER PRIMARY KEY AUTOINCREMENT,
    ogrenci_no       TEXT NOT NULL,
    ogrenci_ad_soyad TEXT NOT NULL,
    ders_id          INTEGER NOT NULL REFERENCES dersler(id),
    UNIQUE (ogrenci_no, ders_id)
);
CREATE TABLE IF NOT EXISTS kitaplar (
    id          INTEGER PRIMARY KEY AUTOINCREMENT,
    ad          TEXT NOT NULL,
    yazar       TEXT NOT NULL,
    best_seller INTEGER NOT NULL
);
CREATE TABLE IF NOT EXISTS kitap_alimlari (
    id          INTEGER PRIMARY KEY AUTOINCREMENT,
    kitap_id    INTEGER NOT NULL REFERENCES kitaplar(id),
    personel_no TEXT NOT NULL,
    zaman       TEXT NOT NULL
);
CREATE TABLE IF NOT EXISTS urunler (
    id     INTEGER PRIMARY KEY AUTOINCREMENT,
    ad     TEXT NOT NULL,
    numara TEXT NOT NULL UNIQUE,
    stokta INTEGER NOT NULL
);
CREATE TABLE IF NOT EXISTS satislar (
    id          INTEGER PRIMARY KEY AUTOINCREMENT,
    personel_no TEXT NOT NULL,
    zaman       TEXT NOT NULL,
    toplam      INTEGER NOT NULL
);
CREATE TABLE IF NOT EXISTS satis_kalemleri (
    id       INTEGER PRIMARY KEY AUTOINCREMENT,
    satis_id INTEGER NOT NULL REFERENCES satislar(id),
    urun_id  INTEGER NOT NULL REFERENCES urunler(id),
    fiat     INTEGER NOT NULL
);
CREATE TABLE IF NOT EXISTS aletler (
    id         INTEGER PRIMARY KEY AUTOINCREMENT,
    ad         TEXT NOT NULL,
    numara     TEXT NOT NULL UNIQUE,
    odunc_alan TEXT NULL
);
CREATE TABLE IF NOT EXISTS alet_odunclari (
    id             INTEGER PRIMARY KEY AUTOINCREMENT,
    alet_id        INTEGER NOT NULL REFERENCES aletler(id),
    alan_kisi      TEXT NOT NULL,
    personel_no    TEXT NOT NULL,
    verilis_zamani TEXT NOT NULL,
    iade_zamani    TEXT NULL
);";

        // Cree les tables si besoin, puis les donnees de depart si la base est vide.
        public static void Hazirla()
        {
            using (SqliteConnection baglanti = BaglantiAc())
            using (SqliteTransaction islem = baglanti.BeginTransaction())
            {
                using (SqliteCommand komut = Komut(baglanti, TABLOLAR, islem))
                {
                    komut.ExecuteNonQuery();
                }

                if (TabloBosMu(baglanti, islem, "hesaplar")) OrnekHesaplariEkle(baglanti, islem);
                if (TabloBosMu(baglanti, islem, "dersler")) OrnekDersleriEkle(baglanti, islem);
                if (TabloBosMu(baglanti, islem, "kitaplar")) OrnekKitaplariEkle(baglanti, islem);
                if (TabloBosMu(baglanti, islem, "urunler")) OrnekUrunleriEkle(baglanti, islem);
                if (TabloBosMu(baglanti, islem, "aletler")) OrnekAletleriEkle(baglanti, islem);

                // Tout ou rien : si une etape echoue, rien n'est enregistre.
                islem.Commit();
            }
        }

        private static bool TabloBosMu(SqliteConnection baglanti, SqliteTransaction islem, string tablo)
        {
            // le nom de table vient de notre code, jamais de l'utilisateur
            using (SqliteCommand komut = Komut(baglanti, "SELECT COUNT(*) FROM " + tablo, islem))
            {
                return (long)komut.ExecuteScalar() == 0;
            }
        }

        // ------------------------------------------------------------------
        // Donnees de depart
        // ------------------------------------------------------------------

        private class OrnekHesap
        {
            public readonly string No, Tip, Tuz, Ozet;
            public OrnekHesap(string no, string tip, string tuz, string ozet)
            {
                No = no; Tip = tip; Tuz = tuz; Ozet = ozet;
            }
        }

        // Comptes de demonstration. Seules les empreintes sont ici :
        // les mots de passe a utiliser sont donnes dans le README.
        private static readonly OrnekHesap[] ornekHesaplar =
        {
            new OrnekHesap("1001", ogrenci.TIP, "Ltun5WKnNThoUt8o4z8BhA==", "n6fa4YfRGvK/2r2x+9d/2/yOa0nmvmXrBoaPgIDu4u4="),
            new OrnekHesap("1002", ogrenci.TIP, "DTkYj+JIoQpY6myFhquBTw==", "BNiMwpeoekWchH/Ma1g6EVt+oib0vnuTB8w+PYgRcZ0="),
            new OrnekHesap("2001", personel_kutuphaneclass.TIP, "VGnALtPX/kqP2kogVJK/Jg==", "IrSIVB1eq1Fvh/5SDjcMOY8+vuVbHAdkT8HTIgF2eRs="),
            new OrnekHesap("3001", reyonclass.TIP, "Ta0FEh8BDuynl+XKKmKDXQ==", "ncTZcbpP39v3+6lFe2xS3T7ijH6qPElRaPZNcpsuU+w="),
            new OrnekHesap("4001", personel_aletclass.TIP, "duzYWmBMjLch+0CHq3P5Vw==", "YOiYS0z6uexaMRdmvYou/AJBJ1Kd03cOyqbajM0WUnc="),
            new OrnekHesap("9001", yoneticiclass.TIP, "HgyL4D5Bt5mbmNYqqHJHqQ==", "R5REew4I5YpOToRqWqj4wCldcqqCA9NCU6cdGSi5smw=")
        };

        private static void OrnekHesaplariEkle(SqliteConnection baglanti, SqliteTransaction islem)
        {
            foreach (OrnekHesap h in ornekHesaplar)
            {
                using (SqliteCommand komut = Komut(baglanti,
                    "INSERT INTO hesaplar (no, tip, sifre_tuzu, sifre_ozeti) VALUES ($no, $tip, $tuz, $ozet)", islem))
                {
                    komut.Parameters.AddWithValue("$no", h.No);
                    komut.Parameters.AddWithValue("$tip", h.Tip);
                    komut.Parameters.AddWithValue("$tuz", h.Tuz);
                    komut.Parameters.AddWithValue("$ozet", h.Ozet);
                    komut.ExecuteNonQuery();
                }
            }
        }

        private static void OrnekDersleriEkle(SqliteConnection baglanti, SqliteTransaction islem)
        {
            object[,] dersler =
            {
                { "Nesne Yonelimli Programlama", "BIL201", 6 },
                { "Veri Yapilari", "BIL202", 6 },
                { "Veritabani Sistemleri", "BIL301", 5 },
                { "Matematik I", "MAT101", 6 },
                { "Fizik I", "FIZ101", 5 },
                { "Olasilik ve Istatistik", "MAT201", 4 },
                { "Ingilizce", "ING101", 3 },
                { "Bilgisayar Aglari", "BIL302", 5 },
                { "Isletim Sistemleri", "BIL303", 5 }
            };
            for (int i = 0; i < dersler.GetLength(0); i++)
            {
                using (SqliteCommand komut = Komut(baglanti,
                    "INSERT INTO dersler (ad, no, kredi) VALUES ($ad, $no, $kredi)", islem))
                {
                    komut.Parameters.AddWithValue("$ad", dersler[i, 0]);
                    komut.Parameters.AddWithValue("$no", dersler[i, 1]);
                    komut.Parameters.AddWithValue("$kredi", dersler[i, 2]);
                    komut.ExecuteNonQuery();
                }
            }
        }

        private static void OrnekKitaplariEkle(SqliteConnection baglanti, SqliteTransaction islem)
        {
            object[,] kitaplar =
            {
                { "Dusunceler", "Blaise Pascal", true },
                { "Savas Sanati", "Sun Tzu", true },
                { "Emile ya da Egitim Uzerine", "Jean-Jacques Rousseau", true },
                { "Bilimsel Zihniyet", "Gaston Bachelard", true },
                { "Kapital", "Karl Marx", true },
                { "Saf Aklin Elestirisi", "Immanuel Kant", true },
                { "Devlet (Politeia)", "Platon", true },
                { "Ikinci Cins", "Simone de Beauvoir", true },
                { "Nasil Dost Edinilir", "Dale Carnegie", false },
                { "Zengin Baba Yoksul Baba", "Robert Kiyosaki", false },
                { "Sinirsiz Guc", "Anthony Robbins", false },
                { "Pozitif Dusunmek", "James Allen", false }
            };
            for (int i = 0; i < kitaplar.GetLength(0); i++)
            {
                using (SqliteCommand komut = Komut(baglanti,
                    "INSERT INTO kitaplar (ad, yazar, best_seller) VALUES ($ad, $yazar, $bestSeller)", islem))
                {
                    komut.Parameters.AddWithValue("$ad", kitaplar[i, 0]);
                    komut.Parameters.AddWithValue("$yazar", kitaplar[i, 1]);
                    komut.Parameters.AddWithValue("$bestSeller", kitaplar[i, 2]);
                    komut.ExecuteNonQuery();
                }
            }
        }

        private static void OrnekUrunleriEkle(SqliteConnection baglanti, SqliteTransaction islem)
        {
            string[] stoktakiler =
            {
                "Macun - 37481", "Şampuan - 25830", "Ekmek - 49702", "Yumurta - 13276",
                "Süt - 78456", "Peynir - 96083", "Çay - 20457", "Bisküvi - 36412",
                "Kek - 29104", "Sosis - 84312", "Meyve - 68291", "Sebze - 47530",
                "Domates - 81652", "Patates - 10739", "Elma - 32509", "Portakal - 50283",
                "Limon - 41875", "Zeytin - 62547", "Bal - 79016", "Fındık - 24380",
                "Kavun - 95623", "Karpuz - 58342", "Makarna - 67291", "Pirinç - 13257",
                "Tuz - 30658", "Şeker - 84912", "Kahve - 46125", "Fasulye - 32704",
                "Biber - 29165", "Sarımsak - 80416"
            };
            // attendus dans une semaine
            string[] gelecekler = { "Yoğurt - 57204", "Tereyağı - 63158", "Un - 21876", "Mercimek - 39542" };

            foreach (string urun in stoktakiler) UrunEkle(baglanti, islem, urun, true);
            foreach (string urun in gelecekler) UrunEkle(baglanti, islem, urun, false);
        }

        private static void UrunEkle(SqliteConnection baglanti, SqliteTransaction islem, string adVeNumara, bool stokta)
        {
            string[] parcalar = adVeNumara.Split(new[] { " - " }, StringSplitOptions.None);
            using (SqliteCommand komut = Komut(baglanti,
                "INSERT INTO urunler (ad, numara, stokta) VALUES ($ad, $numara, $stokta)", islem))
            {
                komut.Parameters.AddWithValue("$ad", parcalar[0]);
                komut.Parameters.AddWithValue("$numara", parcalar[1]);
                komut.Parameters.AddWithValue("$stokta", stokta);
                komut.ExecuteNonQuery();
            }
        }

        private static void OrnekAletleriEkle(SqliteConnection baglanti, SqliteTransaction islem)
        {
            string[,] aletler =
            {
                { "Matkap", "50101" },
                { "Tornavida seti", "50102" },
                { "Çekiç", "50103" },
                { "Pense", "50104" },
                { "Multimetre", "50105" },
                { "Lehim makinesi", "50106" }
            };
            for (int i = 0; i < aletler.GetLength(0); i++)
            {
                using (SqliteCommand komut = Komut(baglanti,
                    "INSERT INTO aletler (ad, numara) VALUES ($ad, $numara)", islem))
                {
                    komut.Parameters.AddWithValue("$ad", aletler[i, 0]);
                    komut.Parameters.AddWithValue("$numara", aletler[i, 1]);
                    komut.ExecuteNonQuery();
                }
            }
        }
    }
}
