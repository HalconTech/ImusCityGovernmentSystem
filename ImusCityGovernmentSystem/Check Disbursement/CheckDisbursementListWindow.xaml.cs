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
namespace ImusCityGovernmentSystem.Check_Disbursement
{
    /// <summary>
    /// Interaction logic for CheckDisbursementListWindow.xaml
    /// </summary>
    public partial class CheckDisbursementListWindow : MetroWindow
    {
        ImusCityHallEntities db = new ImusCityHallEntities();
        public CheckDisbursementListWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            voucherlistlb.ItemsSource = db.Disbursements.ToList();
            voucherlistlb.DisplayMemberPath = "VoucherNo";
            voucherlistlb.SelectedValuePath = "DisbursementID";
            voucherlistlb.SelectedIndex = 0;
        }

        private void voucherlistlb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
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
                switch(dis.PaymentType.Name)
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
            Mouse.OverrideCursor = null;
        }

        private void addbtn_Click(object sender, RoutedEventArgs e)
        {
            AddlCheckDisbursementWindow add = new AddlCheckDisbursementWindow();
            add.Show();
        }
    }
}
