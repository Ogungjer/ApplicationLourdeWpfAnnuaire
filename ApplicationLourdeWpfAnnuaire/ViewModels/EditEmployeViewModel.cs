using ApplicationLourdeWpfAnnuaire.Commands;
using ApplicationLourdeWpfAnnuaire.Models;
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

namespace ApplicationLourdeWpfAnnuaire.ViewModels
{
    public class EditEmployeViewModel : ViewModelBase
    {

        private Employe _employe;
        public Employe Employe
        {
            get { return _employe; }
            set
            {
                _employe = value;
                OnPropertyChanged(nameof(Employe));
            }
        }


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

        

        //Propriété pour accéder au texte du TextBox
        private string _employeNom;
        public string EmployeNom
        {
            get { return _employeNom; }
            set
            {
                _employeNom = value;
                OnPropertyChanged(nameof(EmployeNom));
            }
        }

        private string _employePrenom;
        public string EmployePrenom
        {
            get { return _employePrenom; }
            set
            {
                _employePrenom = value;
                OnPropertyChanged(nameof(EmployePrenom));
            }
        }

        private string _employeTelephoneF;
        public string EmployeTelephoneF
        {
            get { return _employeTelephoneF; }
            set
            {
                _employeTelephoneF = value;
                OnPropertyChanged(nameof(EmployeTelephoneF));
            }
        }

        private string _employeTelephoneP;
        public string EmployeTelephoneP
        {
            get { return _employeTelephoneP; }
            set
            {
                _employeTelephoneP = value;
                OnPropertyChanged(nameof(EmployeTelephoneP));
            }
        }

        private string _employeEmail;
        public string EmployeEmail
        {
            get { return _employeEmail; }
            set
            {
                _employeEmail = value;
                OnPropertyChanged(nameof(EmployeEmail));
            }
        }

        private int _employeSiteID;
        public int EmployeSiteID
        {
            get { return _employeSiteID; }
            set
            {
                _employeSiteID = value;
                OnPropertyChanged(nameof(EmployeSiteID));
            }
        }

        private int _employeDepartementID;
        public int EmployeDepartementID
        {
            get { return _employeDepartementID; }
            set
            {
                _employeDepartementID = value;
                OnPropertyChanged(nameof(EmployeDepartementID));
            }
        }
        public ICommand EditEmployeCommand { get; set; }


        public EditEmployeViewModel(Employe employe)
        {
            LoadDepartements();
            LoadSites();

            Employe = employe;
            EmployeNom = employe.Nom;
            EmployePrenom = employe.Prenom;
            EmployeTelephoneF = employe.TelephoneFixe;
            EmployeTelephoneP = employe.TelephonePortable;
            EmployeEmail = employe.Email;
            EmployeSiteID = employe.SiteID;
            EmployeDepartementID= employe.DepartementID;

            EditEmployeCommand = new RelayCommand(async () => await EditChanges());

        }


        private async Task EditChanges()
        {
            try
            {
                Employe.Nom = EmployeNom;
                Employe.Prenom = EmployePrenom;
                Employe.TelephoneFixe = EmployeTelephoneF;
                Employe.TelephonePortable = EmployeTelephoneP;
                Employe.Email = EmployeEmail;
                Employe.SiteID = EmployeSiteID;
                Employe.DepartementID = EmployeDepartementID;

                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = "https://localhost:7252/api/Employe/" + Employe.EmployeID;
                    string employeJson = JsonConvert.SerializeObject(Employe);
                    StringContent content = new StringContent(employeJson, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PutAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        ShowSuccessMessage("Les Informations de L'Employé ont été bien mise à jour.");
                    }
                    else
                    {
                        ShowErrorMessage("Impossible de mettre à jour le l'employé à l'API.");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Une erreur s'est produite : " + ex.Message);
            }
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

