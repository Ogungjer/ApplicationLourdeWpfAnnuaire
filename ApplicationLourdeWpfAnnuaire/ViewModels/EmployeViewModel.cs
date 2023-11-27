using ApplicationLourdeWpfAnnuaire.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using ApplicationLourdeWpfAnnuaire.Models;
using ApplicationLourdeWpfAnnuaire.Views;

namespace ApplicationLourdeWpfAnnuaire.ViewModels
{
    public class EmployeViewModel : ViewModelBase
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

        private ObservableCollection<Employe> _employeList;
        public ObservableCollection<Employe> EmployeList
        {
            get { return _employeList; }
            set
            {
                _employeList = value;
                OnPropertyChanged(nameof(EmployeList));
            }
        }

        //Propriété pour accéder au texte du TextBox
        private string _newEmployeNom;
        public string? NewEmployeNom
        {
            get { return _newEmployeNom; }
            set
            {
                _newEmployeNom = value;
                OnPropertyChanged(nameof(NewEmployeNom));
            }
        }

        private string _newEmployePrenom;
        public string NewEmployePrenom
        {
            get { return _newEmployePrenom; }
            set
            {
                _newEmployePrenom = value;
                OnPropertyChanged(nameof(NewEmployePrenom));
            }
        }

        private string _newEmployeTelephoneF;
        public string NewEmployeTelephoneF
        {
            get { return _newEmployeTelephoneF; }
            set
            {
                _newEmployeTelephoneF = value;
                OnPropertyChanged(nameof(NewEmployeTelephoneF));
            }
        }

        private string _newEmployeTelephoneP;
        public string NewEmployeTelephoneP
        {
            get { return _newEmployeTelephoneP; }
            set
            {
                _newEmployeTelephoneP = value;
                OnPropertyChanged(nameof(NewEmployeTelephoneP));
            }
        }

        private string _newEmployeEmail;
        public string NewEmployeEmail
        {
            get { return _newEmployeEmail; }
            set
            {
                _newEmployeEmail = value;
                OnPropertyChanged(nameof(NewEmployeEmail));
            }
        }

        private int _newEmployeSiteID;
        public int NewEmployeSiteID
        {
            get { return _newEmployeSiteID; }
            set
            {
                _newEmployeSiteID = value;
                OnPropertyChanged(nameof(NewEmployeSiteID));
            }
        }

        private int _newEmployeDepartementID;
        public int NewEmployeDepartementID
        {
            get { return _newEmployeDepartementID; }
            set
            {
                _newEmployeDepartementID = value;
                OnPropertyChanged(nameof(NewEmployeDepartementID));
            }
        }

        public ICommand AddEmployeCommand { get; set; }

        public ICommand RefreshCommand { get; }
 
        public EmployeViewModel()
        {

            LoadDepartements();

            LoadSites();

            EmployeList = new ObservableCollection<Employe>();

            LoadEmploye();

            AddEmployeCommand = new RelayCommand(AddEmploye);

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

        private async void LoadDepartements()
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

        private async void LoadEmploye()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {

                    string apiUrl = "https://localhost:7252/api/Employe";
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        List<Employe> employes = JsonConvert.DeserializeObject<List<Employe>>(result);

                        // Mets à jour la collection EmployeList avec les données chargées
                        foreach (var employe in employes)
                        {
                            EmployeList.Add(employe);
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
        private async Task AddEmploye()
        {
            try
            {
                // Crée un nouvel objet Employe avec les données de la TextBox
                string newNom = NewEmployeNom;
                string newPrenom = NewEmployePrenom;
                string newTelephoneF = NewEmployeTelephoneF;
                string newTelephoneP = NewEmployeTelephoneP;
                string newEmail = NewEmployeEmail;
                int newSite = NewEmployeSiteID;
                int newDepartement = NewEmployeDepartementID;

                Employe newEmploye = new Employe { 
                    Nom = newNom, 
                    Prenom = newPrenom,
                    TelephoneFixe = newTelephoneF,
                    TelephonePortable = newTelephoneP,
                    Email = newEmail,
                    SiteID = newSite,
                    DepartementID = newDepartement
                };

                // Envoie un nouvel objet Employé à l'API pour l'ajout
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = "https://localhost:7252/api/Employe";
                    string employeJson = JsonConvert.SerializeObject(newEmploye);
                    StringContent content = new StringContent(employeJson, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        // Si l'ajout à l'API est réussi, ajoute également l'employé à la collection
                        EmployeList.Add(newEmploye);
                        ShowSuccessMessage("L'employé a été bien ajouté.");

                    }
                    else
                    {
                        ShowErrorMessage("Impossible d'ajouter le site à l'API.");
                    }
                }

            }
            catch (Exception ex)
            {
                ShowErrorMessage("Une erreur s'est produite : " + ex.Message);
            }
        }

       

        public async Task DeleteEmploye(Employe employeToDelete)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = "https://localhost:7252/api/Employe/" + employeToDelete.EmployeID;

                    HttpResponseMessage response = await client.DeleteAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        EmployeList.Remove(employeToDelete);
                        ShowSuccessMessage("L'employé a été bien supprimé.");
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



        public async void SearchEmploye()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = "https://localhost:7252/api/Employe";
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        List<Employe> allEmployes = JsonConvert.DeserializeObject<List<Employe>>(result);

                        // Filtre la liste en fonction des critères sélectionnés
                        IEnumerable<Employe> filteredEmployes = allEmployes;

                        if (NewEmployeSiteID > 0)
                        {
                            filteredEmployes = filteredEmployes.Where(employe => employe.SiteID == NewEmployeSiteID);
                           
                        }

                        if (NewEmployeDepartementID > 0)
                        {
                            filteredEmployes = filteredEmployes.Where(employe => employe.DepartementID == NewEmployeDepartementID);
                        }

                        if (!string.IsNullOrEmpty(NewEmployeNom))
                        {
                            string nomEmployeToLower = NewEmployeNom.ToLower();
                            filteredEmployes = filteredEmployes.Where(employe => employe.Nom.ToLower().Contains(nomEmployeToLower) || employe.Prenom.ToLower().Contains(nomEmployeToLower));
                            
                        }

                        // Mets à jour la collection EmployeList avec les résultats de la recherche
                        EmployeList = new ObservableCollection<Employe>(filteredEmployes);
                    }
                    else
                    {
                        ShowErrorMessage("Impossible de récupérer les données de l'API.");
                    }
                }
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
                    string apiUrlEmploye = "https://localhost:7252/api/Employe";
                    string apiUrlDepartement = "https://localhost:7252/api/Departement";
                    string apiUrlSite = "https://localhost:7252/api/Site";

                    HttpResponseMessage responseEmploye = await client.GetAsync(apiUrlEmploye);
                    HttpResponseMessage responseDepartement = await client.GetAsync(apiUrlDepartement);
                    HttpResponseMessage responseSite = await client.GetAsync(apiUrlSite);

                    if (responseEmploye.IsSuccessStatusCode && responseDepartement.IsSuccessStatusCode && responseSite.IsSuccessStatusCode)
                    {
                        string resultEmploye = await responseEmploye.Content.ReadAsStringAsync();
                        string resultDepartement = await responseDepartement.Content.ReadAsStringAsync();
                        string resultSite= await responseSite.Content.ReadAsStringAsync();

                        List<Employe> employes = JsonConvert.DeserializeObject<List<Employe>>(resultEmploye);
                        EmployeList = new ObservableCollection<Employe>(employes);
                        NewEmployeNom = null;

                        List<Departement> departements = JsonConvert.DeserializeObject<List<Departement>>(resultDepartement);
                        DepartementList = new ObservableCollection<Departement>(departements);
                        NewEmployeDepartementID = 0;

                        List<Site> sites = JsonConvert.DeserializeObject<List<Site>>(resultSite);
                        SiteList = new ObservableCollection<Site>(sites);
                        NewEmployeSiteID = 0;

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
