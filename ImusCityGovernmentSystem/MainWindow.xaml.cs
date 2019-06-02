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

            if (App.ByPass != true)
            {
                Employee employee = db.Employees.Find(App.EmployeeID);
                empname.Content = "Welcome! " + employee.FirstName + " " + employee.LastName;
            }

            var notif = db.GetCheckExpiryNotice().ToList();

            if (notif.Count != 0)
            {
                bdgNotif.Badge = notif.Count;
                btnNotif.Visibility = Visibility.Visible;
                lvNotif.ItemsSource = notif;

                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvNotif.ItemsSource);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("GroupName");
                view.GroupDescriptions.Add(groupDescription);
            }




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
        {
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
            if (db.LicensingCodes.Find(App.LicenseKey).IsDemo == true)
            {
                demotb.Visibility = Visibility.Visible;
            }
            CheckUserAccess();


        }

        public void CheckUserAccess()
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                IEnumerable<SubModuleUser> submodule = db.SubModuleUsers.Where(m => m.EmployeeID == App.EmployeeID);
                if (submodule.Count() >= 1)
                {
                    foreach (var module in submodule)
                    {
                        CDS.IsEnabled = module.SubModule.Acronym == CDS.Name ? true : false;
                    }
                }
                else
                {
                    modules.IsEnabled = false;
                }
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
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

        private void accessbtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            UserAccess access = new UserAccess();
            Mouse.OverrideCursor = null;
            access.ShowDialog();
        }

        private void btnNotif_Click(object sender, RoutedEventArgs e)
        {
            flyoutNotif.IsOpen = true;
        }

        private void customerbtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            ImusCityGovernmentSystem.General.Customer.CustomerListWindow customer = new General.Customer.CustomerListWindow();
            customer.Show();
            Mouse.OverrideCursor = null;
        }

        private void identicationcardbtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            ImusCityGovernmentSystem.General.IdentificationCard.IdentificationCardListWindow card = new General.IdentificationCard.IdentificationCardListWindow();
            card.Show();
            Mouse.OverrideCursor = null;
        }
    }
}
