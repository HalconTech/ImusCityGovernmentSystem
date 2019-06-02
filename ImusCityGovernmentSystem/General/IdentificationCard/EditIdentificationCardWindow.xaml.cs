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
    /// Interaction logic for EditIdentificationCardWindow.xaml
    /// </summary>
    public partial class EditIdentificationCardWindow : MetroWindow
    {
        public int id;
        public EditIdentificationCardWindow()
        {
            InitializeComponent();
        }

        private void savebtn_Click(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                if (String.IsNullOrEmpty(cardnametb.Text))
                {
                    MessageBox.Show("Please enter card type name");
                }
                else
                {
                    ImusCityHallEntities db = new ImusCityHallEntities();
                    IdentificationCardType card = db.IdentificationCardTypes.Find(id);
                    card.CardType = cardnametb.Text;
                    db.SaveChanges();
                    var audit = new AuditTrailModel
                    {
                        Activity = "Updated an item in identification card list. CARD ID : " + id.ToString(),
                        ModuleName = this.GetType().Name,
                        EmployeeID = App.EmployeeID
                    };

                    SystemClass.InsertLog(audit);
                    MessageBox.Show("Card updated successfully!");
                }
                

            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                IdentificationCardType card = db.IdentificationCardTypes.Find(id);
                cardnametb.Text = card.CardType;
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }
    }
}
