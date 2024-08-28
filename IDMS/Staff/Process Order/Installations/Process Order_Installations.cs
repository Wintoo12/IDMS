using IDMS.Admin.Manage_Installation;
using IDMS.Staff.Manage_Customer;
using IDMS.Staff.Process_Order.Supplies;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDMS.Staff.Process_Order.Installations
{
    public partial class Process_Order_Installations : Form
    {
        public static int iterationCount = 0;
        public static List<int> setpackageId = new List<int>();
        public static float packagePrice;
        public static Dictionary<int, int> productQuantities = new Dictionary<int, int>();
        public static float totalPrice = 0;
        bool orderCollapsed, suppliesCollapsed;

        public Process_Order_Installations()
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

        public void viewProducts()
        {
            Connection.Connection.DB();
            Functions.Functions.query = "Select * from package";
            Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
            Functions.Functions.reader = Functions.Functions.command.ExecuteReader();

            Image buttonImage = new Bitmap(@"C:\Users\Mark Andrew\Documents\3rd Year School Stuff\IDMS\UI purposes\button1.png");
            Image PanelImage = new Bitmap(@"C:\Users\Mark Andrew\Documents\3rd Year School Stuff\IDMS\UI purposes\installationPanel.png");

            if (Functions.Functions.reader.HasRows)
            {
                flowLayoutPanel2.FlowDirection = FlowDirection.TopDown;
                flowLayoutPanel2.WrapContents = true;
                flowLayoutPanel2.AutoScroll = true; // Enable auto-scrolling

                flowLayoutPanel2.HorizontalScroll.Enabled = false;
                flowLayoutPanel2.HorizontalScroll.Visible = false;
                flowLayoutPanel2.WrapContents = false;

                flowLayoutPanel2.Controls.Clear();
                while (Functions.Functions.reader.Read())
                {
                    iterationCount++;

                    int packageID = Convert.ToInt32(Functions.Functions.reader["packageID"]);

                    Panel pnl = new Panel();
                    //pnl.BackgroundImage = Properties.Resources.Untitled_design__42_;
                    pnl.BackgroundImage = PanelImage;
                    pnl.BackgroundImageLayout = ImageLayout.Stretch;
                    pnl.Size = new Size(554, 126);
                    //pnl.Padding = new Padding(5);

                    PictureBox productPhoto = new PictureBox();
                    productPhoto.Size = new Size(135, 97);
                    productPhoto.Location = new Point(17, 12); // Using fileName to store the path

                    string filePath = Functions.Functions.reader["fileName"].ToString(); // Get the file path from the database

                    if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath)) // Check if the file path is not empty and the file exists
                    {
                        productPhoto.Image = Image.FromFile(filePath); // Load image from file path
                        productPhoto.SizeMode = PictureBoxSizeMode.StretchImage; // Set size mode
                    }
                    else
                    {
                        productPhoto.Image = null; // Clear PictureBox if no photo found
                    }

                    Button btnAddtoCart = new Button();
                    btnAddtoCart.Click += new EventHandler(this.btnAddtoCart_click);
                    btnAddtoCart.Tag = packageID;
                    btnAddtoCart.Size = new Size(110, 34);
                    btnAddtoCart.Location = new Point(420, 74);
                    btnAddtoCart.Text = "Add to Cart";
                    btnAddtoCart.Font = new Font("Century Gothic", 10, FontStyle.Regular);

                    Label lblProductName = new Label();
                    //lblProductName.Name = Functions.Functions.reader["productID"].ToString();
                    lblProductName.Text = Functions.Functions.reader["packageName"].ToString();
                    lblProductName.BackColor = Color.Transparent;
                    lblProductName.ForeColor = Color.White;
                    lblProductName.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                    lblProductName.AutoSize = false; // Set AutoSize to false
                    lblProductName.MaximumSize = new Size(430, 0); // Limit the width
                    lblProductName.AutoEllipsis = true; // Show ellipsis when text overflows
                    lblProductName.TextAlign = ContentAlignment.MiddleLeft; // Align text to the left
                    lblProductName.Size = new Size(500, lblProductName.PreferredHeight); // Set size manually
                    lblProductName.Location = new Point(158, 12);

                    Label lblPrice = new Label();
                    float price = Convert.ToSingle(Functions.Functions.reader["totalPrice"]);
                    lblPrice.Text = "₱" + price.ToString("N2");
                    lblPrice.BackColor = Color.Transparent;
                    lblPrice.ForeColor = Color.White;
                    lblPrice.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                    lblPrice.Padding = new Padding(1);
                    lblPrice.AutoSize = true;
                    lblPrice.TextAlign = ContentAlignment.MiddleCenter;
                    lblPrice.Location = new Point(158, 88);

                    pnl.Controls.Add(btnAddtoCart);
                    pnl.Controls.Add(productPhoto);
                    pnl.Controls.Add(lblPrice);
                    pnl.Controls.Add(lblProductName);
                    flowLayoutPanel2.Controls.Add(pnl);
                }
            }
        }

        public void addToCart(int packageID)
        {
            try
            {
                Connection.Connection.DB();
                Functions.Functions.query = "SELECT * FROM package WHERE packageID = @packageID";
                Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
                Functions.Functions.command.Parameters.AddWithValue("@packageID", packageID);
                Functions.Functions.reader = Functions.Functions.command.ExecuteReader();

                Image buttonImage = new Bitmap(@"C:\Users\Mark Andrew\Documents\3rd Year School Stuff\IDMS\UI purposes\button1.png");
                Image PanelImage = new Bitmap(@"C:\Users\Mark Andrew\Documents\3rd Year School Stuff\IDMS\UI purposes\itemsAdded.png");

                if (Functions.Functions.reader.HasRows)
                {
                    flowLayoutPanel3.FlowDirection = FlowDirection.TopDown;
                    flowLayoutPanel3.WrapContents = true;
                    flowLayoutPanel3.AutoScroll = true; // Enable auto-scrolling

                    flowLayoutPanel3.HorizontalScroll.Enabled = false;
                    flowLayoutPanel3.HorizontalScroll.Visible = false;
                    flowLayoutPanel3.WrapContents = false;

                    while (Functions.Functions.reader.Read())
                    {
                        int setpackageID = Convert.ToInt32(Functions.Functions.reader["packageID"].ToString());

                        Process_Order_Installations.setpackageId.Add(setpackageID);
                        Console.WriteLine(setpackageID);

                        Panel pnl = new Panel();
                        pnl.BackgroundImage = PanelImage;
                        pnl.BackgroundImageLayout = ImageLayout.Stretch;
                        pnl.Size = new Size(312, 140);

                        PictureBox pcboxProductPhoto = new PictureBox();
                        pcboxProductPhoto.Size = new Size(100, 85);
                        string filePath = Functions.Functions.reader["fileName"].ToString(); // Get the file path from the database

                        if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath)) // Check if the file path is not empty and the file exists
                        {
                            pcboxProductPhoto.Image = Image.FromFile(filePath); // Load image from file path
                            pcboxProductPhoto.SizeMode = PictureBoxSizeMode.StretchImage; // Set size mode
                        }
                        else
                        {
                            pcboxProductPhoto.Image = null; // Clear PictureBox if no photo found
                        }
                        pcboxProductPhoto.Location = new Point(14, 26);

                        Label lblProductName = new Label();
                        lblProductName.Text = Functions.Functions.reader["packageName"].ToString();
                        Console.WriteLine(lblProductName.Text);
                        lblProductName.BackColor = Color.Transparent;
                        lblProductName.ForeColor = Color.White;
                        lblProductName.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                        lblProductName.AutoSize = false; // Set AutoSize to false
                        lblProductName.MaximumSize = new Size(190, 0); // Limit the width
                        lblProductName.AutoEllipsis = true; // Show ellipsis when text overflows
                        lblProductName.TextAlign = ContentAlignment.MiddleLeft; // Align text to the left
                        lblProductName.Size = new Size(280, lblProductName.PreferredHeight); // Set size manually
                        lblProductName.Location = new Point(123, 19);

                        //Edited Code
                        float price = Convert.ToSingle(Functions.Functions.reader["totalPrice"]);

                        Label lblProductPrice = new Label();
                        lblProductPrice.Text = "₱" + price.ToString("N2");
                        lblProductPrice.BackColor = Color.Transparent;
                        lblProductPrice.ForeColor = Color.White;
                        lblProductPrice.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                        lblProductPrice.AutoSize = true;
                        lblProductPrice.Location = new Point(123, 110);

                        pnl.Controls.Add(pcboxProductPhoto);
                        pnl.Controls.Add(lblProductName);
                        pnl.Controls.Add(lblProductPrice);
                        flowLayoutPanel3.Controls.Add(pnl);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void btnAddtoCart_click(object sender, EventArgs e)
        {
            Button btnAddtoCart = sender as Button;

            if (btnAddtoCart != null)
            {
                int packageID = Convert.ToInt32(btnAddtoCart.Tag);
                // Find the corresponding TextBox by iterating through the controls of the parent Panel
                Panel parentPanel = btnAddtoCart.Parent as Panel;
                addToCart(packageID);
         
            }
            else
            {
                MessageBox.Show("Button not found");
            }

            if (flowLayoutPanel3.Controls.Count > 1)
            {
                MessageBox.Show("You have ordered more than one package", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                flowLayoutPanel3.Controls.Clear();
            }
        }

        private void Process_Order_Installations_Load(object sender, EventArgs e)
        {
            viewProducts();
        }

        public static void ResetOrder()
        {
            setpackageId.Clear();
            packagePrice = 0;
            productQuantities.Clear();
            totalPrice = 0;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to cancel the items in the cart?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                flowLayoutPanel3.Controls.Clear();
                setpackageId.Clear();
            }       
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            if (flowLayoutPanel3 == null || flowLayoutPanel3.Controls.Count == 0)
            {
                MessageBox.Show("You cannot proceed to checkout when your cart is empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                this.Hide();
                Checkout_Installations checkout = new Checkout_Installations();
                checkout.Show();
            }
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            this.Hide();
            StaffDashboard dashboard = new StaffDashboard();
            dashboard.Show();
        }

        private void btnInstallations_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageInstallation_Staff manageInstallation_Staff = new ManageInstallation_Staff();
            manageInstallation_Staff.Show();
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageCustomer_Staff manageCustomer_Staff = new ManageCustomer_Staff();
            manageCustomer_Staff.Show();
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

        private void btnViewStatus_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageSupply_ViewStatusListStaff manageSupply = new ManageSupply_ViewStatusListStaff();
            manageSupply.Show();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                // Clear the flowLayoutPanel when the search box is empty
                flowLayoutPanel2.Controls.Clear();
                viewProducts();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Connection.Connection.DB();
                Functions.Functions.query = "Select * from package where packageName like '%' + @searchedPackage + '%'";
                Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
                Functions.Functions.command.Parameters.AddWithValue("@searchedPackage", txtSearch.Text);

                Functions.Functions.reader = Functions.Functions.command.ExecuteReader();

                if (Functions.Functions.reader.HasRows)
                {
                    flowLayoutPanel2.FlowDirection = FlowDirection.TopDown;
                    flowLayoutPanel2.WrapContents = false;
                    flowLayoutPanel2.AutoScroll = true;

                    flowLayoutPanel2.HorizontalScroll.Enabled = false;
                    flowLayoutPanel2.HorizontalScroll.Visible = false;

                    flowLayoutPanel2.Controls.Clear(); 

                    while (Functions.Functions.reader.Read())
                    {
                        int packageID = Convert.ToInt32(Functions.Functions.reader["packageID"]);

                        Panel pnl = new Panel
                        {
                            BackgroundImage = Image.FromFile(@"C:\Users\Mark Andrew\Documents\3rd Year School Stuff\IDMS\UI purposes\installationPanel.png"),
                            BackgroundImageLayout = ImageLayout.Stretch,
                            Size = new Size(554, 126)
                        };

                        PictureBox productPhoto = new PictureBox
                        {
                            Size = new Size(135, 97),
                            Location = new Point(17, 12),
                            SizeMode = PictureBoxSizeMode.StretchImage
                        };

                        string filePath = Functions.Functions.reader["fileName"].ToString();

                        if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                        {
                            productPhoto.Image = Image.FromFile(filePath);
                        }
                        else
                        {
                            productPhoto.Image = null;
                        }

                        Button btnAddtoCart = new Button();
                        btnAddtoCart.Click += new EventHandler(this.btnAddtoCart_click);
                        btnAddtoCart.Tag = packageID;
                        btnAddtoCart.Size = new Size(110, 34);
                        btnAddtoCart.Location = new Point(420, 74);
                        btnAddtoCart.Text = "Add to Cart";
                        btnAddtoCart.Font = new Font("Century Gothic", 10, FontStyle.Regular);

                        Label lblProductName = new Label();
                        //lblProductName.Name = Functions.Functions.reader["productID"].ToString();
                        lblProductName.Text = Functions.Functions.reader["packageName"].ToString();
                        lblProductName.BackColor = Color.Transparent;
                        lblProductName.ForeColor = Color.White;
                        lblProductName.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                        lblProductName.AutoSize = false; // Set AutoSize to false
                        lblProductName.MaximumSize = new Size(430, 0); // Limit the width
                        lblProductName.AutoEllipsis = true; // Show ellipsis when text overflows
                        lblProductName.TextAlign = ContentAlignment.MiddleLeft; // Align text to the left
                        lblProductName.Size = new Size(500, lblProductName.PreferredHeight); // Set size manually
                        lblProductName.Location = new Point(158, 12);

                        Label lblPrice = new Label();
                        float price = Convert.ToSingle(Functions.Functions.reader["totalPrice"]);
                        lblPrice.Text = "₱" + price.ToString("N2");
                        lblPrice.BackColor = Color.Transparent;
                        lblPrice.ForeColor = Color.White;
                        lblPrice.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                        lblPrice.Padding = new Padding(1);
                        lblPrice.AutoSize = true;
                        lblPrice.TextAlign = ContentAlignment.MiddleCenter;
                        lblPrice.Location = new Point(158, 88);

                        pnl.Controls.Add(btnAddtoCart);
                        pnl.Controls.Add(productPhoto);
                        pnl.Controls.Add(lblPrice);
                        pnl.Controls.Add(lblProductName);
                        flowLayoutPanel2.Controls.Add(pnl);
                    }
                }
                else
                {
                    MessageBox.Show("No packages found matching the search criteria.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    flowLayoutPanel2.Controls.Clear();
                }

                Functions.Functions.reader.Close();
                Connection.Connection.con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


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
    }
}
