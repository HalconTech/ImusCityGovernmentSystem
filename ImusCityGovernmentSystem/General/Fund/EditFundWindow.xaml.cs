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
    /// Interaction logic for EditFundWindow.xaml
    /// </summary>
    public partial class EditFundWindow : MetroWindow
    {
        ImusCityHallEntities db = new ImusCityHallEntities();
        public int FundID;
        public EditFundWindow()
        {
            InitializeComponent();
        }

        private void savebtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (String.IsNullOrEmpty(fundcodetb.Text) || String.IsNullOrEmpty(fundnametb.Text))
            {
                MessageBox.Show("Please input fund code and fund name!");
            }
            else
            {
                ImusCityGovernmentSystem.Model.Fund fund = db.Funds.Find(FundID);
                fund.FundCode = fundcodetb.Text;
                fund.FundName = fundnametb.Text;
                db.SaveChanges();
                Mouse.OverrideCursor = null;
                var audit = new AuditTrailModel
                {
                    Activity = "Updated an item in fund list. FUND ID: " + FundID.ToString(),
                    ModuleName = this.GetType().Name,
                    EmployeeID = App.EmployeeID
                };

                SystemClass.InsertLog(audit);
                MessageBox.Show("Fund updated successfully!");
            }
            Mouse.OverrideCursor = null;
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ImusCityGovernmentSystem.Model.Fund fund = db.Funds.Find(FundID);
            fundcodetb.Text = fund.FundCode;
            fundnametb.Text = fund.FundName;
        }
    }
}
