using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stratego_Jean_Gazon
{
    public class GameTransitionManager
    {
        private readonly Form parentForm;

        // Transition générale (placement, changement de joueur)
        private readonly Panel panelTransition; // Composants graphiques utilisés pour afficher les transitions (placement, changement de joueur, combat).
        // Déclarés en readonly pour garantir qu'ils ne seront jamais réassignés après l'initialisation du constructeu
        private readonly PictureBox pictureBoxTransition;
        private readonly Label labelTransition;

        // Transition combat
        private readonly Panel panelCombat;
        private readonly PictureBox pictureBoxAttaquant;
        private readonly PictureBox pictureBoxDefenseur;
        private readonly Label labelCombatResult;

        public GameTransitionManager(Form parent, Image imageTransition)
        {
            parentForm = parent;

            // --- Panel de transition général ---
            panelTransition = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Black,
                Visible = false
            };

            pictureBoxTransition = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(210, 210),
                BackColor = Color.Transparent,
                Image = imageTransition,
                Anchor = AnchorStyles.None
            };

            labelTransition = new Label
            {
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Font = new Font("Arial", 32, FontStyle.Bold),
                AutoSize = true,
                Anchor = AnchorStyles.None,
                TextAlign = ContentAlignment.MiddleCenter
            };

            panelTransition.Controls.Add(pictureBoxTransition);
            panelTransition.Controls.Add(labelTransition);
            panelTransition.Resize += (s, e) => CenterTransitionElements();

            parent.Controls.Add(panelTransition);

            // --- Panel de combat ---
            panelCombat = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Black,
                Visible = false
            };

            pictureBoxAttaquant = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(140, 140),
                BackColor = Color.Transparent
            };

            pictureBoxDefenseur = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(140, 140),
                BackColor = Color.Transparent
            };

            labelCombatResult = new Label
            {
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Font = new Font("Arial", 36, FontStyle.Bold),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };

            panelCombat.Controls.Add(pictureBoxAttaquant);
            panelCombat.Controls.Add(pictureBoxDefenseur);
            panelCombat.Controls.Add(labelCombatResult);
            panelCombat.Resize += (s, e) => CenterCombatElements();

            parent.Controls.Add(panelCombat);
        }

        
        private void CenterTransitionElements()
        {
            labelTransition.PerformLayout();
            labelTransition.Update();

            pictureBoxTransition.Location = new Point(
                (panelTransition.Width - pictureBoxTransition.Width) / 2,
                (panelTransition.Height - pictureBoxTransition.Height) / 2 - 60
            );
            labelTransition.Location = new Point(
                (panelTransition.Width - labelTransition.Width) / 2,
                (panelTransition.Height - labelTransition.Height) / 2 + 80
            );
        }

        private void CenterCombatElements()
        {
            pictureBoxDefenseur.Location = new Point(
                panelCombat.Width / 4 - pictureBoxDefenseur.Width / 2,
                panelCombat.Height / 2 - pictureBoxDefenseur.Height / 2
            );
            pictureBoxAttaquant.Location = new Point(
                3 * panelCombat.Width / 4 - pictureBoxAttaquant.Width / 2,
                panelCombat.Height / 2 - pictureBoxAttaquant.Height / 2
            );
            labelCombatResult.Location = new Point(
                (panelCombat.Width - labelCombatResult.Width) / 2,
                panelCombat.Height / 4 - labelCombatResult.Height / 2
            );
        }

        // --- Affichage des transitions ---
        public async Task ShowTransition(string message, int durationMs = 2000)
        {
            labelTransition.Text = message;
            labelTransition.AutoSize = true;
            CenterTransitionElements();

            panelTransition.BringToFront();
            panelTransition.Visible = true;
            panelTransition.Update();

            await Task.Delay(durationMs);

            panelTransition.Visible = false;
        }

        public async Task ShowPlacement(Player joueur, int durationMs = 2000)
        {
            string texte = joueur == Player.Player_Blue
                ? "Au joueur Bleu de placer ses pions"
                : "Au joueur Rouge de placer ses pions";
            await ShowTransition(texte, durationMs);
        }

        public async Task ShowChangeTurn(Player joueur, int durationMs = 2000)
        {
            string texte = $"Au tour de : {(joueur == Player.Player_Blue ? "Rouge" : "Bleu")}";
            await ShowTransition(texte, durationMs);
        }

        public async Task ShowCombat(Image imgAttaquant, bool attaquantBlue, Image imgDefenseur, bool defenseurBlue, string resultat, int durationMs = 1800)
        {
            pictureBoxAttaquant.Image = imgAttaquant;
            pictureBoxAttaquant.BackColor = attaquantBlue ? Color.LightBlue : Color.LightCoral;
            pictureBoxDefenseur.Image = imgDefenseur;
            pictureBoxDefenseur.BackColor = defenseurBlue ? Color.LightBlue : Color.LightCoral;

            labelCombatResult.Text = resultat;
            labelCombatResult.AutoSize = true;
            CenterCombatElements();

            panelCombat.BringToFront();
            panelCombat.Visible = true;
            panelCombat.Update();

            await Task.Delay(durationMs);

            panelCombat.Visible = false;
        }
    }
}
