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
using System.Windows.Threading;


namespace SGML
{
    public partial class MenuPrincipale : Window
    {

        DataPRIFDataContext context = new DataPRIFDataContext();
        

        VEHICULE selectedVehicule = null;
        CLIENT selectedClient = null;
        TERME selectedTerme = null;

        int lKMDebut = 0;
        int lValeur = 0;
        DateTime lDateDebut = new DateTime();
        DateTime lDatePremierPaiement = new DateTime();
        Decimal lMontantPaiement = 0;
        int lNombrePaiement = new int();
        string lNIV = "";
        int lIDClient = 0;
        int lKMFin = 0;
        bool lUsage = false;

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
            
            lesContratDeLocation.ItemsSource = context.LOCATIONs;
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
            bool validation = false;
            bool valclient = false;
            bool valvehicule = false;
            bool valterme = false;
            bool valdatedebut = false;
            bool valdatefin = false;

            if (selectedClient != null)
            {
                lIDClient = selectedClient.IDCLIENT;
                valclient = true;
            }
            else
            {
                MessageBox.Show("Vous devez selectionnez un client");
            }

            if (selectedVehicule != null)
            {
                lNIV = selectedVehicule.NIV;
                valvehicule = true;
            }
            else
            {
                MessageBox.Show("Veuillez selectionnez un vehicule");
            }

            if (selectedTerme != null)
            {
                lNombrePaiement = (int)selectedTerme.NBRANNEE * 12;
                valterme = true;
            }
            else
            {
                MessageBox.Show("Le terme de location ne peut pas etre vide");
            }

            if (lKMDebut > 500)
            {
                lUsage = true;
            }

            if (SelectedDateContrat.Text == "")
            {
                MessageBox.Show("Veuillez selectionnez la date du contrat de location");
            }
            else
            {
                lDateDebut = Convert.ToDateTime(SelectedDateContrat.Text);
                valdatedebut = true;
            }

            if (SelectedDatePremierPaiement.Text == "")
            {
                MessageBox.Show("Veuillez selectionnez la date du premier paiement");
            }
            else
            {
                lDatePremierPaiement = Convert.ToDateTime(SelectedDatePremierPaiement.Text);
                valdatefin = true;
            }

            if (valclient && valvehicule && valterme && valdatedebut && valdatefin)
            {
                try
                {
                    context.INSERE_LOCATIONS(lDateDebut, lDatePremierPaiement, lMontantPaiement, lNombrePaiement,
                                             selectedVehicule.NIV, selectedTerme.ID, selectedClient.IDCLIENT,
                                             lKMDebut, lKMFin, lValeur, lUsage);
                    MessageBox.Show("Nouveau contrat de location enregistré");
                }
                catch
                {
                    MessageBox.Show("Exception a mettre ici");
                }
                finally
                {
                    reinitlocation();
                }
            }
        }

        private void ActualKM_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                lKMDebut = Convert.ToInt32(ActualKM.Text);
                ActualKM.Background = Brushes.White;
            }
            catch
            {
                MessageBox.Show("Caractere non valide, veuillez entré un nombre");
                ActualKM.Text = "0";
                ActualKM.Background = Brushes.Orange;
                Dispatcher.BeginInvoke(DispatcherPriority.Input,
                    new Action(delegate ()
                    {
                        ActualKM.Focus();         // Set Logical Focus 
                        Keyboard.Focus(ActualKM); // Set Keyboard Focus
                    }));
            }
            finally
            {
                if (lKMDebut <= 0)
                {
                    MessageBox.Show("valeur invalid, entre un nombre plus grand que zero");
                    ActualKM.Text = "0";
                    ActualKM.Background = Brushes.Orange;
                    Dispatcher.BeginInvoke(DispatcherPriority.Input,
                       new Action(delegate ()
                       {
                            ActualKM.Focus();         // Set Logical Focus 
                            Keyboard.Focus(ActualKM); // Set Keyboard Focus
                        }));
                }
            }
        }

        private void ActualValeur_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                lValeur = Convert.ToInt32(ActualValeur.Text);
                ActualValeur.Background = Brushes.White;
            }
            catch
            {
                MessageBox.Show("Caractere non valide, veuillez entré un nombre");
                ActualValeur.Text = "0";
                ActualValeur.Background = Brushes.Orange;
                Dispatcher.BeginInvoke(DispatcherPriority.Input,
                    new Action(delegate ()
                    {
                        ActualValeur.Focus();         // Set Logical Focus 
                        Keyboard.Focus(ActualValeur); // Set Keyboard Focus
                    }));
            }
            finally
            {
                if (lValeur <= 0)
                {
                    MessageBox.Show("valeur invalid, entre un nombre plus grand que zero");
                    ActualValeur.Text = "0";
                    ActualValeur.Background = Brushes.Orange;
                    Dispatcher.BeginInvoke(DispatcherPriority.Input,
                        new Action(delegate ()
                        {
                            ActualValeur.Focus();     // Set Logical Focus 
                        Keyboard.Focus(ActualValeur); // Set Keyboard Focus
                    }));
                }
            }
        }

        private void reinitlocation()
        {
            selectedClient = null;
            selectedVehicule = null;
            selectedTerme = null;

            SelectedDatePremierPaiement.SelectedDate = null;
            SelectedDateContrat.SelectedDate = null;

            PaiementListClient.SelectedIndex = -1;
            PaiementListVehicule.SelectedIndex = -1;
            TermesListe.SelectedIndex = -1;

            ActualKM.Text = "0";
            ActualValeur.Text = "0";
            montantPaiement.Text = "0";
            

            lKMDebut = 0;
            lValeur = 0;
            lMontantPaiement = 0;
            lNombrePaiement = 0;
            lNIV = "";
            lIDClient = 0;
            lKMFin = 0;
            lUsage = false;
        }


        private void montantPaiement_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                lMontantPaiement = Convert.ToDecimal(montantPaiement.Text);
                montantPaiement.Background = Brushes.White;
            }
            catch
            {
                MessageBox.Show("Caractere non valide, veuillez entré un nombre");
                montantPaiement.Text = "0";
                montantPaiement.Background = Brushes.Orange;
                Dispatcher.BeginInvoke(DispatcherPriority.Input,
                    new Action(delegate ()
                    {
                        montantPaiement.Focus();         // Set Logical Focus 
                        Keyboard.Focus(montantPaiement); // Set Keyboard Focus
                    }));
            }
            finally
            {
                if (lMontantPaiement <= 0)
                {
                    MessageBox.Show("valeur invalid, entre un nombre plus grand que zero");
                    montantPaiement.Text = "0";
                    montantPaiement.Background = Brushes.Orange;
                    Dispatcher.BeginInvoke(DispatcherPriority.Input,
                        new Action(delegate ()
                        {
                            montantPaiement.Focus();     // Set Logical Focus 
                            Keyboard.Focus(montantPaiement); // Set Keyboard Focus
                        }));
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            reinitlocation();
        }

        private void ButtonModifierLocation(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("On load et modifie le contrat");
        }
    }
}
