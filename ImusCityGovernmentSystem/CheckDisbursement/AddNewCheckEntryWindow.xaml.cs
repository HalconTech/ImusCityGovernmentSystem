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
            if(SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                Disbursement disbursement = db.Disbursements.Find(DisbursementID);
                vouchernotb.Text = disbursement.VoucherNo;
            }
        }
    }
}
