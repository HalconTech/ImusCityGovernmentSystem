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
    /// Interaction logic for AddStatusWindow.xaml
    /// </summary>
    public partial class AddStatusWindow : MetroWindow
    {
        public AddStatusWindow()
        {
            InitializeComponent();
        }

        private void savebtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            StatusAdd();
            Mouse.OverrideCursor = null;
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            txtName.Focus();
        }
        public void StatusAdd()
        {
            if (SystemClass.CheckConnection())
            {

                try
                {
                    using (var db = new Model.ImusCityHallEntities())
                    {
                        if (!String.IsNullOrEmpty(txtName.Text) && !String.IsNullOrEmpty(txtCode.Text))
                        {
                            if(db.EmployeeStatus.Any(m => m.EmployeeStatusCode == txtCode.Text))
                            {
                                MessageBox.Show("Employee status code is already used");
                            }
                            else
                            {
                                Model.EmployeeStatu es = new Model.EmployeeStatu();
                                es.EmployeeStatusName = txtName.Text;
                                es.EmployeeStatusCode = txtCode.Text;
                                db.EmployeeStatus.Add(es);
                                db.SaveChanges();

                                var audit = new AuditTrailModel
                                {
                                    Activity = "Added new employee status in the database. STAT CODE: " + txtCode.Text,
                                    ModuleName = this.GetType().Name,
                                    EmployeeID = App.EmployeeID
                                };

                                SystemClass.InsertLog(audit);

                                MessageBox.Show("Status added successfully", "System Success!", MessageBoxButton.OK, MessageBoxImage.Information);
                                TextClear();
                            }
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
            txtName.Clear();
            txtCode.Clear();
            txtName.Focus();
        }
    }
}
