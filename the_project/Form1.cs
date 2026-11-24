using System;
using System.Windows.Forms;

namespace the_project
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(gecis.Tipler);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string kim = comboBox1.SelectedItem as string;
            string ad = textBox1.Text.Trim();
            string soyad = textBox2.Text.Trim();
            string no = textBox3.Text.Trim();

            if (string.IsNullOrEmpty(kim))
            {
                MessageBox.Show("Lutfen bir giris tipi seciniz");
                return;
            }
            if (ad == "" || soyad == "")
            {
                MessageBox.Show("Ad ve soyad bos olamaz");
                return;
            }
            if (!KullaniciDeposu.NumaraGecerliMi(no))
            {
                MessageBox.Show("Numara sadece rakamlardan olusmalidir");
                return;
            }

            // La fabrique cree le bon type (ogrenci, reyonclass, ...) :
            // plus besoin d'un if/else par type d'utilisateur.
            gecis kullanici = gecis.Olustur(kim, no, ad, soyad);
            kullanici.GirisYap(textBox4.Text);
            textBox4.Clear();
            label5.Text = kullanici.girisoldu;

            if (!kullanici.GirisBasarili)
            {
                return;
            }

            Veriler.Girisler.Add(new GirisKaydi(kullanici));
            MessageBox.Show(kullanici + " giris yapti");
            using (Form ekran = kullanici.EkranOlustur())
            {
                ekran.ShowDialog(this);
            }
        }

        // Le yonetici doit maintenant se connecter avec son numero et son mot de passe.
        private void button2_Click(object sender, EventArgs e)
        {
            comboBox1.SelectedItem = yoneticiclass.TIP;
            label5.Text = "Yonetici numarasi ve sifresini giriniz";
            textBox3.Focus();
        }
    }
}
