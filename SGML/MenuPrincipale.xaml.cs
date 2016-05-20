using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace SGML
{
    public partial class MenuPrincipale : Window
    {
        //Gestion de la connexion a la base de donnée... A valider
        //DataPRIFDataContext context = new DataPRIFDataContext("Data Source = ANDRE-PC; Initial Catalog = ML645-05037; Integrated Security = True");
        DataPRIFDataContext context = new DataPRIFDataContext("Data Source=ANDRE-LAPTOP; Initial Catalog = ML645-05037; Integrated Security = True");


        LOCATION selectedLocation = null;
        VEHICULE selectedVehicule = null;
        public static CLIENT selectedClient = null;
        TERME selectedTerme = null;

        bool isInsertMode = false;
        bool isBeingEdited = false;

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
        bool valkm = false;
        bool typeOperationInsere = true;

        public MenuPrincipale(string user)
        {
            InitializeComponent();
            sbUser.Text = user;

            // Validation connection base de donnée
            try
            {
                var testconnection = from m in context.CLIENTs
                                     select m;
            }
            catch
            {
                MessageBox.Show("Erreur de connection a la base de donnée - Fermeture de l'application");
            }
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

        /// <summary>
        /// Affichage de l'ecran d'entré et modification des location
        /// </summary>
        private void nouveauLocation_Click(object sender, RoutedEventArgs e)
        {
            hideAllGrids();
            GridNouvelleLocation.Visibility = Visibility.Visible;
            PaiementListClient.ItemsSource = GetAllClient();
            PaiementListVehicule.ItemsSource = GetAllVehicule();
            TermesListe.ItemsSource = context.TERMEs;

            var sqlListeContrat = from l in context.LOCATIONs
                                  join v in context.VEHICULEs on l.NIV equals v.NIV
                                  join m in context.MODELEs on v.MODELE equals m.ID
                                  join t in context.TYPEs on v.TYPE equals t.ID
                                  join c in context.COULEURs on v.COULEUR equals c.ID
                                  select l;

            lesContratDeLocation.ItemsSource = sqlListeContrat;
        }

        /// <summary>
        /// Affichage de l'ecran des gestion des paiement
        /// </summary>
        private void nouveauPaiment_Click(object sender, RoutedEventArgs e)
        {
            hideAllGrids();
            GridNouveauPaiement.Visibility = Visibility.Visible;
            try
            {
                PaiementListContrat.ItemsSource = context.LOCATIONs;
                ListeDesPaiement.CanUserDeleteRows = false;
            }
            catch 
            {
                MessageBox.Show("Erreur connection a la base de donnée");
            }
        }

        /// <summary>
        /// Affichage de l'ecran de gesion d'impression des rapport - État de compte
        /// </summary>
        private void PrintEtatDeCompte_Click(object sender, RoutedEventArgs e)
        {
            hideAllGrids();
            GridEtatCompte.Visibility = Visibility.Visible;
            EtatListClient.ItemsSource = context.CLIENTs;
        }

        /// <summary>
        /// Fonction pour caché les items des écran précedent
        /// </summary>
        private void hideAllGrids()
        {
            GridNouveauPaiement.Visibility = Visibility.Hidden;
            GridNouvelleLocation.Visibility = Visibility.Hidden;
            GridDashBoard.Visibility = Visibility.Hidden;
            GridEtatCompte.Visibility = Visibility.Hidden;
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
            try
            {
                var getList = context.CLIENTs;
                foreach (var item in getList)
                {
                    myList.Add(item);
                }
            }
            catch 
            {
                MessageBox.Show("Erreur de connection a la base de donnée");
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
                if (typeOperationInsere) //Insertion d'un nouvelle location
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

                        var sqlListeContrat = from l in context.LOCATIONs
                                              join v in context.VEHICULEs on l.NIV equals v.NIV
                                              join m in context.MODELEs on v.MODELE equals m.ID
                                              join t in context.TYPEs on v.TYPE equals t.ID
                                              join c in context.COULEURs on v.COULEUR equals c.ID
                                              select l;

                        lesContratDeLocation.ItemsSource = sqlListeContrat;
                    }
                }

                else // Modification de la location
                {
                    selectedLocation.DATEDEBUT = lDateDebut;
                    selectedLocation.DATEPAIEMENT = lDatePremierPaiement;
                    selectedLocation.MONTANT = lMontantPaiement;
                    selectedLocation.NBRPAIEMENT = lNombrePaiement;
                    selectedLocation.NIV = selectedVehicule.NIV;
                    selectedLocation.TERME = selectedTerme.ID;
                    selectedLocation.IDCLIENT = selectedClient.IDCLIENT;
                    selectedLocation.KM_DEBUT = lKMDebut;
                    selectedLocation.KM_FIN = lKMFin;
                    selectedLocation.VALEUR = lValeur;
                    selectedLocation.USAGE = lUsage;
                    context.SubmitChanges();
                    MessageBox.Show("Modification sauvegarder");
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
                valkm = true;
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
            if (valkm)
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
        }

        /// <summary>
        /// REinitialisation des variable d'ecran
        /// </summary>
        private void reinitlocation()
        {
            selectedLocation = null;
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
            ButtonInsereLcoation.Content = "Inséré Location";
            typeOperationInsere = true;
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
            // Stored proc SELECT LOCATION non utiliser dans ce cas, parce qu'on est bindé directement sur la liste des location
            if (lesContratDeLocation.SelectedItems.Count == 1)
            {
                ButtonInsereLcoation.Content = "Sauvegardé modification";
                typeOperationInsere = false;
                selectedLocation = (LOCATION)lesContratDeLocation.SelectedItem;
                selectedClient = selectedLocation.CLIENT;
                selectedTerme = selectedLocation.TERME1;
                selectedVehicule = selectedLocation.VEHICULE;
                PaiementListClient.SelectedValue = selectedLocation.CLIENT;
                PaiementListVehicule.SelectedValue = selectedLocation.VEHICULE.NIV + " - " + selectedLocation.VEHICULE.MODELE1.MODELE1 +
                                                     ", " + selectedLocation.VEHICULE.ANNEE + ", " + selectedLocation.VEHICULE.TYPE1.TYPE1 +
                                                     ", " + selectedLocation.VEHICULE.COULEUR1.COULEUR1;
                TermesListe.SelectedValue = selectedLocation.TERME1;
                ActualKM.Text = Convert.ToString(selectedLocation.KM_DEBUT);
                ActualValeur.Text = String.Format("{0:0.00}", selectedLocation.VALEUR);
                montantPaiement.Text = String.Format("{0:0.00}", selectedLocation.MONTANT);
                SelectedDateContrat.Text = selectedLocation.DATEDEBUT.ToString();
                SelectedDatePremierPaiement.Text = selectedLocation.DATEPAIEMENT.ToString();
            }
            else
            {
                MessageBox.Show("Vous devez selectionnez un contrat a modifiler");
            }
        }

        private void PaiementListContrat_DropDownClosed(object sender, EventArgs e)
        {
            selectedLocation = (LOCATION)PaiementListContrat.SelectedItem;
            try
            {
                paimentMontantDu.Text = String.Format("{0:0.00}", selectedLocation.MONTANT);
            }
            catch
            {
                paimentMontantDu.Text = "0.00";
            }

            try
            {
                ListeDesPaiement.ItemsSource = GetPaiemmentListe(selectedLocation.IDCLIENT, selectedLocation.NIV);
            }
            catch
            {
                ListeDesPaiement.ItemsSource = null;
            }
        }

        private ObservableCollection<PAIEMENT> GetPaiemmentListe(int idclient, string niv)
        {
            var listPaiement = from m in context.PAIEMENTs
                               where m.IDCLIENT == idclient && m.NIV == niv
                               select m;

            return new ObservableCollection<PAIEMENT>(listPaiement);
        }


        /// <summary>
        /// Gestion du edit dans le tabelau / datagrid
        /// </summary>
        private void dgEmp_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            PAIEMENT myPaiement = e.Row.DataContext as PAIEMENT;

            if (isInsertMode && myPaiement != null) // Mode insertion de record
            {
                var InsertRecord = MessageBox.Show("Voulez vous rajouter un paiement de " + myPaiement.MONTANT + " en date du " + String.Format("{0:dd-MM-yyyy}", myPaiement.DATE) + " ?", "Confirmez", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (InsertRecord == MessageBoxResult.Yes)
                {
                    context.INSERE_PAIEMENT(myPaiement.MONTANT, selectedLocation.VEHICULE.NIV, selectedLocation.CLIENT.IDCLIENT);

                    context.SubmitChanges();
                    ListeDesPaiement.ItemsSource = GetPaiemmentListe(selectedLocation.IDCLIENT, selectedLocation.NIV);

                    MessageBox.Show("Traitement du paiement en date du: " + String.Format("{0:dd-MM-yyyy}", myPaiement.DATE) + " Montant: " + myPaiement.MONTANT + " a été rajouté!", "Paiement Enregistré", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                    ListeDesPaiement.ItemsSource = GetPaiemmentListe(selectedLocation.IDCLIENT, selectedLocation.NIV);
            }
            context.SubmitChanges(); // Sauvegarde les changement si on insere pas
            isInsertMode = false;
        }

        private void dgEmp_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            isInsertMode = true;
        }

        private void dgEmp_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            isBeingEdited = true;
        }

        private void Button_Click_Etat_de_Compte(object sender, RoutedEventArgs e)
        {
            selectedClient = (CLIENT)EtatListClient.SelectedItem;
            EtatCompte myEtatCompte = new EtatCompte(selectedClient);
            myEtatCompte.ShowDialog();

        }

        private void changeUser_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            //Load login form .......
        }
    }
}
