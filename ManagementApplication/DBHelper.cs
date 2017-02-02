using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ManagementApplication
{
         class DBHelper
        {
            private static SqlCommand cmd = null;
            private static SqlDataReader dr = null;

            public int RowCount { get; private set; }

            SqlConnection sqlCnn = new SqlConnection();
            SqlCommand sqlCmd = new SqlCommand();

            //数据库连接Connection对象
            //public static SqlConnection conn= new SqlConnection("Data Source=WIN7U-20121128L;Initial Catalog=HFUTDorManagement;Integrated Security=True");
		 	public static SqlConnection conn= new SqlConnection("Server=localhost;Database=testdb;User ID=SA;Password=Password1!");

            public DBHelper()
            { }

            public static SqlDataReader GetResult(string sql)
            {
                try
                {
                    cmd = new SqlCommand();
                    cmd.CommandText = sql;
                    cmd.Connection = conn;
                    cmd.Connection.Open();
                    dr = cmd.ExecuteReader();
                    return dr;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return null;
                }
                finally
                {
                    dr.Close();
                    cmd.Connection.Close();
                }
            }

            

            //对Select语句,返回int型结果集

            public static int GetSqlResult(string sql)
            {
                try
                {
                    cmd = new SqlCommand();
                    cmd.CommandText = sql;
                    cmd.Connection = conn;
                    cmd.Connection.Open();

                    int a = (int)cmd.ExecuteScalar();
                    return a;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return -1;

                }
                finally
                {
                    cmd.Connection.Close();
                }
            }

            

            //对Update,Insert和Delete语句，返回该命令所影响的行数

            /*public static int GetDsqlResult(string sql)
            {
                try
                {
                    cmd = new SqlCommand();
                    cmd.CommandText = sql;
                    cmd.Connection = conn;
                    cmd.Connection.Open();

                    cmd.ExecuteNonQuery();
                    return 1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return -1;
                }
                finally
                {
                    cmd.Connection.Close();
                }
            }

            */

        }
    }

