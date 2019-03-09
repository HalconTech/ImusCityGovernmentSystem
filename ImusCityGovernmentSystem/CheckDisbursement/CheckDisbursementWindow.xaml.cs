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

namespace ImusCityGovernmentSystem.Check_Disbursement
{
    /// <summary>
    /// Interaction logic for CheckDisbursementWindow.xaml
    /// </summary>
    public partial class CheckDisbursementWindow : MetroWindow
    {
        public CheckDisbursementWindow()
        {
            InitializeComponent();
        }

        private void disbursementlistbtn_Click(object sender, RoutedEventArgs e)
        {
            CheckDisbursementListWindow checklist = new CheckDisbursementListWindow();
            checklist.ShowDialog();
        }

        private void adddisbursementbtn_Click(object sender, RoutedEventArgs e)
        {
            AddlCheckDisbursementWindow add = new AddlCheckDisbursementWindow();
            add.ShowDialog();
        }

        private void payeebtn_Click(object sender, RoutedEventArgs e)
        {
            General.Payee.PayeeListWindow payee = new General.Payee.PayeeListWindow();
            payee.ShowDialog();
        }

        private void fundbtn_Click(object sender, RoutedEventArgs e)
        {
            General.Fund.FundListWindow fund = new General.Fund.FundListWindow();
            fund.ShowDialog();
        }
    }
}
