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
namespace ImusCityGovernmentSystem.General.Fund
{
    /// <summary>
    /// Interaction logic for FundListWindow.xaml
    /// </summary>
    public partial class FundListWindow : MetroWindow
    {

        public FundListWindow()
        {
            InitializeComponent();
        }

        private void searchtb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

                if (String.IsNullOrEmpty(searchtb.Text))
                {

                }
                else
                {
                    var audit = new AuditTrailModel
                    {
                        Activity = "Searched item in fund list. SEARCH KEY: " + searchtb.Text,
                        ModuleName = this.GetType().Name,
                        EmployeeID = App.EmployeeID
                    };

                    SystemClass.InsertLog(audit);
                }

                searchbtn_Click(sender, e);
            }
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                fundlb.ItemsSource = db.Funds.OrderByDescending(m => m.FundID).ToList();
                fundlb.DisplayMemberPath = "FundName";
                fundlb.SelectedValuePath = "FundID";
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
                if (String.IsNullOrEmpty(searchtb.Text))
                {

                }
                else
                {
                    var audit = new AuditTrailModel
                    {
                        Activity = "Searched item in fund list. SEARCH KEY: " + searchtb.Text,
                        ModuleName = this.GetType().Name,
                        EmployeeID = App.EmployeeID
                    };

                    SystemClass.InsertLog(audit);
                }

                ImusCityHallEntities db = new ImusCityHallEntities();
                fundlb.ItemsSource = db.Funds.OrderByDescending(m => m.FundID).Where(m => m.FundName.Contains(searchtb.Text)).ToList();
                fundlb.DisplayMemberPath = "FundName";
                fundlb.SelectedValuePath = "FundID";
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }

        private void editbtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (SystemClass.CheckConnection())
            {
                if (fundlb.SelectedValue == null)
                {
                    Mouse.OverrideCursor = null;
                    MessageBox.Show("Please select an item!");
                }
                else
                {
                    ImusCityHallEntities db = new ImusCityHallEntities();
                    EditFundWindow edit = new EditFundWindow();
                    edit.FundID = (int)fundlb.SelectedValue;
                    Mouse.OverrideCursor = null;
                    edit.ShowDialog();
                    db = new ImusCityHallEntities();
                    fundlb.ItemsSource = db.Funds.OrderByDescending(m => m.FundID).OrderBy(m => m.FundName).ToList();
                    fundlb.DisplayMemberPath = "FundName";
                    fundlb.SelectedValuePath = "FundID";
                }
            }
            else
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }


            Mouse.OverrideCursor = null;
        }

        private void addnewfunbtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                AddNewFundWindow add = new AddNewFundWindow();
                Mouse.OverrideCursor = null;
                add.ShowDialog();
                db = new ImusCityHallEntities();
                fundlb.ItemsSource = db.Funds.OrderByDescending(m => m.FundID).OrderBy(m => m.FundName).ToList();
                fundlb.DisplayMemberPath = "FundName";
                fundlb.SelectedValuePath = "FundID";
            }
            else
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
            Mouse.OverrideCursor = null;
        }
    }
}
