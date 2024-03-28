using IDMS.Admin;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDMS
{
    public partial class Login : Form
    {
        public static string setFName = "";
        public static string setLName = "";

        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            int roleID;

            try
            {
                Connection.Connection.DB();
                Functions.Functions.query = "Select * from users where username = '" + txtUsername.Text + "' and password = '" + txtPassword.Text + "'";
                Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
                Functions.Functions.reader = Functions.Functions.command.ExecuteReader();

                if (Functions.Functions.reader.HasRows)
                {
                    Functions.Functions.reader.Read();
                    roleID = Convert.ToInt32(Functions.Functions.reader["roleID"]);

                    if (roleID == 1)
                    {
                        txtUsername.Text = Functions.Functions.reader["username"].ToString();
                        txtPassword.Text = Functions.Functions.reader["password"].ToString();
                        setFName = Functions.Functions.reader["FName"].ToString();
                        setLName = Functions.Functions.reader["LName"].ToString();

                        this.Hide();
                        Admin.AdminDashboard dashboard = new Admin.AdminDashboard();
                        dashboard.Show();
                    }

                    else if (roleID == 2)
                    {
                        txtUsername.Text = Functions.Functions.reader["username"].ToString();
                        txtPassword.Text = Functions.Functions.reader["password"].ToString();
                        setFName = Functions.Functions.reader["FName"].ToString();
                        setLName = Functions.Functions.reader["LName"].ToString();

                        this.Hide();
                        StaffDashboard dashboard = new StaffDashboard();
                        dashboard.Show();
                    }
                }

                else
                {
                    //MessageBox.Show("Invalid Login!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUsername.Clear();
                    txtPassword.Clear();
                    if (txtUsername.Text == "")
                    {
                        txtUsername.Text = "Username";
                    }
                    if (txtPassword.Text == "")
                    {
                        txtPassword.Text = "Password";
                        txtPassword.PasswordChar = '\0';
                    }


                    Form loginFormBackground = new Form();
                    try
                    {
                        using (Error error = new Error())
                        {
                            loginFormBackground.Opacity = .70d;
                            loginFormBackground.BackColor = Color.Black;
                            loginFormBackground.Location = this.Location;
                            loginFormBackground.ShowInTaskbar = false;
                            //pnlError.BackColor = Color.FromArgb(100, 0, 0, 0);
                            //pnlError.Visible = true;
                            //pnlError.Location = new Point(3, 1);
                            //loginFormBackground.Show();
                            Login login = new Login();
                            login.Enabled = false;
                            //error.Owner = loginFormBackground;
                            error.ShowDialog();
                            loginFormBackground.Dispose();
                        }
                    }
                    catch (Exception ex) 
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtUsername_MouseClick(object sender, MouseEventArgs e)
        {
            if (txtUsername.Text == "Username") 
            {
                txtUsername.Clear();
            }
        }

        private void txtUsername_Leave(object sender, EventArgs e)
        {
            if (txtUsername.Text == "")
            {
                txtUsername.Text = "Username";
            }
        }

        private void txtPassword_MouseClick(object sender, MouseEventArgs e)
        {
            if (txtPassword.Text == "Password")
            {
                txtPassword.Clear();
                txtPassword.PasswordChar = '●';
            }
        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            if (txtPassword.Text == "")
            {
                txtPassword.Text = "Password";
                txtPassword.PasswordChar = '\0';
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            if (txtPassword.Text != "Password")
            {
                txtPassword.PasswordChar = '●';
            }
            else 
            {
                txtPassword.PasswordChar = '\0';
            }
        }

        private void btnViewPass_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtPassword.PasswordChar == '●')
                {
                    btnViewPass.Hide();
                    txtPassword.PasswordChar = '\0';
                    btnHidePass.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnHidePass_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtPassword.PasswordChar == '\0')
                {
                    btnHidePass.Hide();
                    txtPassword.PasswordChar = '●';
                    btnViewPass.Show();
                }
                else
                {
                    btnHidePass.Show();
                    txtPassword.PasswordChar = '\0';
                    btnViewPass.Hide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
