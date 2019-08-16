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
namespace ImusCityGovernmentSystem.General.ControlNumber
{
    /// <summary>
    /// Interaction logic for AddDamageCheckWindow.xaml
    /// </summary>
    public partial class AddDamageCheckWindow : MetroWindow
    {
        public int fundBankId;
        public AddDamageCheckWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();

                FundBank fundBank = db.FundBanks.Find(fundBankId);
                fundname.Text = string.Join("/", fundBank.AccountNumber, fundBank.Fund.FundName, fundBank.Bank.BankCode);
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
                int result;
                if (int.TryParse(checknumbertb.Text, out result))
                {
                    string formatted = result.ToString("D10");
                    if (db.Checks.Where(m => m.Disbursement.FundBankID == fundBankId && m.CheckNo == formatted).Count() >= 1)
                    {
                        MessageBox.Show("The check number that you are trying to add with the corresponding fund is already used");
                    }
                    else if (db.DamageChecks.Where(m => m.FundBankID == fundBankId && m.CheckNumber == formatted).Count() >= 1)
                    {
                        MessageBox.Show("The check number that you are trying to add with the corresponding fund is already in the list of damaged check");
                    }
                    else if(String.IsNullOrEmpty(checknumbertb.Text))
                    {
                        MessageBox.Show("Please input check number");
                    }
                    else
                    {
                        DamageCheck damageCheck = new DamageCheck();
                        damageCheck.FundBankID = fundBankId;
                        damageCheck.CheckNumber = formatted;
                        damageCheck.CreatedDate = DateTime.Now;
                        damageCheck.CreatedBy = App.EmployeeID;
                        db.DamageChecks.Add(damageCheck);
                        db.SaveChanges();
                        MessageBox.Show("Damage check entry added successfuly");
                        checknumbertb.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("Please input numbers only");
                }

            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }
    }
}
