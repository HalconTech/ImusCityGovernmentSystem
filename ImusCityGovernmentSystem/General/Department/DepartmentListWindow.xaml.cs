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
namespace ImusCityGovernmentSystem.General.Department
{
    /// <summary>
    /// Interaction logic for DepartmentListWindow.xaml
    /// </summary>
    public partial class DepartmentListWindow : MetroWindow
    {
        public DepartmentListWindow()
        {
            InitializeComponent();
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
                    Activity = "Searched item in the department list. SEARCH KEY: " + txtSearch.Text,
                    ModuleName = this.GetType().Name,
                    EmployeeID = App.EmployeeID
                };

                SystemClass.InsertLog(audit);
            }

            GetSearchedList(txtSearch.Text);
        }

        private void addbtn_Click(object sender, RoutedEventArgs e)
        {
            AddNewDepartmentWindow ad = new AddNewDepartmentWindow();
            ad.ShowDialog();
            TextClear();
            GetList();
        }


        public void GetSearchedList(string searchkey)
        {
            try
            {
                DList = new List<DepartmentList>();
                using (var db = new ImusCityHallEntities())
                {
                    var get = db.Departments.Where(m => m.DepartmentName.Contains(searchkey) || m.DepartmentCode.Contains(searchkey)).OrderBy(m => m.DepartmentName).ToList();

                    foreach (var item in get)
                    {
                        DepartmentList dl = new DepartmentList();
                        dl.DepartmentID = item.DepartmentID;
                        dl.DepartmentCode = item.DepartmentCode;
                        dl.DepartmentName = item.DepartmentName;
                        dl.DivisionID = item.DivisionID;
                        dl.DivisionName = item.Division.DivisionName;

                        DList.Add(dl);
                    }
               
                    dgDepartmentList.ItemsSource = DList.OrderByDescending(m => m.DepartmentID);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong." + Environment.NewLine + ex.Message, "System Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void refreshbtn_Click(object sender, RoutedEventArgs e)
        {
            TextClear();
            GetList();
        }

        private void editbtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new ImusCityHallEntities())
                {
                    if (dgDepartmentList.SelectedItem != null)
                    {
                        var selectedItem = ((DepartmentList)dgDepartmentList.SelectedItem);
                        EditDepartmentWindow ud = new EditDepartmentWindow();
                        ud.DepartmentID = selectedItem.DepartmentID;
                        ud.ShowDialog();
                        TextClear();
                        GetList();
                    }
                    else
                    {
                        MessageBox.Show("No department Selected", "System Warning!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong." + Environment.NewLine + ex.Message, "System Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            GetList();
        }
        public void TextClear()
        {
            txtSearch.Clear();
        }
        List<DepartmentList> DList = new List<DepartmentList>();
        public void GetList()
        {
            try
            {
                DList = new List<DepartmentList>();
                using (var db = new ImusCityHallEntities())
                {
                    var get = db.Departments.OrderByDescending(m => m.DepartmentID).ToList();

                    foreach (var item in get)
                    {
                        DepartmentList dl = new DepartmentList();
                        dl.DepartmentID = item.DepartmentID;
                        dl.DepartmentCode = item.DepartmentCode;
                        dl.DepartmentName = item.DepartmentName;
                        dl.DivisionID = item.DivisionID;
                        dl.DivisionName = item.Division.DivisionName;

                        DList.Add(dl);
                    }
                    if (!String.IsNullOrEmpty(txtSearch.Text))
                        DList = DList.Where(m => m.DepartmentCode.ToUpper().Contains(txtSearch.Text) || m.DepartmentName.ToUpper().Contains(txtSearch.Text)).ToList();

                
                    dgDepartmentList.ItemsSource = DList;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong." + Environment.NewLine + ex.Message, "System Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        public class DepartmentList
        {
            public int DepartmentID { get; set; }
            public string DepartmentName { get; set; }
            public string DepartmentCode { get; set; }
            public int? DivisionID { get; set; }
            public string DivisionName { get; set; }

        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                GetSearchedList(txtSearch.Text);
            }
        }

    }
}
