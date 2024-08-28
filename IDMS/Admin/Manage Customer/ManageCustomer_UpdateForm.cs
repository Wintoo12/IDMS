using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using System.Drawing.Imaging;
using System.Security.Cryptography;

namespace IDMS.Admin.Manage_Customer
{
    public partial class ManageCustomer_UpdateForm : Form
    {
        public static int photoId = 0;
        public ManageCustomer_UpdateForm()
        {
            InitializeComponent();
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

        private void ManageCustomer_UpdateForm_Load(object sender, EventArgs e)
        {
            //Console.Write(CustomerID);
        }

        public void FillCustomerDetails(int customerID) 
        {
            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=MARK\\SQLEXPRESS03;Initial Catalog=IDMS;Integrated Security=True"))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SELECT * FROM customer WHERE customerID = @customerID", connection))
                    {
                        command.Parameters.AddWithValue("@customerID", customerID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                txtCustomerID.Text = reader["customerID"].ToString();
                                txtFName.Text = reader["FName"].ToString();
                                txtMName.Text = reader["MName"].ToString();
                                txtLName.Text = reader["LName"].ToString();
                                txtFB_acnt.Text = reader["Fb_accnt"].ToString();
                                txtContactNum.Text = reader["contact_num"].ToString();
                                txtBarangay.Text = reader["barangay"].ToString();
                                txtMunicipality.Text = reader["municipality"].ToString();
                                string status = reader["status"].ToString();
                                if (status.Equals("Active", StringComparison.OrdinalIgnoreCase))
                                {
                                    rbtnActive.Checked = true;
                                    rbtnInactive.Checked = false;
                                }
                                else if (status.Equals("Inactive", StringComparison.OrdinalIgnoreCase))
                                {
                                    rbtnInactive.Checked = true;
                                    rbtnActive.Checked = false;
                                }
                                else 
                                {
                                    MessageBox.Show("Error");
                                }
                                
                                txtFilename.Text = reader["fileName"].ToString();

                                string fileName = reader["fileName"].ToString(); // Using fileName to store the path

                                if (!string.IsNullOrEmpty(fileName))
                                {
                                    pcboxCustomerPhoto.ImageLocation = fileName;
                                }
                                else
                                {
                                    pcboxCustomerPhoto.Image = null; // Clear PictureBox if no photo found
                                }

                                Console.WriteLine(customerID);
                                Console.WriteLine(txtFName.Text);
                                Console.WriteLine(txtMName.Text);
                                Console.WriteLine(txtLName.Text);
                                Console.WriteLine(txtFB_acnt.Text);
                                Console.WriteLine(txtContactNum.Text);
                                Console.WriteLine(txtBarangay.Text);
                                Console.WriteLine(txtMunicipality.Text);

                            }
                            else
                            {
                                MessageBox.Show("No data found for the selected customer.");
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                //Connection.Connection.con.Close();
                MessageBox.Show(ex.Message);
            }

        }
            

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to cancel saving customer info?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                this.Hide();
                Admin.ManageCustomer manageCustomer = new ManageCustomer();
                manageCustomer.Show();
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFD = new OpenFileDialog() { Filter = "Image Files (*.jpg; *.jpeg)| *.jpg;*.jpeg", Multiselect = false })
            {
                if (openFD.ShowDialog() == DialogResult.OK)
                {
                    pcboxCustomerPhoto.Image = Image.FromFile(openFD.FileName);
                    txtFilename.Text = openFD.FileName;
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string status = "";
                if (rbtnActive.Checked)
                {
                    status = rbtnActive.Text;
                    rbtnInactive.Checked = false;
                }
                else
                {
                    status = rbtnInactive.Text;
                    rbtnActive.Checked = false;
                }

                string FName = txtFName.Text;
                string MName = txtMName.Text;
                string LName = txtLName.Text;

                DialogResult result = MessageBox.Show("Please verify that the changes made are accurate.", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    Connection.Connection.DB();
                    string filename = txtFilename.Text;

                    Functions.Functions.query = "UPDATE customer SET FName = @FName, MName = @MName, LName = @LName, Fb_accnt = @FBaccnt, contact_num = @ContactNum, barangay = @Brgy," +
                        "municipality = @Municipality, status = @Status, fileName = @FileName WHERE customerID = @CustomerID";
                    Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);

                    Functions.Functions.command.Parameters.AddWithValue("@FName", txtFName.Text);
                    Functions.Functions.command.Parameters.AddWithValue("@MName", txtMName.Text);
                    Functions.Functions.command.Parameters.AddWithValue("@LName", txtLName.Text);
                    Functions.Functions.command.Parameters.AddWithValue("@FBaccnt", txtFB_acnt.Text);
                    Functions.Functions.command.Parameters.AddWithValue("@ContactNum", txtContactNum.Text);
                    Functions.Functions.command.Parameters.AddWithValue("@Brgy", txtBarangay.Text);
                    Functions.Functions.command.Parameters.AddWithValue("@Municipality", txtMunicipality.Text);
                    Functions.Functions.command.Parameters.AddWithValue("@Status", status);
                    Functions.Functions.command.Parameters.AddWithValue("@FileName", filename);
                    Functions.Functions.command.Parameters.AddWithValue("@CustomerID", txtCustomerID.Text);

                    Functions.Functions.command.ExecuteNonQuery();

                    MessageBox.Show("Customer Information is updated in the database!", "Updated!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Connection.Connection.con.Close();

                    this.Hide();
                    ManageCustomer mngCustomer = new ManageCustomer();
                    mngCustomer.Show();
                }
                else
                {
                    this.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtFName_Click(object sender, EventArgs e)
        {
            if (txtFName.Text == "First Name")
            {
                txtFName.Clear();
                txtFName.ForeColor = Color.Black;
            }
        }

        private void txtFName_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFName.Text))
            {
                txtFName.Text = "First Name";
                txtFName.ForeColor = Color.DarkGray;
            }
        }

        private void txtLName_Click(object sender, EventArgs e)
        {
            if (txtLName.Text == "Last Name")
            {
                txtLName.Clear();
                txtLName.ForeColor = Color.Black;
            }
        }

        private void txtLName_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLName.Text))
            {
                txtLName.Text = "Last Name";
                txtLName.ForeColor = Color.DarkGray;
            }
        }

        private void txtBarangay_Click(object sender, EventArgs e)
        {
            if (txtBarangay.Text == "Barangay")
            {
                txtBarangay.Clear();
                txtBarangay.ForeColor = Color.Black;
            }
        }

        private void txtBarangay_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBarangay.Text))
            {
                txtBarangay.Text = "Barangay";
                txtBarangay.ForeColor = Color.DarkGray;
            }
        }

        private void txtMunicipality_Click(object sender, EventArgs e)
        {
            if (txtMunicipality.Text == "Municipality")
            {
                txtMunicipality.Clear();
                txtMunicipality.ForeColor = Color.Black;
            }
        }

        private void txtMunicipality_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMunicipality.Text))
            {
                txtMunicipality.Text = "Municipality";
                txtMunicipality.ForeColor = Color.DarkGray;
            }
        }
    }
}
