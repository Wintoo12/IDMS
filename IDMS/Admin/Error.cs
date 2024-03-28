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
    public partial class Error : Form
    {
        public Error()
        {
            InitializeComponent();
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.Enabled = true;
         }

        private void Error_Load(object sender, EventArgs e)
        {

        }
    }
}
