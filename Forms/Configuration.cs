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
using System.Windows.Forms.VisualStyles;
using Microsoft.Win32;
using TaskLogger.Classes;
using err = TaskLogger.Classes.clsObjects.Error;

namespace TaskLogger.Forms
{
    public partial class Configuration : Form
    {
        public Configuration()
        {
            InitializeComponent();
        }

        private void Error(string function, string msg)
        {
            clsErrorHandling.HandleError(new err(DateTime.Now, this.GetType().Name, function, msg));
        }

        private clsRegistry reg = null;
        private Rectangle GetScreen()
        {
            return Screen.PrimaryScreen.Bounds;
        }

        private void Configuration_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.Manual;
            int x = GetScreen().Width - (this.Width + 20);
            int y = GetScreen().Height - (this.Height + 80);
            this.Location = this.PointToScreen(new Point(x, y));
            this.KeyPreview = true;

            reg = new clsRegistry();

            if(!reg.RegExists())
                reg.CreateReg();

            //  Load details
            clsObjects.ConfigurationDetails cd = reg.ReadReg();
            txtODBCName.Text = cd.ServerName;
            txtDatabase.Text = cd.Database;
            txtUserName.Text = cd.Username;
            txtPassword.Text = cd.Password;
            chkWindows.Checked = cd.UseWindowsCredentials;

            //  Enable or disable based on check box
            txtUserName.Enabled = !chkWindows.Checked;
            txtPassword.Enabled = !chkWindows.Checked;
        }

        private void Configuration_FormClosing(object sender, FormClosingEventArgs e)
        {
            //  Save to reg
            reg.CreateReg(txtODBCName.Text, txtDatabase.Text, txtUserName.Text, txtPassword.Text, chkWindows.Checked);
            try
            {
                clsDatabaseHandling cdb = new clsDatabaseHandling(txtODBCName.Text, txtDatabase.Text, txtUserName.Text, txtPassword.Text, chkWindows.Checked);
                cdb.CheckAndCreateTables();
            }
            catch (Exception ex)
            {
                Error(MethodBase.GetCurrentMethod().Name, ex.Message);
                Environment.Exit(0);
            }
        }
        
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            txtUserName.Enabled = !chkWindows.Checked;
            txtPassword.Enabled = !chkWindows.Checked;
        }

        private void Configuration_KeyDown(object sender, KeyEventArgs e)
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
    }
}
