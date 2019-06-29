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
namespace ImusCityGovernmentSystem
{
    /// <summary>
    /// Interaction logic for LicenseCodeWindow.xaml
    /// </summary>
    public partial class LicenseCodeWindow : MetroWindow
    {
        public LicenseCodeWindow()
        {
            InitializeComponent();
        }

        private void acceptbtn_Click(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                LicensingCode license = db.LicensingCodes.FirstOrDefault(m => m.LicenseKey == licensekey.Text);

                if (license != null && license.ExpirationDate > DateTime.Now.Date)
                {
                    //license.MachineName = Environment.MachineName;
                    license.UserID = App.EmployeeID;
                    db.SaveChanges();
                    MessageBox.Show("License key applied");
                    App.LicenseKey = licensekey.Text;
                    this.Close();
                }
                else if (String.IsNullOrEmpty(licensekey.Text))
                {
                    MessageBox.Show("Please enter a valid license key.");
                }
                else if (license == null)
                {
                    MessageBox.Show("Please enter a valid license key.");
                }
                else if (license.ExpirationDate < DateTime.Now)
                {
                    MessageBox.Show("The license that youve entering is expired!");
                }
                else if (license.LicenseKey == licensekey.Text && license.UserID == App.EmployeeID)
                {
                    MessageBox.Show("This license is associated with another user");
                }
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }

        }
    }
}
