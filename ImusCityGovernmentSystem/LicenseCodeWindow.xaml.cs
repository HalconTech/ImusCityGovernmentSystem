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
            ImusCityHallEntities db = new ImusCityHallEntities();
            LicensingCode license = db.LicensingCodes.FirstOrDefault(m => m.LicenseKey == licensekey.Text);

            if(license != null && license.ExpirationDate > DateTime.Now.Date)
            {
                license.MachineName = Environment.MachineName;
                db.SaveChanges();
                LogInWindow login = new LogInWindow();
                login.Show();
                App.LicenseKey = licensekey.Text;
                this.Close();
            }
            else if (String.IsNullOrEmpty(licensekey.Text))
            {
                MessageBox.Show("Please enter a valid license key.");
            }
            else if(license == null)
            {
                MessageBox.Show("Please enter a valid license key.");
            }
            else if(license.ExpirationDate < DateTime.Now)
            {
                MessageBox.Show("The license that youve entering is expired!");
            }
            else if(!String.IsNullOrEmpty(license.MachineName))
            {
                MessageBox.Show("The license have been already used");
            }
           
        }
    }
}
