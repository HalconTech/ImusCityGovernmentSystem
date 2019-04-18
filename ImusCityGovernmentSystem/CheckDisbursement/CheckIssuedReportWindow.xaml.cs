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
using CrystalDecisions.CrystalReports.Engine;
using ImusCityGovernmentSystem.CheckDisbursement.Model;

namespace ImusCityGovernmentSystem.CheckDisbursement
{
    /// <summary>
    /// Interaction logic for CheckIssuedReportWindow.xaml
    /// </summary>
    public partial class CheckIssuedReportWindow : MetroWindow
    {
        public CheckIssuedReportWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                var fund = from p in db.FundBanks
                           where p.IsActive == true
                           select new
                           {
                               ID = p.FundBankID,
                               Name = p.Fund.FundName + " - " + p.Bank.BankCode + "(" + p.AccountNumber + ")"
                           };
                fundcb.ItemsSource = fund.ToList();
                fundcb.DisplayMemberPath = "Name";
                fundcb.SelectedValuePath = "ID";
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }

        private void generatebtn_Click(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                if (fundcb.SelectedValue == null)
                {
                    MessageBox.Show("Please select fund");
                }
                else if (String.IsNullOrEmpty(startdatedp.Text) && String.IsNullOrEmpty(enddatedp.Text))
                {
                    MessageBox.Show("Please select start date and end date");
                }
                else
                {
                    int accountId = (int)fundcb.SelectedValue;
                    ImusCityHallEntities db = new ImusCityHallEntities();
                    CDSSignatory signatories = db.CDSSignatories.FirstOrDefault();
                    FundBank account = db.FundBanks.Find(accountId);
                    List<CheckIssuedModel> list = new List<CheckIssuedModel>();
                    var result = db.GetCheckIssued(startdatedp.SelectedDate, enddatedp.SelectedDate, accountId);
                    foreach (var checkIssued in result)
                    {
                        var check = new CheckIssuedModel
                        {
                            StartDate = startdatedp.SelectedDate.Value,
                            EndDate = enddatedp.SelectedDate.Value,
                            BankName = checkIssued.BankName,
                            AccoutNumber = checkIssued.AccountNumber,
                            ReportNumber = checkIssued.FundPrefix + "-2015-03-",
                            DateCreated = checkIssued.DateCreated.Value,
                            CheckNo = checkIssued.CheckNo,
                            VoucherNo = checkIssued.VoucherNo,
                            Center = "",
                            CompanyName = checkIssued.CompanyName,
                            PaymentNature = checkIssued.PaymentNature,
                            Amount = checkIssued.Amount.Value,
                            DisbursingOfficer = SystemClass.GetSignatory(signatories.DisbursingOfficer)

                        };
                        list.Add(check);
                    }
                    ReportDocument report;
                    report = new CheckDisbursement.Report.CheckIssuedReport();
                    report.SetDataSource(list);
                    reportviewer.ViewerCore.ReportSource = report;
                }
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }
    }
}