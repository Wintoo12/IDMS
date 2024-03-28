using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDMS.Connection
{
    internal class Connection
    {
        public static SqlConnection con;
        //Connection for the database
        private static string dbConnect = "Data Source=MARK\\SQLEXPRESS03;Initial Catalog=IDMS;Integrated Security=True";

        public static void DB()
        {
            try
            {
                con = new SqlConnection(dbConnect);
                con.Open();
            }
            catch (Exception ex)
            {
                con.Close();
                MessageBox.Show(ex.Message);
            }
        }
    }
}
