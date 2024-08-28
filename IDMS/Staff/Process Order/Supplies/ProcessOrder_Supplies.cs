using IDMS.Admin.Manage_Installation;
using IDMS.Staff.Manage_Customer;
using IDMS.Staff.Process_Order.Installations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDMS.Staff.Process_Order.Supplies
{
    public partial class ProcessOrder_Supplies : Form
    {
        public static int iterationCount = 0;
        public static List<int> setproductId = new List<int>();
        public static Dictionary<int, float> productPrices = new Dictionary<int, float>();
        public static Dictionary<int, int> productQuantities = new Dictionary<int, int>();
        public static float totalPrice = 0;
        bool orderCollapsed, suppliesCollapsed;
        public ProcessOrder_Supplies()
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
            Functions.Functions.query = "Select * from product";
            Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
            Functions.Functions.reader = Functions.Functions.command.ExecuteReader();

            Image buttonImage = new Bitmap(@"C:\Users\Mark Andrew\Documents\3rd Year School Stuff\IDMS\UI purposes\button1.png");
            Image PanelImage = new Bitmap(@"C:\Users\Mark Andrew\Documents\3rd Year School Stuff\IDMS\UI purposes\PanelOrder.png");

            if (Functions.Functions.reader.HasRows)
            {
                flowLayoutPanel2.FlowDirection = FlowDirection.LeftToRight;
                flowLayoutPanel2.WrapContents = true;
                flowLayoutPanel2.AutoScroll = true; // Enable auto-scrolling

                flowLayoutPanel2.HorizontalScroll.Enabled = false;
                flowLayoutPanel2.HorizontalScroll.Visible = false;

                flowLayoutPanel2.Controls.Clear();
                while (Functions.Functions.reader.Read())
                {
                    iterationCount++;

                    int productID = Convert.ToInt32(Functions.Functions.reader["productID"]);

                    Panel pnl = new Panel();
                    //pnl.BackgroundImage = Properties.Resources.Untitled_design__42_;
                    pnl.BackgroundImage = PanelImage;
                    pnl.BackgroundImageLayout = ImageLayout.Stretch;
                    pnl.Size = new Size(172, 236);
                    //pnl.Padding = new Padding(5);

                    PictureBox productPhoto = new PictureBox();
                    productPhoto.Size = new Size(141, 90);
                    productPhoto.Location = new Point(15, 11);

                    string filePath = Functions.Functions.reader["fileName"].ToString();
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
                    btnAddtoCart.Tag = productID;
                    btnAddtoCart.Size = new Size(140, 24);
                    btnAddtoCart.Location = new Point(15, 200);
                    btnAddtoCart.Text = "Add to Cart";
                    btnAddtoCart.Font = new Font("Century Gothic", 10, FontStyle.Regular);

                    TextBox txtQuantity = new TextBox();
                    txtQuantity.Font = new Font("Century Gothic", 11, FontStyle.Regular);
                    txtQuantity.Location = new Point(63, 169);
                    txtQuantity.Size = new Size(38, 26);
                    txtQuantity.Text = "0";
                    txtQuantity.TextAlign = HorizontalAlignment.Center;

                    Button btnDecrease = new Button();
                    btnDecrease.Click += new EventHandler(this.btnDecrease_click);
                    btnDecrease.Size = new Size(45, 24);
                    btnDecrease.Location = new Point(15, 169);
                    btnDecrease.Text = "<";
                    btnDecrease.Font = new Font("Century Gothic", 10, FontStyle.Bold);
                    btnDecrease.Tag = txtQuantity;

                    Button btnIncrease = new Button();
                    btnIncrease.Click += new EventHandler(this.btnIncrease_click);
                    btnIncrease.Size = new Size(45, 24);
                    btnIncrease.Location = new Point(104, 169);
                    btnIncrease.Text = ">";
                    btnIncrease.Font = new Font("Century Gothic", 10, FontStyle.Bold);
                    btnIncrease.Tag = txtQuantity;

                    Label lblProductName = new Label();
                    //lblProductName.Name = Functions.Functions.reader["productID"].ToString();
                    lblProductName.Text = Functions.Functions.reader["productName"].ToString();
                    lblProductName.BackColor = Color.Transparent;
                    lblProductName.ForeColor = Color.White;
                    lblProductName.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                    lblProductName.AutoSize = true; // Set AutoSize to false
                    lblProductName.MaximumSize = new Size(110, 0); // Limit the width
                    lblProductName.TextAlign = ContentAlignment.MiddleCenter; // Align text to the center
                    int labelHeight = TextRenderer.MeasureText(lblProductName.Text, lblProductName.Font).Height; // Calculate label height
                    lblProductName.Location = new Point(40, 107); // Center horizontally and move down

                    Label lblPrice = new Label();
                    lblPrice.Text = "₱" + Functions.Functions.reader["productPrice"].ToString();
                    lblPrice.BackColor = Color.Transparent;
                    lblPrice.ForeColor = Color.White;
                    lblPrice.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                    lblPrice.Padding = new Padding(1);
                    lblPrice.AutoSize = true;
                    lblPrice.TextAlign = ContentAlignment.MiddleCenter;
                    lblPrice.Location = new Point(48, 148);

                    pnl.Controls.Add(btnDecrease);
                    pnl.Controls.Add(btnIncrease);
                    pnl.Controls.Add(btnAddtoCart);
                    pnl.Controls.Add(productPhoto);
                    pnl.Controls.Add(txtQuantity);
                    pnl.Controls.Add(lblPrice);
                    pnl.Controls.Add(lblProductName);
                    flowLayoutPanel2.Controls.Add(pnl);
                }
            }
        }

        public void addToCart(int productID, int quantity) 
        {
            try
            {
                if (quantity == 0)
                {
                    MessageBox.Show("Quantity should not be 0", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                

                Connection.Connection.DB();
                Functions.Functions.query = "SELECT * FROM product WHERE productID = @productID";
                Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
                Functions.Functions.command.Parameters.AddWithValue("@productID", productID);
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
                        int setproductID = Convert.ToInt32(Functions.Functions.reader["productID"].ToString());

                        ProcessOrder_Supplies.setproductId.Add(setproductID);
                        Console.WriteLine(setproductID);

                        Panel pnl = new Panel();
                        pnl.BackgroundImage = PanelImage;
                        pnl.BackgroundImageLayout = ImageLayout.Stretch;
                        pnl.Size = new Size(312, 125);

                        PictureBox pcboxProductPhoto = new PictureBox();
                        pcboxProductPhoto.Size = new Size(100, 85);
                        string filePath = Functions.Functions.reader["fileName"].ToString();
                        if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath)) // Check if the file path is not empty and the file exists
                        {
                            pcboxProductPhoto.Image = Image.FromFile(filePath); // Load image from file path
                            pcboxProductPhoto.SizeMode = PictureBoxSizeMode.StretchImage; // Set size mode
                        }
                        else
                        {
                            pcboxProductPhoto.Image = null; // Clear PictureBox if no photo found
                        }
                        pcboxProductPhoto.Location = new Point(14, 19);

                        Label lblProductName = new Label();
                        lblProductName.Text = Functions.Functions.reader["ProductName"].ToString();
                        Console.WriteLine(lblProductName.Text);
                        lblProductName.BackColor = Color.Transparent;
                        lblProductName.ForeColor = Color.White;
                        lblProductName.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                        lblProductName.AutoSize = true; // Set AutoSize to false
                        lblProductName.Location = new Point(123, 19);

                        Label lblQuantity = new Label();
                        lblQuantity.Text = quantity.ToString() + " pcs.";
                        lblQuantity.BackColor = Color.Transparent;
                        lblQuantity.ForeColor = Color.White;
                        lblQuantity.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                        lblQuantity.AutoSize = true;
                        lblQuantity.Location = new Point(123, 85);
                        if (!ProcessOrder_Supplies.productQuantities.ContainsKey(setproductID))
                        {
                            ProcessOrder_Supplies.productQuantities[setproductID] = quantity;
                        }
                        else
                        {
                            ProcessOrder_Supplies.productQuantities[setproductID] += quantity;
                        }

                        //Edited Code
                        float price = Convert.ToInt32(Functions.Functions.reader["productPrice"]);
                        float quant = quantity;
                        float finalPrice = price * quantity;
                        //Newly Added code
                        if (!ProcessOrder_Supplies.productPrices.ContainsKey(setproductID))
                        {
                            ProcessOrder_Supplies.productPrices.Add(setproductID, finalPrice);
                        }
                        else
                        {
                            ProcessOrder_Supplies.productPrices[setproductID] += finalPrice;
                        }
                        Console.WriteLine(finalPrice);
                        Console.WriteLine(finalPrice);

                        Label lblProductPrice = new Label();
                        lblProductPrice.Text = "₱" + finalPrice.ToString();
                        lblProductPrice.BackColor = Color.Transparent;
                        lblProductPrice.ForeColor = Color.White;
                        lblProductPrice.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                        lblProductPrice.AutoSize = true;
                        lblProductPrice.Location = new Point(123, 70);

                        pnl.Controls.Add(pcboxProductPhoto);
                        pnl.Controls.Add(lblProductName);
                        pnl.Controls.Add(lblProductPrice);
                        pnl.Controls.Add(lblQuantity);
                        flowLayoutPanel3.Controls.Add(pnl);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void btnDecrease_click(object sender, EventArgs e) 
        {
            Button btnDecrease = sender as Button;

            TextBox txtQuantity = btnDecrease.Tag as TextBox;

            // Decrease the quantity
            int quantity = Convert.ToInt32(txtQuantity.Text);
            if (quantity > 0)
            {
                quantity--;
                txtQuantity.Text = quantity.ToString();
            }
            else 
            {
                MessageBox.Show("Error");
            }
        }

        public void btnIncrease_click(object sender, EventArgs e) 
        {
            Button btnIncrease = sender as Button;

            // Retrieve the associated TextBox control from Tag property
            TextBox txtQuantity = btnIncrease.Tag as TextBox;

            // Increase the quantity
            int quantity = Convert.ToInt32(txtQuantity.Text);
            quantity++;
            txtQuantity.Text = quantity.ToString();
        }

        public void btnAddtoCart_click(object sender, EventArgs e)
        {
            Button btnAddtoCart = sender as Button;

            if (btnAddtoCart != null)
            {
                int productID = Convert.ToInt32(btnAddtoCart.Tag);
                // Find the corresponding TextBox by iterating through the controls of the parent Panel
                Panel parentPanel = btnAddtoCart.Parent as Panel;
                TextBox txtQuantity = null;

                foreach (Control control in parentPanel.Controls)
                {
                    if (control is TextBox)
                    {
                        txtQuantity = control as TextBox;
                        break;
                    }
                }

                if (txtQuantity != null)
                {
                    int quantity = Convert.ToInt32(txtQuantity.Text);
                    if (quantity == 0)
                    {
                        MessageBox.Show("Quantity should not be 0", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    addToCart(productID, quantity);
                }
                else
                {
                    MessageBox.Show("Quantity TextBox not found");
                }
            }
            else
            {
                MessageBox.Show("Button not found");
            }
        }
    

        private void ProcessOrder_Supplies_Load(object sender, EventArgs e)
        {
            viewProducts();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to cancel the items in the cart?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                flowLayoutPanel3.Controls.Clear();
                setproductId.Clear();
                viewProducts();
            }
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            this.Hide();
            StaffDashboard dashboard = new StaffDashboard();
            dashboard.Show();

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
                Checkout_Supplies checkout = new Checkout_Supplies();
                checkout.Show();
            }
        }
        public static void ResetOrder()
        {
            setproductId.Clear();
            productPrices.Clear();
            productQuantities.Clear();
            totalPrice = 0;
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
                viewProducts();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Connection.Connection.DB();
            Functions.Functions.query = "Select * from product where productName like '%' + @searchedProduct + '%'";
            Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
            Functions.Functions.command.Parameters.AddWithValue("@searchedProduct", txtSearch.Text);
            Functions.Functions.reader = Functions.Functions.command.ExecuteReader();

            Image buttonImage = new Bitmap(@"C:\Users\Mark Andrew\Documents\3rd Year School Stuff\IDMS\UI purposes\button1.png");
            Image PanelImage = new Bitmap(@"C:\Users\Mark Andrew\Documents\3rd Year School Stuff\IDMS\UI purposes\PanelOrder.png");

            if (Functions.Functions.reader.HasRows)
            {
                flowLayoutPanel2.FlowDirection = FlowDirection.LeftToRight;
                flowLayoutPanel2.WrapContents = true;
                flowLayoutPanel2.AutoScroll = true; // Enable auto-scrolling

                flowLayoutPanel2.HorizontalScroll.Enabled = false;
                flowLayoutPanel2.HorizontalScroll.Visible = false;

                flowLayoutPanel2.Controls.Clear();
                while (Functions.Functions.reader.Read())
                {
                    iterationCount++;

                    int productID = Convert.ToInt32(Functions.Functions.reader["productID"]);

                    Panel pnl = new Panel();
                    //pnl.BackgroundImage = Properties.Resources.Untitled_design__42_;
                    pnl.BackgroundImage = PanelImage;
                    pnl.BackgroundImageLayout = ImageLayout.Stretch;
                    pnl.Size = new Size(172, 236);
                    //pnl.Padding = new Padding(5);

                    PictureBox productPhoto = new PictureBox();
                    productPhoto.Size = new Size(141, 90);
                    productPhoto.Location = new Point(15, 11);

                    string filePath = Functions.Functions.reader["fileName"].ToString();
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
                    btnAddtoCart.Tag = productID;
                    btnAddtoCart.Size = new Size(140, 24);
                    btnAddtoCart.Location = new Point(15, 200);
                    btnAddtoCart.Text = "Add to Cart";
                    btnAddtoCart.Font = new Font("Century Gothic", 10, FontStyle.Regular);

                    TextBox txtQuantity = new TextBox();
                    txtQuantity.Font = new Font("Century Gothic", 11, FontStyle.Regular);
                    txtQuantity.Location = new Point(63, 169);
                    txtQuantity.Size = new Size(38, 26);
                    txtQuantity.Text = "0";
                    txtQuantity.TextAlign = HorizontalAlignment.Center;

                    Button btnDecrease = new Button();
                    btnDecrease.Click += new EventHandler(this.btnDecrease_click);
                    btnDecrease.Size = new Size(45, 24);
                    btnDecrease.Location = new Point(15, 169);
                    btnDecrease.Text = "<";
                    btnDecrease.Font = new Font("Century Gothic", 10, FontStyle.Bold);
                    btnDecrease.Tag = txtQuantity;

                    Button btnIncrease = new Button();
                    btnIncrease.Click += new EventHandler(this.btnIncrease_click);
                    btnIncrease.Size = new Size(45, 24);
                    btnIncrease.Location = new Point(104, 169);
                    btnIncrease.Text = ">";
                    btnIncrease.Font = new Font("Century Gothic", 10, FontStyle.Bold);
                    btnIncrease.Tag = txtQuantity;

                    Label lblProductName = new Label();
                    //lblProductName.Name = Functions.Functions.reader["productID"].ToString();
                    lblProductName.Text = Functions.Functions.reader["productName"].ToString();
                    lblProductName.BackColor = Color.Transparent;
                    lblProductName.ForeColor = Color.White;
                    lblProductName.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                    lblProductName.AutoSize = true; // Set AutoSize to false
                    lblProductName.MaximumSize = new Size(110, 0); // Limit the width
                    lblProductName.TextAlign = ContentAlignment.MiddleCenter; // Align text to the center
                    int labelHeight = TextRenderer.MeasureText(lblProductName.Text, lblProductName.Font).Height; // Calculate label height
                    lblProductName.Location = new Point(40, 107); // Center horizontally and move down

                    Label lblPrice = new Label();
                    lblPrice.Text = "₱" + Functions.Functions.reader["productPrice"].ToString();
                    lblPrice.BackColor = Color.Transparent;
                    lblPrice.ForeColor = Color.White;
                    lblPrice.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                    lblPrice.Padding = new Padding(1);
                    lblPrice.AutoSize = true;
                    lblPrice.TextAlign = ContentAlignment.MiddleCenter;
                    lblPrice.Location = new Point(48, 148);

                    pnl.Controls.Add(btnDecrease);
                    pnl.Controls.Add(btnIncrease);
                    pnl.Controls.Add(btnAddtoCart);
                    pnl.Controls.Add(productPhoto);
                    pnl.Controls.Add(txtQuantity);
                    pnl.Controls.Add(lblPrice);
                    pnl.Controls.Add(lblProductName);
                    flowLayoutPanel2.Controls.Add(pnl);
                }
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
