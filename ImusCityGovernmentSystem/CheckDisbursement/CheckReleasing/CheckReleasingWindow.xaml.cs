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
using ImusCityGovernmentSystem.CheckDisbursement.CheckReleasing;
using ImusCityGovernmentSystem.Model;
using System.IO;
using ImusCityGovernmentSystem.General.Customer;
using System.Runtime.InteropServices;

namespace ImusCityGovernmentSystem.CheckDisbursement.CheckReleasing
{
    /// <summary>
    /// Interaction logic for CheckReleasingWindow.xaml
    /// </summary>
    public partial class CheckReleasingWindow : MetroWindow
    {

        WebCam webcam;
        public CheckReleasingWindow()
        {
            InitializeComponent();
            digitalsig.UseCustomCursor = true;
            digitalsig.Cursor = Cursors.Pen;
            digitalsig.MoveEnabled = false;
     
        }
     

        private void startcapturingbtn_Click(object sender, RoutedEventArgs e)
        {
            webcam.Start();

        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            webcam = new WebCam();
            webcam.InitializeWebCam(ref currentimage);
            LoadCheckList();
            Point targetLoc = this.PointToScreen(new Point(0, 0));
            System.Drawing.Rectangle r = new System.Drawing.Rectangle((int)targetLoc.X, (int)targetLoc.Y, (int)(targetLoc.X + this.Width), (int)(targetLoc.Y + this.Height));
            ClipCursor(ref r);
        }

        public void LoadCheckList()
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                checklistcb.ItemsSource = db.Checks.OrderByDescending(m => m.CheckID).Where(m => m.Status == 0).ToList();
                checklistcb.DisplayMemberPath = "CheckNo";
                checklistcb.SelectedValuePath = "CheckID";
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }
        private void capturebtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Image was captured. Would you like to retake a picture", "Warning", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
            {
                imagecapture.Source = currentimage.Source;
                webcam.Stop();
            }
            else
            {
                webcam.Start();
            }
        }

        public int customerId = 0;
        private void savebtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                if (checklistcb.SelectedValue == null)
                {
                    Mouse.OverrideCursor = null;
                    MessageBox.Show("Please select check");
                }
                else if (imagecapture.Source == null)
                {
                    Mouse.OverrideCursor = null;
                    MessageBox.Show("Image was not captured");
                }
                else if (CustomerExist() && customerId == 0)
                {
                    Mouse.OverrideCursor = null;
                    MessageBoxResult result = MessageBox.Show(CustomerMatched().ToString() + " records matched. Would you like to retrieve his/her information or by clicking 'No' the system will automatically create a new record for this person", "Check Releasing Warning", MessageBoxButton.YesNo);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            int? Id;
                            CustomerMatchedWindow matched = new CustomerMatchedWindow();
                            matched.fname = firstnametb.Text;
                            matched.mname = middlenametb.Text;
                            matched.lname = lastnametb.Text;
                            if (matched.ShowDialog(out Id) == true)
                            {
                                if (Id != null)
                                {
                                    customerId = Id.Value;
                                    LoadCustomer(Id.Value);
                                }
                            }
                            MessageBox.Show("Please click the save button to continue the transaction");
                            break;
                        case MessageBoxResult.No:
                            CreateCustomer();
                            break;
                    }
                }
                else
                {
                    int checkId = (int)checklistcb.SelectedValue;
                    if (db.Customers.Any())
                    {
                        SaveCheckReleased(customerId, checkId);
                        customerId = 0;
                    }
                    else
                    {
                        CreateCustomer();
                        SaveCheckReleased(customerId, checkId);
                        customerId = 0;
                    }

                }

            }
            else
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
            Mouse.OverrideCursor = null;
        }

        public void CreateCustomer()
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                ImusCityGovernmentSystem.Model.Customer customer = new Customer();
                customer.FirstName = firstnametb.Text;
                customer.MiddleName = middlenametb.Text;
                customer.LastName = lastnametb.Text;
                customer.DateAdded = DateTime.Now;
                customer.AddedBy = App.EmployeeID;
                customer.Birthdate = birthdatedp.SelectedDate;
                customer.CompleteAddress = completeaddresstb.Text;
                db.Customers.Add(customer);
                db.SaveChanges();
                MessageBox.Show("Data is saved. Please click the save button to continue the transaction");
                customerId = customer.CustomerID;

                var audit = new AuditTrailModel
                {
                    Activity = "Adde new customer in the database CUSTOMER NAME: " + string.Join(" ", customer.FirstName, customer.MiddleName, customer.LastName),
                    ModuleName = this.GetType().Name,
                    EmployeeID = App.EmployeeID
                };
                SystemClass.InsertLog(audit);
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }
        public void SaveCheckReleased(int Id, int checkId)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                ImusCityGovernmentSystem.Model.CheckRelease released = new CheckRelease();
                released.CustomerID = Id;
                released.ReleasedDate = DateTime.Now;
                released.CheckID = checkId;
                released.Photo = ConvertImageSourceToByte(imagecapture);
                released.ReleasedBy = App.EmployeeID;
                db.CheckReleases.Add(released);

                ImusCityGovernmentSystem.Model.Check check = db.Checks.Find(checkId);
                check.Status = (int)CheckStatus.Released;

                db.SaveChanges();
                MessageBox.Show("Transaction completed");

                var audit = new AuditTrailModel
                {
                    Activity = "Released check to CUSTOMER ID: " + released.CustomerID.ToString() + " with CHECK NUMBER: " + released.Check.CheckNo,
                    ModuleName = this.GetType().Name,
                    EmployeeID = App.EmployeeID
                };
                SystemClass.InsertLog(audit);

                ResetFields();
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }
        public bool CustomerExist()
        {
            ImusCityHallEntities db = new ImusCityHallEntities();
            return db.Customers.Any(m => (m.FirstName == firstnametb.Text || m.MiddleName == middlenametb.Text || m.LastName == lastnametb.Text) && m.IsActive == true);
        }

        public int CustomerMatched()
        {
            ImusCityHallEntities db = new ImusCityHallEntities();
            return db.Customers.Count(m => (m.FirstName == firstnametb.Text || m.MiddleName == middlenametb.Text || m.LastName == lastnametb.Text) && m.IsActive == true);
        }

        private void checklistcb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                if (checklistcb.SelectedValue == null)
                {
                    return;
                }
                int id = (int)checklistcb.SelectedValue;
                ImusCityGovernmentSystem.Model.Check check = db.Checks.Find(id);
                voucherNumbertb.Text = check.Disbursement.VoucherNo;
                checknumbertb.Text = check.CheckNo;
                checkdatetb.Text = check.DateCreated.Value.ToShortDateString();
                companynametb.Text = check.Disbursement.Payee.CompanyName;
                checkdescriptiontb.Text = check.CheckDescription;
                banktb.Text = string.Join("/", check.Disbursement.FundBank.Fund.FundName, check.Disbursement.FundBank.Bank.BankName);
                amountb.Text = string.Format("{0:n}", check.Amount);
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }
        private byte[] ConvertImageSourceToByte(Image value)
        {
            BitmapSource bmpSource = value.Source as BitmapSource;
            MemoryStream ms = new MemoryStream();
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmpSource));
            encoder.Save(ms);
            ms.Seek(0, SeekOrigin.Begin);
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(ms);
            System.Drawing.ImageConverter converter = new System.Drawing.ImageConverter();
            return (byte[])converter.ConvertTo(bitmap, typeof(byte[]));
        }

        private void searchbtn_Click(object sender, RoutedEventArgs e)
        {
            int? Id;

            SearchCustomerWindow customerFinder = new SearchCustomerWindow();

            if (customerFinder.ShowDialog(out Id) == true)
            {
                if (Id != null)
                {
                    customerId = Id.Value;
                    LoadCustomer(Id.Value);
                }
            }
        }

        public void LoadCustomer(int id)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                ImusCityGovernmentSystem.Model.Customer customer = db.Customers.Find(id);
                firstnametb.Text = customer.FirstName;
                middlenametb.Text = customer.MiddleName;
                lastnametb.Text = customer.LastName;
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }

        public void ResetFields()
        {
            SystemClass.ClearTextBoxes(this);
            currentimage.Source = null;
            imagecapture.Source = null;
            voucherNumbertb.Text = "";
            checknumbertb.Text = "";
            checkdatetb.Text = null;
            companynametb.Text = "";
            checkdescriptiontb.Text = "";
            banktb.Text = "";
            amountb.Text = "";
            LoadCheckList();
        }

        private void viewrelchecksbtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            ImusCityGovernmentSystem.CheckDisbursement.CheckReleasing.CheckReleasedListWindow releasing = new CheckReleasedListWindow();
            releasing.Show();
            Mouse.OverrideCursor = null;
        }

        private void clearcapturedimgbtn_Click(object sender, RoutedEventArgs e)
        {
            imagecapture.Source = null;
        }

        private void MetroWindow_Closed(object sender, EventArgs e)
        {
            webcam.Stop();
        }

        [DllImport("user32.dll")]
        static extern void ClipCursor(ref System.Drawing.Rectangle rect);

        [DllImport("user32.dll")]
        static extern void ClipCursor(IntPtr rect);

    }
}
