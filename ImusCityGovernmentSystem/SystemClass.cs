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
        public static bool CheckConnection()
        {

            try
            {
                using (var db = new ImusCityHallEntities())
                {
                    db.Database.Connection.Open();
                    db.Database.Connection.Close();
                }
            }
            catch (SqlException)
            {
                return false;
            }
            return true;
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

        public  const string DBConnectionErrorMessage = "Please check database connection";

        public static string GetSignatory(int? id)
        {
            string result = "";
            ImusCityHallEntities db = new ImusCityHallEntities();
            ImusCityGovernmentSystem.Model.Employee employee = db.Employees.Find(id);
            result = employee.FirstName + " " + employee.MiddleName + " " + employee.LastName;
            return result;
        }

        public static string Employee(int id)
        {
            string result = "";
            ImusCityHallEntities db = new ImusCityHallEntities();
            ImusCityGovernmentSystem.Model.Employee employee = db.Employees.Find(id);
            result = employee.FirstName + " " + employee.LastName;
            return result;
        }
    }
}
