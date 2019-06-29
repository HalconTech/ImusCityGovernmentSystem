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
    /// Interaction logic for SystemSettings.xaml
    /// </summary>
    public partial class SystemSettings : MetroWindow
    {
        public SystemSettings()
        {
            InitializeComponent();
        }

        private void acceptbtn_Click(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                if (db.SystemSettings.Count() >= 1)
                {
                    int numberUsers = Convert.ToInt32(userstb.Text);
                    ImusCityGovernmentSystem.Model.SystemSetting systemSetting = db.SystemSettings.FirstOrDefault();
                    systemSetting.NumberOfUser = numberUsers;
                    db.SaveChanges();
                    MessageBox.Show("System setting updated successfully!");
                }
                else
                {
                    if (String.IsNullOrEmpty(userstb.Text))
                    {
                        MessageBox.Show("Please input number of users field");
                    }
                    else
                    {
                        int numberUsers = Convert.ToInt32(userstb.Text);
                        ImusCityGovernmentSystem.Model.SystemSetting systemSetting = new SystemSetting
                        {
                            NumberOfUser = numberUsers
                        };
                        db.SystemSettings.Add(systemSetting);
                        db.SaveChanges();
                        MessageBox.Show("System setting updated successfully!");
                    }


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
                if (db.SystemSettings.Count() >= 1)
                {               
                    ImusCityGovernmentSystem.Model.SystemSetting systemSetting = db.SystemSettings.FirstOrDefault();
                    userstb.Text = systemSetting.NumberOfUser.ToString();
                }
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }
    }
}
