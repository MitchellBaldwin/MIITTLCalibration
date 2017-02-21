namespace MIITTLCalibration
{
    partial class TTLCalibMain
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pBaroLabel = new System.Windows.Forms.Label();
            this.pBaroUnitsLabel = new System.Windows.Forms.Label();
            this.pBaroTextBox = new System.Windows.Forms.TextBox();
            this.gTempTextBox = new System.Windows.Forms.TextBox();
            this.gTempUnitsLabel = new System.Windows.Forms.Label();
            this.gTempLabel = new System.Windows.Forms.Label();
            this.rHTextBox = new System.Windows.Forms.TextBox();
            this.rHUnitsLabel = new System.Windows.Forms.Label();
            this.rHLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pBaroLabel
            // 
            this.pBaroLabel.ForeColor = System.Drawing.Color.White;
            this.pBaroLabel.Location = new System.Drawing.Point(29, 16);
            this.pBaroLabel.Name = "pBaroLabel";
            this.pBaroLabel.Size = new System.Drawing.Size(260, 23);
            this.pBaroLabel.TabIndex = 0;
            this.pBaroLabel.Text = "Atmospheric Pressure:";
            this.pBaroLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // pBaroUnitsLabel
            // 
            this.pBaroUnitsLabel.AutoSize = true;
            this.pBaroUnitsLabel.ForeColor = System.Drawing.Color.White;
            this.pBaroUnitsLabel.Location = new System.Drawing.Point(401, 16);
            this.pBaroUnitsLabel.Name = "pBaroUnitsLabel";
            this.pBaroUnitsLabel.Size = new System.Drawing.Size(56, 25);
            this.pBaroUnitsLabel.TabIndex = 1;
            this.pBaroUnitsLabel.Text = "inHg";
            // 
            // pBaroTextBox
            // 
            this.pBaroTextBox.Location = new System.Drawing.Point(295, 13);
            this.pBaroTextBox.Name = "pBaroTextBox";
            this.pBaroTextBox.Size = new System.Drawing.Size(100, 31);
            this.pBaroTextBox.TabIndex = 2;
            this.pBaroTextBox.Text = "29.92";
            this.pBaroTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // gTempTextBox
            // 
            this.gTempTextBox.Location = new System.Drawing.Point(295, 57);
            this.gTempTextBox.Name = "gTempTextBox";
            this.gTempTextBox.Size = new System.Drawing.Size(100, 31);
            this.gTempTextBox.TabIndex = 5;
            this.gTempTextBox.Text = "20";
            this.gTempTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // gTempUnitsLabel
            // 
            this.gTempUnitsLabel.AutoSize = true;
            this.gTempUnitsLabel.ForeColor = System.Drawing.Color.White;
            this.gTempUnitsLabel.Location = new System.Drawing.Point(401, 60);
            this.gTempUnitsLabel.Name = "gTempUnitsLabel";
            this.gTempUnitsLabel.Size = new System.Drawing.Size(27, 25);
            this.gTempUnitsLabel.TabIndex = 4;
            this.gTempUnitsLabel.Text = "C";
            // 
            // gTempLabel
            // 
            this.gTempLabel.ForeColor = System.Drawing.Color.White;
            this.gTempLabel.Location = new System.Drawing.Point(29, 60);
            this.gTempLabel.Name = "gTempLabel";
            this.gTempLabel.Size = new System.Drawing.Size(260, 23);
            this.gTempLabel.TabIndex = 3;
            this.gTempLabel.Text = "Gas Temperature:";
            this.gTempLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // rHTextBox
            // 
            this.rHTextBox.Location = new System.Drawing.Point(295, 103);
            this.rHTextBox.Name = "rHTextBox";
            this.rHTextBox.Size = new System.Drawing.Size(100, 31);
            this.rHTextBox.TabIndex = 8;
            this.rHTextBox.Text = "50";
            this.rHTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // rHUnitsLabel
            // 
            this.rHUnitsLabel.AutoSize = true;
            this.rHUnitsLabel.ForeColor = System.Drawing.Color.White;
            this.rHUnitsLabel.Location = new System.Drawing.Point(401, 106);
            this.rHUnitsLabel.Name = "rHUnitsLabel";
            this.rHUnitsLabel.Size = new System.Drawing.Size(31, 25);
            this.rHUnitsLabel.TabIndex = 7;
            this.rHUnitsLabel.Text = "%";
            // 
            // rHLabel
            // 
            this.rHLabel.ForeColor = System.Drawing.Color.White;
            this.rHLabel.Location = new System.Drawing.Point(29, 106);
            this.rHLabel.Name = "rHLabel";
            this.rHLabel.Size = new System.Drawing.Size(260, 23);
            this.rHLabel.TabIndex = 6;
            this.rHLabel.Text = "Relative Humidity:";
            this.rHLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // TTLCalibMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MidnightBlue;
            this.ClientSize = new System.Drawing.Size(1661, 609);
            this.Controls.Add(this.rHTextBox);
            this.Controls.Add(this.rHUnitsLabel);
            this.Controls.Add(this.rHLabel);
            this.Controls.Add(this.gTempTextBox);
            this.Controls.Add(this.gTempUnitsLabel);
            this.Controls.Add(this.gTempLabel);
            this.Controls.Add(this.pBaroTextBox);
            this.Controls.Add(this.pBaroUnitsLabel);
            this.Controls.Add(this.pBaroLabel);
            this.Name = "TTLCalibMain";
            this.Text = "MII PV2 TTL Calibration";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label pBaroLabel;
        private System.Windows.Forms.Label pBaroUnitsLabel;
        private System.Windows.Forms.TextBox pBaroTextBox;
        private System.Windows.Forms.TextBox gTempTextBox;
        private System.Windows.Forms.Label gTempUnitsLabel;
        private System.Windows.Forms.Label gTempLabel;
        private System.Windows.Forms.TextBox rHTextBox;
        private System.Windows.Forms.Label rHUnitsLabel;
        private System.Windows.Forms.Label rHLabel;
    }
}