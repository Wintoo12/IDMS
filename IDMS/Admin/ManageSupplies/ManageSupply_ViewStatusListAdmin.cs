using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IDMS.Functions;
using IDMS.Admin;
using IDMS.Admin.Manage_Employee;
using IDMS.Admin.Generate_Report;

namespace IDMS
{
    public partial class ManageSupply_ViewStatusListAdmin : Form
    {
        bool suppliesCollapsed;
        bool customerCollapsed;
        bool employeesCollapsed;
        bool reportsCollapsed;

        public static int iterationCount = 0;
        public ManageSupply_ViewStatusListAdmin()
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

        public void viewProduct() 
        {
            Connection.Connection.DB();
            Functions.Functions.query = "select * from product order by product_quantity asc";
            Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
            Functions.Functions.reader = Functions.Functions.command.ExecuteReader();

            Image stockImage = new Bitmap(@"C:\Users\Mark Andrew\Documents\3rd Year School Stuff\IDMS\UI purposes\Stocksnew.png");
            Image btnImage = new Bitmap(@"C:\Users\Mark Andrew\Documents\3rd Year School Stuff\IDMS\UI purposes\buttonForStocks.png");
            if (Functions.Functions.reader.HasRows) 
            {
                while (Functions.Functions.reader.Read())
                {
                    iterationCount++;

                    int productID = Convert.ToInt32(Functions.Functions.reader["productID"]);
                    int threshold = Convert.ToInt32(Functions.Functions.reader["item_threshold"]);

                    Panel pnlBack = new Panel();
                    pnlBack.BackgroundImage = stockImage;
                    pnlBack.BackgroundImageLayout = ImageLayout.Stretch;
                    pnlBack.Size = new Size(902, 41);

                    Panel pnlStatusBar = new Panel();
                    pnlStatusBar.BackColor = Color.Transparent;

                    Label lblProductName = new Label();
                    lblProductName.Text = Functions.Functions.reader["productName"].ToString();
                    lblProductName.BackColor = Color.Transparent;
                    lblProductName.ForeColor = Color.FromArgb(120, 165, 255);
                    lblProductName.Font = new Font("Century Gothic", 14, FontStyle.Bold);
                    lblProductName.MaximumSize = new Size(200, 0);
                    lblProductName.AutoSize = true;
                    //lblProductName.Location = new Point(20, 1);
                    lblProductName.Padding = new Padding(10);

                    Label lblProductQuantity = new Label();
                    lblProductQuantity.Text = Functions.Functions.reader["product_quantity"].ToString() + "/" + Convert.ToString(threshold);
                    int stock = Convert.ToInt32(Functions.Functions.reader["product_quantity"]);
                    lblProductQuantity.BackColor = Color.Transparent;
                    lblProductQuantity.ForeColor = Color.Black;
                    lblProductQuantity.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                    lblProductQuantity.AutoSize = true;
                    lblProductQuantity.Location = new Point(230, 2);
                    lblProductQuantity.BringToFront();

                    Button btnRestock = new Button();
                    btnRestock.Click += new EventHandler(this.button_click);
                    btnRestock.Tag = productID;
                    btnRestock.Text = "Restock";
                    btnRestock.AutoSize = true;
                    btnRestock.BackColor = Color.Transparent;
                    btnRestock.ForeColor = Color.White;
                    btnRestock.FlatAppearance.BorderSize = 0;
                    btnRestock.BackgroundImage = btnImage;
                    btnRestock.BackgroundImageLayout = ImageLayout.Stretch;
                    btnRestock.FlatStyle = FlatStyle.Flat;
                    btnRestock.Font = new Font("Century Gothic", 10, FontStyle.Bold);
                    btnRestock.Location = new Point(814, 7);
                    btnRestock.Enabled = false;

                    pnlBack.Controls.Add(lblProductName);
                    pnlBack.Controls.Add(pnlStatusBar);
                    pnlStatusBar.Controls.Add(lblProductQuantity);
                    pnlBack.Controls.Add(btnRestock);
                    flowLayoutPanel2.Controls.Add(pnlBack);
                    flowLayoutPanel2.FlowDirection = FlowDirection.TopDown;

                    colorIndicator(stock, pnlStatusBar, lblProductQuantity, btnRestock, threshold);
                    //colorIndicator(stock, lblProductQuantity, btnRestock);
                }
            }
        }

        public void colorIndicator(int stock, Panel pnlStatusBar, Label lblProductQuantity, Button btnRestock, int threshold)
        {

            if (stock >= 0 && stock <= ((threshold * 0.25) - 1))
            {
                pnlStatusBar.BackColor = Color.FromArgb(239, 1, 1);
                //lblProductQuantity.Text = Convert.ToString(stock) + " " + "/1000";
                btnRestock.Enabled = true;
                pnlStatusBar.Size = new Size(537, 23);
                pnlStatusBar.Location = new Point(215, 9);
            }
            //Orange
            else if (stock >= (threshold * 0.25) && stock <= ((threshold * 0.5) - 1))
            {
                pnlStatusBar.BackColor = Color.FromArgb(239, 179, 1);
                lblProductQuantity.Text = Convert.ToString(stock) + "/" + threshold;
                btnRestock.BackgroundImage = null;
                btnRestock.BackColor = Color.Gray;
                pnlStatusBar.Size = new Size(537, 23);
                pnlStatusBar.Location = new Point(215, 9);
            }
            //Yellow
            else if (stock >= (threshold * 0.5) && stock <= ((threshold * 0.75) - 1))
            {
                pnlStatusBar.BackColor = Color.FromArgb(239, 239, 1);
                lblProductQuantity.Text = Convert.ToString(stock) + "/" + threshold;
                btnRestock.BackColor = Color.Gray;
                btnRestock.BackgroundImage = null;
                pnlStatusBar.Size = new Size(537, 23);
                pnlStatusBar.Location = new Point(215, 9);
            }
            //Teal
            else if (stock >= (threshold * 0.75) && stock <= (threshold - 1))
            {
                pnlStatusBar.BackColor = Color.FromArgb(0, 150, 137);
                lblProductQuantity.Text = Convert.ToString(stock) + "/" + threshold;
                btnRestock.BackColor = Color.Gray;
                btnRestock.BackgroundImage = null;
                pnlStatusBar.Size = new Size(537, 23);
                pnlStatusBar.Location = new Point(215, 9);
            }
            //Green
            else if (stock == threshold)
            {
                pnlStatusBar.BackColor = Color.FromArgb(1, 237, 3);
                lblProductQuantity.Text = Convert.ToString(stock) + "/" + threshold;
                btnRestock.BackColor = Color.Gray;
                btnRestock.BackgroundImage = null;
                pnlStatusBar.Size = new Size(537, 23);
                pnlStatusBar.Location = new Point(215, 9);
            }
        }

        public void button_click(object sender, EventArgs e) 
        {
            Button btn = sender as Button;
            int productID = Convert.ToInt32(btn.Tag);

            ManageSupply_UpdateForm update = new ManageSupply_UpdateForm();
            update.FillSupplyDetails(productID);
            update.Show();
        }

        private void ManageSupply_ViewStatusListAdmin_Load(object sender, EventArgs e)
        {
            viewProduct();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Connection.Connection.DB();
                Functions.Functions.query = "SELECT * FROM product WHERE productName LIKE '%' + @SearchTerm + '%' OR product_quantity LIKE '%' + @SearchTerm + '%' order by product_quantity asc";
                Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
                Functions.Functions.command.Parameters.AddWithValue("@SearchTerm", txtSearch.Text);
                Functions.Functions.reader = Functions.Functions.command.ExecuteReader();

                Image stockImage = new Bitmap(@"C:\Users\Mark Andrew\Documents\3rd Year School Stuff\IDMS\UI purposes\Stocksnew.png");
                Image buttonImage = new Bitmap(@"C:\Users\Mark Andrew\Documents\3rd Year School Stuff\IDMS\UI purposes\button1.png");
                Image btnImage = new Bitmap(@"C:\Users\Mark Andrew\Documents\3rd Year School Stuff\IDMS\UI purposes\buttonForStocks.png");
                // Clear existing panels
                flowLayoutPanel2.Controls.Clear();

                if (Functions.Functions.reader.HasRows)
                {
                    while (Functions.Functions.reader.Read())
                    {
                        int productID = Convert.ToInt32(Functions.Functions.reader["productID"]);
                        int threshold = Convert.ToInt32(Functions.Functions.reader["item_threshold"]);

                        Panel pnlBack = new Panel();
                        pnlBack.BackgroundImage = stockImage;
                        pnlBack.BackgroundImageLayout = ImageLayout.Stretch;
                        pnlBack.Size = new Size(902, 41);

                        Panel pnlStatusBar = new Panel();
                        pnlStatusBar.BackColor = Color.Transparent;

                        Label lblProductName = new Label();
                        lblProductName.Text = Functions.Functions.reader["productName"].ToString();
                        lblProductName.BackColor = Color.Transparent;
                        lblProductName.ForeColor = Color.FromArgb(120, 165, 255);
                        lblProductName.Font = new Font("Century Gothic", 14, FontStyle.Regular);
                        lblProductName.AutoSize = true;
                        //lblProductName.Location = new Point(20, 1);
                        lblProductName.Padding = new Padding(10);

                        Label lblProductQuantity = new Label();
                        lblProductQuantity.Text = Functions.Functions.reader["product_quantity"].ToString() + "/" + Convert.ToString(threshold);
                        int stock = Convert.ToInt32(Functions.Functions.reader["product_quantity"]);
                        lblProductQuantity.BackColor = Color.Transparent;
                        lblProductQuantity.ForeColor = Color.White;
                        lblProductQuantity.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                        lblProductQuantity.AutoSize = true;
                        lblProductQuantity.Location = new Point(230, 2);
                        lblProductQuantity.BringToFront();

                        Button btnRestock = new Button();
                        btnRestock.Click += new EventHandler(this.button_click);
                        btnRestock.Tag = productID;
                        btnRestock.Text = "Restock";
                        btnRestock.AutoSize = true;
                        btnRestock.BackColor = Color.Transparent;
                        btnRestock.ForeColor = Color.White;
                        btnRestock.FlatAppearance.BorderSize = 0;
                        btnRestock.BackgroundImage = btnImage;
                        btnRestock.BackgroundImageLayout = ImageLayout.Stretch;
                        btnRestock.FlatStyle = FlatStyle.Flat;
                        btnRestock.Font = new Font("Century Gothic", 10, FontStyle.Bold);
                        btnRestock.Location = new Point(814, 7);
                        btnRestock.Enabled = false;

                        pnlBack.Controls.Add(lblProductName);
                        pnlBack.Controls.Add(pnlStatusBar);
                        pnlStatusBar.Controls.Add(lblProductQuantity);
                        pnlBack.Controls.Add(btnRestock);
                        flowLayoutPanel2.Controls.Add(pnlBack);
                        flowLayoutPanel2.FlowDirection = FlowDirection.TopDown;

                        colorIndicator(stock, pnlStatusBar, lblProductQuantity, btnRestock, threshold);
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

        private void btnHome_Click(object sender, EventArgs e)
        {
            this.Hide();
            AdminDashboard adminDashboard = new AdminDashboard(); 
            adminDashboard.Show();
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

        private void btnManageSupp_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageSupply_DashboardAdmin manageSupply = new ManageSupply_DashboardAdmin();
            manageSupply.Show();
        }

        private void btnViewSupp_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageSupply_ViewStatusListAdmin viewStatus = new ManageSupply_ViewStatusListAdmin();
            viewStatus.Show();
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
            ManageEmployee manageEmployee = new ManageEmployee();
            manageEmployee.Show();
        }
        private void btnAttendance_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageEmployee_DailyAttendance dailyAttendance = new ManageEmployee_DailyAttendance();
            dailyAttendance.Show();
        }
        private void ReportsTimer_Tick(object sender, EventArgs e)
        {
            if (reportsCollapsed)
            {
                reportsContainer.Height += 10;
                if (reportsContainer.Height == reportsContainer.MaximumSize.Height)
                {
                    reportsCollapsed = false;
                    ReportsTimer.Stop();
                }
            }
            else
            {
                reportsContainer.Height -= 10;
                if (reportsContainer.Height == reportsContainer.MinimumSize.Height)
                {
                    reportsCollapsed = true;
                    ReportsTimer.Stop();
                    btnReports.ForeColor = Color.White;
                    btnReports.BackColor = Color.Transparent;
                    btnReports.Font = new Font("Century Gothic", 14, FontStyle.Bold);
                }
            }
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            ReportsTimer.Start();

            btnReports.ForeColor = Color.Black;
            btnReports.BackColor = Color.White;
            btnReports.Font = new Font("Century Gothic", 14, FontStyle.Italic);
        }

        private void btnReports_Leave(object sender, EventArgs e)
        {
            btnReports.Font = new Font("Century Gothic", 14, FontStyle.Bold);
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

        private void btnInstallations_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageInstallation manageInstallation = new ManageInstallation();
            manageInstallation.Show();
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageCustomer manageCustomer = new ManageCustomer();
            manageCustomer.Show();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                // Clear the flowLayoutPanel when the search box is empty
                flowLayoutPanel2.Controls.Clear();
                viewProduct();
            }
        }
    }
}
