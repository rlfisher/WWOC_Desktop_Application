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
                string checkUser = "EXEC checkUsername @Username =" + tbUsername.Text + ", @Password =" + tbPassword.Text + ";";
                SqlCommand sqlComm = new SqlCommand(checkUser, cnn);
                try
                {
                    SqlDataReader reader = sqlComm.ExecuteReader();
                    if (reader.HasRows == true)
                    {
                        MessageBox.Show("account is a thing");
                    }
                    else
                    {
                        MessageBox.Show("Not a thing");
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
