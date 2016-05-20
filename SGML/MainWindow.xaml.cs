using System.Windows;

namespace SGML
{
    public partial class MainWindow : Window
    {
        int nbressai = 0;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            if (USERNAME.Text == "" || Password.Password == "")
            {
                //Gestion des acces a venir 
                this.Hide();
                MenuPrincipale myMenu = new MenuPrincipale("admin");
                myMenu.Show();
                //MessageBox.Show("Usager et mot de passe sont requis pour obtenir l'accès au système", "Login Incorrect", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

            else if (USERNAME.Text == "admin" && Password.Password == "admin")
            {
                this.Hide();
                MenuPrincipale myMenu = new MenuPrincipale("admin");
                myMenu.Show();
            }
            else if (USERNAME.Text == "Gerant" && Password.Password == "Gerant")
            {
                this.Hide();
                MenuPrincipale myMenu = new MenuPrincipale("Gerant");
                myMenu.Show();
            }
            else if (USERNAME.Text == "Associé" && Password.Password == "Associé")
            {
                this.Hide();
                MenuPrincipale myMenu = new MenuPrincipale("Associé");
                myMenu.Show();
            }
            else if (USERNAME.Text == "Proprietaire" && Password.Password == "Proprietaire")
            {
                this.Hide();
                MenuPrincipale myMenu = new MenuPrincipale("Proprietaire");
                myMenu.Show();
            }
            else
            {
                MessageBox.Show("Erreur usager ou mot de passe", "Login Incorrect", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                nbressai += 1;
            }

            if (nbressai == 3)
            {
                this.Close();
            }

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
