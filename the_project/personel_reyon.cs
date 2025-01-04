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
    public partial class personel_reyon : Form
    {
        public personel_reyon()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.Items.Contains(textBox1.Text + " - " + textBox2.Text))
            {
                MessageBox.Show("urununuz alindi");
                int fiat;
                fiat = int.Parse(textBox3.Text);
                listt.fiat.Add(fiat);
                listt.alinanurun.Add(textBox1.Text + " - " + textBox2.Text);
            }
            else if (comboBox2.Items.Contains(textBox1.Text + " - " + textBox2.Text))
            {
                MessageBox.Show("urunuz stokta yoktur ,bir hafta icinde gelecek");
            }
            else { MessageBox.Show("istediginiz urun bizde yoktur"); }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int toplam = listt.fiat.Sum();
            MessageBox.Show(toplam.ToString());
            listt.toplamfiat.Add(toplam);
        }
    }
}
