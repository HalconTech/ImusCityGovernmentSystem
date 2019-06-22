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
    /// Interaction logic for AddNewPayeeRepresentativeWindow.xaml
    /// </summary>
    public partial class AddNewPayeeRepresentativeWindow : MetroWindow
    {
       
        public AddNewPayeeRepresentativeWindow()
        {
            InitializeComponent();
        }

        private void addreprebtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void savebtn_Click(object sender, RoutedEventArgs e)
        {
            if(SystemClass.CheckConnection())
            {
                Mouse.OverrideCursor = Cursors.Wait;
                if (String.IsNullOrEmpty(fnametb.Text) || String.IsNullOrEmpty(lnametb.Text))
                {
                    Mouse.OverrideCursor = null;
                    MessageBox.Show("Please input first name and last name field!");
                }
                else if(String.IsNullOrEmpty(mobilenotb.Text))
                {
                    MessageBox.Show("Please input mobile number");
                }
                else
                {
                    ImusCityHallEntities db = new ImusCityHallEntities();
                    PayeeRepresentative pr = new PayeeRepresentative();
                    pr.FirstName = fnametb.Text;
                    pr.MiddleName = mnametb.Text;
                    pr.LastName = lnametb.Text;
                    pr.MobileNo = mobilenotb.Text;
                    pr.Landline = landlinetb.Text;
                    db.PayeeRepresentatives.Add(pr);
                    db.SaveChanges();
                    Mouse.OverrideCursor = null;

                    var audit = new AuditTrailModel
                    {
                        Activity = "Added new payee representative in the database. NAME: " + fnametb.Text + " " + lnametb.Text,
                        ModuleName = this.GetType().Name,
                        EmployeeID = App.EmployeeID
                    };

                    SystemClass.InsertLog(audit);
                    MessageBox.Show("Representative addedd successfully!");
                    SystemClass.ClearTextBoxes(this);
                }
            }
            else
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }   
            Mouse.OverrideCursor = null;
        }

        private void clearbtn_Click(object sender, RoutedEventArgs e)
        {
            SystemClass.ClearTextBoxes(this);
        }
       
    }
}
