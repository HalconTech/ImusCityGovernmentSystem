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
        public UserAccess()
        {
            InitializeComponent();
        }

        void LoadEmployee()
        {
            ImusCityHallEntities db = new ImusCityHallEntities();
            var employees = from p in db.Employees
                            orderby p.FirstName
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

        void LoadFirstEmployee(int id)
        {
            ImusCityHallEntities db = new ImusCityHallEntities();
            assignedmodulelb.ItemsSource = db.SubModuleUsers.Where(m => m.EmployeeID == id).OrderBy(m => m.SubModule.Name).ToList();
            assignedmodulelb.DisplayMemberPath = "SubModule.Name";
            assignedmodulelb.SelectedValuePath = "SubModuleUserID";
        }
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                moduleslb.ItemsSource = db.SubModules.OrderBy(m => m.Name).ToList();
                moduleslb.DisplayMemberPath = "Name";
                moduleslb.SelectedValuePath = "SubModuleID";
                LoadEmployee();
                LoadFirstEmployee((int)employeecb.SelectedValue);


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
                ImusCityHallEntities db = new ImusCityHallEntities();
                assignedmodulelb.ItemsSource = db.SubModuleUsers.Where(m => m.EmployeeID == (int)employeecb.SelectedValue).OrderBy(m => m.SubModule.Name).ToList();
                assignedmodulelb.DisplayMemberPath = "SubModule.Name";
                assignedmodulelb.SelectedValuePath = "SubModuleUserID";
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }

        private void removebtn_Click(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                if (assignedmodulelb.SelectedValue != null)
                {
                    ImusCityHallEntities db = new ImusCityHallEntities();
                    SubModuleUser submoduleuser = db.SubModuleUsers.Find((int)assignedmodulelb.SelectedValue);
                    db.SubModuleUsers.Remove(submoduleuser);
                    db.SaveChanges();
                    db = new ImusCityHallEntities();
                    assignedmodulelb.ItemsSource = db.SubModuleUsers.Where(m => m.EmployeeID == (int)employeecb.SelectedValue).OrderBy(m => m.SubModule.Name).ToList();
                    assignedmodulelb.DisplayMemberPath = "SubModule.Name";
                    assignedmodulelb.SelectedValuePath = "SubModuleUserID";
                }
                else
                {
                    MessageBox.Show("Please select as assigned module that will be remove");
                }

            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }

        private void addbtn_Click(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                if (moduleslb.SelectedValue != null)
                {
                    ImusCityHallEntities db = new ImusCityHallEntities();
                    if (!db.SubModuleUsers.Any(m => m.EmployeeID == (int)employeecb.SelectedValue && m.SubModuleID == (int)moduleslb.SelectedValue))
                    {
                        SubModuleUser submoduleuser = new SubModuleUser();
                        var user = new SubModuleUser
                        {
                            SubModuleID = (int)moduleslb.SelectedValue,
                            EmployeeID = (int)employeecb.SelectedValue
                        };
                        db.SubModuleUsers.Add(user);
                        db.SaveChanges();
                        db = new ImusCityHallEntities();
                        assignedmodulelb.ItemsSource = db.SubModuleUsers.Where(m => m.EmployeeID == (int)employeecb.SelectedValue).OrderBy(m => m.SubModule.Name).ToList();
                        assignedmodulelb.DisplayMemberPath = "SubModule.Name";
                        assignedmodulelb.SelectedValuePath = "SubModuleUserID";
                    }
                    else
                    {
                        MessageBox.Show("The module that you are trying to add is already assigned to this employee");
                    }

                }
                else
                {
                    MessageBox.Show("Please select the module that needs to be assigned in this employee");
                }
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }
    }
}
