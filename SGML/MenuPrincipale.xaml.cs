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
using System.Collections.ObjectModel;


namespace SGML
{
    public partial class MenuPrincipale : Window
    {

        DataPRIFDataContext context = new DataPRIFDataContext();
        VEHICULE selectedVehicule = new VEHICULE();
        CLIENT selectedClient = new CLIENT();
        TERME selectedTerme = null;

        public MenuPrincipale()
        {
            InitializeComponent();
        }

        private void aPropos_Click(object sender, RoutedEventArgs e)
        {
            APropos fenetreAPropos = new APropos();
            fenetreAPropos.ShowDialog();
        }

        private void quitter_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Application.Current.Shutdown();
        }

        private void dashboard_Click(object sender, RoutedEventArgs e)
        {
            hideAllGrids();
            GridDashBoard.Visibility = Visibility.Visible;
        }

        private void nouveauLocation_Click(object sender, RoutedEventArgs e)
        {
            hideAllGrids();
            GridNouvelleLocation.Visibility = Visibility.Visible;
            PaiementListClient.ItemsSource = GetAllClient();
            PaiementListVehicule.ItemsSource = GetAllVehicule();
            TermesListe.ItemsSource = context.TERMEs;
        }

        private void nouveauPaiment_Click(object sender, RoutedEventArgs e)
        {
            hideAllGrids();
            GridNouveauPaiement.Visibility = Visibility.Visible;
        }


        private void hideAllGrids()
        {
            GridNouveauPaiement.Visibility = Visibility.Hidden;
            GridNouvelleLocation.Visibility = Visibility.Hidden;
            GridDashBoard.Visibility = Visibility.Hidden;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

           

        }
   
        private List<string> GetAllVehicule()
        {
            List<string> myList = new List<string>();
            var getList = context.SELECT_ALL_VEHICULE();
            foreach (var item in getList)
            {
                string vehicule = item.NIV + " - " + item.MODELE + ", " + item.ANNEE + ", " +
                                  item.TYPE + ", " + item.COULEUR;
                myList.Add(vehicule);
            }
            return myList;
        }

        private ObservableCollection<CLIENT> GetAllClient()
        {
            ObservableCollection<CLIENT> myList = new ObservableCollection<CLIENT>();
            var getList = context.CLIENTs;
            foreach (var item in getList)
            {
                myList.Add(item);
            }
            return myList;
        }

        private void PaiementListClient_DropDownClosed(object sender, EventArgs e)
        {
            selectedClient = PaiementListClient.SelectedItem as CLIENT;
        }


        private void TermesListe_DropDownClosed(object sender, EventArgs e)
        {
            selectedTerme = TermesListe.SelectedItem as TERME;
        }

        private void PaiementListVehicule_DropDownClosed(object sender, EventArgs e)
        {
            foreach (VEHICULE item in context.VEHICULEs)
            { 
                if (PaiementListVehicule.Text.Length > 0)
                { 
                    if (PaiementListVehicule.Text.Substring(0, 23) == item.NIV)
                    {
                        selectedVehicule = item;
                        break;
                    }
                }
            }
        }

        private void insereLocationButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime lDateDebut = new DateTime();
            DateTime lDatePremierPaiement = new DateTime();
            Decimal lMontantPaiement = new Decimal();
            int lNombrePaiement = new int();

            //lDateDebut = (DateTime)SelectedDateContrat.SelectedDate;
            //lDatePremierPaiement = (DateTime)SelectedDatePremierPaiement.SelectedDate;
            //lMontantPaiement = Convert.ToDecimal(montantPaiement);

            if (selectedTerme != null)
            {
                lNombrePaiement = (int)selectedTerme.NBRANNEE * 12;
            }
            else
            {
                MessageBox.Show("Le terme de location ne peut pas etre vide");
            }
            

            

            //context.INSERE_LOCATIONS();
        }


    }
}
