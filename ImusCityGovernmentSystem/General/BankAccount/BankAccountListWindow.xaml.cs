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
                accountslistlb.ItemsSource = db.FundBanks.OrderByDescending(m => m.FundBankID).ToList();
                accountslistlb.DisplayMemberPath = "AccountNumber";
                accountslistlb.SelectedValuePath = "FundBankID";

                bankcb.ItemsSource = db.Banks.OrderBy(m => m.BankName).ToList();
                bankcb.DisplayMemberPath = "BankName";
                bankcb.SelectedValuePath = "BankID";

                fundcb.ItemsSource = db.Funds.OrderBy(m => m.FundName).ToList();
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
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }

        private void accountslistlb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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
            if(SystemClass.CheckConnection())
            {
                int id = (int)accountslistlb.SelectedValue;
                ImusCityHallEntities db = new ImusCityHallEntities();
                FundBank account = db.FundBanks.Find(id);
                account.FundID = (int)fundcb.SelectedValue;
                account.BankID = (int)bankcb.SelectedValue;
                account.AccountNumber = accountnumbertb.Text;

            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }
    }
}
