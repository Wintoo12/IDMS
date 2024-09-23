using IDMS.Admin.Manage_Employee;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDMS.Admin.Generate_Report
{
    public partial class GenerateReport_Sales_Analytics : Form
    {
        bool suppliesCollapsed, customersCollapsed, employeesCollapsed, reportsCollapsed;
        private DateTime startDate;
        private DateTime endDate;

        public GenerateReport_Sales_Analytics()
        {
            InitializeComponent();
            cmbDay.SelectedIndexChanged += new EventHandler(cmbDay_SelectedIndexChanged);
            cmbCategory.SelectedIndexChanged += new EventHandler(cmbCategory_SelectedIndexChanged); // Wire up the event handler
            cmbProducts.SelectedIndexChanged += new EventHandler(cmbProducts_SelectedIndexChanged);

            cmbDay.Items.Clear();
            cmbDay.Items.Add("This Week");
            cmbDay.Items.Add("This Month");
            cmbDay.Items.Add("Last 3 Months");
            cmbDay.SelectedIndex = 0; // Set default selection

            cmbCategory.Items.Clear();
            cmbCategory.Items.Add("Supplies");
            cmbCategory.Items.Add("Package");
            cmbCategory.SelectedIndex = 0;

            // Initialize date range for the default selection
            InitializeDateRange();
            GenerateReport_Sales_Analytics_Load(this, EventArgs.Empty);

            InitializeComboBox();
            initializeChart();
        }

        private void cmbProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            initializeChart();
        }

        private List<string> GetProductNames()
        {
            List<string> productNames = new List<string>();

            try
            {
                Connection.Connection.DB();
                string query = "SELECT productName FROM product"; // Adjust table name as necessary
                using (SqlCommand command = new SqlCommand(query, Connection.Connection.con))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            productNames.Add(reader["productName"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                MessageBox.Show("Error retrieving product names: " + ex.Message);
            }

            return productNames;
        }

        public void fillGFraphSupplies(DateTime startDate, DateTime endDate) 
        {
            try
            {
                string product = cmbProducts.SelectedItem.ToString();
                // Establish the database connection
                Connection.Connection.DB();
                // Define the query
                Functions.Functions.query = "SELECT dateSold, totalPrice FROM sales inner join product on product.productID = sales.productID where productName = @productName AND (dateSold BETWEEN @startDate AND @endDate)";
                Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
                Functions.Functions.command.Parameters.AddWithValue("@productName", product);
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

        public void fillGFraphPackage(DateTime startDate, DateTime endDate)
        {
            try
            {
                string package = cmbProducts.SelectedItem.ToString();
                // Establish the database connection
                Connection.Connection.DB();
                // Define the query
                Functions.Functions.query = "SELECT sales.dateSold, sales.totalPrice FROM sales inner join package on package.packageID = sales.packageID where packageName = @packageName AND (dateSold BETWEEN @startDate AND @endDate)";
                Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
                Functions.Functions.command.Parameters.AddWithValue("@packageName", package);
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

        private void initializeChart() 
        {
            try 
            {
                string selectedValue = cmbCategory.SelectedItem.ToString();

                if (selectedValue == "Supplies")
                {
                    fillGFraphSupplies(startDate, endDate);
                }
                else if (selectedValue == "Package")
                {
                    fillGFraphPackage(startDate, endDate);
                }
                else 
                {
                    MessageBox.Show("Error!");
                }
            }
            catch(Exception ex)
            {
            
            }
        }

        private List<string> GetPackageNames()
        {
            List<string> packageNames = new List<string>();

            try
            {
                Connection.Connection.DB();
                string query = "SELECT packageName FROM package"; // Adjust table name as necessary
                using (SqlCommand command = new SqlCommand(query, Connection.Connection.con))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            packageNames.Add(reader["packageName"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                MessageBox.Show("Error retrieving package names: " + ex.Message);
            }

            return packageNames;
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
                endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1); // Last day of the previous month
            }
            else 
            {
                MessageBox.Show("No date found!");
            }
        }

        private void cmbDay_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                InitializeDateRange();
                GenerateReport_Sales_Analytics_Load(sender, e);
            }
            catch (Exception ex)
            {
                // Handle exception (optional: log or display an error message)
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

        private void GenerateReport_Sales_Analytics_Load(object sender, EventArgs e)
        {
            try
            {
                countProductSales(startDate, endDate);
                countPackageSales(startDate, endDate);
                totalPriceSupplies(startDate, endDate);
                totalPricePackage(startDate, endDate);
            }
            catch (Exception ex)
            {
                // Handle exception
            }
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitializeComboBox();
            initializeChart();
        }

        private void InitializeComboBox()
        {
            string selectedValue = cmbCategory.SelectedItem.ToString(); // Use SelectedItem instead of SelectedValue

            cmbProducts.Items.Clear(); // Clear previous items

            if (selectedValue == "Supplies")
            {
                List<string> productNames = GetProductNames(); // Get product names
                foreach (string productName in productNames)
                {
                    cmbProducts.Items.Add(productName); // Add each product name to cmbProducts
                }
            }
            else if (selectedValue == "Package")
            {
                List<string> packageNames = GetPackageNames(); // Get package names
                foreach (string packageName in packageNames)
                {
                    cmbProducts.Items.Add(packageName); // Add each package name to cmbProducts
                }
            }

            if (cmbProducts.Items.Count > 0)
            {
                cmbProducts.SelectedIndex = 0; // Optionally select the first item if there are items
            }
        }

        private void countProductSales(DateTime startDate, DateTime endDate)
        {
            try
            {
                Connection.Connection.DB();
                Functions.Functions.query = "SELECT COUNT(productID) AS productSold FROM sales WHERE dateSold BETWEEN @startDate AND @endDate";
                Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
                Functions.Functions.command.Parameters.AddWithValue("@startDate", startDate);
                Functions.Functions.command.Parameters.AddWithValue("@endDate", endDate);
                Functions.Functions.reader = Functions.Functions.command.ExecuteReader();

                if (Functions.Functions.reader.HasRows)
                {
                    Functions.Functions.reader.Read();
                    lblSuppliesSold.Text = Functions.Functions.reader["productSold"].ToString();
                }
                Functions.Functions.reader.Close();
            }
            catch (Exception ex)
            {
                // Handle exception
            }
        }

        private void countPackageSales(DateTime startDate, DateTime endDate)
        {
            try
            {
                Connection.Connection.DB();
                Functions.Functions.query = "SELECT COUNT(packageID) AS packageSold FROM sales WHERE dateSold BETWEEN @startDate AND @endDate";
                Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
                Functions.Functions.command.Parameters.AddWithValue("@startDate", startDate);
                Functions.Functions.command.Parameters.AddWithValue("@endDate", endDate);
                Functions.Functions.reader = Functions.Functions.command.ExecuteReader();

                if (Functions.Functions.reader.HasRows)
                {
                    Functions.Functions.reader.Read();
                    lblPackageSold.Text = Functions.Functions.reader["packageSold"].ToString();
                }
                Functions.Functions.reader.Close();
            }
            catch (Exception ex)
            {
                // Handle exception
            }
        }

        private void totalPriceSupplies(DateTime startDate, DateTime endDate)
        {
            try
            {
                Connection.Connection.DB();
                Functions.Functions.query = "SELECT SUM(totalPrice) AS totalPrice FROM sales WHERE productID != 0 AND (dateSold BETWEEN @startDate AND @endDate)";
                Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
                Functions.Functions.command.Parameters.AddWithValue("@startDate", startDate);
                Functions.Functions.command.Parameters.AddWithValue("@endDate", endDate);
                Functions.Functions.reader = Functions.Functions.command.ExecuteReader();

                if (Functions.Functions.reader.HasRows)
                {
                    Functions.Functions.reader.Read();

                    var totalPriceValue = Functions.Functions.reader["totalPrice"];
                    float price = totalPriceValue != DBNull.Value ? Convert.ToSingle(totalPriceValue) : 0;
                    lblTotalPriceSold.Text = price.ToString("N2");
                }
                else
                {
                    lblTotalPriceSold.Text = "0.00";
                }
                Functions.Functions.reader.Close();
            }
            catch (Exception ex)
            {
                // Handle exception
            }
        }

        private void totalPricePackage(DateTime startDate, DateTime endDate)
        {
            try
            {
                Connection.Connection.DB();
                Functions.Functions.query = "SELECT SUM(totalPrice) AS totalPrice FROM sales WHERE (packageID != 0) AND (dateSold BETWEEN @startDate AND @endDate)";
                Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
                Functions.Functions.command.Parameters.AddWithValue("@startDate", startDate);
                Functions.Functions.command.Parameters.AddWithValue("@endDate", endDate);
                Functions.Functions.reader = Functions.Functions.command.ExecuteReader();

                if (Functions.Functions.reader.HasRows)
                {
                    Functions.Functions.reader.Read();

                    var totalPriceValue = Functions.Functions.reader["totalPrice"];
                    float price = totalPriceValue != DBNull.Value ? Convert.ToSingle(totalPriceValue) : 0;
                    lblTotalPackageSold.Text = price.ToString("N2");
                }
                else
                {
                    lblTotalPackageSold.Text = "0.00";
                }
                Functions.Functions.reader.Close();
            }
            catch (Exception ex)
            {
                // Handle exception
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
    }
}
