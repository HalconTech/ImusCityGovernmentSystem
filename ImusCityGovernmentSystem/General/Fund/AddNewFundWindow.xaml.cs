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
namespace ImusCityGovernmentSystem.General.Fund
{
    /// <summary>
    /// Interaction logic for AddNewFundWindow.xaml
    /// </summary>
    public partial class AddNewFundWindow : MetroWindow
    {
     
        public AddNewFundWindow()
        {
            InitializeComponent();
            fundcodetb.Focus();
        }

        private void savebtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if(SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                if (String.IsNullOrEmpty(fundcodetb.Text) || String.IsNullOrEmpty(fundnametb.Text))
                {
                    MessageBox.Show("Please input fund code and fund name");
                }
                else if(String.IsNullOrEmpty(voucherprefixtb.Text))
                {
                    MessageBox.Show("Please enter voucher prefix");
                }
                else if(db.Funds.Any(m => m.FundCode == fundcodetb.Text))
                {
                    MessageBox.Show("The fund code is already used");
                }
                else if(db.Funds.Any(m => m.FundName == fundnametb.Text))
                {
                    MessageBox.Show("The fund name is already used");
                }
                else
                {
                    ImusCityGovernmentSystem.Model.Fund fund = new Model.Fund();
                    fund.FundCode = fundcodetb.Text;
                    fund.FundName = fundnametb.Text;
                    fund.FundPrefix = voucherprefixtb.Text;
                    fund.IsActive = true;
                    db.Funds.Add(fund);
                    db.SaveChanges();
                    Mouse.OverrideCursor = null;

                    var audit = new AuditTrailModel
                    {
                        Activity = "Added new fund in the database. FUND CODE: " + fundcodetb.Text,
                        ModuleName = this.GetType().Name,
                        EmployeeID = App.EmployeeID
                    };

                    SystemClass.InsertLog(audit);

                    MessageBox.Show("New item added successfully.");

                    SystemClass.ClearTextBoxes(this);
                }
            }
            else
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
          
            Mouse.OverrideCursor = null;
        }

        private void clearbtn_Click(object sender, RoutedEventArgs e)
        {
            SystemClass.ClearTextBoxes(this);
        }
    }
}
