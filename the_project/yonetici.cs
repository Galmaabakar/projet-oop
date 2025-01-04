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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.DataSource = listt.list;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox2.DataSource = listt.ders;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listBox3.DataSource = listt.kit;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listBox4.DataSource = listt.alinanurun;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listBox5.DataSource = listt.toplamfiat;
        }
    }
}
