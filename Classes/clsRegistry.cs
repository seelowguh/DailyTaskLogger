using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using err = TaskLogger.Classes.clsObjects.Error;

namespace TaskLogger.Classes
{
    internal class clsRegistry : IDisposable
    {
        private void Error(string function, string msg)
        {
            clsErrorHandling.HandleError(new err(DateTime.Now, this.GetType().Name, function, msg));
        }

        private RegistryKey gKey = Registry.CurrentUser.CreateSubKey("SensataTechnologies").CreateSubKey("TaskLogger");

        private object ReadRegValue(string Name, object DefaultValue)
        {
            try
            {
                return gKey.GetValue(Name, DefaultValue);
            }
            catch (Exception ex)
            {
                return DefaultValue;
            }
        }

        private void SetRegValue(string Name, object Value, RegistryValueKind rvk)
        {
            try
            {
                gKey.SetValue(Name, Value, rvk);
            }
            catch (Exception ex)
            {
                Error(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        internal clsObjects.ConfigurationDetails ReadReg()
        {
            return new clsObjects.ConfigurationDetails(ReadRegValue("ServerName", "").ToString(), ReadRegValue("Database", "").ToString(),
                ReadRegValue("UserName", "").ToString(), ReadRegValue("Password", "").ToString(),
                bool.Parse(ReadRegValue("UseWindowsCredentials", false).ToString()));
        }

        internal bool RegExists()
        {
            bool _regExists = false;
            try
            {
                if (gKey != null)
                    _regExists = ReadRegValue("ServerName", null) != null;
            }
            catch (Exception ex)
            {
                _regExists = false;
            }

            return _regExists;
        }

        internal void CreateReg()
        {
            try
            {
                CreateReg(string.Empty, string.Empty, string.Empty, string.Empty, false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal void CreateReg(string Servername, string DatabaseName, string Username, string Password, bool WindowsCred)
        {
            try
            {
                SetRegValue("ServerName", Servername, RegistryValueKind.String);
                SetRegValue("Database", DatabaseName, RegistryValueKind.String);
                SetRegValue("UserName", Username, RegistryValueKind.String);
                SetRegValue("Password", Password, RegistryValueKind.String);
                SetRegValue("UseWindowsCredentials", WindowsCred, RegistryValueKind.String);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {
            if (gKey != null)
            {
                gKey.Close();
                gKey.Dispose();
            }
        }
    }
}
