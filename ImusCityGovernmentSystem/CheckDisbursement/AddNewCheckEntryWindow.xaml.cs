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
    /// Interaction logic for AddNewCheckEntryWindow.xaml
    /// </summary>
    public partial class AddNewCheckEntryWindow : MetroWindow
    {
        public int DisbursementID;
        public AddNewCheckEntryWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {

                foreach (var item in Enum.GetValues(typeof(PaymentType)))
                {
                    paymenttypecb.Items.Add(item);
                }

                ImusCityHallEntities db = new ImusCityHallEntities();
                Disbursement disbursement = db.Disbursements.Find(DisbursementID);
                vouchernotb.Text = disbursement.VoucherNo;
                payeetb.Text = disbursement.Payee.CompanyName;
                descriptiontb.Text = disbursement.Description;
                paymenttypecb.SelectedIndex = disbursement.PaymentTypeID.HasValue ? disbursement.PaymentTypeID.Value : 0;
                voucheramounttb.Text = String.Format("{0:0.##}", disbursement.Amount);
               
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
                ImusCityGovernmentSystem.Model.Disbursement disbursement = db.Disbursements.Find(DisbursementID);
                var cn = db.ControlNumbers.OrderByDescending(m => m.ControlNoID).FirstOrDefault(m => m.FundBankID == disbursement.FundBankID);
                if (String.IsNullOrEmpty(checknotb.Text))
                {
                    MessageBox.Show("Please provide the check number");
                }
                else if (String.IsNullOrEmpty(checkdesctb.Text))
                {
                    MessageBox.Show("Please provide check description");
                }
                else if (String.IsNullOrEmpty(checkamounttb.Text))
                {
                    MessageBox.Show("Please enter check amount");
                }
                else if (db.Checks.Any(m => m.CheckNo == checknotb.Text))
                {
                    MessageBox.Show("Check number is already been used");
                }           
                else if(db.FundBanks.Find(disbursement.FundBankID).CurrentBalance < Convert.ToDecimal(checkamounttb.Text))
                {
                    MessageBox.Show("Check cannot be created, you have insufficients funds");
                }
                else if(cn == null)
                {
                        MessageBox.Show("Selected fund have no control number set up.");
                }
                else
                {


                    Check check = new Check();
                    check.DisbursementID = DisbursementID;
                    check.CheckNo = checknotb.Text;
                    check.CheckDescription = checkdesctb.Text;
                    check.Amount = Convert.ToDecimal(checkamounttb.Text);
                    check.EmployeeID = App.EmployeeID;
                    check.DateCreated = DateTime.Now;
                    check.Status = (int)CheckStatus.Created;

                    string formatted = string.Format("{0:0000000000}", cn.NextControlNo);
                    check.ControlNo = formatted;
                    db.Checks.Add(check);


                    ImusCityGovernmentSystem.Model.BankTrail banktrail = new BankTrail();
                    banktrail.DebitCredit = "D";
                    banktrail.FundBankID = disbursement.FundBankID;
                    banktrail.Amount = Convert.ToDecimal(checkamounttb.Text);
                    banktrail.EntryName = nameof(BankTrailEntry.CheckCreated);
                    banktrail.CheckID = check.CheckID;
                    banktrail.EntryNameID = (int)BankTrailEntry.CheckCreated;
                    banktrail.EmployeeID = App.EmployeeID;
                    banktrail.DateCreated = DateTime.Now;
                    db.BankTrails.Add(banktrail);

                    ImusCityGovernmentSystem.Model.FundBank account = db.FundBanks.Find(disbursement.FundBankID);
                    account.CurrentBalance -= Convert.ToDecimal(checkamounttb.Text);

                    db.SaveChanges();

                    var audit = new AuditTrailModel
                    {
                        Activity = "Created new check entry DIS ID: " + DisbursementID.ToString(),
                        ModuleName = this.GetType().Name,
                        EmployeeID = App.EmployeeID
                    };

                    SystemClass.InsertLog(audit);
                    MessageBox.Show("Check created successfully!");
                    PrintReport(check.CheckID);

                    checknotb.Clear();
                    checkdesctb.Clear();
                    checkamounttb.Clear();
                }


            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }

        void PrintReport(int id)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            ReportWindow report = new ReportWindow();
            report.id = id;
            App.ReportID = 2;
            report.Show();
            Mouse.OverrideCursor = null;
        }
        private void TextBlock_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            checkdesctb.Text = descriptiontb.Text;
        }

        

    }
}
