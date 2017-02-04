using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskLogger.Classes
{
    class absDatabase
    {
        protected internal string ServerName = string.Empty;
        protected internal string Database = string.Empty;
        protected internal string Username = string.Empty;
        protected internal string Password = string.Empty;
        protected internal bool TrustedConnection = false;

        protected internal virtual string ConnectionString()
        {
            if (TrustedConnection)
                return string.Format("Server={0};Database={1};Trusted_Connection={2};", ServerName, Database, "True");
            else
                return string.Format("Server={0};Database={1};User ID={2};Password={3}", ServerName, Database, Username, Password);

        }

        protected internal int ExecuteNonQuery(string sSql)
        {
            int RowsAffected = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString()))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sSql, conn))
                        RowsAffected = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RowsAffected;
        }

        protected internal virtual bool CheckDatabaseExists(string databaseName)
        {
            string sqlCreateDBQuery;
            bool result = false;

            try
            {
                sqlCreateDBQuery = string.Format("SELECT database_id FROM sys.databases WHERE Name = '{0}';", databaseName);
                using (SqlConnection conn = new SqlConnection(ConnectionString()))
                {
                    conn.Open();
                 using (SqlCommand sqlCmd = new SqlCommand(sqlCreateDBQuery, conn))
                    {
                        using (SqlDataReader dr = sqlCmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                                result = true;
                        }
                    }   
                    conn.Close();
                    conn.Dispose();
                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        protected internal virtual bool CheckTableExists(string tableName)
        {
            string sqlCreateDBQuery;
            bool result = false;

            try
            {
                sqlCreateDBQuery = string.Format("SELECT TABLE_CATALOG from INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{0}';", tableName);
                using (SqlConnection conn = new SqlConnection(ConnectionString()))
                {
                    conn.Open();
                    using (SqlCommand sqlCmd = new SqlCommand(sqlCreateDBQuery, conn))
                    {
                        using (SqlDataReader dr = sqlCmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                                result = true;
                        }
                    }
                    conn.Close();
                    conn.Dispose();
                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        protected internal virtual bool CheckObjectExists(string objName)
        {
            string sqlCreateDBQuery;
            bool result = false;

            try
            {
                sqlCreateDBQuery = string.Format("SELECT * FROM sysobjects WHERE  id = object_id(N'[dbo].[{0}]') ;", objName);
                using (SqlConnection conn = new SqlConnection(ConnectionString()))
                {
                    conn.Open();
                    using (SqlCommand sqlCmd = new SqlCommand(sqlCreateDBQuery, conn))
                    {
                        using (SqlDataReader dr = sqlCmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                                result = true;
                        }
                    }
                    conn.Close();
                    conn.Dispose();
                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        protected internal virtual bool CheckServer()
        {
            bool _CanConnect = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString()))
                {
                    conn.Open();
                    _CanConnect = true;
                    conn.Close();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return _CanConnect;
        }

        #region SQL Queries

        #region Creation
        protected internal string CreateSP_OrganiseGroups()
        {
            string sSql = "CREATE PROCEDURE [dbo].[TL_OrganiseGroups] " +
                          "AS " +
                          "BEGIN " +
                          "SET NOCOUNT ON; " +
                          "DECLARE @xMAXID INT, @xStartID INT; " +
                          "SELECT @xMAXID = MAX(GroupID) FROM TL_Groups; " +
                          "SET @xStartID = 0; " +
                          " " +
                          "DELETE FROM TL_Groups WHERE GroupEnabled = 0; " +
                          " " +
                          "DECLARE @xGroups TABLE (GroupID INT, GroupName VARCHAR(100), GroupEnabled INT); " +
                          "IF @xMAXID <> (SELECT COUNT(*) FROM TL_Groups) " +
                          "BEGIN " +
                          "INSERT INTO @xGroups (GroupID, GroupName, GroupEnabled) SELECT GroupID, GroupName, GroupEnabled FROM TL_Groups; " +
                          " " +
                          "DELETE FROM TL_Groups WHERE GroupID IN (SELECT GroupID FROM @xGroups); " +
                          " " +
                          "WHILE @xStartID <= @xMAXID " +
                          "BEGIN " +
                          "IF @xStartID <> 0 " +
                          "BEGIN " +
                          "DECLARE @xOldID INT, @xOldName VARCHAR(100); " +
                          "SELECT @xOldID = ISNULL(MIN(GroupID),0) FROM @xGroups x WHERE GroupID >= @xStartID AND GroupID NOT IN (SELECT GroupID FROM TL_Groups g); " +
                          "SELECT @xOldName = GroupName FROM @xGroups X where GroupID = @xOldID; " +
                          " " +
                          "IF(SELECT COUNT(*) FROM TL_Groups WHERE GroupName = @xOldName AND GroupID <= @xOldID) = 0 " +
                          "BEGIN " +
                          "IF @xOldID <> @xStartID BEGIN UPDATE TL_Logs SET FK_GroupID = @xStartID WHERE FK_GroupID = @xOldID END; " +
                          " " +
                          "SET IDENTITY_INSERT TL_Groups ON; " +
                          "INSERT INTO TL_Groups(GroupID, GroupName, GroupEnabled) SELECT @xStartID, GroupName, GroupEnabled FROM @xGroups WHERE GroupID = @xOldID; " +
                          "SET IDENTITY_INSERT TL_Groups OFF; " +
                          "END " +
                          "END " +
                          " " +
                          "SET @xStartID = @xStartID + 1; " +
                          "END " +
                          "DECLARE @xMaxForReseed INT; " +
                          "SELECT @xMaxForReseed = MAX(GroupID) FROM TL_Groups; " +
                          "DBCC CHECKIDENT ('TL_Groups', RESEED, @xMaxForReseed); " +
                          "END " +
                          "END " +
                          " ";
            return sSql;
        }

        protected internal string CreateTable_Groups()
        {
            string sSql = "SET ANSI_NULLS ON; " +
                "" +
                " " +
                "SET QUOTED_IDENTIFIER ON; " +
                " " +
                " " +
                "SET ANSI_PADDING ON; " +
                "" +
                " " +
                "CREATE TABLE [dbo].[TL_Groups]( " +
                "[GroupID] [int] IDENTITY(1,1) NOT NULL " +
                ",[GroupName] [varchar](100) NULL" +
                ",[GroupEnabled] [int] NULL" +
                ",CONSTRAINT [PK_TL_Groups] " +
                "PRIMARY KEY CLUSTERED ([GroupID] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) " +
                "ON [PRIMARY]; " +
                "" +
                " " +
                "SET ANSI_PADDING OFF; " +
                " ";

            return sSql;
        }

        protected internal string CreateTable_Logs()
        {
            string sSql =
                "SET ANSI_NULLS ON; " +
                "" +
                "" +
                "SET QUOTED_IDENTIFIER ON; " +
                "" +
                " " +
                "SET ANSI_PADDING ON; " +
                "" +
                " " +
                "CREATE TABLE [dbo].[TL_Logs] (" +
                "[LogID] [int] IDENTITY(1, 1) NOT NULL" +
                ", [FK_GroupID] [int] NOT NULL" +
                ", [LogDescription] [varchar](200) NULL" +
                ", [RequestedBy] [varchar](100) NULL" +
                ", [CreatedTimeStamp] [datetime] NULL" +
                ", [CompletionTimestamp] [datetime] NULL" +
                ", [Status] [varchar](20) NULL" +
                ", CONSTRAINT [PK_TL_Logs] " +
                "PRIMARY KEY CLUSTERED ([LogID] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) " +
                "ON [PRIMARY]; " +
                "" +
                " " +
                "SET ANSI_PADDING OFF; " +
                " ";

            return sSql;
        }
        #endregion

        #region Task

        protected internal string Task_Close(int _id)
        {
            return
                string.Format(
                    "UPDATE TL_Logs SET Status = 'Closed', CompletionTimestamp = GETDATE() WHERE LogID = {0}", _id);
        }

        protected internal string Task_Insert(int _groupID, string _desc, string _reqBy)
        {
            return string.Format(
                "INSERT INTO TL_Logs(FK_GroupID, LogDescription, RequestedBy, CreatedTimestamp, Status) VALUES({0}, '{1}', '{2}', GETDATE(), 'Open');",
                _groupID, _desc, _reqBy);
        }

        protected internal string Task_GetID(int _groupID, string _desc, string _reqBy)
        {
            return string.Format(
                "SELECT MAX(LogID) FROM TL_Logs WHERE FK_GroupID = {0} AND Status = 'Open' AND LogDescription = '{1}' AND RequestedBy = '{2}';",
                _groupID, _desc, _reqBy);
        }

        protected internal string Task_GetOpen()
        {
            return
                "SELECT LogID [ID], GroupName [Grouping], LogDescription [Description], RequestedBy [Requested], CreatedTimestamp [Timestamp], Status FROM TL_Logs JOIN TL_Groups ON GroupID = FK_GroupID WHERE (CompletionTimestamp IS NULL AND Status = 'Open');";
        }

        #endregion

        #region Group

        protected internal string Group_Insert(string _groupName)
        {
            return string.Format("INSERT INTO TL_Groups(GroupName, GroupEnabled) VALUES('{0}', 1);", _groupName);
        }

        protected internal string Group_GetID(string _groupName)
        {
            return
                string.Format(
                    "SELECT TOP 1 GroupID FROM TL_Groups WHERE GroupName = '{0}' AND GroupEnabled = 1 ORDER BY GroupID DESC",
                    _groupName);
        }

        protected internal string Group_Organise()
        {
            return "EXEC dbo.TL_OrganiseGroups";
        }

        protected internal string Group_GetGroups()
        {
            return "SELECT GroupID, GroupName FROM TL_Groups WHERE GroupEnabled = 1;";
        }

        #endregion

        #endregion
    }
}
