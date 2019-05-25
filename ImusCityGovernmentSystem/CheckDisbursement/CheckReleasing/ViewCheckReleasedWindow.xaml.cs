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
using System.IO;

namespace ImusCityGovernmentSystem.CheckDisbursement.CheckReleasing
{
    /// <summary>
    /// Interaction logic for ViewCheckReleasedWindow.xaml
    /// </summary>
    public partial class ViewCheckReleasedWindow : MetroWindow
    {
        public int id;
        public ViewCheckReleasedWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {

            Mouse.OverrideCursor = Cursors.Wait;
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                ImusCityGovernmentSystem.Model.CheckRelease released = db.CheckReleases.Find(id);
                firstnametb.Text = released.Customer.FirstName;
                middlenametb.Text = released.Customer.MiddleName;
                lastnametb.Text = released.Customer.LastName;
                completeaddresstb.Text = released.Customer.CompleteAddress;
                birthdatedp.Text = released.Customer.Birthdate.HasValue ? released.Customer.Birthdate.Value.ToShortDateString() : null;
                checknotb.Text = released.Check.CheckNo;
                vouchernumbertb.Text = released.Check.Disbursement.VoucherNo;
                if (released.Photo != null)
                {
                    Stream StreamObj = new MemoryStream(released.Photo);
                    BitmapImage BitObj = new BitmapImage();
                    BitObj.BeginInit();
                    BitObj.StreamSource = StreamObj;
                    BitObj.EndInit();
                    imagecapture.Source = BitObj;
                }
                if (released.DigitalSignature != null)
                {
                    Stream StreamObj = new MemoryStream(released.DigitalSignature);
                    BitmapImage BitObj = new BitmapImage();
                    BitObj.BeginInit();
                    BitObj.StreamSource = StreamObj;
                    BitObj.EndInit();
                    imagesignature.Source = BitObj;

                }
            }
            else
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
            Mouse.OverrideCursor = null;
        }
    }
}
