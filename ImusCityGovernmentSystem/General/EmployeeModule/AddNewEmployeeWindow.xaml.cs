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
using System.Data.Entity.Validation;
using Microsoft.Win32;
using System.IO;
using System.Text.RegularExpressions;

namespace ImusCityGovernmentSystem.General.EmployeeModule
{
    /// <summary>
    /// Interaction logic for AddNewEmployeeWindow.xaml
    /// </summary>
    public partial class AddNewEmployeeWindow : MetroWindow
    {

        public string newimage;
        public AddNewEmployeeWindow()
        {
            InitializeComponent();


        }
        public bool CheckNumberOfUser()
        {
            ImusCityHallEntities db = new ImusCityHallEntities();
            int totalEmployees = db.Employees.Count() - 2;
            int maxUsers = db.SystemSettings.FirstOrDefault().NumberOfUser.Value;
            if (totalEmployees >= maxUsers)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        //Insertion of new employee information
        private void savebtn_Click(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                Mouse.OverrideCursor = Cursors.Wait;
                try
                {
                    ImusCityHallEntities db = new ImusCityHallEntities();
                    Employee employee = new Employee();

                    string birthdateInput = birthdatedp.Text;
                    DateTime returnBirthDate;

                    string dateHiredInput = datehiredp.Text;
                    DateTime returnDateHired;

                    if (db.Employees.Where(m => m.EmployeeNo == employeenotb.Text).FirstOrDefault() != null)
                    {
                        MessageBox.Show("Employee number already exists!", "System Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        Mouse.OverrideCursor = null;
                        return;
                    }
                    else if (String.IsNullOrEmpty(fnametb.Text))
                    {
                        MessageBox.Show("Please enter first name");
                        Mouse.OverrideCursor = null;
                    }
                    else if (String.IsNullOrEmpty(lnametb.Text))
                    {
                        MessageBox.Show("Please enter last name");
                        Mouse.OverrideCursor = null;
                    }
                    else if (String.IsNullOrEmpty(permaddtb.Text))
                    {
                        MessageBox.Show("Please enter permanent address");
                        Mouse.OverrideCursor = null;
                    }
                    else if (String.IsNullOrEmpty(primaryemailtb.Text))
                    {
                        MessageBox.Show("Please enter primary email");
                        Mouse.OverrideCursor = null;
                    }
                    else if (String.IsNullOrEmpty(employeenotb.Text))
                    {
                        MessageBox.Show("Please enter employee number");
                        Mouse.OverrideCursor = null;
                    }
                    else if (String.IsNullOrEmpty(datehiredp.Text))
                    {
                        MessageBox.Show("Please enter date hired");
                        Mouse.OverrideCursor = null;
                    }
                    else if (divisioncb.SelectedValue == null)
                    {
                        MessageBox.Show("Please select employee division");
                        Mouse.OverrideCursor = null;
                    }
                    else if (positioncb.SelectedValue == null)
                    {
                        MessageBox.Show("Please select employee position");
                        Mouse.OverrideCursor = null;
                    }
                    else if (statuscb.SelectedValue == null)
                    {
                        MessageBox.Show("Please select employee status");
                        Mouse.OverrideCursor = null;
                    }
                    else if (String.IsNullOrEmpty(birthdatedp.Text))
                    {
                        MessageBox.Show("Please enter birthday");
                        Mouse.OverrideCursor = null;
                    }
                    else if (String.IsNullOrEmpty(birthplacetb.Text))
                    {
                        MessageBox.Show("Please enter birthplace");
                        Mouse.OverrideCursor = null;
                    }
                    else if (genderdp.SelectedValue == null)
                    {
                        MessageBox.Show("Please select gender");
                        Mouse.OverrideCursor = null;
                    }
                    else if (String.IsNullOrEmpty(mobilenotb.Text))
                    {
                        MessageBox.Show("Please enter mobile number");
                        Mouse.OverrideCursor = null;
                    }
                    else if (!DateTime.TryParse(birthdateInput, out returnBirthDate))
                    {
                        MessageBox.Show("Please enter valid date format for birthdate (mm/dd/yyyy)");
                    }
                    else if(!DateTime.TryParse(dateHiredInput, out returnDateHired))
                    {
                        MessageBox.Show("Please enter valid date format for date hired (mm/dd/yyyy)");
                    }
                    else if(CheckNumberOfUser())
                    {
                        Mouse.OverrideCursor = null;
                        MessageBox.Show("Maximum number of users already reached. Please contact your vendor");
                    }
                    else
                    {
                        employee.EmployeeNo = employeenotb.Text;

                        int DivID, PosID, StatID;

                        //Employee Information
                        employee.EmployeeDivisionID = Int32.TryParse(divisioncb.SelectedValue.ToString(), out DivID) ? DivID : (int?)null;
                        employee.EmployeePositionID = Int32.TryParse(positioncb.SelectedValue.ToString(), out PosID) ? PosID : (int?)null;
                        employee.EmployeeStatusID = Int32.TryParse(statuscb.SelectedValue.ToString(), out StatID) ? StatID : (int?)null;
                        employee.DateHired = String.IsNullOrEmpty(datehiredp.Text) ? null : datehiredp.SelectedDate;
                        employee.DatePermanency = String.IsNullOrEmpty(datepermanetdp.Text) ? null : datepermanetdp.SelectedDate;
                        employee.DateEndContract = String.IsNullOrEmpty(dateendodp.Text) ? null : dateendodp.SelectedDate;
                        employee.DateResigned = String.IsNullOrEmpty(dateresignationdp.Text) ? null : dateresignationdp.SelectedDate;

                        //Personal Information
                        employee.FirstName = fnametb.Text;
                        employee.MiddleName = mnametb.Text;
                        employee.LastName = lnametb.Text;
                        employee.NameSuffix = namesuffixtb.Text;
                        employee.Birthday = birthdatedp.SelectedDate;
                        employee.Birthplace = birthplacetb.Text;
                        employee.Sex = genderdp.SelectionBoxItem.ToString().Substring(0, 1);
                        employee.Nationality = citizenshiptb.Text;
                        employee.CivilStatus = civilstatuscb.SelectionBoxItem.ToString();
                        employee.Religion = religiontb.Text;
                        employee.TIN = tinnotb.Text;
                        employee.SSS = sssnotb.Text;
                        employee.PhilHealth = philhealthnotb.Text;
                        employee.PAG_IBIG = pagibignotb.Text;

                        //Contact Information
                        employee.PermanentAddress = permaddtb.Text;
                        employee.CurrentAddress = currentadd.Text;
                        employee.TelephoneNo = landlinenotb.Text;
                        employee.MobileNo = mobilenotb.Text;
                        employee.PrimaryEmail = primaryemailtb.Text;
                        employee.SecondaryEmail = secondaryemailtb.Text;

                        employee.Archive = false;
                        employee.Photo = newimage == null ? null : File.ReadAllBytes(newimage);
                        employee.DateEncoded = DateTime.Now;


                        db.Employees.Add(employee);
                        db.SaveChanges();
                        Mouse.OverrideCursor = null;

                        MessageBox.Show("Employee was added to the database succesfully!");

                        db = new ImusCityHallEntities();
                        var newemp = db.Employees.Where(m => m.EmployeeNo == employeenotb.Text).FirstOrDefault();
                        if (db.AspNetUsers.Where(m => m.UserName == employee.EmployeeNo).FirstOrDefault() != null)
                        {
                            MessageBox.Show("User account for this person already exists!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else if (db.AspNetUsers.Where(m => m.Email == primaryemailtb.Text && m.Email != "").FirstOrDefault() != null)
                        {
                            MessageBox.Show("Email already exists!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else
                        {
                            string roleid = "2";
                            if (String.IsNullOrEmpty(roleid))
                            {
                                MessageBox.Show("Role is not specified", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                                return;
                            }
                            else
                            {

                                AspNetUser aspuser = new AspNetUser();
                                AspNetUserRole asproleuser = new AspNetUserRole();
                                aspuser.Id = Guid.NewGuid().ToString();
                                aspuser.UserName = newemp.EmployeeNo;
                                aspuser.Email = newemp.PrimaryEmail;
                                aspuser.EmailConfirmed = true;
                                aspuser.PhoneNumberConfirmed = false;
                                aspuser.TwoFactorEnabled = false;
                                aspuser.LockoutEnabled = true;
                                aspuser.AccessFailedCount = 0;
                                aspuser.SecurityStamp = Guid.NewGuid().ToString();
                                var passwordHasher = new Microsoft.AspNet.Identity.PasswordHasher();
                                aspuser.PasswordHash = passwordHasher.HashPassword("imuscitygov");
                                var adduser = db.AspNetUsers.Add(aspuser);
                                asproleuser.UserId = adduser.Id;
                                asproleuser.RoleId = roleid;
                                db.AspNetUserRoles.Add(asproleuser);
                            }
                        }
                        db.SaveChanges();

                        var audit = new AuditTrailModel
                        {
                            Activity = "Added new employee in the database. EMP NO: " + employeenotb.Text,
                            ModuleName = this.GetType().Name,
                            EmployeeID = App.EmployeeID
                        };

                        SystemClass.InsertLog(audit);

                        MessageBox.Show("Employee user account created" + Environment.NewLine + "Default Password: imuscitygov");
                        ClearTextBoxes();
                    }

                }
                catch (Exception ex)
                {
                    Mouse.OverrideCursor = null;
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }


        }

        private void ClearTextBoxes()
        {
            employeenotb.Text = null;
            datehiredp.SelectedDate = null;
            datepermanetdp.SelectedDate = null;
            dateendodp.SelectedDate = null;
            dateresignationdp.SelectedDate = null;

            //Personal Information
            fnametb.Text = null;
            mnametb.Text = null;
            lnametb.Text = null;
            namesuffixtb.Text = null;
            birthdatedp.SelectedDate = null;
            birthplacetb.Text = null;



            citizenshiptb.Text = null;

            religiontb.Text = null;
            tinnotb.Text = null;
            sssnotb.Text = null;
            philhealthnotb.Text = null;
            pagibignotb.Text = null;

            //Contact Information
            permaddtb.Text = null;
            currentadd.Text = null;
            landlinenotb.Text = null;
            mobilenotb.Text = null;
            primaryemailtb.Text = null;
            secondaryemailtb.Text = null;

            empimage.Source = null;


        }
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                divisioncb.ItemsSource = db.Divisions.Where(m => m.IsActive == true).OrderBy(m => m.DivisionCode).ToList();
                divisioncb.DisplayMemberPath = "DivisionCode";
                divisioncb.SelectedValuePath = "DivisionID";
                divisioncb.SelectedIndex = 0;

                positioncb.ItemsSource = db.EmployeePositions.Where(m => m.Active == true).OrderBy(m => m.EmployeePositionName).ToList();
                positioncb.DisplayMemberPath = "EmployeePositionName";
                positioncb.SelectedValuePath = "EmployeePositionID";
                positioncb.SelectedIndex = 0;

                statuscb.ItemsSource = db.EmployeeStatus.OrderBy(m => m.EmployeeStatusName).ToList();
                statuscb.DisplayMemberPath = "EmployeeStatusName";
                statuscb.SelectedValuePath = "EmployeeStatusID";
                statuscb.SelectedIndex = 0;

                genderdp.SelectedIndex = 0;
                civilstatuscb.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }

        }

        private void browsebtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" + "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic (*.png)|*.png";


            if (op.ShowDialog() == true)
            {
                newimage = op.FileName;
                ImageSource imageSource = new BitmapImage(new Uri(newimage));
                empimage.Source = imageSource;

            }
            Mouse.OverrideCursor = null;
        }

        private void clearbtn_Click(object sender, RoutedEventArgs e)
        {
            ClearTextBoxes();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            currentadd.Text = permaddtb.Text;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            currentadd.Text = null;
        }
    }
}
