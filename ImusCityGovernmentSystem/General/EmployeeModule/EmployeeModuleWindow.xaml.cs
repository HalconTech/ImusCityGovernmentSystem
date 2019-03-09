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

        private void departmentbtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            Department.DepartmentListWindow department = new Department.DepartmentListWindow();
            Mouse.OverrideCursor = null;
            department.ShowDialog();
        }

        private void divionbtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            Division.DivisionListWindow division = new Division.DivisionListWindow();
            Mouse.OverrideCursor = null;
            division.ShowDialog();
        }

        private void positionbtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            Position.PositionWindowList postion = new Position.PositionWindowList();
            Mouse.OverrideCursor = null;
            postion.ShowDialog();
        }

        private void rankbtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            Rank.RankListWindow rank = new Rank.RankListWindow();
            Mouse.OverrideCursor = null;
            rank.ShowDialog();
        }

        private void statusbtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            Rank.Status.StatusListWindow status = new Rank.Status.StatusListWindow();
            Mouse.OverrideCursor = null;
            status.ShowDialog();
        }
    }
}
