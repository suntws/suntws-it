using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Serialization;
using System.Xml;
using System.Configuration;
using System.Text;
using System.Web.UI.HtmlControls;

/// <summary>
/// This Class can be used in any Application as a Common Data Accessing Layer.
/// It provides methods to Communicate with SQL Server.
/// </summary>
/// 
///<remarks>
/// Desc    : Data Access Class
/// Done By : NATHAN
/// Date    : 10-07-2013
/// 
///</remarks>
namespace COTS
{
    public class DataAccess : IDisposable
    {
        private string _Con = ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString;
        private static List<int> RetryEceptionList = new List<int> { 40197, 40501, 10053 };
        public DataAccess(string ConStr)
        {
            _Con = ConStr;
        }
        public string ConnectionString
        {
            get { return _Con; }
            set { _Con = value; }
        }
        public enum Return_Type { DataSet, DataView, DataTable, Nothing, DataTableCollection }
        public object ExecuteReader_SP(string SpName, SqlParameter[] SpParams, Return_Type RetType)
        {
            SqlConnection connection = new SqlConnection(_Con);
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            try
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = SpName;
                cmd.CommandTimeout = 300;
                foreach (SqlParameter Sp in SpParams)
                {
                    cmd.Parameters.Add(Sp);
                }

                GetValue(connection, ds, da, 0);
                if (RetType == Return_Type.DataSet)
                    return ds;
                else if (RetType == Return_Type.DataView)
                {
                    if (ds.Tables.Count > 0)
                        return ds.Tables[0].DefaultView;
                    else
                        return null;
                }
                else if (RetType == Return_Type.DataTable)
                {
                    if (ds.Tables.Count > 0)
                        return ds.Tables[0];
                    else
                        return null;
                }

                else if (RetType == Return_Type.DataTableCollection)
                    return ds.Tables;
                else
                    return null;

            }
            catch (Exception Ex)
            {
                throw new Exception("Error in SUN-TWS Data Access " + Ex.Message);
            }

            finally
            {
                cmd.Dispose();
                cmd = null;
                da.Dispose();
                da = null;
                ds.Dispose();
                ds = null;
                connection.Close();
                connection.Dispose();
                connection = null;
            }
        }
        public object ExecuteReader_SP(string SpName, Return_Type RetType)
        {
            SqlConnection connection = new SqlConnection(_Con);
            SqlCommand command = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(command);
            try
            {
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SpName;
                command.CommandTimeout = 60;
                GetValue(connection, ds, da, 0);
                
                if (RetType == Return_Type.DataSet)
                    return ds;
                else if (RetType == Return_Type.DataView)
                    return ds.Tables[0].DefaultView;
                else if (RetType == Return_Type.DataTable)
                {
                    if (ds.Tables.Count > 0)
                        return ds.Tables[0];
                    else
                        return null;
                }
                else
                    return null;
            }


            catch (Exception Ex)
            {
                throw new Exception("Error in SUN-TWS Data Access " + Ex.Message);
            }

            finally
            {
                command.Dispose();
                command = null;
                da.Dispose();
                da = null;
                ds.Dispose();
                ds = null;
                connection.Close();
                connection.Dispose();
                connection = null;
            }
        }
        public int ExecuteNonQuery(string SQL_Query)
        {
            SqlConnection connection = new SqlConnection(_Con);
            SqlCommand command = new SqlCommand(SQL_Query, connection);
            try
            {
                return (int)ExecuteNonQueryWithRetry(command, connection, 0);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error in SUN-TWS Data Access " + Ex.Message);
            }

            finally
            {
                command.Parameters.Clear();
                command.Dispose();
                connection.Close();
                connection.Dispose();
            }
        }
        public int ExecuteNonQuery_SP(string SpName, SqlParameter[] SpParams)
        {
            SqlConnection connection = new SqlConnection(_Con);
            SqlCommand command = new SqlCommand();
            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SpName;
                command.CommandTimeout = 120;
                foreach (SqlParameter Sp in SpParams)
                {
                    command.Parameters.Add(Sp);
                }
                return (int)ExecuteNonQueryWithRetry(command, connection, 0);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error in SUN-TWS Data Access " + Ex.Message);
            }
            finally
            {
                command.Parameters.Clear();
                command.Dispose();
                connection.Close();
                connection.Dispose();
            }
        }
        public int ExecuteNonQuery_SP(string SpName)
        {
            SqlConnection connection = new SqlConnection(_Con);
            SqlCommand command = new SqlCommand();
            //Command Time out was changed for Updated Quries  TimeOut Error
            // Code Changed by NATHAN on 21-03-2013.
            command.CommandTimeout = 60;
            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SpName;
                return (int)ExecuteNonQueryWithRetry(command, connection, 0);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error in SUN-TWS Data Access " + Ex.Message);
            }
            finally
            {
                command.Dispose();
                connection.Close();
                connection.Dispose();
            }
        }
        public XmlDocument GetDataAsXML(string SpName, SqlParameter[] SpParams)
        {
            XmlDocument xoDoc = new XmlDocument();
            XmlElement xRows;

            SqlDataReader dataReader;
            SqlConnection Cn = new SqlConnection(_Con);
            SqlCommand Cmd = new SqlCommand();
            try
            {
                Cn.Open();
                Cmd.Connection = Cn;
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandText = SpName;
                
                Cmd.CommandTimeout = 120;
                foreach (SqlParameter Sp in SpParams)
                {
                    Cmd.Parameters.Add(Sp);
                }

                dataReader = Cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (dataReader.HasRows)
                {
                    xRows = xoDoc.CreateElement("dataout");
                    xoDoc.AppendChild(xRows);

                    XmlNode root = xoDoc.DocumentElement;

                    while (dataReader.Read())
                    {
                        xRows = xoDoc.CreateElement("item");
                        for (int i = 0; i < dataReader.FieldCount; i++)
                        {
                            xRows.SetAttribute(dataReader.GetName(i).ToLower(), dataReader[i].ToString());
                        }
                        root.AppendChild(xRows);
                    }

                }
                return xoDoc;
            }

            catch (Exception Ex)
            {
                return null;
                throw new Exception("Error in SUN-TWS Data Access " + Ex.Message);
            }

            finally
            {
                Cmd.Dispose();
                Cmd = null;
                Cn.Close();
                Cn.Dispose();
                Cn = null;
            }
        }
        public XmlDocument GetDataAsXML(string strQuery)
        {
            XmlDocument xoDoc = new XmlDocument();
            XmlElement xRows;

            SqlDataReader dataReader;
            SqlConnection oConn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings.Get("Dsn").ToString());
            SqlCommand oCmd = new SqlCommand(strQuery, oConn);
            oCmd.CommandTimeout = 500;

            try
            {
                oConn.Open();
                dataReader = oCmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (dataReader.HasRows)
                {
                    xRows = xoDoc.CreateElement("dataout");
                    xoDoc.AppendChild(xRows);

                    XmlNode root = xoDoc.DocumentElement;

                    while (dataReader.Read())
                    {
                        xRows = xoDoc.CreateElement("item");
                        for (int i = 0; i < dataReader.FieldCount; i++)
                        {
                            xRows.SetAttribute(dataReader.GetName(i).ToLower(), dataReader[i].ToString());
                        }
                        root.AppendChild(xRows);
                    }

                }
                oCmd.Dispose();
                dataReader.Close();
                oConn.Close();
            }
            catch { }
            return xoDoc;

        }
        public DataRow GetSingleRow(string strQry)
        {
            DataRow dr = null;
            using (SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings.Get("Dsn")))
            {
                SqlDataAdapter da = new SqlDataAdapter(strQry, cn);
                cn.Open();
                DataTable ds = new DataTable();
                da.Fill(ds);
                if (ds.Rows.Count > 0)
                    dr = ds.Rows[0];
            }
            return dr;
        }
        public object ExecuteScalar_SP(string SpName, SqlParameter[] SpParams)
        {
            SqlConnection connection = new SqlConnection(_Con);
            SqlCommand command = new SqlCommand();
            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SpName;
                foreach (SqlParameter Sp in SpParams)
                {
                    command.Parameters.Add(Sp);
                }
               return ExecuteScalarWithRetry(command, connection,0);
            }
          
            catch (Exception ex)
            {
                throw new Exception("Error in SUN-TWS Data Access " + ex.Message);
            }
            finally
            {
                command.Parameters.Clear();
                command.Dispose();
                connection.Close();
                connection.Dispose();
            }
        }
        private static object ExecuteNonQueryWithRetry(SqlCommand Cmd, SqlConnection Cn, int retryCount)
        {
            object executeScalarWithRetry;
            try
            {
                Cn.Open();
                Cmd.Connection = Cn;
                executeScalarWithRetry = Cmd.ExecuteNonQuery();
            }
            catch(SqlException ex)
            {
                if(RetryEceptionList.Contains(ex.Number)&& retryCount<3)
                {
                    return ExecuteNonQueryWithRetry(Cmd, Cn, ++retryCount);
                }
                throw;
            }
            return executeScalarWithRetry;
        }
        private static object ExecuteNonQueryWithRetryTn(SqlCommand Cmd, SqlConnection Cn, int retryCount)
        {
            object executeScalarWithRetry;
            try
            {
                Cmd.Connection = Cn;
                executeScalarWithRetry = Cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                if (RetryEceptionList.Contains(ex.Number) && retryCount < 3)
                {
                    return ExecuteNonQueryWithRetry(Cmd, Cn, ++retryCount);
                }
                throw;
            }
            return executeScalarWithRetry;
        }
        private static object ExecuteScalarWithRetry(SqlCommand Cmd, SqlConnection Cn, int retryCount)
        {
            object executeScalarWithRetry;
            try
            {
                Cn.Open();
                Cmd.Connection = Cn;
                executeScalarWithRetry = Cmd.ExecuteScalar();
            }
            catch (SqlException ex)
            {
                if (RetryEceptionList.Contains(ex.Number) && retryCount < 3)
                {
                    return ExecuteScalarWithRetry(Cmd, Cn, ++retryCount);
                }
                throw;
            }
            return executeScalarWithRetry;
        }
        public object ExecuteScalar_SP(string SpName)
        {
            SqlConnection connection = new SqlConnection(_Con);
            SqlCommand command = new SqlCommand();
            try
            {
                
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SpName;
                return ExecuteScalarWithRetry(command, connection, 0);
            }

            catch (Exception Ex)
            {
                throw new Exception("Error in SUN-TWS Data Access " + Ex.Message);
            }

            finally
            {
                command.Parameters.Clear();
                command.Dispose();
                connection.Close();
                connection.Dispose();
            }
        }
        public object ExecuteReader(string SQL_Query, Return_Type RetType)
        {
            SqlConnection connection = new SqlConnection(_Con);
            SqlCommand Cmd = new SqlCommand(SQL_Query, connection);
            SqlDataAdapter da = new SqlDataAdapter(Cmd);
            DataSet ds = new DataSet();

            try
            {
                GetValue(connection, ds, da, 0);
                if (RetType == Return_Type.DataSet)
                    return ds;
                else if (RetType == Return_Type.DataView)
                    return ds.Tables[0].DefaultView;
                else if (RetType == Return_Type.DataTable)
                {
                    if (ds.Tables.Count > 0)
                        return ds.Tables[0];
                    else
                        return null;
                }
                else
                    return null;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error in SUN-TWS Data Access " + Ex.Message);
            }

            finally
            {
                Cmd.Parameters.Clear();
                Cmd.Dispose();
                da.Dispose();
                ds.Dispose();
                connection.Close();
                connection.Dispose();
            }
        }
        public object ExecuteScalar(string SQL_Query)
        {
            SqlConnection connection = new SqlConnection(_Con);
            SqlCommand command = new SqlCommand(SQL_Query, connection);
            try
            {
                return ExecuteScalarWithRetry(command, connection, 0);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in SUN-TWS Data Access " + ex.Message);
            }

            finally
            {
                command.Dispose();
                connection.Close();
                connection.Dispose();
            }


        }
        public bool HasRows(string SQL_Query, SqlParameter[] SpParams)
        {
            SqlConnection Cn = new SqlConnection(_Con);
            SqlCommand Cmd = new SqlCommand(SQL_Query, Cn);
            SqlDataReader Cnt = null;
            foreach (SqlParameter Sp in SpParams)
            {
                Cmd.Parameters.Add(Sp);
            }
            try
            {
                Cn.Open();
                Cnt = Cmd.ExecuteReader();
                return Cnt.HasRows;

            }
            catch (Exception Ex)
            {
                throw new Exception("Error in SUN-TWS Data Access " + Ex.Message);
            }

            finally
            {
                Cnt.Dispose();
                Cnt = null;
                Cmd.Dispose();
                Cn.Close();
                Cn.Dispose();
                Cmd = null;
                Cn = null;
            }
        }
        public bool HasRows(string SQL_Query)
        {
            SqlConnection Cn = new SqlConnection(_Con);
            if (SQL_Query.Split(' ').Length != 0)
                SQL_Query = "select top 1 * from " + SQL_Query;

            SqlCommand Cmd = new SqlCommand(SQL_Query, Cn);
            SqlDataReader Cnt = null;

            try
            {
                Cn.Open();
                Cnt = Cmd.ExecuteReader();
                return Cnt.HasRows;

            }
            catch (Exception Ex)
            {
                throw new Exception("Error in SUN-TWS Data Access " + Ex.Message);
            }

            finally
            {
                Cnt.Dispose();
                Cnt = null;
                Cmd.Dispose();
                Cn.Close();
                Cn.Dispose();
                Cmd = null;
                Cn = null;
            }
        }
        public int MaxValue(string TableName, string ColumnName)
        {
            SqlConnection Cn = new SqlConnection(_Con);
            string SQL_Query = "select max(" + ColumnName + ") from " + TableName;
            SqlCommand Cmd = new SqlCommand(SQL_Query, Cn);
            try
            {
                Cn.Open();
                object obj = Cmd.ExecuteScalar();
                if (Convert.IsDBNull(obj))
                    return 0;
                else
                    return (int)obj;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error in SUN-TWS Data Access " + Ex.Message);
            }

            finally
            {
                Cmd.Dispose();
                Cn.Close();
                Cn.Dispose();
                Cmd = null;
                Cn = null;
            }

        }
        #region IDisposable Members
        public void Dispose()
        {
            _Con = null;
           
        }
        #endregion
        public int ExecuteNonQuery_SP(string SpName, SqlConnection connection, SqlTransaction Tn, SqlParameter[] SpParams)
        {
            SqlCommand command = new SqlCommand(SpName, connection, Tn);
            try
            {
                
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SpName;
                command.CommandTimeout = 60;
                foreach (SqlParameter Sp in SpParams)
                {
                    command.Parameters.Add(Sp);
                }
                return (int)ExecuteNonQueryWithRetryTn(command, connection, 0);
            }

            catch (Exception Ex)
            {
                throw new Exception("Error in SUN-TWS Data Access " + Ex.Message);
            }

            finally
            {
                command.Dispose();
                command = null;
            }
        }
        public static class Serialization
        {
            public static bool Serialize(string FileName, Object Obj)
            {
                try
                {
                    XmlSerializer s = new XmlSerializer(Obj.GetType());
                    System.IO.TextWriter Txt = new System.IO.StreamWriter(FileName);
                    s.Serialize(Txt, Obj);
                    Txt.Flush();
                    Txt.Close();
                    return true;
                }
                catch
                {
                    return false;
                }

            }
            public static bool Serialize(string FileName, Object Obj, Return_Type Ty)
            {
                try
                {
                    XmlSerializer s = new XmlSerializer(Obj.GetType());
                    System.IO.TextWriter Txt = new System.IO.StreamWriter(FileName);

                    DataTable Dt = new DataTable();

                    s.Serialize(Txt, Obj);
                    Txt.Flush();
                    Txt.Close();
                    return true;
                }
                catch
                {
                    return false;
                }

            }
            public static bool Deserialize()
            {
                return false;
            }

        }
        public object ExecuteReader_SP(string SpName, SqlParameter[] SpParams, Return_Type RetType, SqlConnection sqlCon, SqlTransaction sqlTran)
        {
            SqlCommand command = new SqlCommand(SpName, sqlCon, sqlTran);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(command);

            try
            {
                command.Connection = sqlCon;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SpName;
                command.CommandTimeout = 300;
                foreach (SqlParameter Sp in SpParams)
                {
                    command.Parameters.Add(Sp);
                }

                GetValueWithTran(sqlCon, ds, da, 0);
                if (RetType == Return_Type.DataSet)
                    return ds;
                else if (RetType == Return_Type.DataView)
                {
                    if (ds.Tables.Count > 0)
                        return ds.Tables[0].DefaultView;
                    else
                        return null;
                }
                else if (RetType == Return_Type.DataTable)
                {
                    if (ds.Tables.Count > 0)
                        return ds.Tables[0];
                    else
                        return null;
                }

                else if (RetType == Return_Type.DataTableCollection)
                    return ds.Tables;
                else
                    return null;

            }
            catch (Exception Ex)
            {
                throw new Exception("Error in SUN-TWS Data Access " + Ex.Message);
            }

            finally
            {
                command.Parameters.Clear();
                command.Dispose();
                da.Dispose();
                ds.Dispose();
            }
        }

        private void GetValue(SqlConnection connection, DataSet Ds, SqlDataAdapter Da, int retryCount)
        {
            try
            {
                connection.Open();
                Da.Fill(Ds, "T1");
            }
            catch (SqlException ex)
            {
                if (RetryEceptionList.Contains(ex.Number) && retryCount < 3)
                {
                    GetValue(connection,Ds, Da, ++retryCount);
                }
                throw;
            }
        }
        private void GetValueWithTran(SqlConnection connection, DataSet Ds, SqlDataAdapter Da, int retryCount)
        {
            try
            {
                Da.Fill(Ds, "T1");
            }
            catch (SqlException ex)
            {
                if (RetryEceptionList.Contains(ex.Number) && retryCount < 3)
                {
                    GetValue(connection, Ds, Da, ++retryCount);
                }
                throw;
            }
        }
    }
}
