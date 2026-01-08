csharp FicJeu.Network.cs
using System;
using System.Drawing;
using System.Threading.Tasks;
using Stratego_Jean_Gazon.Network;

namespace Stratego_Jean_Gazon
{
    public partial class FicJeu
    {
        private NetworkManager networkManager;

        private void InitializeNetwork()
        {
            networkManager = new NetworkManager();
            networkManager.MessageReceived += Network_MessageReceived;
        }

        // appel depuis le code UI pour démarrer en tant que serveur
        public Task StartServerAsync(int port)
        {
            return networkManager.StartServerAsync(port);
        }

        // appel depuis le code UI pour se connecter à un serveur
        public Task ConnectToServerAsync(string host, int port)
        {
            return networkManager.ConnectToServerAsync(host, port);
        }

        // envoi d'un mouvement
        public Task SendMoveAsync(personnage_base pion, Point from, Point to)
        {
            var mv = new MoveMessage
            {
                FromX = from.X,
                FromY = from.Y,
                ToX = to.X,
                ToY = to.Y,
                IsBlue = pion.Couleur
            };
            return networkManager.SendAsync(mv, MessageType.Move);
        }

        // réception d'un message réseau
        private void Network_MessageReceived(MessageType type, object payload)
        {
            if (type == MessageType.Move && payload is MoveMessage mv)
            {
                // marshal sur le thread UI
                this.Invoke(new Action(() =>
                {
                    // trouver la PictureBox cible / personnage local et appliquer le déplacement
                    // Vous pouvez réutiliser votre logique existante : rechercher le PictureBox à partir des positions
                    // Exemple minimal : mettre à jour dictionnaires et déplacer l'UI si nécessaire
                    Point src = new Point(mv.FromX, mv.FromY);
                    Point dst = new Point(mv.ToX, mv.ToY);

                    // chercher le PictureBox source
                    System.Windows.Forms.PictureBox pbSource = null;
                    personnage_base infoSource = null;
                    foreach (System.Windows.Forms.Control ctrl in grille_manager.PnlGrilleGame.Controls)
                    {
                        if (ctrl is System.Windows.Forms.PictureBox pb && pb.Tag is personnage_base p)
                        {
                            if (p.PositionGrille == src)
                            {
                                pbSource = pb;
                                infoSource = p;
                                break;
                            }
                        }
                    }

                    if (pbSource != null && infoSource != null)
                    {
                        // appliquez exactement la même méthode que vous utilisez localement
                        grille_manager.DeplacerPion(infoSource, pbSource, dst);
                    }
                    else
                    {
                        // si la pièce n'existe pas localement : possible synchronisation nécessaire
                        // à implémenter : demander l'état complet ou ignorer selon votre protocole
                    }
                }));
            }
        }
    }
}