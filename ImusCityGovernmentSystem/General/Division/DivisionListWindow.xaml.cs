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
namespace ImusCityGovernmentSystem.General.Division
{
    /// <summary>
    /// Interaction logic for DivisionListWindow.xaml
    /// </summary>
    public partial class DivisionListWindow : MetroWindow
    {
        public DivisionListWindow()
        {
            InitializeComponent();
        }

        private void searchbtn_Click(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                if (String.IsNullOrEmpty(txtSearch.Text))
                {

                }
                else
                {
                    var audit = new AuditTrailModel
                    {
                        Activity = "Searched item in the division list. SEARCH KEY: " + txtSearch.Text,
                        ModuleName = this.GetType().Name,
                        EmployeeID = App.EmployeeID
                    };
                    SystemClass.InsertLog(audit);
                }
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }



            GetSearchedList(txtSearch.Text);
        }
        public class DivisionList
        {
            public int DivisionID { get; set; }
            public string DivisionCode { get; set; }
            public string DivisionName { get; set; }
        }
        public void TextClear()
        {
            txtSearch.Clear();
        }
        List<DivisionList> DList = new List<DivisionList>();
        public void GetList()
        {
            if (SystemClass.CheckConnection())
            {
                try
                {
                    DList = new List<DivisionList>();
                    using (var db = new ImusCityHallEntities())
                    {
                        var get = db.Divisions.Where(m => m.IsActive == true).OrderBy(m => m.DivisionName).ToList();

                        foreach (var item in get)
                        {
                            DivisionList dl = new DivisionList();
                            dl.DivisionID = item.DivisionID;
                            dl.DivisionCode = item.DivisionCode;
                            dl.DivisionName = item.DivisionName;

                            DList.Add(dl);
                        }
                        if (!String.IsNullOrEmpty(txtSearch.Text))
                            DList = DList.OrderByDescending(m => m.DivisionID).Where(m => m.DivisionCode.ToUpper().Contains(txtSearch.Text) || m.DivisionName.ToUpper().Contains(txtSearch.Text)).ToList();
                        dgDivisionList.ItemsSource = DList;
                        dgDivisionList.SelectedValuePath = "DivisionID";
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

        private void refreshbtn_Click(object sender, RoutedEventArgs e)
        {
            TextClear();
            GetList();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            GetList();
        }

        private void editbtn_Click(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                try
                {
                    using (var db = new ImusCityHallEntities())
                    {
                        if (dgDivisionList.SelectedItem != null)
                        {
                            var selectedItem = ((DivisionList)dgDivisionList.SelectedItem);
                            EditDivisionWindow ud = new EditDivisionWindow();
                            ud.DivisionID = selectedItem.DivisionID;
                            ud.ShowDialog();
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
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }

        }

        private void addbtn_Click(object sender, RoutedEventArgs e)
        {
            AddDivisionWindow ad = new AddDivisionWindow();
            ad.ShowDialog();
            TextClear();
            GetList();
        }
        public void GetSearchedList(string searchkey)
        {
            if (SystemClass.CheckConnection())
            {
                try
                {
                    DList = new List<DivisionList>();
                    using (var db = new ImusCityHallEntities())
                    {
                        var get = db.Divisions.Where(m => m.IsActive == true && (m.DivisionName.Contains(searchkey) || m.DivisionCode.Contains(searchkey))).OrderBy(m => m.DivisionName).ToList();

                        foreach (var item in get)
                        {
                            DivisionList dl = new DivisionList();
                            dl.DivisionID = item.DivisionID;
                            dl.DivisionCode = item.DivisionCode;
                            dl.DivisionName = item.DivisionName;

                            DList.Add(dl);
                        }
                        dgDivisionList.ItemsSource = DList.OrderByDescending(m => m.DivisionID);
                        dgDivisionList.SelectedValuePath = "DivisionID";
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

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                GetSearchedList(txtSearch.Text);
                var audit = new AuditTrailModel
                {
                    Activity = "Searched item in the division list. SEARCH KEY: " + txtSearch.Text,
                    ModuleName = this.GetType().Name,
                    EmployeeID = App.EmployeeID
                };
                SystemClass.InsertLog(audit);
            }
        }

        private void deletebtn_Click(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                if (dgDivisionList.SelectedValue != null)
                {
                    ImusCityHallEntities db = new ImusCityHallEntities();
                    int id = (int)dgDivisionList.SelectedValue;
                    ImusCityGovernmentSystem.Model.Division division = db.Divisions.Find(id);
                    division.IsActive = false;
                    db.SaveChanges();
                    var audit = new AuditTrailModel
                    {
                        Activity = "Deleted an item in the division list. DIV ID: " + id.ToString(),
                        ModuleName = this.GetType().Name,
                        EmployeeID = App.EmployeeID
                    };

                    SystemClass.InsertLog(audit);
                    GetList();
                }
                else
                {
                    MessageBox.Show("Please select an item");
                }
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }
    }
}
