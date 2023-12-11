using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ApplicationLourdeWpfAnnuaire
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Crée la fenêtre principale (MainWindow)
            MainWindow mainWindow = new MainWindow();

            // Démarre l'application en affichant la fenêtre principale
            mainWindow.Show();
        }
    }
}
