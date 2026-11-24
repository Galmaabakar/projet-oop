using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace the_project
{
    public partial class yonetici : Form
    {
        public yonetici()
        {
            InitializeComponent();
        }

        // Re-assigner la meme liste a DataSource ne rafraichit pas la ListBox :
        // il faut d'abord la remettre a null.
        private static void Goster<T>(ListBox kutu, List<T> veri)
        {
            kutu.DataSource = null;
            kutu.DataSource = veri;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Goster(listBox1, listt.list);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Goster(listBox2, listt.ders);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Goster(listBox3, listt.kit);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Goster(listBox4, listt.alinanurun);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Goster(listBox5, listt.toplamfiat);
        }
    }
}
