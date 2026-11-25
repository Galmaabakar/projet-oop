using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace the_project
{
    public partial class yonetici : Form
    {
        public yonetici()
        {
            InitializeComponent();
            foreach (ListBox kutu in new[] { listBox1, listBox2, listBox3, listBox4, listBox5, listBox6 })
            {
                kutu.HorizontalScrollbar = true;
            }
            hesapTipComboBox.Items.AddRange(gecis.Tipler);
            HesaplariGoster();
        }

        // ---- Gestion des comptes (les mots de passe ne sont jamais affiches)

        private void HesaplariGoster()
        {
            hesaplarListBox.DataSource = KullaniciDeposu.Hesaplar();
            hesaplarListBox.SelectedIndex = -1;
        }

        // Cliquer sur un compte remplit son numero et son type.
        private void hesaplarListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string satir = hesaplarListBox.SelectedItem as string;
            if (satir == null) return;
            string[] parcalar = satir.Split(new[] { " - " }, StringSplitOptions.None);
            hesapNoTextBox.Text = parcalar[0];
            hesapTipComboBox.SelectedItem = parcalar[1];
        }

        private void hesapEkleButton_Click(object sender, EventArgs e)
        {
            string mesaj;
            if (KullaniciDeposu.HesapEkle(hesapNoTextBox.Text, hesapTipComboBox.SelectedItem as string,
                                          hesapSifreTextBox.Text, out mesaj))
            {
                HesaplariGoster();
            }
            hesapSifreTextBox.Clear();
            MessageBox.Show(mesaj);
        }

        private void sifreDegistirButton_Click(object sender, EventArgs e)
        {
            string mesaj;
            KullaniciDeposu.SifreDegistir(hesapNoTextBox.Text, hesapSifreTextBox.Text, out mesaj);
            hesapSifreTextBox.Clear();
            MessageBox.Show(mesaj);
        }

        // Les listes sont des BindingList : une fois liee, la ListBox
        // se met a jour toute seule quand une donnee est ajoutee.
        private static void Goster<T>(ListBox kutu, BindingList<T> veri)
        {
            kutu.DataSource = null;
            kutu.DataSource = veri;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Goster(listBox1, Veriler.Girisler);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Goster(listBox2, Veriler.DersKayitlari);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Goster(listBox3, Veriler.KitapAlimlari);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Goster(listBox4, Veriler.SatilanUrunler);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Goster(listBox5, Veriler.Satislar);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Goster(listBox6, Veriler.AletOdunclari);
        }
    }
}
