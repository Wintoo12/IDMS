using IDMS.Admin.Manage_Installation;
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

namespace IDMS.Staff.Manage_Customer
{
    public partial class ManageCustomer_Staff : Form
    {
        public static int iterationCount = 0;
        bool orderCollapsed;
        bool suppliesCollapsed;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handleParam = base.CreateParams;
                handleParam.ExStyle |= 0x02000000;
                return handleParam;
            }
        }

        public ManageCustomer_Staff()
        {
            InitializeComponent();
        }

        public void viewCustomer()
        {
            Connection.Connection.DB();
            Functions.Functions.query = "Select * from customer";
            Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
            Functions.Functions.reader = Functions.Functions.command.ExecuteReader();

            Image stockImage = new Bitmap(@"C:\Users\Mark Andrew\Documents\3rd Year School Stuff\IDMS\UI purposes\Stocks1.png");
            Image buttonImage = new Bitmap(@"C:\Users\Mark Andrew\Documents\3rd Year School Stuff\IDMS\UI purposes\button1.png");
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
                        pnl.Size = new Size(555, 52);
                        //pnl.Padding = new Padding(5);

                        Label lblCustomerName = new Label();
                        //lblProductName.Name = Functions.Functions.reader["productID"].ToString();
                        lblCustomerName.Text = fullname;
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

        public void button_click(Object sender, EventArgs e)
        {
            Button btn = sender as Button;
            int customerID = Convert.ToInt32(btn.Tag);
            viewCustomerDetails(customerID);
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
                MessageBox.Show(ex.Message);
            }

        }

        private void ManageCustomer_Staff_Load(object sender, EventArgs e)
        {
            viewCustomer();
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
                        pnl.Size = new Size(555, 52);
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

                        //pnl.Controls.Add(btnUpdate);
                        pnl.Controls.Add(lblCustomerName);
                        pnl.Controls.Add(btnViewCustomerDetails);
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

        private void btnSupplies_Click(object sender, EventArgs e)
        {
            suppliesTimer.Start();
            btnSupplies.ForeColor = Color.Black;
            btnSupplies.BackColor = Color.White;
            btnSupplies.Font = new Font("Century Gothic", 14, FontStyle.Italic);
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageCustomer_Staff manageCustomer_Staff = new ManageCustomer_Staff();
            manageCustomer_Staff.Show();
        }

        private void btnInstallation_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageInstallation_Staff manageInstallation_Staff = new ManageInstallation_Staff();
            manageInstallation_Staff.Show();
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

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                // Clear the flowLayoutPanel when the search box is empty
                flowLayoutPanel2.Controls.Clear();
                viewCustomer();
            }
        }
    }
}
