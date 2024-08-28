using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDMS.Admin.Manage_Installation
{
    public partial class ManageInstallation_AddForm_3 : Form
    {
        private int _packageID;
        public ManageInstallation_AddForm_3(int PackageID)
        {
            InitializeComponent();
            _packageID = PackageID;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handleParam = base.CreateParams;
                handleParam.ExStyle |= 0x02000000;
                return handleParam;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageInstallation manageInstallation = new ManageInstallation();
            manageInstallation.Show();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageInstallation_AddForm_2 addForm_2 = new ManageInstallation_AddForm_2(_packageID);
            addForm_2.Show();
        }

        private void btnAddToList_Click(object sender, EventArgs e)
        {
            try
            {
                Connection.Connection.DB();
                Functions.Functions.query = "Select ItemName from freeItem where packageID = @packageID and itemName = @itemName";
                Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
                Functions.Functions.command.Parameters.AddWithValue("@packageID", txtPackageID.Text);
                Functions.Functions.command.Parameters.AddWithValue("@itemName", txtProductName.Text);
                Functions.Functions.reader = Functions.Functions.command.ExecuteReader();

                if (Functions.Functions.reader.HasRows)
                {
                    Functions.Functions.reader.Read();
                    string itemName = Functions.Functions.reader["ItemName"].ToString();

                    if (itemName == txtProductName.Text)
                    {
                        MessageBox.Show("This item name is already saved in the database! Please enter a new employee.", "Invalid!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        clear();
                    }
                }
                else 
                {
                    Functions.Functions.reader.Close();
                    InsertNewItem(Connection.Connection.con);
                    MessageBox.Show("The Item is saved in the database.", "Saved!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
                } 
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void InsertNewItem(SqlConnection con) 
        {
            try
            {
                Connection.Connection.DB();
                Functions.Functions.query = "Insert into freeItem (ItemName, quantity, unit, itemDescription, packageID) values(@ItemName, @quantity, @unit, @itemDescription, @packageID)";
                Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);

                Functions.Functions.command.Parameters.AddWithValue("@ItemName", txtProductName.Text);
                Functions.Functions.command.Parameters.AddWithValue("@quantity", txtQuantity.Text);
                Functions.Functions.command.Parameters.AddWithValue("@unit", txtUnit.Text);
                Functions.Functions.command.Parameters.AddWithValue("@itemDescription", txtDescription.Text);
                Functions.Functions.command.Parameters.AddWithValue("@packageID", txtPackageID.Text);

                Functions.Functions.command.ExecuteNonQuery();
                Connection.Connection.con.Close();
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void clear() 
        {
            txtProductName.Clear();
            txtQuantity.Clear();
            txtUnit.Clear();
            txtDescription.Clear();
        }

        private void ManageInstallation_AddForm_3_Load(object sender, EventArgs e)
        {
            txtPackageID.Text = _packageID.ToString();
        }
    }
}
