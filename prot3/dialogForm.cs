using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prot3
{
    public partial class dialogForm : Form
    {
        public dialogForm()
        {
            //InitializeComponent();
        }
        public dialogForm(string titulo, string message, string buttonText1, string buttonText2)
        {
            InitializeComponent();
            this.Text = titulo;
             label1.Text = message;
            siBtn.Text = buttonText1;
            noBtn.Text = buttonText2;
            siBtn.DialogResult = DialogResult.Yes;
            noBtn.DialogResult = DialogResult.No;
            this.DialogResult = DialogResult.None;
           // noBtn.DialogResult = DialogResult.Retry;
        }
        public dialogForm(string titulo, string message, string buttonText1, string buttonText2, string buttonText3)
        {
            InitializeComponent();
            this.Text = titulo;
            label1.Text = message;
            siBtn.Text = buttonText1;
            noBtn.Text = buttonText2;
            siBtn.DialogResult = DialogResult.Yes;
            noBtn.DialogResult = DialogResult.No;
            this.Height =125;
            this.Width = 326;
            SimpleButton cancelButton = new SimpleButton();
            label1.Height = 40;
            siBtn.Location = new Point(25, 45);
            cancelButton.Location = new Point(115, 45);
            noBtn.Location = new Point(205, 45);            
            cancelButton.Height = 25;
            cancelButton.Width = 80;
            siBtn.Height = 25;
            siBtn.Width = 80;
            noBtn.Height = 25;
            noBtn.Width = 80;
            cancelButton.Text = buttonText3;
            Controls.Add(cancelButton);
            cancelButton.DialogResult = DialogResult.Cancel;
            this.DialogResult = DialogResult.None;

            // noBtn.DialogResult = DialogResult.Retry;
        }
        private void dialogForm_Load(object sender, EventArgs e)
        {

        }

        private void siBtn_Click(object sender, EventArgs e)
        {
            
        }

        private void noBtn_Click(object sender, EventArgs e)
        {
        }
    }
}
