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
                payeecb.ItemsSource = db.Payees.OrderBy(m => m.CompanyName).ToList();
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
                    if (payeecb.SelectedValue == null)
                    {
                        MessageBox.Show("Please select payee");
                    }
                    else if (paymenttypecb.SelectedValue == null)
                    {
                        MessageBox.Show("Please select payment type");
                    }
                    else if (String.IsNullOrEmpty(vouchernotb.Text))
                    {
                        MessageBox.Show("Please enter voucher number");
                    }
                    else if (departmentcb.SelectedValue == null)
                    {
                        MessageBox.Show("Please select department");
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
                        disbursement.PayeeID = (int)payeecb.SelectedValue;
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
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                db = new ImusCityHallEntities();
                if (DisbursementID == 0)
                {
                    return;
                }
                else
                {
                    
                    DVList = new List<DisbursementVoucherModel>();
                    var disburse = db.GetDisbursementVoucher(DisbursementID).ToList();
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
                        dvl.Signatory = x.Signatory;
                        dvl.Signatory2 = x.Signatory2;
                        dvl.Signatory3 = x.Signatory3;
                        DVList.Add(dvl);
                    }

                    if (DVList.Count != 0)
                    {
                        ReportWindow rw = new ReportWindow();
                        rw.DVList = DVList;
                        App.ReportID = 1;
                        rw.Show();

                    }
                    else
                    {
                        MessageBox.Show("Report data source is empty.");
                    }
                }
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }

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
