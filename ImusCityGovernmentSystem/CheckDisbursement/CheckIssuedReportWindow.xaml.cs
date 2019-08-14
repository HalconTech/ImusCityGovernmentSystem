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
using ImusCityGovernmentSystem.CheckDisbursement.Model;
using System.Globalization;
using System.Data;

namespace ImusCityGovernmentSystem.CheckDisbursement
{
    /// <summary>
    /// Interaction logic for CheckIssuedReportWindow.xaml
    /// </summary>
    public partial class CheckIssuedReportWindow : MetroWindow
    {
        public CheckIssuedReportWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                startdatedp.SelectedDate = DateTime.Now.Date;
                enddatedp.SelectedDate = DateTime.Now.Date;
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
                    DateTime dateToday = DateTime.Now;
                    string year = dateToday.Year.ToString().Substring(dateToday.Year.ToString().Length - 2);
                    string month = dateToday.Month.ToString().PadLeft(2, '0');

                    int accountId = (int)fundcb.SelectedValue;
                    ImusCityHallEntities db = new ImusCityHallEntities();
                    CDSSignatory signatories = db.CDSSignatories.FirstOrDefault();
                    FundBank account = db.FundBanks.Find(accountId);
                    List<CheckIssuedModel> list = new List<CheckIssuedModel>();
                    if (db.GetCheckIssued(startdatedp.SelectedDate, enddatedp.SelectedDate, accountId).Count() <= 0)
                    {
                        MessageBox.Show("There are no record in this selection");
                        return;
                    }
                    var result = db.GetCheckIssued(startdatedp.SelectedDate, enddatedp.SelectedDate, accountId);
                    string adviceNo = account.AdviceNo.HasValue ? account.AdviceNo.ToString() : null;
                    foreach (var checkIssued in result)
                    {
                        var check = new CheckIssuedModel
                        {
                            StartDate = startdatedp.SelectedDate.Value,
                            EndDate = enddatedp.SelectedDate.Value,
                            BankName = checkIssued.BankName,
                            AccoutNumber = checkIssued.AccountNumber,
                            ReportNumber = string.Join("-", checkIssued.FundPrefix, year, month, adviceNo),
                            DateCreated = checkIssued.DateCreated.Value,
                            CheckNo = checkIssued.CheckNo,
                            VoucherNo = checkIssued.VoucherNo,
                            Center = "",
                            CompanyName = checkIssued.CompanyName,
                            PaymentNature = checkIssued.PaymentNature,
                            Amount = checkIssued.Amount.Value,
                            DisbursingOfficer = SystemClass.GetSignatory(signatories.DisbursingOfficer),
                            CanDelBy = checkIssued.CAN_DELETE_EMP,
                            CanDelDate = checkIssued.CAN_DELETE_DATE,
                            Status = checkIssued.Status,
                            StatusID = checkIssued.StatusID.Value

                        };
                        list.Add(check);
                    }

                    ReportDataSet ds = new ReportDataSet();
                    ds.Locale = CultureInfo.InvariantCulture;
                    // FillDataSet(ds);

                    DataTable chIss = ds.Tables["CheckIssuedDataTable"];
                    foreach (CheckIssuedModel ci in list)
                    {
                        DataRow dr = chIss.Rows.Add();
                        dr.SetField("AccountNumber", ci.AccoutNumber);
                        dr.SetField("Amount", ci.Amount);
                        dr.SetField("BankName", ci.BankName);
                        dr.SetField("CanDelBy", ci.CanDelBy);
                        dr.SetField("CanDelDate", ci.CanDelDate);
                        dr.SetField("Center", ci.Center);
                        dr.SetField("CheckNo", ci.CheckNo);
                        dr.SetField("CompanyName", ci.CompanyName);
                        dr.SetField("DateCreated", ci.DateCreated);
                        dr.SetField("DisbursingOfficer", ci.DisbursingOfficer);
                        dr.SetField("EndDate", ci.EndDate);
                        dr.SetField("PaymentNature", ci.PaymentNature);
                        dr.SetField("ReportNumber", ci.ReportNumber);
                        dr.SetField("StartDate", ci.StartDate);
                        dr.SetField("Status", ci.Status);
                        dr.SetField("StatusID", ci.StatusID);
                        dr.SetField("VoucherNo", ci.VoucherNo);
                    }


                    ReportDocument report;
                    report = new CheckDisbursement.Report.CheckIssuedReport();
                    report.SetDataSource(chIss);
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
