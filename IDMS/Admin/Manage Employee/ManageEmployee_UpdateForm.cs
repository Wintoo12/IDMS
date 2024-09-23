using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDMS.Admin.Manage_Employee
{
    public partial class ManageEmployee_UpdateForm : Form
    {
        public ManageEmployee_UpdateForm()
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

        private void ManageEmployee_UpdateForm_Load(object sender, EventArgs e)
        {

        }

        public void FillEmployeeDetails(int userID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=MARK\\SQLEXPRESS03;Initial Catalog=IDMS;Integrated Security=True"))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SELECT * FROM users WHERE userID = @userID", connection))
                    {
                        command.Parameters.AddWithValue("@userID", userID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                txtEmployeeID.Text = reader["userID"].ToString();
                                txtFName.Text = reader["FName"].ToString();
                                txtMName.Text = reader["MName"].ToString();
                                txtLName.Text = reader["LName"].ToString();
                                txtFB_acnt.Text = reader["Fb_accnt"].ToString();
                                txtContactNum.Text = reader["contact_num"].ToString();
                                txtBarangay.Text = reader["barangay"].ToString();
                                txtMunicipality.Text = reader["municipality"].ToString();
                                txtUsername.Text = reader["username"].ToString();
                                txtPassword.Text = reader["password"].ToString();
                                string status = reader["status"].ToString();
                                if (status.Equals("Active", StringComparison.OrdinalIgnoreCase))
                                {
                                    rbtnActive.Checked = true;
                                    rbtnInactive.Checked = false;
                                }
                                else if (status.Equals("Inactive", StringComparison.OrdinalIgnoreCase))
                                {
                                    rbtnInactive.Checked = true;
                                    rbtnActive.Checked = false;
                                }
                                else
                                {
                                    MessageBox.Show("Error");
                                }

                                txtFilename.Text = reader["fileName"].ToString();

                                string fileName = reader["fileName"].ToString(); // Using fileName to store the path

                                if (!string.IsNullOrEmpty(fileName))
                                {
                                    pcboxEmployeePhoto.ImageLocation = fileName;
                                }
                                else
                                {
                                    pcboxEmployeePhoto.Image = null; // Clear PictureBox if no photo found
                                }

                                Console.WriteLine(userID);
                                Console.WriteLine(txtFName.Text);
                                Console.WriteLine(txtMName.Text);
                                Console.WriteLine(txtLName.Text);
                                Console.WriteLine(txtFB_acnt.Text);
                                Console.WriteLine(txtContactNum.Text);
                                Console.WriteLine(txtBarangay.Text);
                                Console.WriteLine(txtMunicipality.Text);

                            }
                            else
                            {
                                MessageBox.Show("No data found for the selected customer.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Connection.Connection.con.Close();
                MessageBox.Show(ex.Message);
            }

        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin.AdminDashboard adminDashboard = new Admin.AdminDashboard();
            adminDashboard.Show();
        }

        private void btnUpdateEmployee_Click(object sender, EventArgs e)
        {
            try
            {
                string status = "";
                if (rbtnActive.Checked == true)
                {
                    status = rbtnActive.Text;
                    rbtnInactive.Checked = false;
                }
                else
                {
                    status = rbtnInactive.Text;
                    rbtnActive.Checked = false;
                }

                string FName = txtFName.Text;
                string MName = txtMName.Text;
                string LName = txtLName.Text;

                DialogResult result = MessageBox.Show("Please verify that the changes made are accurate.", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    Connection.Connection.DB();
                    string filename = txtFilename.Text;

                    Functions.Functions.query = "Update users set FName = @FName, MName = @MName, LName = @LName, username = @Username, password = @Password, " +
                        "barangay = @Brgy, municipality = @Municipality, fb_accnt = @FBaccnt, contact_num = @ContactNum, filename = @FileName, status = @Status, " +
                        "roleID = 2 where userID = @EmployeeID";
                    Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);

                    Functions.Functions.command.Parameters.AddWithValue("@FName", txtFName.Text);
                    Functions.Functions.command.Parameters.AddWithValue("@MName", txtMName.Text);
                    Functions.Functions.command.Parameters.AddWithValue("@LName", txtLName.Text);
                    Functions.Functions.command.Parameters.AddWithValue("@Username", txtUsername.Text);
                    Functions.Functions.command.Parameters.AddWithValue("@Password", txtPassword.Text);
                    Functions.Functions.command.Parameters.AddWithValue("@FBaccnt", txtFB_acnt.Text);
                    Functions.Functions.command.Parameters.AddWithValue("@ContactNum", txtContactNum.Text);
                    Functions.Functions.command.Parameters.AddWithValue("@Brgy", txtBarangay.Text);
                    Functions.Functions.command.Parameters.AddWithValue("@Municipality", txtMunicipality.Text);
                    Functions.Functions.command.Parameters.AddWithValue("@Status", status);
                    Functions.Functions.command.Parameters.AddWithValue("@FileName", filename);
                    Functions.Functions.command.Parameters.AddWithValue("@EmployeeID", txtEmployeeID.Text);

                    Functions.Functions.command.ExecuteNonQuery();

                    //Console.WriteLine("I don't know why it freezes");

                    MessageBox.Show("Employee information is updated in the database!", "Updated!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Connection.Connection.con.Close();



                    this.Hide();
                    ManageEmployee empDashboard = new ManageEmployee();

                    empDashboard.Show();
                }
                else
                {
                    MessageBox.Show("Error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin.ManageEmployee employeeDashboard = new ManageEmployee();
            employeeDashboard.Show();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFD = new OpenFileDialog() { Filter = "Image Files (*.jpg; *.jpeg)| *.jpg;*.jpeg", Multiselect = false })
            {
                if (openFD.ShowDialog() == DialogResult.OK)
                {
                    pcboxEmployeePhoto.Image = Image.FromFile(openFD.FileName);
                    txtFilename.Text = openFD.FileName;
                }
            }
        }
    }
}
