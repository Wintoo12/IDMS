using IDMS.Admin;
using IDMS.Admin.Manage_Installation;
using IDMS.Staff.Manage_Customer;
using IDMS.Staff.Process_Order.Installations;
using IDMS.Staff.Process_Order.Supplies;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDMS
{
    public partial class StaffDashboard : Form
    {
        bool orderExpand = false;
        public StaffDashboard()
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

        private void StaffDashboard_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = "WELCOME, " + Login.setFName + " " + Login.setLName;
        }

        private void btnSupplies_Click(object sender, EventArgs e)
        {
            pnlSupplies.Visible = true;
        }

        private void btnViewCustomer_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageCustomer_Staff customer_Staff = new ManageCustomer_Staff();
            customer_Staff.Show();
        }

        private void btnProcessOrder_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Visible = true;
        }

        private void btnSuppliesOrder_Click(object sender, EventArgs e)
        {
            this.Hide();
            ProcessOrder_Supplies suppliesOrder = new ProcessOrder_Supplies();
            suppliesOrder.Show();
        }

        private void btnSuppliesOrder_Leave(object sender, EventArgs e)
        {
            flowLayoutPanel1.Visible = false;
        }

        private void btnInstallationOrder_Click(object sender, EventArgs e)
        {
            this.Hide();
            Process_Order_Installations installations = new Process_Order_Installations();
            installations.Show();
        }

        private void btnManageInstallation_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageInstallation_Staff installation = new ManageInstallation_Staff();
            installation.Show();
        }

        private void btnViewSupplies_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageSupply_DashboardStaff manageSupply = new ManageSupply_DashboardStaff();
            manageSupply.Show();
        }

        private void btnViewStatus_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageSupply_ViewStatusListStaff manageSupply = new ManageSupply_ViewStatusListStaff();
            manageSupply.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult = MessageBox.Show("Are you sure you want to logout this account?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (DialogResult == DialogResult.Yes)
            {
                this.Hide();
                Login login = new Login();
                login.Show();
            }
            else
            {
                this.Show();
            }
        }
    }
}
