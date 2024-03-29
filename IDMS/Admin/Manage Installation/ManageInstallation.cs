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

namespace IDMS.Admin
{
    public partial class ManageInstallation : Form
    {
        public static int iterationCount = 0;

        public ManageInstallation()
        {
            InitializeComponent();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.Show();
        }

        private void ManageInstallation_Load(object sender, EventArgs e)
        {
            btnInstallations.ForeColor = Color.Black;
            btnInstallations.BackColor = Color.White;
            btnInstallations.Font = new Font("Century Gothic", 14, FontStyle.Italic);

            addProduct();
        }

        private void btnInstallations_Click(object sender, EventArgs e)
        {

        }

        private void btnAddForm_Click(object sender, EventArgs e)
        {
            this.Hide();
            Manage_Installation.ManageInstallation_AddForm addForm = new Manage_Installation.ManageInstallation_AddForm();
            addForm.Show();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin.AdminDashboard dashboard = new Admin.AdminDashboard();
            dashboard.Show();
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
    }
}
