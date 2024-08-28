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

namespace IDMS.Admin
{
    public partial class AddEmployee : Form
    {
        public AddEmployee()
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

        private void Manage_Employee_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            Connection.Connection.DB();

            // Define the query with parameters
            string query = "SELECT FName, LName FROM users WHERE FName = @FName AND LName = @LName";
            using (SqlCommand command = new SqlCommand(query, Connection.Connection.con))
            {
                // Add parameters to the command
                command.Parameters.AddWithValue("@FName", txtFName.Text);
                command.Parameters.AddWithValue("@LName", txtLName.Text);

                try
                {
                    // Execute the reader
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            string Fname = reader["FName"].ToString();
                            string Lname = reader["LName"].ToString();

                            if (Fname == txtFName.Text && Lname == txtLName.Text)
                            {
                                MessageBox.Show("This employee is already saved in the database! Please enter a new employee.", "Invalid!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                clear();;
                            }
                        }
                        else
                        {
                            // Insert new user
                            InsertNewUser();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void InsertNewUser()
        {
            string FName = txtFName.Text;
            string MName = txtMName.Text;
            string LName = txtLName.Text;

            Connection.Connection.DB();

            string filename = txtFilename.Text;

            string query = "INSERT INTO users (FName, MName, LName, username, password, barangay, municipality, fb_accnt, contact_num, fileName, status, roleID) " +
                           "VALUES (@FName, @MName, @LName, @Username, @Password, @Barangay, @Municipality, @FBAccnt, @ContactNum, @Filename, 'Active', 2)";
            using (SqlCommand command = new SqlCommand(query, Connection.Connection.con))
            {
                // Add parameters to the command
                command.Parameters.AddWithValue("@FName", char.ToUpper(FName[0]) + FName.Substring(1));
                command.Parameters.AddWithValue("@MName", char.ToUpper(MName[0]) + MName.Substring(1));
                command.Parameters.AddWithValue("@LName", char.ToUpper(LName[0]) + LName.Substring(1));
                command.Parameters.AddWithValue("@Username", txtUsername.Text);
                command.Parameters.AddWithValue("@Password", txtPassword.Text);
                command.Parameters.AddWithValue("@Barangay", txtBarangay.Text);
                command.Parameters.AddWithValue("@Municipality", txtMunicipality.Text);
                command.Parameters.AddWithValue("@FBAccnt", txtFB_accnt.Text);
                command.Parameters.AddWithValue("@ContactNum", txtContactNum.Text);
                command.Parameters.AddWithValue("@Filename", filename);

                try
                {
                    // Execute the query
                    command.ExecuteNonQuery();
                    MessageBox.Show("Customer Information is saved in the database!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Close the connection
                    Connection.Connection.con.Close();

                    this.Hide();
                    Admin.ManageEmployee employeeDashboard = new Admin.ManageEmployee();
                    employeeDashboard.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Connection.Connection.con.Close();
                }
            }
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


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin.ManageEmployee manage = new ManageEmployee();
            manage.Show();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin.AdminDashboard adminDashboard = new AdminDashboard();
            adminDashboard.Show();
        }

        private void btnSupplies_Click(object sender, EventArgs e)
        {

        }

        public void clear()
        {
            txtFName.Clear();
            txtMName.Clear();
            txtLName.Clear();
            txtMunicipality.Clear();
            txtBarangay.Clear();
            txtUsername.Clear();
            txtPassword.Clear();
            txtContactNum.Clear();
            txtFilename.Clear();
            txtFB_accnt.Clear();
            pcboxEmployeePhoto.Image = null;
        }
            
        
    }
}

