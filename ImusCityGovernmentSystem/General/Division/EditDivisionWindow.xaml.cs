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
    /// Interaction logic for EditDivisionWindow.xaml
    /// </summary>
    public partial class EditDivisionWindow : MetroWindow
    {
        public int DivisionID;
        public EditDivisionWindow()
        {
            InitializeComponent();
        }

        private void savebtn_Click(object sender, RoutedEventArgs e)
        {
            DivisionUpdate();
        }

        public void DivisionUpdate()
        {
            if(SystemClass.CheckConnection())
            {
                try
                {
                    using (var db = new ImusCityHallEntities())
                    {
                        var find = db.Divisions.Find(DivisionID);
                        find.DivisionCode = txtCode.Text;
                        find.DivisionName = txtName.Text;
                        find.DepartmentID = (int)departmentcb.SelectedValue;
                        db.SaveChanges();

                        var audit = new AuditTrailModel
                        {
                            Activity = "Updated an item in division list. DIV ID: " + DivisionID.ToString(),
                            ModuleName = this.GetType().Name,
                            EmployeeID = App.EmployeeID
                        };

                        SystemClass.InsertLog(audit);
                        MessageBox.Show("Division updated successfully", "System Success!", MessageBoxButton.OK, MessageBoxImage.Information);
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
        public void LoadDivision()
        {
            if(SystemClass.CheckConnection())
            {
                try
                {
                    using (var db = new ImusCityHallEntities())
                    {
                        var find = db.Divisions.Find(DivisionID);

                        txtCode.Text = find.DivisionCode;
                        txtName.Text = find.DivisionName;
                        departmentcb.SelectedValue = find.DepartmentID;
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
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if(SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                departmentcb.ItemsSource = db.Departments.OrderBy(m => m.DepartmentName).ToList();
                departmentcb.DisplayMemberPath = "DepartmentName";
                departmentcb.SelectedValuePath = "DepartmentID";
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
            LoadDivision();
        }
    }
}
