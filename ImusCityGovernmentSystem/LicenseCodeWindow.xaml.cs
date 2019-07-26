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
        public string subModuleCode;
        public LicenseCodeWindow()
        {
            InitializeComponent();
        }

        private void acceptbtn_Click(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                var licenseList = from p in db.LicensingCodes
                                  select new
                                  {
                                      p.LicenseId,
                                      p.ExpirationDate,
                                      p.SubModule,
                                      LicenseKey = p.LicenseKey.Replace("\r\n", string.Empty)
                                  };

              
                if (String.IsNullOrEmpty(licensekey.Text))
                {
                    MessageBox.Show("The license key field is empty");
                    return;
                }
                var licenseCode = licenseList.FirstOrDefault(m => m.LicenseKey == licensekey.Text);
                LicensingCode license = new LicensingCode()
                {
                    LicenseId = licenseCode.LicenseId,
                    LicenseKey = licenseCode.LicenseKey,
                    ExpirationDate = licenseCode.ExpirationDate,
                    SubModule = licenseCode.SubModule
                };


                if (license != null && license.ExpirationDate > DateTime.Now.Date && license.SubModule.Acronym == subModuleCode && license.LicenseKey == App.LicenseKey)
                {
                    LicensingCode dbLicense = db.LicensingCodes.Find(license.LicenseId);
                    //license.MachineName = Environment.MachineName;
                    //license.UserID = App.EmployeeID;
                    dbLicense.ActivatedDate = DateTime.Now;
                    db.SaveChanges();
                    MessageBox.Show("License key applied");
                    this.Close();
                }
                else if (String.IsNullOrEmpty(licensekey.Text))
                {
                    MessageBox.Show("The license key field is empty");
                }
                else if (license == null)
                {
                    MessageBox.Show("Please enter a valid license key.");
                }
                else if (license.ExpirationDate < DateTime.Now)
                {
                    MessageBox.Show("The license that youve entering is expired!");
                }
                else if (licensekey.Text != App.LicenseKey)
                {
                    MessageBox.Show("Please enter a valid license key.");
                }
                else if (license.SubModule.Acronym != subModuleCode && license != null)
                {
                    MessageBox.Show("The license that you are applying is not meant for the specified module");
                }
                //else if (license.LicenseKey == licensekey.Text && license.UserID == App.EmployeeID)
                //{
                //    MessageBox.Show("This license is associated with another user");
                //}
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }

        }
    }
}
