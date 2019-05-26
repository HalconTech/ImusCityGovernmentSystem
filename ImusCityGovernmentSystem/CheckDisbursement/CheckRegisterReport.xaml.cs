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
    /// Interaction logic for CheckRegisterReport.xaml
    /// </summary>
    public partial class CheckRegisterReport : MetroWindow
    {
        public CheckRegisterReport()
        {
            InitializeComponent();
            this.Title = "Check Register Report";
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if(SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                var fund = from p in db.FundBanks
                           where p.IsActive == true
                           select new
                           {
                               ID = p.FundBankID,
                               Name = p.Bank.BankName + " - " + p.Fund.FundName
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
            if(SystemClass.CheckConnection())
            {
                if(fundcb.SelectedValue == null)
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
                    List<CheckRegisterModel> list = new List<CheckRegisterModel>();
                    if (db.GetCheckRegister(startdatedp.SelectedDate, enddatedp.SelectedDate, accountId).Count() <= 0)
                    {
                        MessageBox.Show("There are no record in this selection");
                        return;
                    }
                    var result = db.GetCheckRegister(startdatedp.SelectedDate, enddatedp.SelectedDate, accountId);
                    foreach (var checkregister in result)
                    {
                        var check = new CheckRegisterModel
                        {
                            //FundName = checkregister.FundName,
                            FundName = checkregister.BankName,
                            Branch = checkregister.Branch,
                            AccoutNumber = checkregister.AccountNumber,
                            DateCreated = checkregister.DateCreated.Value,
                            CheckNo = checkregister.CheckNo,
                            CompanyName = checkregister.CompanyName,
                            Amount = checkregister.Amount.Value
                        };
                        list.Add(check);
                    }
                    ReportDocument report;
                    report = new CheckDisbursement.Report.CheckRegisterReport();
                    report.SetDataSource(list);
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
