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
    /// Interaction logic for AddCheckDisbursementWindow.xaml
    /// </summary>
    public partial class AddCheckDisbursementWindow : MetroWindow
    {
        public List<DisbursementVoucherModel> DVList = new List<DisbursementVoucherModel>();
        public List<VoucherItemsModel> voucherList = new List<VoucherItemsModel>();
        public AddCheckDisbursementWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            optionalpayee.IsEnabled = false;
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                payeecb.ItemsSource = db.Payees.Where(m => m.IsActive == true).OrderBy(m => m.CompanyName).ToList();
                payeecb.DisplayMemberPath = "CompanyName";
                payeecb.SelectedValuePath = "PayeeID";
                foreach (var item in Enum.GetValues(typeof(PaymentType)))
                {
                    paymenttypecb.Items.Add(item);
                }
                paymenttypecb.SelectedIndex = 0;
                paymenttypecb.IsEnabled = false;
                departmentcb.ItemsSource = db.Departments.OrderBy(m => m.DepartmentName).ToList();
                departmentcb.DisplayMemberPath = "DepartmentName";
                departmentcb.SelectedValuePath = "DepartmentID";
                voucheritemsdg.ItemsSource = voucherList;
                voucherprefixtb.Text = VoucherPrefix();
                IncrementAdviceNo();
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }

        public string VoucherPrefix()
        {
            DateTime dateToday = DateTime.Now;
            string year = dateToday.Year.ToString().Substring(dateToday.Year.ToString().Length - 2);
            string month = dateToday.Month.ToString().PadLeft(2, '0');
            return string.Join("-", year, month);
        }
        private void savebtn_Click(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                try
                {
                    ImusCityHallEntities db = new ImusCityHallEntities();
                    CDSSignatory signatories = db.CDSSignatories.FirstOrDefault();
                    if (payeecb.SelectedValue == null && optionpayeecb.IsChecked == false)
                    {
                        MessageBox.Show("Please select payee from the dropdown list");
                    }
                    else if (optionpayeecb.IsChecked == true && String.IsNullOrEmpty(optionalpayee.Text))
                    {

                        MessageBox.Show("Please enter payee name");
                    }
                    else if (paymenttypecb.SelectedValue == null)
                    {
                        MessageBox.Show("Please select payment type");
                    }
                    else if (signatories == null)
                    {
                        Mouse.OverrideCursor = null;
                        MessageBox.Show("Please add report signatories");
                    }
                    else if (String.IsNullOrEmpty(vouchernotb.Text))
                    {
                        MessageBox.Show("Please enter voucher number");
                    }
                    else if (voucherList.Count() == 0)
                    {
                        MessageBox.Show("Please input at least one voucher item");
                    }
                    else
                    {
                        Disbursement disbursement = new Disbursement();
                        disbursement.PayeeID = optionpayeecb.IsChecked == true ? null : payeecb.SelectedValue == null ? (int?)null : (int)payeecb.SelectedValue;
                        disbursement.PayeeName = optionalpayee.Text.ToUpper();
                        disbursement.PayeeRepID = payeerepcb.SelectedValue == null ? (int?)null : (int)payeerepcb.SelectedValue;
                        disbursement.DepartmentID = departmentcb.SelectedValue == null ? null : (int?)departmentcb.SelectedValue;
                        disbursement.PaymentType = paymenttypecb.Text;
                        disbursement.VoucherNo = voucherprefixtb.Text + vouchernotb.Text;
                        disbursement.DateCreated = DateTime.Now;
                        disbursement.ProjectName = projectnametb.Text;
                        disbursement.Obligated = obligatedcb.IsChecked;
                        disbursement.DocCompleted = documentcb.IsChecked;
                        disbursement.CreatedBy = App.EmployeeID;
                        foreach (var voucherItem in voucherList)
                        {
                            var item = new DisbursementItem()
                            {
                                Explanation = voucherItem.Explanation,
                                Amount = Convert.ToDecimal(voucherItem.Amount)
                            };
                            disbursement.DisbursementItems.Add(item);
                        }
                        var x = db.Disbursements.Add(disbursement);
                        db.SaveChanges();
                        var audit = new AuditTrailModel
                        {
                            Activity = "Created disbursement document",
                            ModuleName = this.GetType().Name,
                            EmployeeID = App.EmployeeID
                        };
                        SystemClass.InsertLog(audit);
                        MessageBox.Show("Check Disbursement Created!");
                        Print(x.DisbursementID);
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);

            }
        }


        public void Print(int DisbursementID)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            ReportWindow report = new ReportWindow();
            report.id = DisbursementID;
            App.ReportID = 1;
            report.Show();
            Mouse.OverrideCursor = null;
        }
        private void payeecb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                if (payeecb.SelectedValue == null)
                {
                    return;
                }
                else
                {
                    payeerepcb.ItemsSource = db.PayeeRepresentativeViews.Where(m => m.PayeeID == (int)payeecb.SelectedValue).OrderBy(m => m.PayeeRepresentativeName).ToList();
                    payeerepcb.DisplayMemberPath = "PayeeRepresentativeName";
                    payeerepcb.SelectedValuePath = "PayeeRepID";
                }
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }

        }

        public void IncrementAdviceNo()
        {
            if (SystemClass.CheckConnection())
            {
                bool isForAudit = false;

                ImusCityHallEntities db = new ImusCityHallEntities();

                var accountlist = db.FundBanks.ToList();
                foreach (var x in accountlist.Where(m => m.IsActive == true && m.IsProcessed == false))
                {
                    x.IsProcessed = true;
                    x.AdviceNo = (x.AdviceNo + 1);
                    isForAudit = true;
                    db.SaveChanges();
                }

                if (isForAudit == true)
                {
                    var audit = new AuditTrailModel
                    {
                        Activity = "Advice No Incremented successful.",
                        ModuleName = this.GetType().Name,
                        EmployeeID = App.EmployeeID
                    };

                    SystemClass.InsertLog(audit);
                }
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }

        private void optionpayeecb_Checked(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                payeecb.ItemsSource = db.Payees.Where(m => m.IsActive == true).OrderBy(m => m.CompanyName).ToList();
                payeecb.DisplayMemberPath = "CompanyName";
                payeecb.SelectedValuePath = "PayeeID";
                payeecb.IsEnabled = false;
                payeerepcb.ItemsSource = null;
                payeerepcb.IsEnabled = false;
                optionalpayee.IsEnabled = true;
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }

        private void optionpayeecb_Unchecked(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                payeecb.ItemsSource = db.Payees.Where(m => m.IsActive == true).OrderBy(m => m.CompanyName).ToList();
                payeecb.DisplayMemberPath = "CompanyName";
                payeecb.SelectedValuePath = "PayeeID";
                payeecb.IsEnabled = true;
                payeerepcb.IsEnabled = true;
                payeerepcb.ItemsSource = null;
                optionalpayee.IsEnabled = false;
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }
    }
}
