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
        public AddCheckDisbursementWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                payeecb.ItemsSource = db.Payees.Where(m => m.IsActive == true).OrderBy(m => m.CompanyName).ToList();
                payeecb.DisplayMemberPath = "CompanyName";
                payeecb.SelectedValuePath = "PayeeID";



                paymenttypecb.ItemsSource = db.PaymentTypes.ToList();
                paymenttypecb.DisplayMemberPath = "Name";
                paymenttypecb.SelectedValuePath = "PaymentTypeID";
                paymenttypecb.SelectedIndex = 0;

                departmentcb.ItemsSource = db.Departments.OrderBy(m => m.DepartmentName).ToList();
                departmentcb.DisplayMemberPath = "DepartmentName";
                departmentcb.SelectedValuePath = "DepartmentID";
                departmentcb.SelectedIndex = 0;
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
                try
                {
                    ImusCityHallEntities db = new ImusCityHallEntities();
                    if (payeecb.SelectedValue == null && optionpayeecb.IsChecked == false)
                    {
                        MessageBox.Show("Please select payee from the dropdown list");
                    }
                    else if (optionpayeecb.IsChecked == true)
                    {
                        MessageBox.Show("Please enter payee name");
                    }
                    else if (paymenttypecb.SelectedValue == null)
                    {
                        MessageBox.Show("Please select payment type");
                    }
                    else if (String.IsNullOrEmpty(vouchernotb.Text))
                    {
                        MessageBox.Show("Please enter voucher number");
                    }
                    else if (String.IsNullOrEmpty(descriptiontb.Text))
                    {
                        MessageBox.Show("Please enter description");
                    }
                    else if (String.IsNullOrEmpty(amounttb.Text))
                    {
                        MessageBox.Show("Please enter amount");
                    }
                    else
                    {
                        Disbursement disbursement = new Disbursement();
                        disbursement.PayeeID = optionpayeecb.IsChecked == true ? null : (int?)payeecb.SelectedValue;
                        disbursement.PaymentTypeID = (int)paymenttypecb.SelectedValue;
                        disbursement.VoucherNo = vouchernotb.Text;
                        disbursement.DateCreated = DateTime.Now;
                        disbursement.DepartmentID = (int)departmentcb.SelectedValue;
                        disbursement.ProjectName = projectnametb.Text;
                        disbursement.Description = descriptiontb.Text;
                        disbursement.Amount = Convert.ToDecimal(amounttb.Text);
                        disbursement.Obligated = obligatedcb.IsChecked;
                        disbursement.DocCompleted = documentcb.IsChecked;
                        disbursement.PayeeRepID = (int)payeerepcb.SelectedValue;
                        disbursement.PayeeName = optionalpayee.Text;
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
                        PrintCheck(x.DisbursementID);
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


        public void PrintCheck(int DisbursementID)
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
    }
}
