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
using System.Data.SqlClient;
using System.IO.IsolatedStorage;
using System.IO;
using System.Data.Common;

namespace ImusCityGovernmentSystem
{
    /// <summary>
    /// Interaction logic for LogInWindow.xaml
    /// </summary>
    public partial class LogInWindow : MetroWindow
    {
        public LogInWindow()
        {
            InitializeComponent();
            usernametb.Focus();

        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (Rememberme.IsChecked == true)
            {
                IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetUserStoreForAssembly();
                using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream("login", FileMode.Create, isolatedStorage))
                {
                    using (StreamWriter sw = new StreamWriter(stream))
                    {
                        sw.WriteLine(usernametb.Text);
                        sw.WriteLine(passwordpb.Password);
                    }
                }
            }
        }
        private void loginbtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (String.IsNullOrEmpty(usernametb.Text) || String.IsNullOrEmpty(passwordpb.Password))
                {
                    MessageBox.Show("Please input your sign-in credentials");
                }
                else
                {
                    if (SystemClass.CheckConnection())
                    {
                        using (var db = new ImusCityHallEntities())
                        {
                            var passwordHasher = new Microsoft.AspNet.Identity.PasswordHasher();
                            string pass = "";
                            var asp = db.AspNetUsers.Where(m => m.UserName == usernametb.Text).FirstOrDefault();

                            if (asp != null)
                            {
                                pass = passwordHasher.VerifyHashedPassword(asp.PasswordHash, passwordpb.Password).ToString();
                            }
                            else
                            {
                                MessageBox.Show("Log-in failed!");
                                Mouse.OverrideCursor = null;
                                return;
                            }

                            if (pass == "Success")
                            {
                                Mouse.OverrideCursor = Cursors.Wait;
                                var emp = db.Employees.FirstOrDefault(m => m.EmployeeNo == usernametb.Text);
                                App.EmployeeID = emp.EmployeeID;

                                if (passwordpb.Password == "imuscitygov")
                                {
                                    Mouse.OverrideCursor = null;
                                    MessageBox.Show("Please change your default password.");
                                    ChangePasswordWindow password = new ChangePasswordWindow();
                                    password.Show();
                                }
                                else if (emp.SecurityQuestionUsers.Count < 3)
                                {
                                    Mouse.OverrideCursor = null;
                                    MessageBox.Show("Please set-up your security questions.");
                                    SecurityQuestion secquestion = new SecurityQuestion();
                                    secquestion.Show();
                                }
                                else
                                {
                                    var audit = new AuditTrailModel
                                    {
                                        Activity = "Log-in to the system",
                                        ModuleName = this.GetType().Name,
                                        EmployeeID = App.EmployeeID
                                    };

                                    SystemClass.InsertLog(audit);
                                    MainWindow mw = new MainWindow();
                                    mw.Password = passwordpb.Password;
                                    mw.Show();
                                    this.Close();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Log-in failed!");
                                Mouse.OverrideCursor = null;
                                return;
                            }


                        }
                    }
                    else
                    {
                        MessageBox.Show(SystemClass.DBConnectionErrorMessage);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            Mouse.OverrideCursor = null;
        }

        private void usernametb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                loginbtn_Click(sender, e);
            }
        }
        private void passwordpb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                loginbtn_Click(sender, e);
            }
        }
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                LicensingCode license = db.LicensingCodes.FirstOrDefault(m => m.MachineName == Environment.MachineName);

                if (license == null)
                {
                    MessageBox.Show("This software have not been licensed to this machine. Please input valid license key!");
                    LicenseCodeWindow lc = new LicenseCodeWindow();
                    lc.Show();
                    this.Close();
                }
                else if (license.ExpirationDate < DateTime.Now)
                {
                    MessageBox.Show("This license that have been issued to this machine is expired! Please input new license");
                    LicenseCodeWindow lc = new LicenseCodeWindow();
                    lc.Show();
                    this.Close();
                }
                else
                {
                    if (license.IsDemo == true)
                    {
                        App.LicenseKey = license.LicenseKey;
                        this.Title = "GISI (DEMO MODE)";
                    }
                    else
                    {
                        App.LicenseKey = license.LicenseKey;
                        this.Title = "GISI";
                    }

                }
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetUserStoreForAssembly();
            if (isolatedStorage.FileExists("login"))
            {
                using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream("login", FileMode.OpenOrCreate, isolatedStorage))
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        string uname = sr.ReadLine();
                        string pass = sr.ReadLine();

                        usernametb.Text = uname;
                        passwordpb.Password = pass;

                    }
                    Rememberme.IsChecked = true;
                }

            }
        }
        private void Rememberme_Unchecked_1(object sender, RoutedEventArgs e)
        {
            if (Rememberme.IsChecked == false)
            {
                IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetUserStoreForAssembly();
                isolatedStorage.DeleteFile("login");
            }
        }
        private void Label_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (String.IsNullOrEmpty(usernametb.Text))
                {
                    MessageBox.Show("Please input your username.");
                }
                else
                {

                    using (var db = new ImusCityHallEntities())
                    {

                        //var passwordHasher = new Microsoft.AspNet.Identity.PasswordHasher();
                        //string pass = "";
                        var asp = db.AspNetUsers.Where(m => m.UserName == usernametb.Text).FirstOrDefault();

                        if (asp != null)
                        {
                            var emp = db.Employees.FirstOrDefault(m => m.EmployeeNo == asp.UserName);
                            App.EmployeeID = emp.EmployeeID;

                            if (emp.SecurityQuestionUsers.Count < 3)
                            {
                                Mouse.OverrideCursor = null;
                                MessageBox.Show("Please set-up your security questions.");
                                SecurityQuestion secquestion = new SecurityQuestion();
                                secquestion.Show();
                            }
                            else
                            {
                                Mouse.OverrideCursor = null;
                                MessageBox.Show("Please answer one (1) security question.");
                                ForgotPassword fp = new ForgotPassword();
                                fp.Show();

                            }
                            //pass = passwordHasher.VerifyHashedPassword(asp.PasswordHash, passwordpb.Password).ToString();
                        }
                        else
                        {
                            MessageBox.Show("Incorrect username!");
                            Mouse.OverrideCursor = null;
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            Mouse.OverrideCursor = null;
        }


    }
}
