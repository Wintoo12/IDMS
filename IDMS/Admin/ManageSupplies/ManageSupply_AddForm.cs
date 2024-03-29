using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDMS
{
    public partial class ManageSupply_Add_form : Form
    {
        public ManageSupply_Add_form()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Connection.Connection.DB();
                Functions.Functions.query = "Insert into product(productName, productPrice, product_quantity, product_Limit, supplierID) values('" + txtProductName.Text + "','" + " " + "','" + " " + "','" + " " + "','" + " " + "')";
                Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
                Functions.Functions.command.ExecuteNonQuery();

                MessageBox.Show("The Product is saved into the database", "Saved!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Connection.Connection.con.Close();

                txtProductName.Clear();
                this.Hide();
                ManageSupply_DashboardAdmin dashboard = new ManageSupply_DashboardAdmin();
                dashboard.Show();   
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageSupply_DashboardAdmin dashboard = new ManageSupply_DashboardAdmin();
            dashboard.Show();
        }
    }
}
