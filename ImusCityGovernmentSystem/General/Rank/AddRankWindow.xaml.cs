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
    /// Interaction logic for AddRankWindow.xaml
    /// </summary>
    public partial class AddRankWindow : MetroWindow
    {
        public AddRankWindow()
        {
            InitializeComponent();
        }

        private void savebtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            RankAdd();
            Mouse.OverrideCursor = null;
        }
        public void RankAdd()
        {
            try
            {
                using (var db = new ImusCityHallEntities())
                {
                    if (!String.IsNullOrEmpty(txtName.Text))
                    {
                        Model.EmployeeRank er = new Model.EmployeeRank();
                        er.EmployeeRankName = txtName.Text;
                        db.EmployeeRanks.Add(er);
                        db.SaveChanges();

                        var audit = new AuditTrailModel
                        {
                            Activity = "Added new employee rank in the database. RANK NAME: " + txtName.Text,
                            ModuleName = this.GetType().Name,
                            EmployeeID = App.EmployeeID
                        };

                        SystemClass.InsertLog(audit);

                        MessageBox.Show("Rank added successfully", "System Success!", MessageBoxButton.OK, MessageBoxImage.Information);
                        TextClear();

                    }
                    else
                    {
                        MessageBox.Show("Fill up necessary fields.", "System Information!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong." + Environment.NewLine + ex.Message, "System Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        public void TextClear()
        {
            txtName.Clear();
            txtName.Focus();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            txtName.Focus();
        }
    }
}
