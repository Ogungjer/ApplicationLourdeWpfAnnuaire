using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ApplicationLourdeWpfAnnuaire.Commands;
using ApplicationLourdeWpfAnnuaire.Models;
using ApplicationLourdeWpfAnnuaire.ViewModels;
using Newtonsoft.Json;

public class EditDepartementWiewModel : ViewModelBase
{
    private Departement _departement;
    public Departement Departement
    {
        get { return _departement; }
        set
        {
            _departement = value;
            OnPropertyChanged(nameof(Departement));
        }
    }

    private string _departementName;
    public string DepartementName
    {
        get { return _departementName; }
        set
        {
            _departementName = value;
            OnPropertyChanged(nameof(DepartementName));
        }
    }

    public ICommand EditCommand { get; set; }

    public EditDepartementWiewModel (Departement departement)
    {
        Departement = departement;
        DepartementName = departement.NomDepartement;
        EditCommand = new RelayCommand(async () => await EditChanges());
    }

    private async Task EditChanges()
    {
        try
        {
            Departement.NomDepartement = DepartementName;

            using (HttpClient client = new HttpClient())
            {
                string apiUrl = "https://localhost:7252/api/Departement/" + Departement.DepartementID;
                string departementJson = JsonConvert.SerializeObject(Departement);
                StringContent content = new StringContent(departementJson, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    ShowSuccessMessage("Le Nom du departement a été bien modifié.");
                }
                else
                {
                    ShowErrorMessage("Impossible de mettre à jour le departement à l'API.");
                }
            }
        }
        catch (Exception ex)
        {
            ShowErrorMessage("Une erreur s'est produite : " + ex.Message);
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
