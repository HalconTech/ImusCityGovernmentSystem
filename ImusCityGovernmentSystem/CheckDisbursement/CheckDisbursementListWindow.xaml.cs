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
using ImusCityGovernmentSystem.CheckDisbursement;

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
                    checkcb.IsEnabled = false;
                    cashcb.IsEnabled = false;
                    otherscb.IsEnabled = false;
                    switch (dis.PaymentTypeID)
                    {
                        case (int)PaymentType.Cash:
                            checkcb.IsChecked = true;
                            cashcb.IsChecked = false;
                            otherscb.IsChecked = false;
                            break;
                        case (int)PaymentType.Check:
                            cashcb.IsChecked = true;
                            checkcb.IsChecked = false;
                            otherscb.IsChecked = false;
                            break;
                        case (int)PaymentType.Others:
                            otherscb.IsChecked = true;
                            cashcb.IsChecked = false;
                            checkcb.IsChecked = false;
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
            ImusCityGovernmentSystem.CheckDisbursement.AddCheckDisbursementWindow add = new CheckDisbursement.AddCheckDisbursementWindow();      
            add.Show();
        }
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            ReportWindow report = new ReportWindow();
            report.id = (int)voucherlistlb.SelectedValue;
            App.ReportID = 1;
            report.Show();
            Mouse.OverrideCursor = null;
        }

        private void checkbtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if(voucherlistlb.SelectedValue == null)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show("Please select an item");
            }
            else
            {
                CheckDisbursement.AddNewCheckEntryWindow addcheck = new CheckDisbursement.AddNewCheckEntryWindow();
                Mouse.OverrideCursor = null;
                addcheck.DisbursementID = (int)voucherlistlb.SelectedValue;
                addcheck.ShowDialog();
            }
            Mouse.OverrideCursor = null;
        }
    }
}
