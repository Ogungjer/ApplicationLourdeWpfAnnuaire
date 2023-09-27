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

namespace ApplicationLourdeWpfAnnuaire.Views
{
    /// <summary>
    /// Logique d'interaction pour AdminLogin.xaml
    /// </summary>
    public partial class AdminLogin : Window
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public AdminLogin()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Récupére le nom d'utilisateur et le mot de passe saisis
            Username = UsernameTextBox.Text;
            Password = PasswordBox.Password;

            // Vérifie si les champs sont vides
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Veuillez saisir un nom d'utilisateur et un mot de passe.");
                return;
            }

            // Indique que la connexion est réussie et ferme la fenêtre
            DialogResult = true;
        }
    }
}
