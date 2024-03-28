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
    public partial class ManageReservations: Form
    {
        public ManageReservations()
        {
            InitializeComponent();
        }

        private void btnReservations_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void ManageReservations_Load(object sender, EventArgs e)
        {
            btnReservations.ForeColor = Color.Black;
            btnReservations.BackColor = Color.White;
            btnReservations.Font = new Font("Century Gothic", 14, FontStyle.Italic);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.Show();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin.AdminDashboard dashboard = new Admin.AdminDashboard();
            dashboard.Show();
        }
    }
}
