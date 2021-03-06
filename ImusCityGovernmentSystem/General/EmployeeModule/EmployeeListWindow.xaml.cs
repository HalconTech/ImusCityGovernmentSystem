﻿using MahApps.Metro.Controls;
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
using System.IO;

namespace ImusCityGovernmentSystem.General.EmployeeModule
{
    /// <summary>
    /// Interaction logic for EmployeeListWindow.xaml
    /// </summary>
    public partial class EmployeeListWindow : MetroWindow
    {

        public EmployeeListWindow()
        {
            InitializeComponent();

            if (SystemClass.CheckConnection())
            {
                employeelistlb.ItemsSource = LoadEmployees(searchtb.Text);
                employeelistlb.DisplayMemberPath = "FirstName";
                employeelistlb.SelectedValuePath = "EmployeeID";
                employeelistlb.SelectedIndex = 0;
                searchtb.Focus();
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }


        }

        public List<Employee> LoadEmployees(string searchKey)
        {
            List<Employee> employeeList = new List<Employee>();
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();


                if (String.IsNullOrEmpty(searchKey))
                {
                    foreach (var employee in db.Employees.Where(m => m.EmployeeNo != "123456" && m.EmployeeNo != "0000001"))
                    {
                        employee.FirstName = string.Join(" ", employee.FirstName, employee.LastName);
                        employeeList.Add(employee);
                    }
                }
                else
                {
                    foreach (var employee in db.Employees.Where(m => (m.FirstName.Contains(searchKey) || m.MiddleName.Contains(searchKey) || m.LastName.Contains(searchKey)) && m.EmployeeNo != "123456" && m.EmployeeNo != "0000001"))
                    {
                        employee.FirstName = string.Join(" ", employee.FirstName, employee.LastName);
                        employeeList.Add(employee);
                    }
                }


                return employeeList;
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
                return employeeList;
            }

        }
        private void employeelistlb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                Mouse.OverrideCursor = Cursors.Wait;
                ImusCityHallEntities db = new ImusCityHallEntities();
                if (employeelistlb.SelectedValue == null)
                {
                    return;
                }
                else
                {
                    int EmployeeID = (int)employeelistlb.SelectedValue;
                    Employee employee = db.Employees.Find(EmployeeID);
                    employeenotb.Text = employee.EmployeeNo;
                    nametb.Text = employee.FirstName + " " + employee.MiddleName + " " + employee.LastName;
                    divisiontb.Text = employee.Division == null ? null : employee.Division.DivisionName;
                    positiontb.Text = employee.EmployeePosition == null ? null : employee.EmployeePosition.EmployeePositionName;
                    departmenttb.Text = employee.Division == null ? null : employee.Division.Department.DepartmentName;
                    emailtb.Text = employee.PrimaryEmail;
                    contactnotb.Text = employee.MobileNo;
                    if (employee.Photo == null)
                    {

                        this.empimage.Source = null;
                    }
                    else
                    {
                        Stream StreamObj = new MemoryStream(employee.Photo);
                        BitmapImage BitObj = new BitmapImage();
                        BitObj.BeginInit();
                        BitObj.StreamSource = StreamObj;
                        BitObj.EndInit();
                        this.empimage.Source = BitObj;

                    }
                    Mouse.OverrideCursor = null;

                }
                Mouse.OverrideCursor = null;
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }

        }

        private void editbtn_Click(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                if (employeelistlb.SelectedValue == null)
                {
                    MessageBox.Show("Please select an employee to edit!");
                }
                else
                {
                    EditEmployeeWindow editemp = new EditEmployeeWindow();
                    editemp.EmployeeID = (int)employeelistlb.SelectedValue;
                    editemp.ShowDialog();

                    employeelistlb.ItemsSource = LoadEmployees(searchtb.Text);
                    employeelistlb.DisplayMemberPath = "FirstName";
                    employeelistlb.SelectedValuePath = "EmployeeID";
                    employeelistlb.SelectedIndex = 0;


                }

            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }

        }

        private void addempbtn_Click(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                AddNewEmployeeWindow add = new AddNewEmployeeWindow();
                add.ShowDialog();
                employeelistlb.ItemsSource = LoadEmployees(searchtb.Text);
                employeelistlb.DisplayMemberPath = "FirstName";
                employeelistlb.SelectedValuePath = "EmployeeID";
                employeelistlb.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }


        }

        private void resetpasswordbtn_Click(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                if (employeelistlb.SelectedValue == null)
                {
                    MessageBox.Show("Please select an employee");
                }
                else
                {
                    var employee = db.Employees.Find((int)employeelistlb.SelectedValue);
                    AspNetUser aspuser = db.AspNetUsers.FirstOrDefault(m => m.UserName == employee.EmployeeNo);
                    var passwordHasher = new Microsoft.AspNet.Identity.PasswordHasher();
                    aspuser.PasswordHash = passwordHasher.HashPassword("imuscitygov");
                    db.SaveChanges();
                    MessageBox.Show("Employee account has been reset to" + Environment.NewLine + "Default Password: imuscitygov");
                }
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }


        }

        private void searchbtn_Click(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                Mouse.OverrideCursor = Cursors.Wait;
                employeelistlb.ItemsSource = LoadEmployees(searchtb.Text);
                employeelistlb.DisplayMemberPath = "FirstName";
                employeelistlb.SelectedValuePath = "EmployeeID";
                employeelistlb.SelectedIndex = 0;

                if (String.IsNullOrEmpty(searchtb.Text))
                {

                }
                else
                {
                    var audit = new AuditTrailModel
                    {
                        Activity = "Searched item in the employee list. SEARCH KEY: " + searchtb.Text,
                        ModuleName = this.GetType().Name,
                        EmployeeID = App.EmployeeID
                    };
                    SystemClass.InsertLog(audit);
                }

                Mouse.OverrideCursor = null;
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }

        }

        private void searchtb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (String.IsNullOrEmpty(searchtb.Text))
                {
                    MessageBox.Show("Please enter search key");
                }
                else
                {
                    searchbtn_Click(sender, e);

                }

            }
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                ImusCityGovernmentSystem.Model.Employee employee = db.Employees.Find(App.EmployeeID);
                resetpasswordbtn.IsEnabled = employee.IsAdmin == true ? true : false;
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }
    }
}
