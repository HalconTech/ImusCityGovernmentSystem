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
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                var fund = from p in db.Funds
                           select new
                           {
                               FundID = p.FundID,
                               FundName = p.FundCode + " (" + p.Branch + ")"
                           };
                fundcb.ItemsSource = fund.ToList();
                fundcb.DisplayMemberPath = "FundName";
                fundcb.SelectedValuePath = "FundID";


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
                else if (String.IsNullOrEmpty(createddatedp.Text))
                {
                    MessageBox.Show("Please select date");
                }
                else
                {
                    ImusCityHallEntities db = new ImusCityHallEntities();
                    List<CheckRegisterModel> list = new List<CheckRegisterModel>();
                    var result = db.GetCheckRegister(createddatedp.SelectedDate, (int)fundcb.SelectedValue);
                    //CurrencyToWords convert = new CurrencyToWords();
                    decimal totalAmount = db.GetCheckRegister(createddatedp.SelectedDate, (int)fundcb.SelectedValue).Sum(m => m.Amount).Value;
                    string sqlQuery = "SELECT dbo.fnCurrencyToWords({0})";
                    Object[] parameters = { totalAmount };
                    string amountInWords = db.Database.SqlQuery<string>(sqlQuery, parameters).FirstOrDefault();
                    foreach (var checkregister in result)
                    {
                        var check = new CheckRegisterModel
                        {
                            FundName = checkregister.FundName,
                            Branch = checkregister.Branch,
                            AccoutNumber = checkregister.AccountNumber,
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
