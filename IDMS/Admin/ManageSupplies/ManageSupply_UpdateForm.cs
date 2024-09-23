using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDMS
{
    public partial class ManageSupply_UpdateForm : Form
    {
        private bool errorShown = false;
        public ManageSupply_UpdateForm()
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

        private void ManageSupply_UpdateForm_Load(object sender, EventArgs e)
        {

        }

        public void FillSupplyDetails(int productID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=MARK\\SQLEXPRESS03;Initial Catalog=IDMS;Integrated Security=True"))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SELECT * FROM product WHERE productID = @productID", connection))
                    {
                        command.Parameters.AddWithValue("@productID", productID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                txtProductID.Text = reader["productID"].ToString();
                                txtProductName.Text = reader["productName"].ToString();
                                txtQuantity.Text = reader["product_quantity"].ToString();
                                txtUnit.Text = reader["productUnit"].ToString();
                                txtThreshold.Text = reader["item_threshold"].ToString();
                                txtPrice.Text = reader["productPrice"].ToString();
                                txtSupplierNamePy.Text = reader["supplierName"].ToString();
                                txtContactNum.Text = reader["contact_num"].ToString();
                                txtLink.Text = reader["shopLink"].ToString();

                                string supplier = reader["store_type"].ToString();
                                if (supplier.Equals("online store", StringComparison.OrdinalIgnoreCase))
                                {
                                    rbtnOnline.Checked = true;
                                    rbtnPhysical.Checked = false;

                                }
                                else if (supplier.Equals("physical store", StringComparison.OrdinalIgnoreCase))
                                {
                                    rbtnPhysical.Checked = true;
                                    rbtnOnline.Checked = false;
                                }
                                else
                                {
                                    MessageBox.Show("Error");
                                }

                                string status = reader["status"].ToString();
                                if (status.Equals("Available", StringComparison.OrdinalIgnoreCase))
                                {
                                    rbtnAvailable.Checked = true;
                                    rbtnNotAvailable.Checked = false;
                                }
                                else if (status.Equals("Not Available", StringComparison.OrdinalIgnoreCase))
                                {
                                    rbtnNotAvailable.Checked = true;
                                    rbtnAvailable.Checked = false;
                                }
                                else
                                {
                                    MessageBox.Show("Error");
                                }

                                txtFilename.Text = reader["fileName"].ToString();

                                string fileName = reader["fileName"].ToString(); // Using fileName to store the path

                                if (!string.IsNullOrEmpty(fileName))
                                {
                                    pcboxProductPhoto.ImageLocation = fileName;
                                }
                                else
                                {
                                    pcboxProductPhoto.Image = null; // Clear PictureBox if no photo found
                                }
                            }
                            else
                            {
                                MessageBox.Show("No data found for the selected product.");
                            }
                        }
                    }
                }
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

        private void rbtnOnline_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnOnline.Checked == true)
            {
                pnlOnline.Visible = true;
                pnlPhysical.Visible = false;
                txtSupplierNamePy.Text = " ";
                txtContactNum.Text = " ";
            }
        }

        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            ValidateQuantityAndThreshold();
        }

        private void rbtnPhysical_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnPhysical.Checked == true)
            {
                pnlPhysical.Visible = true;
                pnlOnline.Visible = false;
                txtSupplierNameOL.Text = " ";
                txtLink.Text = " ";
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFD = new OpenFileDialog() { Filter = "Image Files (*.jpg; *.jpeg)| *.jpg;*.jpeg", Multiselect = false })
            {
                if (openFD.ShowDialog() == DialogResult.OK)
                {
                    pcboxProductPhoto.Image = Image.FromFile(openFD.FileName);
                    txtFilename.Text = openFD.FileName;
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string supplierName = "";

            try
            {
                string store_type = "";
                if (rbtnOnline.Checked == true)
                {
                    store_type = rbtnOnline.Text;
                    rbtnPhysical.Checked = false;
                    supplierName = txtSupplierNameOL.Text;
                }
                else
                {
                    store_type = rbtnPhysical.Text;
                    rbtnOnline.Checked = false;
                    supplierName = txtSupplierNamePy.Text;
                }

                string status = "";
                if (rbtnAvailable.Checked == true)
                {
                    status = rbtnAvailable.Text;
                    rbtnNotAvailable.Checked = false;
                }
                else
                {
                    status = rbtnNotAvailable.Text;
                    rbtnAvailable.Checked = false;
                }

                DialogResult result = MessageBox.Show("Please confirm if the information that has been updated is correct and true.", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                {
                    Connection.Connection.DB();

                    string filename = txtFilename.Text; // Get the file path directly

                    Functions.Functions.query = "UPDATE product SET productName = @productName, productPrice = @productPrice, product_quantity = @product_quantity, productUnit = @productUnit, item_threshold = @item_threshold, " +
                                                "fileName = @fileName, store_type = @store_type, contact_num = @contact_num, supplierName = @supplierName, shopLink = @shopLink, status = @status " +
                                                "WHERE productID = @productID";

                    Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);

                    Functions.Functions.command.Parameters.AddWithValue("@productName", txtProductName.Text);
                    Functions.Functions.command.Parameters.AddWithValue("@productPrice", Convert.ToDouble(txtPrice.Text));
                    Functions.Functions.command.Parameters.AddWithValue("@product_quantity", txtQuantity.Text);
                    Functions.Functions.command.Parameters.AddWithValue("@productUnit", txtUnit.Text);
                    Functions.Functions.command.Parameters.AddWithValue("@item_threshold", txtThreshold.Text);
                    Functions.Functions.command.Parameters.AddWithValue("@fileName", filename);
                    Functions.Functions.command.Parameters.AddWithValue("@store_type", store_type);
                    Functions.Functions.command.Parameters.AddWithValue("@contact_num", txtContactNum.Text);
                    Functions.Functions.command.Parameters.AddWithValue("@supplierName", supplierName);
                    Functions.Functions.command.Parameters.AddWithValue("@shopLink", txtLink.Text);
                    Functions.Functions.command.Parameters.AddWithValue("@status", status);
                    Functions.Functions.command.Parameters.AddWithValue("@productID", txtProductID.Text);

                    Functions.Functions.command.ExecuteNonQuery();

                    MessageBox.Show("Product information is updated in the database!", "Updated!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Connection.Connection.con.Close();

                    this.Hide();
                    ManageSupply_DashboardAdmin admin = new ManageSupply_DashboardAdmin();
                    admin.Show();
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

        private void ValidateQuantityAndThreshold()
        {
            if (double.TryParse(txtQuantity.Text, out double quantity) &&
            double.TryParse(txtThreshold.Text, out double threshold))
            {
                if (quantity > threshold && !errorShown)
                {
                    MessageBox.Show("Error, quantity must be lesser than the threshold", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    errorShown = true;
                }
                else if (quantity <= threshold)
                {
                    errorShown = false;
                }
            }
            else
            {
                errorShown = false;
            }
        }

        private void txtQuantity_Click(object sender, EventArgs e)
        {
            if (txtQuantity.Text == "Enter Quantity")
            {
                txtQuantity.Clear();
                txtQuantity.ForeColor = Color.Black;
            }
        }

        private void txtQuantity_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtQuantity.Text))
            {
                txtQuantity.Text = "Enter Quantity";
                txtQuantity.ForeColor = Color.DarkGray;
            }
        }

        private void txtUnit_Click(object sender, EventArgs e)
        {
            if (txtUnit.Text == "Enter Unit")
            {
                txtUnit.Clear();
                txtUnit.ForeColor = Color.Black;
            }
        }

        private void txtUnit_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUnit.Text))
            {
                txtUnit.Text = "Enter Unit";
                txtUnit.ForeColor = Color.DarkGray;
            }
        }

        private void txtPrice_Click(object sender, EventArgs e)
        {
            if (txtPrice.Text == "Enter Price")
            {
                txtPrice.Clear();
                txtPrice.ForeColor = Color.Black;
            }
        }

        private void txtPrice_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                txtPrice.Text = "Enter Price";
                txtPrice.ForeColor = Color.DarkGray;
            }
        }

        private void txtThreshold_Click(object sender, EventArgs e)
        {
            if (txtThreshold.Text == "Enter Item Threshold")
            {
                txtThreshold.Clear();
                txtThreshold.ForeColor = Color.Black;
            }
        }

        private void txtThreshold_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtThreshold.Text))
            {
                txtThreshold.Text = "Enter Item Threshold";
                txtThreshold.ForeColor = Color.DarkGray;
            }
        }

        private void txtQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
            {
                return;
            }

            // Check if the key is a digit or a decimal point
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                // Display an error message
                MessageBox.Show("Invalid input, please input a quantity.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void txtUnit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
            {
                return;
            }

            // Check if the key is a letter
            if (!char.IsLetter(e.KeyChar))
            {
                // Display an error message
                MessageBox.Show("Invalid input, please input letters only.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Handled = true; // Suppress the key press
            }
        }

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
            {
                return;
            }

            // Check if the key is a digit or a decimal point
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                // Display an error message
                MessageBox.Show("Invalid input, please input a price.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void txtThreshold_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtThreshold_TextChanged(object sender, EventArgs e)
        {
            ValidateQuantityAndThreshold();
        }
    }
}