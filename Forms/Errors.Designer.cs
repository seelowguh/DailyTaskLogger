namespace TaskLogger.Forms
{
    partial class Errors
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
            this.dgvErrors = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvErrors)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvErrors
            // 
            this.dgvErrors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvErrors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvErrors.Location = new System.Drawing.Point(0, 0);
            this.dgvErrors.Name = "dgvErrors";
            this.dgvErrors.Size = new System.Drawing.Size(564, 212);
            this.dgvErrors.TabIndex = 0;
            // 
            // Errors
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 212);
            this.Controls.Add(this.dgvErrors);
            this.Name = "Errors";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Errors";
            this.Load += new System.EventHandler(this.Errors_Load);
            this.DoubleClick += new System.EventHandler(this.Errors_DoubleClick);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Errors_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvErrors)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvErrors;
    }
}