using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ImusCityGovernmentSystem.Model;
using System.Data.SqlClient;
using System.Net;
using System.Net.Sockets;
namespace ImusCityGovernmentSystem
{
    class SystemClass
    {
        public static void ClearTextBoxes(DependencyObject obj)
        {

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {

                if (obj is TextBox)

                    ((TextBox)obj).Text = null;

                ClearTextBoxes(VisualTreeHelper.GetChild(obj, i));

            }

        }
        public static bool TestConnectionEF()
        {
            using (var db = new ImusCityHallEntities())
            {
                try
                {
                    db.Database.Connection.Open();
                    if (db.Database.Connection.State == System.Data.ConnectionState.Open)
                    {
                        MessageBox.Show(@"INFO: ConnectionString: " + db.Database.Connection.ConnectionString
                            + "\n DataBase: " + db.Database.Connection.Database
                            + "\n DataSource: " + db.Database.Connection.DataSource
                            + "\n ServerVersion: " + db.Database.Connection.ServerVersion
                            + "\n TimeOut: " + db.Database.Connection.ConnectionTimeout);
                        db.Database.Connection.Close();
                        return true;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public static void CheckConnection()
        {
            ImusCityHallEntities db = new ImusCityHallEntities();
            try
            {
                db.Database.Connection.Open();
                db.Database.Connection.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("System is not connected to database. Please contact the administrator. Thank you!");
                return;
            }
            return;
        }

        public static void InsertLog(AuditTrailModel model)
        {
            using (var db = new ImusCityHallEntities())
            {
                Employee employee = db.Employees.Find(model.EmployeeID);
                var audit = new AuditTrail()
                {
                    LogDate = DateTime.Now,
                    IPAddress = GetLocalIPAddress(),
                    ComputerName = Environment.MachineName,
                    ModuleName = model.ModuleName,
                    Activity = model.Activity,
                    UserID = model.EmployeeID,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName
                };

                db.AuditTrails.Add(audit);
                db.SaveChanges();
            }
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        
    }
}
