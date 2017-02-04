using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskLogger.Forms;
using TaskLogger.Properties;

namespace TaskLogger
{
    class ContextMenus
    {
        public ContextMenuStrip Create()
        {
            // Add the default menu options.
            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem item;
            ToolStripSeparator sep;

            // Configure
            item = new ToolStripMenuItem();
            item.Text = "Configuration";
            item.Click += new EventHandler(Configuration_Click);
            item.Image = Resources.cog_512;
            menu.Items.Add(item);

            // Separator.
            sep = new ToolStripSeparator();
            menu.Items.Add(sep);

            // Log
            item = new ToolStripMenuItem();
            item.Text = "Log Task";
            item.Click += new EventHandler(LogTask_Click);
            item.Image = Resources.chat_alt_stroke_512;
            menu.Items.Add(item);

            //  View open logs
            item = new ToolStripMenuItem();
            item.Text = "Open Tasks";
            item.Click += new EventHandler(OpenTasks_Click);
            item.Image = Resources.Thumb_Up_Vector;
            menu.Items.Add(item);

            // Separator.
            sep = new ToolStripSeparator();
            menu.Items.Add(sep);

            //  Errors.
            item = new ToolStripMenuItem();
            item.Text = "View Errors";
            item.Click += new EventHandler(ViewErrors_Click);
            item.Image = Resources.Traffic_Cone;
            menu.Items.Add(item);

            // Exit.
            item = new ToolStripMenuItem();
            item.Text = "Exit";
            item.Click += new System.EventHandler(Exit_Click);
            menu.Items.Add(item);

            return menu;
        }

        void Configuration_Click(object sender, EventArgs e)
        {
            //  Log task
            Configuration Config = new Configuration();
            Config.Show();
        }

        void LogTask_Click(object sender, EventArgs e)
        {
            //  Log task
            Log Logger = new Log();
            Logger.Show();
        }

        void OpenTasks_Click(object sender, EventArgs e)
        {
            OpenTasks opn = new OpenTasks();
            opn.Show();
        }

        void ViewErrors_Click(object sender, EventArgs e)
        {
            Errors er = new Errors();
            er.Show();
        }

        void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    
    
    }
}
