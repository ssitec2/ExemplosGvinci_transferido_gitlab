using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Web;

namespace GAdapter
{
	public class ReadyAdaptation
	{
		private string NewDatabase = HttpContext.Current.Session["DataBaseAdapterName"].ToString();
		private Adapter Adapta;

		public List<string> ExecutionErro
		{
			get
			{
				return HttpContext.Current.Session["ExecutionErro"] as List<string>;
			}
			set
			{
				HttpContext.Current.Session["ExecutionErro"] = value;
			}
		}

		public Dictionary<string, string> AdaptedTables
		{
			get
			{
				return HttpContext.Current.Session["AdaptedTables"] as Dictionary<string, string>;
			}
			set
			{
				HttpContext.Current.Session["AdaptedTables"] = value;
			}
		}

		public Dictionary<string, string> AdaptedFields
		{
			get
			{
				return HttpContext.Current.Session["AdaptedFields"] as Dictionary<string, string>;
			}
			set
			{
				HttpContext.Current.Session["AdaptedFields"] = value;
			}
		}

		public TableStruct AdaptingTable
		{
			get
			{
				return HttpContext.Current.Session["AdaptingTable"] as TableStruct;
			}
			set
			{
				HttpContext.Current.Session["AdaptingTable"] = value;
			}
		}

		public string AdapterStep
		{
			get
			{
				return HttpContext.Current.Session["AdapterStep"] as string;
			}
			set
			{
				HttpContext.Current.Session["AdapterStep"] = value;
			}
		}

		public List<TableStruct> AvaibleTables
		{
			get
			{
				return HttpContext.Current.Session["AvaibleTables"] as List<TableStruct>;
			}
			set
			{
				HttpContext.Current.Session["AvaibleTables"] = value;
			}
		}

        public Util.DbConnectionInfo ConnectionInfoDB
        {
            get
            {
                return (Util.DbConnectionInfo)HttpContext.Current.Session["ConnectionStringDB"];
            }
            set
            {
                HttpContext.Current.Session["ConnectionStringDB"] = value;
            }
        }

        public Util.DbConnectionInfo ConnectionInfoDBtemp
        {
            get
            {
                return (Util.DbConnectionInfo)HttpContext.Current.Session["ConnectionInfoDBtemp"];
            }
            set
            {
                HttpContext.Current.Session["ConnectionInfoDBtemp"] = value;
            }
        }

		public string ScriptFile
		{
			get
			{
				return HttpContext.Current.Session["ScriptFile"].ToString();
			}
			set
			{
				HttpContext.Current.Session["ScriptFile"] = value;
			}
		}

		public bool ExecErro
		{
			get
			{
				return (ExecutionErro.Count > 0);
			}
		}

		public string GetErro()
		{
			string Erro = "";

			foreach (string Err in ExecutionErro)
			{
				Erro += Err + "\r\n";
			}
			return Erro;
		}

		public ReadyAdaptation()
		{
		}

        public ReadyAdaptation(Util.DbConnectionInfo ConnectionStringDB, Util.DbConnectionInfo ConnectionStringDBTemp)
		{
            this.ConnectionInfoDB = ConnectionStringDB;
            this.ConnectionInfoDBtemp = ConnectionStringDBTemp;
           // this.ServerType = ServerType;
			//this.DatabaseName = DBName;
		}

		public void NewTable()
		{
			AdaptedTables[AdaptingTable.TableName] = AdaptingTable.TableName;
		}

		public void ChangeTable(string NewTableName)
		{
			AdaptedTables[AdaptingTable.TableName] = NewTableName;
		}

		public void ChangeField(string OldFieldName, string NewFieldName)
		{
			AdaptedFields[AdaptingTable.TableName + "." + OldFieldName] = NewFieldName;
		}

		public void Init()
		{
			HttpContext.Current.Session["ExecutionErro"] = new List<string>();
			if (AdaptedTables == null) AdaptedTables = new Dictionary<string, string>();
			if (AdaptedFields == null) AdaptedFields = new Dictionary<string, string>();
            Adapta = new Adapter(ConnectionInfoDB, ConnectionInfoDBtemp);
			Adapta.OnFieldTableChangedEvent += new Adapter.OnFieldTableChangedEventHandler(_CreateMDFInstance_OnFieldTableChangedEvent);
			Adapta.OnTableChangedEvent += new Adapter.OnTableChangedEventHandler(_CreateMDFInstance_OnTableChangedEvent);
			Adapta.OnTableCreatedOrRenamedEvent += new Adapter.OnTableCreatedOrRenamedEventHandler(_CreateMDFInstance_OnTableCreatedOrRenamedEvent);
		}

		public bool Run(ref string Error)
		{
			bool HasDiff = false;
			bool HasErro = false;
			string Script = "";
			Script = Adapta.MakeScript(ref HasDiff, ref HasErro);

			if (HasErro)
			{
				foreach (string _err in Adapta._ErroList)
				{
					Error += _err + "\r\n";
				}
			}

            if (HasDiff && Script.Length > 0)
			{
				HttpContext.Current.Session["script"] = Script;
				HttpContext.Current.Session["Diffs"] = GetErro() + Adapta.DiffsText;
				HttpContext.Current.Session["Warning"] = GetErro() + Adapta.WarningText;
				HttpContext.Current.Session["HasErro"] = HasErro;				
				HttpContext.Current.Response.Redirect("Status.aspx");
			}

			return (HasErro);
		}

		public void DropDbTmp()
		{
			Init();
            CleanDatabase(Util.GetConnectionString(ConnectionInfoDBtemp,true));
		}

		public List<string> LoadScriptSQL(string FileName_Path)
		{
			List<string> SQLScript = new List<string>();
			StreamReader File = new StreamReader(FileName_Path);
			string FileLine = "";
			SQLScript.Clear();
			while (FileLine != null)
			{
				FileLine = File.ReadLine();
				if (FileLine != null)
					SQLScript.Add(FileLine);
			}
			File.Close();
			return SQLScript;

		}

		public void CheckAdaptation(string Script, string ConnectionStringDB, string ConnectionStringDBTemp)
		{
			Init();

			string erro = "";
			bool _HasErro = false;

			_HasErro = Adapta.CheckAdapterDb(Script, ConnectionStringDB, ConnectionStringDBTemp);

			HttpContext.Current.Session["script"] = Script;
			if (Adapta.WarningList.Count > 0)
			{
				HttpContext.Current.Session["Warning"] = Adapta.WarningText;			
			}

			if (_HasErro)
			{
				foreach (string Erro in Adapta._ErroList)
				{
					erro += Erro + "\r\n";
				}
				HttpContext.Current.Session["Diffs"] = Adapta.DiffsText + "\r\n" + erro;
				HttpContext.Current.Session["HasErro"] = true;
			}
			else
			{
				HttpContext.Current.Session["HasErro"] = false;
			}

			HttpContext.Current.Response.Redirect("Status.aspx");
		}

		void _CreateMDFInstance_OnTableChangedEvent(object sender, OnTableChangedEventArgs Args)
		{
			if (AdaptedTables.ContainsKey(Args.Table.TableName))
			{
				AdapterStep = "OnTableChangedEvent";
				AdaptingTable = Args.Table;
				AvaibleTables = new List<TableStruct> { Args.OldTable };
				Adapta.NewTable();
			}
			else
			{
				AdapterStep = "OnTableChangedEvent";
				AdaptingTable = Args.Table;
				AvaibleTables = new List<TableStruct> { Args.OldTable };
				HttpContext.Current.Response.Redirect("AdapterInterface.aspx");
			}
		}

		void _CreateMDFInstance_OnTableCreatedOrRenamedEvent(object sender, OnTableCreatedOrRenamedEventArgs Args)
		{
			if (AdaptedTables.ContainsKey(Args.Table.TableName))
			{
				AdapterStep = "OnTableCreatedOrRenamedEvent";
				AdaptingTable = Args.Table;
				AvaibleTables = Args.AvaibleTables;
				Adapta.ChangeTable(AdaptedTables[Args.Table.TableName]);
			}
			else
			{
				AdapterStep = "OnTableCreatedOrRenamedEvent";
				AdaptingTable = Args.Table;
				AvaibleTables = Args.AvaibleTables;
				HttpContext.Current.Response.Redirect("AdapterInterface.aspx");
			}
		}

		void _CreateMDFInstance_OnFieldTableChangedEvent(object sender, OnFieldTableChangedEventArgs Args)
		{
			Args.Field.OldFieldName = AdaptedFields[AdaptingTable.TableName + "." + Args.Field.FieldName];
		}

		private SqlConnection SQLConnect(string stringConnection)
		{
			try
			{
				SqlConnection Con = new SqlConnection(stringConnection);
				Con.Open();
				return Con;
			}
			catch (Exception e)
			{
				ExecutionErro.Add("Erro ao abrir conexão com o banco");
				ExecutionErro.Add(e.Message.ToString());
				return null;
			}
		}

		/* *******************************************************************************************
		* Roda o Script de Criação do Banco
		*********************************************************************************************/
		public void DBCreateByScript(string _ConnectionString, List<string> SQLScript)
		{
            string ExecuteLine = "";
            try
            {
                SQLScript = SQLScript.Where(l => l.Trim().Length > 0 && (!l.StartsWith("/*") || !l.EndsWith("*/"))).ToList<string>();

                switch (ConnectionInfoDB.ServerType)
                {
                    case Util.DatabaseType.SQL:
                       SqlConnection Connect = SQLConnect(_ConnectionString);
                       SqlTransaction _SqlTransaction = Connect.BeginTransaction();
                       
                       try
                        {

                            if (Connect != null)
                            {
                                //SqlTransaction _SqlTransaction = Connect.BeginTransaction();
                                ExecuteCommand(_ConnectionString + ";Initial Catalog=master", "if db_id('"+NewDatabase+"') is  null CREATE DATABASE [" + NewDatabase + "]");
                                SqlCommand Command = new SqlCommand("USE [" + NewDatabase + "]", Connect, _SqlTransaction);
								CleanDatabase(Util.GetConnectionString(ConnectionInfoDBtemp,true));
								Command.ExecuteNonQuery();

                                foreach (string Line in SQLScript)
                                {
                                    if (Line.Trim().ToUpper() != "GO")
                                    {
                                        ExecuteLine += Line + "\r\n";
                                    }
                                    else
                                    {
                                        //Command.CommandText = ExecuteLine;
                                        //int RetVal = Command.ExecuteNonQuery();

                                        Command = new SqlCommand(ExecuteLine, Connect, _SqlTransaction);
                                        Command.ExecuteNonQuery();
                                        ExecuteLine = "";
                                    }
                                }
                                if (ExecuteLine.Length > 0)
                                {
                                   Command = new SqlCommand(ExecuteLine, Connect, _SqlTransaction);
                                   Command.ExecuteNonQuery();
                                   ExecuteLine = "";
                                }
                                _SqlTransaction.Commit();
                                Connect.Close();
                            }
                            else
                            {
                                ExecutionErro.Add("Erro ao criar ao abrir Conexão.");
                            }
                        }
                        catch (Exception e)
                        {
                            _SqlTransaction.Rollback();
                            Connect.Close();
                            ExecutionErro.Add("Erro ao criar o banco de dados temporário");
                            ExecutionErro.Add(e.Message.ToString());
                        }
                        break;
                    case Util.DatabaseType.ORACLE:
                        break;
                    default:
                        break;
                        }
                    }
                    catch (Exception Ex)
                    {
                    }
                }
		
		public void CleanDatabase(string CSM )
		{
			string Query = "";
			if (ConnectionInfoDBtemp.ServerType == Util.DatabaseType.SQL)
			{

				Query = "DECLARE   " +
					"@ObjName	VARCHAR(1024), " +
					"@ParentName	VARCHAR(1024), " +
					"@xtype		VARCHAR(1024), " +
					"@sqlcmd		VARCHAR(1024)  " +
					"DECLARE TRIGGERCURSOR CURSOR  FOR  " +
					"select * from  " +
					"( " +
					"SELECT F.name, p.name as parent , F.xtype FROM sysobjects as F  " +
					"full join sysobjects as P on F.parent_obj = P.id " +
					"where F.xtype in ('F','Pk', 'D', 'V')  " +
					"union  " +
					"select TABLE_NAME, '' as parent, 'T'  as xtype from INFORMATION_SCHEMA.TABLES where TABLE_TYPE = 'BASE TABLE' " +
					")as L " +
					"order by CASE L.xtype " +
					"WHEN 'D' THEN 1 " +
					"WHEN 'F' THEN 2 " +
					"WHEN 'PK'THEN 3 " +
					"WHEN 'V' THEN 4 " +
					"WHEN 'T' THEN 5 " +
					"END " +
					"OPEN TRIGGERCURSOR " +
					"FETCH NEXT FROM TRIGGERCURSOR " +
					"INTO @ObjName, @ParentName , @xtype; " +
					"WHILE (@@FETCH_STATUS = 0) " +
						"BEGIN " +
							"IF @xtype = 'V'  " +
							"SELECT @sqlcmd = 'Drop view [' + @ObjName + ']'" +
							"IF @xtype in('D','F','PK')  " +
							"SELECT @sqlcmd = 'ALTER TABLE [' + @ParentName + '] DROP CONSTRAINT [' + @ObjName +']' " +
							"IF @xtype = 'T' " +
							"SELECT @sqlcmd = 'Drop Table [' + @ObjName + ']'" +
							"exec( @sqlcmd) " +
							"FETCH NEXT FROM TRIGGERCURSOR " +
							"INTO @ObjName, @ParentName,@xtype " +
						"END " +
					"CLOSE TRIGGERCURSOR " +
					"DEALLOCATE TRIGGERCURSOR ";
			}
			else if (ConnectionInfoDBtemp.ServerType == Util.DatabaseType.MYSQL)
			{
				Query = "SET FOREIGN_KEY_CHECKS = 0; " +
						"SET @View = NULL; " +
						"SELECT GROUP_CONCAT(table_schema, '.', table_name) INTO @View " +
						"FROM information_schema.views " +
						"WHERE table_schema = '" + ConnectionInfoDBtemp.DbName+ "'; " +

						"SET @View = CONCAT('DROP VIEW  ', @View); " +
						"PREPARE stmt FROM @View; " +
						"EXECUTE stmt; " +
						"DEALLOCATE PREPARE stmt; " +
						"SET FOREIGN_KEY_CHECKS = 1; " +

						"SET FOREIGN_KEY_CHECKS = 0;  " +
						"SET @tables = NULL; " +
						"SELECT GROUP_CONCAT(table_schema, '.', table_name) INTO @tables " +
						"FROM information_schema.tables  " +
						"WHERE table_schema = '" + ConnectionInfoDBtemp.DbName + "' AND TABLE_TYPE = 'BASE TABLE'; " +

						"SET @tables = CONCAT('DROP TABLE ', @tables); " +
						"PREPARE stmt FROM @tables; " +
						"EXECUTE stmt; " +
						"DEALLOCATE PREPARE stmt; " +
						"SET FOREIGN_KEY_CHECKS = 1; ";
			}
			ExecuteCommand(CSM, Query);
		}
		/* *******************************************************************************************
		* Deleta o Banco de Dados Local passado como parâmetro
		*********************************************************************************************/
		public void DropDatabase(string DatabaseName, string CSM)
		{
			ExecuteCommand(CSM, "IF EXISTS(SELECT name FROM sys.databases WHERE name = '" + DatabaseName + "') " +
								"ALTER DATABASE " + DatabaseName + " SET SINGLE_USER WITH ROLLBACK IMMEDIATE;");
			ExecuteCommand(CSM, "IF EXISTS(SELECT name FROM sys.databases WHERE name = '" + DatabaseName + "') DROP DATABASE " + DatabaseName);
		}

		/* *******************************************************************************************
		* Executa um determinado comando no SQL.
		*********************************************************************************************/
		public bool ExecuteCommand(string _ConnectionString, string queryString)
		{
			using (SqlConnection connection = new SqlConnection(_ConnectionString))
			{
				SqlCommand command = new SqlCommand(queryString, connection);
				try
				{
					command.Connection.Open();
					int Ret = command.ExecuteNonQuery();
					command.Connection.Close();
					command.Connection.Dispose();
					return false;
				}
				catch(Exception e)
				{
					command.Connection.Close();
					command.Connection.Dispose();
					ExecutionErro.Add(e.Message.ToString());
					return true;
				}
			}
		}

		public bool Createbackup()
		{
            string DatabaseName = ConnectionInfoDB.DbName;
            string FileName = DatabaseName + ".bak";
           
			string BackupCommand = "if exists(SELECT * FROM msdb..backupset where database_name = '" + DatabaseName + "' AND [type] = 'D' and is_snapshot = 0) " +
													"BEGIN " +
													"BACKUP  DATABASE " + DatabaseName + " TO DISK= '" + FileName + "' WITH  DIFFERENTIAL ,  NOUNLOAD ,  NAME = N'Backup de Adaptação " + DatabaseName + " " + DateTime.Now.ToString() + "',  NOSKIP ,  STATS = 10,  NOFORMAT " +
													"END " +
													"ELSE " +
													"Begin " +
													"BACKUP  DATABASE " + DatabaseName + " TO DISK= '" + FileName + "' WITH  INIT ,  NOUNLOAD ,  NAME = N'Backup de Adaptação " + DatabaseName + " " + DateTime.Now.ToString() + "',  NOSKIP ,  STATS = 10,  NOFORMAT " +
													"END ";

            return ExecuteCommand(Util.GetConnectionString(ConnectionInfoDB) + " ;Database=MASTER", BackupCommand);
		}

		public bool Restorebackup()
		{
            string DatabaseName = ConnectionInfoDB.DbName;
            string FileName = DatabaseName + ".bak";
            string cnnScring = Util.GetConnectionString(ConnectionInfoDBtemp) + " ;Database=MASTER";

			//Realiza o RollBack
			//Restaura o ultimo banco de dados Full
			string RestoreCommand1 = "DECLARE @LastBackup int;\r\n" +
									"SET @LastBackup = (SELECT Top 1 position FROM msdb..backupset where database_name = '" + DatabaseName + "' And type = 'D' and is_snapshot = 0 " +
									"order by backup_finish_date desc)\r\n" +

									"Restore Database " + DatabaseName + " " +
									"from Disk = '" + FileName + "' " +
									"WITH FILE = @LastBackup ,NORECOVERY";

			//Restaura o ultimo banco de dados Incremental
			string RestoreCommand2 = "DECLARE @LastBackup int;\r\n" +
								"SET @LastBackup = (SELECT Top 1 position FROM msdb..backupset where database_name = '" + DatabaseName + "' and is_snapshot = 0 " +
								"order by backup_finish_date desc)\r\n" +

								"Restore Database  " + DatabaseName + " " +
								"FROM Disk = '" + FileName + "' " +
								"With FILE = @LastBackup ,Recovery, stats=10";

			try
			{
				ExecuteCommand(cnnScring, "ALTER DATABASE " + DatabaseName + " SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
				ExecuteCommand(cnnScring, RestoreCommand1);
				ExecuteCommand(cnnScring, RestoreCommand2);
				ExecuteCommand(cnnScring, "EXEC sp_dboption '" + DatabaseName + "', 'single user', 'false'");
				ExecutionErro.Add("Backup Restaurado.");
				return false;
			}
			catch (Exception Err)
			{
				ExecuteCommand(cnnScring, "EXEC sp_dboption '" + DatabaseName + "', 'single user', 'false'");
				ExecutionErro.Add("Erro ao restaurar o Backup.\r\n\r\nError message: " + Err.Message.ToString());
				return true;
			}
		}
	}
}
