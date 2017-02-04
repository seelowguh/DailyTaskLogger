using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskLogger.Classes;
using TaskLogger.Forms;
using TaskLogger.Properties;

namespace TaskLogger
{
    class ProcessIcon : IDisposable
    {
        private NotifyIcon ni;

        public ProcessIcon()
        {
            ni = new NotifyIcon();
        }

        public void Display()
        {
            ni.MouseDoubleClick += new MouseEventHandler(ni_MouseDoubleClick);
            ni.Icon = Resources.aperture_alt_512;
            ni.Text = "Task Logger";
            ni.Visible = true;

            ni.ContextMenuStrip = new ContextMenus().Create();
        }

        void ni_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //  Log task
                Log Logger = new Log();
                Logger.Show();
            }
        }

       
        public void Dispose()
        {
            ni.Dispose();
        }
    }
}
