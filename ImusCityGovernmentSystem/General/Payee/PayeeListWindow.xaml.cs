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
namespace ImusCityGovernmentSystem.General.Payee
{
    /// <summary>
    /// Interaction logic for PayeeListWindow.xaml
    /// </summary>
    public partial class PayeeListWindow : MetroWindow
    {

        public PayeeListWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                payeelb.ItemsSource = db.Payees.OrderByDescending(m => m.PayeeID).ToList();
                payeelb.DisplayMemberPath = "CompanyName";
                payeelb.SelectedValuePath = "PayeeID";
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }

        }

        private void searchbtn_Click(object sender, RoutedEventArgs e)
        {
            if(SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                if (String.IsNullOrEmpty(searchtb.Text))
                {

                }
                else
                {
                    var audit = new AuditTrailModel
                    {
                        Activity = "Searched item in the payee list. SEARCH KEY: " + searchtb.Text,
                        ModuleName = this.GetType().Name,
                        EmployeeID = App.EmployeeID
                    };
                    SystemClass.InsertLog(audit);
                }

                payeelb.ItemsSource = db.Payees.Where(m => m.CompanyName.Contains(searchtb.Text)).OrderByDescending(m => m.PayeeID).ToList();
                payeelb.DisplayMemberPath = "CompanyName";
                payeelb.SelectedValuePath = "PayeeID";
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
                        Activity = "Searched item in the payee list. SEARCH KEY: " + searchtb.Text,
                        ModuleName = this.GetType().Name,
                        EmployeeID = App.EmployeeID
                    };
                    SystemClass.InsertLog(audit);
                }

                searchbtn_Click(sender, e);
            }
        }

        private void addnewpayeebtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if(SystemClass.CheckConnection())
            {

                ImusCityHallEntities db = new ImusCityHallEntities();
                AddPayeeWindow add = new AddPayeeWindow();
                Mouse.OverrideCursor = null;
                add.ShowDialog();
                db = new ImusCityHallEntities();
                payeelb.ItemsSource = db.Payees.OrderByDescending(m => m.PayeeID).ToList();
                payeelb.DisplayMemberPath = "CompanyName";
                payeelb.SelectedValuePath = "PayeeID";
            }
            else
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }

        private void editbtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if(SystemClass.CheckConnection())
            {
                if (payeelb.SelectedValue == null)
                {
                    Mouse.OverrideCursor = null;
                    MessageBox.Show("Please select a payee to edit");
                }
                else
                {
                    ImusCityHallEntities db = new ImusCityHallEntities();
                    EditPayeeWindow edit = new EditPayeeWindow();
                    Mouse.OverrideCursor = null;
                    edit.PayeeID = (int)payeelb.SelectedValue;
                    Mouse.OverrideCursor = null;
                    edit.ShowDialog();
                    db = new ImusCityHallEntities();
                    payeelb.ItemsSource = db.Payees.OrderByDescending(m => m.PayeeID).ToList();
                    payeelb.DisplayMemberPath = "CompanyName";
                    payeelb.SelectedValuePath = "PayeeID";
                }

            }
            else
            {
                Mouse.OverrideCursor = null;
            }
            Mouse.OverrideCursor = null;
        }
    }
}
