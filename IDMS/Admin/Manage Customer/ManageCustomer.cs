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
using System.Linq.Expressions;
using System.Runtime.Versioning;
using System.IO;
using IDMS.Admin.Generate_Report;
using IDMS.Admin.Manage_Employee;

namespace IDMS.Admin
{
    public partial class ManageCustomer : Form
    {
        public static int iterationCount = 0;
        bool customerCollapsed;
        bool suppliesCollapsed;
        bool employeesCollapsed;
        bool reportsCollapsed;

        public ManageCustomer()
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

        public void viewCustomer() 
        {
            Connection.Connection.DB();
            Functions.Functions.query = "Select * from customer";
            Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
            Functions.Functions.reader = Functions.Functions.command.ExecuteReader();

            Image stockImage = new Bitmap(@"C:\Users\Mark Andrew\Documents\3rd Year School Stuff\IDMS\UI purposes\Stocks1.png");
            Image buttonImage = new Bitmap(@"C:\Users\Mark Andrew\Documents\3rd Year School Stuff\IDMS\UI purposes\button1.png");
            Image updateImage = new Bitmap(@"C:\Users\Mark Andrew\\Documents\3rd Year School Stuff\IDMS\UI purposes\433536375_448308910878597_7653485980491732914_n.png");
            try
            {
                if (Functions.Functions.reader.HasRows)
                {
                    while (Functions.Functions.reader.Read())
                    {
                        iterationCount++;

                        int customerID = Convert.ToInt32(Functions.Functions.reader["customerID"]);
                        
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

                        Label lblCustomerName = new Label();
                        lblCustomerName.Name = Functions.Functions.reader["customerID"].ToString();
                        lblCustomerName.Text = fullname;
                        lblCustomerName.BackColor = Color.Transparent;
                        lblCustomerName.ForeColor = Color.White;
                        lblCustomerName.Font = new Font("Century Gothic", 14, FontStyle.Bold);
                        lblCustomerName.AutoSize = true;
                        lblCustomerName.Padding = new Padding(15);

                        Button btnViewCustomerDetails = new Button();
                        btnViewCustomerDetails.Click += new EventHandler(this.btnViewDetails_click);
                        btnViewCustomerDetails.Name = "btnViewDetails" + (iterationCount).ToString();
                        btnViewCustomerDetails.Text = "View Details";
                        btnViewCustomerDetails.Tag = customerID;
                        btnViewCustomerDetails.AutoSize = true;
                        btnViewCustomerDetails.BackColor = Color.Transparent;
                        btnViewCustomerDetails.BackgroundImageLayout = ImageLayout.Stretch;
                        btnViewCustomerDetails.BackgroundImage = buttonImage;
                        btnViewCustomerDetails.ForeColor = Color.FromArgb(115, 160, 254);
                        btnViewCustomerDetails.FlatAppearance.BorderSize = 0;
                        btnViewCustomerDetails.FlatStyle = FlatStyle.Flat;
                        btnViewCustomerDetails.Font = new Font("Century Gothic", 10, FontStyle.Bold);
                        btnViewCustomerDetails.Size = new Size(67, 28);
                        btnViewCustomerDetails.Location = new Point(440, 11);

                        Button btnUpdate = new Button();
                        btnUpdate.Click += new EventHandler(this.btnUpdate_click);
                        btnUpdate.Name = "btnUpdate" + (iterationCount).ToString();
                        btnUpdate.Text = "Edit Details";
                        btnUpdate.Tag = customerID;
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
                        pnl.Controls.Add(lblCustomerName);
                        pnl.Controls.Add(btnViewCustomerDetails);
                        flowLayoutPanel2.Controls.Add(pnl);
                        flowLayoutPanel2.FlowDirection = FlowDirection.TopDown;
                    }
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void btnViewDetails_click(Object sender, EventArgs e) 
        {
            Button btn = sender as Button;
            int customerID = Convert.ToInt32(btn.Tag);
            viewCustomerDetails(customerID);

            Console.WriteLine("View Details Clicked: " + customerID);

            //Manage_Customer.ManageCustomer_UpdateForm update = new Manage_Customer.ManageCustomer_UpdateForm();
            //update.FillCustomerDetails(customerID);
        }

        public void btnUpdate_click(Object sender, EventArgs e)
        {
            Button btnUpdate = sender as Button;
            int customerID = Convert.ToInt32(btnUpdate.Tag);

            Console.WriteLine("Update Clicked: " + customerID);

            this.Hide();
            Manage_Customer.ManageCustomer_UpdateForm update = new Manage_Customer.ManageCustomer_UpdateForm();
            update.FillCustomerDetails(customerID);

            update.Show();
        }

        public void viewCustomerDetails(int customerID)
        {
            try
            {
                Connection.Connection.DB();
                Functions.Functions.query = "SELECT * FROM customer WHERE customerID = @customerID";
                Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
                Functions.Functions.command.Parameters.AddWithValue("@customerID", customerID);
                Functions.Functions.reader = Functions.Functions.command.ExecuteReader();

                if (Functions.Functions.reader.HasRows)
                {
                    Functions.Functions.reader.Read();
                    string FName = Functions.Functions.reader["FName"].ToString();
                    string MName = Functions.Functions.reader["MName"].ToString();
                    string LName = Functions.Functions.reader["LName"].ToString();
                    string fullname = FName + " " + MName + " " + LName;
                    lblCustomerDetails.Text = fullname;

                    string brgy = Functions.Functions.reader["barangay"].ToString();
                    string municipality = Functions.Functions.reader["municipality"].ToString();
                    string address = brgy + ", " + municipality;

                    lblAddress.Text = address;
                    lblNumber.Text = Functions.Functions.reader["contact_num"].ToString();
                    lblFbName.Text = Functions.Functions.reader["fb_accnt"].ToString();

                    string filePath = Functions.Functions.reader["fileName"].ToString(); // Get the file path from the database

                    if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath)) // Check if the file path is not empty and the file exists
                    {
                        pcboxCustomerPhoto.Image = Image.FromFile(filePath); // Load image from file path
                        pcboxCustomerPhoto.SizeMode = PictureBoxSizeMode.StretchImage; // Set size mode
                    }
                    else
                    {
                        pcboxCustomerPhoto.Image = null; // Clear PictureBox if no photo found
                    }
                }
                else
                {
                    lblCustomerDetails.Text = "Error";
                    pcboxCustomerPhoto.Image = null; // Clear PictureBox in case of error
                    Console.WriteLine("No rows returned from the query."); // Debug output
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddCustomerDetails_Click(object sender, EventArgs e)
        {
            this.Hide();
            Manage_Customer.ManageCustomer_AddForm addCustomer = new Manage_Customer.ManageCustomer_AddForm();
            addCustomer.Show();
        }

        private void ManageCustomer_Load(object sender, EventArgs e)
        {
            btnCustomer.ForeColor = Color.Black;
            btnCustomer.BackColor = Color.White;
            btnCustomer.Font = new Font("Century Gothic", 14, FontStyle.Italic);
            viewCustomer();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin.AdminDashboard adminDashboard = new Admin.AdminDashboard();
            adminDashboard.Show();
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
            ManageSupply_DashboardAdmin supplyDashboard = new ManageSupply_DashboardAdmin();
            supplyDashboard.Show();
        }

        private void btnViewSupp_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageSupply_ViewStatusListAdmin viewList = new ManageSupply_ViewStatusListAdmin();
            viewList.Show();
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

        private void button_click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            //Console.WriteLine(btn.Name); 
            int customerID = Convert.ToInt32(btn.Tag);
            viewCustomerDetails(customerID);
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Connection.Connection.DB();
                Functions.Functions.query = "SELECT * FROM customer WHERE FName LIKE '%' + @SearchTerm + '%' OR LName LIKE '%' + @SearchTerm + '%'";
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
                        int customerID = Convert.ToInt32(Functions.Functions.reader["customerID"]);

                        Panel pnl = new Panel();
                        //pnl.BackgroundImage = Properties.Resources.Untitled_design__42_;
                        pnl.BackgroundImage = stockImage;
                        pnl.BackgroundImageLayout = ImageLayout.Stretch;
                        pnl.Size = new Size(556, 52);
                        //pnl.Padding = new Padding(5);

                        Label lblCustomerName = new Label();
                        //lblProductName.Name = Functions.Functions.reader["productID"].ToString();
                        lblCustomerName.Text = Functions.Functions.reader["FName"].ToString() + " " + Functions.Functions.reader["MName"].ToString() + " " + Functions.Functions.reader["LName"].ToString();
                        lblCustomerName.BackColor = Color.Transparent;
                        lblCustomerName.ForeColor = Color.White;
                        lblCustomerName.Font = new Font("Century Gothic", 14, FontStyle.Bold);
                        lblCustomerName.AutoSize = true;
                        lblCustomerName.Padding = new Padding(15);

                        Button btnViewCustomerDetails = new Button();
                        btnViewCustomerDetails.Click += new EventHandler(this.button_click);
                        btnViewCustomerDetails.Name = "btnViewDetails" + (iterationCount).ToString();
                        btnViewCustomerDetails.Text = "View Details";
                        btnViewCustomerDetails.Tag = customerID;
                        btnViewCustomerDetails.AutoSize = true;
                        btnViewCustomerDetails.BackColor = Color.Transparent;
                        btnViewCustomerDetails.BackgroundImageLayout = ImageLayout.Stretch;
                        btnViewCustomerDetails.BackgroundImage = buttonImage;
                        btnViewCustomerDetails.ForeColor = Color.FromArgb(115, 160, 254);
                        btnViewCustomerDetails.FlatStyle = FlatStyle.Flat;
                        btnViewCustomerDetails.Font = new Font("Century Gothic", 10, FontStyle.Bold);
                        btnViewCustomerDetails.Size = new Size(67, 28);
                        btnViewCustomerDetails.Location = new Point(440, 11);

                        Button btnUpdate = new Button();
                        btnUpdate.Click += new EventHandler(this.btnUpdate_click);
                        btnUpdate.Name = "btnUpdate" + (iterationCount).ToString();
                        btnUpdate.Text = "Edit Details";
                        btnUpdate.Tag = customerID;
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
                        pnl.Controls.Add(lblCustomerName);
                        pnl.Controls.Add(btnViewCustomerDetails);
                        flowLayoutPanel2.Controls.Add(pnl);
                        flowLayoutPanel2.FlowDirection = FlowDirection.TopDown;
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

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                // Clear the flowLayoutPanel when the search box is empty
                flowLayoutPanel2.Controls.Clear();
                viewCustomer();
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
