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
namespace ImusCityGovernmentSystem.General.IdentificationCard
{
    /// <summary>
    /// Interaction logic for IdentificationCardListWindow.xaml
    /// </summary>
    public partial class IdentificationCardListWindow : MetroWindow
    {
        public IdentificationCardListWindow()
        {
            InitializeComponent();
        }

        void LoadCards()
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                cardslb.ItemsSource = db.IdentificationCardTypes.Where(m => m.IsActive == true).OrderBy(m => m.CardType).ToList();
                cardslb.DisplayMemberPath = "CardType";
                cardslb.SelectedValuePath = "IdentificationCardTypeID";
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCards();
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
                        Activity = "Searched item in ID card list. SEARCH KEY: " + searchtb.Text,
                        ModuleName = this.GetType().Name,
                        EmployeeID = App.EmployeeID
                    };

                    SystemClass.InsertLog(audit);
                }
                searchbtn_Click(sender, e);
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
                        Activity = "Searched item in ID card list. SEARCH KEY: " + searchtb.Text,
                        ModuleName = this.GetType().Name,
                        EmployeeID = App.EmployeeID
                    };

                    SystemClass.InsertLog(audit);
                }

                ImusCityHallEntities db = new ImusCityHallEntities();
                cardslb.ItemsSource = db.IdentificationCardTypes.Where(m => m.CardType.Contains(searchtb.Text) && m.IsActive == true).OrderBy(m => m.CardType).ToList();
                cardslb.DisplayMemberPath = "CardType";
                cardslb.SelectedValuePath = "IdentificationCardTypeID";
            }
        }
        private void editbtn_Click(object sender, RoutedEventArgs e)
        {
            if(cardslb.SelectedValue == null)
            {
                MessageBox.Show("Please a card that you want to update");
            }
            else
            {
                IdentificationCard.EditIdentificationCardWindow edit = new EditIdentificationCardWindow();
                edit.id = (int)cardslb.SelectedValue;
                edit.ShowDialog();
                LoadCards();
            }
        }

        private void deletebtn_Click(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                if (cardslb.SelectedValue != null)
                {
                    ImusCityHallEntities db = new ImusCityHallEntities();
                    int id = (int)cardslb.SelectedValue;
                    ImusCityGovernmentSystem.Model.IdentificationCardType card = db.IdentificationCardTypes.Find(id);
                    card.IsActive = false;
                    db.SaveChanges();
                    LoadCards();

                    var audit = new AuditTrailModel
                    {
                        Activity = "Deleted item in the fund list. BANK ID: " + id.ToString(),
                        ModuleName = this.GetType().Name,
                        EmployeeID = App.EmployeeID
                    };
                    SystemClass.InsertLog(audit);
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

        private void addnewbankbtn_Click(object sender, RoutedEventArgs e)
        {
            IdentificationCard.AddNewIdentificationCardWindow add = new AddNewIdentificationCardWindow();
            add.ShowDialog();
            LoadCards();
        }
    }
}
