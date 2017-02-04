using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Microsoft.VisualBasic.Devices;
using TaskLogger.Classes;
using err = TaskLogger.Classes.clsObjects.Error;
using Microsoft.VisualBasic;

namespace TaskLogger.Forms
{
    public partial class Log : Form
    {
        public Log()
        {
            InitializeComponent();
        }

        private void Error(string function, string msg)
        {
            clsErrorHandling.HandleError(new err(DateTime.Now, this.GetType().Name, function, msg));
        }

        private DateTime gDueDate = new DateTime();
        
        private clsObjects.ConfigurationDetails Config;
        private clsRegistry reg = null;
        private clsDatabaseHandling db = null;
        private List<clsObjects.Group> Groups;

        private Rectangle GetScreen()
        {
            return Screen.PrimaryScreen.Bounds;
        }

        private void Log_Load(object sender, EventArgs e)
        {
            //  Set size & position
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.Manual;
            int x = GetScreen().Width - (this.Width + 20);
            int y = GetScreen().Height - (this.Height + 80);
            this.Location = this.PointToScreen(new Point(x, y));
            this.KeyPreview = true;

            //  Initialise and populate form
            reg = new clsRegistry();
            Config = reg.ReadReg();
            db = new clsDatabaseHandling(Config.ServerName, Config.Database, Config.Username, Config.Password, Config.UseWindowsCredentials);
            Groups = db.GetGroups();
            cmbGroup.Items.Insert(0, "");
            foreach (clsObjects.Group g in Groups)
            {
                cmbGroup.Items.Insert(g.ID, g.GroupName);
            }
        }

        private void Log_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (txtDescription.Text != string.Empty || txtRequested.Text != string.Empty ||
                cmbGroup.Text != string.Empty)
            {
                //  Are you sure?
                
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            int GroupID = 0;
            int TaskID = 0;
            //  Save to database
            //  If group is new - add it
            if (!Groups.Any(x => x.GroupName == cmbGroup.Text))
                GroupID = db.InsertNewGroup(cmbGroup.Text);
            else
                GroupID = Groups[Groups.FindIndex(x => x.GroupName == cmbGroup.Text)].ID;

                //  Save to DB
                TaskID = db.InsertNewTask(GroupID, txtDescription.Text, txtRequested.Text);
            if (TaskID != 0)
            {
                clsOutlookHanding o = new clsOutlookHanding();
                o.CreateTask(TaskID, DateTime.Now, cmbGroup.Text, txtDescription.Text, txtRequested.Text
                    , (lblDueDate.Text == string.Empty ? null : lblDueDate.Text));
                CloseLog();
            }
                
        }

        private void CloseLog()
        {
            txtDescription.Text = string.Empty;
            txtRequested.Text = string.Empty;
            cmbGroup.Text = string.Empty;
            this.Close();
        }

        private void chkDueDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDueDate.Checked)
            {
                DateTime DueDate;// = new DateTime();
                if (Dialog.ShowInputDialog(out DueDate) == DialogResult.OK)
                {
                    lblDueDate.Enabled = chkDueDate.Enabled;
                    lblDueDate.Text = DueDate.ToShortDateString() + " " + DueDate.ToShortTimeString();
                }
            }
        }

        private void Log_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
                this.Close();
            else if(e.KeyData == (Keys.Control | Keys.Enter))
            {
                btnSubmit.PerformClick();
                e.Handled = true;
            }


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

    static class Dialog
    {
        internal static DialogResult ShowInputDialog(out DateTime input)
        {
            System.Drawing.Size size = new System.Drawing.Size(200, 70);
            Form inputBox = new Form();
            inputBox.MdiParent = Log.ActiveForm;

            inputBox.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            inputBox.ClientSize = size;
            inputBox.Text = "Due Date";

            Point p = new Point();
            p.X = (inputBox.Parent.Width / 2) - (size.Width / 2);
            p.Y = (inputBox.Parent.Height / 2) - (size.Height / 2);

            DateTimePicker dtp = new DateTimePicker();
            dtp.Size = new Size(size.Width - 10, 23);
            //dtp.Location = new Point(5, 5);
            dtp.Location = p;
            dtp.Value = DateTime.Now;
            inputBox.Controls.Add(dtp);

            Button okButton = new Button();
            okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(75, 23);
            okButton.Text = "&OK";
            okButton.Location = new System.Drawing.Point(size.Width - 80 - 80, 39);
            inputBox.Controls.Add(okButton);

            Button cancelButton = new Button();
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(75, 23);
            cancelButton.Text = "&Cancel";
            cancelButton.Location = new System.Drawing.Point(size.Width - 80, 39);
            inputBox.Controls.Add(cancelButton);

            inputBox.AcceptButton = okButton;
            inputBox.CancelButton = cancelButton;

            DialogResult result = inputBox.ShowDialog();
            input = dtp.Value;
            return result;
        }
    }

}
