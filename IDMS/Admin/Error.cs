using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
            this.Close();
            Login login = new Login();
            login.panelHide();

        }

        private void Error_Load(object sender, EventArgs e)
        {
           
        }
    }
}
