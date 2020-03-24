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
                        reader.Close();
                        //Account Exists time to check the password
                        string checkPass = "EXEC checkUsername @Username =" + tbUsername.Text + ", @Password =" + tbPassword.Text + ";";
                        sqlComm = new SqlCommand(checkPass, cnn);
                        reader = sqlComm.ExecuteReader(); 
                        if (reader.HasRows == true)
                        {
                            reader.Read();
                            string pullUser = "EXEC PullUser @UserId =" + reader["userID"] + ";";
                            reader.Close(); //password matches
                            sqlComm = new SqlCommand(pullUser, cnn);
                            reader = sqlComm.ExecuteReader(); reader.Read();
                            //saves info from database to new user object
                            user.UserID = Convert.ToInt32(reader["userID"]);
                            user.username = reader["username"].ToString();
                            user.password = reader["password"].ToString();
                            user.name = reader["name"].ToString();
                            user.accessLevel = Convert.ToInt32(reader["accesslevel"]);
                            user.lockedOut = Convert.ToInt32(reader["lockedOut"]);
                            user.counter = 0;
                            reader.Close();
                            //some security checks
                            if (user.lockedOut == 1)//checks if user is already locked out
                            {
                                MessageBox.Show("Account is locked");
                            }
                            else
                            {
                                Form MainMenu = new MainMenu();
                                MainMenu.Show();
                                this.Hide();
                            }
                            user.updateDatabase();//updates the database after possible adjustments have been made
                        }//end if username is in the database
                        else//have to incriment counter for the username
                        {
                            string pullCounter = "EXEC pullCounter @Username =" + user.username + ";";
                            sqlComm = new SqlCommand(pullCounter, cnn);
                            reader = sqlComm.ExecuteReader(); reader.Read();
                            user.counter = Convert.ToInt32(reader["counter"]);
                            user.counter++;
                            reader.Close();
                            if(user.counter >= 3)
                            {
                                user.lockedOut = 1;
                                MessageBox.Show("Failed too many attempts account locked");
                            }
                            else
                            {
                                MessageBox.Show("Invalid Login");
                            }

                            string pushCounter = "EXEC incrementCounter @Username =" + user.username + ", @Counter =" + user.counter + ";";
                            sqlComm = new SqlCommand(pushCounter, cnn);
                            sqlComm.ExecuteNonQuery();
                        }//end incriment for failed password
                        

                        

                        
                    }//end username is not in database
                    else
                    {
                        MessageBox.Show("Account does not exist");//tells user username no bueno
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
