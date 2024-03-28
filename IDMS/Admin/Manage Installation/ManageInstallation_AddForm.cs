using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDMS.Admin.Manage_Installation
{
    public partial class ManageInstallation_AddForm : Form
    {
        public ManageInstallation_AddForm()
        {
            InitializeComponent();
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
    }
}
