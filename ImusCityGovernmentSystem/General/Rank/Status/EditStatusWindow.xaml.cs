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
namespace ImusCityGovernmentSystem.General.Rank.Status
{
    /// <summary>
    /// Interaction logic for EditStatusWindow.xaml
    /// </summary>
    public partial class EditStatusWindow : MetroWindow
    {
        public int StatusID;
        public EditStatusWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadStatus();
        }
        public void LoadStatus()
        {
            try
            {
                using (var db = new ImusCityHallEntities())
                {
                    var find = db.EmployeeStatus.Find(StatusID);      
                    txtName.Text = find.EmployeeStatusName;
                    txtCode.Text = find.EmployeeStatusCode;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong." + Environment.NewLine + ex.Message, "System Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public void StatusUpdate()
        {
            try
            {
                using (var db = new ImusCityHallEntities())
                {
                    var find = db.EmployeeStatus.Find(StatusID);
                    find.EmployeeStatusName = txtName.Text;
                    find.EmployeeStatusCode = txtCode.Text;
                    db.SaveChanges();

                    var audit = new AuditTrailModel
                    {
                        Activity = "Updated an item in employee status list. STAT ID: " + StatusID.ToString(),
                        ModuleName = this.GetType().Name,
                        EmployeeID = App.EmployeeID
                    };

                    SystemClass.InsertLog(audit);
                    MessageBox.Show("Rank updated successfully", "System Success!", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong." + Environment.NewLine + ex.Message, "System Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            StatusUpdate();
        }
    }
}
