using MahApps.Metro.Controls;
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

namespace ImusCityGovernmentSystem.General.EmployeeModule
{
    /// <summary>
    /// Interaction logic for EmployeeModuleWindow.xaml
    /// </summary>
    public partial class EmployeeModuleWindow : MetroWindow
    {
        public EmployeeModuleWindow()
        {
            InitializeComponent();
        }

        private void employeelistbtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            EmployeeListWindow employee = new EmployeeListWindow();
            Mouse.OverrideCursor = null;
            employee.ShowDialog();
        }

        private void addempbtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            AddNewEmployeeWindow addemp = new AddNewEmployeeWindow();
            Mouse.OverrideCursor = null;
            addemp.ShowDialog();
        }
    }
}
