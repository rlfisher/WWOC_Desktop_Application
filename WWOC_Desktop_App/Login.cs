using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace WWOC_Desktop_App
{
    //to use panels or tab control for multiple form selection.
    public partial class Login : Form
    {
        
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            using (SqlConnection cnn = new SqlConnection("Data Source=10.135.85.184;Initial Catalog=GROUP4;User ID=Group4;Password=Grp4s2117"))
            {
                cnn.Open();
                //string checkUser = "EXEC checkUsername @Username =" + tbUsername.Text + ", @Password =" + tbPassword.Text + ";";
                string checkUser = "EXEC isUsername @Username =" + tbUsername.Text + ";";
                SqlCommand sqlComm = new SqlCommand(checkUser, cnn);
                try
                {
                    SqlDataReader reader = sqlComm.ExecuteReader();
                    if (reader.HasRows == true)//username is in the database
                    {
                        user user = new user();
                        reader.Read();
                        user.username = reader["username"].ToString();
                        //reader.Close();
                        //Account Exists time to check the password
                        string checkPass = "EXEC checkUsername @Username =" + tbUsername.Text + ", @Password =" + tbPassword.Text + ";";
                        sqlComm = new SqlCommand(checkPass, cnn);
                        reader = sqlComm.ExecuteReader();
                        if(reader.HasRows == true)
                        {
                            reader.Read();
                            string pullUser = "EXEC PullUser @UserId =" + reader["userID"] + ";";
                            //reader.Close(); //password matches
                            sqlComm = new SqlCommand(pullUser, cnn);
                            reader = sqlComm.ExecuteReader();
                            reader.Read();
                            //saves info from database to new user object
                            user.UserID = Convert.ToInt32(reader["userID"]);
                            user.username = reader["username"].ToString();
                            user.password = reader["password"].ToString();
                            user.name = reader["name"].ToString();
                            user.accessLevel = Convert.ToInt32(reader["accesslevel"]);
                            user.lockedOut = Convert.ToInt32(reader["lockedOut"]);
                            user.counter = 0;
                            //some security checks
                            if (user.lockedOut == 1)//checks if user is already locked out
                            {
                                MessageBox.Show("Account is locked");
                            }
                            else if (user.counter >= 3)//checks if 3rd attempt to sign in has been made
                            {
                                MessageBox.Show("3 failed attempts account is now locked");
                                user.lockedOut = 1;
                            }
                            else
                            {
                                Form MainMenu = new MainMenu();
                                MainMenu.Show();
                                this.Hide();
                            }
                        }
                        else
                        {
                            string pullCounter = "EXEC pullCounter @Username =" + user.username + ";";
                            sqlComm = new SqlCommand(pullCounter, cnn);
                            reader = sqlComm.ExecuteReader();
                        }
                        

                        

                        user.updateDatabase();//updates the database after possible adjustments have been made
                    }
                    else
                    {
                        MessageBox.Show("Account does not exist");
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("bad stop it!!! " + ex);
                }
               
                cnn.Close();
            }
        }
    }
}
