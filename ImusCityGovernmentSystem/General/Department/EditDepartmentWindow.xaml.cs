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
namespace ImusCityGovernmentSystem.General.Department
{
    /// <summary>
    /// Interaction logic for EditDepartmentWindow.xaml
    /// </summary>
    public partial class EditDepartmentWindow : MetroWindow
    {
        public int DepartmentID;
        public EditDepartmentWindow()
        {
            InitializeComponent();
        }

        private void savebtn_Click(object sender, RoutedEventArgs e)
        {
            DepartmentUpdate();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDepartment();
        }
        public void DepartmentUpdate()
        {

            if (SystemClass.CheckConnection())
            {
                try
                {
                    using (var db = new ImusCityHallEntities())
                    {
                        var find = db.Departments.Find(DepartmentID);
                        find.DepartmentCode = txtCode.Text;
                        find.DepartmentName = txtName.Text;
                        find.DivisionID = Convert.ToInt32(cbDivision.SelectedValue);
                        db.SaveChanges();

                        var audit = new AuditTrailModel
                        {
                            Activity = "Updated an item in department list. DEPT ID: " + DepartmentID.ToString(),
                            ModuleName = this.GetType().Name,
                            EmployeeID = App.EmployeeID
                        };

                        SystemClass.InsertLog(audit);
                        MessageBox.Show("Department updated successfully", "System Success!", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
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
        public void LoadDepartment()
        {

            if (SystemClass.CheckConnection())
            {
                try
                {
                    using (var db = new ImusCityHallEntities())
                    {
                        cbDivision.ItemsSource = db.Divisions.OrderBy(m => m.DivisionName).ToList();
                        cbDivision.DisplayMemberPath = "DivisionCode";
                        cbDivision.SelectedValuePath = "DivisionID";
                        var find = db.Departments.Find(DepartmentID);
                        txtCode.Text = find.DepartmentCode;
                        txtName.Text = find.DepartmentName;
                        cbDivision.SelectedValue = find.DivisionID;


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
    }
}
