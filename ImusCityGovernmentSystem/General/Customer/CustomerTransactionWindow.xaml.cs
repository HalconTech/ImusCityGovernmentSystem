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
using ImusCityGovernmentSystem.General.Customer.Model;
using MahApps.Metro.Controls;

namespace ImusCityGovernmentSystem.General.Customer
{
    /// <summary>
    /// Interaction logic for CustomerTransactionWindow.xaml
    /// </summary>
    public partial class CustomerTransactionWindow : MetroWindow
    {
        public int id;
        public CustomerTransactionWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                ImusCityGovernmentSystem.Model.Customer customer = db.Customers.Find(id);
                this.Title = string.Join(" ", customer.FirstName, customer.LastName, "Transactions");
                List<CustomerTransactionsEntity> transactionlist = new List<CustomerTransactionsEntity>();
                var result = db.GetCustomerTransactions(id);
                foreach (var item in result)
                {
                    var transaction = new CustomerTransactionsEntity
                    {
                        Activity = item.Activity,
                        TransactionDate = item.TransactionDate.Value.ToShortDateString()
                    };

                    transactionlist.Add(transaction);
                }

                customerTransactiondg.ItemsSource = transactionlist;
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }
    }
}
