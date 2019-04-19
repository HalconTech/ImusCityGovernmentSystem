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
using MahApps.Metro.Controls;

namespace ImusCityGovernmentSystem.General.Customer
{
    /// <summary>
    /// Interaction logic for AddNewCustomerWindow.xaml
    /// </summary>
    public partial class AddNewCustomerWindow : MetroWindow
    {
        public AddNewCustomerWindow()
        {
            InitializeComponent();
        }

        private void savebtn_Click(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                ImusCityGovernmentSystem.Model.Customer customer = new ImusCityGovernmentSystem.Model.Customer();
                customer.FirstName = firstnametb.Text;
                customer.MiddleName = string.IsNullOrEmpty(middlenamebt.Text) ? null : middlenamebt.Text;
                customer.LastName = lastnametb.Text;
                customer.DateAdded = DateTime.Now;
                customer.AddedBy = App.EmployeeID;
                customer.CompleteAddress = compaddresstb.Text;
                customer.Birthdate = bdaydp.SelectedDate;
                customer.IsActive = true;
                db.Customers.Add(customer);
                db.SaveChanges();
                MessageBox.Show("Customer added");

                var audit = new AuditTrailModel
                {
                    Activity = "Added new customer in the database CUSTOMER NAME: " + string.Join(" ", customer.FirstName, customer.MiddleName, customer.LastName),
                    ModuleName = this.GetType().Name,
                    EmployeeID = App.EmployeeID
                };
                SystemClass.InsertLog(audit);
                ClearTextBox();
                
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }

        public void ClearTextBox()
        {
            firstnametb.Clear();
            middlenamebt.Clear();
            lastnametb.Clear();
            bdaydp.Text = null;
            compaddresstb.Clear();
        }
    }
}
