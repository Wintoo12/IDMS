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
using IDMS.Admin.Manage_Installation;

namespace IDMS.Staff.Manage_Installation
{
    public partial class ManageInstallation_ViewDetailsStaff : Form
    {
        private int PackageID;
        public ManageInstallation_ViewDetailsStaff()
        {
            InitializeComponent();
        }

        private void ManageInstallation_ViewDetailsStaff_Load(object sender, EventArgs e)
        {
            fillDataComponents(PackageID);
            fillDataFreeItem(PackageID);
        }

        public void FillInstallationDetails(int packageID)
        {
            PackageID = packageID;
            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=MARK\\SQLEXPRESS03;Initial Catalog=IDMS;Integrated Security=True"))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SELECT * FROM package WHERE packageID = @packageID", connection))
                    {
                        command.Parameters.AddWithValue("@packageID", packageID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                if (reader.Read()) // Ensure Read() is called only if there are rows
                                {
                                    lblPName.Text = reader["packageName"].ToString();
                                    lblCapacity.Text = reader["capacity"].ToString();
                                    lblType.Text = reader["type"].ToString();
                                    float price = Convert.ToSingle(reader["totalPrice"]);
                                    lblPrice.Text = "₱" + price.ToString("N2");
                                    float downPayment = Convert.ToSingle(reader["downPayment"]);
                                    lblDownPayment.Text = "₱" + downPayment.ToString("N2");
                                    lblWarranty.Text = reader["warranty"].ToString();
                                    string status = reader["status"].ToString();

                                    string filePath = reader["fileName"].ToString(); // Corrected to use reader

                                    if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                                    {
                                        pcboxPackagePhoto.Image = Image.FromFile(filePath);
                                        pcboxPackagePhoto.SizeMode = PictureBoxSizeMode.StretchImage;
                                    }
                                    else
                                    {
                                        pcboxPackagePhoto.Image = null;
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("No data found for the selected package.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void fillDataComponents(int packageID)
        {
            Connection.Connection.DB();
            Functions.Functions.query = "Select component.componentName as [Component Name], component.componentQuantity as [Component Quantity], component.componentUnit as [Component Unit], component.componentDescription as [Component Description] from component where packageID = @packageID";
            Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
            Functions.Functions.command.Parameters.AddWithValue("@packageID", packageID);
            SqlDataAdapter adapter = new SqlDataAdapter(Functions.Functions.command);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dgvComponents.DataSource = dt;
        }

        public void fillDataFreeItem(int packageID)
        {
            Connection.Connection.DB();
            Functions.Functions.query = "Select freeItem.ItemName as [Item Name], freeItem.quantity as [Quantity], freeItem.unit as [Unit], freeItem.itemDescription as [Description] from freeItem where packageID = @packageID";
            Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
            Functions.Functions.command.Parameters.AddWithValue("@packageID", packageID);
            SqlDataAdapter adapter = new SqlDataAdapter(Functions.Functions.command);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dgvFreeItem.DataSource = dt;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageInstallation_Staff intstallation = new ManageInstallation_Staff();
            intstallation.Show();
        }
    }
}
