using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.Data.Sqlite;

namespace the_project
{
    // Les donnees de l'application.
    //  - Au demarrage, Yukle() lit tout depuis la base SQLite.
    //  - Chaque methode ...Ekle / ...Kaydet ecrit d'abord dans la base,
    //    puis met a jour la liste en memoire.
    // Ce sont des BindingList : les ListBox/ComboBox qui les affichent
    // se mettent a jour toutes seules.
    public static class Veriler
    {
        public static readonly BindingList<GirisKaydi> Girisler = new BindingList<GirisKaydi>();
        public static readonly BindingList<Ders> Dersler = new BindingList<Ders>();
        public static readonly BindingList<DersKaydi> DersKayitlari = new BindingList<DersKaydi>();
        public static readonly BindingList<Kitap> Kitaplar = new BindingList<Kitap>();
        public static readonly BindingList<KitapAlimi> KitapAlimlari = new BindingList<KitapAlimi>();
        public static readonly BindingList<Urun> Urunler = new BindingList<Urun>();
        public static readonly BindingList<SatisKalemi> SatilanUrunler = new BindingList<SatisKalemi>();
        public static readonly BindingList<Satis> Satislar = new BindingList<Satis>();
        public static readonly BindingList<Alet> Aletler = new BindingList<Alet>();
        public static readonly BindingList<AletOdunc> AletOdunclari = new BindingList<AletOdunc>();

        public static Urun UrunBul(string ad, string numara)
        {
            return Urunler.FirstOrDefault(u => u.Eslesir(ad, numara));
        }

        // ------------------------------------------------------------------
        // Lecture : remplit toutes les listes depuis la base
        // ------------------------------------------------------------------

        public static void Yukle()
        {
            foreach (var liste in new IBindingList[] { Girisler, Dersler, DersKayitlari, Kitaplar, KitapAlimlari,
                                                       Urunler, SatilanUrunler, Satislar, Aletler, AletOdunclari })
            {
                liste.Clear();
            }

            using (SqliteConnection b = Veritabani.BaglantiAc())
            {
                Oku(b, "SELECT no, ad_soyad, tip, zaman FROM girisler ORDER BY id", r =>
                    Girisler.Add(new GirisKaydi(r.GetString(0), r.GetString(1), r.GetString(2), r.GetDateTime(3))));

                Oku(b, "SELECT id, ad, no, kredi FROM dersler ORDER BY id", r =>
                    Dersler.Add(new Ders(r.GetString(1), r.GetString(2), r.GetInt32(3)) { Id = r.GetInt64(0) }));
                Dictionary<long, Ders> dersler = Dersler.ToDictionary(d => d.Id);
                Oku(b, "SELECT ogrenci_no, ogrenci_ad_soyad, ders_id FROM ders_kayitlari ORDER BY id", r =>
                    DersKayitlari.Add(new DersKaydi(r.GetString(0), r.GetString(1), dersler[r.GetInt64(2)])));

                Oku(b, "SELECT id, ad, yazar, best_seller FROM kitaplar ORDER BY id", r =>
                    Kitaplar.Add(new Kitap(r.GetString(1), r.GetString(2), r.GetBoolean(3)) { Id = r.GetInt64(0) }));
                Dictionary<long, Kitap> kitaplar = Kitaplar.ToDictionary(k => k.Id);
                Oku(b, "SELECT kitap_id, personel_no, zaman FROM kitap_alimlari ORDER BY id", r =>
                    KitapAlimlari.Add(new KitapAlimi(kitaplar[r.GetInt64(0)], r.GetString(1), r.GetDateTime(2))));

                Oku(b, "SELECT id, ad, numara, stokta FROM urunler ORDER BY id", r =>
                    Urunler.Add(new Urun(r.GetString(1), r.GetString(2), r.GetBoolean(3)) { Id = r.GetInt64(0) }));
                Dictionary<long, Urun> urunler = Urunler.ToDictionary(u => u.Id);
                // lignes de ticket regroupees par vente
                var kalemler = new Dictionary<long, List<SatisKalemi>>();
                Oku(b, "SELECT satis_id, urun_id, fiat FROM satis_kalemleri ORDER BY id", r =>
                {
                    SatisKalemi kalem = new SatisKalemi(urunler[r.GetInt64(1)], r.GetInt32(2));
                    long satisId = r.GetInt64(0);
                    if (!kalemler.ContainsKey(satisId)) kalemler[satisId] = new List<SatisKalemi>();
                    kalemler[satisId].Add(kalem);
                    SatilanUrunler.Add(kalem);
                });
                Oku(b, "SELECT id, personel_no, zaman FROM satislar ORDER BY id", r =>
                {
                    long id = r.GetInt64(0);
                    List<SatisKalemi> satisKalemleri = kalemler.ContainsKey(id) ? kalemler[id] : new List<SatisKalemi>();
                    Satislar.Add(new Satis(satisKalemleri, r.GetString(1), r.GetDateTime(2)) { Id = id });
                });

                Oku(b, "SELECT id, ad, numara, odunc_alan FROM aletler ORDER BY id", r =>
                    Aletler.Add(new Alet(r.GetString(1), r.GetString(2))
                    {
                        Id = r.GetInt64(0),
                        OduncAlan = r.IsDBNull(3) ? null : r.GetString(3)
                    }));
                Dictionary<long, Alet> aletler = Aletler.ToDictionary(a => a.Id);
                Oku(b, "SELECT id, alet_id, alan_kisi, personel_no, verilis_zamani, iade_zamani FROM alet_odunclari ORDER BY id", r =>
                    AletOdunclari.Add(new AletOdunc(aletler[r.GetInt64(1)], r.GetString(2), r.GetString(3), r.GetDateTime(4),
                                                    r.IsDBNull(5) ? (DateTime?)null : r.GetDateTime(5))
                    { Id = r.GetInt64(0) }));
            }
        }

        // Execute un SELECT et appelle "satirIsle" pour chaque ligne.
        private static void Oku(SqliteConnection baglanti, string sql, Action<SqliteDataReader> satirIsle)
        {
            using (SqliteCommand komut = Veritabani.Komut(baglanti, sql))
            using (SqliteDataReader okuyucu = komut.ExecuteReader())
            {
                while (okuyucu.Read()) satirIsle(okuyucu);
            }
        }

        // Execute un INSERT et renvoie l'id cree par SQLite.
        private static long EkleVeIdAl(SqliteCommand komut)
        {
            komut.CommandText += "; SELECT last_insert_rowid();";
            return (long)komut.ExecuteScalar();
        }

        // ------------------------------------------------------------------
        // Ecriture
        // ------------------------------------------------------------------

        public static void GirisKaydet(GirisKaydi kayit)
        {
            using (SqliteConnection b = Veritabani.BaglantiAc())
            using (SqliteCommand komut = Veritabani.Komut(b,
                "INSERT INTO girisler (no, ad_soyad, tip, zaman) VALUES ($no, $adSoyad, $tip, $zaman)"))
            {
                komut.Parameters.AddWithValue("$no", kayit.No);
                komut.Parameters.AddWithValue("$adSoyad", kayit.AdSoyad);
                komut.Parameters.AddWithValue("$tip", kayit.Tip);
                komut.Parameters.AddWithValue("$zaman", kayit.Zaman);
                komut.ExecuteNonQuery();
            }
            Girisler.Add(kayit);
        }

        public static void DersEkle(Ders ders)
        {
            using (SqliteConnection b = Veritabani.BaglantiAc())
            using (SqliteCommand komut = Veritabani.Komut(b,
                "INSERT INTO dersler (ad, no, kredi) VALUES ($ad, $no, $kredi)"))
            {
                komut.Parameters.AddWithValue("$ad", ders.Ad);
                komut.Parameters.AddWithValue("$no", ders.No);
                komut.Parameters.AddWithValue("$kredi", ders.Kredi);
                ders.Id = EkleVeIdAl(komut);
            }
            Dersler.Add(ders);
        }

        public static void DersKaydiEkle(DersKaydi kayit)
        {
            using (SqliteConnection b = Veritabani.BaglantiAc())
            using (SqliteCommand komut = Veritabani.Komut(b,
                "INSERT INTO ders_kayitlari (ogrenci_no, ogrenci_ad_soyad, ders_id) VALUES ($no, $adSoyad, $dersId)"))
            {
                komut.Parameters.AddWithValue("$no", kayit.OgrenciNo);
                komut.Parameters.AddWithValue("$adSoyad", kayit.OgrenciAdSoyad);
                komut.Parameters.AddWithValue("$dersId", kayit.Ders.Id);
                komut.ExecuteNonQuery();
            }
            DersKayitlari.Add(kayit);
        }

        public static void KitapEkle(Kitap kitap)
        {
            using (SqliteConnection b = Veritabani.BaglantiAc())
            using (SqliteCommand komut = Veritabani.Komut(b,
                "INSERT INTO kitaplar (ad, yazar, best_seller) VALUES ($ad, $yazar, $bestSeller)"))
            {
                komut.Parameters.AddWithValue("$ad", kitap.Ad);
                komut.Parameters.AddWithValue("$yazar", kitap.Yazar);
                komut.Parameters.AddWithValue("$bestSeller", kitap.BestSeller);
                kitap.Id = EkleVeIdAl(komut);
            }
            Kitaplar.Add(kitap);
        }

        public static void KitapAlimiEkle(KitapAlimi alim)
        {
            using (SqliteConnection b = Veritabani.BaglantiAc())
            using (SqliteCommand komut = Veritabani.Komut(b,
                "INSERT INTO kitap_alimlari (kitap_id, personel_no, zaman) VALUES ($kitapId, $personelNo, $zaman)"))
            {
                komut.Parameters.AddWithValue("$kitapId", alim.Kitap.Id);
                komut.Parameters.AddWithValue("$personelNo", alim.PersonelNo);
                komut.Parameters.AddWithValue("$zaman", alim.Zaman);
                komut.ExecuteNonQuery();
            }
            KitapAlimlari.Add(alim);
        }

        // Une vente = une ligne dans "satislar" + une ligne par produit.
        // La transaction garantit que tout est enregistre, ou rien.
        public static void SatisEkle(Satis satis)
        {
            using (SqliteConnection b = Veritabani.BaglantiAc())
            using (SqliteTransaction islem = b.BeginTransaction())
            {
                using (SqliteCommand komut = Veritabani.Komut(b,
                    "INSERT INTO satislar (personel_no, zaman, toplam) VALUES ($personelNo, $zaman, $toplam)", islem))
                {
                    komut.Parameters.AddWithValue("$personelNo", satis.PersonelNo);
                    komut.Parameters.AddWithValue("$zaman", satis.Zaman);
                    komut.Parameters.AddWithValue("$toplam", satis.Toplam);
                    satis.Id = EkleVeIdAl(komut);
                }
                foreach (SatisKalemi kalem in satis.Kalemler)
                {
                    using (SqliteCommand komut = Veritabani.Komut(b,
                        "INSERT INTO satis_kalemleri (satis_id, urun_id, fiat) VALUES ($satisId, $urunId, $fiat)", islem))
                    {
                        komut.Parameters.AddWithValue("$satisId", satis.Id);
                        komut.Parameters.AddWithValue("$urunId", kalem.Urun.Id);
                        komut.Parameters.AddWithValue("$fiat", kalem.Fiat);
                        komut.ExecuteNonQuery();
                    }
                }
                islem.Commit();
            }
            foreach (SatisKalemi kalem in satis.Kalemler) SatilanUrunler.Add(kalem);
            Satislar.Add(satis);
        }

        public static void AletOduncKaydet(AletOdunc odunc)
        {
            using (SqliteConnection b = Veritabani.BaglantiAc())
            using (SqliteTransaction islem = b.BeginTransaction())
            {
                using (SqliteCommand komut = Veritabani.Komut(b,
                    "UPDATE aletler SET odunc_alan = $alanKisi WHERE id = $aletId", islem))
                {
                    komut.Parameters.AddWithValue("$alanKisi", odunc.AlanKisi);
                    komut.Parameters.AddWithValue("$aletId", odunc.Alet.Id);
                    komut.ExecuteNonQuery();
                }
                using (SqliteCommand komut = Veritabani.Komut(b,
                    "INSERT INTO alet_odunclari (alet_id, alan_kisi, personel_no, verilis_zamani) " +
                    "VALUES ($aletId, $alanKisi, $personelNo, $zaman)", islem))
                {
                    komut.Parameters.AddWithValue("$aletId", odunc.Alet.Id);
                    komut.Parameters.AddWithValue("$alanKisi", odunc.AlanKisi);
                    komut.Parameters.AddWithValue("$personelNo", odunc.PersonelNo);
                    komut.Parameters.AddWithValue("$zaman", odunc.VerilisZamani);
                    odunc.Id = EkleVeIdAl(komut);
                }
                islem.Commit();
            }
            odunc.Alet.OduncAlan = odunc.AlanKisi;
            AletOdunclari.Add(odunc);
        }

        public static void AletIadeKaydet(AletOdunc odunc)
        {
            DateTime simdi = DateTime.Now;
            using (SqliteConnection b = Veritabani.BaglantiAc())
            using (SqliteTransaction islem = b.BeginTransaction())
            {
                using (SqliteCommand komut = Veritabani.Komut(b,
                    "UPDATE alet_odunclari SET iade_zamani = $zaman WHERE id = $id", islem))
                {
                    komut.Parameters.AddWithValue("$zaman", simdi);
                    komut.Parameters.AddWithValue("$id", odunc.Id);
                    komut.ExecuteNonQuery();
                }
                using (SqliteCommand komut = Veritabani.Komut(b,
                    "UPDATE aletler SET odunc_alan = NULL WHERE id = $aletId", islem))
                {
                    komut.Parameters.AddWithValue("$aletId", odunc.Alet.Id);
                    komut.ExecuteNonQuery();
                }
                islem.Commit();
            }
            odunc.IadeEt(simdi);
            odunc.Alet.OduncAlan = null;
            // la ligne a change : on previent les listes affichees
            AletOdunclari.ResetItem(AletOdunclari.IndexOf(odunc));
        }
    }
}
