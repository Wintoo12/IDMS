using IDMS.Admin.Manage_Employee;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDMS.Admin.Generate_Report
{
    public partial class Generate_Report_Attendance_Analytics : Form
    {
        bool suppliesCollapsed;
        bool customersCollapsed;
        bool employeesCollapsed;
        bool reportsCollapsed;
        private DateTime startDate;
        private DateTime endDate;
        private bool isSearchPerformed = false;

        public Generate_Report_Attendance_Analytics()
        {
            InitializeComponent();
            cmbDay.SelectedIndexChanged += new EventHandler(cmbDay_SelectedIndexChanged);
            cmbStatus.SelectedIndexChanged += new EventHandler(cmbStatus_SelectedIndexChanged);

            cmbDay.Items.Clear();
            cmbDay.Items.Add("This Week");
            cmbDay.Items.Add("This Month");
            cmbDay.Items.Add("Last 3 Months");
            cmbDay.SelectedIndex = 0; // Set default selection

            cmbStatus.Items.Clear();
            cmbStatus.Items.Add("Present");
            cmbStatus.Items.Add("Excused");
            cmbStatus.Items.Add("Late");
            cmbStatus.Items.Add("Absent");
            cmbStatus.SelectedIndex = 0;
            
            // Initialize date range for the default selection
            InitializeDateRange();
            Generate_Report_Attendance_Analytics_Load(this, EventArgs.Empty);

            suggestTextBox();
        }

        private void suggestTextBox() 
        {
            try
            {
                Connection.Connection.DB();
                Functions.Functions.query = "Select FName + ' ' + LName as [Full Name] from users where FName like '%" + txtSearch.Text + "%' or LName like '%" + txtSearch.Text + "%' and roleID = 2";
                Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
                Functions.Functions.reader = Functions.Functions.command.ExecuteReader();
                AutoCompleteStringCollection name = new AutoCompleteStringCollection();

                while (Functions.Functions.reader.Read())
                {
                    name.Add(Functions.Functions.reader.GetString(0));
                }

                txtSearch.AutoCompleteCustomSource = name;
                Functions.Functions.reader.Close();
                Connection.Connection.con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (Functions.Functions.reader != null && !Functions.Functions.reader.IsClosed)
                {
                    Functions.Functions.reader.Close();
                }
                if (Connection.Connection.con != null && Connection.Connection.con.State == ConnectionState.Open)
                {
                    Connection.Connection.con.Close();
                }
            }
        }

        private void cmbDay_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitializeDateRange();
            UpdateAttendanceSummaryForEmployee();
        }

        private void InitializeDateRange()
        {
            string selectedValue = cmbDay.SelectedItem.ToString();

            if (selectedValue == "This Week")
            {
                startDate = DateTime.Now.StartOfWeek(DayOfWeek.Monday); // Assuming the week starts on Monday
                endDate = DateTime.Now;
            }
            else if (selectedValue == "This Month")
            {
                startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                endDate = DateTime.Now;
            }
            else if (selectedValue == "Last 3 Months")
            {
                startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-3);
                endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
            }
            else
            {
                MessageBox.Show("No date found!");
            }
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

        private void UpdateAttendanceSummaryForEmployee()
        {

            if (!isSearchPerformed)
            {
                return;
            }

            string employeeName = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(employeeName))
            {
                MessageBox.Show("Please enter an employee name.");
                return;
            }

            try
            {
                // Open the database connection
                Connection.Connection.DB();

                // Query to get the attendance summary for the specified employee within the specified date range
                string query = @"
                SELECT 
                    SUM(CASE WHEN attendance.status = 'Present' THEN 1 ELSE 0 END) AS Present,
                    SUM(CASE WHEN attendance.status = 'Absent' THEN 1 ELSE 0 END) AS Absent,
                    SUM(CASE WHEN attendance.status = 'Late' THEN 1 ELSE 0 END) AS Late,
                    SUM(CASE WHEN attendance.status = 'Excused' THEN 1 ELSE 0 END) AS Excused
                FROM 
                    attendance 
                INNER JOIN 
                    users ON users.userID = attendance.userID 
                WHERE 
                    (users.FName + ' ' + users.LName LIKE @employeeName OR users.LName + ' ' + users.FName LIKE @employeeName)
                    AND attendance.date BETWEEN @startDate AND @endDate";

                using (SqlCommand command = new SqlCommand(query, Connection.Connection.con))
                {
                    command.Parameters.AddWithValue("@employeeName", "%" + employeeName + "%");
                    command.Parameters.AddWithValue("@startDate", startDate);
                    command.Parameters.AddWithValue("@endDate", endDate);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();

                            lblPresent.Text = reader["Present"].ToString();
                            lblLate.Text = reader["Late"].ToString();
                            lblExcused.Text = reader["Excused"].ToString();
                            lblAbsent.Text = reader["Absent"].ToString();
                        }
                        else 
                        {
                            lblPresent.Text = "0";
                            lblLate.Text = "0";
                            lblExcused.Text = "0";
                            lblAbsent.Text = "0";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                // Close the connection
                if (Connection.Connection.con != null && Connection.Connection.con.State == ConnectionState.Open)
                {
                    Connection.Connection.con.Close();
                }
            }
        }

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            initializeChart();
        }

        public void fillGraphPresent(DateTime startDate, DateTime endDate) 
        {
            try 
            {
                Connection.Connection.DB();
                Functions.Functions.query = @"SELECT
                    users.LName as LastName, SUM(CASE WHEN attendance.status = 'Present' THEN 1 ELSE 0 END) AS Present
                FROM
                    attendance
                INNER JOIN 
                    users ON users.userID = attendance.userID
                WHERE
                    date between @startDate AND @endDate
                GROUP BY
                   users.LName";
                Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
                Functions.Functions.command.Parameters.AddWithValue("@startDate", startDate);
                Functions.Functions.command.Parameters.AddWithValue("@endDate", endDate);

                SqlDataAdapter adapter = new SqlDataAdapter(Functions.Functions.command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                chart1.DataSource = dt;

                chart1.Series[0].Name = "Present";
                chart1.Series["Present"].XValueMember = "LastName";
                chart1.Series["Present"].YValueMembers = "Present";
                chart1.Series["Present"]["PixelPointWidth"] = "30";

                chart1.Titles.Clear();
                chart1.Titles.Add("Attendance");

                chart1.DataBind();
                chart1.Invalidate();
            }
            catch (Exception ex)
            {
                // Handle any errors that may occur
                MessageBox.Show("An error occurred: " + ex.Message);
            }

        }

        public void fillGraphExcused(DateTime startDate, DateTime endDate)
        {
            try
            {
                Connection.Connection.DB();
                Functions.Functions.query = @"SELECT
                    users.LName as LastName, SUM(CASE WHEN attendance.status = 'Excused' THEN 1 ELSE 0 END) AS Excused
                FROM
                    attendance
                INNER JOIN 
                    users ON users.userID = attendance.userID
                WHERE
                    date between @startDate AND @endDate
                GROUP BY
                   users.LName";
                Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
                Functions.Functions.command.Parameters.AddWithValue("@startDate", startDate);
                Functions.Functions.command.Parameters.AddWithValue("@endDate", endDate);

                SqlDataAdapter adapter = new SqlDataAdapter(Functions.Functions.command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                chart1.DataSource = dt;

                chart1.Series[0].Name = "Excused";
                chart1.Series["Excused"].XValueMember = "LastName";
                chart1.Series["Excused"].YValueMembers = "Excused";
                chart1.Series["Excused"]["PixelPointWidth"] = "30";

                chart1.Titles.Clear();
                chart1.Titles.Add("Attendance");

                chart1.DataBind();
                chart1.Invalidate();
            }
            catch (Exception ex)
            {
                // Handle any errors that may occur
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        public void fillGraphLate(DateTime startDate, DateTime endDate)
        {
            try
            {
                Connection.Connection.DB();
                Functions.Functions.query = @"SELECT
                    users.LName as LastName, SUM(CASE WHEN attendance.status = 'Late' THEN 1 ELSE 0 END) AS Late
                FROM
                    attendance
                INNER JOIN 
                    users ON users.userID = attendance.userID
                WHERE
                    date between @startDate AND @endDate
                GROUP BY
                    users.LName";
                Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
                Functions.Functions.command.Parameters.AddWithValue("@startDate", startDate);
                Functions.Functions.command.Parameters.AddWithValue("@endDate", endDate);

                SqlDataAdapter adapter = new SqlDataAdapter(Functions.Functions.command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                chart1.DataSource = dt;

                chart1.Series[0].Name = "Late";
                chart1.Series["Late"].XValueMember = "LastName";
                chart1.Series["Late"].YValueMembers = "Late";
                chart1.Series["Late"]["PixelPointWidth"] = "30";

                chart1.Titles.Clear();
                chart1.Titles.Add("Attendance");

                chart1.DataBind();
                chart1.Invalidate();
            }
            catch (Exception ex)
            {
                // Handle any errors that may occur
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        public void fillGraphAbsent(DateTime startDate, DateTime endDate)
        {
            try
            {
                Connection.Connection.DB();
                Functions.Functions.query = @"SELECT
                    users.LName as LastName, SUM(CASE WHEN attendance.status = 'Absent' THEN 1 ELSE 0 END) AS Absent
                FROM
                    attendance
                INNER JOIN 
                    users ON users.userID = attendance.userID
                WHERE
                    date between @startDate AND @endDate
                GROUP BY
                   users.LName";
                Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
                Functions.Functions.command.Parameters.AddWithValue("@startDate", startDate);
                Functions.Functions.command.Parameters.AddWithValue("@endDate", endDate);

                SqlDataAdapter adapter = new SqlDataAdapter(Functions.Functions.command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                chart1.DataSource = dt;

                chart1.Series[0].Name = "Absent";
                chart1.Series["Absent"].XValueMember = "LastName";
                chart1.Series["Absent"].YValueMembers = "Absent";
                chart1.Series["Absent"]["PixelPointWidth"] = "30";

                chart1.Titles.Clear();
                chart1.Titles.Add("Attendance");

                chart1.DataBind();
                chart1.Invalidate();
            }
            catch (Exception ex)
            {
                // Handle any errors that may occur
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void initializeChart() 
        {
            try
            {
                string selectedValue = cmbStatus.SelectedItem.ToString();

                if (selectedValue == "Present")
                {
                    fillGraphPresent(startDate, endDate);
                }
                else if (selectedValue == "Excused")
                {
                    fillGraphExcused(startDate, endDate);
                }
                else if (selectedValue == "Late")
                {
                    fillGraphLate(startDate, endDate);
                }
                else if (selectedValue == "Absent")
                {
                    fillGraphAbsent(startDate, endDate);
                }
                else 
                {
                    MessageBox.Show("Error!");
                }
            }
            catch (Exception ex) 
            {
            
            }
        }


        private void Generate_Report_Attendance_Analytics_Load(object sender, EventArgs e)
        {

            reportsTimer.Start();
            reportsCollapsed = true;
            btnReports.ForeColor = Color.Black;
            btnReports.BackColor = Color.White;
            btnReports.Font = new Font("Century Gothic", 14, FontStyle.Italic);
            
            try
            {
                Connection.Connection.DB();
                Functions.Functions.query = "Select FName + ' ' + LName as [Full Name] from users where FName like '%" + txtSearch.Text + "%' or LName like '%" + txtSearch.Text + "%' and roleID = 2";
                Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
                Functions.Functions.reader = Functions.Functions.command.ExecuteReader();
                AutoCompleteStringCollection name = new AutoCompleteStringCollection();

                while (Functions.Functions.reader.Read())
                {
                    name.Add(Functions.Functions.reader.GetString(0));
                }

                txtSearch.AutoCompleteCustomSource = name;
                Functions.Functions.reader.Close();
                Connection.Connection.con.Close();

                
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (Functions.Functions.reader != null && !Functions.Functions.reader.IsClosed)
                {
                    Functions.Functions.reader.Close();
                }
                if (Connection.Connection.con != null && Connection.Connection.con.State == ConnectionState.Open)
                {
                    Connection.Connection.con.Close();
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text.Trim()))
            {
                MessageBox.Show("Please enter an employee name.");
                return;
            }

            isSearchPerformed = true;
            UpdateAttendanceSummaryForEmployee();

            try
            {
                // Open the database connection
                Connection.Connection.DB();

                // First Query: Get the attendance summary and user ID
                Functions.Functions.query = @"
                SELECT 
                    users.userID
                FROM 
                    attendance 
                INNER JOIN 
                    users ON users.userID = attendance.userID 
                WHERE 
                    users.FName + ' ' + users.LName LIKE @searchText AND users.roleID = 2
                GROUP BY 
                    users.userID";

                Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
                Functions.Functions.command.Parameters.AddWithValue("@searchText", "%" + txtSearch.Text + "%");

                Functions.Functions.reader = Functions.Functions.command.ExecuteReader();

                int userId = 0;

                if (Functions.Functions.reader.HasRows)
                {
                    Functions.Functions.reader.Read();
                    userId = Convert.ToInt32(Functions.Functions.reader["userID"]);

                    // Close the reader after reading the necessary data
                    Functions.Functions.reader.Close();
                }
                else
                {
                    // Close the reader
                    Functions.Functions.reader.Close();
                    return;
                }

                // Second Query: Get the employee photo for the found user ID
                if (userId != 0)
                {
                    Functions.Functions.query = "SELECT fileName FROM users WHERE userID = @userID";
                    Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
                    Functions.Functions.command.Parameters.AddWithValue("@userID", userId);

                    Functions.Functions.reader = Functions.Functions.command.ExecuteReader();

                    if (Functions.Functions.reader.HasRows)
                    {
                        Functions.Functions.reader.Read();

                        string fileName = Functions.Functions.reader["fileName"].ToString(); // Using fileName to store the path

                        if (!string.IsNullOrEmpty(fileName))
                        {
                            pcboxEmployeePhoto.ImageLocation = fileName;
                        }
                        else
                        {
                            pcboxEmployeePhoto.Image = null; // Clear PictureBox if no photo found
                        }

                        Functions.Functions.reader.Close();
                    }

                    Functions.Functions.reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                // Close the connection
                if (Connection.Connection.con != null && Connection.Connection.con.State == ConnectionState.Open)
                {
                    Connection.Connection.con.Close();
                }
                if (Functions.Functions.reader != null && !Functions.Functions.reader.IsClosed)
                {
                    Functions.Functions.reader.Close();
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            pcboxEmployeePhoto.Image = null;
            lblPresent.Text = "0";
            lblLate.Text = "0";
            lblExcused.Text = "0";
            lblAbsent.Text = "0";
            txtSearch.Text = "";
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

        private void btnInstallations_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageInstallation manageInstallation = new ManageInstallation();
            manageInstallation.Show();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            this.Hide();
            AdminDashboard home = new AdminDashboard();
            home.Show();
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

        private void btnAttendanceReport_Click(object sender, EventArgs e)
        {
            this.Hide();
            Generate_Report_Attendance_Analytics attendance_Analytics = new Generate_Report_Attendance_Analytics();
            attendance_Analytics.Show();
        }

        private void btnSalesReport_Click(object sender, EventArgs e)
        {
            this.Hide();
            GenerateReport_Sales_Analytics sales_Analytics = new GenerateReport_Sales_Analytics();
            sales_Analytics.Show();
        }
    }
}