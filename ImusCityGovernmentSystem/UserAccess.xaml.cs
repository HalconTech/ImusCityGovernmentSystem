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
namespace ImusCityGovernmentSystem
{
    /// <summary>
    /// Interaction logic for UserAccess.xaml
    /// </summary>
    public partial class UserAccess : MetroWindow
    {
        List<UserAccessModel> userAccessList = new List<UserAccessModel>();
        public UserAccess()
        {
            InitializeComponent();
        }

        void LoadEmployee()
        {
            ImusCityHallEntities db = new ImusCityHallEntities();
            var employees = from p in db.Employees
                            orderby p.FirstName
                            where p.EmployeeID != App.EmployeeID
                            select new
                            {
                                Name = p.FirstName + " " + p.MiddleName + " " + p.LastName,
                                EmployeeID = p.EmployeeID
                            };

            employeecb.ItemsSource = employees.ToList();
            employeecb.DisplayMemberPath = "Name";
            employeecb.SelectedValuePath = "EmployeeID";
            employeecb.SelectedIndex = 0;
        }

        public void LoadUserAccess(int id)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                userAccessList.Clear();
                db = new ImusCityHallEntities();
                var subModules = db.SubModules.OrderBy(m => m.Name).ToList();
                foreach (var subModule in subModules)
                {
                    bool selectedSubModule = false;
                    if (db.SubModuleUsers.Any(m => m.EmployeeID == id && m.SubModuleID == subModule.SubModuleID))
                    {
                        selectedSubModule = true;
                    }
                    UserAccessModel userAccess = new UserAccessModel()
                    {
                        Name = subModule.Name,
                        Id = subModule.SubModuleID,
                        IsSelected = selectedSubModule
                    };
                    userAccessList.Add(userAccess);
                }

                moduleslb.ItemsSource = userAccessList;
                moduleslb.SelectedValuePath = "Id";
                moduleslb.Items.Refresh();
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                LoadEmployee();
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }

        private void employeecb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                int employeeId = (int)employeecb.SelectedValue;
                LoadUserAccess(employeeId);
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
                if(employeecb.SelectedValue == null)
                {
                    MessageBox.Show("Please select/load an employee");
                }
                else
                {
                    ImusCityHallEntities db = new ImusCityHallEntities();
                    int employeeId = (int)employeecb.SelectedValue;
                    foreach (var userAccess in userAccessList.Where(m => m.IsSelected == true))
                    {
                        if (db.SubModuleUsers.Any(m => m.EmployeeID == employeeId && m.SubModuleID == userAccess.Id))
                        {

                        }
                        else
                        {
                            SubModuleUser userSubModule = new SubModuleUser();
                            userSubModule.EmployeeID = employeeId;
                            userSubModule.SubModuleID = userAccess.Id;       
                            db.SubModuleUsers.Add(userSubModule);
                            db.SaveChanges();
                        }
                    }

                    foreach (var userAccess in userAccessList.Where(m => m.IsSelected == false))
                    {
                        if (db.SubModuleUsers.Any(m => m.EmployeeID == employeeId && m.SubModuleID == userAccess.Id))
                        {
                            SubModuleUser userSubModule = db.SubModuleUsers.FirstOrDefault(m => m.EmployeeID == employeeId && m.SubModuleID == userAccess.Id );
                            db.SubModuleUsers.Remove(userSubModule);
                            db.SaveChanges();
                        }
                    }
                    MessageBox.Show("Employee user access updated successfully");
                }

            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
          
        }
    }
}
