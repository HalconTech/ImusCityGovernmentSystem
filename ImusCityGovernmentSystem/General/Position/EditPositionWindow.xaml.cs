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
namespace ImusCityGovernmentSystem.General.Position
{
    /// <summary>
    /// Interaction logic for EditPositionWindow.xaml
    /// </summary>
    public partial class EditPositionWindow : MetroWindow
    {
        public int PositionID;
        public EditPositionWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPosition();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            PositionUpdate();
        }
        public void LoadPosition()
        {
            try
            {
                using (var db = new ImusCityHallEntities())
                {
                    cbRank.ItemsSource = db.EmployeeRanks.OrderBy(m => m.EmployeeRankName).ToList();
                    cbRank.DisplayMemberPath = "EmployeeRankName";
                    cbRank.SelectedValuePath = "EmployeeRankID";

                    var find = db.EmployeePositions.Find(PositionID);
                    txtName.Text = find.EmployeePositionName;
                    txtDesc.Text = find.Description;
                    cbRank.SelectedValue = find.EmployeeRankID;
                    if (find.Active == true)
                        chkActive.IsChecked = true;
                    else
                        chkActive.IsChecked = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong." + Environment.NewLine + ex.Message, "System Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public void PositionUpdate()
        {
            try
            {
                using (var db = new ImusCityHallEntities())
                {
                    var find = db.EmployeePositions.Find(PositionID);
                    find.EmployeePositionName = txtName.Text;
                    find.Description = txtDesc.Text;
                    find.EmployeeRankID = Convert.ToInt32(cbRank.SelectedValue);
                    if (chkActive.IsChecked == true)
                        find.Active = true;
                    else
                        find.Active = false;
                    db.SaveChanges();

                    var audit = new AuditTrailModel
                    {
                        Activity = "Updated an item in employee position list. POS ID: " + PositionID.ToString(),
                        ModuleName = this.GetType().Name,
                        EmployeeID = App.EmployeeID
                    };

                    SystemClass.InsertLog(audit);
                    MessageBox.Show("Position updated successfully", "System Success!", MessageBoxButton.OK, MessageBoxImage.Information);
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
