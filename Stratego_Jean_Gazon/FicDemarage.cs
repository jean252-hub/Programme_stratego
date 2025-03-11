using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stratego_Jean_Gazon
{
    public partial class FMenu : Form
    {
        public FMenu()
        {
            InitializeComponent();
        }

        private void bquitter_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bregle_Click(object sender, EventArgs e)
        {
            FicRegles Regle_jeux = new FicRegles();
            Regle_jeux.ShowDialog(this);
        }
    }
}
