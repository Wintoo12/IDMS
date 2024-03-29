using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IDMS.Functions;

namespace IDMS
{
    public partial class ManageSupply_ViewStatusListAdmin : Form
    {

        public static int iterationCount = 0;
        public ManageSupply_ViewStatusListAdmin()
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
                    lblProductQuantity.Text = Functions.Functions.reader["product_quantity"].ToString() + "/1000";
                    int stock = Convert.ToInt32(Functions.Functions.reader["product_quantity"]);
                    lblProductQuantity.BackColor = Color.Transparent;
                    lblProductQuantity.ForeColor = Color.White;
                    lblProductQuantity.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                    lblProductQuantity.AutoSize = true;
                    lblProductQuantity.Location = new Point(230, 2);
                    lblProductQuantity.BringToFront();

                    Button btnRestock = new Button();
                    btnRestock.Click += new EventHandler(this.button_click);
                    btnRestock.Text = "Restock";
                    btnRestock.AutoSize = true;
                    btnRestock.BackColor = Color.Transparent;
                    btnRestock.ForeColor = Color.White;
                    btnRestock.FlatAppearance.BorderSize = 0;
                    btnRestock.BackgroundImage = btnImage;
                    btnRestock.BackgroundImageLayout = ImageLayout.Stretch;
                    btnRestock.FlatStyle = FlatStyle.Flat;
                    btnRestock.Font = new Font("Century Gothic", 10, FontStyle.Bold);
                    btnRestock.Location = new Point(814, 7);
                    btnRestock.Enabled = false;

                    pnlBack.Controls.Add(lblProductName);
                    pnlBack.Controls.Add(pnlStatusBar);
                    pnlStatusBar.Controls.Add(lblProductQuantity);
                    pnlBack.Controls.Add(btnRestock);
                    flowLayoutPanel2.Controls.Add(pnlBack);
                    flowLayoutPanel2.FlowDirection = FlowDirection.TopDown;

                    colorIndicator(stock, pnlStatusBar, lblProductQuantity, btnRestock);
                    //colorIndicator(stock, lblProductQuantity, btnRestock);
                }
            }
        }

        public void colorIndicator(int stock, Panel pnlStatusBar ,Label lblProductQuantity, Button btnRestock)
        {

            if (stock >= 0 && stock <= 249)
            {
                pnlStatusBar.BackColor = Color.FromArgb(239, 1, 1);
                //lblProductQuantity.Text = Convert.ToString(stock) + " " + "/1000";
                btnRestock.Enabled = true;
                pnlStatusBar.Size = new Size(537, 23);
                pnlStatusBar.Location = new Point(215, 9);
            }
            //Orange
            else if (stock >= 250 && stock <= 499)
            {
                pnlStatusBar.BackColor = Color.FromArgb(239, 179, 1);
                lblProductQuantity.Text = Convert.ToString(stock) + " " + "/1000";
                btnRestock.BackgroundImage = null;
                btnRestock.BackColor = Color.Gray;
                pnlStatusBar.Size = new Size(537, 23);
                pnlStatusBar.Location = new Point(215, 9);
            }
            //Yellow
            else if (stock >= 500 && stock <= 749)
            {
                pnlStatusBar.BackColor = Color.FromArgb(239, 239, 1);
                lblProductQuantity.Text = Convert.ToString(stock) + " " + "/1000";
                btnRestock.BackColor = Color.Gray;
                btnRestock.BackgroundImage = null;
                pnlStatusBar.Size = new Size(537, 23);
                pnlStatusBar.Location = new Point(215, 9);
            }
            //Teal
            else if (stock >= 750 && stock <= 999)
            {
                pnlStatusBar.BackColor = Color.FromArgb(0, 150, 137);
                lblProductQuantity.Text = Convert.ToString(stock) + " " + "/1000";
                btnRestock.BackColor = Color.Gray;
                btnRestock.BackgroundImage = null;
                pnlStatusBar.Size = new Size(537, 23);
                pnlStatusBar.Location = new Point(215, 9);
            }
            //Green
            else if (stock == 1000)
            {
                pnlStatusBar.BackColor = Color.FromArgb(1, 237, 3);
                lblProductQuantity.Text = Convert.ToString(stock) + " " + "/1000";
                btnRestock.BackColor = Color.Gray;
                btnRestock.BackgroundImage = null;
                pnlStatusBar.Size = new Size(537, 23);
                pnlStatusBar.Location = new Point(215, 9);
            }
        }

        public void button_click(object sender, EventArgs e) 
        {
            Button btn = sender as Button;
            ManageSupply_UpdateForm update = new ManageSupply_UpdateForm();
            update.Show();
        }

        private void ManageSupply_ViewStatusListAdmin_Load(object sender, EventArgs e)
        {
            viewProduct();
        }
    }
}
