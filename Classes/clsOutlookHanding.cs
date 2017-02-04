using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using TaskLogger.Classes.Abstracts;
using Outlook = Microsoft.Office.Interop.Outlook;
using err = TaskLogger.Classes.clsObjects.Error;

namespace TaskLogger.Classes
{
    internal class clsOutlookHanding : absOutlook
    {
        internal void CreateTask(int taskID, DateTime createTime, string subject, string message, string requested, string dueDate = null)
        {
            Outlook.Application app = null;
            Outlook.TaskItem oTask = null;
            try
            {
                app = new Outlook.Application();
                oTask = app.CreateItem(Outlook.OlItemType.olTaskItem);
                oTask.Subject = TaskSubject(taskID, subject);
                oTask.StartDate = createTime;
                oTask.Body = string.Format("Requested By:\t{0}\n\n{1}", requested, message);
                if (dueDate != null)
                    oTask.DueDate = Convert.ToDateTime(dueDate);
                oTask.Status = Outlook.OlTaskStatus.olTaskInProgress;
                oTask.UserProperties.Add(taskID.ToString(), Outlook.OlUserPropertyType.olNumber);
                oTask.Save();
                oTask.Close(Outlook.OlInspectorClose.olSave);
            }
            catch (Exception ex)
            {
                Error(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
            finally
            {
                if (app != null)
                    Marshal.ReleaseComObject(app);
                if (oTask != null)
                    Marshal.ReleaseComObject(oTask);
            }
        }

        internal List<Outlook.TaskItem> GetTaskList(Outlook.OlTaskStatus status)
        {
            List<Outlook.TaskItem> tItems = new List<Outlook.TaskItem>();
            Outlook.Application app = null;

            try
            {
                app = new Outlook.Application();
                foreach (
                    Outlook.TaskItem taskItem in
                        app.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderTasks).Items)
                {
                    if (taskItem != null)
                    {
                        tItems.Add(taskItem);
                        Marshal.ReleaseComObject(taskItem);
                    }
                }
            }
            catch (Exception ex)
            {
                Error(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
            finally
            {
                if (app != null)
                    Marshal.ReleaseComObject(app);
            }

            return tItems;
        }

        internal void CloseTask(Outlook.TaskItem tItem)
        {
            try
            {
                tItem.Status = Outlook.OlTaskStatus.olTaskComplete;
                tItem.Save();
            }
            catch (Exception ex)
            {
                Error(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
            
        }

        internal void DeleteTask(Outlook.TaskItem tItem)
        {
            Outlook.Application app = null;
            Outlook.Selection selection = null;
            Outlook.TaskItem task = null;

            try
            {
                app = new Outlook.Application();
                selection = app.ActiveExplorer().Selection;

                foreach (object objSelected in selection)
                {
                    if (objSelected is Outlook.TaskItem)
                    {
                        task = objSelected as Outlook.TaskItem;
                        if(objSelected == tItem)
                            task.Delete();
                        Marshal.ReleaseComObject(task);
                    }
                }
            }
            catch (Exception ex)
            {
                Error(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
            finally
            {
                if (selection != null)
                    Marshal.ReleaseComObject(selection);
                if (app != null)
                    Marshal.ReleaseComObject(app);
            }
        }

        internal List<Outlook.TaskItem> FindCompleteTaskList()
        {
            Outlook.Application app = new Outlook.Application();
            Outlook.Items folderItems = null;
            List<Outlook.TaskItem> tItems = new List<Outlook.TaskItem>();

            try
            {
                folderItems = app.GetNamespace("MAPI").GetDefaultFolder(Outlook.OlDefaultFolders.olFolderTasks).Items;
                folderItems.IncludeRecurrences = true;

                if (folderItems.Count > 0)
                {
                    foreach (Outlook.TaskItem item in folderItems)
                    {
                        if (item.Subject.Substring(0,4).ToUpper() == "TASK" && item.Status == Outlook.OlTaskStatus.olTaskComplete)
                            tItems.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Error(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
            finally
            {
                if (folderItems != null)
                    Marshal.ReleaseComObject(folderItems);
            }
            return tItems;
        }

        internal Outlook.TaskItem FindTask(string searchCriteria)
        {
            Outlook.Application app = new Outlook.Application();
            Outlook.TaskItem taskItem = null;
            Outlook.Items folderItems = null;

            try
            {
                folderItems = app.GetNamespace("MAPI").GetDefaultFolder(Outlook.OlDefaultFolders.olFolderTasks).Items;
                folderItems.IncludeRecurrences = true;

                if (folderItems.Count > 0)
                {
                    foreach (Outlook.TaskItem item in folderItems)
                    {
                        if (item.Subject == searchCriteria)
                            taskItem = item;
                    }
                }
            }
            catch (Exception ex)
            {
                Error(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
            finally
            {
                if (folderItems != null) 
                    Marshal.ReleaseComObject(folderItems);
            }
            return taskItem;
        }

        internal Outlook.TaskItem FindTask(int ID, string sub)
        {
            return FindTask(TaskSubject(ID, sub));
        }

    }
}
