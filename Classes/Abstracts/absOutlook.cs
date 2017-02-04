using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using err = TaskLogger.Classes.clsObjects.Error;
namespace TaskLogger.Classes.Abstracts
{
    class absOutlook
    {
        internal virtual void Error(string function, string msg)
        {
            clsErrorHandling.HandleError(new err(DateTime.Now, this.GetType().Name, function, msg));
        }

        internal virtual string TaskSubject(int TaskID, string subject)
        {
            return string.Format("Task{0}; {1}", TaskID, subject);
        }

        internal virtual void DecompileTaskSubject(string _TaskSubject, out int _TaskID, out string _Subject)
        {
            _TaskID = Convert.ToInt32(_TaskSubject.Substring(5, (_TaskSubject.Length - 5) - _TaskSubject.IndexOf(';')));
            _Subject = _TaskSubject.Substring(5 + _TaskID.ToString().Length + 2);
        }
    }
}
