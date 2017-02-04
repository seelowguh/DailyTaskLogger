using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TaskLogger.Classes.Abstracts;

namespace TaskLogger.Classes
{
    internal static class clsErrorHandling
    {
        internal static List<clsObjects.Error> ErrorList = new List<clsObjects.Error>();

        internal static void HandleError(clsObjects.Error err)
        {
            try
            {
                string sApplicationName = "TaskLogger";
                if (!EventLog.SourceExists(sApplicationName))
                    EventLog.CreateEventSource(sApplicationName, "Application");

                EventLog.WriteEntry(sApplicationName,
                    string.Format("Location:{3}; Function:{2}; Timestamp:{1}; Message:{0}; ", err.ErrorMessage,
                        err.ErrorDateTime, err.Function, err.Location), EventLogEntryType.Warning);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                ErrorList.Add(err);
            }
    }
    }
}
