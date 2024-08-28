using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDMS.Admin.Manage_Customer
{
    public partial class ManageCustomer_AddForm : Form
    {
        public ManageCustomer_AddForm()
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

        private void btnSaveInfo_Click(object sender, EventArgs e)
        {
            try
            {
                string FName = txtFName.Text;
                string MName = txtMName.Text;
                string LName = txtLName.Text;

                DialogResult result = MessageBox.Show("Please confirm if the information that is provided is correct.", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    // Check if the customer already exists in the database
                    Connection.Connection.DB();
                    Functions.Functions.query = "SELECT COUNT(*) FROM customer WHERE FName = @FName AND MName = @MName AND LName = @LName";
                    Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
                    Functions.Functions.command.Parameters.AddWithValue("@FName", FName);
                    Functions.Functions.command.Parameters.AddWithValue("@MName", MName);
                    Functions.Functions.command.Parameters.AddWithValue("@LName", LName);
                    int count = (int)Functions.Functions.command.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show("Customer with the same name already exists in the database. Please provide unique information.", "Duplicate Customer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; // Exit the method without saving duplicate information
                    }

                    // Get the file path of the image
                    string imagePath = txtFilename.Text; // You need to provide a TextBox or some input mechanism to get the file path from the user

                    // Insert the customer information into the database
                    string filename = txtFilename.Text;
                    Functions.Functions.query = "INSERT INTO customer (FName, MName, LName, Fb_accnt, contact_num, barangay, municipality, status, fileName) " +
                        "VALUES ('" + char.ToUpper(FName[0]) + FName.Substring(1) + "','" + char.ToUpper(MName[0]) + MName.Substring(1) + "','" + char.ToUpper(LName[0]) + LName.Substring(1) + "','" + txtFB_acnt.Text + "','" +
                        txtContactNum.Text + "','" + txtBarangay.Text + "','" + txtMunicipality.Text + "','" + "Active" + "','" + filename + "')";
                    Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
                    Functions.Functions.command.CommandTimeout = 5000;
                    Functions.Functions.command.ExecuteNonQuery();

                    MessageBox.Show("Customer information is saved to the database!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Connection.Connection.con.Close();

                    this.Hide();
                    Admin.ManageCustomer customerDashboard = new ManageCustomer();
                    customerDashboard.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {

            pcboxCustomerImage.Image = null;
            using (OpenFileDialog openFD = new OpenFileDialog() { Filter = "Image Files (*.jpg; *.jpeg)| *.jpg;*.jpeg", Multiselect = false })
            {
                if (openFD.ShowDialog() == DialogResult.OK)
                {
                    pcboxCustomerImage.Image = Image.FromFile(openFD.FileName);
                    txtFilename.Text = openFD.FileName;
                }
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

        private void txtContactNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
            {
                return;
            }

            // Check if the key is a digit or a decimal point
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                // Display an error message
                MessageBox.Show("Invalid input, please input a threshold.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Handled = true; // Suppress the key press
            }
            else
            {
                // Allow only one decimal point
                if (e.KeyChar == '.' && (sender as TextBox).Text.Contains("."))
                {
                    e.Handled = true; // Suppress the key press
                }
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

        private void txtFName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar) || char.IsLetter(e.KeyChar) || e.KeyChar == '-' || e.KeyChar == '\'')
            {
                return;
            }

            MessageBox.Show("Invalid input, please enter letters, dash, or hyphen only.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            e.Handled = true;
        }

        private void txtLName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar) || char.IsLetter(e.KeyChar) || e.KeyChar == '-' || e.KeyChar == '\'')
            {
                return;
            }

            MessageBox.Show("Invalid input, please enter letters, dash, or hyphen only.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            e.Handled = true;
        }
    }
}
