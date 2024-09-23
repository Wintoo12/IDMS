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
    public partial class ManageInstallation_AddForm_2 : Form
    {
        int PackageID;
        public ManageInstallation_AddForm_2(int packageID)
        {
            InitializeComponent();
            PackageID = packageID; // Set the packageID in the textbox
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

        private void btnNext_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageInstallation_AddForm_3 addForm_3 = new ManageInstallation_AddForm_3(PackageID);
            addForm_3.Show();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageInstallation installation = new ManageInstallation();
            installation.Show();
        }

        private void btnAddToList_Click(object sender, EventArgs e)
        {
            try
            {
                Connection.Connection.DB();
                Functions.Functions.query = "SELECT componentName FROM component WHERE packageID = @packageID AND componentName = @componentName";
                Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
                Functions.Functions.command.Parameters.AddWithValue("@packageID", txtPackageID.Text);
                Functions.Functions.command.Parameters.AddWithValue("@componentName", txtProductName.Text);
                Functions.Functions.reader = Functions.Functions.command.ExecuteReader();

                if (Functions.Functions.reader.HasRows)
                {
                    Functions.Functions.reader.Read();
                    string componentName = Functions.Functions.reader["componentName"].ToString();

                    if (componentName == txtProductName.Text)
                    {
                        MessageBox.Show("This component name is already saved in the database! Please enter a new component.", "Invalid!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        clear();
                    }
                }
                else 
                {
                    Functions.Functions.reader.Close();
                    InsertNewComponent(Connection.Connection.con);
                    MessageBox.Show("The component is saved in the database!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void InsertNewComponent(SqlConnection con) 
        {
            try
            {
                Connection.Connection.DB();
                Functions.Functions.query = "Insert into component (componentName, componentQuantity, componentUnit, componentDescription, packageID) values (@componentName, @componentQuantity, @componentUnit, @componentDescription, @packageID)";
                Functions.Functions.command = new SqlCommand(Functions.Functions.query, con);

                Functions.Functions.command.Parameters.AddWithValue("@componentName", txtProductName.Text);
                Functions.Functions.command.Parameters.AddWithValue("@componentQuantity", txtQuantity.Text);
                Functions.Functions.command.Parameters.AddWithValue("@componentUnit", txtUnit.Text);
                Functions.Functions.command.Parameters.AddWithValue("@componentDescription", txtDescription.Text);
                Functions.Functions.command.Parameters.AddWithValue("@packageID", txtPackageID.Text);

                PackageID = Convert.ToInt32(Functions.Functions.command.ExecuteScalar());
                Functions.Functions.command.ExecuteNonQuery();
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

        private void ManageInstallation_AddForm_2_Load(object sender, EventArgs e)
        {
            Console.WriteLine(PackageID);
            txtPackageID.Text = PackageID.ToString();
        }
    }
}
