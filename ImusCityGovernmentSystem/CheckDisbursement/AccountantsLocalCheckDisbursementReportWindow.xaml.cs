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
using CrystalDecisions.CrystalReports.Engine;
namespace ImusCityGovernmentSystem.CheckDisbursement
{
    /// <summary>
    /// Interaction logic for AccountantsLocalCheckDisbursementReportWindow.xaml
    /// </summary>
    public partial class AccountantsLocalCheckDisbursementReportWindow : MetroWindow
    {
        public AccountantsLocalCheckDisbursementReportWindow()
        {
            InitializeComponent();
            this.Title = "Accountant's Advice of Local Check Disbursement";
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                var fund = from p in db.FundBanks
                           where p.IsActive == true
                           select new
                           {
                               ID = p.FundBankID,
                               Name = p.Fund.FundName + " - " + p.Bank.BankCode + "(" + p.AccountNumber + ")"
                           };
                fundcb.ItemsSource = fund.ToList();
                fundcb.DisplayMemberPath = "Name";
                fundcb.SelectedValuePath = "ID";
                startdatedp.SelectedDate = DateTime.Now.Date;
                enddatedp.SelectedDate = DateTime.Now.Date;
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }
        private void generatebtn_Click(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                if (fundcb.SelectedValue == null)
                {
                    MessageBox.Show("Please select fund");
                }
                else if (String.IsNullOrEmpty(startdatedp.Text) || String.IsNullOrEmpty(enddatedp.Text))
                {
                    MessageBox.Show("Please select start date and end date");
                }
                else
                {
                    int accountId = (int)fundcb.SelectedValue;
                    ImusCityHallEntities db = new ImusCityHallEntities();
                    CDSSignatory signatories = db.CDSSignatories.FirstOrDefault();
                    FundBank account = db.FundBanks.Find(accountId);
                    List<CheckRegisterModel> list = new List<CheckRegisterModel>();
                    if(db.GetCheckRegister(startdatedp.SelectedDate, enddatedp.SelectedDate, accountId).Count() <= 0)
                    {
                        MessageBox.Show("There are no record in this selection");
                        return;
                    }
                    var result = db.GetCheckRegister(startdatedp.SelectedDate,enddatedp.SelectedDate,accountId);
                    CurrencyToWords convert = new CurrencyToWords();
                    double totalAmount = Convert.ToDouble(db.GetCheckRegister(startdatedp.SelectedDate, enddatedp.SelectedDate, accountId).Sum(m => m.Amount).Value);
                    string amountInWords = convert.NumberToWords(totalAmount).ToUpper();
                    foreach (var checkregister in result)
                    {
                        var check = new CheckRegisterModel
                        {
                            //FundName = checkregister.FundName,
                            FundName = checkregister.BankName,
                            Branch = checkregister.Branch,
                            AccoutNumber = checkregister.AccountNumber,
                            BankName = checkregister.BankName,
                            DateCreated = checkregister.DateCreated.Value,
                            CheckNo = checkregister.CheckNo,
                            CompanyName = checkregister.CompanyName,
                            Amount = checkregister.Amount.Value,
                            AmountInWords = amountInWords
                        };
                        list.Add(check);
                    }
                    ReportDocument report;
                    report = new CheckDisbursement.Report.AccountantCheckDisbursementReport();
                    report.SetDataSource(list);
                    report.SetParameterValue("Signatory1", SystemClass.GetSignatory(signatories.CItyAccountant));
                    report.SetParameterValue("Signatory2", SystemClass.GetSignatory(signatories.AccountantRepresentative));
                    report.SetParameterValue("AdviceNo", account.AdviceNo.HasValue ? account.AdviceNo.ToString() : "");
                    report.SetParameterValue("employee", "Generated By: " + SystemClass.Employee(App.EmployeeID));
                    reportviewer.ViewerCore.ReportSource = report;
                }
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }

    }
}
