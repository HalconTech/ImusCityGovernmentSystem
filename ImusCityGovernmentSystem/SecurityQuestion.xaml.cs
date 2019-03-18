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

namespace ImusCityGovernmentSystem
{
    /// <summary>
    /// Interaction logic for SecurityQuestion.xaml
    /// </summary>
    public partial class SecurityQuestion : MetroWindow
    {
        List<QuestionList> QList = new List<QuestionList>();
        public SecurityQuestion()
        {
            InitializeComponent();
            LoadQuestion();
        }
        public void LoadQuestion()
        {
            if(SystemClass.CheckConnection())
            {
                using (var db = new ImusCityHallEntities())
                {
                    //int total = db.SecurityQuestionBanks.Count();
                    //decimal toTakeD = (total / 3);
                    //int toTake = (int)toTakeD;
                    var ques = db.SecurityQuestionBanks.ToList();

                    cbSecurityQuestion1.ItemsSource = ques.OrderBy(m => Guid.NewGuid()).Take(7).ToList();
                    cbSecurityQuestion1.DisplayMemberPath = "Question";
                    cbSecurityQuestion1.SelectedValuePath = "SecurityQuestionID";

                    cbSecurityQuestion2.ItemsSource = ques.OrderBy(m => Guid.NewGuid()).Take(7).ToList();
                    cbSecurityQuestion2.DisplayMemberPath = "Question";
                    cbSecurityQuestion2.SelectedValuePath = "SecurityQuestionID";

                    cbSecurityQuestion3.ItemsSource = ques.OrderBy(m => Guid.NewGuid()).Take(7).ToList();
                    cbSecurityQuestion3.DisplayMemberPath = "Question";
                    cbSecurityQuestion3.SelectedValuePath = "SecurityQuestionID";
                }
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
                using (var db = new ImusCityHallEntities())
                {


                    QList = new List<QuestionList>();
                    if (!String.IsNullOrEmpty(cbSecurityQuestion1.Text) && !String.IsNullOrEmpty(txtAnswer1.Text))
                    {
                        QuestionList ql = new QuestionList();
                        int QuestionID1 = Convert.ToInt32(cbSecurityQuestion1.SelectedValue);
                        ql.EmployeeID = App.EmployeeID;
                        ql.QuestionID = QuestionID1;
                        ql.Answer = txtAnswer1.Text;
                        QList.Add(ql);
                    }
                    else
                    {
                        MessageBox.Show("Please answer security questions.");
                        return;
                    }
                    if (!String.IsNullOrEmpty(cbSecurityQuestion2.Text) && !String.IsNullOrEmpty(txtAnswer2.Text))
                    {
                        QuestionList ql = new QuestionList();
                        int QuestionID2 = Convert.ToInt32(cbSecurityQuestion2.SelectedValue);
                        ql.EmployeeID = App.EmployeeID;
                        ql.QuestionID = QuestionID2;
                        ql.Answer = txtAnswer2.Text;
                        QList.Add(ql);
                    }
                    else
                    {
                        MessageBox.Show("Please answer security questions.");
                        return;
                    }
                    if (!String.IsNullOrEmpty(cbSecurityQuestion3.Text) && !String.IsNullOrEmpty(txtAnswer3.Text))
                    {
                        QuestionList ql = new QuestionList();
                        int QuestionID3 = Convert.ToInt32(cbSecurityQuestion3.SelectedValue);
                        ql.EmployeeID = App.EmployeeID;
                        ql.QuestionID = QuestionID3;
                        ql.Answer = txtAnswer3.Text;
                        QList.Add(ql);
                    }
                    else
                    {
                        MessageBox.Show("Please answer security questions.");
                        return;
                    }

                    foreach (var x in QList)
                    {
                        SecurityQuestionUser squ = new SecurityQuestionUser();
                        squ.EmployeeID = x.EmployeeID;
                        squ.SecurityQuestionID = x.QuestionID;
                        squ.Answer = x.Answer;
                        db.SecurityQuestionUsers.Add(squ);
                        db.SaveChanges();
                    }

                    MessageBox.Show("Security Question set-up succesfully!");
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show(SystemClass.DBConnectionErrorMessage);
            }
        
        }
    }
    public class QuestionList
    {
        public int EmployeeID { get; set; }
        public int QuestionID { get; set; }
        public string Answer { get; set; }
    }
}
