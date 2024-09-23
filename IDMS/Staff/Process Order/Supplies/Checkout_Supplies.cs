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

namespace IDMS.Staff.Process_Order.Supplies
{
    public partial class Checkout_Supplies : Form
    {
        float totalPrice = 0;
        public Checkout_Supplies()
        {
            InitializeComponent();
        }

        public void viewCheckout()
        {
            try
            {
                foreach (int productID in ProcessOrder_Supplies.setproductId)
                {
                    Connection.Connection.DB();
                    Functions.Functions.query = "SELECT * FROM product WHERE productID = @ProductID";
                    Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
                    Functions.Functions.command.Parameters.AddWithValue("@ProductID", productID);
                    Functions.Functions.reader = Functions.Functions.command.ExecuteReader();

                    Console.WriteLine(ProcessOrder_Supplies.setproductId);

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
                            pnl.Size = new Size(355, 125);
                            pnl.Tag = productID; // Store product ID in the Tag property of the panel

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
                            lblProductName.BackColor = Color.Transparent;
                            lblProductName.ForeColor = Color.White;
                            lblProductName.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                            lblProductName.AutoSize = true;
                            lblProductName.Location = new Point(123, 19);

                            Label lblQuantity = new Label();
                            lblQuantity.Name = "lblQuantity"; // Give the label a name to find it later
                            if (ProcessOrder_Supplies.productQuantities.ContainsKey(productID))
                            {
                                lblQuantity.Text = ProcessOrder_Supplies.productQuantities[productID].ToString() + " pcs.";
                            }
                            else
                            {
                                lblQuantity.Text = "0 pcs.";
                            }
                            lblQuantity.BackColor = Color.Transparent;
                            lblQuantity.ForeColor = Color.White;
                            lblQuantity.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                            lblQuantity.AutoSize = true;
                            lblQuantity.Location = new Point(123, 85);

                            Label lblProductPrice = new Label();
                            float productPrice = 0;
                            if (ProcessOrder_Supplies.productPrices.ContainsKey(productID))
                            {
                                productPrice = ProcessOrder_Supplies.productPrices[productID];
                                lblProductPrice.Text = "₱ " + productPrice.ToString("N2");
                            }
                            else
                            {
                                lblProductPrice.Text = "₱0.00";
                            }
                            lblProductPrice.BackColor = Color.Transparent;
                            lblProductPrice.ForeColor = Color.White;
                            lblProductPrice.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                            lblProductPrice.AutoSize = true;
                            lblProductPrice.Location = new Point(123, 70);

                            pnl.Controls.Add(pcboxProductPhoto);
                            pnl.Controls.Add(lblProductName);
                            pnl.Controls.Add(lblProductPrice);
                            pnl.Controls.Add(lblQuantity);
                            flowLayoutPanel2.Controls.Add(pnl);

                            totalPrice += productPrice;

                            Panel pnl2 = new Panel();
                            pnl2.BackgroundImage = stockImage;
                            pnl2.BackgroundImageLayout = ImageLayout.Stretch;
                            pnl2.Size = new Size(491, 37);

                            Label lblProductName2 = new Label();
                            lblProductName2.Text = Functions.Functions.reader["ProductName"].ToString();
                            lblProductName2.BackColor = Color.Transparent;
                            lblProductName2.ForeColor = Color.White;
                            lblProductName2.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                            lblProductName2.AutoSize = true;
                            lblProductName2.Location = new Point(5, 7);

                            Label lblProductPrice2 = new Label();
                            float productPrice2 = 0;
                            if (ProcessOrder_Supplies.productPrices.ContainsKey(productID))
                            {
                                productPrice2 = ProcessOrder_Supplies.productPrices[productID];
                                lblProductPrice2.Text = "₱ " + productPrice2.ToString("N2");
                            }
                            else
                            {
                                lblProductPrice2.Text = "₱0";
                            }
                            lblProductPrice2.BackColor = Color.Transparent;
                            lblProductPrice2.ForeColor = Color.White;
                            lblProductPrice2.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                            lblProductPrice2.AutoSize = true;
                            lblProductPrice2.Location = new Point(378, 7);

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

        private void Checkout_Supplies_Load(object sender, EventArgs e)
        {
            viewCheckout();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to cancel the items in the cart?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                ProcessOrder_Supplies.ResetOrder();
                this.Hide();
                ProcessOrder_Supplies order = new ProcessOrder_Supplies();
                order.Show();
            }
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            try
            {

                // Loop through each panel to process quantities and calculate the total price
                foreach (Panel pnl in flowLayoutPanel2.Controls.OfType<Panel>())
                {
                    Label lblQuantity = pnl.Controls.OfType<Label>().FirstOrDefault(l => l.Name == "lblQuantity");
                    if (lblQuantity != null && pnl.Tag != null)
                    {
                        int productID = (int)pnl.Tag;
                        int quantity = int.Parse(lblQuantity.Text.Replace(" pcs.", ""));

                        if (ProcessOrder_Supplies.productPrices.ContainsKey(productID))
                        {
                            float productPrice = ProcessOrder_Supplies.productPrices[productID];

                            Console.WriteLine(productID);
                            Console.WriteLine(productPrice);
                            Console.WriteLine(quantity);
                            // Perform the database insertion for each productID and quantity
                            Connection.Connection.DB();
                            Functions.Functions.query = "INSERT INTO sales (sales_quantity, productID, packageID, dateSold, totalPrice) VALUES (@SalesQuantity, @ProductID, null, @DateSold, @TotalPrice) UPDATE product SET product_quantity = product_quantity - @SalesQuantity WHERE productID = @ProductID";
                            Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
                            Functions.Functions.command.Parameters.AddWithValue("@SalesQuantity", quantity);
                            Functions.Functions.command.Parameters.AddWithValue("@ProductID", productID);
                            Functions.Functions.command.Parameters.AddWithValue("@DateSold", DateTime.Now);
                            Functions.Functions.command.Parameters.AddWithValue("@TotalPrice", productPrice);
                            Functions.Functions.command.ExecuteNonQuery();

                        }
                    }
                }
                MessageBox.Show("The sales have been recorded", "Sold!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
                ProcessOrder_Supplies supplies = new ProcessOrder_Supplies();
                supplies.Show();

                // After the loop, update the total price label
                //Console.WriteLine(totalPrice);

                Connection.Connection.DB();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
       
