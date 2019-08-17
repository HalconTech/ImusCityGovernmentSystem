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
                List<VoucherItemsModel> voucherList = new List<VoucherItemsModel>();
                ImusCityHallEntities db = new ImusCityHallEntities();
                Disbursement disbursement = db.Disbursements.Find(DisbursementID);
                vouchernotb.Text = disbursement.VoucherNo;
                payeetb.Text = disbursement.Payee == null ? disbursement.PayeeName : disbursement.Payee.CompanyName;

                paymenttypetb.Text = disbursement.PaymentType;
                decimal totalAmount = disbursement.DisbursementItems.Sum(m => m.Amount);
                voucheramounttb.Text = String.Format("{0:n}", totalAmount);

                foreach (var item in disbursement.DisbursementItems)
                {
                    var voucherItem = new VoucherItemsModel()
                    {
                        Explanation = item.Explanation,
                        Amount = String.Format("{0:n}", item.Amount)
                    };
                    voucherList.Add(voucherItem);
                }
                voucheritemsdg.ItemsSource = voucherList;

                LoadFund();

            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }

        public void LoadFund()
        {
            ImusCityHallEntities db = new ImusCityHallEntities();
            List<FundModel> fundList = new List<FundModel>();
            foreach (var item in db.FundBanks)
            {
                var fund = new FundModel()
                {
                    id = item.FundBankID,
                    FundBankName = string.Join("-", item.Fund.FundPrefix, item.AccountNumber, item.Fund.FundName, item.Bank.BankCode),
                    FundName = item.Fund.FundName,
                    BankName = item.Bank.BankCode,
                    Prefix = item.Fund.FundPrefix,
                    AccountNumber = item.AccountNumber,
                    CurrentBalance = item.CurrentBalance.HasValue ? item.CurrentBalance.Value : 0
                };

                fundList.Add(fund);
            }

            fundcb.ItemsSource = fundList;
            fundcb.SelectedValuePath = "id";
            fundcb.DisplayMemberPath = "FundBankName";
        }

        private void savebtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                if (SystemClass.CheckConnection())
                {
                    if (fundcb.SelectedValue == null)
                    {
                        Mouse.OverrideCursor = null;
                        MessageBox.Show("Please select fund");
                    }
                    else
                    {
                        ImusCityHallEntities db = new ImusCityHallEntities();
                        int fundBankId = (int)fundcb.SelectedValue;
                        var fundBank = db.FundBanks.Find(fundBankId);
                        ImusCityGovernmentSystem.Model.Disbursement disbursement = db.Disbursements.Find(DisbursementID);

                        var controlNumber = db.ControlNumbers.OrderByDescending(m => m.ControlNoID).FirstOrDefault(m => m.FundBankID == fundBankId && m.Active == true);

                        if (String.IsNullOrEmpty(checknotb.Text))
                        {
                            MessageBox.Show("Please provide the check number");
                            this.Close();
                        }
                        else if (String.IsNullOrEmpty(checkdesctb.Text))
                        {
                            Mouse.OverrideCursor = null;
                            MessageBox.Show("Please provide check description");
                        }
                        else if (checkamounttb.Text.Equals("0.00"))
                        {
                            Mouse.OverrideCursor = null;
                            MessageBox.Show("Please enter check amount");
                        }
                        else if (db.Checks.Any(m => m.CheckNo == checknotb.Text && m.Status != (int)CheckStatus.Deleted && m.Disbursement.FundBankID == fundBankId))
                        {
                            Mouse.OverrideCursor = null;
                            MessageBox.Show("Check number is already been used");
                        }
                        else if (db.FundBanks.Find(fundBankId).CurrentBalance < Convert.ToDecimal(checkamounttb.Text))
                        {
                            Mouse.OverrideCursor = null;
                            MessageBox.Show("Check cannot be created, you have insufficients funds");
                        }
                        else if (controlNumber == null)
                        {
                            Mouse.OverrideCursor = null;
                            MessageBox.Show("Selected fund have no available check number");
                        }
                        else if(db.DamageChecks.Where(m => m.FundBankID == fundBankId && m.CheckNumber == checknotb.Text).Count() >= 1)
                        {
                            Mouse.OverrideCursor = null;
                            MessageBox.Show("The current check number is in the list of damage checks");
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

                            //Increment of the Check Number.
                            if (controlNumber.EndingControlNo == controlNumber.NextControlNo)
                                controlNumber.Active = false;
                            else
                                controlNumber.NextControlNo++;

                            check.ControlNo = checknotb.Text;
                            var savedCheck = db.Checks.Add(check);


                            ImusCityGovernmentSystem.Model.BankTrail banktrail = new BankTrail();
                            banktrail.DebitCredit = "D";
                            banktrail.FundBankID = fundBankId;
                            banktrail.Amount = Convert.ToDecimal(checkamounttb.Text);
                            banktrail.EntryName = nameof(BankTrailEntry.CheckCreated);
                            banktrail.CheckID = savedCheck.CheckID;
                            banktrail.EntryNameID = (int)BankTrailEntry.CheckCreated;
                            banktrail.EmployeeID = App.EmployeeID;
                            banktrail.DateCreated = DateTime.Now;
                            db.BankTrails.Add(banktrail);

                            ImusCityGovernmentSystem.Model.FundBank account = db.FundBanks.Find(fundBankId);
                            account.CurrentBalance -= Convert.ToDecimal(checkamounttb.Text);

                            disbursement.VoucherNo = string.Join("-", fundBank.Fund.FundPrefix, disbursement.VoucherNo);
                            disbursement.FundBankID = fundBankId;

                            db.SaveChanges();

                            var audit = new AuditTrailModel
                            {
                                Activity = "Created new check entry DIS ID: " + DisbursementID.ToString(),
                                ModuleName = this.GetType().Name,
                                EmployeeID = App.EmployeeID
                            };

                            SystemClass.InsertLog(audit);
                            Mouse.OverrideCursor = null;
                            MessageBox.Show("Check created successfully!");
                            PrintReport(check.CheckID);

                            checknotb.Clear();
                            checkdesctb.Clear();
                            checkamounttb.Clear();
                            currentbalancetb.Text = "";
                            LoadFund();
                        }

                    }
                }
                else
                {
                    Mouse.OverrideCursor = null;
                    MessageBox.Show(SystemClass.DBConnectionErrorMessage);
                }
            }
            catch (Exception ex)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(ex.ToString());
            }
        }

        void PrintReport(int id)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            ImusCityHallEntities db = new ImusCityHallEntities();
            string bankCode = db.Checks.Find(id).Disbursement.FundBank.Bank.BankCode;
            ReportWindow report = new ReportWindow();
            report.id = id;
            report.bankName = bankCode;
            App.ReportID = 2;
            report.Show();
            Mouse.OverrideCursor = null;
        }

        private void fundcb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (fundcb.SelectedValue == null)
            {
                return;
            }
            else
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                int fundBankId = (int)fundcb.SelectedValue;
                var controlNumber = db.FundBanks.Find(fundBankId).ControlNumbers;
                var fundBank = db.FundBanks.Find(fundBankId);
                if (controlNumber == null)
                {
                    MessageBox.Show("Selected fund have no available check number");
                    checknotb.Clear();
                }
                else if(controlNumber.Where(m => m.Active == true).Count() < 1)
                {
                    MessageBox.Show("Selected fund have no available check number");
                    checknotb.Clear();
                }
                else if (controlNumber.Where(m => m.Active == true).Count() > 1)
                {
                    MessageBox.Show("Selected fund have multiple active check control number. Set only one active control number to be used");
                    checknotb.Clear();
                }
                else
                {
                        int checkNo = controlNumber.Where(m => m.Active == true).FirstOrDefault().NextControlNo.HasValue ? controlNumber.Where(m => m.Active == true).FirstOrDefault().NextControlNo.Value : 0;
                        string formatted = checkNo.ToString("D10");
                        checknotb.Text = formatted;
                        currentbalancetb.Text = String.Format("{0:n}", fundBank.CurrentBalance);
                                
                }
            }

        }
    }
}
