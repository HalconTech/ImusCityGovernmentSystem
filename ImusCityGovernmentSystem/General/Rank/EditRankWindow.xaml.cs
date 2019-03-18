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
namespace ImusCityGovernmentSystem.General.Rank
{
    /// <summary>
    /// Interaction logic for EditRankWindow.xaml
    /// </summary>
    public partial class EditRankWindow : MetroWindow
    {
        public int RankID;
        public EditRankWindow()
        {
            InitializeComponent();
        }

        private void savebtn_Click(object sender, RoutedEventArgs e)
        {
            RankUpdate();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadRank();
        }
        public void LoadRank()
        {
            try
            {
                using (var db = new ImusCityHallEntities())
                {
                    var find = db.EmployeeRanks.Find(RankID);
                    txtName.Text = find.EmployeeRankName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong." + Environment.NewLine + ex.Message, "System Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public void RankUpdate()
        {
            try
            {
                using (var db = new ImusCityHallEntities())
                {
                    var find = db.EmployeeRanks.Find(RankID);
                    find.EmployeeRankName = txtName.Text;
                    db.SaveChanges();

                    var audit = new AuditTrailModel
                    {
                        Activity = "Updated an item in employee rank list. RANK ID: " + RankID.ToString(),
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
    }
}
