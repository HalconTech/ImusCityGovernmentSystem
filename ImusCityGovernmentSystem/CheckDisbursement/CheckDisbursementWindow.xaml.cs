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
namespace ImusCityGovernmentSystem.Check_Disbursement
{
    /// <summary>
    /// Interaction logic for CheckDisbursementWindow.xaml
    /// </summary>
    public partial class CheckDisbursementWindow : MetroWindow
    {
        public CheckDisbursementWindow()
        {
            InitializeComponent();
        }

        private void disbursementlistbtn_Click(object sender, RoutedEventArgs e)
        {
            CheckDisbursementListWindow checklist = new CheckDisbursementListWindow();
            checklist.ShowDialog();
        }

        private void adddisbursementbtn_Click(object sender, RoutedEventArgs e)
        {
            ImusCityGovernmentSystem.CheckDisbursement.AddCheckDisbursementWindow add = new CheckDisbursement.AddCheckDisbursementWindow();
            add.ShowDialog();
        }

        private void payeebtn_Click(object sender, RoutedEventArgs e)
        {
            General.Payee.PayeeListWindow payee = new General.Payee.PayeeListWindow();
            payee.ShowDialog();
        }

        private void fundbtn_Click(object sender, RoutedEventArgs e)
        {
            General.Fund.FundListWindow fund = new General.Fund.FundListWindow();
            fund.ShowDialog();
        }

        private void checkbtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            CheckDisbursement.CheckListWindow check = new CheckDisbursement.CheckListWindow();
            Mouse.OverrideCursor = null;
            check.ShowDialog();
        }

        private void checkregisterbtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            CheckDisbursement.CheckRegisterReport register = new CheckDisbursement.CheckRegisterReport();
            register.Show();
            Mouse.OverrideCursor = null;
        }

        private void accountantreportbtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            CheckDisbursement.AccountantsLocalCheckDisbursementReportWindow accountant = new CheckDisbursement.AccountantsLocalCheckDisbursementReportWindow();
            accountant.Show();
            Mouse.OverrideCursor = null;
        }

        private void bankbtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            General.Bank.BankListWindow bank = new General.Bank.BankListWindow();
            bank.Show();
            Mouse.OverrideCursor = null;
        }

        private void accountsbtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            General.BankAccount.BankAccountListWindow account = new General.BankAccount.BankAccountListWindow();
            account.Show();
            Mouse.OverrideCursor = null;
        }

        private void cdssignatoriesbtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            CheckDisbursement.CheckDisbursementSignatoryWindow signature = new CheckDisbursement.CheckDisbursementSignatoryWindow();
            Mouse.OverrideCursor = null;
            signature.Show();
        }

        private void checkissuedbtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            CheckDisbursement.CheckIssuedReportWindow check = new CheckDisbursement.CheckIssuedReportWindow();
            Mouse.OverrideCursor = null;
            check.Show();
        }

        private void checkreleasingbtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            CheckDisbursement.CheckReleasing.CheckReleasingWindow releasing = new CheckDisbursement.CheckReleasing.CheckReleasingWindow();
            Mouse.OverrideCursor = null;
            releasing.Show();
        }

        private void controlnumberbtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            General.ControlNumber.ControlNumberList control = new General.ControlNumber.ControlNumberList();
            Mouse.OverrideCursor = null;
            control.ShowDialog();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                LicensingCode license = db.LicensingCodes.FirstOrDefault(m => m.MachineName == Environment.MachineName && m.SubModule.Acronym == "CDS");

                if (license == null)
                {
                    MessageBox.Show("This machine is not yet licensed. Please contact your administrator");
                    LicenseCodeWindow lc = new LicenseCodeWindow();
                    lc.Show();
                    this.Close();
                }
                else if (license.ExpirationDate < DateTime.Now)
                {
                    MessageBox.Show("This license that have been issued to this machine is expired! Please input new license");
                    LicenseCodeWindow lc = new LicenseCodeWindow();
                    lc.Show();
                    this.Close();
                }
                else
                {
                    if (license.IsDemo == true)
                    {
                        App.LicenseKey = license.LicenseKey;
                        this.Title = "Check Disbursement Module (DEMO)";
                    }
                    else
                    {
                        App.LicenseKey = license.LicenseKey;
                        this.Title = "Check Disbursement Module";
                    }

                }
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }
    }
}
