using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace HDDNCONIAMP.Utils
{
    /// <summary> 
    /// 说明：这是一个针对System.Data.SQLite的数据库常规操作封装的通用类。 
    /// huangjie add 2014-10-31
    /// </summary>    
    public class SQLiteDBHelper
    {
        private string dbPath = Application.StartupPath + "\\DataBase.db3";
        private string connectionString = string.Empty;

        #region 数据库连接必要条件参数

        private SQLiteConnection dbConnection = null;
        /// <summary>
        /// 数据库连接
        /// </summary>
        public SQLiteConnection DbConnection
        {
            get
            {
                if (this.dbConnection == null)
                {
                    // 若没打开，就变成自动打开关闭的
                    this.Open();
                    this.AutoOpenClose = true;
                }
                return this.dbConnection;
            }
            set
            {
                this.dbConnection = value;
            }
        }
        private SQLiteCommand dbCommand = null;
        /// <summary>
        /// 命令
        /// </summary>
        public SQLiteCommand DbCommand
        {
            get
            {
                return this.dbCommand;
            }
            set
            {
                this.dbCommand = value;
            }
        }
        private SQLiteDataAdapter dbDataAdapter = null;
        /// <summary>
        /// 数据库适配器
        /// </summary>
        public SQLiteDataAdapter DbDataAdapter
        {
            get
            {
                return this.dbDataAdapter;
            }
            set
            {
                this.dbDataAdapter = value;
            }
        }

        /// <summary>
        /// 数据库连接
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return this.connectionString;
            }
            set
            {
                this.connectionString = value;
            }
        }

        private SQLiteTransaction dbTransaction = null;
        private bool inTransaction = false;
        /// <summary>
        /// 是否已采用事务
        /// </summary>
        public bool InTransaction
        {
            get
            {
                return this.inTransaction;
            }
            set
            {
                this.inTransaction = value;
            }
        }
        private bool autoOpenClose = false;
        /// <summary>
        /// 默认打开关闭数据库选项（默认为否）
        /// </summary>
        public bool AutoOpenClose
        {
            get
            {
                return autoOpenClose;
            }
            set
            {
                autoOpenClose = value;
            }
        }

        #endregion


        /// <summary> 
        /// 构造函数 
        /// </summary> 
        /// <param name="dbPath">SQLite数据库文件路径</param> 
        public SQLiteDBHelper()
        {
            this.connectionString = "Data Source=" + dbPath;
        }
        /// <summary>
        /// 这时主要的获取数据库连接的方法
        /// </summary>
        /// <returns>数据库连接</returns>
        public IDbConnection Open()
        {
            this.Open(this.ConnectionString);
            return this.dbConnection;
        }

        /// <summary>
        /// 获得新的数据库连接
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <returns>数据库连接</returns>
        public IDbConnection Open(string connectionString)
        {
            // 若是空的话才打开
            if (this.dbConnection == null || this.dbConnection.State == ConnectionState.Closed)
            {
                this.ConnectionString = connectionString;
                this.dbConnection = new SQLiteConnection(this.ConnectionString);
                this.dbConnection.Open();
            }

            this.AutoOpenClose = false;
            return this.dbConnection;
        }

        #region 建立本地数据库

        /// <summary> 
        /// 创建SQLite数据库文件 
        /// </summary> 
        /// <param name="dbPath">要创建的SQLite数据库文件路径</param> 
        public void CreateDB()
        {
            if (!System.IO.File.Exists(dbPath))
            {
                // 自动打开
                if (this.DbConnection == null)
                {
                    this.AutoOpenClose = true;
                    this.Open();
                }
                else if (this.DbConnection.State == ConnectionState.Closed)
                {
                    this.Open();
                }
                this.dbCommand = this.DbConnection.CreateCommand();
                this.dbCommand.CommandText = "CREATE TABLE Demo(id integer NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE)";
                this.dbCommand.ExecuteNonQuery();
                this.dbCommand.CommandText = "DROP TABLE Demo";
                this.dbCommand.ExecuteNonQuery();
            }
        }

        #endregion

        #region 判断表是否存在

        /// <summary>
        /// 判断表是否存在
        /// true:存在；false:不存在
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public bool IsExistTable(string tableName)
        {
            // 自动打开
            if (this.DbConnection == null)
            {
                this.AutoOpenClose = true;
                this.Open();
            }
            else if (this.DbConnection.State == ConnectionState.Closed)
            {
                this.Open();
            }
            this.dbCommand = this.DbConnection.CreateCommand();
            string strSQL = string.Format("select * from sqlite_master where [type]='table' and [name]='{0}'", tableName);
            this.dbCommand.CommandText = strSQL;
            DataSet ds = new DataSet();
            dbDataAdapter = new SQLiteDataAdapter(this.dbCommand);
            dbDataAdapter.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
                return true;
            else
                return false;
        }

        #endregion

        #region ExecuteNonQuery

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="commandText">sql查询</param>
        /// <returns>影响行数</returns>
        public int ExecuteNonQuery(string commandText)
        {
            return ExecuteNonQuery(commandText, null);
        }
        /// <summary> 
        /// 对SQLite数据库执行增删改操作，返回受影响的行数。 
        /// </summary> 
        /// <param name="commandText">要执行的增删改的SQL语句</param> 
        /// <param name="parameters">执行增删改语句所需要的参数，参数必须以它们在SQL语句中的顺序为准</param> 
        /// <returns></returns> 
        public int ExecuteNonQuery(string commandText, SQLiteParameter[] parameters)
        {
            // 自动打开
            if (this.DbConnection == null)
            {
                this.AutoOpenClose = true;
                this.Open();
            }
            else if (this.DbConnection.State == ConnectionState.Closed)
            {
                this.Open();
            }
            this.dbCommand = this.DbConnection.CreateCommand();
            this.dbCommand.CommandText = commandText;
            if (this.dbTransaction != null)
            {
                this.dbCommand.Transaction = this.dbTransaction;
            }
            if (parameters != null)
            {
                this.dbCommand.Parameters.Clear();
                for (int i = 0; i < parameters.Length; i++)
                {
                    this.dbCommand.Parameters.Add(parameters[i]);
                }
            }
            int returnValue = this.dbCommand.ExecuteNonQuery();
            // 自动关闭           
            this.dbCommand.Parameters.Clear();
            return returnValue;
        }

        #endregion

        #region ExecuteReader

        /// <summary>
        /// 执行一个查询语句，返回一个关联的SQLiteDataReader实例 
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public SQLiteDataReader ExecuteReader(string commandText)
        {
            return ExecuteReader(commandText, null);
        }

        /// <summary> 
        /// 执行一个查询语句，返回一个关联的SQLiteDataReader实例 
        /// </summary> 
        /// <param name="commandText">要执行的查询语句</param> 
        /// <param name="dbParameters">执行SQL查询语句所需要的参数，参数必须以它们在SQL语句中的顺序为准</param> 
        /// <returns></returns> 
        public SQLiteDataReader ExecuteReader(string commandText, SQLiteParameter[] dbParameters)
        {
            // 自动打开
            if (this.DbConnection == null)
            {
                this.AutoOpenClose = true;
                this.Open();
            }
            else if (this.DbConnection.State == ConnectionState.Closed)
            {
                this.Open();
            }

            this.dbCommand = this.DbConnection.CreateCommand();
            this.dbCommand.CommandText = commandText;
            if (dbParameters != null)
            {
                this.dbCommand.Parameters.Clear();
                for (int i = 0; i < dbParameters.Length; i++)
                {
                    if (dbParameters[i] != null)
                    {
                        this.dbCommand.Parameters.Add(dbParameters[i]);
                    }
                }
            }
            // 这里要关闭数据库才可以的
            SQLiteDataReader dbDataReader = null;
            dbDataReader = this.dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
            return dbDataReader;
        }

        #endregion

        #region ExecuteDataTable

        public DataTable ExecuteDataTable(string commandText)
        {
            return ExecuteDataTable(commandText, null);
        }
        /// <summary> 
        /// 执行一个查询语句，返回一个包含查询结果的DataTable 
        /// </summary> 
        /// <param name="sql">要执行的查询语句</param> 
        /// <param name="parameters">执行SQL查询语句所需要的参数，参数必须以它们在SQL语句中的顺序为准</param> 
        /// <returns></returns> 
        public DataTable ExecuteDataTable(string commandText, SQLiteParameter[] parameters)
        {
            // 自动打开
            if (this.DbConnection == null)
            {
                this.AutoOpenClose = true;
                this.Open();
            }
            else if (this.DbConnection.State == ConnectionState.Closed)
            {
                this.Open();
            }
            this.dbCommand = this.DbConnection.CreateCommand();
            this.dbCommand.CommandText = commandText;
            if (parameters != null)
            {
                this.dbCommand.Parameters.AddRange(parameters);
            }
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(this.dbCommand);
            DataTable data = new DataTable();
            adapter.Fill(data);
            // 自动关闭
            if (this.AutoOpenClose)
            {
                this.Close();
            }
            return data;
        }

        #endregion

        #region ExecuteScalar

        /// <summary>
        /// 执行一个查询语句，返回查询结果的第一行第一列 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public Object ExecuteScalar(string commandText)
        {
            return ExecuteScalar(commandText, null);
        }
        /// <summary> 
        /// 执行一个查询语句，返回查询结果的第一行第一列 
        /// </summary> 
        /// <param name="sql">要执行的查询语句</param> 
        /// <param name="dbParameters">执行SQL查询语句所需要的参数，参数必须以它们在SQL语句中的顺序为准</param> 
        /// <returns></returns> 
        public Object ExecuteScalar(string commandText, SQLiteParameter[] dbParameters)
        {
            // 自动打开
            if (this.DbConnection == null)
            {
                this.AutoOpenClose = true;
                this.Open();
            }
            else if (this.DbConnection.State == ConnectionState.Closed)
            {
                this.Open();
            }

            this.dbCommand = this.DbConnection.CreateCommand();
            this.dbCommand.CommandText = commandText;
            if (this.dbTransaction != null)
            {
                this.dbCommand.Transaction = this.dbTransaction;
            }
            if (dbParameters != null)
            {
                this.dbCommand.Parameters.Clear();
                for (int i = 0; i < dbParameters.Length; i++)
                {
                    if (dbParameters[i] != null)
                    {
                        this.dbCommand.Parameters.Add(dbParameters[i]);
                    }
                }
            }
            object returnValue = this.dbCommand.ExecuteScalar();
            // 自动关闭
            if (this.AutoOpenClose)
            {
                this.Close();
            }
            else
            {
                this.dbCommand.Parameters.Clear();
            }
            return returnValue;
        }

        #endregion

        #region public IDbTransaction BeginTransaction() 事务开始
        /// <summary>
        /// 事务开始
        /// </summary>
        /// <returns>事务</returns>
        public IDbTransaction BeginTransaction()
        {
            if (!this.InTransaction)
            {
                this.InTransaction = true;
                this.dbTransaction = this.DbConnection.BeginTransaction();
            }

            return this.dbTransaction;
        }
        #endregion

        #region public void CommitTransaction() 提交事务
        /// <summary>
        /// 提交事务
        /// </summary>
        public void CommitTransaction()
        {
            if (this.InTransaction)
            {
                this.InTransaction = false;
                this.dbTransaction.Commit();
            }           
        }
        #endregion

        #region public void RollbackTransaction() 回滚事务
        /// <summary>
        /// 回滚事务
        /// </summary>
        public void RollbackTransaction()
        {
            if (this.InTransaction)
            {
                this.InTransaction = false;
                this.dbTransaction.Rollback();
            }

        }
        #endregion

        #region public void Close() 关闭数据库连接
        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void Close()
        {
            if (this.dbConnection != null)
            {
                this.dbConnection.Close();
                this.dbConnection.Dispose();
            }

            this.Dispose();
        }
        #endregion

        #region public void Dispose() 内存回收
        /// <summary>
        /// 内存回收
        /// </summary>
        public void Dispose()
        {
            if (this.dbCommand != null)
            {
                this.dbCommand.Dispose();
            }
            if (this.dbDataAdapter != null)
            {
                this.dbDataAdapter.Dispose();
            }
            this.dbConnection = null;
        }
        #endregion

    }

}

