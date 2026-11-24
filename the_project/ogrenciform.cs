using System;
using System.Linq;
using System.Windows.Forms;

namespace the_project
{
    public partial class ogrenciform : Form
    {
        private readonly ogrenci ogrenci;

        public ogrenciform(ogrenci ogrenci)
        {
            InitializeComponent();
            this.ogrenci = ogrenci;

            // les infos de l'etudiant viennent de la connexion
            textBox8.Text = ogrenci.ad;
            textBox7.Text = ogrenci.soyad;
            textBox6.Text = ogrenci.no;

            // le catalogue est partage : un cours ajoute ici apparait pour tous
            comboBox1.DataSource = Veriler.Dersler;
            BasligiGuncelle();
        }

        // Le credit est celui du cours choisi (plus besoin de le taper).
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Ders ders = comboBox1.SelectedItem as Ders;
            textBox5.Text = ders == null ? "" : ders.Kredi.ToString();
        }

        private void BasligiGuncelle()
        {
            Text = $"Ders secimi - {ogrenci.ad} {ogrenci.soyad} ({ogrenci.ToplamKredi}/{ogrenci.KREDI_LIMITI} kredi)";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //ogrenci ders listesine ders eklemesi icin
            string ad = textBox1.Text.Trim();
            string no = textBox3.Text.Trim();
            int kredi;
            if (ad == "")
            {
                MessageBox.Show("ders adi bos olamaz");
                return;
            }
            if (!int.TryParse(textBox4.Text, out kredi) || kredi <= 0)
            {
                MessageBox.Show("gecerli bir kredi giriniz");
                return;
            }
            if (Veriler.Dersler.Any(d => string.Equals(d.Ad, ad, StringComparison.CurrentCultureIgnoreCase)
                                      || (no != "" && d.No == no)))
            {
                MessageBox.Show("bu ders zaten listede");
                return;
            }

            Ders ders = new Ders(ad, no, kredi);
            Veriler.Dersler.Add(ders);
            comboBox1.SelectedItem = ders;
            textBox1.Clear();
            textBox3.Clear();
            textBox4.Clear();
            MessageBox.Show("ders basari ile eklendi");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //ogrenci ders secimi icin
            string mesaj;
            ogrenci.DersAl(comboBox1.SelectedItem as Ders, out mesaj);
            MessageBox.Show(mesaj);
            BasligiGuncelle();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // recapitulatif des cours choisis, puis fermeture
            var dersler = ogrenci.AldigiDersler.ToList();
            if (dersler.Count == 0)
            {
                MessageBox.Show("henuz ders secmediniz");
                return;
            }
            string liste = string.Join(Environment.NewLine, dersler.Select(k => $"- {k.Ders.Ad} ({k.Ders.Kredi} kredi)"));
            MessageBox.Show($"Sectiginiz dersler:{Environment.NewLine}{liste}{Environment.NewLine}{Environment.NewLine}Toplam: {ogrenci.ToplamKredi}/{ogrenci.KREDI_LIMITI} kredi",
                "basari ile kaydedildi");
            Close();
        }
    }
}
