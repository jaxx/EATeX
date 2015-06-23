namespace EATeX.UI
{
    partial class SettingsWindow
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
            this.lblTexLocation = new System.Windows.Forms.Label();
            this.txtTexLocation = new System.Windows.Forms.TextBox();
            this.btnBrowseTexLocation = new System.Windows.Forms.Button();
            this.lblTemplateLocation = new System.Windows.Forms.Label();
            this.txtTemplateLocation = new System.Windows.Forms.TextBox();
            this.btnBrowseTemplateLocation = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTexLocation
            // 
            this.lblTexLocation.AutoSize = true;
            this.lblTexLocation.Location = new System.Drawing.Point(38, 32);
            this.lblTexLocation.Name = "lblTexLocation";
            this.lblTexLocation.Size = new System.Drawing.Size(107, 13);
            this.lblTexLocation.TabIndex = 0;
            this.lblTexLocation.Text = "TeX-distribution path:";
            // 
            // txtTexLocation
            // 
            this.txtTexLocation.Location = new System.Drawing.Point(41, 48);
            this.txtTexLocation.Name = "txtTexLocation";
            this.txtTexLocation.Size = new System.Drawing.Size(276, 20);
            this.txtTexLocation.TabIndex = 1;
            // 
            // btnBrowseTexLocation
            // 
            this.btnBrowseTexLocation.Location = new System.Drawing.Point(323, 45);
            this.btnBrowseTexLocation.Name = "btnBrowseTexLocation";
            this.btnBrowseTexLocation.Size = new System.Drawing.Size(86, 24);
            this.btnBrowseTexLocation.TabIndex = 2;
            this.btnBrowseTexLocation.Text = "Browse...";
            this.btnBrowseTexLocation.UseVisualStyleBackColor = true;
            this.btnBrowseTexLocation.Click += new System.EventHandler(this.btnBrowseMikTexLocation_Click);
            // 
            // lblTemplateLocation
            // 
            this.lblTemplateLocation.AutoSize = true;
            this.lblTemplateLocation.Location = new System.Drawing.Point(38, 88);
            this.lblTemplateLocation.Name = "lblTemplateLocation";
            this.lblTemplateLocation.Size = new System.Drawing.Size(136, 13);
            this.lblTemplateLocation.TabIndex = 3;
            this.lblTemplateLocation.Text = "Template file (.tex) location:";
            // 
            // txtTemplateLocation
            // 
            this.txtTemplateLocation.Location = new System.Drawing.Point(41, 104);
            this.txtTemplateLocation.Name = "txtTemplateLocation";
            this.txtTemplateLocation.Size = new System.Drawing.Size(276, 20);
            this.txtTemplateLocation.TabIndex = 4;
            // 
            // btnBrowseTemplateLocation
            // 
            this.btnBrowseTemplateLocation.Location = new System.Drawing.Point(323, 101);
            this.btnBrowseTemplateLocation.Name = "btnBrowseTemplateLocation";
            this.btnBrowseTemplateLocation.Size = new System.Drawing.Size(86, 24);
            this.btnBrowseTemplateLocation.TabIndex = 5;
            this.btnBrowseTemplateLocation.Text = "Browse...";
            this.btnBrowseTemplateLocation.UseVisualStyleBackColor = true;
            this.btnBrowseTemplateLocation.Click += new System.EventHandler(this.btnBrowseTemplateLocation_Click);
            // 
            // SettingsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(447, 362);
            this.Controls.Add(this.btnBrowseTemplateLocation);
            this.Controls.Add(this.txtTemplateLocation);
            this.Controls.Add(this.lblTemplateLocation);
            this.Controls.Add(this.btnBrowseTexLocation);
            this.Controls.Add(this.txtTexLocation);
            this.Controls.Add(this.lblTexLocation);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsWindow";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTexLocation;
        private System.Windows.Forms.TextBox txtTexLocation;
        private System.Windows.Forms.Button btnBrowseTexLocation;
        private System.Windows.Forms.Label lblTemplateLocation;
        private System.Windows.Forms.TextBox txtTemplateLocation;
        private System.Windows.Forms.Button btnBrowseTemplateLocation;
    }
}