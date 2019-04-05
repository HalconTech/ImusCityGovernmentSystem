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
    /// Interaction logic for AddNewBankWindow.xaml
    /// </summary>
    public partial class AddNewBankWindow : MetroWindow
    {
        public AddNewBankWindow()
        {
            InitializeComponent();
        }

        private void savebtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                if (String.IsNullOrEmpty(bankcodetb.Text) || String.IsNullOrEmpty(banknametb.Text))
                {
                    MessageBox.Show("Please input bank code and bank name");
                }
                else if (String.IsNullOrEmpty(branchtb.Text))
                {
                    MessageBox.Show("Please enter branch name");
                }
                else if (db.Banks.Any(m => m.BankCode == bankcodetb.Text))
                {
                    MessageBox.Show("The bank code is already used");
                }
                else
                {
                    ImusCityGovernmentSystem.Model.Bank bank = new Model.Bank();
                    bank.BankCode = bankcodetb.Text;
                    bank.BankName = banknametb.Text;
                    bank.Branch = branchtb.Text;
                    bank.IsActive = true;
                    db.Banks.Add(bank);
                    db.SaveChanges();
                    Mouse.OverrideCursor = null;

                    var audit = new AuditTrailModel
                    {
                        Activity = "Added new fund in the database. BANK CODE: " + bankcodetb.Text,
                        ModuleName = this.GetType().Name,
                        EmployeeID = App.EmployeeID
                    };

                    SystemClass.InsertLog(audit);

                    MessageBox.Show("New item added successfully.");

                    SystemClass.ClearTextBoxes(this);
                }
            }
            else
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }

            Mouse.OverrideCursor = null;
        }

        private void clearbtn_Click(object sender, RoutedEventArgs e)
        {
            SystemClass.ClearTextBoxes(this);
        }
    }
}
