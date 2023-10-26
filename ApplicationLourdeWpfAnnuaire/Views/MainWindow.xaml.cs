using ApplicationLourdeWpfAnnuaire.Views;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace ApplicationLourdeWpfAnnuaire
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Gestion du clic sur le bouton "Admin"
        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {
            // Affiche une fenêtre de connexion pour l'admin
            var adminLoginWindow = new AdminLogin();
            if (adminLoginWindow.ShowDialog() == true)
            {
                // Vérifie le nom d'utilisateur et le mot de passe
                if (IsValidAdmin(adminLoginWindow.Username, adminLoginWindow.Password))
                {
                    // Affiche l'interface du mode admin
                    VisitorMode.Visibility = Visibility.Collapsed;
                    AdminMode.Visibility = Visibility.Visible;
                } 
                else
                {
                    MessageBox.Show("Nom d'utilisateur ou mot de passe incorrect.");
                }
            }
        }

        private void VisitorButton_Click(object sender, RoutedEventArgs e)
        {
            VisitorMode.Visibility = Visibility.Visible;
            AdminMode.Visibility = Visibility.Collapsed;
        }

        private void ManageSiteButton_Click(object sender, RoutedEventArgs e)
        {
            // Affiche une fenêtre de gestion des sites
            var SiteManagementWindow = new SiteManagementWindow();
            SiteManagementWindow.Show();
        }

        // Vérification sécurisée du nom d'utilisateur et du mot de passe dans la base de données
        private bool IsValidAdmin(string username, string password)
        {
            const string DB_NAME = "AnnuaireEntreprise";
            const string SERVER_NAME = "ZENBOOK13\\SQLEXPRESS";
            const string connectionString = $"Server={SERVER_NAME};Database={DB_NAME};Trusted_Connection=True;TrustServerCertificate=true";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM admin WHERE pseudo = @username AND pwd = @password", connection);
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password);

                int count = Convert.ToInt32(command.ExecuteScalar());

                return count == 1;
            }
        }
    }
}
