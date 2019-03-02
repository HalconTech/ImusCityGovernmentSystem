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
namespace ImusCityGovernmentSystem
{
    /// <summary>
    /// Interaction logic for ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : MetroWindow
    {
        public ReportWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ReportDocument report = new ReportDocument();
            using (ImusCityHallEntities db = new ImusCityHallEntities())
            {
                report.SetDataSource(from c in db.Payees
                                     select new { c.PayeeID });
            }
            reportviewer.ViewerCore.ReportSource = report;
        }
    }
}
