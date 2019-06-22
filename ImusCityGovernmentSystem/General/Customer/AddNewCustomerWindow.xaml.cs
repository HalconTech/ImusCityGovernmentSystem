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
using ImusCityGovernmentSystem.General.Customer.Model;
namespace ImusCityGovernmentSystem.General.Customer
{
    /// <summary>
    /// Interaction logic for AddNewCustomerWindow.xaml
    /// </summary>
    public partial class AddNewCustomerWindow : MetroWindow
    {
        List<IdentificationCardModel> gd = new List<IdentificationCardModel>();
        public AddNewCustomerWindow()
        {
            InitializeComponent();
        }

        public void LoadIdentificationCards()
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                gd.Clear();
                db = new ImusCityHallEntities();
                var identificationCards = db.IdentificationCardTypes.Where(m => m.IsActive == true).OrderBy(m => m.CardType).ToList();
                foreach (var dr in identificationCards)
                {
                    IdentificationCardModel i = new IdentificationCardModel()
                    {
                        Name = dr.CardType.ToString(),
                        Id = dr.IdentificationCardTypeID
                    };
                    gd.Add(i);
                }

                customercardlb.ItemsSource = gd;
                customercardlb.SelectedValuePath = "Id";
                customercardlb.Items.Refresh();
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
                if(String.IsNullOrEmpty(firstnametb.Text))
                {
                    MessageBox.Show("Please enter first name");
                }
                else if(String.IsNullOrEmpty(lastnametb.Text))
                {
                    MessageBox.Show("Please enter last name");
                }
                else if(String.IsNullOrEmpty(bdaydp.Text))
                {
                    MessageBox.Show("Please enter birthdate");
                }
                else if(String.IsNullOrEmpty(compaddresstb.Text))
                {
                    MessageBox.Show("Please enter complete address");
                }
                else
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

                 
                    foreach (var list in gd.Where(m => m.IsSelected == true))
                    {
                        CustomerIdentificationCard custCard = new CustomerIdentificationCard();
                        IdentificationCardType card = db.IdentificationCardTypes.Find(list.Id);
                        custCard.CustomerID = customer.CustomerID;
                        custCard.IdentificationCardTypeID = list.Id;
                        custCard.IdentificationNumber = list.CardNumber;
                        db.CustomerIdentificationCards.Add(custCard);
                    }
                    db.SaveChanges();
                    MessageBox.Show("Customer added");

                    var audit = new AuditTrailModel
                    {
                        Activity = "Added new customer in the database CUSTOMER NAME: " + string.Join(" ", customer.FirstName, customer.MiddleName, customer.LastName),
                        ModuleName = this.GetType().Name,
                        EmployeeID = App.EmployeeID
                    };
                    SystemClass.InsertLog(audit);
                    LoadIdentificationCards();
                    ClearTextBox();
                }
              
                
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

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadIdentificationCards();
        }
    }
}
