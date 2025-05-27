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
    using System.Drawing;
    using System.Windows.Forms;

    namespace Stratego_Jean_Gazon
    {
        public class Other
        {
            private Button bValider;
            private Form Jeu;
            private Button Button_Pret;

            public Other(Button Valider, Form FicJeu,Button btnpret)
            {
                bValider = Valider;
                Jeu = FicJeu;
                Button_Pret = btnpret;
            }

            public void bValider_position()
            {
                int margin = 10; // Marge entre le bouton et le bord de la fenêtre

                int x = Jeu.ClientSize.Width - bValider.Width - margin;
                int y = Jeu.ClientSize.Height - bValider.Height - margin;

                bValider.Location = new System.Drawing.Point(x, y);
                Button_Pret.Location = new Point(
                                            Jeu.ClientSize.Width - (2 * Button_Pret.Width) - 30,
                                               Jeu.ClientSize.Height - Button_Pret.Height - 10
            );
            }
        }
    }
}
