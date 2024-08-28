using IDMS.Admin.Manage_Employee;
using IDMS.Staff.Manage_Customer;
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

namespace IDMS.Admin
{
    public partial class ManageEmployee : Form
    {
        public static int iterationCount;
        bool customerCollapsed;
        bool suppliesCollapsed;
        bool employeesCollapsed;
        bool reportsCollapsed;

        public ManageEmployee()
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

        public void viewEmployee()
        {
            Connection.Connection.DB();
            Functions.Functions.query = "Select * from users where roleID = 2";
            Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
            Functions.Functions.reader = Functions.Functions.command.ExecuteReader();

            Image stockImage = new Bitmap(@"C:\Users\Mark Andrew\Documents\3rd Year School Stuff\IDMS\UI purposes\Stocks1.png");
            Image buttonImage = new Bitmap(@"C:\Users\Mark Andrew\Documents\3rd Year School Stuff\IDMS\UI purposes\button1.png");
            if (Functions.Functions.reader.HasRows)
            {
                while (Functions.Functions.reader.Read())
                {
                    iterationCount++;

                    int staffID = Convert.ToInt32(Functions.Functions.reader["userID"]);

                    string fname = Functions.Functions.reader["FName"].ToString();
                    string mname = Functions.Functions.reader["MName"].ToString();
                    string lname = Functions.Functions.reader["LName"].ToString();
                    string fullname = fname + " " + mname + " " + lname;

                    Panel pnl = new Panel();
                    //pnl.BackgroundImage = Properties.Resources.Untitled_design__42_;
                    pnl.BackgroundImage = stockImage;
                    pnl.BackgroundImageLayout = ImageLayout.Stretch;
                    pnl.Size = new Size(556, 52);
                    //pnl.Padding = new Padding(5);

                    Label lblEmployeeName = new Label();
                    //lblProductName.Name = Functions.Functions.reader["productID"].ToString();
                    lblEmployeeName.Text = fullname;
                    lblEmployeeName.BackColor = Color.Transparent;
                    lblEmployeeName.ForeColor = Color.White;
                    lblEmployeeName.Font = new Font("Century Gothic", 14, FontStyle.Bold);
                    lblEmployeeName.AutoSize = true;
                    lblEmployeeName.Padding = new Padding(15);

                    Button btnViewEmployeeDetails = new Button();
                    btnViewEmployeeDetails.Click += new EventHandler(this.button_click);
                    btnViewEmployeeDetails.Name = "btnViewDetails" + (iterationCount).ToString();
                    btnViewEmployeeDetails.Text = "View Details";
                    btnViewEmployeeDetails.Tag = staffID;
                    btnViewEmployeeDetails.AutoSize = true;
                    btnViewEmployeeDetails.BackColor = Color.Transparent;
                    btnViewEmployeeDetails.BackgroundImageLayout = ImageLayout.Stretch;
                    btnViewEmployeeDetails.BackgroundImage = buttonImage;
                    btnViewEmployeeDetails.ForeColor = Color.FromArgb(115, 160, 254);
                    btnViewEmployeeDetails.FlatStyle = FlatStyle.Flat;
                    btnViewEmployeeDetails.Font = new Font("Century Gothic", 10, FontStyle.Bold);
                    btnViewEmployeeDetails.Size = new Size(67, 28);
                    btnViewEmployeeDetails.Location = new Point(440, 11);

                    Button btnUpdate = new Button();
                    btnUpdate.Click += new EventHandler(this.btnUpdate_click);
                    btnUpdate.Name = "btnUpdate" + (iterationCount).ToString();
                    btnUpdate.Text = "Edit Details";
                    btnUpdate.Tag = staffID;
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
                    pnl.Controls.Add(lblEmployeeName);
                    pnl.Controls.Add(btnViewEmployeeDetails);
                    flowLayoutPanel2.Controls.Add(pnl);
                    flowLayoutPanel2.FlowDirection = FlowDirection.TopDown;

                }
            }
        }

        public void viewEmployeeDetails(int staffID)
        {
            Connection.Connection.DB();
            Functions.Functions.query = "SELECT * FROM users WHERE userID = @staffID";
            Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
            Functions.Functions.command.Parameters.AddWithValue("@staffID", staffID);
            Functions.Functions.reader = Functions.Functions.command.ExecuteReader();

            if (Functions.Functions.reader.HasRows)
            {
                Functions.Functions.reader.Read();

                string fname = Functions.Functions.reader["FName"].ToString();
                string mname = Functions.Functions.reader["MName"].ToString();
                string lname = Functions.Functions.reader["LName"].ToString();
                string fullname = fname + " " + mname + " " + lname;
                string brgy = Functions.Functions.reader["barangay"].ToString();
                string municipality = Functions.Functions.reader["municipality"].ToString();
                string address = brgy + ", " + municipality;

                lblEmployeeName.Text = fullname;

                lblAddress.Text = address;
                lblNumber.Text = Functions.Functions.reader["contact_num"].ToString();
                lblFbName.Text = Functions.Functions.reader["fb_accnt"].ToString();

                string filePath = Functions.Functions.reader["fileName"].ToString(); // Get the file path from the database

                if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath)) // Check if the file path is not empty and the file exists
                {
                    pcboxEmployeePhoto.Image = Image.FromFile(filePath); // Load image from file path
                    pcboxEmployeePhoto.SizeMode = PictureBoxSizeMode.StretchImage; // Set size mode
                }
                else
                {
                    pcboxEmployeePhoto.Image = null; // Clear PictureBox if no photo found
                }
            }
            else
            {
                lblEmployeeName.Text = "Error";
            }
        }


        private void button_click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            //Console.WriteLine(btn.Name); 
            int staffID = Convert.ToInt32(btn.Tag);
            viewEmployeeDetails(staffID);
        }

        private void btnUpdate_click(object sender, EventArgs e) 
        {
            Button btn = sender as Button;
            int staffID = Convert.ToInt32(btn.Tag);

            this.Hide();
            Manage_Employee.ManageEmployee_UpdateForm update = new Manage_Employee.ManageEmployee_UpdateForm();
            update.FillEmployeeDetails(staffID);
            update.Show();
        }

        private void ManageEmployee_Load(object sender, EventArgs e)
        {
            employeeTimer.Start();
            employeesCollapsed = true;
            btnEmployees.ForeColor = Color.Black;
            btnEmployees.BackColor = Color.White;
            btnEmployees.Font = new Font("Century Gothic", 14, FontStyle.Italic);
            viewEmployee();
        }

        private void btnAddEmployee_Click(object sender, EventArgs e)
        {
            this.Hide();
            AddEmployee addEmployee = new AddEmployee();
            addEmployee.Show();
        }

        private void pcboxProductPhoto_Click(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Connection.Connection.DB();
                Functions.Functions.query = "SELECT * FROM users WHERE (FName LIKE '%' + @SearchTerm + '%' OR LName LIKE '%' + @SearchTerm + '%') AND roleID = 2";
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
                        int userID = Convert.ToInt32(Functions.Functions.reader["userID"]);

                        Panel pnl = new Panel();
                        //pnl.BackgroundImage = Properties.Resources.Untitled_design__42_;
                        pnl.BackgroundImage = stockImage;
                        pnl.BackgroundImageLayout = ImageLayout.Stretch;
                        pnl.Size = new Size(556, 52);
                        //pnl.Padding = new Padding(5);

                        Label lblEmployeeName = new Label();
                        //lblProductName.Name = Functions.Functions.reader["productID"].ToString();
                        lblEmployeeName.Text = Functions.Functions.reader["FName"].ToString() + " " + Functions.Functions.reader["MName"].ToString() + " " + Functions.Functions.reader["LName"].ToString();
                        lblEmployeeName.BackColor = Color.Transparent;
                        lblEmployeeName.ForeColor = Color.White;
                        lblEmployeeName.Font = new Font("Century Gothic", 14, FontStyle.Bold);
                        lblEmployeeName.AutoSize = true;
                        lblEmployeeName.Padding = new Padding(15);

                        Button btnViewEmployeeDetails = new Button();
                        btnViewEmployeeDetails.Click += new EventHandler(this.button_click);
                        btnViewEmployeeDetails.Name = "btnViewDetails" + (iterationCount).ToString();
                        btnViewEmployeeDetails.Text = "View Details";
                        btnViewEmployeeDetails.Tag = userID;
                        btnViewEmployeeDetails.AutoSize = true;
                        btnViewEmployeeDetails.BackColor = Color.Transparent;
                        btnViewEmployeeDetails.BackgroundImageLayout = ImageLayout.Stretch;
                        btnViewEmployeeDetails.BackgroundImage = buttonImage;
                        btnViewEmployeeDetails.ForeColor = Color.FromArgb(115, 160, 254);
                        btnViewEmployeeDetails.FlatStyle = FlatStyle.Flat;
                        btnViewEmployeeDetails.Font = new Font("Century Gothic", 10, FontStyle.Bold);
                        btnViewEmployeeDetails.Size = new Size(67, 28);
                        btnViewEmployeeDetails.Location = new Point(440, 11);

                        Button btnUpdate = new Button();
                        btnUpdate.Click += new EventHandler(this.btnUpdate_click);
                        btnUpdate.Name = "btnUpdate" + (iterationCount).ToString();
                        btnUpdate.Text = "Edit Details";
                        btnUpdate.Tag = userID;
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
                        pnl.Controls.Add(lblEmployeeName);
                        pnl.Controls.Add(btnViewEmployeeDetails);
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

        private void suppliesTimer_Tick(object sender, EventArgs e)
        {
            if (suppliesCollapsed)
            {
                suppliesContainer.Height += 10;
                if (suppliesContainer.Height == suppliesContainer.MaximumSize.Height)
                {
                    suppliesCollapsed = false;
                    suppliesTimer.Stop();
                    btnSupplies.ForeColor = Color.White;
                    btnSupplies.BackColor = Color.Transparent;
                    btnSupplies.Font = new Font("Century Gothic", 14, FontStyle.Bold);
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
            ManageSupply_DashboardAdmin admin = new ManageSupply_DashboardAdmin();
            admin.Show();
        }

        private void btnViewSupp_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageSupply_ViewStatusListAdmin viewStatus = new ManageSupply_ViewStatusListAdmin();
            viewStatus.Show();
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

        private void employeeTimer_Tick(object sender, EventArgs e)
        {
            if (employeesCollapsed)
            {
                employeeContainer.Height += 10;
                if (employeeContainer.Height == employeeContainer.MaximumSize.Height)
                {
                    employeesCollapsed = false;
                    employeeTimer.Stop();
                }
            }
            else
            {
                employeeContainer.Height -= 10;
                if (employeeContainer.Height == employeeContainer.MinimumSize.Height)
                {
                    employeesCollapsed = true;
                    employeeTimer.Stop();
                    btnEmployees.ForeColor = Color.White;
                    btnEmployees.BackColor = Color.Transparent;
                    btnEmployees.Font = new Font("Century Gothic", 14, FontStyle.Bold);
                }
            }
        }

        private void btnEmployees_Click(object sender, EventArgs e)
        {
            employeeTimer.Start();
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
            AdminDashboard adminDashboard = new AdminDashboard();
            adminDashboard.Show();
        }

        private void btnAttendanceReport_Click(object sender, EventArgs e)
        {
            this.Hide();
            Generate_Report.Generate_Report_Attendance_Analytics attendance_Analytics = new Generate_Report.Generate_Report_Attendance_Analytics();
            attendance_Analytics.Show();
        }

        private void btnSalesReport_Click(object sender, EventArgs e)
        {
            this.Hide();
            Generate_Report.GenerateReport_Sales_Analytics sales_Analytics = new Generate_Report.GenerateReport_Sales_Analytics();
            sales_Analytics.Show();
        }

        private void btnInstallations_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageInstallation manageInstallation = new ManageInstallation();
            manageInstallation.Show();
        }

        private void btnLogout_Click_1(object sender, EventArgs e)
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

        private void btnAttendance_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageEmployee_DailyAttendance attendance = new ManageEmployee_DailyAttendance();
            attendance.Show();
        }
    }
}
