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
namespace ImusCityGovernmentSystem.General.Rank.Status
{
    /// <summary>
    /// Interaction logic for StatusListWindow.xaml
    /// </summary>
    public partial class StatusListWindow : MetroWindow
    {
        List<StatusList> SList = new List<StatusList>();
        public StatusListWindow()
        {
            InitializeComponent();
        }
        public class StatusList
        {
            public int StatusID { get; set; }
            public string StatusName { get; set; }
            public string StatusCode { get; set; }
        }
        private void searchbtn_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtSearch.Text))
            {

            }
            else
            {
                var audit = new AuditTrailModel
                {
                    Activity = "Searched item in the employee status list. SEARCH KEY: " + txtSearch.Text,
                    ModuleName = this.GetType().Name,
                    EmployeeID = App.EmployeeID
                };
                SystemClass.InsertLog(audit);
            }

            GetSearchedList(txtSearch.Text);
        }
        public void GetList()
        {
            try
            {
                SList = new List<StatusList>();
                using (var db = new Model.ImusCityHallEntities())
                {
                    var get = db.EmployeeStatus.OrderBy(m => m.EmployeeStatusName).ToList();

                    foreach (var item in get)
                    {
                        StatusList sl = new StatusList();
                        sl.StatusID = item.EmployeeStatusID;
                        sl.StatusName = item.EmployeeStatusName;
                        sl.StatusCode = item.EmployeeStatusCode;
                        SList.Add(sl);

                    }
                    if (!String.IsNullOrEmpty(txtSearch.Text))
                        SList = SList.Where(m => m.StatusName.ToUpper().Contains(txtSearch.Text) || m.StatusCode.ToUpper().Contains(txtSearch.Text)).ToList();
                    dgStatusList.ItemsSource = SList;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong." + Environment.NewLine + ex.Message, "System Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        public void TextClear()
        {
            txtSearch.Clear();
        }

        private void refreshbtn_Click(object sender, RoutedEventArgs e)
        {
            GetList();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            GetList();
        }

        private void addbtn_Click(object sender, RoutedEventArgs e)
        {
            AddStatusWindow add = new AddStatusWindow();
            add.ShowDialog();
            TextClear();
            GetList();
        }

        private void editbtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new ImusCityHallEntities())
                {
                    if (dgStatusList.SelectedItem != null)
                    {
                        var selectedItem = ((StatusList)dgStatusList.SelectedItem);
                        EditStatusWindow us = new EditStatusWindow();
                        us.StatusID = selectedItem.StatusID;
                        us.ShowDialog();
                        TextClear();
                        GetList();
                    }
                    else
                    {
                        MessageBox.Show("No division Selected", "System Warning!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong." + Environment.NewLine + ex.Message, "System Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        public void GetSearchedList(string searchkey)
        {
            try
            {
                SList = new List<StatusList>();
                using (var db = new Model.ImusCityHallEntities())
                {
                    var get = db.EmployeeStatus.Where(m => m.EmployeeStatusName.Contains(searchkey)).OrderBy(m => m.EmployeeStatusName).ToList();

                    foreach (var item in get)
                    {
                        StatusList sl = new StatusList();
                        sl.StatusID = item.EmployeeStatusID;
                        sl.StatusName = item.EmployeeStatusName;
                        sl.StatusCode = item.EmployeeStatusCode;
                        SList.Add(sl);

                    }

                    dgStatusList.ItemsSource = SList;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong." + Environment.NewLine + ex.Message, "System Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (String.IsNullOrEmpty(txtSearch.Text))
                {

                }
                else
                {
                    var audit = new AuditTrailModel
                    {
                        Activity = "Searched item in the employee status list. SEARCH KEY: " + txtSearch.Text,
                        ModuleName = this.GetType().Name,
                        EmployeeID = App.EmployeeID
                    };
                    SystemClass.InsertLog(audit);
                }

                GetSearchedList(txtSearch.Text);
            }
        }
    }
}
