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
    public partial class FicRegles : Form
    {
        public FicRegles()
        {
            InitializeComponent();
        }

        private void FicRegles_Load(object sender, EventArgs e)
        {
            afficher_regle();
        }
        private void afficher_regle()
        {
            rtRegle.Text = "📜 HÉLA, GUERRIER ! PRÉPARE-TOI À L’ULTIME BATAILLE ! ⚔️\n\n" +
        "Bienvenue sur le champ de bataille, où seule la ruse et le courage décideront du vainqueur. " +
        "Ton armée est prête, tes espions sont en place… mais sauras-tu déjouer les pièges de l’ennemi ?\n\n" +

        "══════════════════════════════════════════\n" +
        "🎖️ 1. LA MISSION\n" +
        "══════════════════════════════════════════\n" +
        "Ton objectif est simple : CAPTURER LE DRAPEAU ENNEMI tout en protégeant le tien.\n\n" +

        "══════════════════════════════════════════\n" +
        "🛡️ 2. LA FORMATION DE TON ARMÉE\n" +
        "══════════════════════════════════════════\n" +
        "Avant le combat, dispose secrètement tes troupes sur le champ de bataille. Chaque guerrier a une valeur, " +
        "plus il est puissant, plus il domine l’adversaire !\n\n" +

        "• 👑 Marshal (10) - L’élite, la terreur du champ de bataille.\n" +
        "• 🏅 Général (9) - Un stratège redoutable.\n" +
        "• 🏰 Colonels (8), ⚔️ Commandants (7), 🛡️ Capitaines (6)\n" +
        "• 🎖️ Lieutenants (5), 🎯 Sergents (4)\n" +
        "• 🧨 Démineurs (3) - Essentiels pour désamorcer les bombes.\n" +
        "• 🏃 Éclaireurs (2) - Rapides, ils traversent le champ de bataille.\n" +
        "• 🕵️ Espions (1) - Seuls capables d’éliminer un Marshal !\n" +
        "• 💣 Bombes - Explosives, elles anéantissent quiconque ose s’y frotter… sauf les démineurs.\n" +
        "• 🚩 Le Drapeau - Protège-le coûte que coûte !\n\n" +

        "⚠️ Une fois ton armée positionnée, elle ne pourra plus bouger jusqu'à la bataille !\n\n" +

        "══════════════════════════════════════════\n" +
        "⚔️ 3. LE COMBAT\n" +
        "══════════════════════════════════════════\n" +
        "À chaque tour, déplace l’une de tes unités d’une case (sauf les bombes et le drapeau qui restent fixes).\n" +
        "Si tu rencontres un ennemi, le combat s’engage :\n\n" +

        "🛡️ L’unité la plus forte l’emporte !\n" +
        "💀 Si deux unités de même valeur s’affrontent, elles sont éliminées toutes les deux.\n" +
        "🕵️ L’espion (1) est le seul à pouvoir éliminer un Marshal (10) d’un coup fatal.\n" +
        "💣 Une unité attaquant une bombe est détruite sur-le-champ ! Seul un démineur (3) peut la désamorcer.\n\n" +

        "L’ennemi ne révélera ses forces qu’au moment de l’affrontement… Saura-tu anticiper ses mouvements ?\n\n" +

        "══════════════════════════════════════════\n" +
        "🏆 4. LA VICTOIRE\n" +
        "══════════════════════════════════════════\n" +
        "Tu remportes la guerre si :\n\n" +

        "✅ Tu captures le drapeau ennemi.\n" +
        "✅ Ton adversaire ne peut plus faire de mouvement.\n\n" +

        "Seul un grand stratège peut dominer le champ de bataille. Seras-tu ce guerrier légendaire ?\n\n" +

        "🔥 EN AVANT, HÉROS ! Ta destinée t’appelle. Place tes forces avec sagesse, " +
        "trompe ton adversaire et mène ton armée vers la victoire !";
        }

        private void bRegle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rtRegle_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
