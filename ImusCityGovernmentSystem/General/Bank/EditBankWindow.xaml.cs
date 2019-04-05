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
namespace ImusCityGovernmentSystem.General.Bank
{
    /// <summary>
    /// Interaction logic for EditBankWindow.xaml
    /// </summary>
    public partial class EditBankWindow : MetroWindow
    {
        public int BankID;
        public EditBankWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                ImusCityGovernmentSystem.Model.Bank bank = db.Banks.Find(BankID);
                bankcodetb.Text = bank.BankCode;
                banknametb.Text = bank.BankName;
                branchtb.Text = bank.Branch;
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }

        private void savebtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (SystemClass.CheckConnection())
            {
                if (String.IsNullOrEmpty(bankcodetb.Text) || String.IsNullOrEmpty(banknametb.Text))
                {
                    MessageBox.Show("Please input bank code and bank name!");
                }
                else if (String.IsNullOrEmpty(branchtb.Text))
                {
                    MessageBox.Show("Please enter branch name");
                }
                else
                {
                    ImusCityHallEntities db = new ImusCityHallEntities();
                    ImusCityGovernmentSystem.Model.Bank bank = db.Banks.Find(BankID);
                    bank.BankCode = bankcodetb.Text;
                    bank.BankName = banknametb.Text;
                    bank.Branch = branchtb.Text;
                    db.SaveChanges();
                    Mouse.OverrideCursor = null;
                    var audit = new AuditTrailModel
                    {
                        Activity = "Updated an item in bank list. BANK ID: " + BankID.ToString(),
                        ModuleName = this.GetType().Name,
                        EmployeeID = App.EmployeeID
                    };

                    SystemClass.InsertLog(audit);
                    MessageBox.Show("Bank updated successfully!");
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
