using IDMS.Admin.Manage_Installation;
using IDMS.Staff.Manage_Customer;
using IDMS.Staff.Process_Order.Installations;
using IDMS.Staff.Process_Order.Supplies;
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

namespace IDMS
{
    public partial class ManageSupply_ViewStatusListStaff : Form
    {
        public static int iterationCount = 0;
        bool orderCollapsed, suppliesCollapsed;
        public ManageSupply_ViewStatusListStaff()
        {
            InitializeComponent();
        }

        public void viewProduct()
        {
            Connection.Connection.DB();
            Functions.Functions.query = "Select * from product";
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

                    pnlBack.Controls.Add(lblProductName);
                    pnlBack.Controls.Add(pnlStatusBar);
                    pnlStatusBar.Controls.Add(lblProductQuantity);
                    flowLayoutPanel2.Controls.Add(pnlBack);
                    flowLayoutPanel2.FlowDirection = FlowDirection.TopDown;

                    colorIndicator(stock, pnlStatusBar, lblProductQuantity, threshold);
                    //colorIndicator(stock, lblProductQuantity, btnRestock);
                }
            }
        }

        public void colorIndicator(int stock, Panel pnlStatusBar, Label lblProductQuantity, int threshold)
        {

            if (stock >= 0 && stock <= 249)
            {
                pnlStatusBar.BackColor = Color.FromArgb(239, 1, 1);
                //lblProductQuantity.Text = Convert.ToString(stock) + " " + "/1000";
                pnlStatusBar.Size = new Size(537, 23);
                pnlStatusBar.Location = new Point(215, 9);
            }
            //Orange
            else if (stock >= 250 && stock <= 499)
            {
                pnlStatusBar.BackColor = Color.FromArgb(239, 179, 1);
                lblProductQuantity.Text = Convert.ToString(stock) + "/" + Convert.ToString(threshold);
                pnlStatusBar.Size = new Size(537, 23);
                pnlStatusBar.Location = new Point(215, 9);
            }
            //Yellow
            else if (stock >= 500 && stock <= 749)
            {
                pnlStatusBar.BackColor = Color.FromArgb(239, 239, 1);
                lblProductQuantity.Text = Convert.ToString(stock) + "/" + Convert.ToString(threshold);
                pnlStatusBar.Size = new Size(537, 23);
                pnlStatusBar.Location = new Point(215, 9);
            }
            //Teal
            else if (stock >= 750 && stock <= 999)
            {
                pnlStatusBar.BackColor = Color.FromArgb(0, 150, 137);
                lblProductQuantity.Text = Convert.ToString(stock) + "/" + Convert.ToString(threshold);
                pnlStatusBar.Size = new Size(537, 23);
                pnlStatusBar.Location = new Point(215, 9);
            }
            //Green
            else if (stock == 1000)
            {
                pnlStatusBar.BackColor = Color.FromArgb(1, 237, 3);
                lblProductQuantity.Text = Convert.ToString(stock) + "/" + Convert.ToString(threshold);
                pnlStatusBar.Size = new Size(537, 23);
                pnlStatusBar.Location = new Point(215, 9);
            }
        }

        private void ManageSupply_ViewStatusListStaff_Load(object sender, EventArgs e)
        {
            viewProduct();
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

        private void btnInstallations_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageInstallation_Staff manageInstallation_Staff = new ManageInstallation_Staff();
            manageInstallation_Staff.Show();
        }

        private void btnEmployees_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageCustomer_Staff manageCustomer_Staff = new ManageCustomer_Staff();
            manageCustomer_Staff.Show();
        }

        private void ordersTimer_Tick(object sender, EventArgs e)
        {
            if (orderCollapsed)
            {
                ordersContainer.Height += 10;
                if (ordersContainer.Height == ordersContainer.MaximumSize.Height)
                {
                    orderCollapsed = false;
                    ordersTimer.Stop();
                }
            }
            else
            {
                ordersContainer.Height -= 10;
                if (ordersContainer.Height == ordersContainer.MinimumSize.Height)
                {
                    orderCollapsed = true;
                    ordersTimer.Stop();
                    btnOrder.ForeColor = Color.White;
                    btnOrder.BackColor = Color.Transparent;
                    btnOrder.Font = new Font("Century Gothic", 14, FontStyle.Bold);
                }
            }
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            ordersTimer.Start();

            btnOrder.ForeColor = Color.Black;
            btnOrder.BackColor = Color.White;
            btnOrder.Font = new Font("Century Gothic", 14, FontStyle.Italic);
        }

        private void btnOrder_Leave(object sender, EventArgs e)
        {
            btnOrder.Font = new Font("Century Gothic", 14, FontStyle.Bold);
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

        private void btnVIewSupplies_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageSupply_DashboardStaff manageSupply = new ManageSupply_DashboardStaff();
            manageSupply.Show();
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

                        pnlBack.Controls.Add(lblProductName);
                        pnlBack.Controls.Add(pnlStatusBar);
                        pnlStatusBar.Controls.Add(lblProductQuantity);
                        flowLayoutPanel2.Controls.Add(pnlBack);
                        flowLayoutPanel2.FlowDirection = FlowDirection.TopDown;

                        colorIndicator(stock, pnlStatusBar, lblProductQuantity, threshold);
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

        private void btnViewStatus_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageSupply_ViewStatusListStaff manageSupply = new ManageSupply_ViewStatusListStaff();
            manageSupply.Show();
        }

    }
}
