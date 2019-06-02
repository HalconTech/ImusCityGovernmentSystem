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
using MahApps.Metro.Controls;

namespace ImusCityGovernmentSystem.General.IdentificationCard
{
    /// <summary>
    /// Interaction logic for AddNewIdentificationCardWindow.xaml
    /// </summary>
    public partial class AddNewIdentificationCardWindow : MetroWindow
    {
        public AddNewIdentificationCardWindow()
        {
            InitializeComponent();
        }

        private void savebtn_Click(object sender, RoutedEventArgs e)
        {
            if(SystemClass.CheckConnection())
            {
                if(!String.IsNullOrEmpty(cardnametb.Text))
                {
                    ImusCityHallEntities db = new ImusCityHallEntities();
                    IdentificationCardType card = new IdentificationCardType();
                    card.CardType = cardnametb.Text;
                    card.IsActive = true;
                    card.DateAdded = DateTime.Now;
                    db.IdentificationCardTypes.Add(card);
                    db.SaveChanges();

                    var audit = new AuditTrailModel
                    {
                        Activity = "Added new identification card type in the database. CARD NAME: " + cardnametb.Text,
                        ModuleName = this.GetType().Name,
                        EmployeeID = App.EmployeeID
                    };

                    SystemClass.InsertLog(audit);
                    MessageBox.Show("New type of card inserted in the database");
                }
                else
                {
                    MessageBox.Show("Please type in the card type name");
                }
               
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }

        private void clearbtn_Click(object sender, RoutedEventArgs e)
        {
            cardnametb.Clear();
        }
    }
}
