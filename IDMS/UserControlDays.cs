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
    public partial class UserControlDays : UserControl
    {
        public event EventHandler<string> DayClicked;
        public static string Day, date, weekdays;
        public UserControlDays(string day)
        {
            InitializeComponent();
            Day = day;
            lbdays.Text = day;
            ckbDays.Hide();
        }

        private void sunday() 
        {
            try
            {
                DateTime day = DateTime.Parse(date);
                weekdays = day.ToString("ddd");
                if (weekdays == "SUN")
                {
                    lbdays.ForeColor = Color.FromArgb(255, 128, 128);
                }
                else
                {
                    lbdays.ForeColor = Color.FromArgb(64, 64, 64);
                }
            }
            catch (Exception ex) 
            {
            
            }
        }

        private void pnlDays_Click(object sender, EventArgs e)
        {
            if (ckbDays.Checked == false)
            {
                ckbDays.Checked = true;
                pnlDays.BackColor = Color.FromArgb(255, 150, 79);
                Console.WriteLine("Clicked");

                string clickedDay = lbdays.Text;
                DayClicked?.Invoke(this, clickedDay);

                //add logic for info display
            }
            else
            {
                ckbDays.Checked = false;
                pnlDays.BackColor = Color.White;
                Console.WriteLine("Unclicked");
            }
        }

        private void UserControlDays_Load(object sender, EventArgs e)
        {
            //sunday();
        }

        public void days(int numday)
        {
            lbdays.Text = numday + "";
        }
        
    }
}
