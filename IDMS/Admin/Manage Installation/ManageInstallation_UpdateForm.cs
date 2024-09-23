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

namespace IDMS.Admin.Manage_Installation
{
    public partial class ManageInstallation_UpdateForm : Form
    {
        private int PackageID;
        public ManageInstallation_UpdateForm(int packageID)
        {
            InitializeComponent();
            PackageID = packageID;
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

        private void ManageInstallation_UpdateForm_Load(object sender, EventArgs e)
        {
            FillInstallationDetails(PackageID);
        }

        public void FillInstallationDetails(int packageID) 
        {
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
                                reader.Read();
                                txtPackageID.Text = reader["packageID"].ToString();
                                txtPackageName.Text = reader["packageName"].ToString();
                                txtCapacity.Text = reader["capacity"].ToString();
                                txtMachineType.Text = reader["type"].ToString();
                                txtTotalPrice.Text = reader["totalPrice"].ToString();
                                txtDownPayment.Text = reader["downPayment"].ToString();
                                txtWarranty.Text = reader["warranty"].ToString();
                                string status = reader["status"].ToString();
                                if (status.Equals("Available", StringComparison.OrdinalIgnoreCase))
                                {
                                    rbtnAvailable.Checked = true;
                                    rbtnNotAvailable.Checked = false;
                                }
                                else if (status.Equals("Not Available", StringComparison.OrdinalIgnoreCase))
                                {
                                    rbtnNotAvailable.Checked = true;
                                    rbtnAvailable.Checked = false;
                                }
                                else
                                {
                                    MessageBox.Show("Error");
                                }

                                txtFileName.Text = reader["fileName"].ToString();

                                string fileName = reader["fileName"].ToString(); // Using fileName to store the path

                                if (!string.IsNullOrEmpty(fileName))
                                {
                                    pcboxPackagePhoto.ImageLocation = fileName;
                                }
                                else
                                {
                                    pcboxPackagePhoto.Image = null; // Clear PictureBox if no photo found
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
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                string status = "";
                if (rbtnAvailable.Checked == true)
                {
                    status = rbtnAvailable.Text;
                    rbtnNotAvailable.Checked = false;
                }
                else
                {
                    status = rbtnNotAvailable.Text;
                    rbtnAvailable.Checked = false;
                }

                DialogResult result = MessageBox.Show("Please confirm if the information that has been updated is correct and true.", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                {
                    Connection.Connection.DB();

                    string filename = txtFileName.Text; // Get the file path directly

                    Functions.Functions.query = "UPDATE package SET packageName = @packageName, capacity = @capacity, totalPrice = @TotalPrice, downPayment = @DownPayment, warranty = @Warranty, " +
                                                "fileName = @fileName, type = @Type, status = @status " +
                                                "WHERE packageID = @packageID";

                    Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);

                    Functions.Functions.command.Parameters.AddWithValue("@packageName", txtPackageName.Text);
                    Functions.Functions.command.Parameters.AddWithValue("@capacity", txtCapacity.Text);
                    Functions.Functions.command.Parameters.AddWithValue("@TotalPrice", Convert.ToDouble(txtTotalPrice.Text));
                    Functions.Functions.command.Parameters.AddWithValue("@DownPayment", Convert.ToDouble(txtDownPayment.Text));
                    Functions.Functions.command.Parameters.AddWithValue("@Warranty", txtWarranty.Text); 
                    Functions.Functions.command.Parameters.AddWithValue("@Type", txtMachineType.Text);
                    Functions.Functions.command.Parameters.AddWithValue("@fileName", filename);
                    Functions.Functions.command.Parameters.AddWithValue("@status", status);
                    Functions.Functions.command.Parameters.AddWithValue("@packageID", txtPackageID.Text);

                    Functions.Functions.command.ExecuteNonQuery();

                    MessageBox.Show("Package information is updated in the database!", "Updated!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Connection.Connection.con.Close();

                    int packageID = Convert.ToInt32(txtPackageID.Text);
                    this.Hide();
                    ManageInstallation_UpdateForm_2 update = new ManageInstallation_UpdateForm_2(packageID);
                    update.Show();
                }
                else
                {
                    this.Show();
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show("Error! " + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFD = new OpenFileDialog() { Filter = "Image Files (*.jpg; *.jpeg)| *.jpg;*.jpeg", Multiselect = false })
            {
                if (openFD.ShowDialog() == DialogResult.OK)
                {
                    pcboxPackagePhoto.Image = Image.FromFile(openFD.FileName);
                    txtFileName.Text = openFD.FileName;
                }
            }
        }
    }
}
