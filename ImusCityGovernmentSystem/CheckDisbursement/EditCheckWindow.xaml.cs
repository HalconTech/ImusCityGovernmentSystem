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
    /// Interaction logic for EditCheckWindow.xaml
    /// </summary>
    public partial class EditCheckWindow : MetroWindow
    {
        public int CheckID;
        public EditCheckWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                foreach (var item in Enum.GetValues(typeof(CheckStatus)))
                {
                    checkstatuscb.Items.Add(item);
                }

                ImusCityHallEntities db = new ImusCityHallEntities();
                ImusCityGovernmentSystem.Model.Check check = db.Checks.Find(CheckID);
                checknumbertb.Text = check.CheckNo;
                checkdatecreateddp.Text = check.DateCreated.Value.ToShortDateString();
                checkstatuscb.SelectedIndex = check.Status.HasValue ? check.Status.Value : 0;
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
                ImusCityGovernmentSystem.Model.Check check = db.Checks.Find(CheckID);
                check.DateCreated = checkdatecreateddp.SelectedDate;
                check.Status = checkstatuscb.SelectedIndex;
                db.SaveChanges();
                MessageBox.Show("Check entry updated successfully");
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }
    }
}
