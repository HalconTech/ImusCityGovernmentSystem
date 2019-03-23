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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using ImusCityGovernmentSystem.Model;
namespace ImusCityGovernmentSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        ImusCityHallEntities db = new ImusCityHallEntities();
        public string Password;
        public MainWindow()
        {
            InitializeComponent();
            Employee employee = db.Employees.Find(App.EmployeeID);
            empname.Content = "Welcome! " + employee.FirstName + " " + employee.LastName;

            
            bdgNotif.Badge = 3;
        }

        private void empmgntbtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            General.EmployeeModule.EmployeeListWindow emplist = new General.EmployeeModule.EmployeeListWindow();
            emplist.ShowDialog();
            Mouse.OverrideCursor = null;
        }

        private void btnlogout_Click(object sender, RoutedEventArgs e)
        {
            LogInWindow lw = new LogInWindow();
            lw.Show();
            this.Close();
           
        }

        private void payeebtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            General.Payee.PayeeListWindow payee = new General.Payee.PayeeListWindow();
            Mouse.OverrideCursor = null;
            payee.ShowDialog();
          
        }

        private void fundbtn_Click(object sender, RoutedEventArgs e)
        {;
            Mouse.OverrideCursor = Cursors.Wait;
            General.Fund.FundListWindow fund = new General.Fund.FundListWindow();
            Mouse.OverrideCursor = null;
            fund.ShowDialog();
        }

        private void checkdisbursementbtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            ImusCityGovernmentSystem.Check_Disbursement.CheckDisbursementWindow check = new Check_Disbursement.CheckDisbursementWindow();
            Mouse.OverrideCursor = null;
            check.ShowDialog();         
        }

        private void departmentbtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            General.Department.DepartmentListWindow dept = new General.Department.DepartmentListWindow();
            dept.Show();
            Mouse.OverrideCursor = null;
        }

        private void divisionbtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            General.Division.DivisionListWindow division = new General.Division.DivisionListWindow();
            division.Show();
            Mouse.OverrideCursor = null;
        }

        private void positionbtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            General.Position.PositionWindowList position = new General.Position.PositionWindowList();
            position.Show();
            Mouse.OverrideCursor = null;
        }

        private void ranktbn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            General.Rank.RankListWindow rank = new General.Rank.RankListWindow();
            rank.Show();
            Mouse.OverrideCursor = null;
        }

        private void statusbtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            General.Rank.Status.StatusListWindow status = new General.Rank.Status.StatusListWindow();
            status.Show();
            Mouse.OverrideCursor = null;
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
           if(db.LicensingCodes.Find(App.LicenseKey).IsDemo == true)
           {
               demotb.Visibility = Visibility.Visible;
           }
        }

        private void addempbtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            General.EmployeeModule.AddNewEmployeeWindow addemp = new General.EmployeeModule.AddNewEmployeeWindow();
            addemp.Show();
            Mouse.OverrideCursor = null;
        }

        private void empmanagebtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            General.EmployeeModule.EmployeeModuleWindow employee = new General.EmployeeModule.EmployeeModuleWindow();
            Mouse.OverrideCursor = null;
            employee.ShowDialog();
        }

        private void btnNotif_Click(object sender, RoutedEventArgs e)
        {
            flyoutNotif.IsOpen = true;
        }
    }
}
