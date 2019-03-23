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
namespace ImusCityGovernmentSystem.General.Division
{
    /// <summary>
    /// Interaction logic for AddDivisionWindow.xaml
    /// </summary>
    public partial class AddDivisionWindow : MetroWindow
    {
        public AddDivisionWindow()
        {
            InitializeComponent();
        }

        private void savebtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            DivisionAdd();
            Mouse.OverrideCursor = null;
        }
        public void DivisionAdd()
        {
            if(SystemClass.CheckConnection())
            {
                try
                {
                    using (var db = new ImusCityHallEntities())
                    {
                        if (!String.IsNullOrEmpty(txtCode.Text) && !String.IsNullOrEmpty(txtName.Text))
                        {
                            Model.Division d = new Model.Division();
                            d.DivisionCode = txtCode.Text;
                            d.DivisionName = txtName.Text;
                            d.IsActive = true;
                            db.Divisions.Add(d);
                            db.SaveChanges();

                            var audit = new AuditTrailModel
                            {
                                Activity = "Added new division in the database. DEPT CODE: " + txtCode.Text,
                                ModuleName = this.GetType().Name,
                                EmployeeID = App.EmployeeID
                            };

                            SystemClass.InsertLog(audit);
                            MessageBox.Show("Division added successfully", "System Success!", MessageBoxButton.OK, MessageBoxImage.Information);
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
            txtCode.Clear();
            txtName.Clear();
            txtCode.Focus();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            txtCode.Focus();
        }
    }
}
