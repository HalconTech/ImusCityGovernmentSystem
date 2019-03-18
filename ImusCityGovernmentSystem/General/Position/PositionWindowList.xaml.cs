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
namespace ImusCityGovernmentSystem.General.Position
{
    /// <summary>
    /// Interaction logic for PositionWindowList.xaml
    /// </summary>
    public partial class PositionWindowList : MetroWindow
    {
        List<PositionList> PList = new List<PositionList>();
        public PositionWindowList()
        {
            InitializeComponent();
        }
        public class PositionList
        {
            public int PositionID { get; set; }
            public string PositionName { get; set; }
            public string Description { get; set; }
            public int? RankID { get; set; }
            public string Rank { get; set; }
            public bool? IsActive { get; set; }
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
                    Activity = "Searched item in the employee position list. SEARCH KEY: " + txtSearch.Text,
                    ModuleName = this.GetType().Name,
                    EmployeeID = App.EmployeeID
                };
                SystemClass.InsertLog(audit);
            }

            GetSearchedList(txtSearch.Text);
        }
        public void GetList()
        {
            if(SystemClass.CheckConnection())
            {
                try
                {
                    PList = new List<PositionList>();
                    using (var db = new Model.ImusCityHallEntities())
                    {
                        var get = db.EmployeePositions.OrderBy(m => m.EmployeePositionName).ToList();

                        foreach (var item in get)
                        {
                            PositionList pl = new PositionList();
                            pl.PositionID = item.EmployeePositionID;
                            pl.PositionName = item.EmployeePositionName;
                            pl.Description = item.Description;
                            pl.RankID = item.EmployeeRankID;
                            if (item.EmployeeRank == null)
                            {

                            }
                            else
                            {
                                pl.Rank = item.EmployeeRank.EmployeeRankName;
                            }

                            pl.IsActive = item.Active;
                            PList.Add(pl);
                        }
                        if (!String.IsNullOrEmpty(txtSearch.Text))
                            PList = PList.Where(m => m.PositionName.ToUpper().Contains(txtSearch.Text) || m.Description.ToUpper().Contains(txtSearch.Text)).ToList();
                        dgPositionList.ItemsSource = PList.OrderByDescending(m => m.PositionID);
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
        public void TextClear()
        {
            txtSearch.Clear();
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
            if(SystemClass.CheckConnection())
            {
                try
                {
                    using (var db = new ImusCityHallEntities())
                    {
                        if (dgPositionList.SelectedItem != null)
                        {
                            var selectedItem = ((PositionList)dgPositionList.SelectedItem);
                            EditPositionWindow up = new EditPositionWindow();
                            up.PositionID = selectedItem.PositionID;
                            up.ShowDialog();
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
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
         
        }

        private void addbtn_Click(object sender, RoutedEventArgs e)
        {
            AddPositionWindow ap = new AddPositionWindow();
            ap.ShowDialog();
            TextClear();
            GetList();
        }
        public void GetSearchedList(string searchkey)
        {
            if(SystemClass.CheckConnection())
            {
                try
                {
                    PList = new List<PositionList>();
                    using (var db = new Model.ImusCityHallEntities())
                    {
                        var get = db.EmployeePositions.Where(m => m.EmployeePositionName.Contains(searchkey)).OrderBy(m => m.EmployeePositionName).ToList();

                        foreach (var item in get)
                        {
                            PositionList pl = new PositionList();
                            pl.PositionID = item.EmployeePositionID;
                            pl.PositionName = item.EmployeePositionName;
                            pl.Description = item.Description;
                            pl.RankID = item.EmployeeRankID;
                            if (item.EmployeeRank == null)
                            {

                            }
                            else
                            {
                                pl.Rank = item.EmployeeRank.EmployeeRankName;
                            }

                            pl.IsActive = item.Active;
                            PList.Add(pl);
                        }
                        dgPositionList.ItemsSource = PList.OrderByDescending(m => m.PositionID);
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
                if (String.IsNullOrEmpty(txtSearch.Text))
                {

                }
                else
                {
                    var audit = new AuditTrailModel
                    {
                        Activity = "Searched item in the employee position list. SEARCH KEY: " + txtSearch.Text,
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
