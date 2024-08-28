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
using System.Xml.Linq;

namespace IDMS.Admin.Manage_Installation
{
    public partial class ManageInstallation_UpdateForm_2 : Form
    {
        private List<Item> items;
        private int currentItemIndex = -1;
        private int PackageID = 0;
        public ManageInstallation_UpdateForm_2(int packageID)
        {
            InitializeComponent();
            LoadItems(packageID);

            PackageID = packageID;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
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

        private void LoadItems(int packageID)
        {
            DataAccess dataAccess = new DataAccess();
            items = dataAccess.GetItems(packageID);
            if (items.Count > 0)
            {
                currentItemIndex = 0;
                DisplayCurrentItem();
            }
        }

        private void DisplayCurrentItem()
        {
            if (currentItemIndex >= 0 && currentItemIndex < items.Count)
            {
                Item currentItem = items[currentItemIndex];
                txtComponentID.Text = currentItem.ID.ToString();
                txtProductName.Text = currentItem.Name;
                txtQuantity.Text = currentItem.Quantity.ToString();
                txtUnit.Text = currentItem.Unit;
                txtDescription.Text = currentItem.Description;
                txtPackageID.Text = currentItem.PackageID.ToString();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (currentItemIndex >= 0 && currentItemIndex < items.Count)
            {
                Item currentItem = items[currentItemIndex];
                try
                {
                    currentItem.Name = txtProductName.Text;
                    currentItem.Quantity = int.Parse(txtQuantity.Text);
                    currentItem.Unit = txtUnit.Text;
                    currentItem.Description = txtDescription.Text;
                    currentItem.PackageID = int.Parse(txtPackageID.Text);

                    if (string.IsNullOrWhiteSpace(currentItem.Name) ||
                        string.IsNullOrWhiteSpace(currentItem.Unit) ||
                        string.IsNullOrWhiteSpace(currentItem.Description))
                    {
                        MessageBox.Show("Please fill in all fields.");
                        return;
                    }

                    UpdateItem(currentItem);
                }
                catch (FormatException)
                {
                    MessageBox.Show("Please enter valid values for quantity and package ID.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while updating the item: {ex.Message}\n{ex.StackTrace}");
                }
            }
        }

        private void UpdateItem(Item item)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=MARK\\SQLEXPRESS03;Initial Catalog=IDMS;Integrated Security=True"))
                {
                    string query = @"
                    UPDATE component 
                    SET componentName = @Name, 
                        componentQuantity = @Quantity, 
                        componentUnit = @Unit, 
                        componentDescription = @Description 
                    WHERE packageID = @packageID AND componentID = @ID";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Name", item.Name);
                    command.Parameters.AddWithValue("@Quantity", item.Quantity);
                    command.Parameters.AddWithValue("@Unit", item.Unit);
                    command.Parameters.AddWithValue("@Description", item.Description);
                    command.Parameters.AddWithValue("@ID", item.ID);
                    command.Parameters.AddWithValue("@packageID", item.PackageID);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        MessageBox.Show("No rows were updated. Please check if the item exists.");
                    }
                    else
                    {
                        MessageBox.Show("Item updated successfully.");
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"SQL error occurred while updating the item: {sqlEx.Message}\n{sqlEx.StackTrace}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while updating the item: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to cancel update?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes) 
            {
                this.Hide();
                ManageInstallation manageInstallation = new ManageInstallation();
                manageInstallation.Show();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageInstallation_UpdateForm_3 updateForm_3 = new ManageInstallation_UpdateForm_3(PackageID);
            updateForm_3.Show();
        }
    }
}
