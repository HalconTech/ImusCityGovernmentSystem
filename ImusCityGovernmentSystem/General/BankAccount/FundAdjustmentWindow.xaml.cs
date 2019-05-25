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
using ImusCityGovernmentSystem.CheckDisbursement;
namespace ImusCityGovernmentSystem.General.BankAccount
{
    /// <summary>
    /// Interaction logic for FundAdjustmentWindow.xaml
    /// </summary>
    public partial class FundAdjustmentWindow : MetroWindow
    {
        public int FundBankID;
        public FundAdjustmentWindow()
        {
            InitializeComponent();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            bool approvedDecimalPoint = false;

            if (e.Text == ".")
            {
                if (!((TextBox)sender).Text.Contains("."))
                    approvedDecimalPoint = true;
            }

            if (!(char.IsDigit(e.Text, e.Text.Length - 1) || approvedDecimalPoint))
                e.Handled = true;
        }
        private void savebtn_Click(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                ImusCityGovernmentSystem.Model.FundBank account = db.FundBanks.Find(FundBankID);

                ImusCityGovernmentSystem.Model.BankTrail banktrail = new BankTrail();


                switch (adjustmenttypecb.Text.Substring(0, 1))
                {
                    case "D":
                        if (Convert.ToDecimal(amounttb.Text) > account.CurrentBalance)
                        {
                            MessageBox.Show("Cannot be debited, you will have an insufficients funds");
                            return;
                        }
                        account.CurrentBalance -= Convert.ToDecimal(amounttb.Text);
                        banktrail.DebitCredit = "D";
                        break;
                    case "C":
                        account.CurrentBalance += Convert.ToDecimal(amounttb.Text);
                        banktrail.DebitCredit = "C";
                        break;
                }
                banktrail.FundBankID = FundBankID;
                banktrail.Amount = Convert.ToDecimal(amounttb.Text);
                banktrail.EntryName = nameof(BankTrailEntry.Adjustment);
                banktrail.CheckID = null;
                banktrail.EntryNameID = (int)BankTrailEntry.Adjustment;
                banktrail.EmployeeID = App.EmployeeID;
                banktrail.DateCreated = DateTime.Now;
                db.BankTrails.Add(banktrail);

                db.SaveChanges();


                var audit = new AuditTrailModel
                {
                    Activity = "Adjusted current amount of FUNDBANK ID: " + FundBankID.ToString(),
                    ModuleName = this.GetType().Name,
                    EmployeeID = App.EmployeeID
                };

                SystemClass.InsertLog(audit);
                MessageBox.Show("Adjustment added succesfully");
                this.Close();
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
                FundBank fundbank = db.FundBanks.Find(FundBankID);
                fundtb.Text = fundbank.Fund.FundName;
                banktb.Text = fundbank.Bank.BankName;
                accountnumbertb.Text = fundbank.AccountNumber;
                currentamounttb.Text  = String.Format("{0:0.##}", fundbank.CurrentBalance);
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }
    }
}
