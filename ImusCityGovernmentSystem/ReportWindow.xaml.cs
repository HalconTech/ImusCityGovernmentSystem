using CrystalDecisions.CrystalReports.Engine;
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
using ImusCityGovernmentSystem.CheckDisbursement.Report;

namespace ImusCityGovernmentSystem
{
    /// <summary>
    /// Interaction logic for ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : MetroWindow
    {
        public int id;
        public ReportWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadReport(App.ReportID);
        }

        public void LoadReport(int reportid)
        {
            switch (reportid)
            {
                case 1:
                    this.Title = "Check Disbursement Voucher";
                    CheckDisbursementVoucher(id);
                    break;
                case 2:
                    this.Title = "Check Report";
                    CheckReport(id);
                    break;
            }
        }
        public void CheckDisbursementVoucher(int id)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                List<DisbursementVoucherModel> DVList = new List<DisbursementVoucherModel>();
                db = new ImusCityHallEntities();
                DVList = new List<DisbursementVoucherModel>();
                var disburse = db.GetDisbursementVoucher(id).ToList();
                CDSSignatory signatories = db.CDSSignatories.FirstOrDefault();
                foreach (var x in disburse)
                {
                    DisbursementVoucherModel dvl = new DisbursementVoucherModel();
                    dvl.Amount = x.Amount.HasValue ? x.Amount.Value : 0;
                    dvl.Certification = x.Certification;
                    dvl.CompanyAddress = x.CompanyAddress;
                    dvl.CompanyName = x.CompanyName;
                    dvl.DateCreated = x.DateCreated.Value;
                    dvl.DepartmentCode = x.DepartmentCode;
                    dvl.Description = x.Description;
                    dvl.DocumentCompleted = x.DocumentCompleted;
                    dvl.Name = x.Name;
                    dvl.Obligated = x.Obligated;
                    dvl.ObligationRequestNo = x.ObligationRequestNo;
                    dvl.TIN_EmpNo = x.TIN_EmpNo;
                    dvl.Unit_Project = x.Unit_Project;
                    dvl.VoucherNo = x.VoucherNo;
                    dvl.PaymentName = x.PaymentName;
                    dvl.Signatory = GetSignatory(signatories.CItyAccountant);
                    dvl.Signatory2 = GetSignatory(signatories.CityTreasurer);
                    dvl.Signatory3 = GetSignatory(signatories.CityAdministrator);
                    DVList.Add(dvl);

                    ReportDocument report;
                    report = new DisbursementVoucherReport();
                    report.SetDataSource(DVList);
                    reportviewer.ViewerCore.ReportSource = report;
                }
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }

        }
        public void CheckReport(int id)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                CDSSignatory signatories = db.CDSSignatories.FirstOrDefault();
                CheckModel check = new CheckModel();
                ImusCityGovernmentSystem.Model.Check checklist = db.Checks.Find(id);
                check.CheckNo = checklist.CheckNo;
                check.CheckDate = checklist.DateCreated.Value;
                check.CompanyName = checklist.Disbursement.Payee.CompanyName;
                CurrencyToWords convert = new CurrencyToWords();
                double amount = Convert.ToDouble(checklist.Amount.Value);
                check.AmountInWords = convert.NumberToWords(amount).ToUpper();
                check.CheckDescription = checklist.CheckDescription;
                check.VoucherNo = checklist.Disbursement.VoucherNo;
                check.Amount = checklist.Amount.Value;
                check.Signatory1 = GetSignatory(signatories.CityMayor);
                check.Signatory2 = GetSignatory(signatories.CityTreasurer);
                ReportDocument report;
                report = new CheckReport();
                report.SetDataSource(new[] { check });
                reportviewer.ViewerCore.ReportSource = report;

            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }

        public string GetSignatory(int? id)
        {
            string result = "";
            ImusCityHallEntities db = new ImusCityHallEntities();
            ImusCityGovernmentSystem.Model.Employee employee = db.Employees.Find(id);
            result = employee.FirstName + " " + employee.MiddleName + " " + employee.LastName;
            return result;
        }
    }
}
