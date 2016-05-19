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
using System.Windows.Navigation;
using System.Windows.Shapes;

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
                //this.Hide();
                MenuPrincipale myMenu = new MenuPrincipale();
                myMenu.Show();
                this.Close();
                //MessageBox.Show("Usager et mot de passe sont requis pour obtenir l'accès au système", "Login Incorrect", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

            else if (USERNAME.Text == "SGI" && Password.Password == "admin")
            {
                this.Hide();
                MenuPrincipale myMenu = new MenuPrincipale();
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
