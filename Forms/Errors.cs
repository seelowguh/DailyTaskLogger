using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskLogger.Classes;

namespace TaskLogger.Forms
{
    public partial class Errors : Form
    {
        public Errors()
        {
            InitializeComponent();
        }

        private Rectangle GetScreen()
        {
            return Screen.PrimaryScreen.Bounds;
        }

        private void Errors_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.Manual;
            int x = GetScreen().Width - (this.Width + 20);
            int y = GetScreen().Height - (this.Height + 80);
            this.Location = this.PointToScreen(new Point(x, y));
            this.KeyPreview = true;

            dgvErrors.AutoGenerateColumns = true;
            LoadErrors();
        }

        private void LoadErrors()
        {
            dgvErrors.DataSource = clsErrorHandling.ErrorList;

        }

        private void Errors_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
                this.Close();
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (ModifierKeys == Keys.None && keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessDialogKey(keyData);
        }

        private void Errors_DoubleClick(object sender, EventArgs e)
        {
            clsErrorHandling.ErrorList.Clear();
        }
    }
}
