using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stratego_Jean_Gazon.Reseau;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace Stratego_Jean_Gazon
{
    public partial class FicSettings : Form
    {
        public FicSettings()
        {
            InitializeComponent();
        }

        private void btnValidersettings_Click(object sender, EventArgs e)
        {
            // Récupération sécurisée des valeurs saisies
            var ip = tbAddressIP.Text;
            int port;
            int.TryParse(tbPortServeur.Text, out port);
            // Appliquer aux classes serveur et client (valeurs globales par défaut)
            ReseauServeur.DefaultAdresseIP = ip;
            ReseauServeur.DefaultPort = port;

            ReseauClient.DefaultAdresseIP = ip;
            ReseauClient.DefaultPort = port;
            
            MessageBox.Show("Paramètres enregistrés avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            this.Close();
        }

        private void IsServeur_CheckedChanged(object sender, EventArgs e)
        {
            if (IsServeur.Checked)
            {
                // Récupère l'IP locale (IPv4) et utilise le port par défaut du serveur
                var localIp = GetLocalIPv4Address();
                var port = ReseauServeur.DefaultPort;

                // Affiche dans les textboxes et désactive la modification
                tbAddressIP.Text = localIp;
                tbPortServeur.Text = port.ToString();
                tbAddressIP.Enabled = true;
                tbPortServeur.Enabled = false;

                // Met à jour les valeurs par défaut utilisées par le réseau
                ReseauServeur.DefaultAdresseIP = localIp;
                ReseauServeur.DefaultPort = port;
                ReseauClient.DefaultAdresseIP = localIp;
                ReseauClient.DefaultPort = port;

                FicJeu.IsServeur = true;
            }
            else
            {
                // Réactive la modification manuelle
                tbAddressIP.Enabled = true;
                tbPortServeur.Enabled = true;
                FicJeu.IsServeur = false;
            }
        }

        private string GetLocalIPv4Address()
        {
            try
            {
                // Parcours des interfaces réseau opérationnelles et filtrage des adresses APIPA / loopback
                foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (ni.OperationalStatus != OperationalStatus.Up)
                        continue;
                    if (ni.NetworkInterfaceType == NetworkInterfaceType.Loopback ||
                        ni.NetworkInterfaceType == NetworkInterfaceType.Tunnel)
                        continue;

                    var ipProps = ni.GetIPProperties();
                    foreach (var ua in ipProps.UnicastAddresses)
                    {
                        var ip = ua.Address;
                        if (ip.AddressFamily != AddressFamily.InterNetwork)
                            continue;
                        var s = ip.ToString();
                        if (IPAddress.IsLoopback(ip))
                            continue;
                        // Exclure les adresses APIPA (169.254.x.x)
                        if (s.StartsWith("169.254.", StringComparison.OrdinalIgnoreCase))
                            continue;
                        // Retourne la première IPv4 locale valide trouvée
                        return s;
                    }
                }

                // Repli : résolution DNS du nom d'hôte (filtre APIPA)
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork &&
                        !IPAddress.IsLoopback(ip) &&
                        !ip.ToString().StartsWith("169.254.", StringComparison.OrdinalIgnoreCase))
                        return ip.ToString();
                }
            }
            catch
            {
                // Ignorer et tomber sur le fallback
            }

            // Fallback sûr
            return "127.0.0.1";
        }
    }
}
