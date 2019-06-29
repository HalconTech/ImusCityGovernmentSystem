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
namespace ImusCityGovernmentSystem.General.BankAccount
{
    /// <summary>
    /// Interaction logic for BankAccountListWindow.xaml
    /// </summary>
    public partial class BankAccountListWindow : MetroWindow
    {
        public BankAccountListWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                accountslistlb.ItemsSource = db.FundBanks.Where(m => m.IsActive == true).OrderByDescending(m => m.FundBankID).ToList();
                accountslistlb.DisplayMemberPath = "AccountNumber";
                accountslistlb.SelectedValuePath = "FundBankID";

                bankcb.ItemsSource = db.Banks.Where(m => m.IsActive == true).OrderBy(m => m.BankName).ToList();
                bankcb.DisplayMemberPath = "BankName";
                bankcb.SelectedValuePath = "BankID";

                fundcb.ItemsSource = db.Funds.Where(m => m.IsActive == true).OrderBy(m => m.FundName).ToList();
                fundcb.DisplayMemberPath = "FundName";
                fundcb.SelectedValuePath = "FundID";



            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }

        private void LoadSelected(int id)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                FundBank account = db.FundBanks.Find(id);

                bankcb.ItemsSource = db.Banks.OrderBy(m => m.BankName).ToList();
                bankcb.DisplayMemberPath = "BankName";
                bankcb.SelectedValuePath = "BankID";
                bankcb.SelectedValue = account.BankID;

                fundcb.ItemsSource = db.Funds.OrderBy(m => m.FundName).ToList();
                fundcb.DisplayMemberPath = "FundName";
                fundcb.SelectedValuePath = "FundID";
                fundcb.SelectedValue = account.FundID;

                accountnumbertb.Text = account.AccountNumber;
                startingbalancetb.Text = String.Format("{0:n}", account.StartingBalance);
                currentbalancetb.Text = String.Format("{0:n}", account.CurrentBalance);
                advicenumbertb.Text = account.AdviceNo.HasValue ? account.AdviceNo.ToString() : "";
                flooramounttb.Text = String.Format("{0:n}", account.AmountLimit);

            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }

        private void accountslistlb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (accountslistlb.SelectedValue == null)
            {
                return;
            }
            int id = (int)accountslistlb.SelectedValue;
            LoadSelected(id);
        }

        private void editbtn_Click(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                if (accountslistlb.SelectedValue != null)
                {
                    MessageBox.Show("You are now in edit mode, changes will apply after you click the save button");
                    fundcb.IsEnabled = true;
                    bankcb.IsEnabled = true;
                    accountnumbertb.IsEnabled = true;
                    flooramounttb.IsEnabled = true;
                    savebtn.IsEnabled = true;
                    editbtn.IsEnabled = false;
                }
                else
                {
                    MessageBox.Show("Please select an account number in the list");
                }

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
                int id = (int)accountslistlb.SelectedValue;
                FundBank account = db.FundBanks.Find(id);
                account.FundID = (int)fundcb.SelectedValue;
                account.BankID = (int)bankcb.SelectedValue;
                account.AccountNumber = accountnumbertb.Text;
                account.AmountLimit = Convert.ToDecimal(flooramounttb.Text);
                db.SaveChanges();

                var audit = new AuditTrailModel
                {
                    Activity = "Updated bank account in the database. FUND BANK ID: " + id.ToString(),
                    ModuleName = this.GetType().Name,
                    EmployeeID = App.EmployeeID
                };

                SystemClass.InsertLog(audit);
                MessageBox.Show("Account updated succesfully");

                fundcb.IsEnabled = false;
                bankcb.IsEnabled = false;
                accountnumbertb.IsEnabled = false;
                flooramounttb.IsEnabled = false;
                savebtn.IsEnabled = false;
                editbtn.IsEnabled = true;


                db = new ImusCityHallEntities();
                accountslistlb.ItemsSource = db.FundBanks.Where(m => m.IsActive == true).OrderByDescending(m => m.FundBankID).ToList();
                accountslistlb.DisplayMemberPath = "AccountNumber";
                accountslistlb.SelectedValuePath = "FundBankID";
                accountslistlb.SelectedValue = id;
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }

        private void addbtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            General.BankAccount.AddBankAccountWindow add = new AddBankAccountWindow();
            Mouse.OverrideCursor = null;
            add.ShowDialog();
            ImusCityHallEntities db = new ImusCityHallEntities();
            db = new ImusCityHallEntities();
            accountslistlb.ItemsSource = db.FundBanks.Where(m => m.IsActive == true).OrderByDescending(m => m.FundBankID).ToList();
            accountslistlb.DisplayMemberPath = "AccountNumber";
            accountslistlb.SelectedValuePath = "FundBankID";
        }

        private void adjustmentbtn_Click(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                if (accountslistlb.SelectedValue != null)
                {
                    int id = (int)accountslistlb.SelectedValue;
                    FundAdjustmentWindow adjustment = new FundAdjustmentWindow();
                    adjustment.FundBankID = id;
                    adjustment.ShowDialog();
                    LoadSelected(id);
                }
                else
                {
                    MessageBox.Show("Please select an account number in the list");
                }

            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }
    }
}
