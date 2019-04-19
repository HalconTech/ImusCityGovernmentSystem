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
    /// Interaction logic for CustomerListWindow.xaml
    /// </summary>
    public partial class CustomerListWindow : MetroWindow
    {
        public CustomerListWindow()
        {
            InitializeComponent();
        }

        private void addbtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            ImusCityGovernmentSystem.General.Customer.AddNewCustomerWindow add = new AddNewCustomerWindow();
            Mouse.OverrideCursor = null;
            add.ShowDialog();
            LoadCustomer(searchtb.Text);

        }
        public void LoadCustomer(string searchkey)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                List<CustomerEntity> customerList = new List<CustomerEntity>();
                if (!String.IsNullOrEmpty(searchtb.Text))
                {
                    var result = from p in db.Customers orderby p.FirstName where (p.FirstName.Contains(searchkey) || p.MiddleName.Contains(searchkey) || p.LastName.Contains(searchkey)) && p.IsActive == true select p;
                    foreach (var item in result)
                    {
                        var customer = new CustomerEntity
                        {
                            CustomerId = item.CustomerID,
                            Name = item.FirstName + " " + item.MiddleName + " " + item.LastName,
                            Birthdate = item.Birthdate.HasValue ? item.Birthdate.Value.ToShortDateString() : ""
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
                    var result = from p in db.Customers orderby p.FirstName where p.IsActive == true select p;
                    foreach (var item in result)
                    {
                        var customer = new CustomerEntity
                        {
                            CustomerId = item.CustomerID,
                            Name = item.FirstName + " " + item.MiddleName + " " + item.LastName,
                            Birthdate = item.Birthdate.HasValue ? item.Birthdate.Value.ToShortDateString() : ""
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

        private void searchtb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoadCustomer(searchtb.Text);
            }
        }

        private void searchbtn_Click(object sender, RoutedEventArgs e)
        {
            LoadCustomer(searchtb.Text);
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCustomer(searchtb.Text);
        }

        private void editbtn_Click(object sender, RoutedEventArgs e)
        {
            if (customerdg.SelectedValue == null)
            {
                MessageBox.Show("Please select a customer from the list");
            }
            else
            {
                ImusCityGovernmentSystem.General.Customer.EditCustomerWindow edit = new EditCustomerWindow();
                edit.id = (int)customerdg.SelectedValue;
                edit.ShowDialog();
                LoadCustomer(searchtb.Text);
            }
        }

        private void deletebtn_Click(object sender, RoutedEventArgs e)
        {
            if (customerdg.SelectedValue == null)
            {
                MessageBox.Show("Please select a customer from the list");
            }
            else
            {
                if (SystemClass.CheckConnection())
                {
                    int id = (int)customerdg.SelectedValue;
                    ImusCityHallEntities db = new ImusCityHallEntities();
                    ImusCityGovernmentSystem.Model.Customer customer = db.Customers.Find(id);
                    customer.IsActive = false;
                    db.SaveChanges();
                    MessageBox.Show("Customer removed");
                    LoadCustomer(searchtb.Text);
                }
                else
                {
                    MessageBox.Show(SystemClass.DBConnectionErrorMessage);
                }
            }
        }
    }
}
