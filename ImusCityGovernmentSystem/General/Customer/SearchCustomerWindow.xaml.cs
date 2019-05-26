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
using ImusCityGovernmentSystem.Model;
using ImusCityGovernmentSystem.General.Customer.Model;
namespace ImusCityGovernmentSystem.General.Customer
{
    /// <summary>
    /// Interaction logic for SearchCustomerWindow.xaml
    /// </summary>
    public partial class SearchCustomerWindow : MetroWindow
    {
        public SearchCustomerWindow()
        {
            InitializeComponent();
            searchtb.Focus();
        }

        public void CustomerFinder(string searchkey)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                List<CustomerEntity> customerList = new List<CustomerEntity>();
                var result = from p in db.Customers orderby p.FirstName where p.IsActive == true select p;
                if (!String.IsNullOrEmpty(searchtb.Text))
                {
                    foreach (var item in result.Where(m => m.FirstName.Contains(searchkey) || m.MiddleName.Contains(searchkey) || m.LastName.Contains(searchkey)))
                    {
                        var customer = new CustomerEntity
                        {
                            CustomerId = item.CustomerID,
                            Name = item.FirstName + " " + item.MiddleName + " " + item.LastName,
                            DateAdded = item.DateAdded.Value.ToShortDateString()
                        };
                        customerList.Add(customer);
                    }
                    var audit = new AuditTrailModel
                    {
                        Activity = "Searched item in customer list. SEARCH KEY: " + searchkey,
                        ModuleName = this.GetType().Name,
                        EmployeeID = App.EmployeeID
                    };
                    SystemClass.InsertLog(audit);
                }
                else
                {
                    foreach (var item in result)
                    {
                        var customer = new CustomerEntity
                        {
                            CustomerId = item.CustomerID,
                            Name = item.FirstName + " " + item.MiddleName + " " + item.LastName,
                            DateAdded = item.DateAdded.Value.ToShortDateString()
                        };
                        customerList.Add(customer);
                    }
                }
                customerdg.ItemsSource = customerList;
                customerdg.SelectedValuePath = "CustomerId";
            }
            else
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
            Mouse.OverrideCursor = null;
        }

        public bool? ShowDialog(out int? customerId)
        {
            using (var db = new ImusCityHallEntities())
            {
                var x = ShowDialog();
                customerId = x == true ? (int)customerdg.SelectedValue : (int?)null;
                return x;
            }
        }

        private void customerdg_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (customerdg.SelectedItem != null)
            {
                this.DialogResult = true;
                this.Close();
            }
            else
            {

            }
        }

        private void customerdg_KeyDown(object sender, KeyEventArgs e)
        {
            if (customerdg.SelectedItem != null)
            {
                this.DialogResult = true;
                this.Close();
            }
            else
            {

            }
        }

        private void MetroWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        private void searchbtn_Click(object sender, RoutedEventArgs e)
        {
            CustomerFinder(searchtb.Text);
        }

        private void searchtb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                CustomerFinder(searchtb.Text);

        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            CustomerFinder(null);
        }
    }
}
