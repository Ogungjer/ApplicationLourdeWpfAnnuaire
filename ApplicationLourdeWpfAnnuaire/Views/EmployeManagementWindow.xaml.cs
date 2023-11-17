using ApplicationLourdeWpfAnnuaire.Models;
using ApplicationLourdeWpfAnnuaire.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Logique d'interaction pour EmployeManagementWindow.xaml
    /// </summary>
    public partial class EmployeManagementWindow : Window
    {
        public ObservableCollection<Employe> EmployeList { get; set; }
        public EmployeManagementWindow()
        {
            InitializeComponent();
            DataContext = new EmployeViewModel();

        }

        private void AddEmployeWindowButton_Click(object sender, RoutedEventArgs e)
        {
            // Affiche une fenêtre d'ajout d'employé
            var AddEmployeWindow = new AddEmployeWindow();
            AddEmployeWindow.Show();
        }

        private void EditEmployeButton_Click(object sender, RoutedEventArgs e)
        {
            Employe selectedEmploye = MaDataGrid.SelectedItem as Employe;

            if (selectedEmploye != null)
            {
                EditEmployeViewModel editEmployeViewModel = new EditEmployeViewModel(selectedEmploye);
                EditEmployeWindow editEmployeWindow = new EditEmployeWindow(editEmployeViewModel);

                // Écoute l'événement Closed pour mettre à jour la liste après la fermeture de la fenêtre d'édition.
                editEmployeWindow.Closed += (s, args) =>
                {
                    // Mets à jour la liste des employe après la fermeture de la fenêtre d'édition.
                    MaDataGrid.Items.Refresh();
                };

                editEmployeWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un site à éditer.");
            }
        }

        private void DeleteEmployeButton_Click(object sender, RoutedEventArgs e)
        {
            Employe selectedEmploye = MaDataGrid.SelectedItem as Employe;

            if (selectedEmploye != null)
            {
                EmployeViewModel viewModel = (EmployeViewModel)DataContext;

                // Appele la méthode de suppression de la ViewModel en passant l'employé à supprimer.
                viewModel.DeleteEmploye(selectedEmploye);
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un site à supprimer.");
            }
        }
    }
}
