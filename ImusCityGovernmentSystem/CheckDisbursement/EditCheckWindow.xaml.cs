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
namespace ImusCityGovernmentSystem.CheckDisbursement
{
    /// <summary>
    /// Interaction logic for EditCheckWindow.xaml
    /// </summary>
    public partial class EditCheckWindow : MetroWindow
    {
        public int CheckID;
        public EditCheckWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                foreach (var item in Enum.GetValues(typeof(CheckStatus)))
                {
                    checkstatuscb.Items.Add(item);
                }

                ImusCityHallEntities db = new ImusCityHallEntities();
                ImusCityGovernmentSystem.Model.Check check = db.Checks.Find(CheckID);
                checknumbertb.Text = check.CheckNo;
                checkdatecreateddp.Text = check.DateCreated.Value.ToShortDateString();
                checkstatuscb.SelectedIndex = check.Status.HasValue ? check.Status.Value : 0;
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
                ImusCityGovernmentSystem.Model.Check check = db.Checks.Find(CheckID);
                check.DateCreated = checkdatecreateddp.SelectedDate;
                check.Status = checkstatuscb.SelectedIndex;
           
                if(checkstatuscb.SelectedIndex == (int)CheckStatus.Cancelled)
                {
                    ImusCityGovernmentSystem.Model.BankTrail banktrail = new BankTrail();
                    ImusCityGovernmentSystem.Model.Disbursement disbursement = db.Disbursements.Find(check.DisbursementID);
                    banktrail.DebitCredit = "C";
                    banktrail.FundBankID = disbursement.FundBankID;
                    banktrail.Amount = Convert.ToDecimal(check.Amount);
                    banktrail.EntryName = nameof(BankTrailEntry.CheckCancelled);
                    banktrail.CheckID = check.CheckID;
                    banktrail.EntryNameID = (int)BankTrailEntry.CheckCancelled;
                    banktrail.EmployeeID = App.EmployeeID;
                    banktrail.DateCreated = DateTime.Now;
                    db.BankTrails.Add(banktrail);

                    ImusCityGovernmentSystem.Model.FundBank account = db.FundBanks.Find(disbursement.FundBankID);
                    account.CurrentBalance += Convert.ToDecimal(check.Amount);

                }
                db.SaveChanges();

                var audit = new AuditTrailModel
                {
                    Activity = "Updated check in the database. CHECK NUMBER: " + check.CheckNo,
                    ModuleName = this.GetType().Name,
                    EmployeeID = App.EmployeeID
                };

                SystemClass.InsertLog(audit);

                MessageBox.Show("Check entry updated successfully");
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }
    }
}
