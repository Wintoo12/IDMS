using IDMS.Admin;
using IDMS.Admin.Manage_Employee;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Remoting.Channels;
using System.IO;
using IDMS.Admin.Generate_Report;

namespace IDMS
{
    
    public partial class ManageSupply_DashboardAdmin : Form
    {
        bool suppliesCollapsed;
        bool customerCollapsed;
        bool employeesCollapsed;
        bool reportsCollapsed;
        public static int iterationCount = 0;

        public ManageSupply_DashboardAdmin()
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

        private void ManageSupply_DashboardAdmin_Load(object sender, EventArgs e)
        {
            suppliesTimer.Start();
            suppliesCollapsed = true;
            btnManageSupp.ForeColor = Color.Black;
            btnManageSupp.BackColor = Color.White;
            btnSupplies.BackColor = Color.DarkGray;
            btnManageSupp.Font = new Font("Century Gothic", 14, FontStyle.Italic);

            addProduct();
        }

        private void btnManageSupp_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void btnViewSupp_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageSupply_ViewStatusListAdmin viewList = new ManageSupply_ViewStatusListAdmin();
            viewList.Show();
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            btnCustomer.ForeColor = Color.Black;
            btnCustomer.BackColor = Color.White;
            btnCustomer.Font = new Font("Century Gothic", 14, FontStyle.Italic);

            this.Hide();
            ManageCustomer manageCustomer = new ManageCustomer();
            manageCustomer.Show();
        }

        private void btnCustomer_Leave(object sender, EventArgs e)
        {
            btnCustomer.Font = new Font("Century Gothic", 14, FontStyle.Bold);
        }

        private void btnManageCust_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin.ManageCustomer customer = new Admin.ManageCustomer();
            customer.Show();
        }

        private void btnInstallations_Click(object sender, EventArgs e)
        {
            btnInstallations.ForeColor = Color.Black;
            btnInstallations.BackColor = Color.White;
            btnInstallations.Font = new Font("Century Gothic", 14, FontStyle.Italic);

            this.Hide();
            Admin.ManageInstallation installation = new Admin.ManageInstallation();
            installation.Show();
        }

        private void employeesTimer_Tick(object sender, EventArgs e)
        {
            if (employeesCollapsed)
            {
                employeeContainer.Height += 10;
                if (employeeContainer.Height == employeeContainer.MaximumSize.Height)
                {
                    employeesCollapsed = false;
                    employeesTimer.Stop();
                }
            }
            else
            {
                employeeContainer.Height -= 10;
                if (employeeContainer.Height == employeeContainer.MinimumSize.Height)
                {
                    employeesCollapsed = true;
                    employeesTimer.Stop();
                    btnEmployees.ForeColor = Color.White;
                    btnEmployees.BackColor = Color.Transparent;
                    btnEmployees.Font = new Font("Century Gothic", 14, FontStyle.Bold);
                }
            }
        }

        private void btnEmployees_Click(object sender, EventArgs e)
        {
            employeesTimer.Start();

            btnEmployees.ForeColor = Color.Black;
            btnEmployees.BackColor = Color.White;
            btnEmployees.Font = new Font("Century Gothic", 14, FontStyle.Italic);
        }

        private void btnEmployees_Leave(object sender, EventArgs e)
        {
            btnEmployees.Font = new Font("Century Gothic", 14, FontStyle.Bold);
        }

        private void btnManageEmployee_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageEmployee employee = new ManageEmployee();
            employee.Show();
        }

        private void btnAttendance_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageEmployee_DailyAttendance attendance = new ManageEmployee_DailyAttendance();
            attendance.Show();
        }

        private void reportsTimer_Tick(object sender, EventArgs e)
        {
            if (reportsCollapsed)
            {
                reportsContainer.Height += 10;
                if (reportsContainer.Height == reportsContainer.MaximumSize.Height)
                {
                    reportsCollapsed = false;
                    reportsTimer.Stop();
                }
            }
            else
            {
                reportsContainer.Height -= 10;
                if (reportsContainer.Height == reportsContainer.MinimumSize.Height)
                {
                    reportsCollapsed = true;
                    reportsTimer.Stop();
                    btnReports.ForeColor = Color.White;
                    btnReports.BackColor = Color.Transparent;
                    btnReports.Font = new Font("Century Gothic", 14, FontStyle.Bold);
                }
            }
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            reportsTimer.Start();

            btnReports.ForeColor = Color.Black;
            btnReports.BackColor = Color.White;
            btnReports.Font = new Font("Century Gothic", 14, FontStyle.Italic);
        }

        private void btnReports_Leave(object sender, EventArgs e)
        {
            btnReports.Font = new Font("Century Gothic", 14, FontStyle.Bold);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
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
        

        public void addProduct() 
        {
            Connection.Connection.DB();
            Functions.Functions.query = "Select * from product order by productName asc";
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
                    btnViewProductDetails.Location = new Point(440, 11);

                    Button btnUpdate = new Button();
                    btnUpdate.Click += new EventHandler(this.btnUpdate_click);
                    btnUpdate.Name = "btnUpdate" + (iterationCount).ToString();
                    btnUpdate.Text = "Edit Details";
                    btnUpdate.Tag = productID;
                    btnUpdate.AutoSize = true;
                    btnUpdate.BackColor = Color.Transparent;
                    btnUpdate.BackgroundImageLayout = ImageLayout.Stretch;
                    btnUpdate.BackgroundImage = buttonImage;
                    btnUpdate.ForeColor = Color.FromArgb(115, 160, 254);
                    btnUpdate.FlatStyle = FlatStyle.Flat;
                    btnUpdate.FlatAppearance.BorderSize = 0;
                    btnUpdate.Size = new Size(67, 28);
                    btnUpdate.Font = new Font("Century Gothic", 10, FontStyle.Bold);
                    //btnUpdate.Visible = false;
                    btnUpdate.Location = new Point(340, 11);

                    pnl.Controls.Add(btnUpdate);
                    pnl.Controls.Add(lblProductName);
                    pnl.Controls.Add(btnViewProductDetails);
                    flowLayoutPanel2.Controls.Add(pnl);
                    flowLayoutPanel2.FlowDirection = FlowDirection.TopDown;
                    
                }
            }  
        }

        private void button_click(object sender, EventArgs e) 
        {
            Button btn = sender as Button;
            //Console.WriteLine(btn.Name); 
            int productID = Convert.ToInt32(btn.Tag);
            viewProductDetails(productID);
        }

        public void btnUpdate_click(object sender, EventArgs e) 
        {
            Button btn = sender as Button;
            int productID = Convert.ToInt32(btn.Tag);

            this.Hide();
            ManageSupply_UpdateForm update = new ManageSupply_UpdateForm();
            update.FillSupplyDetails(productID);

            update.Show();
        }

        private void btnAddSupplies_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageSupply_Add_form addSupply = new ManageSupply_Add_form();
            addSupply.Show();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin.AdminDashboard adminDashboard = new Admin.AdminDashboard();
            adminDashboard.Show();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Connection.Connection.DB();
                Functions.Functions.query = "SELECT * FROM product WHERE productName LIKE '%' + @SearchTerm + '%' order by productName asc";
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
                        //pnl.BackgroundImage = Properties.Resources.Untitled_design__42_;
                        pnl.BackgroundImage = stockImage;
                        pnl.BackgroundImageLayout = ImageLayout.Stretch;
                        pnl.Size = new Size(556, 52);
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
                        btnViewProductDetails.Location = new Point(440, 11);

                        Button btnUpdate = new Button();
                        btnUpdate.Click += new EventHandler(this.btnUpdate_click);
                        btnUpdate.Name = "btnUpdate" + (iterationCount).ToString();
                        btnUpdate.Text = "Edit Details";
                        btnUpdate.Tag = productID;
                        btnUpdate.AutoSize = true;
                        btnUpdate.BackColor = Color.Transparent;
                        btnUpdate.BackgroundImageLayout = ImageLayout.Stretch;
                        btnUpdate.BackgroundImage = buttonImage;
                        btnUpdate.ForeColor = Color.FromArgb(115, 160, 254);
                        btnUpdate.FlatStyle = FlatStyle.Flat;
                        btnUpdate.FlatAppearance.BorderSize = 0;
                        btnUpdate.Size = new Size(67, 28);
                        btnUpdate.Font = new Font("Century Gothic", 10, FontStyle.Bold);
                        //btnUpdate.Visible = false;
                        btnUpdate.Location = new Point(340, 11);

                        pnl.Controls.Add(btnUpdate);
                        pnl.Controls.Add(lblProductName);
                        pnl.Controls.Add(btnViewProductDetails);
                        flowLayoutPanel2.Controls.Add(pnl);
                        flowLayoutPanel2.FlowDirection = FlowDirection.TopDown;
                    }
                }
                else
                {
                    MessageBox.Show("No matching products found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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

        private void btnSalesReport_Click(object sender, EventArgs e)
        {
            this.Hide();
            GenerateReport_Sales_Analytics sales_Analytics = new GenerateReport_Sales_Analytics();
            sales_Analytics.Show();
        }

        private void btnAttendanceReport_Click(object sender, EventArgs e)
        {
            this.Hide();
            Generate_Report_Attendance_Analytics attendance_Analytics = new Generate_Report_Attendance_Analytics();
            attendance_Analytics.Show();
        }
    }
}
