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
    /// Interaction logic for DamageCheckList.xaml
    /// </summary>
    public partial class DamageCheckList : MetroWindow
    {
        public int fundBankId;
        public DamageCheckList()
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
                List<DamageCheck> damageCheckList = new List<DamageCheck>();
                foreach(var item in db.DamageChecks.Where(m => m.FundBankID == fundBankId))
                {
                    var damageCheck = new DamageCheck()
                    {
                        DamageCheckId = item.DamageCheckId,
                        CheckNumber = item.CheckNumber,
                        CreatedDate = item.CreatedDate
                    };

                    damageCheckList.Add(damageCheck);
                }
                damgecheckdg.ItemsSource = damageCheckList.OrderBy(m => m.CheckNumber);
                damgecheckdg.SelectedValuePath = "DamageCheckId";
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }

        private void deletebtn_Click(object sender, RoutedEventArgs e)
        {
            if(damgecheckdg.SelectedValue == null)
            {
                MessageBox.Show("Please select an item");
            }
            else
            {
                if (SystemClass.CheckConnection())
                {
                    ImusCityHallEntities db = new ImusCityHallEntities();
                    int damageCheckId = (int)damgecheckdg.SelectedValue;
                    DamageCheck damageCheckDelete = db.DamageChecks.Find(damageCheckId);
                    db.DamageChecks.Remove(damageCheckDelete);
                    db.SaveChanges();
                    MessageBox.Show("Deleted damage check");
                    db = new ImusCityHallEntities();
                    List<DamageCheck> damageCheckList = new List<DamageCheck>();
                    foreach (var item in db.DamageChecks.Where(m => m.FundBankID == fundBankId))
                    {
                        var damageCheck = new DamageCheck()
                        {
                            DamageCheckId = item.DamageCheckId,
                            CheckNumber = item.CheckNumber,
                            CreatedDate = item.CreatedDate
                        };

                        damageCheckList.Add(damageCheck);
                    }
                    damgecheckdg.ItemsSource = damageCheckList.OrderBy(m => m.CheckNumber);
                    damgecheckdg.SelectedValuePath = "DamageCheckId";
                }
                else
                {
                    MessageBox.Show(SystemClass.DBConnectionErrorMessage);
                }
            }
        }
    }
}
