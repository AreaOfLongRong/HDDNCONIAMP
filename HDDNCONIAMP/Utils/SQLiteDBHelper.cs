using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace HDDNCONIAMP.Utils
{
    /// <summary> 
    /// ˵��������һ�����System.Data.SQLite�����ݿⳣ�������װ��ͨ���ࡣ 
    /// huangjie add 2014-10-31
    /// </summary>    
    public class SQLiteDBHelper
    {
        private string dbPath = Application.StartupPath + "\\DataBase.db3";
        private string connectionString = string.Empty;

        #region ���ݿ����ӱ�Ҫ��������

        private SQLiteConnection dbConnection = null;
        /// <summary>
        /// ���ݿ�����
        /// </summary>
        public SQLiteConnection DbConnection
        {
            get
            {
                if (this.dbConnection == null)
                {
                    // ��û�򿪣��ͱ���Զ��򿪹رյ�
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
        /// ����
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
        /// ���ݿ�������
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
        /// ���ݿ�����
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
        /// �Ƿ��Ѳ�������
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
        /// Ĭ�ϴ򿪹ر����ݿ�ѡ�Ĭ��Ϊ��
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
        /// ���캯�� 
        /// </summary> 
        /// <param name="dbPath">SQLite���ݿ��ļ�·��</param> 
        public SQLiteDBHelper()
        {
            this.connectionString = "Data Source=" + dbPath;
        }
        /// <summary>
        /// ��ʱ��Ҫ�Ļ�ȡ���ݿ����ӵķ���
        /// </summary>
        /// <returns>���ݿ�����</returns>
        public IDbConnection Open()
        {
            this.Open(this.ConnectionString);
            return this.dbConnection;
        }

        /// <summary>
        /// ����µ����ݿ�����
        /// </summary>
        /// <param name="connectionString">���ݿ������ַ���</param>
        /// <returns>���ݿ�����</returns>
        public IDbConnection Open(string connectionString)
        {
            // ���ǿյĻ��Ŵ�
            if (this.dbConnection == null || this.dbConnection.State == ConnectionState.Closed)
            {
                this.ConnectionString = connectionString;
                this.dbConnection = new SQLiteConnection(this.ConnectionString);
                this.dbConnection.Open();
            }

            this.AutoOpenClose = false;
            return this.dbConnection;
        }

        #region �����������ݿ�

        /// <summary> 
        /// ����SQLite���ݿ��ļ� 
        /// </summary> 
        /// <param name="dbPath">Ҫ������SQLite���ݿ��ļ�·��</param> 
        public void CreateDB()
        {
            if (!System.IO.File.Exists(dbPath))
            {
                // �Զ���
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

        #region �жϱ��Ƿ����

        /// <summary>
        /// �жϱ��Ƿ����
        /// true:���ڣ�false:������
        /// </summary>
        /// <param name="tableName">����</param>
        /// <returns></returns>
        public bool IsExistTable(string tableName)
        {
            // �Զ���
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
        /// ִ�в�ѯ
        /// </summary>
        /// <param name="commandText">sql��ѯ</param>
        /// <returns>Ӱ������</returns>
        public int ExecuteNonQuery(string commandText)
        {
            return ExecuteNonQuery(commandText, null);
        }
        /// <summary> 
        /// ��SQLite���ݿ�ִ����ɾ�Ĳ�����������Ӱ��������� 
        /// </summary> 
        /// <param name="commandText">Ҫִ�е���ɾ�ĵ�SQL���</param> 
        /// <param name="parameters">ִ����ɾ���������Ҫ�Ĳ���������������������SQL����е�˳��Ϊ׼</param> 
        /// <returns></returns> 
        public int ExecuteNonQuery(string commandText, SQLiteParameter[] parameters)
        {
            // �Զ���
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
            // �Զ��ر�           
            this.dbCommand.Parameters.Clear();
            return returnValue;
        }

        #endregion

        #region ExecuteReader

        /// <summary>
        /// ִ��һ����ѯ��䣬����һ��������SQLiteDataReaderʵ�� 
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public SQLiteDataReader ExecuteReader(string commandText)
        {
            return ExecuteReader(commandText, null);
        }

        /// <summary> 
        /// ִ��һ����ѯ��䣬����һ��������SQLiteDataReaderʵ�� 
        /// </summary> 
        /// <param name="commandText">Ҫִ�еĲ�ѯ���</param> 
        /// <param name="dbParameters">ִ��SQL��ѯ�������Ҫ�Ĳ���������������������SQL����е�˳��Ϊ׼</param> 
        /// <returns></returns> 
        public SQLiteDataReader ExecuteReader(string commandText, SQLiteParameter[] dbParameters)
        {
            // �Զ���
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
            // ����Ҫ�ر����ݿ�ſ��Ե�
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
        /// ִ��һ����ѯ��䣬����һ��������ѯ�����DataTable 
        /// </summary> 
        /// <param name="sql">Ҫִ�еĲ�ѯ���</param> 
        /// <param name="parameters">ִ��SQL��ѯ�������Ҫ�Ĳ���������������������SQL����е�˳��Ϊ׼</param> 
        /// <returns></returns> 
        public DataTable ExecuteDataTable(string commandText, SQLiteParameter[] parameters)
        {
            // �Զ���
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
            // �Զ��ر�
            if (this.AutoOpenClose)
            {
                this.Close();
            }
            return data;
        }

        #endregion

        #region ExecuteScalar

        /// <summary>
        /// ִ��һ����ѯ��䣬���ز�ѯ����ĵ�һ�е�һ�� 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public Object ExecuteScalar(string commandText)
        {
            return ExecuteScalar(commandText, null);
        }
        /// <summary> 
        /// ִ��һ����ѯ��䣬���ز�ѯ����ĵ�һ�е�һ�� 
        /// </summary> 
        /// <param name="sql">Ҫִ�еĲ�ѯ���</param> 
        /// <param name="dbParameters">ִ��SQL��ѯ�������Ҫ�Ĳ���������������������SQL����е�˳��Ϊ׼</param> 
        /// <returns></returns> 
        public Object ExecuteScalar(string commandText, SQLiteParameter[] dbParameters)
        {
            // �Զ���
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
            // �Զ��ر�
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

        #region public IDbTransaction BeginTransaction() ����ʼ
        /// <summary>
        /// ����ʼ
        /// </summary>
        /// <returns>����</returns>
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

        #region public void CommitTransaction() �ύ����
        /// <summary>
        /// �ύ����
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

        #region public void RollbackTransaction() �ع�����
        /// <summary>
        /// �ع�����
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

        #region public void Close() �ر����ݿ�����
        /// <summary>
        /// �ر����ݿ�����
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

        #region public void Dispose() �ڴ����
        /// <summary>
        /// �ڴ����
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

