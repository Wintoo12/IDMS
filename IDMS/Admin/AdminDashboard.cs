﻿using System;
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
    public partial class AdminDashboard : Form
    {
        bool suppliesCollapsed;
        bool customerCollapsed;
        bool employeesCollapsed;
        bool reportsCollapsed;

        public AdminDashboard()
        {
            InitializeComponent();
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

        private void AdminDashboard_Load(object sender, EventArgs e)
        {
            lblGreetings.Text = "Good Day Admin!";
            btnHome.ForeColor = Color.Black;
            btnHome.BackColor = Color.White;
            btnHome.Font = new Font("Century Gothic", 14, FontStyle.Italic);
            int stock = 1000;
            
            //Red
            if (stock >= 0 && stock <= 249)
            {
                pnlStatus1.BackColor = Color.FromArgb(239, 1, 1);
                lblsStocks.Text = Convert.ToString(stock) + " " + "/1000";
                btnRestock.Enabled = true;
            }
            //Orange
            else if (stock >= 250 && stock <= 499)
            {
                pnlStatus1.BackColor = Color.FromArgb(239, 179, 1);
                lblsStocks.Text = Convert.ToString(stock) + " " + "/1000";
                btnRestock.BackgroundImage = null;
                btnRestock.BackColor = Color.Gray;
            }
            //Yellow
            else if (stock >= 500 && stock <= 749)
            {
                pnlStatus1.BackColor = Color.FromArgb(239, 239, 1);
                lblsStocks.Text = Convert.ToString(stock) + " " + "/1000";
                btnRestock.BackColor = Color.Gray;
                btnRestock.BackgroundImage = null;
            }
            //Teal
            else if (stock >= 750 && stock <= 999)
            {
                pnlStatus1.BackColor = Color.FromArgb(0, 150, 137);
                lblsStocks.Text = Convert.ToString(stock) + " " + "/1000";
                btnRestock.BackColor = Color.Gray;
                btnRestock.BackgroundImage = null;
            }
            //Green
            else if (stock == 1000)
            {
                pnlStatus1.BackColor = Color.FromArgb(1, 237, 3);
                lblsStocks.Text = Convert.ToString(stock) + " " + "/1000";
                btnRestock.BackColor = Color.Gray;
                btnRestock.BackgroundImage = null;
            }
        }

        private void btnHome_Leave(object sender, EventArgs e)
        {
            btnHome.ForeColor = Color.White;
            btnHome.BackColor = Color.Transparent;
            btnHome.Font = new Font("Century Gothic", 14, FontStyle.Bold);
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void SuppliesTimer_Tick(object sender, EventArgs e)
        {
            if (suppliesCollapsed)
            {
                suppliesContainer.Height += 10;
                if (suppliesContainer.Height == suppliesContainer.MaximumSize.Height)
                {
                    suppliesCollapsed = false;
                    SuppliesTimer.Stop();
                }
            }
            else
            {
                suppliesContainer.Height -= 10;
                if (suppliesContainer.Height == suppliesContainer.MinimumSize.Height)
                {
                    suppliesCollapsed = true;
                    SuppliesTimer.Stop();
                    btnSupplies.ForeColor = Color.White;
                    btnSupplies.BackColor = Color.Transparent;
                    btnSupplies.Font = new Font("Century Gothic", 14, FontStyle.Bold);
                }
            }
        }

        private void btnSupplies_Click(object sender, EventArgs e)
        {
            SuppliesTimer.Start();
            btnSupplies.ForeColor = Color.Black;
            btnSupplies.BackColor = Color.White;
            btnSupplies.Font = new Font("Century Gothic", 14, FontStyle.Italic);
        }

        private void btnSupplies_Leave(object sender, EventArgs e)
        {
            btnSupplies.Font = new Font("Century Gothic", 14, FontStyle.Bold);
        }

        private void btnManageSupp_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageSupply_DashboardAdmin supplyDashboard = new ManageSupply_DashboardAdmin();
            supplyDashboard.Show();
        }

        private void btnViewSupp_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageSupply_ViewStatusListAdmin viewList = new ManageSupply_ViewStatusListAdmin();
            viewList.Show();
        }

        private void CustomerTimer_Tick(object sender, EventArgs e)
        {
            if (customerCollapsed)
            {
                customerContainer.Height += 10;
                if (customerContainer.Height == customerContainer.MaximumSize.Height)
                {
                    customerCollapsed = false;
                    customerTimer.Stop();
                }
            }
            else
            {
                customerContainer.Height -= 10;
                if (customerContainer.Height == customerContainer.MinimumSize.Height)
                {
                    customerCollapsed = true;
                    customerTimer.Stop();
                    btnCustomer.ForeColor = Color.White;
                    btnCustomer.BackColor = Color.Transparent;
                    btnCustomer.Font = new Font("Century Gothic", 14, FontStyle.Bold);
                }
            }
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            customerTimer.Start();

            btnCustomer.ForeColor = Color.Black;
            btnCustomer.BackColor = Color.White;
            btnCustomer.Font = new Font("Century Gothic", 14, FontStyle.Italic);
        }

        private void btnCustomer_Leave(object sender, EventArgs e)
        {
            btnCustomer.Font = new Font("Century Gothic", 14, FontStyle.Bold);
        }

        private void btnMaanageCust_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin.ManageCustomer customer = new Admin.ManageCustomer();
            customer.Show();
        }

        private void btnInstallations_Click(object sender, EventArgs e)
        {
            btnInstallations.ForeColor = Color.Black;
            btnInstallations.BackColor = Color.White;
            btnInstallations.Font = new Font("Century Gothic", 14, FontStyle.Italic);

            this.Hide();
            Admin.ManageInstallation installation = new Admin.ManageInstallation();
            installation.Show();
        }

        private void btnReservations_Click(object sender, EventArgs e)
        {
            btnReservations.ForeColor = Color.Black;
            btnReservations.BackColor = Color.White;
            btnReservations.Font = new Font("Century Gothic", 14, FontStyle.Italic);

            this.Hide();
            Admin.ManageReservations reservations = new Admin.ManageReservations();
            reservations.Show();
        }

        private void employeesTimer_Tick(object sender, EventArgs e)
        {
            if (employeesCollapsed)
            {
                employeeContainer.Height += 10;
                if (employeeContainer.Height == employeeContainer.MaximumSize.Height)
                {
                    employeesCollapsed = false;
                    employeesTimer.Stop();
                }
            }
            else
            {
                employeeContainer.Height -= 10;
                if (employeeContainer.Height == employeeContainer.MinimumSize.Height)
                {
                    employeesCollapsed = true;
                    employeesTimer.Stop();
                    btnEmployees.ForeColor = Color.White;
                    btnEmployees.BackColor = Color.Transparent;
                    btnEmployees.Font = new Font("Century Gothic", 14, FontStyle.Bold);
                }
            }
        }

        private void btnEmployees_Click(object sender, EventArgs e)
        {
            employeesTimer.Start();

            btnEmployees.ForeColor = Color.Black;
            btnEmployees.BackColor = Color.White;
            btnEmployees.Font = new Font("Century Gothic", 14, FontStyle.Italic);
        }

        private void btnEmployees_Leave(object sender, EventArgs e)
        {
            btnEmployees.Font = new Font("Century Gothic", 14, FontStyle.Bold);
        }

        private void btnManageEmployee_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageEmployee employee = new ManageEmployee();
            employee.Show();
        }

        private void btnAttendance_Click(object sender, EventArgs e)
        {
            this.Hide();
            Manage_Employee.ManageEmployee_DailyAttendance attendance = new Manage_Employee.ManageEmployee_DailyAttendance();
            attendance.Show();
        }

        private void reportsTimer_Tick(object sender, EventArgs e)
        {
            if (reportsCollapsed)
            {
                reportsContainer.Height += 10;
                if (reportsContainer.Height == reportsContainer.MaximumSize.Height)
                {
                    reportsCollapsed = false;
                    reportsTimer.Stop();
                }
            }
            else
            {
                reportsContainer.Height -= 10;
                if (reportsContainer.Height == reportsContainer.MinimumSize.Height)
                {
                    reportsCollapsed = true;
                    reportsTimer.Stop();
                    btnReports.ForeColor = Color.White;
                    btnReports.BackColor = Color.Transparent;
                    btnReports.Font = new Font("Century Gothic", 14, FontStyle.Bold);
                }
            }
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            reportsTimer.Start();

            btnReports.ForeColor = Color.Black;
            btnReports.BackColor = Color.White;
            btnReports.Font = new Font("Century Gothic", 14, FontStyle.Italic);
        }

        private void btnReports_Leave(object sender, EventArgs e)
        {
            btnReports.Font = new Font("Century Gothic", 14, FontStyle.Bold);
        }

        /*Note: Wala pani connection sa attendance nga form kay kulang pa*/
    }
}
