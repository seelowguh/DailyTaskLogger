namespace TaskLogger.Forms
{
    partial class OpenTasks
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
            this.dgvOpenTasks = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOpenTasks)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvOpenTasks
            // 
            this.dgvOpenTasks.AllowUserToAddRows = false;
            this.dgvOpenTasks.AllowUserToDeleteRows = false;
            this.dgvOpenTasks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOpenTasks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOpenTasks.Location = new System.Drawing.Point(0, 0);
            this.dgvOpenTasks.Name = "dgvOpenTasks";
            this.dgvOpenTasks.Size = new System.Drawing.Size(641, 254);
            this.dgvOpenTasks.TabIndex = 0;
            this.dgvOpenTasks.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvOpenTasks_CellDoubleClick);
            // 
            // OpenTasks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(641, 254);
            this.Controls.Add(this.dgvOpenTasks);
            this.Name = "OpenTasks";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "OpenTasks";
            this.Load += new System.EventHandler(this.OpenTasks_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OpenTasks_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOpenTasks)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvOpenTasks;
    }
}