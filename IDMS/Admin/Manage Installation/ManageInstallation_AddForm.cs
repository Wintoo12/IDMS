using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDMS.Admin.Manage_Installation
{
    public partial class ManageInstallation_AddForm : Form
    {
        private int PackageID;

        public ManageInstallation_AddForm()
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

        private void ManageInstallation_AddForm_Load(object sender, EventArgs e)
        {
            btnInstallations.ForeColor = Color.Black;
            btnInstallations.BackColor = Color.White;
            btnInstallations.Font = new Font("Century Gothic", 14, FontStyle.Italic);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin.ManageInstallation installation = new Admin.ManageInstallation();
            installation.Show();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                Connection.Connection.DB();
                Functions.Functions.query = "Select packageName from package where packageName = @packageName";
                Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
                Functions.Functions.command.Parameters.AddWithValue("@packageName", txtProductName.Text);
                Functions.Functions.reader = Functions.Functions.command.ExecuteReader();

                if (Functions.Functions.reader.HasRows)
                {
                    Functions.Functions.reader.Read();
                    string packageName = Functions.Functions.reader["packageName"].ToString();

                    if (packageName == txtProductName.Text)
                    {
                        MessageBox.Show("This package name is already saved in the database! Please enter a new employee.", "Invalid!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        clear();
                    }
                }
                else 
                {
                    InsertNewPackage();
                    this.Hide();
                    ManageInstallation_AddForm_2 addForm_2 = new ManageInstallation_AddForm_2(PackageID);
                    addForm_2.Show();
                }

            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void InsertNewPackage() 
        {
            try
            {
                Connection.Connection.DB();
                
                string fileName = txtFileName.Text;
                Functions.Functions.query = "Insert into package (packageName, capacity, type, totalPrice, downPayment, warranty, fileName, status) " +
                    "values(@packageName, @capacity, @Type, @TotalPrice, @DownPayment, @Warranty, @fileName, 'Available')";
                Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);

                Functions.Functions.command.Parameters.AddWithValue("@packageName", txtProductName.Text);
                Functions.Functions.command.Parameters.AddWithValue("@capacity", txtCapacity.Text);
                Functions.Functions.command.Parameters.AddWithValue("@Type", txtMachineType.Text);
                Functions.Functions.command.Parameters.AddWithValue("@TotalPrice", Convert.ToDouble(txtTotalPrice.Text));
                Functions.Functions.command.Parameters.AddWithValue("@DownPayment", Convert.ToDouble(txtDownPayment.Text));
                Functions.Functions.command.Parameters.AddWithValue("@Warranty", txtWarranty.Text);
                Functions.Functions.command.Parameters.AddWithValue("@fileName", txtFileName.Text);

                PackageID = Convert.ToInt32(Functions.Functions.command.ExecuteScalar());
                Functions.Functions.command.ExecuteNonQuery();
                Connection.Connection.con.Close();
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void clear() 
        {
            pcboxPackagePhoto.Image = null;
            txtProductName.Clear();
            txtCapacity.Clear();
            txtMachineType.Clear();
            txtTotalPrice.Clear();
            txtDownPayment.Clear();
            txtWarranty.Clear();
            txtWarranty.Clear();
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
