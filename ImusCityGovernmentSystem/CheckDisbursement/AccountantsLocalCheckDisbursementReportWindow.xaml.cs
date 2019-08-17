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
using System.Globalization;
using System.Data;

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

                    if(db.GetCheckRegister(startdatedp.SelectedDate, enddatedp.SelectedDate, accountId).Count() <= 0)
                    {
                        MessageBox.Show("There are no record in this selection");
                        return;
                    }
                    var result = db.GetCheckRegister(startdatedp.SelectedDate,enddatedp.SelectedDate,accountId).Where(m => m.Status == "Created" || m.Status == "Cancelled" || m.Status == "Damaged");
                    CurrencyToWords convert = new CurrencyToWords();
                    double totalAmount = Convert.ToDouble(db.GetCheckRegister(startdatedp.SelectedDate, enddatedp.SelectedDate, accountId).Sum(m => m.Amount).Value);
                    string amountInWords = convert.NumberToWords(totalAmount).ToUpper();

                    ReportDataSet ds = new ReportDataSet();
                    ds.Locale = CultureInfo.InvariantCulture;
                    DataTable checkRegisterList = ds.Tables["CheckRegisterDataTable"];
                    foreach (var checkregister in result)
                    {
                        DataRow dr = checkRegisterList.Rows.Add();
                        dr.SetField("FundName", checkregister.FundName);
                        dr.SetField("Branch", checkregister.Branch);
                        dr.SetField("AccountNumber", checkregister.AccountNumber);
                        dr.SetField("BankName", checkregister.BankName);
                        dr.SetField("DateCreated", checkregister.DateCreated);
                        dr.SetField("CheckNo", checkregister.CheckNo);
                        dr.SetField("CompanyName", checkregister.CompanyName);
                        dr.SetField("Amount", checkregister.Amount);
                        dr.SetField("AmountInWords",amountInWords);
                        dr.SetField("Status", checkregister.Status);
                    }

                    ReportDocument report;
                    report = new CheckDisbursement.Report.AccountantCheckDisbursementReport();
                    report.SetDataSource(checkRegisterList);
                    report.SetParameterValue("Signatory1", SystemClass.GetSignatory(signatories.CityAccountant));
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
