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
    public partial class personel_kutuphane : Form
    {
        public personel_kutuphane()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "" || textBox2.Text.Trim() == "")
            {
                MessageBox.Show("kitap adi ve yazar bos olamaz");
                return;
            }
            // meme format " - " que les livres deja presents dans la liste
            string kita = textBox1.Text.Trim() + " - " + textBox2.Text.Trim();
            comboBox2.Items.Add(kita);
            MessageBox.Show("kitabiniz basari ile kaydedildi");
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            // SelectedItem est null si rien n'est choisi -> NullReferenceException avant
            if (comboBox1.SelectedItem == null && comboBox2.SelectedItem == null)
            {
                MessageBox.Show("lutfen bir kitap seciniz");
                return;
            }
            if (comboBox2.SelectedItem != null) listt.kit.Add(comboBox2.SelectedItem.ToString());
            if (comboBox1.SelectedItem != null) listt.kit.Add(comboBox1.SelectedItem.ToString());
            MessageBox.Show("Kitabiniz basari ile alindi");
        }
    }
}
