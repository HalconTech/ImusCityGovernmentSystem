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
namespace ImusCityGovernmentSystem.General.Fund
{
    /// <summary>
    /// Interaction logic for AddNewFundWindow.xaml
    /// </summary>
    public partial class AddNewFundWindow : MetroWindow
    {
        ImusCityHallEntities db = new ImusCityHallEntities();
        public AddNewFundWindow()
        {
            InitializeComponent();
            fundcodetb.Focus();
        }

        private void savebtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if(String.IsNullOrEmpty(fundcodetb.Text) || String.IsNullOrEmpty(fundnametb.Text))
            {
                MessageBox.Show("Please input fund code and fund name!");
            }
            else
            {
                ImusCityGovernmentSystem.Model.Fund fund = new Model.Fund();
                fund.FundCode = fundcodetb.Text;
                fund.FundName = fundnametb.Text;
                db.Funds.Add(fund);
                db.SaveChanges();
                Mouse.OverrideCursor = null;

                var audit = new AuditTrailModel
                {
                    Activity = "Added new fund in the database. DEPT CODE: " + fundcodetb.Text,
                    ModuleName = this.GetType().Name,
                    EmployeeID = App.EmployeeID
                };

                SystemClass.InsertLog(audit);

                MessageBox.Show("New item added successfully.");

                SystemClass.ClearTextBoxes(this);
            }
            Mouse.OverrideCursor = null;
        }

        private void clearbtn_Click(object sender, RoutedEventArgs e)
        {
            SystemClass.ClearTextBoxes(this);
        }
    }
}
