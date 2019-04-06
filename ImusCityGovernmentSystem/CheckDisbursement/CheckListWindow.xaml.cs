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
                if (!String.IsNullOrEmpty(searchkey))
                {
                    ImusCityHallEntities db = new ImusCityHallEntities();
                    var result = from p in db.Checks
                                 select new
                                 {
                                     CheckID = p.CheckID,
                                     CheckNumber = p.CheckNo,
                                     VoucherNumber = p.Disbursement.VoucherNo,
                                     CompanyName = p.Disbursement.Payee.CompanyName,
                                     CheckDescription = p.CheckDescription,
                                     Amount = p.Amount,
                                     Status = p.Status == 0 ? "Created" : p.Status == 1 ? "Cancelled" : p.Status == 2 ? "Released" : "Released"
                                 };
                    if (payeerb.IsChecked == true)
                    {
                        checklistdg.ItemsSource = result.Where(m => m.CompanyName.Contains(searchkey)).OrderByDescending(m => m.CheckID).ToList();
                        checklistdg.SelectedValuePath = "CheckID";
                    }
                    else if (descrb.IsChecked == true)
                    {
                        checklistdg.ItemsSource = result.Where(m => m.CheckDescription.Contains(searchkey)).OrderByDescending(m => m.CheckID).ToList();
                        checklistdg.SelectedValuePath = "CheckID";
                    }
                    else if (checknorb.IsChecked == true)
                    {
                        checklistdg.ItemsSource = result.Where(m => m.CheckNumber.Contains(searchkey)).OrderByDescending(m => m.CheckID).ToList();
                        checklistdg.SelectedValuePath = "CheckID";
                    }
                    else if (allrb.IsChecked == true)
                    {
                        LoadItems();
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
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                var result = from p in db.Checks
                             select new
                             {
                                 CheckID = p.CheckID,
                                 CheckNumber = p.CheckNo,
                                 VoucherNumber = p.Disbursement.VoucherNo,
                                 CompanyName = p.Disbursement.Payee.CompanyName,
                                 CheckDescription = p.CheckDescription,
                                 Amount = p.Amount,
                                 Status = p.Status == 0 ? "Created" : p.Status == 1 ? "Cancelled" : p.Status == 2 ? "Released" : "Released"
                             };

                checklistdg.ItemsSource = result.OrderByDescending(m => m.CheckID).ToList();
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

        private void searchkeytb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                GetSearched(searchkeytb.Text);
            }
        }

        private void allrb_Checked(object sender, RoutedEventArgs e)
        {
            LoadItems();
        }

        private void editbtn_Click(object sender, RoutedEventArgs e)
        {
            if (checklistdg.SelectedValue != null)
            {
                int id = (int)checklistdg.SelectedValue;
                EditCheckWindow edit = new EditCheckWindow();
                edit.CheckID = id;
                edit.ShowDialog();
                LoadItems();
            }
            else
            {
                MessageBox.Show("Please select an entry");
            }
        }
    }
}
