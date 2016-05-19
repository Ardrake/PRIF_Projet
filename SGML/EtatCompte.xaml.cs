using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace SGML
{
    /// <summary>
    /// Interaction logic for EtatCompte.xaml
    /// </summary>
    public partial class EtatCompte : Window
    {
        public EtatCompte(CLIENT etatClient)
        {
            DataPRIFDataContext context = new DataPRIFDataContext();

            InitializeComponent();

            if (etatClient != null)
            {
                tbNonClient.Text = etatClient.PRENOM + " " + etatClient.NOM;
                tbTelephone.Text = FormattedPhoneNumber(etatClient.TELEPHONE);


                var sqlListeLocation = from l in context.LOCATIONs
                                       where l.CLIENT.IDCLIENT == etatClient.IDCLIENT
                                       select l;

                

                if (sqlListeLocation.Count() == 0)
                {
                    MessageBox.Show("Aucune vehicule loué");
                }
                if (sqlListeLocation.Count() == 1)
                {
                    MessageBox.Show("un vehicule loué");
                    LOCATION myLocation = sqlListeLocation.First();

                    tbVehicule.Text = myLocation.VEHICULE.MODELE1.MODELE1 + " " + myLocation.VEHICULE.ANNEE + ", NIV : " 
                        + myLocation.NIV;

                    var sqlListePaiement = from p in context.PAIEMENTs
                                           where p.NIV == myLocation.NIV && p.CLIENT == myLocation.CLIENT
                                           select p;

                    GridListePaiement.ItemsSource = sqlListePaiement;

                }
                else
                {
                    MessageBox.Show("Gestion rapport multi vehicule");
                }
                



            }
            else
            {
                MessageBox.Show("Erreur client");
            }


        }

        public string FormattedPhoneNumber(string tel)
        {
            if (tel == null)
                return string.Empty;

            switch (tel.Length)
            {
                case 7:
                    return Regex.Replace(tel, @"(\d{3})(\d{4})", "$1-$2");
                case 10:
                    return Regex.Replace(tel, @"(\d{3})(\d{3})(\d{4})", "($1) $2-$3");
                case 11:
                    return Regex.Replace(tel, @"(\d{1})(\d{3})(\d{3})(\d{4})", "$1-$2-$3-$4");
                default:
                    return tel;
            }
        }
    }
}
