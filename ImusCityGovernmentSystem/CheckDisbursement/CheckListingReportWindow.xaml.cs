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
using MahApps.Metro.Controls;
using ImusCityGovernmentSystem.Model;
using ImusCityGovernmentSystem.CheckDisbursement.Model;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;
using System.Globalization;

namespace ImusCityGovernmentSystem.CheckDisbursement
{
    /// <summary>
    /// Interaction logic for CheckListingReportWindow.xaml
    /// </summary>
    public partial class CheckListingReportWindow : MetroWindow
    {
        public CheckListingReportWindow()
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
                if (String.IsNullOrEmpty(startdatedp.Text) || String.IsNullOrEmpty(enddatedp.Text))
                {
                    MessageBox.Show("Please select start date and end date");
                }
                else
                {
                    //int accountId = (int)fundcb.SelectedValue;
                    ImusCityHallEntities db = new ImusCityHallEntities();
                    CDSSignatory signatories = db.CDSSignatories.FirstOrDefault();
                    //FundBank account = db.FundBanks.Find(accountId);
                    List<CheckListingModel> list = new List<CheckListingModel>();
                    if (db.GetCheckListing(startdatedp.SelectedDate, enddatedp.SelectedDate).Count() <= 0)
                    {
                        MessageBox.Show("There are no record in this selection");
                        return;
                    }
                    var result = db.GetCheckListing(startdatedp.SelectedDate, enddatedp.SelectedDate);
                    foreach (var x in result)
                    {
                        var check = new CheckListingModel
                        {
                            AccountNumber = x.AccountNumber,
                            BankID = x.BankID,
                            BankName = x.BankName,
                            CheckAmount = x.CheckAmount.Value,
                            CheckDateCreated = x.CheckDateCreated.Value,
                            CheckDescription = x.CheckDescription,
                            CheckNo = x.CheckNo,
                            CheckUser = x.CHECK_USER,
                            ControlNo = x.ControlNo,
                            FundID = x.FundID.Value,
                            ProjectName = x.ProjectName,
                            Status = x.Status,
                            StatusID = x.StatusID.Value,
                            VoucherDateCreated = x.VoucherDateCreated.Value,
                            VoucherNumber = x.VoucherNo,
                            CanDelBy = x.CAN_DELETE_EMP,
                            CanDelDate = x.CAN_DELETE_DATE
                        };
                        list.Add(check);
                    }
                    if (!String.IsNullOrEmpty(fundcb.Text))
                    { 
                        int accountId = (int)fundcb.SelectedValue;
                        list = list.Where(m => m.FundID == accountId).ToList();
                    }
                    if (!String.IsNullOrEmpty(statuscb.Text))
                    {
                        if(statuscb.Text != "All")
                        {
                            list = list.Where(m => m.Status == statuscb.Text).ToList();
                        }
                    }
                       

                    ReportDataSet ds = new ReportDataSet();
                    ds.Locale = CultureInfo.InvariantCulture;
                   // FillDataSet(ds);

                    DataTable chkList = ds.Tables["CheckListingDataTable"];
                    foreach(CheckListingModel ch in list)
                    {
                        DataRow dr = chkList.Rows.Add();
                        dr.SetField("AccountNumber", ch.AccountNumber);
                        dr.SetField("BankID", ch.BankID);
                        dr.SetField("BankName", ch.BankName);
                        dr.SetField("CanDelBy", ch.CanDelBy);
                        dr.SetField("CanDelDate", ch.CanDelDate);
                        dr.SetField("CheckAmount", ch.CheckAmount);
                        dr.SetField("CheckDateCreated", ch.CheckDateCreated);
                        dr.SetField("CheckDescription", ch.CheckDescription);
                        dr.SetField("CheckNo", ch.CheckNo);
                        dr.SetField("CheckUser", ch.CheckUser);
                        dr.SetField("ControlNo", ch.ControlNo);
                        dr.SetField("FundID", ch.FundID);
                        dr.SetField("ProjectName", ch.ProjectName);
                        dr.SetField("Status", ch.Status);
                        dr.SetField("StatusID", ch.StatusID);
                        dr.SetField("VoucherDateCreated", ch.VoucherDateCreated);
                        dr.SetField("VoucherNumber", ch.VoucherNumber);
                    }



                    ReportDocument report;
                    report = new CheckDisbursement.Report.CheckListingReport();
                    report.SetDataSource(chkList);
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
