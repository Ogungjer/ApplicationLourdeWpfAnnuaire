using ApplicationLourdeWpfAnnuaire.ViewModels;
using System;
using System.Collections.Generic;
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
    /// Logique d'interaction pour AddEmployeWindow.xaml
    /// </summary>
    public partial class AddEmployeWindow : Window
    {
        public AddEmployeWindow()
        {
            InitializeComponent();
            DataContext = new EmployeViewModel();
        }

    }
}
