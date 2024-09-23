namespace IDMS
{
    partial class UserControlDays
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbdays = new System.Windows.Forms.Label();
            this.pnlDays = new System.Windows.Forms.Panel();
            this.ckbDays = new System.Windows.Forms.CheckBox();
            this.pnlDays.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbdays
            // 
            this.lbdays.AutoSize = true;
            this.lbdays.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbdays.Location = new System.Drawing.Point(13, 7);
            this.lbdays.Name = "lbdays";
            this.lbdays.Size = new System.Drawing.Size(25, 20);
            this.lbdays.TabIndex = 2;
            this.lbdays.Text = "00";
            // 
            // pnlDays
            // 
            this.pnlDays.BackColor = System.Drawing.Color.White;
            this.pnlDays.Controls.Add(this.ckbDays);
            this.pnlDays.Controls.Add(this.lbdays);
            this.pnlDays.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDays.Location = new System.Drawing.Point(0, 0);
            this.pnlDays.Name = "pnlDays";
            this.pnlDays.Padding = new System.Windows.Forms.Padding(1);
            this.pnlDays.Size = new System.Drawing.Size(54, 60);
            this.pnlDays.TabIndex = 3;
            this.pnlDays.Click += new System.EventHandler(this.pnlDays_Click);
            // 
            // ckbDays
            // 
            this.ckbDays.AutoSize = true;
            this.ckbDays.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckbDays.Location = new System.Drawing.Point(4, 10);
            this.ckbDays.Name = "ckbDays";
            this.ckbDays.Size = new System.Drawing.Size(15, 14);
            this.ckbDays.TabIndex = 3;
            this.ckbDays.UseVisualStyleBackColor = true;
            // 
            // UserControlDays
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlDays);
            this.Name = "UserControlDays";
            this.Size = new System.Drawing.Size(54, 60);
            this.Load += new System.EventHandler(this.UserControlDays_Load);
            this.pnlDays.ResumeLayout(false);
            this.pnlDays.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbdays;
        private System.Windows.Forms.Panel pnlDays;
        private System.Windows.Forms.CheckBox ckbDays;
    }
}
