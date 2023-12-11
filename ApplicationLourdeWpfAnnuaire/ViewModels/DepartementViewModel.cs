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
using System.ComponentModel;

namespace ApplicationLourdeWpfAnnuaire.ViewModels
{
    public class DepartementViewModel : ViewModelBase
    {
        private ObservableCollection<Departement> _departementList;
        public ObservableCollection<Departement> DepartementList
        {
            get { return _departementList; }
            set
            {
                _departementList = value;
                OnPropertyChanged(nameof(DepartementList));
            }
        }

        // Propriété pour accéder au texte du TextBox
        private string _newDepartementNom;
        public string NewDepartementNom
        {
            get { return _newDepartementNom; }
            set
            {
                _newDepartementNom = value;
                OnPropertyChanged(nameof(NewDepartementNom));
            }
        }

        public ICommand AddDepartementCommand { get; set; }

        public ICommand RefreshCommand { get; }
        public DepartementViewModel()
        {
            DepartementList = new ObservableCollection<Departement>();

            LoadDepartement();

            AddDepartementCommand = new RelayCommand(AddDepartement);

            RefreshCommand = new RelayCommand(ExecuteRefreshCommand);

        }

        private async void LoadDepartement()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {

                    string apiUrl = "https://localhost:7252/api/Departement";
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        List<Departement> departements = JsonConvert.DeserializeObject<List<Departement>>(result);

                        // Mets à jour la collection DepartementList avec les données chargées
                        foreach (var departement in departements)
                        {
                            DepartementList.Add(departement);
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

        private async Task AddDepartement()
        {
            try
            {
                // Crée un nouvel objet Departement avec les données de la TextBox
                string newDepartementNom = NewDepartementNom;
                Departement newDepartement = new Departement { NomDepartement = newDepartementNom };

                // Envoie un nouvel objet Departement à l'API pour l'ajout
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = "https://localhost:7252/api/Departement";
                    string departementJson = JsonConvert.SerializeObject(newDepartement);
                    StringContent content = new StringContent(departementJson, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        // Si l'ajout à l'API est réussi, ajoute également le departement à la collection
                        DepartementList.Add(newDepartement);
                        ShowSuccessMessage("Le nouveau departement a été bien ajouté.");
                    }
                    else
                    {
                        ShowErrorMessage("Un ou plusieurs salariés sont déjà affectés à ce département.");
                    }
                }

                // Efface le champ de saisie après l'ajout
                NewDepartementNom = "";
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Une erreur s'est produite : " + ex.Message);
            }
        }

        public async Task DeleteDepartement(Departement departementToDelete)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = "https://localhost:7252/api/Departement/" + departementToDelete.DepartementID;

                    HttpResponseMessage response = await client.DeleteAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        DepartementList.Remove(departementToDelete);
                        ShowSuccessMessage("Le departement a été bien supprimé.");
                    }
                    else
                    {
                        ShowErrorMessage("Un ou plusieurs salariés sont déjà affectés à ce département.");
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
                    string apiUrl = "https://localhost:7252/api/Departement";
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        List<Departement> departements = JsonConvert.DeserializeObject<List<Departement>>(result);
                        DepartementList = new ObservableCollection<Departement>(departements);
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
