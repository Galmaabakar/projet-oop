using System;
using System.Linq;
using System.Windows.Forms;

namespace the_project
{
    public partial class personel_kutuphane : Form
    {
        private readonly personel_kutuphaneclass personel;

        public personel_kutuphane(personel_kutuphaneclass personel)
        {
            InitializeComponent();
            this.personel = personel;
            Text = $"Kutuphane - {personel.ad} {personel.soyad}";

            comboBox1.Items.AddRange(Veriler.Kitaplar.Where(k => k.BestSeller).ToArray<object>());
            comboBox2.Items.AddRange(Veriler.Kitaplar.Where(k => !k.BestSeller).ToArray<object>());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string mesaj;
            Kitap kitap = personel.KitapEkle(textBox1.Text, textBox2.Text, out mesaj);
            if (kitap != null)
            {
                comboBox2.Items.Add(kitap);
                comboBox2.SelectedItem = kitap;
                textBox1.Clear();
                textBox2.Clear();
            }
            MessageBox.Show(mesaj);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Kitap bestSeller = comboBox1.SelectedItem as Kitap;
            Kitap diger = comboBox2.SelectedItem as Kitap;
            if (bestSeller == null && diger == null)
            {
                MessageBox.Show("lutfen bir kitap seciniz");
                return;
            }
            if (bestSeller != null) personel.KitapVer(bestSeller);
            if (diger != null) personel.KitapVer(diger);

            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            MessageBox.Show("Kitabiniz basari ile alindi");
        }
    }
}
