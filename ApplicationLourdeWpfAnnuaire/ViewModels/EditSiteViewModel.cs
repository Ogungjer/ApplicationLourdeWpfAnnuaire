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

public class EditSiteViewModel : ViewModelBase
{
    private Site _site;
    public Site Site
    {
        get { return _site; }
        set
        {
            _site = value;
            OnPropertyChanged(nameof(Site));
        }
    }

    private string _siteName;
    public string SiteName
    {
        get { return _siteName; }
        set
        {
            _siteName = value;
            OnPropertyChanged(nameof(SiteName));
        }
    }

    public ICommand EditCommand { get; set; }

    public EditSiteViewModel(Site site)
    {
        Site = site;
        SiteName = site.Ville;
        EditCommand = new RelayCommand(async () => await EditChanges());
    }

    private async Task EditChanges()
    {
        try
        {
            Site.Ville = SiteName;

            using (HttpClient client = new HttpClient())
            {
                string apiUrl = "https://localhost:7252/api/Site/" + Site.SiteID;
                string siteJson = JsonConvert.SerializeObject(Site);
                StringContent content = new StringContent(siteJson, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    ShowSuccessMessage("Le Nom du site a été bien modifié.");
                }
                else
                {
                    ShowErrorMessage("Un ou plusieurs salariés sont déjà affectés à ce site.");
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
