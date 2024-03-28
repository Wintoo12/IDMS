using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDMS.Functions
{
    internal class Functions
    {
        public static string query = ""; //A variable that will hold SQL statements or Query
        public static SqlConnection con; //Variable for connection string
        public static SqlCommand command; //Process the SQL statement and connection
        public static SqlDataReader reader;//Retrieve data from the database 

        public static void fill(string q, DataGridView dgv)
        {
            //String q -> Retrieved SQL statement
            //DataGridview dgv -> a componenet where the retrieved SQL statements are displayed
            try
            {
                Connection.Connection.DB(); //Calling the server location
                DataTable dt = new DataTable(); //this is used for storing the data from the database
                SqlDataAdapter adapter = null;
                SqlCommand command = new SqlCommand(q, Connection.Connection.con);
                adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);
                dgv.DataSource = dt; //retrieve all the records from the database and display it in the datagridview
                Connection.Connection.con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
