using IDMS.Admin.Generate_Report;
using IDMS.Admin.Manage_Employee;
using IDMS.Admin.Manage_Installation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace IDMS.Admin
{
    public partial class ManageInstallation : Form
    {
        public static int iterationCount = 0;
        bool suppliesCollapsed;
        bool employeesCollapsed;
        bool customerCollapsed;
        bool reportsCollapsed;

        
        public ManageInstallation()
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
            this.Hide();
            Login login = new Login();
            login.Show();
        }

        private void ManageInstallation_Load(object sender, EventArgs e)
        {
            btnInstallations.ForeColor = Color.Black;
            btnInstallations.BackColor = Color.White;
            btnInstallations.Font = new Font("Century Gothic", 14, FontStyle.Italic);

            viewPackage();
        }

        private void btnInstallations_Click(object sender, EventArgs e)
        {
            Admin.ManageInstallation manageInstallation = new Admin.ManageInstallation();
            manageInstallation.Show();
        }

        private void btnAddForm_Click(object sender, EventArgs e)
        {
            this.Hide();
            Manage_Installation.ManageInstallation_AddForm addForm = new Manage_Installation.ManageInstallation_AddForm();
            addForm.Show();
        }

        public void viewPackage()
        {
            Connection.Connection.DB();
            Functions.Functions.query = "Select * from package";
            Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
            Functions.Functions.reader = Functions.Functions.command.ExecuteReader();

            Image stockImage = new Bitmap(@"C:\Users\Mark Andrew\Documents\3rd Year School Stuff\IDMS\UI purposes\Stocks1.png");
            Image buttonImage = new Bitmap(@"C:\Users\Mark Andrew\Documents\3rd Year School Stuff\IDMS\UI purposes\button1.png");

            if (Functions.Functions.reader.HasRows)
            {
                while (Functions.Functions.reader.Read())
                {
                    iterationCount++;

                    int packageID = Convert.ToInt32(Functions.Functions.reader["packageID"]);

                    Panel pnl = new Panel();
                    //pnl.BackgroundImage = Properties.Resources.Untitled_design__42_;
                    pnl.BackgroundImage = stockImage;
                    pnl.BackgroundImageLayout = ImageLayout.Stretch;
                    pnl.Size = new Size(954, 52);
                    //pnl.Padding = new Padding(5);

                    Label lblProductName = new Label();
                    //lblProductName.Name = Functions.Functions.reader["productID"].ToString();
                    lblProductName.Text = Functions.Functions.reader["packageName"].ToString();
                    lblProductName.BackColor = Color.Transparent;
                    lblProductName.ForeColor = Color.White;
                    lblProductName.Font = new Font("Century Gothic", 14, FontStyle.Bold);
                    lblProductName.AutoSize = true;
                    lblProductName.Padding = new Padding(15);

                    Button btnViewProductDetails = new Button();
                    btnViewProductDetails.Click += new EventHandler(this.button_click);
                    btnViewProductDetails.Name = "btnViewDetails" + (iterationCount).ToString();
                    btnViewProductDetails.Text = "View Details";
                    btnViewProductDetails.Tag = packageID;
                    btnViewProductDetails.AutoSize = true;
                    btnViewProductDetails.BackColor = Color.Transparent;
                    btnViewProductDetails.BackgroundImageLayout = ImageLayout.Stretch;
                    btnViewProductDetails.BackgroundImage = buttonImage;
                    btnViewProductDetails.ForeColor = Color.FromArgb(115, 160, 254);
                    btnViewProductDetails.FlatStyle = FlatStyle.Flat;
                    btnViewProductDetails.Font = new Font("Century Gothic", 10, FontStyle.Bold);
                    btnViewProductDetails.Size = new Size(67, 28);
                    btnViewProductDetails.Location = new Point(831, 12);

                    pnl.Controls.Add(lblProductName);
                    pnl.Controls.Add(btnViewProductDetails);
                    //pnl.Controls.Add(btnUpdate);
                    flowLayoutPanel2.Controls.Add(pnl);
                    flowLayoutPanel2.FlowDirection = FlowDirection.TopDown;

                }
            }
        }

        private void button_click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            Console.WriteLine(btn.Name);
            int productID = Convert.ToInt32(btn.Tag);

            this.Hide();
            ManageInstallation_ViewDetails installation = new ManageInstallation_ViewDetails();
            installation.FillInstallationDetails(productID);
            installation.Show();
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

        private void btnSupplies_Click_1(object sender, EventArgs e)
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

        private void btnEmployees_Click_1(object sender, EventArgs e)
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

        private void btnHome_Click(object sender, EventArgs e)
        {
            this.Hide();
            AdminDashboard dashboard = new AdminDashboard(); 
            dashboard.Show();
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
                viewPackage();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Connection.Connection.DB();
                Functions.Functions.query = "SELECT * FROM package WHERE packageName LIKE '%' + @SearchTerm + '%' order by packageName asc";
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
                        int packageID = Convert.ToInt32(Functions.Functions.reader["packageID"]);

                        Panel pnl = new Panel();
                        //pnl.BackgroundImage = Properties.Resources.Untitled_design__42_;
                        pnl.BackgroundImage = stockImage;
                        pnl.BackgroundImageLayout = ImageLayout.Stretch;
                        pnl.Size = new Size(954, 52);
                        //pnl.Padding = new Padding(5);

                        Label lblProductName = new Label();
                        //lblProductName.Name = Functions.Functions.reader["productID"].ToString();
                        lblProductName.Text = Functions.Functions.reader["packageName"].ToString();
                        lblProductName.BackColor = Color.Transparent;
                        lblProductName.ForeColor = Color.White;
                        lblProductName.Font = new Font("Century Gothic", 14, FontStyle.Bold);
                        lblProductName.AutoSize = true;
                        lblProductName.Padding = new Padding(15);

                        Button btnViewProductDetails = new Button();
                        btnViewProductDetails.Click += new EventHandler(this.button_click);
                        btnViewProductDetails.Name = "btnViewDetails" + (iterationCount).ToString();
                        btnViewProductDetails.Text = "View Details";
                        btnViewProductDetails.Tag = packageID;
                        btnViewProductDetails.AutoSize = true;
                        btnViewProductDetails.BackColor = Color.Transparent;
                        btnViewProductDetails.BackgroundImageLayout = ImageLayout.Stretch;
                        btnViewProductDetails.BackgroundImage = buttonImage;
                        btnViewProductDetails.ForeColor = Color.FromArgb(115, 160, 254);
                        btnViewProductDetails.FlatStyle = FlatStyle.Flat;
                        btnViewProductDetails.Font = new Font("Century Gothic", 10, FontStyle.Bold);
                        btnViewProductDetails.Size = new Size(67, 28);
                        btnViewProductDetails.Location = new Point(831, 12);

                        pnl.Controls.Add(lblProductName);
                        pnl.Controls.Add(btnViewProductDetails);
                        //pnl.Controls.Add(btnUpdate);
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
    }
    
}
