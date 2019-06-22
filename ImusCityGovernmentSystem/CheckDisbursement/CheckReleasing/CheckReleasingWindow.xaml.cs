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
using System.Drawing.Imaging;

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
            LoadIdentificationCardType();
        }

        public void LoadIdentificationCardType()
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                idtypecb.ItemsSource = db.IdentificationCardTypes.Where(m => m.IsActive == true).OrderBy(m => m.CardType).ToList();
                idtypecb.DisplayMemberPath = "CardType";
                idtypecb.SelectedValuePath = "IdentificationCardTypeID";
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
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
                else if (digitalsignatureimg.Source == null)
                {
                    Mouse.OverrideCursor = null;
                    MessageBox.Show("Signature was not captured");
                }
                else if (idtypecb.SelectedValue == null || String.IsNullOrEmpty(idcardnumbertb.Text))
                {
                    MessageBox.Show("Please select identification card presented and id number");
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
                        if(db.Customers.Any(m => m.FirstName == firstnametb.Text && m.LastName == lastnametb.Text))
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
                if (idtypecb.SelectedValue == null || String.IsNullOrEmpty(idcardnumbertb.Text))
                {
                    MessageBox.Show("Please select identification card presented and id number");
                }
                else
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
                    customer.IsActive = true;
                    db.Customers.Add(customer);

                    CustomerIdentificationCard custCard = new CustomerIdentificationCard();
                    custCard.CustomerID = customer.CustomerID;
                    custCard.IdentificationCardTypeID = (int)idtypecb.SelectedValue;
                    custCard.IdentificationNumber = idcardnumbertb.Text;
                    db.CustomerIdentificationCards.Add(custCard);

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
                released.DigitalSignature = ConvertImageSourceToByte(digitalsignatureimg);
                released.ReleasedBy = App.EmployeeID;
                released.IdentificationCardNumber = idcardnumbertb.Text;
                released.IdentificationCardTypeID = (int)idtypecb.SelectedValue;
                db.CheckReleases.Add(released);

                ImusCityGovernmentSystem.Model.Check check = db.Checks.Find(checkId);
                check.Status = (int)CheckStatus.Released;

                int cardId = (int)idtypecb.SelectedValue;
                CustomerIdentificationCard customerCard = db.CustomerIdentificationCards.FirstOrDefault(m => m.IdentificationCardTypeID == cardId && m.CustomerID == Id && m.IdentificationCardType.IsActive == true);
                if (customerCard != null)
                {
                    customerCard.IdentificationNumber = idcardnumbertb.Text;
                    db.SaveChanges();
                }
                else
                {
                    CustomerIdentificationCard custCard = new CustomerIdentificationCard();
                    custCard.CustomerID = Id;
                    custCard.IdentificationCardTypeID = (int)idtypecb.SelectedValue;
                    custCard.IdentificationNumber = idcardnumbertb.Text;
                    db.CustomerIdentificationCards.Add(custCard);
                }

                db.SaveChanges();
                MessageBox.Show("Check has been successfully released");
                LoadIdentificationCardType();

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
            return db.Customers.Any(m => (m.FirstName == firstnametb.Text || m.LastName == lastnametb.Text) && m.IsActive == true);
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
                birthdatedp.SelectedDate = customer.Birthdate;
                completeaddresstb.Text = customer.CompleteAddress;

                if (db.CheckReleases.Any(m => m.CustomerID == customer.CustomerID))
                {
                    CheckRelease releasedCheck = db.CheckReleases.Where(m => m.CustomerID == customer.CustomerID).OrderByDescending(m => m.CheckReleaseID).FirstOrDefault();
                    CustomerIdentificationCard customerCard = db.CustomerIdentificationCards.FirstOrDefault(m => m.IdentificationCardTypeID == releasedCheck.IdentificationCardTypeID && m.CustomerID == customer.CustomerID && m.IdentificationCardType.IsActive == true);
                    if (customerCard != null)
                    {
                        idtypecb.SelectedValue = customerCard.IdentificationCardTypeID;
                        idcardnumbertb.Text = customerCard.IdentificationNumber;
                    }
                    else
                    {
                        idtypecb.SelectedValue = releasedCheck.IdentificationCardTypeID;
                        idcardnumbertb.Text = releasedCheck.IdentificationCardNumber;
                    }
                }
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
            digitalsignatureimg.Source = null;
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

        private void sigcapturebtn_Click(object sender, RoutedEventArgs e)
        {
            double width = digitalsig.ActualWidth;
            double height = digitalsig.ActualHeight;
            RenderTargetBitmap bmpCopied = new RenderTargetBitmap((int)Math.Round(width), (int)Math.Round(height), 96, 96, PixelFormats.Default);
            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(digitalsig);
                dc.DrawRectangle(vb, null, new Rect(new System.Windows.Point(), new System.Windows.Size(width, height)));
            }
            bmpCopied.Render(dv);
            System.Drawing.Bitmap bitmap;
            using (MemoryStream outStream = new MemoryStream())
            {
                // from System.Media.BitmapImage to System.Drawing.Bitmap 
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bmpCopied));
                enc.Save(outStream);
                bitmap = new System.Drawing.Bitmap(outStream);
                Stream StreamObj = new MemoryStream(outStream.ToArray());
                BitmapImage BitObj = new BitmapImage();
                BitObj.BeginInit();
                BitObj.StreamSource = StreamObj;
                BitObj.EndInit();
                digitalsignatureimg.Source = BitObj;
            }

        }
        private ImageCodecInfo getEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];
            return null;
        }

        private byte[] SignatureToBitmapBytes()
        {
            int margin = (int)this.digitalsig.Margin.Left;
            int width = (int)this.digitalsig.ActualWidth - margin;
            int height = (int)this.digitalsig.ActualHeight - margin;
            RenderTargetBitmap rtb =
            new RenderTargetBitmap(width, height, 96d, 96d, PixelFormats.Default);
            BmpBitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));
            byte[] bitmapBytes;
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                ms.Position = 0;
                bitmapBytes = ms.ToArray();
            }

            return bitmapBytes;
        }

        private void clearsigbtn_Click(object sender, RoutedEventArgs e)
        {
            digitalsig.Strokes.Clear();
        }

        private void clearsignbtn_Click(object sender, RoutedEventArgs e)
        {
            digitalsignatureimg.Source = null;
        }

        private void stopcamerabtn_Click(object sender, RoutedEventArgs e)
        {
            webcam.Stop();
        }

        private void idtypecb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                if(idtypecb.SelectedValue == null)
                {
                    return;
                }
                int cardId = (int)idtypecb.SelectedValue;
                CustomerIdentificationCard custCard = db.CustomerIdentificationCards.FirstOrDefault(m => m.CustomerID == customerId && m.IdentificationCardTypeID == cardId);
                if(custCard == null)
                {
                    idcardnumbertb.Text = string.Empty;
                }
                else
                {
                    idcardnumbertb.Text = custCard.IdentificationNumber;
                }

            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
           
        }
    }
}
