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
    /// Interaction logic for AddNewCheckEntryWindow.xaml
    /// </summary>
    public partial class AddNewCheckEntryWindow : MetroWindow
    {
        public int DisbursementID;
        public AddNewCheckEntryWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                Disbursement disbursement = db.Disbursements.Find(DisbursementID);
                vouchernotb.Text = disbursement.VoucherNo;
                payeetb.Text = disbursement.Payee.CompanyName;
                descriptiontb.Text = disbursement.Description;
                paymenttypetb.Text = disbursement.PaymentType.Name;
                fundcb.ItemsSource = db.Funds.OrderBy(m => m.FundName).ToList();
                fundcb.DisplayMemberPath = "FundCode";
                fundcb.SelectedValuePath = "FundID";
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
                ImusCityHallEntities db = new ImusCityHallEntities();
                if (String.IsNullOrEmpty(checknotb.Text))
                {
                    MessageBox.Show("Please provide the check number");
                }
                else if (fundcb.SelectedValue == null)
                {
                    MessageBox.Show("Please select fund");
                }
                else if (String.IsNullOrEmpty(checkdesctb.Text))
                {
                    MessageBox.Show("Please provide check description");
                }
                else if (String.IsNullOrEmpty(checkamounttb.Text))
                {
                    MessageBox.Show("Please enter check amount");
                }
                else if(db.Checks.Any(m => m.CheckNo == checknotb.Text))
                {
                    MessageBox.Show("Check number is already been used");
                }
                else
                {
                   
                    Check check = new Check();
                    check.DisbursementID = DisbursementID;
                    check.FundID = (int)fundcb.SelectedValue;
                    check.CheckNo = checknotb.Text;
                    check.CheckDescription = checkdesctb.Text;
                    check.Amount = Convert.ToDecimal(checkamounttb.Text);
                    check.EmployeeID = App.EmployeeID;
                    check.DateCreated = DateTime.Now;
                    db.Checks.Add(check);
                    db.SaveChanges();

                    var audit = new AuditTrailModel
                    {
                        Activity = "Created new check entry DIS ID: " + DisbursementID.ToString(),
                        ModuleName = this.GetType().Name,
                        EmployeeID = App.EmployeeID
                    };

                    SystemClass.InsertLog(audit);
                    MessageBox.Show("Check created successfully!");
                }


            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }
    }
}
