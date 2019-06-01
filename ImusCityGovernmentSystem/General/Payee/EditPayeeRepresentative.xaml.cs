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
namespace ImusCityGovernmentSystem.General.Payee
{
    /// <summary>
    /// Interaction logic for EditPayeeRepresentative.xaml
    /// </summary>
    public partial class EditPayeeRepresentative : MetroWindow
    {
        public int id;
        public EditPayeeRepresentative()
        {
            InitializeComponent();
        }

        public void LoadPayee()
        {
            if(SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                payeecb.ItemsSource = db.Payees.OrderBy(m => m.CompanyName).ToList();
                payeecb.DisplayMemberPath = "CompanyName";
                payeecb.SelectedValuePath = "PayeeID";
            }   
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if(SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                PayeeRepresentative representative = db.PayeeRepresentatives.Find(id);
                fnametb.Text = representative.FirstName;
                mnametb.Text = representative.MiddleName;
                lnametb.Text = representative.LastName;
                mobilenotb.Text = representative.MobileNo;
                landlinetb.Text = representative.Landline;
                payeecb.SelectedValue = representative.PayeeID;
                  
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }

        private void savebtn_Click(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                if(payeecb.SelectedValue == null)
                {
                    MessageBox.Show("Please select payee");
                }
                else
                {
                    ImusCityHallEntities db = new ImusCityHallEntities();
                    PayeeRepresentative representative = db.PayeeRepresentatives.Find(id);
                    representative.FirstName = fnametb.Text;
                    representative.MiddleName = mnametb.Text;
                    representative.LastName = lnametb.Text;
                    representative.MobileNo = mobilenotb.Text;
                    representative.Landline = landlinetb.Text;
                    representative.PayeeID = (int)payeecb.SelectedValue;
                }
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }
    }
}
