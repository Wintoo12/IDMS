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

namespace IDMS.Admin.Manage_Employee
{
    public partial class ManageEmployee_DailyAttendance : Form
    {
        public static int iterationCount = 0;
        public static DateTime dateVal = DateTime.Now.Date;

        public static int day = dateVal.Day;
        public static string month = DateTime.Now.ToString("MMMM");
        public static int year = dateVal.Year;
        public static string today = month + " " + day  + ", " + year; 


        public ManageEmployee_DailyAttendance()
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

        private void ManageEmployee_DailyAttendance_Load(object sender, EventArgs e)
        {
            dailyAttendance();
            lblDate.Text = "Attendance as of " + today;
            if (IsAttendanceRecordedToday())
            {
                DisableAttendanceControls();
                MessageBox.Show("Attendance has already been recorded for today.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void dailyAttendance() 
        {
            Connection.Connection.DB();
            Functions.Functions.query = "Select * from users where roleID = 2 AND status = 'Active'";
            Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
            Functions.Functions.reader = Functions.Functions.command.ExecuteReader();

            Image stockImage = new Bitmap(@"C:\Users\Mark Andrew\Documents\3rd Year School Stuff\IDMS\UI purposes\pnlDark.png");
            Image buttonImage = new Bitmap(@"C:\Users\Mark Andrew\Documents\3rd Year School Stuff\IDMS\UI purposes\button1.png");

            if (Functions.Functions.reader.HasRows)
            {
                while (Functions.Functions.reader.Read())
                {
                    iterationCount++;
                    string firstname = Functions.Functions.reader["FName"].ToString();  
                    string middlename = Functions.Functions.reader["MName"].ToString();
                    string lastname = Functions.Functions.reader["LName"].ToString();
                    string fullname = firstname + " " + middlename + " " + lastname;
                    int userID = Convert.ToInt32(Functions.Functions.reader["userID"]);

                    Panel pnl = new Panel();
                    //pnl.BackgroundImage = Properties.Resources.Untitled_design__42_;
                    pnl.BackgroundImage = stockImage;
                    pnl.BackgroundImageLayout = ImageLayout.Stretch;
                    pnl.Size = new Size(910, 58);
                    //pnl.Padding = new Padding(5);

                    Label lbStafftName = new Label();
                    //lblProductName.Name = Functions.Functions.reader["productID"].ToString();
                    lbStafftName.Text = firstname + " " + middlename + " " + lastname;
                    lbStafftName.BackColor = Color.Transparent;
                    lbStafftName.ForeColor = Color.White;
                    lbStafftName.Font = new Font("Century Gothic", 14, FontStyle.Bold);
                    lbStafftName.AutoSize = true;
                    lbStafftName.Padding = new Padding(15);
                    lbStafftName.Tag = userID;

                    RadioButton rbtnPres = new RadioButton();
                    rbtnPres.Click += new EventHandler(this.rbtnPresent_checked);
                    rbtnPres.Name = "rbtnPresent" + userID;
                    rbtnPres.Text = "Present";
                    rbtnPres.BackColor = Color.Transparent;
                    rbtnPres.ForeColor = Color.White;
                    rbtnPres.Font = new Font("Century Gothic", 14, FontStyle.Regular);
                    rbtnPres.Location = new Point(502, 16);
                    rbtnPres.AutoSize = true;

                    RadioButton rbtnLt = new RadioButton();
                    rbtnLt.Name = "rbtnLate" + userID;
                    rbtnLt.Text = "Late";
                    rbtnLt.BackColor = Color.Transparent;
                    rbtnLt.ForeColor = Color.White;
                    rbtnLt.Font = new Font("Century Gothic", 14, FontStyle.Regular);
                    rbtnLt.Location = new Point(603, 16);
                    rbtnLt.AutoSize = true;

                    RadioButton rbtnAbs = new RadioButton();
                    rbtnAbs.Name = "rbtnAbsent" + userID;
                    rbtnAbs.Text = "Absent";
                    rbtnAbs.BackColor = Color.Transparent;
                    rbtnAbs.ForeColor = Color.White;
                    rbtnAbs.Font = new Font("Century Gothic", 14, FontStyle.Regular);
                    rbtnAbs.Location = new Point(678, 16);
                    rbtnAbs.AutoSize = true;

                    RadioButton rbtnEx = new RadioButton();
                    rbtnEx.Name = "rbtnExcused" + userID;
                    rbtnEx.Text = "Excused";
                    rbtnEx.BackColor = Color.Transparent;
                    rbtnEx.ForeColor = Color.White;
                    rbtnEx.Font = new Font("Century Gothic", 14, FontStyle.Regular);
                    rbtnEx.Location = new Point(778, 16);
                    rbtnEx.AutoSize = true;

                    pnl.Controls.Add(lbStafftName);
                    pnl.Controls.Add(rbtnPres);
                    pnl.Controls.Add(rbtnLt);
                    pnl.Controls.Add(rbtnAbs);
                    pnl.Controls.Add(rbtnEx);
                    flowLayoutPanel2.Controls.Add(pnl);
                    flowLayoutPanel2.FlowDirection = FlowDirection.TopDown;

                }
            }

        }

        private void rbtnPresent_checked(object sender, EventArgs e)
        {
            RadioButton rbtn = sender as RadioButton;
            //selectedUserID = Convert.ToInt32(rbtn.Tag); // Store selected user ID
        }

        public void viewAttendance(int userID)
        {
            Connection.Connection.DB();
            Functions.Functions.query = "SELECT * FROM users WHERE userID = @userID";
            Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
            Functions.Functions.command.Parameters.AddWithValue("@userID", userID);
            Functions.Functions.reader = Functions.Functions.command.ExecuteReader();

            if (Functions.Functions.reader.HasRows)
            {
                Functions.Functions.reader.Read();
                //lblProductDetails.Text = Functions.Functions.reader["productName"].ToString();
            }
        }

        private void btnSubmitAttendance_Click(object sender, EventArgs e)
        {
            DateTime today = DateTime.Today;

            if (IsAttendanceRecordedToday())
            {
                DisableAttendanceControls();
                MessageBox.Show("Attendance has already been recorded for today.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            bool anySuccess = false; // Variable to track if any records were successfully added

            foreach (Control control in flowLayoutPanel2.Controls)
            {
                if (control is Panel panel)
                {
                    Label nameLabel = panel.Controls.OfType<Label>().FirstOrDefault();
                    if (nameLabel == null || nameLabel.Tag == null) continue;

                    int userID;
                    if (!int.TryParse(nameLabel.Tag.ToString(), out userID)) continue;

                    RadioButton selectedRadioButton = panel.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
                    if (selectedRadioButton != null)
                    {
                        string status = selectedRadioButton.Text;

                        try
                        {
                            Connection.Connection.DB();
                            Functions.Functions.query = "INSERT INTO attendance (userID, date, status) VALUES (@UserID, @Date, @Status)";
                            Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
                            Functions.Functions.command.Parameters.AddWithValue("@UserID", userID);
                            Functions.Functions.command.Parameters.AddWithValue("@Date", DateTime.Now);
                            Functions.Functions.command.Parameters.AddWithValue("@Status", status);

                            int rowsAffected = Functions.Functions.command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                anySuccess = true; // Set to true if at least one record is successfully added
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            Connection.Connection.con.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please select an attendance status for user " + userID + "!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            // Show a single success message if any records were successfully added
            if (anySuccess)
            {
                MessageBox.Show("Attendance added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DisableAttendanceControls();
            }
            else
            {
                MessageBox.Show("No attendance records were added.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool IsAttendanceRecordedToday()
        {
            try
            {
                Connection.Connection.DB();
                Functions.Functions.query = "SELECT COUNT(*) FROM attendance WHERE CAST(date AS DATE) = CAST(@Date AS DATE)";
                Functions.Functions.command = new SqlCommand(Functions.Functions.query, Connection.Connection.con);
                Functions.Functions.command.Parameters.AddWithValue("@Date", DateTime.Today);

                int count = (int)Functions.Functions.command.ExecuteScalar();
                return count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error checking attendance: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                Connection.Connection.con.Close();
            }
        }

        private void DisableAttendanceControls()
        {
            foreach (Control control in flowLayoutPanel2.Controls)
            {
                if (control is Panel panel)
                {
                    foreach (Control childControl in panel.Controls)
                    {
                        if (childControl is RadioButton || childControl is Button)
                        {
                            childControl.Enabled = false;
                            btnSubmitAttendance.Enabled = false;
                        }
                    }
                }
            }
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin.AdminDashboard adminDashboard = new Admin.AdminDashboard();
            adminDashboard.Show();
        }
    }
}
