using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data.SqlClient;


namespace WWOC_Desktop_App
{
    class user
    {
        public int UserID { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public int accessLevel { get; set; }
        public int lockedOut { get; set; }
        public int counter { get; set; }

        /*Updates the database in the event that the accessLevel,lockedout, or counter is changed during runtime.
         * Edit stored procedure "UpdateUsers" if more features should be added.
         * Returns: nothing
         */
        public void updateDatabase()
        {
            using (SqlConnection cnn = new SqlConnection("Data Source=10.135.85.184;Initial Catalog=GROUP4;User ID=Group4;Password=Grp4s2117"))
            {
                cnn.Open();
                SqlCommand sqlComm = new SqlCommand(("EXEC UpdateUsers @UserId =" + UserID + ", @AccessLevel =" + accessLevel + ", @LockedOut =" + lockedOut + ", @Counter =" + counter + ";"), cnn);
                sqlComm.ExecuteNonQuery();
                cnn.Close();
            }
        }//end updateDatabase

    }//end user
}
