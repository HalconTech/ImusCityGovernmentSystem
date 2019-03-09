using CrystalDecisions.CrystalReports.Engine;
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
using ImusCityGovernmentSystem.CheckDisbursement.Report;

namespace ImusCityGovernmentSystem
{
    /// <summary>
    /// Interaction logic for ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : MetroWindow
    {
        ReportDocument report;
        public List<DisbursementVoucherModel> DVList = new List<DisbursementVoucherModel>();

        public ReportWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {

            if (App.ReportID == 1)
            {
                if(DVList.Count != 0)
                {
                    report = new DisbursementVoucherReport();
                    report.SetDataSource(DVList);
                    reportviewer.ViewerCore.ReportSource = report;
                }
            }
        }
    }
}
