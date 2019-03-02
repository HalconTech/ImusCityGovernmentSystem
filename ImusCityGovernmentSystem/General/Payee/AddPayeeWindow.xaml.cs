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
using System.Text.RegularExpressions;
namespace ImusCityGovernmentSystem.General.Payee
{
    /// <summary>
    /// Interaction logic for AddPayeeWindow.xaml
    /// </summary>
    public partial class AddPayeeWindow : MetroWindow
    {
        ImusCityHallEntities db = new ImusCityHallEntities();
        public AddPayeeWindow()
        {
            InitializeComponent();
          
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            AddNewPayeeRepresentativeWindow add = new AddNewPayeeRepresentativeWindow();
            Mouse.OverrideCursor = null;
            add.ShowDialog();
            MetroWindow_Loaded(sender, e);

        }

        class PayeeRepresentativeClass
        {
            public string Name { get; set; }
            public int Id { get; set; }
            public bool IsSelected { get; set; }
        }
        List<PayeeRepresentativeClass> gd = new List<PayeeRepresentativeClass>();
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPayeeRepresentative();         
        }

        public void LoadPayeeRepresentative()
        {
            gd.Clear();
            db = new ImusCityHallEntities();
            var representative = db.PayeeRepresentativeViews.Where(m => m.PayeeID == null).OrderBy(m => m.PayeeRepresentativeName).ToList();
            foreach (var dr in representative)
            {
                PayeeRepresentativeClass i = new PayeeRepresentativeClass()
                {
                    Name = dr.PayeeRepresentativeName.ToString(),
                    Id = dr.PayeeRepID
                };
                gd.Add(i);
            }

            represetativelb.ItemsSource = gd;
            represetativelb.Items.Refresh();
        }
        private void savebtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (String.IsNullOrEmpty(payeenotb.Text) || String.IsNullOrEmpty(companynametb.Text) || String.IsNullOrEmpty(companyaddresstb.Text))
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show("Please input company name, company address and payee number");
            }
            else if(gd.Where(m => m.IsSelected == true).Count() == 0)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show("Please select at least one representative for the payee!");
            }
            else
            {
                if(db.Payees.FirstOrDefault(m => m.PayeeNo == payeenotb.Text) != null)
                {
                    Mouse.OverrideCursor = null;
                    MessageBox.Show("Payee number already exist!");
                }
                else
                {
                    ImusCityGovernmentSystem.Model.Payee payee = new Model.Payee();
                    payee.PayeeNo = payeenotb.Text;
                    payee.CompanyName = companynametb.Text;
                    payee.CompanyAddress = companyaddresstb.Text;
                    payee.TIN = tinnotb.Text;
                    payee.LandlineNo = contactnotb.Text;
                    db.Payees.Add(payee);
                    db.SaveChanges();


                    foreach (var list in gd.Where(m => m.IsSelected == true))
                    {
                        PayeeRepresentative pr = db.PayeeRepresentatives.Find(list.Id);
                        pr.PayeeID = payee.PayeeID;
                        db.SaveChanges();
                    }
                    Mouse.OverrideCursor = null;

                    var audit = new AuditTrailModel
                    {
                        Activity = "Added new payee in the database. PAYEE NO." + payeenotb.Text,
                        ModuleName = this.GetType().Name,
                        EmployeeID = App.EmployeeID
                    };

                    SystemClass.InsertLog(audit);
                    MessageBox.Show("Payee added succefully!");
                    SystemClass.ClearTextBoxes(this);
                    LoadPayeeRepresentative();

                }
           
            }
            Mouse.OverrideCursor = null;
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
