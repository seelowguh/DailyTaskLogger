using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskLogger.Classes;

namespace TaskLogger.Forms
{
    public partial class OpenTasks : Form
    {
        public OpenTasks()
        {
            InitializeComponent();
        }
        
        private Rectangle GetScreen()
        {
            return Screen.PrimaryScreen.Bounds;
        }

        private clsDatabaseHandling db = null;
        private clsRegistry reg = null;
        private clsOutlookHanding outlook = null;

        private Thread tasksThread = null;

        private void OpenTasks_Load(object sender, EventArgs e)
        {
            //  Set size & position
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.Manual;
            int x = GetScreen().Width - (this.Width + 20);
            int y = GetScreen().Height - (this.Height + 80);
            this.Location = this.PointToScreen(new Point(x, y));
            this.KeyPreview = true;

            reg = new clsRegistry();
            clsObjects.ConfigurationDetails cd = reg.ReadReg();
            db = new clsDatabaseHandling(cd.ServerName, cd.Database, cd.Username, cd.Password, cd.UseWindowsCredentials);
            outlook = new clsOutlookHanding();

            dgvOpenTasks.DataSource = db.GetOpenTasks();

        }

        private void dgvOpenTasks_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvOpenTasks.ColumnCount - 1)
            {
                //  Status column - close
                db.CloseTask(Convert.ToInt32(dgvOpenTasks.Rows[e.RowIndex].Cells[0].Value));

                //  Delete outlook task on separate thread
                StartCloseThread(Convert.ToInt32(dgvOpenTasks.Rows[e.RowIndex].Cells[0].Value),
                    dgvOpenTasks.Rows[e.RowIndex].Cells[1].Value.ToString());
                
                dgvOpenTasks.DataSource = db.GetOpenTasks();
            }
            else
            {
                MessageBox.Show(
                    string.Format("Request type:\t{0}\n\n{1}", dgvOpenTasks.Rows[e.RowIndex].Cells[1].Value,
                        dgvOpenTasks.Rows[e.RowIndex].Cells[2].Value),
                    dgvOpenTasks.Rows[e.RowIndex].Cells[3].Value.ToString());

            }
        }

        #region CloseTask
        private void StartCloseThread(int ID, string sub)
        {
            tasksThread = new Thread(new ParameterizedThreadStart(delegate(object o) { CloseTask(ID, sub); }));
            tasksThread.IsBackground = true;
            tasksThread.Start();
        }

        private void CloseTask(int ID, string sub)
        {
            outlook.CloseTask(outlook.FindTask(ID, sub));
        }
        #endregion

        private void OpenTasks_KeyDown(object sender, KeyEventArgs e)
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
