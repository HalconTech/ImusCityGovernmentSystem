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
using Microsoft.Win32;
using System.IO;
namespace ImusCityGovernmentSystem.General.EmployeeModule
{
    /// <summary>
    /// Interaction logic for EditEmployeeWindow.xaml
    /// </summary>
    public partial class EditEmployeeWindow : MetroWindow
    {
     
        public int EmployeeID;
        public string newimage;
        public EditEmployeeWindow()
        {
            InitializeComponent();

            if(SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();
                divisioncb.ItemsSource = db.Divisions.Where(m => m.IsActive == true).OrderBy(m => m.DivisionCode).ToList();
                divisioncb.DisplayMemberPath = "DivisionCode";
                divisioncb.SelectedValuePath = "DivisionID";

                positioncb.ItemsSource = db.EmployeePositions.Where(m => m.Active == true).OrderBy(m => m.EmployeePositionName).ToList();
                positioncb.DisplayMemberPath = "EmployeePositionName";
                positioncb.SelectedValuePath = "EmployeePositionID";

                statuscb.ItemsSource = db.EmployeeStatus.OrderBy(m => m.EmployeeStatusName).ToList();
                statuscb.DisplayMemberPath = "EmployeeStatusName";
                statuscb.SelectedValuePath = "EmployeeStatusID";
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
          
        }

        private void savebtn_Click(object sender, RoutedEventArgs e)
        {
            if(SystemClass.CheckConnection())
            {
                Mouse.OverrideCursor = Cursors.Wait;
                try
                {
                    ImusCityHallEntities db = new ImusCityHallEntities();
                    Employee employee = db.Employees.Find(EmployeeID);

                    if (String.IsNullOrEmpty(fnametb.Text))
                    {
                        MessageBox.Show("Please enter first name");
                        Mouse.OverrideCursor = null;
                    }
                    else if (String.IsNullOrEmpty(lnametb.Text))
                    {
                        MessageBox.Show("Please enter last name");
                        Mouse.OverrideCursor = null;
                    }
                    else if (String.IsNullOrEmpty(permaddtb.Text))
                    {
                        MessageBox.Show("Please enter permanent address");
                        Mouse.OverrideCursor = null;
                    }
                    else if (String.IsNullOrEmpty(primaryemailtb.Text))
                    {
                        MessageBox.Show("Please enter primary email");
                        Mouse.OverrideCursor = null;
                    }
                    else if (String.IsNullOrEmpty(employeenotb.Text))
                    {
                        MessageBox.Show("Please enter employee number");
                        Mouse.OverrideCursor = null;
                    }
                    else if (String.IsNullOrEmpty(datehiredp.Text))
                    {
                        MessageBox.Show("Please enter date hired");
                        Mouse.OverrideCursor = null;
                    }
                    else if (divisioncb.SelectedValue == null)
                    {
                        MessageBox.Show("Please select employee division");
                        Mouse.OverrideCursor = null;
                    }
                    else if (positioncb.SelectedValue == null)
                    {
                        MessageBox.Show("Please select employee position");
                        Mouse.OverrideCursor = null;
                    }
                    else if (statuscb.SelectedValue == null)
                    {
                        MessageBox.Show("Please select employee status");
                        Mouse.OverrideCursor = null;
                    }
                    else if (String.IsNullOrEmpty(birthdatedp.Text))
                    {
                        MessageBox.Show("Please enter birthday");
                        Mouse.OverrideCursor = null;
                    }
                    else if (String.IsNullOrEmpty(birthplacetb.Text))
                    {
                        MessageBox.Show("Please enter birthplace");
                        Mouse.OverrideCursor = null;
                    }
                    else if (genderdp.SelectedValue == null)
                    {
                        MessageBox.Show("Please select gender");
                        Mouse.OverrideCursor = null;
                    }
                    else if (civilstatuscb.SelectedValue == null)
                    {
                        MessageBox.Show("Please select civil status");
                        Mouse.OverrideCursor = null;
                    }
                    else if (String.IsNullOrEmpty(mobilenotb.Text) || String.IsNullOrEmpty(landlinenotb.Text))
                    {
                        MessageBox.Show("Please enter mobile number or landline number");
                        Mouse.OverrideCursor = null;
                    }
                    else
                    {
                        employee.EmployeeNo = employeenotb.Text;

                        int DeptID, PosID, StatID;

                        //Employee Information
                        employee.EmployeeDivisionID = Int32.TryParse(divisioncb.SelectedValue.ToString(), out DeptID) ? DeptID : (int?)null;
                        employee.EmployeePositionID = Int32.TryParse(positioncb.SelectedValue.ToString(), out PosID) ? PosID : (int?)null;
                        employee.EmployeeStatusID = Int32.TryParse(positioncb.SelectedValue.ToString(), out StatID) ? StatID : (int?)null;
                        employee.DateHired = String.IsNullOrEmpty(datehiredp.Text) ? null : datehiredp.SelectedDate;
                        employee.DatePermanency = String.IsNullOrEmpty(datepermanetdp.Text) ? null : datepermanetdp.SelectedDate;
                        employee.DateEndContract = String.IsNullOrEmpty(dateendodp.Text) ? null : dateendodp.SelectedDate;
                        employee.DateResigned = String.IsNullOrEmpty(dateresignationdp.Text) ? null : dateresignationdp.SelectedDate;

                        //Personal Information
                        employee.FirstName = fnametb.Text;
                        employee.MiddleName = mnametb.Text;
                        employee.LastName = lnametb.Text;
                        employee.NameSuffix = namesuffixtb.Text;
                        employee.Birthday = birthdatedp.SelectedDate;
                        employee.Birthplace = birthplacetb.Text;
                        employee.Sex = genderdp.SelectionBoxItem.ToString().Substring(0, 1);
                        employee.Nationality = citizenshiptb.Text;
                        employee.CivilStatus = civilstatuscb.SelectionBoxItem.ToString();
                        employee.Religion = religiontb.Text;
                        employee.TIN = tinnotb.Text;
                        employee.SSS = sssnotb.Text;
                        employee.PhilHealth = philhealthnotb.Text;
                        employee.PAG_IBIG = pagibignotb.Text;

                        //Contact Information
                        employee.PermanentAddress = permaddtb.Text;
                        employee.CurrentAddress = currentadd.Text;
                        employee.TelephoneNo = landlinenotb.Text;
                        employee.MobileNo = mobilenotb.Text;
                        employee.PrimaryEmail = primaryemailtb.Text;
                        employee.SecondaryEmail = secondaryemailtb.Text;

                        employee.Archive = false;
                        if (!String.IsNullOrEmpty(newimage))
                        {
                            FileStream fStream = File.OpenRead(newimage);
                            byte[] contents = new byte[fStream.Length];
                            fStream.Read(contents, 0, (int)fStream.Length);
                            fStream.Close();
                            employee.Photo = contents;
                        }
                        else
                        {

                        }
                        employee.DateEncoded = DateTime.Now;
                        db.SaveChanges();
                        Mouse.OverrideCursor = null;

                        var audit = new AuditTrailModel
                        {
                            Activity = "Updated an employee EMP ID: " + EmployeeID.ToString(),
                            ModuleName = this.GetType().Name,
                            EmployeeID = App.EmployeeID
                        };

                        SystemClass.InsertLog(audit);

                        MessageBox.Show("Employee was updated to the database succesfully!");
                    }



                }
                catch (Exception ex)
                {
                    Mouse.OverrideCursor = null;
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
         
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if(SystemClass.CheckConnection())
            {
                try
                {
                    ImusCityHallEntities db = new ImusCityHallEntities();
                    Employee employee = db.Employees.Find(EmployeeID);

                    employeenotb.Text = employee.EmployeeNo;



                    //Employee Information
                    divisioncb.SelectedValue = employee.EmployeeDivisionID;
                    positioncb.SelectedValue = employee.EmployeePositionID;
                    statuscb.SelectedValue = employee.EmployeeStatusID;
                    datehiredp.SelectedDate = employee.DateHired;
                    datepermanetdp.SelectedDate = employee.DatePermanency;
                    dateendodp.SelectedDate = employee.DateEndContract;
                    dateresignationdp.SelectedDate = employee.DateResigned;

                    //Personal Information
                    fnametb.Text = employee.FirstName;
                    mnametb.Text = employee.MiddleName;
                    lnametb.Text = employee.LastName;
                    namesuffixtb.Text = employee.NameSuffix;
                    birthdatedp.SelectedDate = employee.Birthday;
                    birthplacetb.Text = employee.Birthplace;
                    genderdp.SelectedIndex = employee.Sex == "M" ? 0 : 1;

                    citizenshiptb.Text = employee.Nationality;
                    civilstatuscb.Text = employee.CivilStatus.ToString();

                    religiontb.Text = employee.Religion;
                    tinnotb.Text = employee.TIN;
                    sssnotb.Text = employee.SSS;
                    philhealthnotb.Text = employee.PhilHealth;
                    pagibignotb.Text = employee.PAG_IBIG;

                    //Contact Information
                    permaddtb.Text = employee.PermanentAddress;
                    currentadd.Text = employee.CurrentAddress;
                    landlinenotb.Text = employee.TelephoneNo;
                    mobilenotb.Text = employee.MobileNo;
                    primaryemailtb.Text = employee.PrimaryEmail;
                    secondaryemailtb.Text = employee.SecondaryEmail;

                    activechk.IsChecked = employee.Archive;

                    if (employee.Photo == null)
                    {


                    }
                    else
                    {
                        Stream StreamObj = new MemoryStream(employee.Photo);
                        BitmapImage BitObj = new BitmapImage();
                        BitObj.BeginInit();
                        BitObj.StreamSource = StreamObj;
                        BitObj.EndInit();
                        this.empimage.Source = BitObj;

                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }

        private void browsebtn_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" + "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic (*.png)|*.png";


            if (op.ShowDialog() == true)
            {
                newimage = op.FileName;
                ImageSource imageSource = new BitmapImage(new Uri(newimage));
                empimage.Source = imageSource;

            }
            Mouse.OverrideCursor = null;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            currentadd.Text = permaddtb.Text;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            currentadd.Text = null;
        }
    }
}
