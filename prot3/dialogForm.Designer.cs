﻿namespace prot3
{
    partial class dialogForm
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
            this.siBtn = new DevExpress.XtraEditors.SimpleButton();
            this.noBtn = new DevExpress.XtraEditors.SimpleButton();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // siBtn
            // 
            this.siBtn.Location = new System.Drawing.Point(62, 83);
            this.siBtn.Name = "siBtn";
            this.siBtn.Size = new System.Drawing.Size(94, 29);
            this.siBtn.TabIndex = 0;
            this.siBtn.Click += new System.EventHandler(this.siBtn_Click);
            // 
            // noBtn
            // 
            this.noBtn.Location = new System.Drawing.Point(200, 83);
            this.noBtn.Name = "noBtn";
            this.noBtn.Size = new System.Drawing.Size(94, 29);
            this.noBtn.TabIndex = 1;
            this.noBtn.Click += new System.EventHandler(this.noBtn_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(348, 80);
            this.label1.TabIndex = 2;
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(347, 132);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.noBtn);
            this.Controls.Add(this.siBtn);
            this.Name = "dialogForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "dialogForm";
            this.Load += new System.EventHandler(this.dialogForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton siBtn;
        private DevExpress.XtraEditors.SimpleButton noBtn;
        private System.Windows.Forms.Label label1;
    }
}