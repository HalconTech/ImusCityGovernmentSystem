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
namespace ImusCityGovernmentSystem.General.Rank
{
    /// <summary>
    /// Interaction logic for RankListWindow.xaml
    /// </summary>
    public partial class RankListWindow : MetroWindow
    {
        public class RankList
        {
            public int RankID { get; set; }
            public string RankName { get; set; }
        }
        List<RankList> RList = new List<RankList>();
        public RankListWindow()
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
                    Activity = "Searched item in the empoyee rank list. SEARCH KEY: " + txtSearch.Text,
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
                RList = new List<RankList>();
                using (var db = new ImusCityHallEntities())
                {
                    var get = db.EmployeeRanks.OrderBy(m => m.EmployeeRankName).ToList();

                    foreach (var item in get)
                    {
                        RankList rl = new RankList();
                        rl.RankID = item.EmployeeRankID;
                        rl.RankName = item.EmployeeRankName;
                        RList.Add(rl);

                    }
                    if (!String.IsNullOrEmpty(txtSearch.Text))
                        RList = RList.Where(m => m.RankName.ToUpper().Contains(txtSearch.Text)).ToList();
                    dgRankList.ItemsSource = RList;
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
            TextClear();
            GetList();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            GetList();
        }

        private void addbtn_Click(object sender, RoutedEventArgs e)
        {
            AddRankWindow ar = new AddRankWindow();
            ar.ShowDialog();
            TextClear();
            GetList();
        }

        private void editbtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new ImusCityHallEntities())
                {
                    if (dgRankList.SelectedItem != null)
                    {
                        var selectedItem = ((RankList)dgRankList.SelectedItem);
                        EditRankWindow ud = new EditRankWindow();
                        ud.RankID = selectedItem.RankID;
                        ud.ShowDialog();
                        TextClear();
                        GetList();
                    }
                    else
                    {
                        MessageBox.Show("No rank Selected", "System Warning!", MessageBoxButton.OK, MessageBoxImage.Information);
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
                RList = new List<RankList>();
                using (var db = new ImusCityHallEntities())
                {
                    var get = db.EmployeeRanks.Where(m => m.EmployeeRankName.Contains(searchkey)).OrderBy(m => m.EmployeeRankName).ToList();

                    foreach (var item in get)
                    {
                        RankList rl = new RankList();
                        rl.RankID = item.EmployeeRankID;
                        rl.RankName = item.EmployeeRankName;
                        RList.Add(rl);

                    }
                    dgRankList.ItemsSource = RList;
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
                        Activity = "Searched item in the empoyee rank list. SEARCH KEY: " + txtSearch.Text,
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
