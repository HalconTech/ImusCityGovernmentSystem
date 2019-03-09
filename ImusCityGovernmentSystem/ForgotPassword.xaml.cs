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
    /// Interaction logic for ForgotPassword.xaml
    /// </summary>
    public partial class ForgotPassword : MetroWindow
    {
        int? QuestionID;
        public ForgotPassword()
        {
            InitializeComponent();
        }
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadInitialQuestion();
        }

        public void LoadInitialQuestion()
        {
            try
            {
                using (var db = new ImusCityHallEntities())
                {
                    var getques = db.SecurityQuestionUsers.Where(m => m.EmployeeID == App.EmployeeID).OrderBy(m => Guid.NewGuid()).FirstOrDefault();
                    if (getques != null)
                    {
                        txtSecQues.Text = getques.SecurityQuestionBank.Question;
                        QuestionID = getques.SecurityQuestionID;
                    }
                    else
                    {
                        MessageBox.Show("Question not available.");
                        return;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new ImusCityHallEntities())
                {
                    if (!String.IsNullOrEmpty(txtAnswer.Text))
                    {
                        var getanswer = db.SecurityQuestionUsers.Where(m => m.EmployeeID == App.EmployeeID && m.SecurityQuestionID == QuestionID).FirstOrDefault();

                        if (getanswer != null)
                        {
                            string inputAnswer = txtAnswer.Text;
                            if (inputAnswer.TrimStart().Trim().TrimEnd().ToLower() == getanswer.Answer.TrimStart().Trim().TrimEnd().ToLower())
                            {
                                ChangePasswordWindow cp = new ChangePasswordWindow();
                                cp.Show();
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("Incorrect answer.");
                                return;
                            }

                        }
                        else
                        {
                            MessageBox.Show("Question not available.");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please input answer.");
                        return;
                    }



                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
