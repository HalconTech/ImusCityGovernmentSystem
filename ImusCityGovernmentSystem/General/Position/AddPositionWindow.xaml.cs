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
    /// Interaction logic for AddPositionWindow.xaml
    /// </summary>
    public partial class AddPositionWindow : MetroWindow
    {
        public AddPositionWindow()
        {
            InitializeComponent();
        }
        public void PositionAdd()
        {
            if(SystemClass.CheckConnection())
            {
                try
                {
                    using (var db = new ImusCityHallEntities())
                    {
                        if (!String.IsNullOrEmpty(txtName.Text) && !String.IsNullOrEmpty(txtDesc.Text) && !String.IsNullOrEmpty(cbRank.Text))
                        {
                            Model.EmployeePosition ep = new EmployeePosition();
                            ep.EmployeePositionName = txtName.Text;
                            ep.Description = txtDesc.Text;
                            ep.EmployeeRankID = Convert.ToInt32(cbRank.SelectedValue);
                            ep.Active = true;
                            ep.IsAdmin = admincb.IsChecked == true ? true : false;                      
                            db.EmployeePositions.Add(ep);
                            db.SaveChanges();

                            var audit = new AuditTrailModel
                            {
                                Activity = "Added new position in the database. DEPT CODE: " + txtName.Text,
                                ModuleName = this.GetType().Name,
                                EmployeeID = App.EmployeeID
                            };

                            SystemClass.InsertLog(audit);

                            MessageBox.Show("Position added successfully", "System Success!", MessageBoxButton.OK, MessageBoxImage.Information);
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
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
           
        }

        public void TextClear()
        {
            txtDesc.Clear();
            txtName.Clear();
            txtName.Focus();
            cbRank.Text = "";
           
        }
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if(SystemClass.CheckConnection())
            {
                try
                {
                    using (var db = new ImusCityHallEntities())
                    {
                        cbRank.ItemsSource = db.EmployeeRanks.OrderBy(m => m.EmployeeRankName).ToList();
                        cbRank.DisplayMemberPath = "EmployeeRankName";
                        cbRank.SelectedValuePath = "EmployeeRankID";
                        txtName.Focus();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Something went wrong." + Environment.NewLine + ex.Message, "System Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            PositionAdd();
            Mouse.OverrideCursor = null;
        }
    }
}
