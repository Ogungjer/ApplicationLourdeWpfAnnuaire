using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ApplicationLourdeWpfAnnuaire.Models;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using ApplicationLourdeWpfAnnuaire.Commands;
using System.Windows.Input;
using ApplicationLourdeWpfAnnuaire.Views;

namespace ApplicationLourdeWpfAnnuaire.ViewModels
{
    public class SiteViewModel : ViewModelBase
    {
        private ObservableCollection<Site> _siteList;
        public ObservableCollection<Site> SiteList
        {
            get { return _siteList; }
            set
            {
                _siteList = value;
                OnPropertyChanged(nameof(SiteList));
            }
        }

        // Propriété pour accéder au texte du TextBox
        private string _newSiteVille;
        public string NewSiteVille
        {
            get { return _newSiteVille; }
            set
            {
                _newSiteVille = value;
                OnPropertyChanged(nameof(NewSiteVille));
            }
        }

        public ICommand AddSiteCommand { get; set; }

        public ICommand RefreshCommand { get; }

        public SiteViewModel()
        {
            SiteList = new ObservableCollection<Site>(); 

            LoadSites();

            AddSiteCommand = new RelayCommand(AddSite);

            RefreshCommand = new RelayCommand(ExecuteRefreshCommand);

        }

        private async void LoadSites()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
      
                    string apiUrl = "https://localhost:7252/api/Site";
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        List<Site> sites = JsonConvert.DeserializeObject<List<Site>>(result);

                        // Mets à jour la collection SiteList avec les données chargées
                        foreach (var site in sites)
                        {
                            SiteList.Add(site);
                        }
                    }
                    else
                    {
                        ShowErrorMessage("Impossible de récupérer les données de l'API.");
                    }
                }
                catch (Exception ex)
                {
                   ShowErrorMessage("Une erreur s'est produite : " + ex.Message);
                }
            }
        }

        private async Task AddSite()
        {
            try
            {
                // Crée un nouvel objet Site avec les données de la TextBox
                string newVille = NewSiteVille;
                Site newSite = new Site { Ville = newVille };

                // Envoie un nouvel objet Site à l'API pour l'ajout
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = "https://localhost:7252/api/Site"; 
                    string siteJson = JsonConvert.SerializeObject(newSite);
                    StringContent content = new StringContent(siteJson, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        // Si l'ajout à l'API est réussi, ajoute également le site à la collection
                        SiteList.Add(newSite);
                        ShowSuccessMessage("Le nouveau site a été bien ajouté.");
                    }
                    else
                    {
                        ShowErrorMessage("Un ou plusieurs salariés sont déjà affectés à ce site.");
                    }
                }

                // Efface le champ de saisie après l'ajout
                NewSiteVille = "";
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Une erreur s'est produite : " + ex.Message);
            }
        }

        public async Task DeleteSite(Site siteToDelete)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = "https://localhost:7252/api/Site/" + siteToDelete.SiteID;

                    HttpResponseMessage response = await client.DeleteAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        SiteList.Remove(siteToDelete);
                        ShowSuccessMessage("Le site a été bien supprimé.");
                    }
                    else
                    {
                        ShowErrorMessage("Un ou plusieurs salariés sont déjà affectés à ce site.");
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                ShowErrorMessage("Une erreur de requête HTTP s'est produite : " + ex.Message);
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Une erreur s'est produite : " + ex.Message);
            }
        }

        private async Task ExecuteRefreshCommand()
        {
           
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string apiUrl = "https://localhost:7252/api/Site";
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        List<Site> sites = JsonConvert.DeserializeObject<List<Site>>(result);
                        SiteList = new ObservableCollection<Site>(sites);
                    }
                    else
                    {
                        ShowErrorMessage("Impossible de récupérer les données de l'API.");
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("Une erreur s'est produite : " + ex.Message);
                }
            }

        }


        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ShowSuccessMessage(string message)
        {
            MessageBox.Show(message, "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
