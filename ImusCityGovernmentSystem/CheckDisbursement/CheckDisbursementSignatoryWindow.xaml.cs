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
namespace ImusCityGovernmentSystem.CheckDisbursement
{
    /// <summary>
    /// Interaction logic for CheckDisbursementSignatoryWindow.xaml
    /// </summary>
    public partial class CheckDisbursementSignatoryWindow : MetroWindow
    {
        public CheckDisbursementSignatoryWindow()
        {
            InitializeComponent();
        }

        private void savebtn_Click(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                if (db.CDSSignatories.Any())
                {
                    ImusCityGovernmentSystem.Model.CDSSignatory signatory = db.CDSSignatories.FirstOrDefault();
                    signatory.CityMayor = (int)mayorcb.SelectedValue;
                    signatory.CityTreasurer = (int)treasurercb.SelectedValue;
                    signatory.CItyAccountant = (int)accountantcb.SelectedValue;
                    signatory.AccountantRepresentative = (int)accountantrepcb.SelectedValue;
                    signatory.CityAdministrator = (int)administratorcb.SelectedValue;
                    db.SaveChanges();
                    MessageBox.Show("Signatories for CDS was updated");
                }
                else
                {
                    ImusCityGovernmentSystem.Model.CDSSignatory signatory = new CDSSignatory();
                    signatory.CityMayor = (int)mayorcb.SelectedValue;
                    signatory.CityTreasurer = (int)treasurercb.SelectedValue;
                    signatory.CItyAccountant = (int)accountantcb.SelectedValue;
                    signatory.AccountantRepresentative = (int)accountantrepcb.SelectedValue;
                    signatory.CityAdministrator = (int)administratorcb.SelectedValue;
                    db.CDSSignatories.Add(signatory);
                    db.SaveChanges();
                    MessageBox.Show("Signatories for CDS was updated");
                }

            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                var result = from p in db.Employees
                             select new
                             {
                                 ID = p.EmployeeID,
                                 Name = p.FirstName + " " + p.LastName
                             };

                mayorcb.ItemsSource = result.OrderBy(m => m.Name).ToList();
                mayorcb.DisplayMemberPath = "Name";
                mayorcb.SelectedValuePath = "ID";

                treasurercb.ItemsSource = result.OrderBy(m => m.Name).ToList();
                treasurercb.DisplayMemberPath = "Name";
                treasurercb.SelectedValuePath = "ID";

                accountantcb.ItemsSource = result.OrderBy(m => m.Name).ToList();
                accountantcb.DisplayMemberPath = "Name";
                accountantcb.SelectedValuePath = "ID";

                accountantrepcb.ItemsSource = result.OrderBy(m => m.Name).ToList();
                accountantrepcb.DisplayMemberPath = "Name";
                accountantrepcb.SelectedValuePath = "ID";

                administratorcb.ItemsSource = result.OrderBy(m => m.Name).ToList();
                administratorcb.DisplayMemberPath = "Name";
                administratorcb.SelectedValuePath = "ID";

                if(db.CDSSignatories.Any())
                {
                    ImusCityGovernmentSystem.Model.CDSSignatory signatory = db.CDSSignatories.FirstOrDefault();
                    mayorcb.SelectedValue = signatory.CityMayor;
                    treasurercb.SelectedValue = signatory.CityTreasurer;
                    accountantcb.SelectedValue = signatory.CItyAccountant;
                    accountantrepcb.SelectedValue = signatory.AccountantRepresentative;
                    administratorcb.SelectedValue = signatory.CityAdministrator;
                }
                else
                {
                   
                }
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }
    }
}
