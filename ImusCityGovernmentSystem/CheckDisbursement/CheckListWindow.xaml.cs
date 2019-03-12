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
namespace ImusCityGovernmentSystem.CheckDisbursement
{
    /// <summary>
    /// Interaction logic for CheckListWindow.xaml
    /// </summary>
    public partial class CheckListWindow : MetroWindow
    {
        public CheckListWindow()
        {
            InitializeComponent();
            payeerb.IsChecked = true;
        }

        public void GetSearched(string searchkey)
        {
            if (SystemClass.CheckConnection())
            {
                if(!String.IsNullOrEmpty(searchkey))
                {
                    ImusCityHallEntities db = new ImusCityHallEntities();
                    if (payeerb.IsChecked == true)
                    {
                        checklistdg.ItemsSource = db.Checks.Where(m => m.Disbursement.Payee.CompanyName.Contains(searchkey)).OrderByDescending(m => m.CheckID).ToList();
                        checklistdg.SelectedValuePath = "CheckID";
                    }
                    else if (descrb.IsChecked == true)
                    {
                        checklistdg.ItemsSource = db.Checks.Where(m => m.CheckDescription.Contains(searchkey)).OrderByDescending(m => m.CheckID).ToList();
                        checklistdg.SelectedValuePath = "CheckID";
                    }
                    else if (checknorb.IsChecked == true)
                    {
                        checklistdg.ItemsSource = db.Checks.Where(m => m.CheckNo.Contains(searchkey)).OrderByDescending(m => m.CheckID).ToList();
                        checklistdg.SelectedValuePath = "CheckID";
                    }
                }
                else
                {
                    MessageBox.Show("Please enter search key");
                }
               
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }

        public void LoadItems()
        {
            if(SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                checklistdg.ItemsSource = db.Checks.OrderByDescending(m => m.CheckID).ToList();
                checklistdg.SelectedValuePath = "CheckID";
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
         
        }
        private void searchbtn_Click(object sender, RoutedEventArgs e)
        {
            GetSearched(searchkeytb.Text);
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadItems();
        }
    }
}
