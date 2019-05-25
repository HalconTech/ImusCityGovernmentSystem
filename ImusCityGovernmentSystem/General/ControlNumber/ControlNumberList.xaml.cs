using ImusCityGovernmentSystem.Model;
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

namespace ImusCityGovernmentSystem.General.ControlNumber
{
    /// <summary>
    /// Interaction logic for ControlNumberList.xaml
    /// </summary>
    public partial class ControlNumberList : MetroWindow
    {
        public ControlNumberList()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();

                accountcb.ItemsSource = db.FundBanks.OrderBy(m => m.Fund.FundName).ToList();
                accountcb.SelectedValuePath = "FundBankID";
                GetList();

            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }

        public void GetList()
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();


                dgControlNumberList.ItemsSource = db.ControlNumbers.OrderByDescending(m => m.ControlNoID).ToList();
                dgControlNumberList.SelectedValuePath = "ControlNoID";

            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }

        public void ResetState()
        {
            accountcb.Text = "";
            fundtb.Text = "";
            banktb.Text = "";
            controlnobegintb.Value = null;
            controlnocurrenttb.Value = null;
            controlnoendtb.Value = null;
            activechk.IsChecked = true;
            addbtn.IsEnabled = true;
            savebtn.IsEnabled = false;
        }

        private void savebtn_Click(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                if (dgControlNumberList.SelectedValue != null)
                {
                    ImusCityHallEntities db = new ImusCityHallEntities();
                    int id = (int)dgControlNumberList.SelectedValue;

                    addbtn.IsEnabled = false;
                    savebtn.IsEnabled = true;

                    var x = db.ControlNumbers.Find(id);

                    if (x != null)
                    {
                        int accountID = Convert.ToInt32(accountcb.SelectedValue);
                        if (String.IsNullOrEmpty(controlnobegintb.Value.ToString()))
                        {
                            MessageBox.Show("Control Number Start cannot be empty.");
                            return;
                        }
                        if (String.IsNullOrEmpty(controlnoendtb.Value.ToString()))
                        {
                            MessageBox.Show("Control Number End cannot be empty.");
                            return;
                        }
                        if (String.IsNullOrEmpty(controlnocurrenttb.Value.ToString()))
                        {
                            MessageBox.Show("Control Number Current cannot be empty.");
                            return;
                        }

                        int start = Convert.ToInt32(controlnobegintb.Value.Value);
                        int end = Convert.ToInt32(controlnoendtb.Value.Value);
                        int next = Convert.ToInt32(controlnocurrenttb.Value.Value);

                        if(IsWithin(next,start,end) != true)
                        {
                            MessageBox.Show("Next control number is not within the range.");
                            return;
                        }


                        x.FundBankID = accountID;
                        x.BeginingControlNo = start;
                        x.EndingControlNo = end;
                        x.NextControlNo = next;
                        //x.Date = DateTime.Now;
                        if (activechk.IsChecked == true)
                            x.Active = true;
                        else
                            x.Active = false;
                        db.SaveChanges();
                        MessageBox.Show("Control Number has successfully edited.");
                        ResetState();
                        GetList();
                    }
                }
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }

        private void addbtn_Click(object sender, RoutedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();

                int accountID = Convert.ToInt32(accountcb.SelectedValue);

                var fundbank = db.FundBanks.Find(accountID);
                if (fundbank != null)
                {
                    if(db.ControlNumbers.Where(m=>m.FundBankID == accountID&& m.Active == true) == null)
                    {
                        MessageBox.Show("There is an active series for this Account.");
                        return;
                    }
                    if (String.IsNullOrEmpty(controlnobegintb.Value.ToString()))
                    {
                        MessageBox.Show("Control Number Start cannot be empty.");
                        return;
                    }
                    if (String.IsNullOrEmpty(controlnoendtb.Value.ToString()))
                    {
                        MessageBox.Show("Control Number End cannot be empty.");
                        return;
                    }
                    if (String.IsNullOrEmpty(controlnocurrenttb.Value.ToString()))
                    {
                        MessageBox.Show("Control Number Current cannot be empty.");
                        return;
                    }
                    Model.ControlNumber cn = new Model.ControlNumber();

                    int start = Convert.ToInt32(controlnobegintb.Value.Value);
                    int end = Convert.ToInt32(controlnoendtb.Value.Value);
                    int next = Convert.ToInt32(controlnocurrenttb.Value.Value);

                    if (IsWithin(next, start, end) != true)
                    {
                        MessageBox.Show("Next control number is not within the range.");
                        return;
                    }
                    cn.FundBankID = accountID;
                    cn.BeginingControlNo = next;
                    cn.EndingControlNo = end;
                    cn.NextControlNo = next;
                    cn.Date = DateTime.Now;
                    if (activechk.IsChecked == true)
                        cn.Active = true;
                    else
                        cn.Active = false;
                    db.ControlNumbers.Add(cn);
                    db.SaveChanges();
                    MessageBox.Show("Control Number has successfully added.");
                    ResetState();
                    GetList();
                }
                else
                {
                    MessageBox.Show("Select an account to be process.");
                }



            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }

        private void btnToggleActive_Click(object sender, RoutedEventArgs e)
        {

            if (SystemClass.CheckConnection())
            {
                if (dgControlNumberList.SelectedValue != null)
                {
                    ImusCityHallEntities db = new ImusCityHallEntities();
                    int id = (int)dgControlNumberList.SelectedValue;
                    ImusCityGovernmentSystem.Model.ControlNumber controlnumber = db.ControlNumbers.Find(id);
                    controlnumber.Active = false;
                    db.SaveChanges();

                    GetList();
                    ResetState();
                }
                else
                {
                    MessageBox.Show("Please select an item");
                }
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }

        private void refbtn_Click(object sender, RoutedEventArgs e)
        {
            GetList();
            ResetState();
        }

        private void accountcb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                ImusCityHallEntities db = new ImusCityHallEntities();

                int accountID = Convert.ToInt32(accountcb.SelectedValue);

                var fundbank = db.FundBanks.Find(accountID);
                if (fundbank != null)
                {
                    fundtb.Text = fundbank.Fund.FundName;
                    banktb.Text = fundbank.Bank.BankName;
                }


            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }

        private void dgControlNumberList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SystemClass.CheckConnection())
            {
                if (dgControlNumberList.SelectedValue != null)
                {
                    ImusCityHallEntities db = new ImusCityHallEntities();
                    int id = (int)dgControlNumberList.SelectedValue;

                    addbtn.IsEnabled = false;
                    savebtn.IsEnabled = true;

                    var x = db.ControlNumbers.Find(id);

                    if (x != null)
                    {
                        accountcb.SelectedValue = x.FundBankID;
                        fundtb.Text = x.FundBank.Fund.FundName;
                        banktb.Text = x.FundBank.Bank.BankName;
                        controlnobegintb.Value = x.BeginingControlNo.Value;
                        controlnoendtb.Value = x.EndingControlNo.Value;
                        controlnocurrenttb.Value = x.NextControlNo.Value;

                        if (x.Active == true)
                            activechk.IsChecked = true;
                        else
                            activechk.IsChecked = false;

                    }
                }
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        }
        public static bool IsWithin(int value, int minimum, int maximum)
        {
            return value >= minimum && value <= maximum;
        }

    }
}
