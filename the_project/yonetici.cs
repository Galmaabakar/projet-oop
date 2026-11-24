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
