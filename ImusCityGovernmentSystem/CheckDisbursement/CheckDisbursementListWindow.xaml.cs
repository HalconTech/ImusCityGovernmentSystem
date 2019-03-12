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
using ImusCityGovernmentSystem;
namespace ImusCityGovernmentSystem.Check_Disbursement
{
    /// <summary>
    /// Interaction logic for CheckDisbursementListWindow.xaml
    /// </summary>
    public partial class CheckDisbursementListWindow : MetroWindow
    {

        public List<DisbursementVoucherModel> DVList = new List<DisbursementVoucherModel>();

        public CheckDisbursementListWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                voucherlistlb.ItemsSource = db.Disbursements.OrderByDescending(m => m.DisbursementID).ToList();
                voucherlistlb.DisplayMemberPath = "VoucherNo";
                voucherlistlb.SelectedValuePath = "DisbursementID";
                voucherlistlb.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }

        }

        private void voucherlistlb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                db = new ImusCityHallEntities();
                if (voucherlistlb.SelectedValue == null)
                {
                    return;
                }
                else
                {

                    int DisbursementID = (int)voucherlistlb.SelectedValue;
                    Disbursement dis = db.Disbursements.Find(DisbursementID);
                    vouchernotb.Text = dis.VoucherNo;
                    datetb.Text = dis.DateCreated.HasValue ? dis.DateCreated.Value.ToShortDateString() : null;
                    switch (dis.PaymentType.Name)
                    {
                        case "Check":
                            checkcb.IsChecked = true;
                            break;
                        case "Cash":
                            cashcb.IsChecked = true;
                            break;
                        case "Others":
                            otherscb.IsChecked = true;
                            break;
                    }
                    payeetb.Text = dis.Payee.CompanyName;
                    projectnametb.Text = dis.ProjectName;
                    departmenttb.Text = dis.Department.DepartmentName;
                    descriptiontb.Text = dis.Description;
                    obligatedcb.IsChecked = dis.Obligated;
                    documentcb.IsChecked = dis.DocCompleted;
                    amounttb.Text = String.Format("{0:n}", dis.Amount);
                    Mouse.OverrideCursor = null;

                }
            }
            else
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
         

            
            Mouse.OverrideCursor = null;
        }

        private void addbtn_Click(object sender, RoutedEventArgs e)
        {
            AddlCheckDisbursementWindow add = new AddlCheckDisbursementWindow();
            add.Show();
        }
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if(SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                db = new ImusCityHallEntities();
                if (voucherlistlb.SelectedValue == null)
                {
                    return;
                }
                else
                {
                    int DisbursementID = (int)voucherlistlb.SelectedValue;

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
    }
}
