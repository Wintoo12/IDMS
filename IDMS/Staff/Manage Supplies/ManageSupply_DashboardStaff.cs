using IDMS.Admin.Manage_Installation;
using IDMS.Staff.Manage_Customer;
using IDMS.Staff.Process_Order.Installations;
using IDMS.Staff.Process_Order.Supplies;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDMS
{
    public partial class ManageSupply_DashboardStaff : Form
    {
        public static int iterationCount = 0;
        bool orderCollapsed, suppliesCollapsed;
        public ManageSupply_DashboardStaff()
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

        public void addProduct()
        {
            Connection.Connection.DB();
            Functions.Functions.query = "Select * from product";
            Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
            Functions.Functions.reader = Functions.Functions.command.ExecuteReader();

            Image stockImage = new Bitmap(@"C:\Users\Mark Andrew\Documents\3rd Year School Stuff\IDMS\UI purposes\Stocks1.png");
            Image buttonImage = new Bitmap(@"C:\Users\Mark Andrew\Documents\3rd Year School Stuff\IDMS\UI purposes\button1.png");

            if (Functions.Functions.reader.HasRows)
            {
                while (Functions.Functions.reader.Read())
                {
                    iterationCount++;

                    int productID = Convert.ToInt32(Functions.Functions.reader["productID"]);

                    Panel pnl = new Panel();
                    //pnl.BackgroundImage = Properties.Resources.Untitled_design__42_;
                    pnl.BackgroundImage = stockImage;
                    pnl.BackgroundImageLayout = ImageLayout.Stretch;
                    pnl.Size = new Size(555, 52);
                    //pnl.Padding = new Padding(5);

                    Label lblProductName = new Label();
                    //lblProductName.Name = Functions.Functions.reader["productID"].ToString();
                    lblProductName.Text = Functions.Functions.reader["productName"].ToString();
                    lblProductName.BackColor = Color.Transparent;
                    lblProductName.ForeColor = Color.White;
                    lblProductName.Font = new Font("Century Gothic", 14, FontStyle.Bold);
                    lblProductName.AutoSize = true;
                    lblProductName.Padding = new Padding(15);

                    Button btnViewProductDetails = new Button();
                    btnViewProductDetails.Click += new EventHandler(this.button_click);
                    btnViewProductDetails.Name = "btnViewDetails" + (iterationCount).ToString();
                    btnViewProductDetails.Text = "View Details";
                    btnViewProductDetails.Tag = productID;
                    btnViewProductDetails.AutoSize = true;
                    btnViewProductDetails.BackColor = Color.Transparent;
                    btnViewProductDetails.BackgroundImageLayout = ImageLayout.Stretch;
                    btnViewProductDetails.BackgroundImage = buttonImage;
                    btnViewProductDetails.ForeColor = Color.FromArgb(115, 160, 254);
                    btnViewProductDetails.FlatStyle = FlatStyle.Flat;
                    btnViewProductDetails.Font = new Font("Century Gothic", 10, FontStyle.Bold);
                    btnViewProductDetails.Size = new Size(67, 28);
                    btnViewProductDetails.Location = new Point(435, 11);

                    pnl.Controls.Add(lblProductName);
                    pnl.Controls.Add(btnViewProductDetails);
                    flowLayoutPanel2.Controls.Add(pnl);
                    flowLayoutPanel2.FlowDirection = FlowDirection.TopDown;

                }
            }
        }

        public void viewProductDetails(int productID)
        {
            Connection.Connection.DB();
            Functions.Functions.query = "SELECT * FROM product WHERE productID = @productID";
            Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
            Functions.Functions.command.Parameters.AddWithValue("@productID", productID);
            Functions.Functions.reader = Functions.Functions.command.ExecuteReader();

            if (Functions.Functions.reader.HasRows)
            {
                Functions.Functions.reader.Read();
                lblProductDetails.Text = Functions.Functions.reader["productName"].ToString();

                string filePath = Functions.Functions.reader["fileName"].ToString(); // Get the file path from the database

                if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath)) // Check if the file path is not empty and the file exists
                {
                    pcboxProductPhoto.Image = Image.FromFile(filePath); // Load image from file path
                    pcboxProductPhoto.SizeMode = PictureBoxSizeMode.StretchImage; // Set size mode
                }
                else
                {
                    pcboxProductPhoto.Image = null; // Clear PictureBox if no photo found
                }

                lblQuantity.Text = Functions.Functions.reader["product_quantity"].ToString();

                // Ensure data is not null or empty
                string productUnit = Functions.Functions.reader["productUnit"].ToString();
                string productPrice = Functions.Functions.reader["productPrice"].ToString();
                string supplierName = Functions.Functions.reader["supplierName"].ToString();

                Console.WriteLine($"Product Unit: {productUnit}");
                Console.WriteLine($"Product Price: {productPrice}");
                Console.WriteLine($"Supplier Name: {supplierName}");

                lblUnit.AutoSize = false;
                lblUnit.MaximumSize = new Size(panel22.Width, 0); // Allow label to grow vertically
                lblUnit.Size = new Size(panel22.Width, 100); // Set an initial height
                lblUnit.Text = productUnit; // Set the text
                lblUnit.BorderStyle = BorderStyle.None; // Optional: visualize the label boundaries
                lblUnit.TextAlign = ContentAlignment.TopLeft; // Align text to top left
                lblUnit.BackColor = Color.Transparent; // Ensure background color is not hiding text
                lblUnit.ForeColor = Color.Black; // Ensure text color is visible

                lblPrice.AutoSize = false;
                lblPrice.MaximumSize = new Size(panel32.Width, 0); // Allow label to grow vertically
                lblPrice.Size = new Size(panel32.Width, 100); // Set an initial height
                lblPrice.Text = productPrice; // Set the text
                lblPrice.BorderStyle = BorderStyle.None; // Optional: visualize the label boundaries
                lblPrice.TextAlign = ContentAlignment.TopLeft; // Align text to top left
                lblPrice.BackColor = Color.Transparent; // Ensure background color is not hiding text
                lblPrice.ForeColor = Color.Black; // Ensure text color is visible

                lblSupplier.AutoSize = false;
                lblSupplier.MaximumSize = new Size(panel33.Width, 0); // Allow label to grow vertically
                lblSupplier.Size = new Size(panel33.Width, 100); // Set an initial height
                lblSupplier.Text = supplierName; // Set the text
                lblSupplier.BorderStyle = BorderStyle.None; // Optional: visualize the label boundaries
                lblSupplier.TextAlign = ContentAlignment.TopLeft; // Align text to top left
                lblSupplier.BackColor = Color.Transparent; // Ensure background color is not hiding text
                lblSupplier.ForeColor = Color.Black; // Ensure text color is visible
            }
            else
            {
                MessageBox.Show("No data found for the selected product.");
            }

            Connection.Connection.con.Close(); // Make sure to close the connection

        }

        private void button_click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            //Console.WriteLine(btn.Name); 
            int productID = Convert.ToInt32(btn.Tag);
            viewProductDetails(productID);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Connection.Connection.DB();
                Functions.Functions.query = "SELECT * FROM product WHERE productName LIKE '%' + @SearchTerm + '%'";
                Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
                Functions.Functions.command.Parameters.AddWithValue("@SearchTerm", txtSearch.Text);
                Functions.Functions.reader = Functions.Functions.command.ExecuteReader();

                Image stockImage = new Bitmap(@"C:\Users\Mark Andrew\Documents\3rd Year School Stuff\IDMS\UI purposes\Stocks1.png");
                Image buttonImage = new Bitmap(@"C:\Users\Mark Andrew\Documents\3rd Year School Stuff\IDMS\UI purposes\button1.png");

                // Clear existing panels
                flowLayoutPanel2.Controls.Clear();

                if (Functions.Functions.reader.HasRows)
                {
                    while (Functions.Functions.reader.Read())
                    {
                        int productID = Convert.ToInt32(Functions.Functions.reader["productID"]);

                        Panel pnl = new Panel();
                        pnl.BackgroundImage = stockImage;
                        pnl.BackgroundImageLayout = ImageLayout.Stretch;
                        pnl.Size = new Size(555, 52);

                        Label lblProductName = new Label();
                        lblProductName.Text = Functions.Functions.reader["productName"].ToString();
                        lblProductName.BackColor = Color.Transparent;
                        lblProductName.ForeColor = Color.White;
                        lblProductName.Font = new Font("Century Gothic", 14, FontStyle.Bold);
                        lblProductName.AutoSize = true;
                        lblProductName.Padding = new Padding(15);

                        Button btnViewProductDetails = new Button();
                        btnViewProductDetails.Click += new EventHandler(this.button_click);
                        btnViewProductDetails.Name = "btnViewDetails" + productID.ToString();
                        btnViewProductDetails.Text = "View Details";
                        btnViewProductDetails.Tag = productID;
                        btnViewProductDetails.AutoSize = true;
                        btnViewProductDetails.BackColor = Color.Transparent;
                        btnViewProductDetails.BackgroundImageLayout = ImageLayout.Stretch;
                        btnViewProductDetails.BackgroundImage = buttonImage;
                        btnViewProductDetails.ForeColor = Color.FromArgb(115, 160, 254);
                        btnViewProductDetails.FlatStyle = FlatStyle.Flat;
                        btnViewProductDetails.Font = new Font("Century Gothic", 10, FontStyle.Bold);
                        btnViewProductDetails.Size = new Size(67, 28);
                        btnViewProductDetails.Location = new Point(435, 11);

                        pnl.Controls.Add(lblProductName);
                        pnl.Controls.Add(btnViewProductDetails);
                        flowLayoutPanel2.Controls.Add(pnl);
                    }
                }
                else
                {
                    MessageBox.Show("No match found!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ManageSupply_DashboardStaff_Load(object sender, EventArgs e)
        {
            addProduct();
        }

        private void orderTimer_Tick(object sender, EventArgs e)
        {
            if (orderCollapsed)
            {
                ordersContainer.Height += 10;
                if (ordersContainer.Height == ordersContainer.MaximumSize.Height)
                {
                    orderCollapsed = false;
                    orderTimer.Stop();
                }
            }
            else
            {
                ordersContainer.Height -= 10;
                if (ordersContainer.Height == ordersContainer.MinimumSize.Height)
                {
                    orderCollapsed = true;
                    orderTimer.Stop();
                    btnOrder.ForeColor = Color.White;
                    btnOrder.BackColor = Color.Transparent;
                    btnOrder.Font = new Font("Century Gothic", 14, FontStyle.Bold);
                }
            }
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            orderTimer.Start();

            btnOrder.ForeColor = Color.Black;
            btnOrder.BackColor = Color.White;
            btnOrder.Font = new Font("Century Gothic", 14, FontStyle.Italic);
        }

        private void btnOrder_Leave(object sender, EventArgs e)
        {
            btnOrder.Font = new Font("Century Gothic", 14, FontStyle.Bold);
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            this.Hide();
            StaffDashboard dashboard = new StaffDashboard();
            dashboard.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            DialogResult = MessageBox.Show("Are you sure you want to logout this account?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (DialogResult == DialogResult.Yes)
            {
                this.Hide();
                Login login = new Login();
                login.Show();
            }
            else
            {
                this.Show();
            }
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageCustomer_Staff manageCustomer_Staff = new ManageCustomer_Staff();
            manageCustomer_Staff.Show();
        }

        private void btnInstallations_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageInstallation_Staff manageInstallation_Staff = new ManageInstallation_Staff();
            manageInstallation_Staff.Show();
        }

        private void btnSupplies_Click(object sender, EventArgs e)
        {
            suppliesTimer.Start();
            btnSupplies.ForeColor = Color.Black;
            btnSupplies.BackColor = Color.White;
            btnSupplies.Font = new Font("Century Gothic", 14, FontStyle.Italic);
        }

        private void btnSupplies_Leave(object sender, EventArgs e)
        {
            btnSupplies.Font = new Font("Century Gothic", 14, FontStyle.Bold);
        }

        private void btnSuppliesOrder_Click(object sender, EventArgs e)
        {
            this.Hide();
            ProcessOrder_Supplies orderSupplies = new ProcessOrder_Supplies();
            orderSupplies.Show();
        }

        private void btnInstallationOrder_Click(object sender, EventArgs e)
        {
            this.Hide();
            Process_Order_Installations orderInstallation = new Process_Order_Installations();
            orderInstallation.Show();
        }

        private void btnVIewSupplies_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageSupply_DashboardStaff manageSupply = new ManageSupply_DashboardStaff();
            manageSupply.Show();
        }

        private void btnViewStatus_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageSupply_ViewStatusListStaff manageSupply = new ManageSupply_ViewStatusListStaff();
            manageSupply.Show();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                // Clear the flowLayoutPanel when the search box is empty
                flowLayoutPanel2.Controls.Clear();
                addProduct();
            }
        }

        private void suppliesTimer_Tick(object sender, EventArgs e)
        {
            if (suppliesCollapsed)
            {
                suppliesContainer.Height += 10;
                if (suppliesContainer.Height == suppliesContainer.MaximumSize.Height)
                {
                    suppliesCollapsed = false;
                    suppliesTimer.Stop();
                }
            }
            else
            {
                suppliesContainer.Height -= 10;
                if (suppliesContainer.Height == suppliesContainer.MinimumSize.Height)
                {
                    suppliesCollapsed = true;
                    suppliesTimer.Stop();
                    btnSupplies.ForeColor = Color.White;
                    btnSupplies.BackColor = Color.Transparent;
                    btnSupplies.Font = new Font("Century Gothic", 14, FontStyle.Bold);
                }
            }
        }


    }
}
