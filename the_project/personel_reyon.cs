using System;
using System.Linq;
using System.Windows.Forms;

namespace the_project
{
    public partial class personel_reyon : Form
    {
        private readonly reyonclass personel;
        private readonly Sepet sepet = new Sepet();

        public personel_reyon(reyonclass personel)
        {
            InitializeComponent();
            this.personel = personel;

            comboBox1.Items.AddRange(Veriler.Urunler.Where(u => u.Stokta).ToArray<object>());
            comboBox2.Items.AddRange(Veriler.Urunler.Where(u => !u.Stokta).ToArray<object>());
            BasligiGuncelle();
        }

        private void BasligiGuncelle()
        {
            Text = $"Reyon - {personel.ad} {personel.soyad} | sepet: {sepet.Kalemler.Count} urun, {sepet.Toplam} TL";
        }

        // Choisir un produit dans la liste remplit son nom et son numero.
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Urun urun = comboBox1.SelectedItem as Urun;
            if (urun == null) return;
            textBox1.Text = urun.Ad;
            textBox2.Text = urun.Numara;
            textBox3.Focus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Urun urun = Veriler.UrunBul(textBox1.Text, textBox2.Text);
            if (urun == null)
            {
                MessageBox.Show("istediginiz urun bizde yoktur");
                return;
            }
            if (!urun.Stokta)
            {
                MessageBox.Show("urunuz stokta yoktur ,bir hafta icinde gelecek");
                return;
            }
            int fiat;
            if (!int.TryParse(textBox3.Text, out fiat) || fiat < 0)
            {
                MessageBox.Show("gecerli bir fiat giriniz");
                return;
            }

            sepet.Ekle(urun, fiat);
            textBox3.Clear();
            BasligiGuncelle();
            MessageBox.Show("urununuz alindi");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Satis satis = personel.SatisYap(sepet);
            if (satis == null)
            {
                MessageBox.Show("henuz urun alinmadi");
                return;
            }
            BasligiGuncelle();
            MessageBox.Show($"{satis.Kalemler.Count} urun, toplam: {satis.Toplam} TL", "Toplam fiat");
        }

        private void personel_reyon_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sepet.Bos) return;
            DialogResult cevap = MessageBox.Show("Sepette odenmemis urunler var. Yine de cikilsin mi?",
                "Reyon", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (cevap == DialogResult.No) e.Cancel = true;
        }
    }
}
