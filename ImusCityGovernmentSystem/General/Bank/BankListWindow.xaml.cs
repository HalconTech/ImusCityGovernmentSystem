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
using ImusCityGovernmentSystem.General.Bank;
namespace ImusCityGovernmentSystem.General.Bank
{
    /// <summary>
    /// Interaction logic for BankListWindow.xaml
    /// </summary>
    public partial class BankListWindow : MetroWindow
    {
        public BankListWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                banklb.ItemsSource = db.Banks.Where(m => m.IsActive == true).OrderByDescending(m => m.BankID).ToList();
                banklb.DisplayMemberPath = "BankName";
                banklb.SelectedValuePath = "BankID";
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }

        private void searchtb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (String.IsNullOrEmpty(searchtb.Text))
                {

                }
                else
                {
                    var audit = new AuditTrailModel
                    {
                        Activity = "Searched item in fund list. SEARCH KEY: " + searchtb.Text,
                        ModuleName = this.GetType().Name,
                        EmployeeID = App.EmployeeID
                    };

                    SystemClass.InsertLog(audit);
                }

                searchbtn_Click(sender, e);
            }
        }

        private void searchbtn_Click(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                if (String.IsNullOrEmpty(searchtb.Text))
                {

                }
                else
                {
                    var audit = new AuditTrailModel
                    {
                        Activity = "Searched item in fund list. SEARCH KEY: " + searchtb.Text,
                        ModuleName = this.GetType().Name,
                        EmployeeID = App.EmployeeID
                    };

                    SystemClass.InsertLog(audit);
                }

                ImusCityHallEntities db = new ImusCityHallEntities();
                banklb.ItemsSource = db.Banks.Where(m => m.IsActive == true && m.BankName.Contains(searchtb.Text)).OrderByDescending(m => m.BankID).ToList();
                banklb.DisplayMemberPath = "BankName";
                banklb.SelectedValuePath = "BankID";
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }

        private void deletebtn_Click(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                if (banklb.SelectedValue != null)
                {
                    ImusCityHallEntities db = new ImusCityHallEntities();
                    int id = (int)banklb.SelectedValue;
                    ImusCityGovernmentSystem.Model.Bank bank = db.Banks.Find(id);
                    bank.IsActive = false;
                    db.SaveChanges();
                    db = new ImusCityHallEntities();
                    banklb.ItemsSource = db.Banks.Where(m => m.IsActive == true).OrderByDescending(m => m.BankID).ToList();
                    banklb.DisplayMemberPath = "BankName";
                    banklb.SelectedValuePath = "BankID";

                    var audit = new AuditTrailModel
                    {
                        Activity = "Deleted item in the fund list. BANK ID: " + id.ToString(),
                        ModuleName = this.GetType().Name,
                        EmployeeID = App.EmployeeID
                    };
                    SystemClass.InsertLog(audit);
                }
                else
                {
                    MessageBox.Show("Please select an item");
                }
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }

        private void addnewbankbtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                AddNewBankWindow add = new AddNewBankWindow();
                Mouse.OverrideCursor = null;
                add.ShowDialog();
                db = new ImusCityHallEntities();
                banklb.ItemsSource = db.Banks.Where(m => m.IsActive == true).OrderByDescending(m => m.BankID).ToList();
                banklb.DisplayMemberPath = "BankName";
                banklb.SelectedValuePath = "BankID";
            }
            else
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
            Mouse.OverrideCursor = null;
        }

        private void editbtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (SystemClass.CheckConnection())
            {
                if (banklb.SelectedValue == null)
                {
                    Mouse.OverrideCursor = null;
                    MessageBox.Show("Please select an item!");
                }
                else
                {
                    ImusCityHallEntities db = new ImusCityHallEntities();
                    EditBankWindow edit = new EditBankWindow();
                    edit.BankID = (int)banklb.SelectedValue;
                    Mouse.OverrideCursor = null;
                    edit.ShowDialog();
                    db = new ImusCityHallEntities();
                    banklb.ItemsSource = db.Banks.Where(m => m.IsActive == true).OrderByDescending(m => m.BankID).ToList();
                    banklb.DisplayMemberPath = "BankName";
                    banklb.SelectedValuePath = "BankID";
                }
            }
            else
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }


            Mouse.OverrideCursor = null;
        }
    }
}
