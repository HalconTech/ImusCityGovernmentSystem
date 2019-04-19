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
namespace ImusCityGovernmentSystem.General.Customer
{
    /// <summary>
    /// Interaction logic for EditCustomerWindow.xaml
    /// </summary>
    public partial class EditCustomerWindow : MetroWindow
    {
        public int id;
        public EditCustomerWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                ImusCityGovernmentSystem.Model.Customer customer = db.Customers.Find(id);
                firstnametb.Text = customer.FirstName;
                middlenamebt.Text = customer.MiddleName;
                lastnametb.Text = customer.LastName;
                bdaydp.SelectedDate = customer.Birthdate;
                compaddresstb.Text = customer.CompleteAddress;
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }

        private void savebtn_Click(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                ImusCityGovernmentSystem.Model.Customer customer = db.Customers.Find(id);
                customer.FirstName = firstnametb.Text;
                customer.MiddleName = string.IsNullOrEmpty(middlenamebt.Text) ? null : middlenamebt.Text;
                customer.LastName = lastnametb.Text;
                customer.DateAdded = DateTime.Now;
                customer.AddedBy = App.EmployeeID;
                customer.CompleteAddress = compaddresstb.Text;
                customer.Birthdate = bdaydp.SelectedDate;
                customer.IsActive = true;
                db.SaveChanges();
                MessageBox.Show("Customer updated");

                var audit = new AuditTrailModel
                {
                    Activity = "Updated customer in the database. CUSTOMER ID: " + id.ToString(),
                    ModuleName = this.GetType().Name,
                    EmployeeID = App.EmployeeID
                };
                SystemClass.InsertLog(audit);
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }
    }
}
