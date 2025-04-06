using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stratego_Jean_Gazon
{
    using System;
    using System.Windows.Forms;

    namespace Stratego_Jean_Gazon
    {
        public class Other
        {
            private Button bValider;
            private Form Jeu;

            public Other(Button Valider, Form FicJeu)
            {
                bValider = Valider;
                Jeu = FicJeu;
            }

            public void bValider_position()
            {
                int margin = 10; // Marge entre le bouton et le bord de la fenêtre

                int x = Jeu.ClientSize.Width - bValider.Width - margin;
                int y = Jeu.ClientSize.Height - bValider.Height - margin;

                bValider.Location = new System.Drawing.Point(x, y);
            }
        }
    }
}
