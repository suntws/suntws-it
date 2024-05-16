using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;
using System.Reflection;
using System.IO;

namespace GT
{
    class DBAccess
    {
        private string _Con = "";
        public DBAccess()
        {
            _Con = "Data Source=" + Program.strLocalDbPath + ";Database=GTDB" + Program.strPlantName + ";User ID=sa;Password=" + Program.strLocalDbPass
                + ";Trusted_Connection=False;";
           // _Con = "Data Source=" + Program.strLocalDbPath + ";Database=GTDB" + Program.strPlantName + ";User ID=sa;Password=" + Program.strLocalDbPass
           //    + ";Trusted_Connection=False;";
        }
        public string ConnectionString
        {
            get { return _Con; }
            set { _Con = value; }
        }
        private static List<int> RetryEceptionList = new List<int> { 40197, 40501, 10053 };
        public enum Return_Type { DataSet, DataView, DataTable, Nothing, DataTableCollection }

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
                    GetValue(connection, Ds, Da, ++retryCount);
                }
                throw;
            }
        }
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
                cmd.CommandTimeout = 1200;
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
                throw new Exception("Error in SUN-TWS GT Data Access " + Ex.Message);
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
                command.CommandTimeout = 1200;
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
                throw new Exception("Error in SUN-TWS GT Data Access " + Ex.Message);
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
        public int ExecuteNonQuery_SP(string SpName, SqlParameter[] SpParams)
        {
            SqlConnection connection = new SqlConnection(_Con);
            SqlCommand command = new SqlCommand();
            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SpName;
                command.CommandTimeout = 1200;
                foreach (SqlParameter Sp in SpParams)
                {
                    command.Parameters.Add(Sp);
                }
                return (int)ExecuteNonQueryWithRetry(command, connection, 0);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error in SUN-TWS GT Data Access " + Ex.Message);
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
            command.CommandTimeout = 1200;
            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SpName;
                return (int)ExecuteNonQueryWithRetry(command, connection, 0);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error in SUN-TWS GT Data Access " + Ex.Message);
            }
            finally
            {
                command.Dispose();
                connection.Close();
                connection.Dispose();
            }
        }
        public object ExecuteScalar_SP(string SpName, SqlParameter[] SpParams)
        {
            SqlConnection connection = new SqlConnection(_Con);
            SqlCommand command = new SqlCommand();
            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SpName;
                command.CommandTimeout = 1200;
                foreach (SqlParameter Sp in SpParams)
                {
                    command.Parameters.Add(Sp);
                }
                return ExecuteScalarWithRetry(command, connection, 0);
            }

            catch (Exception ex)
            {
                throw new Exception("Error in SUN-TWS GT Data Access " + ex.Message);
            }
            finally
            {
                command.Parameters.Clear();
                command.Dispose();
                connection.Close();
                connection.Dispose();
            }
        }
        public object ExecuteScalar_SP(string SpName)
        {
            SqlConnection connection = new SqlConnection(_Con);
            SqlCommand command = new SqlCommand();
            try
            {

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SpName;
                command.CommandTimeout = 1200;
                return ExecuteScalarWithRetry(command, connection, 0);
            }

            catch (Exception Ex)
            {
                throw new Exception("Error in SUN-TWS GT Data Access " + Ex.Message);
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
    }
}
