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
namespace ImusCityGovernmentSystem.General.Department
{
    /// <summary>
    /// Interaction logic for AddNewDepartmentWindow.xaml
    /// </summary>
    public partial class AddNewDepartmentWindow : MetroWindow
    {
        public AddNewDepartmentWindow()
        {
            InitializeComponent();
        }

        private void savebtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            DepartmentAdd();
            Mouse.OverrideCursor = null;
        }
        public void TextClear()
        {
            txtCode.Clear();
            txtName.Clear();
            txtCode.Focus();
        }
        public void DepartmentAdd()
        {
            if (SystemClass.CheckConnection())
            {
                try
                {
                    using (var db = new ImusCityHallEntities())
                    {
                        if (!String.IsNullOrEmpty(txtCode.Text) && !String.IsNullOrEmpty(txtName.Text))
                        {
                            if(db.Departments.Any(m => m.DepartmentCode == txtCode.Text))
                            {
                                MessageBox.Show("Department code is already used");
                            }
                            else
                            {
                                Model.Department d = new Model.Department();
                                d.DepartmentCode = txtCode.Text;
                                d.DepartmentName = txtName.Text;
                                d.IsActive = true;
                                db.Departments.Add(d);
                                db.SaveChanges();

                                var audit = new AuditTrailModel
                                {
                                    Activity = "Added new department in the database. DEPT CODE: " + txtCode.Text,
                                    ModuleName = this.GetType().Name,
                                    EmployeeID = App.EmployeeID
                                };

                                SystemClass.InsertLog(audit);
                                MessageBox.Show("Department added successfully", "System Success!", MessageBoxButton.OK, MessageBoxImage.Information);
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

    }
}
