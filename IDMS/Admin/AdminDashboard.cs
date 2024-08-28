using IDMS.Admin.Generate_Report;
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

namespace IDMS.Admin
{
    public partial class AdminDashboard : Form
    {
        bool suppliesCollapsed;
        bool customerCollapsed;
        bool employeesCollapsed;
        bool reportsCollapsed;
        public static int iterationCount;
        private DateTime startDate = DateTime.Now.StartOfWeek(DayOfWeek.Monday); // Assuming the week starts on Monday
        private DateTime endDate = DateTime.Now;

        public AdminDashboard()
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

        private void AdminDashboard_Load(object sender, EventArgs e)
        {
            lblGreetings.Text = "Good Day Admin!";
            btnHome.ForeColor = Color.Black;
            btnHome.BackColor = Color.White;
            btnHome.Font = new Font("Century Gothic", 14, FontStyle.Italic);

            
            restockNeeded();
            fillGFraph(startDate, endDate);
        }

        public void fillGFraph(DateTime startDate, DateTime endDate)
        {
            try
            {
                // Establish the database connection
                Connection.Connection.DB();
                // Define the query
                Functions.Functions.query = "SELECT dateSold, totalPrice FROM sales where dateSold between @startDate AND @endDate";
                Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
                Functions.Functions.command.Parameters.AddWithValue("@startDate", startDate);
                Functions.Functions.command.Parameters.AddWithValue("@endDate", endDate);

                // Create a DataAdapter to execute the query and fill the DataTable
                SqlDataAdapter adapter = new SqlDataAdapter(Functions.Functions.command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // Set the DataSource of the chart
                chart1.DataSource = dt;

                // Ensure the series exists or add a new series

                // Set the X and Y value members for the series
                chart1.Series[0].Name = "Profits";
                chart1.Series["Profits"].XValueMember = "dateSold";
                chart1.Series["Profits"].YValueMembers = "totalPrice";
                chart1.Series["Profits"]["PixelPointWidth"] = "30";

                // Clear any existing titles and add a new title
                chart1.Titles.Clear();
                chart1.Titles.Add("Sales");

                // Refresh the chart to update the display
                chart1.DataBind();
                chart1.Invalidate();
            }
            catch (Exception ex)
            {
                // Handle any errors that may occur
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void btnHome_Leave(object sender, EventArgs e)
        {
            btnHome.ForeColor = Color.White;
            btnHome.BackColor = Color.Transparent;
            btnHome.Font = new Font("Century Gothic", 14, FontStyle.Bold);
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void SuppliesTimer_Tick(object sender, EventArgs e)
        {
            if (suppliesCollapsed)
            {
                suppliesContainer.Height += 10;
                if (suppliesContainer.Height == suppliesContainer.MaximumSize.Height)
                {
                    suppliesCollapsed = false;
                    SuppliesTimer.Stop();
                }
            }
            else
            {
                suppliesContainer.Height -= 10;
                if (suppliesContainer.Height == suppliesContainer.MinimumSize.Height)
                {
                    suppliesCollapsed = true;
                    SuppliesTimer.Stop();
                    btnSupplies.ForeColor = Color.White;
                    btnSupplies.BackColor = Color.Transparent;
                    btnSupplies.Font = new Font("Century Gothic", 14, FontStyle.Bold);
                }
            }
        }

        private void btnSupplies_Click(object sender, EventArgs e)
        {
            SuppliesTimer.Start();
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
            ManageSupply_DashboardAdmin supplyDashboard = new ManageSupply_DashboardAdmin();
            supplyDashboard.Show();
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

        private void btnMaanageCust_Click(object sender, EventArgs e)
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
            Manage_Employee.ManageEmployee_DailyAttendance attendance = new Manage_Employee.ManageEmployee_DailyAttendance();
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

        private void btnReports_Click_1(object sender, EventArgs e)
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


        private void btnSalesReport_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            GenerateReport_Sales_Analytics salesReport = new GenerateReport_Sales_Analytics();
            salesReport.Show();
        }

        private void btnAttendanceReport_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            Generate_Report_Attendance_Analytics attendace = new Generate_Report_Attendance_Analytics();
            attendace.Show();
        }

        public void restockNeeded() 
        {
            Connection.Connection.DB();
            Functions.Functions.query = "Select * from product where product_quantity <= 249 and status = 'Available'";
            Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
            Functions.Functions.reader = Functions.Functions.command.ExecuteReader();

            Image stockImage = new Bitmap(@"C:\Users\Mark Andrew\Documents\3rd Year School Stuff\IDMS\UI purposes\Stocksnew.png");
            Image btnImage = new Bitmap(@"C:\Users\Mark Andrew\Documents\3rd Year School Stuff\IDMS\UI purposes\buttonForStocks.png");
            if (Functions.Functions.reader.HasRows)
            {

                flowLayoutPanel2.FlowDirection = FlowDirection.TopDown;
                flowLayoutPanel2.WrapContents = true;
                flowLayoutPanel2.AutoScroll = true; // Enable auto-scrolling

                flowLayoutPanel2.HorizontalScroll.Enabled = false;
                flowLayoutPanel2.HorizontalScroll.Visible = false;
                flowLayoutPanel2.WrapContents = false;

                while (Functions.Functions.reader.Read())
                {
                    iterationCount++;

                    int productID = Convert.ToInt32(Functions.Functions.reader["productID"]);
                    int threshold = Convert.ToInt32(Functions.Functions.reader["item_threshold"]);

                    Panel pnlBack = new Panel();
                    pnlBack.BackgroundImage = stockImage;
                    pnlBack.BackgroundImageLayout = ImageLayout.Stretch;
                    pnlBack.Size = new Size(922, 37);

                    Panel pnlStatusBar = new Panel();
                    pnlStatusBar.BackColor = Color.Transparent;
                    
                    Label lblProductName = new Label();
                    lblProductName.Text = Functions.Functions.reader["productName"].ToString();
                    lblProductName.BackColor = Color.Transparent;
                    lblProductName.ForeColor = Color.FromArgb(120, 165, 255);
                    lblProductName.Font = new Font("Century Gothic", 10, FontStyle.Regular);
                    lblProductName.AutoSize = true;
                    //lblProductName.Location = new Point(20, 1);
                    lblProductName.Padding = new Padding(10);

                    Label lblProductQuantity = new Label();
                    lblProductQuantity.Text = Functions.Functions.reader["product_quantity"].ToString() + "/" + Convert.ToString(threshold);
                    int stock = Convert.ToInt32(Functions.Functions.reader["product_quantity"]);
                    lblProductQuantity.BackColor = Color.Transparent;
                    lblProductQuantity.ForeColor = Color.White;
                    lblProductQuantity.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                    lblProductQuantity.Location = new Point(300, 1);
                    lblProductQuantity.BringToFront();

                    Button btnRestock = new Button();
                    btnRestock.Click += new EventHandler(this.button_click);
                    btnRestock.Text = "Restock";
                    btnRestock.Tag = productID;
                    btnRestock.Size = new Size(78, 24);
                    btnRestock.BackColor = Color.Transparent;
                    btnRestock.ForeColor = Color.White;
                    btnRestock.FlatAppearance.BorderSize = 0;
                    btnRestock.BackgroundImage = Properties.Resources.Untitled_design__40_;
                    btnRestock.BackgroundImageLayout = ImageLayout.Stretch;
                    btnRestock.FlatStyle = FlatStyle.Flat;
                    btnRestock.Font = new Font("Century Gothic", 9, FontStyle.Bold);
                    btnRestock.Location = new Point(820, 6);
                    btnRestock.Enabled = false;

                    pnlBack.Controls.Add(lblProductName);
                    pnlBack.Controls.Add(pnlStatusBar);
                    pnlStatusBar.Controls.Add(lblProductQuantity);
                    pnlBack.Controls.Add(btnRestock);
                    flowLayoutPanel2.Controls.Add(pnlBack);
                    

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
                        pnlStatusBar.Size = new Size(650, 20);
                        pnlStatusBar.Location = new Point(150, 8);
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

        private void btnViewAllStatus_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageSupply_ViewStatusListAdmin viewAll = new ManageSupply_ViewStatusListAdmin();
            viewAll.Show();
        }
    }
}
