using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using err = TaskLogger.Classes.clsObjects.Error;

namespace TaskLogger.Classes
{
    class clsDatabaseHandling : absDatabase
    {
        internal clsDatabaseHandling(string _ServerName,string _Database,string _Username ,string _Password ,bool _TrustedConnection )
        {
            ServerName = _ServerName;
            Username = _Username;
            Password = _Password;
            TrustedConnection = _TrustedConnection;
            Database = _Database;
        }

        private void Error(string function, string msg)
        {
            clsErrorHandling.HandleError(new err(DateTime.Now, this.GetType().Name, function, msg));
        }

        internal List<clsObjects.Group> GetGroups()
        {
            //  Sorts groups
            ExecuteNonQuery(Group_Organise());

            List<clsObjects.Group> outputlist = new List<clsObjects.Group>();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString()))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(Group_GetGroups(), conn))
                    using (SqlDataReader dr = cmd.ExecuteReader())
                        while (dr.Read())
                            outputlist.Add(new clsObjects.Group(dr.GetInt32(0), dr.GetString(1)));
                    conn.Close();
                }
            }
            catch (OdbcException ex)
            {
                Error(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
            catch (Exception ex)
            {
                Error(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
            return outputlist;
        }

        internal int InsertNewGroup(string GroupName)
        {
            int outputvalue = -1;
            int rowsAffected = 0;
            try
            {
                rowsAffected = ExecuteNonQuery(Group_Insert(GroupName));
                if(rowsAffected != 0)
                    using (SqlConnection conn = new SqlConnection(ConnectionString()))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand(Group_GetID(GroupName), conn))
                        using (SqlDataReader dr = cmd.ExecuteReader())
                            while (dr.Read())
                                outputvalue = dr.GetInt32(0);
                        conn.Close();
                    }
            }
            catch (OdbcException ex)
            {
                Error(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
            catch (Exception ex)
            {
                Error(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
            return outputvalue;
        }

        internal int InsertNewTask(int GroupID, string Desc, string RequestBy)
        {
            int TaskID = 0;
            
            try
            {
            ExecuteNonQuery(Task_Insert(GroupID, Desc, RequestBy));

            string sSql = Task_GetID(GroupID, Desc, RequestBy);

                using (SqlConnection conn = new SqlConnection(ConnectionString()))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sSql, conn))
                    using (SqlDataReader dr = cmd.ExecuteReader())
                        while (dr.Read())
                            TaskID = dr.GetInt32(0);
                    conn.Close();
                }
            }
            catch (OdbcException ex)
            {
                Error(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
            catch (Exception ex)
            {
                Error(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return TaskID;
        }

        internal DataTable GetOpenTasks()
        {
            string sSql = Task_GetOpen();
            DataTable dg = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString()))
                {
                    conn.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(sSql, conn))
                        da.Fill(dg);
                    conn.Close();
                }
            }
            catch (OdbcException ex)
            {
                Error(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
            catch (Exception ex)
            {
                Error(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
            return dg;
        }

        internal void CloseTask(int _ID)
        {
            ExecuteNonQuery(Task_Close(_ID));
        }

        internal void CheckAndCreateTables()
        {
            try
            {
                if(!CheckServer())
                    throw new Exception("Cannot connect to server.");
                if(!CheckDatabaseExists(Database))
                    throw  new Exception("Cannot connect to database.");
                if (!CheckTableExists("TL_Groups"))
                    ExecuteNonQuery(CreateTable_Groups());
                if (!CheckTableExists("TL_Logs"))
                    ExecuteNonQuery(CreateTable_Logs());
                if (!CheckObjectExists("TL_OrganiseGroups"))
                    ExecuteNonQuery(CreateSP_OrganiseGroups());
            }
            catch (Exception ex)
            {
                Error(MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }

        }

    }
}
