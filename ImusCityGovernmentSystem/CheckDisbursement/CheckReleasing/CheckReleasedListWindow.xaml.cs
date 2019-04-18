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
using ImusCityGovernmentSystem.Model;
using ImusCityGovernmentSystem.CheckDisbursement.CheckReleasing.Model;
using MahApps.Metro.Controls;

namespace ImusCityGovernmentSystem.CheckDisbursement.CheckReleasing
{
    /// <summary>
    /// Interaction logic for CheckReleasedListWindow.xaml
    /// </summary>
    public partial class CheckReleasedListWindow : MetroWindow
    {
        public CheckReleasedListWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadReleasedChecks(searchkeytb.Text);
        }

        public void LoadReleasedChecks(string id)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                List<CheckReleasedListingModel> released = new List<CheckReleasedListingModel>();
                if (String.IsNullOrEmpty(id))
                {
                    List<CheckRelease> checkreleasedlist = db.CheckReleases.ToList();
                    foreach (var item in checkreleasedlist)
                    {
                        var check = new CheckReleasedListingModel
                        {
                            CheckNumber = item.Check.CheckNo,
                            VoucherNumber = item.Check.Disbursement.VoucherNo,
                            Name = string.Join(" ", item.Customer.FirstName, item.Customer.MiddleName, item.Customer.LastName),
                            DateReleased = item.ReleasedDate.Value.ToShortDateString(),
                            BankName = item.Check.Disbursement.FundBank.Bank.BankCode,
                            ReleasedId = item.CheckReleaseID
                        };
                        released.Add(check);
                    }
                }
                else
                {
                    List<CheckRelease> checkreleasedlist = db.CheckReleases
                        .Where(m => m.Check.CheckNo.Contains(id) || m.Check.Disbursement.VoucherNo.Contains(id))
                        .ToList();
                    foreach (var item in checkreleasedlist)
                    {
                        var check = new CheckReleasedListingModel
                        {
                            CheckNumber = item.Check.CheckNo,
                            VoucherNumber = item.Check.Disbursement.VoucherNo,
                            Name = string.Join(" ", item.Customer.FirstName, item.Customer.MiddleName, item.Customer.LastName),
                            DateReleased = item.ReleasedDate.Value.ToShortDateString(),
                            BankName = item.Check.Disbursement.FundBank.Bank.BankCode,
                            ReleasedId = item.CheckReleaseID
                        };
                        released.Add(check);
                    }

                    var audit = new AuditTrailModel
                    {
                        Activity = "Searched item in released check list. SEARCH KEY: " + id,
                        ModuleName = this.GetType().Name,
                        EmployeeID = App.EmployeeID
                    };

                    SystemClass.InsertLog(audit);
                }

                releasedcheckdg.ItemsSource = released;
                releasedcheckdg.SelectedValuePath = "ReleasedId";
                Mouse.OverrideCursor = null;
            }
            else
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
            Mouse.OverrideCursor = null;
        }

        private void searchbtn_Click(object sender, RoutedEventArgs e)
        {
            LoadReleasedChecks(searchkeytb.Text);
        }

        private void searchkeytb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                searchbtn_Click(sender, e);
            }
        }

        private void viewbtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if(releasedcheckdg.SelectedValue != null)
            {
                ImusCityGovernmentSystem.CheckDisbursement.CheckReleasing.ViewCheckReleasedWindow view = new ViewCheckReleasedWindow();
                view.id = (int)releasedcheckdg.SelectedValue;
                Mouse.OverrideCursor = null;
                view.Show();
            }
        }
    }
}
