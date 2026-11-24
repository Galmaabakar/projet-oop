using System;
using System.Linq;
using System.Windows.Forms;

namespace the_project
{
    // Ecran du personnel qui prete les outils (personel_alet).
    public partial class personel_alet : Form
    {
        private readonly personel_aletclass personel;

        public personel_alet(personel_aletclass personel)
        {
            InitializeComponent();
            this.personel = personel;
            Text = $"Alet deposu - {personel.ad} {personel.soyad}";
            Yenile();
        }

        // Recharge la liste des outils (leur etat a pu changer).
        private void Yenile()
        {
            object secili = comboBox1.SelectedItem;
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(Veriler.Aletler.ToArray<object>());
            if (secili != null) comboBox1.SelectedItem = secili;

            listBox1.Items.Clear();
            listBox1.Items.AddRange(Veriler.Aletler.Where(a => !a.Musait).ToArray<object>());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string mesaj;
            if (personel.OduncVer(comboBox1.SelectedItem as Alet, textBox1.Text, out mesaj))
            {
                textBox1.Clear();
                Yenile();
            }
            MessageBox.Show(mesaj);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // on peut rendre l'outil choisi dans la liste des prets ou dans la liste deroulante
            Alet alet = (listBox1.SelectedItem ?? comboBox1.SelectedItem) as Alet;
            string mesaj;
            if (personel.IadeAl(alet, out mesaj))
            {
                Yenile();
            }
            MessageBox.Show(mesaj);
        }
    }
}
