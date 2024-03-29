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

        private void ManageSupply_DashboardAdmin_Load(object sender, EventArgs e)
        {
            suppliesTimer.Start();
            suppliesCollapsed = false;
            btnManageSupp.ForeColor = Color.Black;
            btnManageSupp.BackColor = Color.White;
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

        private void customerTimer_Tick(object sender, EventArgs e)
        {
            if (customerCollapsed)
            {
                customerContainer.Height += 10;
                if (customerContainer.Height == customerContainer.MaximumSize.Height)
                {
                    customerCollapsed = false;
                    customerTimer.Stop();
                }
            }
            else
            {
                customerContainer.Height -= 10;
                if (customerContainer.Height == customerContainer.MinimumSize.Height)
                {
                    customerCollapsed = true;
                    customerTimer.Stop();
                    btnCustomer.ForeColor = Color.White;
                    btnCustomer.BackColor = Color.Transparent;
                    btnCustomer.Font = new Font("Century Gothic", 14, FontStyle.Bold);
                }
            }
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            customerTimer.Start();

            btnCustomer.ForeColor = Color.Black;
            btnCustomer.BackColor = Color.White;
            btnCustomer.Font = new Font("Century Gothic", 14, FontStyle.Italic);
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

        private void btnReservations_Click(object sender, EventArgs e)
        {
            btnReservations.ForeColor = Color.Black;
            btnReservations.BackColor = Color.White;
            btnReservations.Font = new Font("Century Gothic", 14, FontStyle.Italic);

            this.Hide();
            Admin.ManageReservations reservations = new Admin.ManageReservations();
            reservations.Show();
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
            Functions.Functions.query = "SELECT * FROM product WHERE productID = @ProductID";
            Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
            Functions.Functions.command.Parameters.AddWithValue("@ProductID", productID);
            Functions.Functions.reader = Functions.Functions.command.ExecuteReader();

            if (Functions.Functions.reader.HasRows)
            {
                Functions.Functions.reader.Read();
                lblProductDetails.Text = Functions.Functions.reader["productName"].ToString();
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
                    pnl.Size = new Size(538, 52);
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
                    btnViewProductDetails.Location = new Point(420, 11);

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
            Console.WriteLine(btn.Name); 
            int productID = Convert.ToInt32(btn.Tag);
            viewProductDetails(productID);
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
                        pnl.Size = new Size(538, 52);

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
                        btnViewProductDetails.Location = new Point(420, 11);

                        pnl.Controls.Add(lblProductName);
                        pnl.Controls.Add(btnViewProductDetails);
                        flowLayoutPanel2.Controls.Add(pnl);
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
