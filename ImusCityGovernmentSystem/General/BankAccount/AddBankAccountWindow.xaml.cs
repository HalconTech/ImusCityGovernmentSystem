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
using System.Text.RegularExpressions;

namespace ImusCityGovernmentSystem.General.BankAccount
{
    /// <summary>
    /// Interaction logic for AddBankAccountWindow.xaml
    /// </summary>
    public partial class AddBankAccountWindow : MetroWindow
    {
        public AddBankAccountWindow()
        {
            InitializeComponent();
        }

        private void savebtn_Click(object sender, RoutedEventArgs e)
        {
            if(SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                if(fundcb.SelectedValue == null)
                {
                    MessageBox.Show("Please select fund");
                }
                else if(bankcb.SelectedValue == null)
                {
                    MessageBox.Show("Please select bank");
                }
                else if(String.IsNullOrEmpty(accountnumbertb.Text))
                {
                    MessageBox.Show("Please input account number");
                }
                else if(db.FundBanks.Any(m => m.AccountNumber == accountnumbertb.Text))
                {
                    MessageBox.Show("The account number is already used");
                }
                else if(String.IsNullOrEmpty(startingbalancetb.Text))
                {
                    MessageBox.Show("Please input starting balance");
                }
                else if(String.IsNullOrEmpty(currentbalancetb.Text))
                {
                    MessageBox.Show("Please input current balance");
                }
                else if(String.IsNullOrEmpty(advicenumbertb.Text))
                {
                    MessageBox.Show("Please input advice number");
                }
                else
                {
                    ImusCityGovernmentSystem.Model.FundBank account = new FundBank();
                    account.FundID = (int)fundcb.SelectedValue;
                    account.BankID = (int)bankcb.SelectedValue;
                    account.AccountNumber = accountnumbertb.Text;
                    account.CurrentBalance = Convert.ToDecimal(currentbalancetb.Text);
                    account.StartingBalance = Convert.ToDecimal(startingbalancetb.Text);
                    account.IsActive = true;
                    account.DateAdded = DateTime.Now;
                    account.AdviceNo = Convert.ToInt32(advicenumbertb.Text);
                    account.IsProcessed = true;
                    db.FundBanks.Add(account);
                    db.SaveChanges();
                    MessageBox.Show("Account added successfully");
                    SystemClass.ClearTextBoxes(this);
                    
                }
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            bool approvedDecimalPoint = false;

            if (e.Text == ".")
            {
                if (!((TextBox)sender).Text.Contains("."))
                    approvedDecimalPoint = true;
            }

            if (!(char.IsDigit(e.Text, e.Text.Length - 1) || approvedDecimalPoint))
                e.Handled = true;
        }
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
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

        private void clearbtn_Click(object sender, RoutedEventArgs e)
        {
            SystemClass.ClearTextBoxes(this);
        }
    }
}
