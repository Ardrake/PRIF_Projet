using System.Linq;
using System.Windows;

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
            //DataPRIFDataContext context = new DataPRIFDataContext("Data Source = ANDRE-PC; Initial Catalog = ML645-05037; Integrated Security = True");
            DataPRIFDataContext context = new DataPRIFDataContext("Data Source=ANDRE-LAPTOP; Initial Catalog = ML645-05037; Integrated Security = True");

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
                if (sqlListeLocation.Count() >= 1)
                {
                    //MessageBox.Show("un vehicule loué ou pluesieur ");
                    LOCATION myLocation = sqlListeLocation.First();

                    tbVehicule.Text = myLocation.VEHICULE.MODELE1.MODELE1 + " " + myLocation.VEHICULE.ANNEE + ", NIV : " 
                        + myLocation.NIV;

                    var sqlListePaiement = from p in context.PAIEMENTs
                                           where p.NIV == myLocation.NIV && p.CLIENT == myLocation.CLIENT
                                           select p;

                    GridListePaiement.ItemsSource = sqlListePaiement;

                    tbNombreTotalPaiement.Text = sqlListePaiement.Count().ToString();
                    tbNombreTotalPaye.Text = string.Format("{0:0.00}$", sqlListePaiement.Sum(item => item.MONTANT));
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
