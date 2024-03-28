using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDMS.Admin
{
    public partial class ManageInstallation : Form
    {
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
    }
}
