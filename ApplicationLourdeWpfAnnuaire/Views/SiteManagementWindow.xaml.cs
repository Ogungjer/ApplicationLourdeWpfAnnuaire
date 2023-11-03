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
    /// Logique d'interaction pour SiteManagementWindow.xaml
    /// </summary>
    public partial class SiteManagementWindow : Window
    {

        public ObservableCollection<Site> SiteList { get; set; }

        public SiteManagementWindow()
        {
            InitializeComponent();
            DataContext = new SiteViewModel();
        }

        private void EditSiteButton_Click(object sender, RoutedEventArgs e)
        {
            Site selectedSite = MaDataGrid.SelectedItem as Site;

            if (selectedSite != null)
            {
                EditSiteViewModel editSiteViewModel = new EditSiteViewModel(selectedSite);
                EditSiteWindow editSiteWindow = new EditSiteWindow(editSiteViewModel);

                // Écoute l'événement Closed pour mettre à jour la liste après la fermeture de la fenêtre d'édition.
                editSiteWindow.Closed += (s, args) =>
                {
                    // Mets à jour la liste des sites après la fermeture de la fenêtre d'édition.
                    MaDataGrid.Items.Refresh();
                };

                editSiteWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un site à éditer.");
            }
        }

        private void DeleteSiteButton_Click(object sender, RoutedEventArgs e)
        {
            Site selectedSite = MaDataGrid.SelectedItem as Site;

            if (selectedSite != null)
            {
                SiteViewModel viewModel = (SiteViewModel)DataContext;

                // Appele la méthode de suppression de la ViewModel en passant le site à supprimer.
                viewModel.DeleteSite(selectedSite);
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un site à supprimer.");
            }
        }






    }
}
