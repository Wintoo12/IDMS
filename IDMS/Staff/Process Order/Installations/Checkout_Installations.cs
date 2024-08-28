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
using System.IO;

namespace IDMS.Staff.Process_Order.Installations
{
    public partial class Checkout_Installations : Form
    {
        private List<int> packageIDs = new List<int>();
        private float totalPrice = 0;
        public Checkout_Installations()
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

        public void viewCheckout()
        {
            try
            {
                packageIDs.Clear();  // Clear previous data
                totalPrice = 0;      // Reset total price

                foreach (int packageID in Process_Order_Installations.setpackageId)
                {
                    Connection.Connection.DB();
                    Functions.Functions.query = "SELECT * FROM package WHERE packageID = @packageID";
                    Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
                    Functions.Functions.command.Parameters.AddWithValue("@packageID", packageID);
                    Functions.Functions.reader = Functions.Functions.command.ExecuteReader();
                        
                    Console.WriteLine(Process_Order_Installations.setpackageId);

                    Image buttonImage = new Bitmap(@"C:\Users\Mark Andrew\Documents\3rd Year School Stuff\IDMS\UI purposes\button1.png");
                    Image PanelImage = new Bitmap(@"C:\Users\Mark Andrew\Documents\3rd Year School Stuff\IDMS\UI purposes\itemsAdded.png");
                    Image stockImage = new Bitmap(@"C:\Users\Mark Andrew\Documents\3rd Year School Stuff\IDMS\UI purposes\Stocks1.png");

                    if (Functions.Functions.reader.HasRows)
                    {
                        flowLayoutPanel2.FlowDirection = FlowDirection.TopDown;
                        flowLayoutPanel2.WrapContents = true;
                        flowLayoutPanel2.AutoScroll = true;

                        flowLayoutPanel2.HorizontalScroll.Enabled = false;
                        flowLayoutPanel2.HorizontalScroll.Visible = false;
                        flowLayoutPanel2.WrapContents = false;

                        flowLayoutPanel3.FlowDirection = FlowDirection.TopDown;
                        flowLayoutPanel3.WrapContents = true;
                        flowLayoutPanel3.AutoScroll = true;

                        flowLayoutPanel3.HorizontalScroll.Enabled = false;
                        flowLayoutPanel3.HorizontalScroll.Visible = false;
                        flowLayoutPanel3.WrapContents = false;

                        while (Functions.Functions.reader.Read())
                        {
                            Panel pnl = new Panel();
                            pnl.BackgroundImage = PanelImage;
                            pnl.BackgroundImageLayout = ImageLayout.Stretch;
                            pnl.Size = new Size(355, 140);
                            pnl.Tag = packageID; // Store product ID in the Tag property of the panel

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
                            pcboxProductPhoto.Location = new Point(14, 24);

                            Label lblProductName = new Label();
                            lblProductName.Text = Functions.Functions.reader["packageName"].ToString();
                            lblProductName.BackColor = Color.Transparent;
                            lblProductName.ForeColor = Color.White;
                            lblProductName.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                            lblProductName.AutoSize = false; // Set AutoSize to false
                            lblProductName.MaximumSize = new Size(190, 0); // Limit the width
                            lblProductName.AutoEllipsis = true; // Show ellipsis when text overflows
                            lblProductName.TextAlign = ContentAlignment.MiddleLeft; // Align text to the left
                            lblProductName.Size = new Size(280, lblProductName.PreferredHeight);
                            lblProductName.Location = new Point(123, 19);

                            Label lblProductPrice = new Label();
                            float productPrice = Convert.ToSingle(Functions.Functions.reader["totalPrice"]);
                            lblProductPrice.Text = "₱" + productPrice.ToString("N2");
                            lblProductPrice.BackColor = Color.Transparent;
                            lblProductPrice.ForeColor = Color.White;
                            lblProductPrice.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                            lblProductPrice.AutoSize = true;
                            lblProductPrice.Location = new Point(123, 105);

                            pnl.Controls.Add(pcboxProductPhoto);
                            pnl.Controls.Add(lblProductName);
                            pnl.Controls.Add(lblProductPrice);
                            flowLayoutPanel2.Controls.Add(pnl);

                            totalPrice += productPrice;
                            packageIDs.Add(packageID);

                            Panel pnl2 = new Panel();
                            pnl2.BackgroundImage = stockImage;
                            pnl2.BackgroundImageLayout = ImageLayout.Stretch;
                            pnl2.Size = new Size(491, 75);

                            Label lblProductName2 = new Label();
                            lblProductName2.Text = Functions.Functions.reader["packageName"].ToString();
                            lblProductName2.BackColor = Color.Transparent;
                            lblProductName2.ForeColor = Color.White;
                            lblProductName2.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                            lblProductName2.AutoSize = false; // Set AutoSize to false
                            lblProductName2.MaximumSize = new Size(280, 0); // Limit the width
                            lblProductName2.AutoEllipsis = true; // Show ellipsis when text overflows
                            lblProductName2.TextAlign = ContentAlignment.MiddleLeft; // Align text to the left
                            lblProductName2.Size = new Size(320, lblProductName.PreferredHeight);
                            lblProductName2.Location = new Point(5, 1);

                            Label lblProductPrice2 = new Label();
                            float productPrice2 = Convert.ToSingle(Functions.Functions.reader["totalPrice"]);
                            lblProductPrice2.Text = "₱" + productPrice2.ToString("N2");
                            lblProductPrice2.BackColor = Color.Transparent;
                            lblProductPrice2.ForeColor = Color.White;
                            lblProductPrice2.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                            lblProductPrice2.AutoSize = true;
                            lblProductPrice2.Location = new Point(378, 28);

                            pnl2.Controls.Add(lblProductPrice2);
                            pnl2.Controls.Add(lblProductName2);
                            flowLayoutPanel3.Controls.Add(pnl2);
                        }
                    }
                }

                lblTotalPrice.Text = totalPrice.ToString("N2");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Checkout_Installations_Load(object sender, EventArgs e)
        {
            viewCheckout();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to cancel the items in the cart?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                Process_Order_Installations.ResetOrder();
                this.Hide();
                Process_Order_Installations cancel = new Process_Order_Installations();
                cancel.ShowDialog();
            }
        }

        private void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            try
            {
                Connection.Connection.DB();

                foreach (int packageID in packageIDs)
                {
                    Functions.Functions.query = "Insert into sales(sales_quantity, productID, packageID, dateSold, totalPrice) values(1, null, @packageID, @DateSold, @totalPrice)";
                    Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
                    Functions.Functions.command.Parameters.AddWithValue("@packageID", packageID);
                    Functions.Functions.command.Parameters.AddWithValue("@DateSold", DateTime.Now);
                    Functions.Functions.command.Parameters.AddWithValue("@totalPrice", totalPrice);
                    Functions.Functions.command.ExecuteNonQuery();

                    Console.WriteLine(packageID);
                    Console.WriteLine(totalPrice);
                }
                MessageBox.Show("The sales have been recorded", "Sold!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
                Process_Order_Installations order = new Process_Order_Installations();
                order.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
