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
    /// Logique d'interaction pour DepartementManagementWindow.xaml
    /// </summary>
    public partial class DepartementManagementWindow : Window
    {

        public ObservableCollection<Departement> DepartementList { get; set; }

        public DepartementManagementWindow()
        {
            InitializeComponent();
            DataContext = new DepartementViewModel();
        }

        private void EditDepartementButton_Click(object sender, RoutedEventArgs e)
        {
            Departement selectedDepartement = MaDataGrid.SelectedItem as Departement;

            if (selectedDepartement != null)
            {
                EditDepartementWiewModel editDepartementViewModel = new EditDepartementWiewModel(selectedDepartement);
                EditDepartementWindow editDepartementWindow = new EditDepartementWindow(editDepartementViewModel);

                // Écoute l'événement Closed pour mettre à jour la liste après la fermeture de la fenêtre d'édition.
                editDepartementWindow.Closed += (s, args) =>
                {
                    // Mets à jour la liste des sites après la fermeture de la fenêtre d'édition.
                    MaDataGrid.Items.Refresh();
                };

                editDepartementWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un site à éditer.");
            }
        }

        private void DeleteDepartementButton_Click(object sender, RoutedEventArgs e)
        {
            Departement selectedDepartement = MaDataGrid.SelectedItem as Departement;

            if (selectedDepartement != null)
            {
                DepartementViewModel viewModel = (DepartementViewModel)DataContext;

                // Appele la méthode de suppression de la ViewModel en passant le departement à supprimer.
                viewModel.DeleteDepartement(selectedDepartement);
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un site à supprimer.");
            }
        }

    }
}
