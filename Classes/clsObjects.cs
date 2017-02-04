using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskLogger.Classes
{
    internal class clsObjects
    {
        internal struct ConfigurationDetails
        {
            public string ServerName;
            public string Database;
            public string Username;
            public string Password;
            public bool UseWindowsCredentials;

            public ConfigurationDetails(string _ServerName, string _DatabaseName, string _Username, string _Password,
                bool _UseWindowsCredentials)
            {
                ServerName = _ServerName;
                Database = _DatabaseName;
                Username = _Username;
                Password = _Password;
                UseWindowsCredentials = _UseWindowsCredentials;
            }
        }

        internal struct Group
        {
            public int ID;
            public string GroupName;

            public Group(int _id, string _groupname)
            {
                ID = _id;
                GroupName = _groupname;
            }
        }

        internal class Error
        {
            public DateTime ErrorDateTime { get; set; }
            public string Location { get; set; }
            public string Function { get; set; }
            public string ErrorMessage { get; set; }

            public Error(DateTime dt, string loc, string func, string msg)
            {
                ErrorDateTime = dt;
                Location = loc;
                Function = func;
                ErrorMessage = msg;
            }
        }
    }
}
