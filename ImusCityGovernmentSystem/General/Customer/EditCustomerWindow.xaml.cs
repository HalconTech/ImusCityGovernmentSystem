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
    /// Interaction logic for EditCustomerWindow.xaml
    /// </summary>
    public partial class EditCustomerWindow : MetroWindow
    {
        List<IdentificationCardModel> gd = new List<IdentificationCardModel>();
        public int id;
        public EditCustomerWindow()
        {
            InitializeComponent();
        }

        public void LoadIdentificationCards(int id)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                gd.Clear();
                db = new ImusCityHallEntities();
                var identicationCards = db.IdentificationCardTypes.Where(m => m.IsActive == true).OrderBy(m => m.CardType).ToList();
                foreach (var dr in identicationCards)
                {
                    bool selectedCard = false;
                    string cardNumber = string.Empty;
                    if (db.CustomerIdentificationCards.Any(m => m.CustomerID == id && m.IdentificationCardTypeID == dr.IdentificationCardTypeID))
                    {
                        selectedCard = true;
                        cardNumber = db.CustomerIdentificationCards.FirstOrDefault(m => m.CustomerID == id && m.IdentificationCardTypeID == dr.IdentificationCardTypeID).IdentificationNumber;
                    }
                    IdentificationCardModel i = new IdentificationCardModel()
                    {
                        Name = dr.CardType,
                        Id = dr.IdentificationCardTypeID,
                        IsSelected = selectedCard,
                        CardNumber = cardNumber
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
                LoadIdentificationCards(customer.CustomerID);
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
                if (String.IsNullOrEmpty(firstnametb.Text))
                {
                    MessageBox.Show("Please enter firs name");
                }
                else if (String.IsNullOrEmpty(lastnametb.Text))
                {
                    MessageBox.Show("Please enter last name");
                }
                else if (String.IsNullOrEmpty(bdaydp.Text))
                {
                    MessageBox.Show("Please enter birthdate");
                }
                else if (String.IsNullOrEmpty(compaddresstb.Text))
                {
                    MessageBox.Show("Please enter complete address");
                }
                else
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

                    foreach (var list in gd.Where(m => m.IsSelected == true))
                    {
                        if (db.CustomerIdentificationCards.Any(m => m.CustomerID == customer.CustomerID && m.IdentificationCardTypeID == list.Id))
                        {
                            CustomerIdentificationCard custCard = db.CustomerIdentificationCards.FirstOrDefault(m => m.CustomerID == customer.CustomerID && m.IdentificationCardTypeID == list.Id);
                            custCard.IdentificationNumber = list.CardNumber;
                            db.SaveChanges();
                        }
                        else
                        {
                            CustomerIdentificationCard custCard = new CustomerIdentificationCard();
                            IdentificationCardType card = db.IdentificationCardTypes.Find(list.Id);
                            custCard.CustomerID = customer.CustomerID;
                            custCard.IdentificationCardTypeID = list.Id;
                            custCard.IdentificationNumber = list.CardNumber;
                            db.CustomerIdentificationCards.Add(custCard);
                            db.SaveChanges();
                        }
                    }

                    foreach (var list in gd.Where(m => m.IsSelected == false))
                    {
                        if (db.CustomerIdentificationCards.Any(m => m.CustomerID == customer.CustomerID && m.IdentificationCardTypeID == list.Id))
                        {
                            CustomerIdentificationCard custCard = db.CustomerIdentificationCards.FirstOrDefault(m => m.CustomerID == customer.CustomerID && m.IdentificationCardTypeID == list.Id);
                            db.CustomerIdentificationCards.Remove(custCard);
                            db.SaveChanges();
                        }
                    }

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

            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }
    }
}
