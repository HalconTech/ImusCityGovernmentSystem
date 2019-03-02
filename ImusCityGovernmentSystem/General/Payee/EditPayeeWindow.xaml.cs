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
    /// Interaction logic for EditPayeeWindow.xaml
    /// </summary>
    public partial class EditPayeeWindow : MetroWindow
    {
        public int PayeeID;
        ImusCityHallEntities db = new ImusCityHallEntities();
        public EditPayeeWindow()
        {
            InitializeComponent();
        }

        private void savebtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (gd.Where(m => m.IsSelected == true).Count() == 0)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show("Please select at least representative for the payee!");
            }
            else if (String.IsNullOrEmpty(payeenotb.Text) || String.IsNullOrEmpty(companynametb.Text) || String.IsNullOrEmpty(companyaddresstb.Text))
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show("Please input company name, company address and payee number");
            }
            else
            {
                ImusCityGovernmentSystem.Model.Payee payee = db.Payees.Find(PayeeID);
                payee.PayeeNo = payeenotb.Text;
                payee.CompanyName = companynametb.Text;
                payee.CompanyAddress = companyaddresstb.Text;
                payee.TIN = tinnotb.Text;
                payee.LandlineNo = contactnotb.Text;
                db.SaveChanges();

                foreach (var list in gd)
                {
                    if(list.IsSelected == true)
                    {
                        PayeeRepresentative pr = db.PayeeRepresentatives.Find(list.Id);
                        pr.PayeeID = payee.PayeeID;
                        db.SaveChanges();
                    }
                    else
                    {
                        PayeeRepresentative pr = db.PayeeRepresentatives.Find(list.Id);
                        pr.PayeeID = null;
                        db.SaveChanges();
                    }
                  
                }
                Mouse.OverrideCursor = null;

                var audit = new AuditTrailModel
                {
                    Activity = "Updated an item in payee list. PAYEE ID: " + PayeeID.ToString(),
                    ModuleName = this.GetType().Name,
                    EmployeeID = App.EmployeeID
                };

                SystemClass.InsertLog(audit);

                MessageBox.Show("Payee updated successfully!");
            }
            Mouse.OverrideCursor = null;
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ImusCityGovernmentSystem.Model.Payee payee = db.Payees.Find(PayeeID);
            payeenotb.Text = payee.PayeeNo;
            companynametb.Text = payee.CompanyName;
            companyaddresstb.Text = payee.CompanyAddress;
            tinnotb.Text = payee.TIN;
            contactnotb.Text = payee.LandlineNo;

            gd.Clear();
            db = new ImusCityHallEntities();
            if(db.PayeeRepresentatives.Where(m => m.PayeeID == PayeeID).Count() == 0)
            {
                var representative = db.PayeeRepresentativeViews.OrderBy(m => m.PayeeRepresentativeName).ToList();
                foreach (var dr in representative)
                {
                    PayeeRepresentativeClass i = new PayeeRepresentativeClass()
                    {
                        Name = dr.PayeeRepresentativeName.ToString(),
                        Id = dr.PayeeRepID
                    };
                    gd.Add(i);
                }
            }
            else
            {
                var representative = db.PayeeRepresentativeViews.OrderBy(m => m.PayeeRepresentativeName).ToList();
                foreach (var dr in representative)
                {
                    PayeeRepresentativeClass i = new PayeeRepresentativeClass()
                    {
                        Name = dr.PayeeRepresentativeName.ToString(),
                        Id = dr.PayeeRepID,
                        IsSelected = dr.PayeeID == null || dr.PayeeID != PayeeID ? false : true
                    };
                    gd.Add(i);
                }
            }
           

            represetativelb.ItemsSource = gd;
            represetativelb.Items.Refresh();
        }

        class PayeeRepresentativeClass
        {
            public string Name { get; set; }
            public int Id { get; set; }
            public bool IsSelected { get; set; }
        }
        List<PayeeRepresentativeClass> gd = new List<PayeeRepresentativeClass>();

        private void addrepbtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            AddNewPayeeRepresentativeWindow add = new AddNewPayeeRepresentativeWindow();
            Mouse.OverrideCursor = null;
            add.ShowDialog();
            MetroWindow_Loaded(sender, e);
        }
    }
}
