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
    /// Interaction logic for CustomerMatchedWindow.xaml
    /// </summary>
    public partial class CustomerMatchedWindow : MetroWindow
    {
        public string fname, mname, lname;
        public CustomerMatchedWindow()
        {
            InitializeComponent();
        }

        public void CustomerFinder(string fname, string mname, string lname)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                List<CustomerEntity> customerList = new List<CustomerEntity>();
                var result = from p in db.Customers orderby p.FirstName where (p.FirstName.Contains(fname) || p.MiddleName.Contains(mname) || p.LastName.Contains(lname)) || p.IsActive == true select p;
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

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            CustomerFinder(fname, mname, lname);
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
        public bool? ShowDialog(out int? customerId)
        {
            using (var db = new ImusCityHallEntities())
            {
                var x = ShowDialog();
                customerId = x == true ? (int)customerdg.SelectedValue : (int?)null;
                return x;
            }
        }
    }
}
