using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;

namespace IDMS
{
    public partial class ManageSupply_Add_form : Form
    {
        private bool errorShown = false;
        public ManageSupply_Add_form()
        {
            InitializeComponent();
            //txtPrice.KeyPress += new KeyPressEventHandler(txtPrice_KeyPress);
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
            try
            {
                ValidateQuantityAndThreshold();
                Connection.Connection.DB();
                Functions.Functions.query = "SELECT productName FROM product WHERE productName = @productName";
                Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
                Functions.Functions.command.Parameters.AddWithValue("@productName", txtProductName.Text);
                Functions.Functions.reader = Functions.Functions.command.ExecuteReader();

                if (Functions.Functions.reader.HasRows)
                {
                    Functions.Functions.reader.Read();
                    string productName = Functions.Functions.reader["productName"].ToString();

                    if (productName == txtProductName.Text)
                    {
                        MessageBox.Show("This product is already saved in the database! Please enter a new product.", "Invalid!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        clear();
                        Functions.Functions.reader.Close();
                        return;
                    }
                }

                Functions.Functions.reader.Close();
                insertProduct();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error! " + ex.Message);
            }
            finally
            {
                Connection.Connection.con.Close();
            }
        }

        private void insertProduct() 
        {
            string supplierName = "";
            string storeType = "";

            try
            {
                if (rbtnOnline.Checked)
                {
                    supplierName = txtSupplierNameOL.Text;
                    storeType = rbtnOnline.Text;
                }
                else if (rbtnPhysical.Checked)
                {
                    supplierName = txtSupplierNamePy.Text;
                    storeType = rbtnPhysical.Text;
                }

                Connection.Connection.DB();
                string filename = txtFilename.Text;

                string query = "INSERT INTO product(productName, productPrice, product_quantity, productUnit, item_threshold, fileName, store_type, contact_num, supplierName, shopLink, status) " +
                               "VALUES (@productName, @productPrice, @product_quantity, @productUnit, @item_threshold, @fileName, @store_type, @contact_num, @supplierName, @shopLink, 'Available')";

                using (SqlCommand command = new SqlCommand(query, Connection.Connection.con))
                {
                    command.Parameters.AddWithValue("@productName", txtProductName.Text);
                    command.Parameters.AddWithValue("@productPrice", Convert.ToDouble(txtPrice.Text));
                    command.Parameters.AddWithValue("@product_quantity", Convert.ToInt32(txtQuantity.Text));
                    command.Parameters.AddWithValue("@productUnit", txtUnit.Text);
                    command.Parameters.AddWithValue("@item_threshold", Convert.ToInt32(txtThreshold.Text));
                    command.Parameters.AddWithValue("@fileName", filename);
                    command.Parameters.AddWithValue("@store_type", storeType);
                    command.Parameters.AddWithValue("@contact_num", txtPhonNum.Text);
                    command.Parameters.AddWithValue("@supplierName", supplierName);
                    if (txtLink != null)
                    {
                        command.Parameters.AddWithValue("@shopLink", txtLink.Text);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@shopLink", "null");
                    }

                    command.ExecuteNonQuery();
                    MessageBox.Show("The Product is saved into the database", "Saved!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    txtProductName.Clear();
                    this.Hide();
                    ManageSupply_DashboardAdmin dashboard = new ManageSupply_DashboardAdmin();
                    dashboard.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Connection.Connection.con.Close();
            }
        }

        private void clear() 
        {
            txtProductName.Clear();
            txtQuantity.Text = "Enter Quantity";
            txtUnit.Text = "Enter Unit";
            txtThreshold.Text = "Enter Item Threshold";
            txtSupplierNamePy.Clear();
            txtSupplierNameOL.Clear();
            txtFilename.Clear();
            txtLink.Clear();
            txtPhonNum.Clear();
            txtPrice.Clear();
            rbtnOnline.Checked = false;
            rbtnPhysical.Checked = false;
            pcboxProductPhoto.Image = null;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to cancel adding the product?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                this.Hide();
                ManageSupply_DashboardAdmin dashboard = new ManageSupply_DashboardAdmin();
                dashboard.Show();
            }
        }

        private void ManageSupply_Add_form_Load(object sender, EventArgs e)
        {
            fillData();
        }

        public void fillData()
        {
            try
            {
                Connection.Connection.DB();
                Functions.Functions.query = "Select product.productName as [Product Name], product.productPrice as [Price], CONCAT(product_quantity, ' ', productUnit) as [Quantity], product.item_threshold as [Item Threshold], product.status as [Status] from product";
                Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
                SqlDataAdapter adapter = new SqlDataAdapter(Functions.Functions.command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvProducts.DataSource = dt;
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void rbtnOnline_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnOnline.Checked == true)
            {
                pnlOnline.Visible = true;
                pnlPhysical.Visible = false;
                txtSupplierNamePy.Text = " ";
                txtPhonNum.Text = " ";
            }
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
            pcboxProductPhoto.Image = null;
            using (OpenFileDialog openFD = new OpenFileDialog() { Filter = "Image Files (*.jpg; *.jpeg)| *.jpg;*.jpeg", Multiselect = false })
            {
                if (openFD.ShowDialog() == DialogResult.OK)
                {
                    pcboxProductPhoto.Image = Image.FromFile(openFD.FileName);
                    txtFilename.Text = openFD.FileName;
                }
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

        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            ValidateQuantityAndThreshold();
        }

        private void txtThreshold_TextChanged(object sender, EventArgs e)
        {
            ValidateQuantityAndThreshold();
        }

        private void txtPhonNum_KeyPress(object sender, KeyPressEventArgs e)
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
    }
}