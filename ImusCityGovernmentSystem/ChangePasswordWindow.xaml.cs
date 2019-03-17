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
    /// Interaction logic for ChangePasswordWindow.xaml
    /// </summary>
    public partial class ChangePasswordWindow : MetroWindow
    {

        ImusCityHallEntities db = new ImusCityHallEntities();
        public ChangePasswordWindow()
        {
            InitializeComponent();
        }

        private void savebtn_Click(object sender, RoutedEventArgs e)
        {
            if(newpasswordpb.Password != confirmpasswordpb.Password)
            {
                MessageBox.Show("Password mismatch!");
            }
            else
            {
                Employee employee = db.Employees.Find(App.EmployeeID);
                AspNetUser asp = db.AspNetUsers.FirstOrDefault(m => m.UserName == employee.EmployeeNo);
                var passwordHasher = new Microsoft.AspNet.Identity.PasswordHasher();
                asp.PasswordHash = passwordHasher.HashPassword(confirmpasswordpb.Password);
                db.SaveChanges();
                MessageBox.Show("Password updated successfully!");
                this.Close();
            }
         
            
        }

        private void newpasswordpb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                savebtn_Click(sender, e);
            }
        }

        private void confirmpasswordpb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                savebtn_Click(sender, e);
            }
        }
    }
}
