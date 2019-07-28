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
            List<Disbursement> disbursementList = new List<Disbursement>();
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                foreach (var item in db.Disbursements)
                {
                    var disbursement = new Disbursement()
                    {
                        DisbursementID = item.DisbursementID,
                        VoucherNo = item.VoucherNo,
                        PayeeName = item.Payee == null ? item.PayeeName : item.Payee.CompanyName
                    };
                    disbursementList.Add(disbursement);
                }
                voucherlistdg.ItemsSource = disbursementList;
                voucherlistdg.SelectedValuePath = "DisbursementID";
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }

        }
        private void addbtn_Click(object sender, RoutedEventArgs e)
        {
            ImusCityGovernmentSystem.CheckDisbursement.AddCheckDisbursementWindow add = new CheckDisbursement.AddCheckDisbursementWindow();
            add.Show();
        }
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            ImusCityHallEntities db = new ImusCityHallEntities();
            CDSSignatory signatories = db.CDSSignatories.FirstOrDefault();
            if (voucherlistdg.SelectedValue == null)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show("Please select an item");
            }
            else if (signatories == null)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show("Please add report signatories");
            }
            else if (signatories.CItyAccountant.Equals(null) || signatories.CityTreasurer.Equals(null) || signatories.CityAdministrator.Equals(null))
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show("Please add report signatories");
            }
            else
            {
                ReportWindow report = new ReportWindow();
                report.id = (int)voucherlistdg.SelectedValue;
                App.ReportID = 1;
                report.Show();
                Mouse.OverrideCursor = null;
            }
            Mouse.OverrideCursor = null;
        }

        private void checkbtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (voucherlistdg.SelectedValue == null)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show("Please select an item");
            }
            else
            {
                CheckDisbursement.AddNewCheckEntryWindow addcheck = new CheckDisbursement.AddNewCheckEntryWindow();
                Mouse.OverrideCursor = null;
                addcheck.DisbursementID = (int)voucherlistdg.SelectedValue;

                if (SystemClass.CheckConnection())
                {
                    ImusCityHallEntities db = new ImusCityHallEntities();
                    CDSSignatory signatories = db.CDSSignatories.FirstOrDefault();
                    if (signatories == null)
                    {
                        MessageBox.Show("Please add report signatories");
                    }
                    else if (signatories.CityMayor.Equals(null) || signatories.CityTreasurer.Equals(null))
                    {
                        MessageBox.Show("Please add report signatories");
                    }
                    else
                    {
                        Disbursement disbursement = db.Disbursements.Find((int)voucherlistdg.SelectedValue);
                        var controlNumber = disbursement.FundBank.ControlNumbers.FirstOrDefault(m => m.Active == true);
                        if (controlNumber == null)
                        {
                            MessageBox.Show("Selected fund have no available check number");
                            return;
                        }
                        else
                        {
                            addcheck.ShowDialog();
                        }
                    }
                }
                else
                {
                    MessageBox.Show(SystemClass.DBConnectionErrorMessage);
                }


            }
            Mouse.OverrideCursor = null;
        }

        private void voucherlistdg_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                db = new ImusCityHallEntities();
                if (voucherlistdg.SelectedValue == null)
                {
                    return;
                }
                else
                {
                    int DisbursementID = (int)voucherlistdg.SelectedValue;
                    Disbursement dis = db.Disbursements.Find(DisbursementID);
                    vouchernotb.Text = dis.VoucherNo;
                    datetb.Text = dis.DateCreated.HasValue ? dis.DateCreated.Value.ToShortDateString() : null;
                    checkcb.IsEnabled = false;
                    cashcb.IsEnabled = false;
                    otherscb.IsEnabled = false;
                    switch (dis.PaymentTypeID)
                    {
                        case (int)PaymentType.Check:
                            checkcb.IsChecked = true;
                            cashcb.IsChecked = false;
                            otherscb.IsChecked = false;
                            paymenttypetb.Text = "Check";
                            break;
                        case (int)PaymentType.Cash:
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
                    payeetb.Text = dis.Payee == null ? dis.PayeeName : dis.Payee.CompanyName;
                    projectnametb.Text = dis.ProjectName;
                    departmenttb.Text = dis.Department == null ? null : dis.Department.DepartmentName;
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
    }
}
