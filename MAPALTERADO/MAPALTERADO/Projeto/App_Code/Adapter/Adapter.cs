using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Data;

namespace GAdapter
{

    [Serializable()]
    public class DataBaseStruct
    {
        public string DatabaseName { get; set; }

        public List<TableStruct> Tables { get; set; }

        public List<ProcedureStruct> Procedures { get; set; }

        public List<FunctionsStruct> Functions { get; set; }
    }

    [Serializable()]
    public class TableStruct
    {

        public string TableName { get; set; }

        public string OldTableName { get; set; }

        public List<FieldStruct> Fields { get; set; }

        public List<TriggerStruct> Triggers { get; set; }

        public List<IndexStruct> Index { get; set; }

        public List<IndexStruct> PrimaryKey { get; set; }

        public List<FKStruct> ForeignKey { get; set; }

        public bool Create { get; set; }

        public bool Drop { get; set; }

        public bool Change { get; set; }

        public override string ToString()
        {
            return TableName;
        }
    }

    [Serializable()]
    public class FieldStruct
    {

        public string FieldName { get; set; }

        public string OldFieldName { get; set; }

        public string TmpFieldName { get; set; }

        public string DefaultValue { get; set; }

        public int? FieldSize { get; set; }

        public int? NumericPrecision { get; set; }

        public int? NumericScale { get; set; }

        public bool IsNullable { get; set; }

        public bool IsIdentity { get; set; }

        public bool UsedToUpdate { get; set; }

        public long? SeedValue { get; set; }

        public long? IncrementValue { get; set; }

        public bool IsNotForReplication { get; set; }

        public string FieldType { get; set; }

        public bool FieldCorrect { get; set; }

        public bool Create { get; set; }

        public bool Renamed { get; set; }

        public bool Drop { get; set; }

        public bool Change { get; set; }

        public bool Exist { get; set; }

        public int ChangesFound { get; set; }

        public DefaultValueStruct DefaultValueCommand { get; set; }

        public override string ToString()
        {
            return FieldName;
        }
    }

    public class OnTableCreatedOrRenamedEventArgs : EventArgs
    {
        public TableStruct Table { get; set; }

        public List<TableStruct> AvaibleTables { get; set; }

    }

    public class OnTableChangedEventArgs : EventArgs
    {
        public TableStruct Table { get; set; }

        public TableStruct OldTable { get; set; }

    }

    public class OnFieldTableChangedEventArgs : EventArgs
    {

        public FieldStruct Field { get; set; }

    }

    [Serializable()]
    public class TriggerStruct
    {
        public string Name { get; set; }

        public string CreateCommand { get; set; }

        public string DropCommand { get; set; }

        public bool RunDrop { get; set; }

        public bool RunCreate { get; set; }
    }

    [Serializable()]
    public class DefaultValueStruct
    {
        public string CreateCommand { get; set; }

        public string DropCommand { get; set; }

        public bool RunDrop { get; set; }

        public bool RunCreate { get; set; }
    }

    [Serializable()]
    public class ProcedureStruct
    {
        public string Name { get; set; }

        public string DropCommand { get; set; }

        public string CreateCommand { get; set; }

        public bool RunDrop { get; set; }

        public bool RunCreate { get; set; }

        public List<string> DependenceView { get; set; }
    }

    [Serializable()]
    public class FunctionsStruct
    {
        public string Name { get; set; }

        public string DropCommand { get; set; }

        public string CreateCommand { get; set; }

        public bool RunDrop { get; set; }

        public bool RunCreate { get; set; }

    }

    [Serializable()]
    public class IndexStruct
    {
        public string Name { get; set; }

        public int FillFactor { get; set; }

        public Dictionary<string, bool> ColumnName { get; set; }

        public string IsCluster { get; set; }

        public bool IsUnique { get; set; }

        public string DropCommand { get; set; }

        public string CreateCommand { get; set; }

        public bool RunDrop { get; set; }

        public bool RunCreate { get; set; }
    }

    [Serializable()]
    public class FKStruct
    {
        public string Name { get; set; }

        public bool UpdateCascate { get; set; }

        public bool DeleteCascate { get; set; }

        public string ReferencedTable { get; set; }

        public Dictionary<string, string> ColReferencedColReferencing { get; set; } //Coluna Referenciada, e coluna de origem

        public string DropCommand { get; set; }

        public string CreateCommand { get; set; }

        public bool RunDrop { get; set; }

        public bool RunCreate { get; set; }
    }

    public enum CommandType
    {
        CreateFunctions,
        CreateDefaultValue,
        CreateTable,
        CreateProcedure,
        CreateTrigger,
        CreateIndex,
        CreateFK,
        CreatePK,
        CreateField,
        DropDefaultValue,
        DropDefaultValueO,
        DropDefaultValueTMP,
        DropTable,
        DropProcedure,
        DropTrigger,
        DropTriggerO,
        DropFKO,
        DropPKO,
        DropFK,
        DropPK,
        DropField,
        DropFieldO,
        DropUpdateField,
        AlterField,
        NumericPrecision,
        RenameTable,
        RenameField,
        RenameIdentityField,
        RenameTemp,
        UpdateField,
        DropFunctions
    }

    public class ProgrammabilityCommandsStruct
    {
        public string Command { get; set; }
        public CommandType Type { get; set; }

    }

    [Serializable()]
    public class CommandStruct
    {
        public string TableName { get; set; }

        public string ReferencedTableAndColumn { get; set; }

        public string ColumnName { get; set; }

        public string CommandName { get; set; }

        public string CommandCreate { get; set; }

        public string CommandDrop { get; set; }

        public string Type { get; set; }

        public bool ExecDrop { get; set; }

        public bool ExecCreate { get; set; }

    }

    public class SqlTypeConverter
    {
        private string[][] SqlTable = new string[32][];

        public SqlTypeConverter()
        {

            SqlTable[0] = new string[32] { "", "binary", "varbinary", "char", "varchar", "nchar", "nvarchar", "datetime", "smalldatetime", "date", "time", "datetimeoffset", "datetime2", "decimal", "numeric", "float", "real", "bigint", "int", "smallint", "tinyint", "money", "smallmoney", "bit", "timestamp", "uniqueidentifier", "image", "ntext", "text", "sql_variant", "xml", "hierarchyid" };
            SqlTable[1] = new string[32] { "binary", "S", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "N", "N", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "N", "N", "I", "I", "I" };
            SqlTable[2] = new string[32] { "varbinary", "I", "S", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "N", "N", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "N", "N", "I", "I", "I" };
            SqlTable[3] = new string[32] { "char", "E", "E", "S", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "E", "I", "I", "I", "I", "I", "I", "I" };
            SqlTable[4] = new string[32] { "varchar", "E", "E", "I", "S", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "E", "I", "I", "I", "I", "I", "I", "I" };
            SqlTable[5] = new string[32] { "nchar", "E", "E", "I", "I", "S", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "E", "I", "N", "I", "I", "I", "I", "I" };
            SqlTable[6] = new string[32] { "nvarchar", "E", "E", "I", "I", "I", "S", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "E", "I", "N", "I", "I", "I", "I", "I" };
            SqlTable[7] = new string[32] { "datetime", "E", "E", "I", "I", "I", "I", "S", "I", "I", "I", "I", "I", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "N", "N", "N", "N", "I", "N", "N" };
            SqlTable[8] = new string[32] { "smalldatetime", "E", "E", "I", "I", "I", "I", "I", "S", "I", "I", "I", "I", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "N", "N", "N", "N", "I", "N", "N" };
            SqlTable[9] = new string[32] { "date", "E", "E", "I", "I", "I", "I", "I", "I", "S", "N", "I", "I", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "I", "N", "N" };
            SqlTable[10] = new string[32] { "time", "E", "E", "I", "I", "I", "I", "I", "I", "N", "S", "I", "I", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "I", "N", "N" };
            SqlTable[11] = new string[32] { "datetimeoffset", "E", "E", "I", "I", "I", "I", "I", "I", "I", "I", "S", "I", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "I", "N", "N" };
            SqlTable[12] = new string[32] { "datetime2", "E", "E", "I", "I", "I", "I", "I", "I", "I", "I", "I", "S", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "I", "N", "N" };
            SqlTable[13] = new string[32] { "decimal", "I", "I", "I", "I", "I", "I", "I", "I", "N", "N", "N", "N", "S", "C", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "N", "N", "N", "N", "I", "N", "N" };
            SqlTable[14] = new string[32] { "numeric", "I", "I", "I", "I", "I", "I", "I", "I", "N", "N", "N", "N", "C", "S", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "N", "N", "N", "N", "I", "N", "N" };
            SqlTable[15] = new string[32] { "float", "I", "I", "I", "I", "I", "I", "I", "I", "N", "N", "N", "N", "I", "I", "S", "I", "I", "I", "I", "I", "I", "I", "I", "N", "N", "N", "N", "N", "I", "N", "N" };
            SqlTable[16] = new string[32] { "real", "I", "I", "I", "I", "I", "I", "I", "I", "N", "N", "N", "N", "I", "I", "I", "S", "I", "I", "I", "I", "I", "I", "I", "N", "N", "N", "N", "N", "I", "N", "N" };
            SqlTable[17] = new string[32] { "bigint", "I", "I", "I", "I", "I", "I", "I", "I", "N", "N", "N", "N", "I", "I", "I", "I", "S", "I", "I", "I", "I", "I", "I", "I", "N", "N", "N", "N", "I", "N", "N" };
            SqlTable[18] = new string[32] { "int", "I", "I", "I", "I", "I", "I", "I", "I", "N", "N", "N", "N", "I", "I", "I", "I", "I", "S", "I", "I", "I", "I", "I", "I", "N", "N", "N", "N", "I", "N", "N" };
            SqlTable[19] = new string[32] { "smallint", "I", "I", "I", "I", "I", "I", "I", "I", "N", "N", "N", "N", "I", "I", "I", "I", "I", "I", "S", "I", "I", "I", "I", "I", "N", "N", "N", "N", "I", "N", "N" };
            SqlTable[20] = new string[32] { "tinyint", "I", "I", "I", "I", "I", "I", "I", "I", "N", "N", "N", "N", "I", "I", "I", "I", "I", "I", "I", "S", "I", "I", "I", "I", "N", "N", "N", "N", "I", "N", "N" };
            SqlTable[21] = new string[32] { "money", "I", "I", "I", "I", "I", "I", "I", "I", "N", "N", "N", "N", "I", "I", "I", "I", "I", "I", "I", "I", "S", "I", "I", "I", "N", "N", "N", "N", "I", "N", "N" };
            SqlTable[22] = new string[32] { "smallmoney", "I", "I", "I", "I", "I", "I", "I", "I", "N", "N", "N", "N", "I", "I", "I", "I", "I", "I", "I", "I", "I", "S", "I", "I", "N", "N", "N", "N", "I", "N", "N" };
            SqlTable[23] = new string[32] { "bit", "I", "I", "I", "I", "I", "I", "I", "I", "N", "N", "N", "N", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "S", "I", "N", "N", "N", "N", "I", "N", "N" };
            SqlTable[24] = new string[32] { "timestamp", "I", "I", "I", "I", "N", "N", "I", "I", "N", "N", "N", "N", "I", "I", "N", "N", "I", "I", "I", "I", "I", "I", "I", "S", "N", "N", "N", "N", "N", "N", "N" };
            SqlTable[25] = new string[32] { "uniqueidentifier", "I", "I", "I", "I", "I", "I", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "S", "N", "N", "N", "I", "N", "N" };
            SqlTable[26] = new string[32] { "image", "I", "I", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "I", "N", "S", "N", "N", "N", "N", "N" };
            SqlTable[27] = new string[32] { "ntext", "N", "N", "I", "I", "I", "I", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "S", "I", "N", "I", "N" };
            SqlTable[28] = new string[32] { "text", "N", "N", "I", "I", "I", "I", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "I", "S", "N", "I", "N" };
            SqlTable[29] = new string[32] { "sql_variant", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "N", "E", "N", "N", "N", "S", "N", "N" };
            SqlTable[30] = new string[32] { "xml", "E", "E", "E", "E", "E", "E", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "S", "N" };
            SqlTable[31] = new string[32] { "hierarchyid", "E", "E", "E", "E", "E", "E", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "S" };

            //S = SameType
            //N = Not Allowed 
            //E = Explicit conversion
            //I = Implicit conversion
            //C = Requires explicit CAST to prevent the loss of precision or scale that might occur in implicit conversion
        }

        public string GetConverterType(string From, string To)
        {

            int Item = 0;
            //Corre as linhas procurando o tipo
            for (int i = 0; i < SqlTable.Count(); i++)
            {
                if (SqlTable[i][0] == From)
                {
                    Item = i;
                    break;
                }
            }

            //Corre as Coluna Procurando o tipo
            if (Item > 0)
            {
                for (int i = 0; i < SqlTable[0].Length; i++)
                {
                    if (SqlTable[0][i] == To)
                    {
                        return SqlTable[Item][i].ToUpper();
                    }
                }
            }
            return "";
        }
    }

    public static class Util
    {
        [Serializable]
        public struct DbConnectionInfo
        {
            public string Server;
            public string UserName;
            public string UserPassword;
            public string DbName;
            public bool TrustedConnection;
            public DatabaseType ServerType;
        }

        public enum DatabaseType  { SQL = 0, MYSQL = 1, ORACLE = 2 };

        public static string GetConnectionString(Util.DbConnectionInfo Connection)
        {
            return GetConnectionString(Connection, false);
        }

        public static string GetConnectionString(Util.DbConnectionInfo Connection, bool WithDatabase)
        {
            string StrConnnection = "";

            switch (Connection.ServerType)
            {
                case DatabaseType.SQL:
                    if (Connection.TrustedConnection)
                    {
                        StrConnnection =  "Server=" + Connection.Server + ";Trusted_Connection=true";
                    }
                    else
                    {
                        StrConnnection =  "Server=" + Connection.Server + " ;User ID=" + Connection.UserName + " ;Password=" + Connection.UserPassword + " ;Trusted_Connection=False";
                    }

                    if (WithDatabase)
                    {
                        StrConnnection += " ;Database=" + Connection.DbName;
                    }
                    break;
                case DatabaseType.MYSQL:
                        StrConnnection =  String.Format("Data Source={0};Password={1};User Id={2};",Connection.Server,Connection.UserPassword,Connection.UserName,Connection.DbName);
                        if (WithDatabase)
                        {
                            StrConnnection += String.Format("Database={0};",Connection.DbName);
                        }
                    break;
                case DatabaseType.ORACLE:
                    break;
                default:
                    break;
            }
            return StrConnnection;
        }
    }
    public class Adapter
    {
        public List<CommandStruct> CommandProgrammability = new List<CommandStruct>();

        public Util.DatabaseType ServerType;
        public Util.DbConnectionInfo DbOldInfo;
        public Util.DbConnectionInfo DbNewInfo;

        public DataBaseStruct DBStructNew = new DataBaseStruct();
        public DataBaseStruct DBStructOld = new DataBaseStruct();
        public SqlTypeConverter SqlConverter = new SqlTypeConverter();
        public List<ProgrammabilityCommandsStruct> ProgrammabilityCommand = new List<ProgrammabilityCommandsStruct>();

        public List<string> _ErroList = new List<string>();
        public List<string> WarningList = new List<string>();
        public Dictionary<string, List<string>> DiffsFound = new Dictionary<string, List<string>>();

        List<string> AdapterCommands = new List<string>();

        private TableStruct AdaptingTable;
        private string OldTable = null;

        private string _ConnectitonStringOld;
        private string _ConnectitonStringNew;

        private bool IsCanceling = false;
        private bool ShouldRunNextStep = false;

        public delegate void OnTableChangedEventHandler(object sender, OnTableChangedEventArgs Args);
        public event OnTableChangedEventHandler OnTableChangedEvent;

        private bool CheckingAdapter { get; set; }
        private bool CreateFunctions { get; set; }

        public void RaiseOnTableChangedEvent(OnTableChangedEventArgs Args)
        {
            if (OnTableChangedEvent != null)
            {
                OnTableChangedEvent(this, Args);
            }

        }

        public delegate void OnTableCreatedOrRenamedEventHandler(object sender, OnTableCreatedOrRenamedEventArgs Args);
        public event OnTableCreatedOrRenamedEventHandler OnTableCreatedOrRenamedEvent;

        public void RaiseOnTableCreatedOrRenamedEvent(OnTableCreatedOrRenamedEventArgs Args)
        {
            if (OnTableCreatedOrRenamedEvent != null)
            {
                OnTableCreatedOrRenamedEvent(this, Args);
            }
        }

        public delegate void OnFieldTableChangedEventHandler(object sender, OnFieldTableChangedEventArgs Args);
        public event OnFieldTableChangedEventHandler OnFieldTableChangedEvent;

        public void RaiseOnFieldTableChangedEvent(OnFieldTableChangedEventArgs Args)
        {
            if (OnFieldTableChangedEvent != null)
            {
                OnFieldTableChangedEvent(this, Args);
            }
        }

        private void SetConnections(Util.DbConnectionInfo OldDatabase, Util.DbConnectionInfo NewDatabase)
        {
            switch (OldDatabase.ServerType)
            {
                case Util.DatabaseType.SQL:

                    if (OldDatabase.TrustedConnection)
                    {
                        _ConnectitonStringOld = String.Format("Server={0};Trusted_Connection = True;Database={1}", OldDatabase.Server, OldDatabase.DbName);
                    }
                    else
                    {
                        _ConnectitonStringOld = String.Format("Server={0};Persist Security Info=True;User ID={1};Password={2};Database={3}", OldDatabase.Server, OldDatabase.UserName, OldDatabase.UserPassword, OldDatabase.DbName);
                    }

                    if (NewDatabase.TrustedConnection)
                    {
                        _ConnectitonStringNew = String.Format("Server={0};Trusted_Connection = True;Database={1}", NewDatabase.Server, NewDatabase.DbName);
                    }
                    else
                    {
                        _ConnectitonStringNew = String.Format("Server={0};Persist Security Info=True;User ID={1};Password={2};Database={3}", NewDatabase.Server, NewDatabase.UserName, NewDatabase.UserPassword, NewDatabase.DbName);
                    }
                    break;
                case Util.DatabaseType.MYSQL:
                    _ConnectitonStringOld = String.Format("Server={0};Database={1};Uid={2};Pwd={3};", OldDatabase.Server, OldDatabase.DbName, OldDatabase.UserName, OldDatabase.UserPassword);
                    _ConnectitonStringNew = String.Format("Server={0};Database={1};Uid={2};Pwd={3};", NewDatabase.Server, NewDatabase.DbName, NewDatabase.UserName, NewDatabase.UserPassword);
                    break;
                case Util.DatabaseType.ORACLE:
                    break;
                default:
                    break;
            }
        }

        public Adapter(Util.DbConnectionInfo OldDatabase, Util.DbConnectionInfo NewDatabase)
        {
            DbOldInfo = OldDatabase;
            DbNewInfo = NewDatabase;

            SetConnections(OldDatabase, NewDatabase);
            ServerType = OldDatabase.ServerType;
        }

        public void CancelAdapter()
        {
            IsCanceling = true;
            ShouldRunNextStep = false;
        }

        private void NextStep()
        {
            ShouldRunNextStep = true;
        }

        public void ChangeTable(string OldTable)
        {
            this.OldTable = OldTable;
            NextStep();
        }

        public void NewTable()
        {
            this.OldTable = null;
            NextStep();
        }

        public string DiffsText
        {
            get
            {
                string TableDiffs = "";
                foreach (KeyValuePair<string, List<string>> Table in DiffsFound)
                {
                    string Diffs = "  ";
                    foreach (string _Diffs in Table.Value)
                    {
                        Diffs += _Diffs.ToString() + "\r\n  ";
                    }
                    TableDiffs = TableDiffs + "Tabela: " + Table.Key.ToString() + "\r\n" + Diffs + "\r\n";
                }
                return TableDiffs;
            }

        }

        public string WarningText
        {
            get
            {
                string Warning = "";

                foreach (string _Warning in WarningList)
                {
                    Warning += _Warning.ToString() + "\r\n  ";
                }
                return Warning;
            }
        }

        public string _FunctionDropFKCreate
        {
            get
            {
                return "DROP PROCEDURE IF EXISTS DROP_FOREIGNKEY ; " +
                        "CREATE PROCEDURE DROP_FOREIGNKEY(IN P_TABLE_NAME VARCHAR(255), IN P_CONSTRAINT_NAME VARCHAR(255)) " +
                        "SQL SECURITY INVOKER " +
                        "BEGIN " +
                        "DECLARE TOT INT; " +
                        "SELECT COUNT(*) INTO TOT FROM information_schema.TABLE_CONSTRAINTS WHERE CONSTRAINT_SCHEMA = DATABASE() AND TABLE_NAME = P_TABLE_NAME AND CONSTRAINT_NAME = P_CONSTRAINT_NAME; " +
                        "If (Tot > 0) Then " +
                            "SET @cmd = CONCAT('ALTER TABLE `', P_TABLE_NAME, '` DROP FOREIGN KEY `', P_CONSTRAINT_NAME, '`'); " +
                            "PREPARE querycmd from @cmd; " +
                            "EXECUTE querycmd; " +
                            "DEALLOCATE PREPARE querycmd; " +
                        "END IF; " +
                    "END;";

            }
        }

        public string _FunctionDropFKDrop
        {
            get
            {

                return "DROP PROCEDURE IF EXISTS DROP_FOREIGNKEY ; ";
            }
        }

        private string _QDependenceView(string DatabaseName)
        {

            switch ((int)ServerType)
            {
                case 0: //SQL
                    return "select distinct V.name as 'View', dep.name as 'Depending' from " +
                            "(select * from sys.sql_dependencies) as d " +
                            "inner join " +
                            "(select t.name, object_id from sys.views as t) as dep " +
                            "on d.referenced_major_id = dep.object_id " +
                            "inner join " +
                            "(select t.name, object_id from sys.views as t) as V " +
                            "on V.object_id = d.object_id";
				case 1: //MySQL

					return	"";

                default:
                    return "Não Implementado";

            }
        }

        private string _QCompareDB(string DatabaseName)
        {

            switch ((int)ServerType)
            {
                case 0: //SQL
                    return "SELECT c.TABLE_CATALOG, c.TABLE_NAME,c.COLUMN_NAME,c.Column_DEFAULT,c.IS_NULLABLE,c.DATA_TYPE,c.CHARACTER_MAXIMUM_LENGTH, c.NUMERIC_PRECISION , c.NUMERIC_SCALE , ident.is_identity, ident.seed_value, ident.increment_value,ident.is_not_for_replication " +
                            "FROM INFORMATION_SCHEMA.Columns c " +
                            "inner join  sys.tables t " +
                            "on t.NAME = c.TABLE_NAME AND " +
                            "t.is_ms_shipped = 0 " +
                            "left join " +
                            "(select * from sys.identity_columns i)as ident " +
                            "on t.object_id = ident.object_id And c.COLUMN_NAME = ident.name " +
                            "order by c.TABLE_NAME";

                case 1://MYSQL

                    return "select TB_A.*, TB_B.is_Identity, TB_B.seed_value, 1 as 'increment_value',0 as 'is_not_for_replication'  from " +
                            "(SELECT TABLE_SCHEMA , TABLE_NAME, COLUMN_NAME, COLUMN_DEFAULT, IS_NULLABLE,DATA_TYPE, Column_type as 'CHARACTER_MAXIMUM_LENGTH',NUMERIC_PRECISION,NUMERIC_SCALE   " +
                            "FROM INFORMATION_SCHEMA.COLUMNS " +
                            "WHERE TABLE_SCHEMA = '" + DatabaseName + "') as TB_A "  +
                            "inner join " +
                                "(SELECT T.TABLE_SCHEMA, C.TABLE_NAME, COLUMN_NAME, " +
                                    "Case When EXTRA = 'auto_increment' Then 'true' Else 'false' End as 'is_Identity', " +
                                    "Case When EXTRA = 'auto_increment' Then T.AUTO_INCREMENT  Else 0 End  as 'seed_value'   " +
                                    "from INFORMATION_SCHEMA.Columns as C " +
                                    "inner join  " +
                                    "INFORMATION_SCHEMA.TABLES as T " +
                                    "on T.Table_name = C.Table_name " +
                                    "AND T.TABLE_SCHEMA = C.TABLE_SCHEMA " +
                                    "WHERE TABLE_TYPE = 'BASE TABLE'" +
                                ") as TB_B " +
                                "on TB_A.TABLE_NAME = TB_B.TABLE_NAME AND TB_A.TABLE_SCHEMA = TB_B.TABLE_SCHEMA AND TB_A.COLUMN_NAME = TB_B.COLUMN_NAME";

                default:
                    return "Não Implementado";

            }
        }

        private string _QProcedures(string DatabaseName)
        {

            switch ((int)ServerType)
            {
                case 0: //SQL
                    return "SELECT b.name, a.text, colid , b.xtype from syscomments a,sysobjects b where a.id=b.id and b.parent_obj = 0 order by colid";

                case 1://MYsql
                    return "(select ROUTINE_NAME as 'name', ROUTINE_DEFINITION as 'text',1 as 'colid', 'P' as 'xtype' from INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = '" + DatabaseName + "') " +
                            "union " +
                            "(select TABLE_NAME as 'name', VIEW_DEFINITION as 'text',1 as 'colid', 'V' as 'xtype'  from INFORMATION_SCHEMA.Views WHERE TABLE_SCHEMA = '" + DatabaseName + "')";

                default:
                    return "Não Implementado";

            }
        }

        private string _QTriggers(string DatabaseName)
        {

            switch ((int)ServerType)
            {
                case 0: //SQL
                    return "SELECT sys1.name trigger_name, " +
                                        "sys2.name le_name,  c.text trigger_body " +
                                        "FROM sysobjects sys1 " +
                                        "JOIN sysobjects sys2 ON sys1.parent_obj = sys2.id " +
                                        "JOIN syscomments c ON sys1.id = c.id " +
                                        "WHERE sys1.xtype = 'TR'";

                case 1:
                    return "select trigger_Name as 'trigger_name', EVENT_OBJECT_TABLE as 'le_name',ACTION_STATEMENT as 'trigger_body'  from INFORMATION_SCHEMA.triggers WHERE TRIGGER_SCHEMA ='" + DatabaseName + "'";

                default:
                    return "Não Implementado";

            }

        }

        private string _QPKs(string DatabaseName)
        {

            switch ((int)ServerType)
            {
                case 0: //SQL
                    return "select t.name as TableName, c.name as ColumnName,i.name IndexName,i.fill_factor,i.type_desc,i.is_unique,i.is_primary_key, ic.is_descending_key " +
                                    "from sys.indexes  i " +
                                    "inner join sys.tables t " +
                                    "on t.object_id = i.object_id " +
                                    "inner join sys.index_columns ic " +
                                    "on ic.object_id = i.object_id " +
                                    "and ic.index_id = i.index_id " +
                                    "inner join sys.all_columns c " +
                                    "on c.object_id = i.object_id " +
                                    "and c.column_id = ic.column_id " +
                                    "order by t.name, i.name, ic.key_ordinal";

                case 1: //MYSQL
                    return "select TB_A.TABLE_NAME AS 'TableName', TB_B.COLUMN_NAME, TB_A.CONSTRAINT_NAME AS 'IndexName',  " +
				"0 as 'fill_factor',  " +
				"CASE WHEN TB_A.CONSTRAINT_TYPE ='PRIMARY KEY' THEN 'CLUSTERED' ELSE 'NONCLUSTERED' END AS 'type_desc',  " +
				"CASE WHEN TB_A.CONSTRAINT_TYPE  IN ('PRIMARY KEY', 'UNIQUE') THEN 'true' ELSE 'false' END AS 'is_unique',  " +
				"CASE WHEN TB_A.CONSTRAINT_TYPE ='PRIMARY KEY' THEN 'true' ELSE 'false' END AS 'is_primary_key',  " +
				"'false'  as 'is_descending_key' , TB_A.CONSTRAINT_SCHEMA  " +
				"from INFORMATION_SCHEMA.table_constraints as TB_A  " +
				"inner join  " +
				"INFORMATION_SCHEMA.key_column_usage as TB_B " +
				"on TB_B.TABLE_NAME = TB_A.TABLE_NAME and TB_B.CONSTRAINT_NAME = TB_A.CONSTRAINT_NAME AND TB_B.CONSTRAINT_SCHEMA = TB_A.CONSTRAINT_SCHEMA " +
				"WHERE TB_A.CONSTRAINT_SCHEMA = '" + DatabaseName + "' AND TB_A.CONSTRAINT_TYPE <> 'FOREIGN KEY' " +
				"Group by TB_A.TABLE_NAME, TB_B.COLUMN_NAME, TB_A.CONSTRAINT_NAME, TB_A.CONSTRAINT_SCHEMA; ";

                default:
                    return "Não Implementado";
            }

        }

        private string _QFKs(string DatabaseName)
        {
            switch ((int)ServerType)
            {
                case 0: //SQL
                    return "select s.name as Constraint_name " +
                                    ", ObjectProperty(fk.constid, 'CnstIsUpdateCascade') as 'UpdateCascade' " +
                                    ", ObjectProperty(fk.constid, 'CnstIsDeleteCascade') as 'DeleteCascade' " +
                                    ", o2.name as Referenced_Object_name " +
                                    ", c2.name as Referenced_Column_Name " +
                                    ", o1.name as Referencing_Object_name " +
                                    ", c1.name as referencing_column_Name " +
                                    "from sysforeignkeys fk " +
                                    "inner join sysobjects o1 on fk.fkeyid = o1.id " +
                                    "inner join sysobjects o2 on fk.rkeyid = o2.id " +
                                    "inner join syscolumns c1 on c1.id = o1.id and c1.colid = fk.fkey " +
                                    "inner join syscolumns c2 on c2.id = o2.id and c2.colid = fk.rkey " +
                                    "inner join sysobjects s on fk.constid = s.id " +
                                    "order by s.name, fk.keyno";

                case 1: //MYSQL
                    return "select R.Constraint_name, " +
                            "case when R.UPDATE_RULE ='CASCADE' then 1 else 0 end as 'UpdateCascade',  " +
                            "case when R.DELETE_RULE ='CASCADE' then 1 else 0 end as 'DeleteCascade', " +
                            "R.REFERENCED_TABLE_NAME  as 'Referenced_Object_name', " +
                            "K.REFERENCED_COLUMN_NAME as 'Referenced_Column_Name', " +
                            "R.TABLE_NAME as 'Referencing_Object_name', " +
                            "K.COLUMN_NAME as 'referencing_column_Name'  " +
                            "from INFORMATION_SCHEMA.referential_constraints as R " +
                            "inner join  " +
                            "INFORMATION_SCHEMA.key_column_usage as K " +
                            "on K.CONSTRAINT_SCHEMA = R.CONSTRAINT_SCHEMA " +
                            "AND K.CONSTRAINT_NAME = R.CONSTRAINT_NAME " +
                            "WHERE R.CONSTRAINT_SCHEMA = '" + DatabaseName + "';";

                default:
                    return "Não Implementado";
            }

        }

        /*******************************************
        *   Início do adapta
        ********************************************/
        public string MakeScript(ref bool HasDiff, ref bool HasErro)
        {

            DataTable DT_Old = QueryExecute(_ConnectitonStringOld, _QCompareDB(DbOldInfo.DbName), ref HasErro);
            DataTable DT_New = QueryExecute(_ConnectitonStringNew, _QCompareDB(DbNewInfo.DbName), ref HasErro);

            if (!HasErro) // Vamos Verificar se existe algum dado para comparar
            {
                LoadDBStruct(DBStructOld, DT_Old);
                LoadDBStruct(DBStructNew, DT_New);

                if (CompareDbStruct(DBStructOld, DBStructNew))
                {
                    HasDiff = true;
                    return CreateScript(DBStructOld, DBStructNew);
                }
            }
            return "";
        }

        /*******************************************
        *   Verifica adaptação
        ********************************************/

        public bool CheckAdapterDb(string script)
        {
            return CheckAdapterDb(script, _ConnectitonStringOld, _ConnectitonStringNew);
        }

        public bool CheckAdapterDb(string script, string ConStringOld, string ConStringNew)
        {
            bool HasDiff = false;
            bool HasErro = false;

            DBStructOld = new DataBaseStruct();
            DBStructNew = new DataBaseStruct();

            _ErroList.Clear();
            DiffsFound.Clear();

            List<string> ScriptCommand = (script.ToString().Split(new char[] { '\n', '\r' })).ToList().Where(l => l.Length > 0).ToList();
            RunScript(ConStringOld, ScriptCommand);

            if (_ErroList.Count == 0)
            {
                CheckingAdapter = true;
                DataTable DT_Old = QueryExecute(ConStringOld, _QCompareDB(DBStructOld.DatabaseName), ref  HasErro);
                DataTable DT_New = QueryExecute(ConStringNew, _QCompareDB(DBStructNew.DatabaseName), ref  HasErro);
                if (!HasErro)
                {
                    LoadDBStruct(DBStructOld, DT_Old);
                    LoadDBStruct(DBStructNew, DT_New);

                    HasDiff = CompareDbStruct(DBStructOld, DBStructNew);
                }
                CheckingAdapter = false;
            }
            else
            {
                return true;
            }

            return (HasDiff || HasErro);
        }

        private void AddDiff(string Table, string Diff)
        {
            if (DiffsFound.ContainsKey(Table))
            {
                bool HasErro = false;
                foreach (string Erro in DiffsFound[Table])
                {
                    if (Erro == Diff)
                    {
                        HasErro = true;
                        break;

                    }
                }
                if (!HasErro)
                {
                    DiffsFound[Table].Add(Diff);
                }
            }
            else
            {
                List<string> _diff = new List<string>();
                _diff.Add(Diff);
                DiffsFound.Add(Table, _diff);
            }
        }

        public bool BoolParse(string Value)
        {
            if (Value.ToString().ToUpper() == "TRUE" ||
                Value.ToString().ToUpper() == "YES")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string GetDefault(string Value)
        {
            Value = Value.Trim(new char[] { ' ', '(', ')' });

            //Customização de Adaptação para o GAS2007
            if (Value.IndexOf(" defNada ") > 0)
            {
                return "''";
            }
            else if (Value.IndexOf(" defZero ") > 0)
            {
                return "0";
            }
            return Value;
        }

        /*******************************************
        *   Carrega database no Struct do programa
        ********************************************/

        private void LoadDBStruct(DataBaseStruct DBStruct, DataTable DT)
        {
            DataView DV = new DataView(DT);  // Banco de Dados antigo

            DBStruct.Tables = new List<TableStruct>();
            DBStruct.Procedures = new List<ProcedureStruct>();
            DBStruct.Functions = new List<FunctionsStruct>();

            if (DT.Rows.Count > 0)
            {
                DBStruct.DatabaseName = DT.Rows[0].ItemArray[0].ToString();
            }

            foreach (DataRow Row in DT.Rows)
            {
                TableStruct TableST = (from t in DBStruct.Tables
                                       where t.TableName == Row["TABLE_NAME"].ToString()
                                       select t).SingleOrDefault();

                if (TableST == null)
                {
                    TableST = new TableStruct();
                    TableST.Fields = new List<FieldStruct>();
                    TableST.Triggers = new List<TriggerStruct>();
                    TableST.Index = new List<IndexStruct>();
                    TableST.PrimaryKey = new List<IndexStruct>();
                    TableST.ForeignKey = new List<FKStruct>();

                    TableST.TableName = Row["TABLE_NAME"].ToString();
                    TableST.Create = false;
                    TableST.Change = false;
                    TableST.Drop = true;
                    DBStruct.Tables.Add(TableST);
                }

                FieldStruct FieldST = new FieldStruct();

                //Carregando os dados dos campos
                FieldST.DefaultValueCommand = new DefaultValueStruct();
                FieldST.FieldName = Row["COLUMN_NAME"].ToString();

                FieldST.DefaultValue = (Row["Column_DEFAULT"].ToString().Length > 0 ? GetDefault(Row["Column_DEFAULT"].ToString()) : "");
                FieldST.IsNullable = BoolParse(Row["IS_NULLABLE"].ToString());
                FieldST.FieldType = Row["DATA_TYPE"].ToString();

                if (ServerType == Util.DatabaseType.MYSQL)
                {
                    string[] Column_type = Row["CHARACTER_MAXIMUM_LENGTH"].ToString().Split(new char[] { '(', ')' });
                    if (Column_type.Count() == 3)
                    {
                        int FieldSize = 0;
                        if (int.TryParse(Column_type[1].ToString(), out FieldSize))
                        {
                            FieldST.FieldSize = FieldSize;
                        }
                    }
                }
                else
                {
                    if (!(Row["CHARACTER_MAXIMUM_LENGTH"] is DBNull)) { FieldST.FieldSize = Convert.ToInt32(Row["CHARACTER_MAXIMUM_LENGTH"]); }
                }

                if (!(Row["NUMERIC_PRECISION"] is DBNull)) { FieldST.NumericPrecision = Convert.ToInt32(Row["NUMERIC_PRECISION"]); }
                if (!(Row["NUMERIC_SCALE"] is DBNull)) { FieldST.NumericScale = Convert.ToInt32(Row["NUMERIC_SCALE"]); }
                FieldST.IsIdentity = BoolParse(Row["is_identity"].ToString());
                FieldST.SeedValue = Row["seed_value"] is DBNull ? 0 : Convert.ToInt64(Row["seed_value"]);
                FieldST.IncrementValue = Row["increment_value"] is DBNull ? 0 : Convert.ToInt64(Row["increment_value"]);
                FieldST.IsNotForReplication = BoolParse(Row["is_not_for_replication"].ToString());

                FieldST.OldFieldName = Row["COLUMN_NAME"].ToString();
                FieldST.Create = false;
                FieldST.Change = false;
                FieldST.Renamed = false;
                FieldST.Drop = true;
                FieldST.Exist = true;
                FieldST.UsedToUpdate = false;

                if (Row[1].ToString() != "") //Se tiver um default value para esse campo vamos criar o script de drop e create
                {
                    string CreateCommand = "";
                    string DropCommand = "";
                    ScriptDefaultValue(TableST.TableName, FieldST.FieldName, FieldST.DefaultValue, ref CreateCommand, ref DropCommand);
                    FieldST.DefaultValueCommand.CreateCommand = CreateCommand;
                    FieldST.DefaultValueCommand.DropCommand = DropCommand;
                    FieldST.DefaultValueCommand.RunCreate = false;
                    FieldST.DefaultValueCommand.RunDrop = false;
                }
                TableST.Fields.Add(FieldST);
            }
        }

        /* **********************************************************************
        * Roda o Script para criar os bancos de dados 
        *************************************************************************/
		public void RunScript(string ConnectionString, List<string> SQLScript)
        {
            string ExecuteLine = "";
            DataContext DbContext = new DataContext(ConnectionString);
            try
            {
                SQLScript = SQLScript.Where(l => l.Trim().Length > 0 && (!l.StartsWith("/*") || !l.EndsWith("*/"))).ToList<string>();

                switch (ServerType)
                {
                    case Util.DatabaseType.SQL:
                         
                            foreach (string Line in SQLScript)
                            {
                                if (Line.Trim().ToUpper() != "GO")
                                {
                                    ExecuteLine += Line + "\r\n";
                                }
                                else
                                {
                                    int RetVal = DbContext.ExecuteCommand(ExecuteLine);
                                    ExecuteLine = "";
                                }
                            }
                            if (ExecuteLine.Length > 0)
                            {
                                int RetVal = DbContext.ExecuteCommand(ExecuteLine);
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
                _ErroList.Add(Ex.Message.ToString());
            }
            finally
            {
                DbContext.Connection.Close();
                DbContext.Dispose();
            }
        }

        /* **********************************************************************
        * Monta o Script de criação da tabela.
        *************************************************************************/
        public void CreateTable(TableStruct Table)
        {
            StringBuilder CreateTableCommand = new StringBuilder();
            bool Needcomma = false;
            switch ((int)ServerType)
            {
                case 0: //SQL

                    CreateTableCommand.Append("CREATE TABLE [" + Table.TableName + "](\r\n");
                    foreach (FieldStruct Field in Table.Fields)
                    {
                        if (Needcomma) CreateTableCommand.Append(",\r\n");

                        string FieldSize = GetFieldSize(Field);
                        string DefaultValue = (Field.DefaultValue.Length > 0) ? DefaultValue = "\t\tDEFAULT " + Field.DefaultValue : "\t\t";
                        string IsNullable = (!Field.IsNullable) ? "  NOT NULL" : "\t\tNULL";
                        string IsIdentity = (Field.IsIdentity) ? "  IDENTITY (" + Field.SeedValue + ",  " + Field.IncrementValue + " ) " + (Field.IsNotForReplication ? "NOT FOR REPLICATION" : "") : "";
                        CreateTableCommand.Append("\t[" + Field.FieldName + "]\t\t" + Field.FieldType + FieldSize + DefaultValue + IsNullable + IsIdentity);
                        Needcomma = true;
                    }
                    CreateTableCommand.Append("\r\n)");

                    AddCommandStruct(CommandType.CreateTable, CreateTableCommand.ToString());
                    break;

                case 1://MYSQL

                    CreateTableCommand.Append("CREATE TABLE `" + Table.TableName + "`(\r\n");
                    foreach (FieldStruct Field in Table.Fields)
                    {
                        if (Needcomma) CreateTableCommand.Append(",\r\n");

                        string FieldSize = GetFieldSize(Field);
                        string DefaultValue = (Field.DefaultValue.Length > 0) ? DefaultValue = "\t\tDEFAULT '" + Field.DefaultValue +"'": "\t\t";
                        string IsNullable = (!Field.IsNullable) ? "  NOT NULL" : "\t\tNULL";
                        string IsIdentity = (Field.IsIdentity) ? "  AUTO_INCREMENT " : "";
                        CreateTableCommand.Append("\t`" + Field.FieldName + "`\t\t" + Field.FieldType + FieldSize  + IsNullable + DefaultValue +  IsIdentity);
                        Needcomma = true;
                    }
                    string PKCols = "";
                    foreach (IndexStruct PK in Table.PrimaryKey)
                    {
                        foreach (KeyValuePair<string,bool> Col in PK.ColumnName)
                        {
                            PKCols += "`" + Col.Key + "`,";
                        }
                        PKCols = PKCols.Trim(new char[] { ',' });
                        CreateTableCommand.Append(",\r\nPRIMARY KEY (" + PKCols + ")");
                        PK.RunCreate = false;
                    }
                    CreateTableCommand.Append(")");
                    AddCommandStruct(CommandType.CreateTable, CreateTableCommand.ToString());
                    break;
            }
        }

        public string GetFieldSize(FieldStruct Field)
        {																																												//INT tem tamanho no MYSQL
            if (Field.FieldType.ToUpper() == "VARCHAR" || Field.FieldType.ToUpper() == "NCHAR" || Field.FieldType.ToUpper() == "VARBINARY" || Field.FieldType.ToUpper() == "NVARCHAR" || Field.FieldType.ToUpper() == "INT" || Field.FieldType.ToUpper() == "NVARCHAR" && Field.FieldSize != null)
            {
                if (Field.FieldType.ToUpper() == "INT" && ServerType == Util.DatabaseType.SQL) return ""; // INT não tem tamanho no SQL

                if (Field.FieldSize != -1 && Field.FieldSize != null)
                {
                    return " (" + Field.FieldSize.ToString() + ")";
                }
                else
                {
                    return " (MAX)";
                }
            }
            else if (Field.FieldType.ToUpper() == "DECIMAL")
            {
                return " (" + Field.NumericPrecision.ToString() + ", " + Field.NumericScale.ToString() + ")";
            }
            else
            {
                return "";
            }
        }

        /********************************************************************************************
        * Carrega todas as chaves primárias e índices
        *********************************************************************************************/
        public void LoadKeys()
        {
            LoadKeyPK(_ConnectitonStringNew, DBStructNew, DbNewInfo.DbName, false);
            LoadKeyPK(_ConnectitonStringOld, DBStructOld, DbOldInfo.DbName, true);
        }

        public void LoadKeyPK(string ConnectionString, DataBaseStruct DBStruct, string DbName, bool Drop)
        {
            LoadKeysFK(ConnectionString, DBStruct, Drop); //Carrega as FKs

            DataView DV_PK = new DataView(QueryExecute(ConnectionString, _QPKs(DbName)));

            foreach (DataRowView item in DV_PK)
            {

                IndexStruct Index = new IndexStruct();
                Index.ColumnName = new Dictionary<string, bool>();

                TableStruct PK = (from t in DBStruct.Tables
                                  where t.TableName == item.Row.ItemArray[0].ToString()
                                  select t).SingleOrDefault();

                if (PK != null)
                {
                    IndexStruct FoundIndex = new IndexStruct();

                    bool IsUnique = false;
                    bool Isprimary = false;
                    bool IsDecendent = false;

                    bool.TryParse(item.Row.ItemArray[5].ToString(), out IsUnique);
                    bool.TryParse(item.Row.ItemArray[6].ToString(), out Isprimary);
                    bool.TryParse(item.Row.ItemArray[7].ToString(), out IsDecendent);

                    if (Isprimary) //  Verifica se é primaryKey ou index
                    {
                        FoundIndex = (from t in PK.PrimaryKey
                                      where t.Name == item.Row.ItemArray[0].ToString() + "." + item.Row.ItemArray[2].ToString()
                                      select t).SingleOrDefault();

                    }
                    else
                    {

                        FoundIndex = (from t in PK.Index
                                      where t.Name == item.Row.ItemArray[0].ToString() + "." + item.Row.ItemArray[2].ToString()
                                      select t).SingleOrDefault();
                    }

                    if (FoundIndex == null)
                    {

                        Index.ColumnName.Add(item.Row.ItemArray[1].ToString(), IsDecendent);
                        Index.Name = item.Row.ItemArray[0].ToString() + "." + item.Row.ItemArray[2].ToString();
                        Index.IsCluster = item.Row.ItemArray[4].ToString();
                        Index.IsUnique = (IsUnique);
                        Index.FillFactor = Convert.ToInt32(item.Row.ItemArray[3]);
                        Index.RunDrop = Drop;

                        if (Isprimary) //  Verifica se é primaryKey ou index
                        {
                            PK.PrimaryKey.Add(Index);
                        }
                        else
                        {
                            PK.Index.Add(Index);
                        }
                    }
                    else
                    {
                        FoundIndex.ColumnName.Add(item.Row.ItemArray[1].ToString(), IsDecendent);
                    }
                }
            }

            foreach (TableStruct Table in DBStruct.Tables)//Vamos carregar todos os comandos
            {
                foreach (IndexStruct PK in Table.PrimaryKey)
                {
                    PK.CreateCommand = CreatePKCommand(Table.TableName, PK, true);
                    PK.DropCommand = CreatePKCommand(Table.TableName, PK, false);
                }
            }

            foreach (TableStruct Table in DBStruct.Tables)//Vamos carregar todos os comandos
            {
                foreach (IndexStruct Index in Table.Index)
                {
                    Index.CreateCommand = CreateIndexCommand(Table.TableName, Index, true);
                    Index.DropCommand = CreateIndexCommand(Table.TableName, Index, false);
                }
            }

        }

        /********************************************************************************************
        * Carrega todas os  FK  para dentro da struct
        *********************************************************************************************/
        public void LoadKeysFK(string ConnectionString, DataBaseStruct DBStruct, bool drop)
        {
            DataView DV_FK = new DataView(QueryExecute(ConnectionString, _QFKs(DBStruct.DatabaseName)));

            foreach (DataRowView item in DV_FK)
            {

                FKStruct Fk = new FKStruct();
                Fk.ColReferencedColReferencing = new Dictionary<string, string>();

                TableStruct Table = (from t in DBStruct.Tables
                                     where t.TableName == item.Row.ItemArray[5].ToString()
                                     select t).SingleOrDefault();

                if (Table != null)
                {

                    FKStruct FoundFK = (from t in Table.ForeignKey
                                        where t.Name == item.Row.ItemArray[0].ToString()
                                        select t).SingleOrDefault();
                    if (FoundFK == null)
                    {
                        Fk.Name = item.Row.ItemArray[0].ToString();
                        Fk.UpdateCascate = (item.Row.ItemArray[1].ToString() == "1");
                        Fk.DeleteCascate = (item.Row.ItemArray[2].ToString() == "1");
                        Fk.ReferencedTable = item.Row.ItemArray[3].ToString();
                        Fk.RunDrop = drop;
                        Fk.ColReferencedColReferencing.Add(item.Row.ItemArray[4].ToString(), item.Row.ItemArray[6].ToString());
                        Table.ForeignKey.Add(Fk);
                    }
                    else
                    {
                        FoundFK.ColReferencedColReferencing.Add(item.Row.ItemArray[4].ToString(), item.Row.ItemArray[6].ToString());
                    }
                }
            }

            foreach (TableStruct Table in DBStruct.Tables)//Vamos carregar todos os comandos
            {
                foreach (FKStruct Fk in Table.ForeignKey)
                {
                    Fk.CreateCommand = CreateFKCommand(Table.TableName, Fk, true);
                    Fk.DropCommand = CreateFKCommand(Table.TableName, Fk, false);
                }
            }
        }

        /* *******************************************************************************************
        * Carrega todas as triggers para dentro da variável ScriptTrigger
        * baseado nas triggers criadas no banco modelo
        *********************************************************************************************/

        public void LoadTrigger()
        {

            switch ((int)ServerType)
            {
                case 0: //SQL
                    DataTable DT_Triggers = new DataTable();

                    DataView DV_Triggers = new DataView(QueryExecute(_ConnectitonStringNew, _QTriggers(DbNewInfo.DbName)));
                    foreach (DataRowView item in DV_Triggers)
                    {
                        TriggerStruct Trigger = new TriggerStruct();
                        Trigger.RunCreate = false;
                        Trigger.RunDrop = false;
                        Trigger.Name = item[0].ToString();
                        Trigger.CreateCommand = item[2].ToString();
                        Trigger.DropCommand = ("IF EXISTS (SELECT name FROM sysobjects WHERE name = '" + item[0].ToString() + "' AND type = 'TR')\r\n" +
                                               "Drop Trigger " + item[0].ToString());

                        TableStruct Table = (from t in DBStructNew.Tables
                                             where t.TableName == item[1].ToString()
                                             select t).SingleOrDefault();
                        Table.Triggers.Add(Trigger);
                    }

                    DV_Triggers = new DataView(QueryExecute(_ConnectitonStringOld, _QTriggers(DbOldInfo.DbName)));
                    foreach (DataRowView item in DV_Triggers)
                    {
                        TriggerStruct Trigger = new TriggerStruct();
                        Trigger.RunCreate = false;
                        Trigger.RunDrop = true;
                        Trigger.Name = item[0].ToString();
                        Trigger.CreateCommand = item[2].ToString();
                        Trigger.DropCommand = ("IF EXISTS (SELECT name FROM sysobjects WHERE name = '" + item[0].ToString() + "' AND type = 'TR')\r\n" +
                                               "Drop Trigger " + item[0].ToString());

                        TableStruct Table = (from t in DBStructOld.Tables
                                             where t.TableName == item[1].ToString()
                                             select t).SingleOrDefault();
                        Table.Triggers.Add(Trigger);
                    }
                    break;
                case 1://MYSQL
                    break;
            }
        }

        /* *******************************************************************************************
        * Carrega todas as  PROCEDURE  para dentro da variável ScriptProcedure
        *********************************************************************************************/
        public void LoadProcedure(string NewDatabaseName, string OldDatabaseName)
        {
            DataTable DT_Procedures = new DataTable();

            //Carrega as procedures na tabela NOVA
            DataView DV_Procedures = new DataView(QueryExecute(_ConnectitonStringNew, _QProcedures(NewDatabaseName)));
            foreach (DataRowView item in DV_Procedures)
            {

                ProcedureStruct ProcItemN = (from p in DBStructNew.Procedures // A proc foi encontrada vamos marcar para não dropar 
                                             where p.Name == item[0].ToString()
                                             select p).SingleOrDefault();

                if (ProcItemN != null)
                {
                    ProcItemN.CreateCommand = ProcItemN.CreateCommand + item[1].ToString();
                }
                else
                {

                    ProcedureStruct Proc = new ProcedureStruct();
                    Proc.RunCreate = false;
                    Proc.RunDrop = false;
                    string XType = item["xtype"].ToString().ToUpper().Trim();
                    string Type = XType == "V" ? "VIEW" : "PROCEDURE";
                    Proc.Name = item[0].ToString();
					string Command = item[1].ToString();
					if (Type == "VIEW")
					{
						Command = Command.Replace(String.Format("`{0}`.", DBStructNew.DatabaseName), "");
					}
					Proc.CreateCommand = ScriptsViewAndProcCreate(Type, item[0].ToString(), Command);
                    Proc.DropCommand = ScriptsViewAndProcDrop(Type, item[0].ToString());
                    Proc.DependenceView = new List<string>();
                    DBStructNew.Procedures.Add(Proc);
                }

            }

            //Carrega as procedures na tabela OLD
            DV_Procedures = new DataView(QueryExecute(_ConnectitonStringOld, _QProcedures(OldDatabaseName)));
            foreach (DataRowView item in DV_Procedures)
            {

                ProcedureStruct ProcItemO = (from p in DBStructOld.Procedures // A proc foi encontrada vamos marcar para não dropar 
                                             where p.Name == item[0].ToString()
                                             select p).SingleOrDefault();

                if (ProcItemO != null)
                {
                    ProcItemO.CreateCommand = ProcItemO.CreateCommand + item[1].ToString();
                }
                else
                {

                    ProcedureStruct Proc = new ProcedureStruct();
                    Proc.RunCreate = false;
                    Proc.RunDrop = true;
                    Proc.Name = item[0].ToString();
					string XType = item["xtype"].ToString().ToUpper().Trim();
					string Type = "";
					switch (XType)
					{
						case "V":
							Type = "VIEW";
							break;
						case "FN":
							Type = "FUNCTION";
							break;
						default:
							Type = "PROCEDURE";
							break;
					}
                    string Command = item[1].ToString();
					if (Type == "VIEW" )
					{
						Command = Command.Replace(String.Format("`{0}`.", DBStructOld.DatabaseName), "");
					}
					Proc.CreateCommand = ScriptsViewAndProcCreate(Type, item[0].ToString(), Command);
                    Proc.DropCommand = ScriptsViewAndProcDrop(Type, item[0].ToString());
                    Proc.DependenceView = new List<string>();
                    DBStructOld.Procedures.Add(Proc);
                }
            }

            //Vamos Setar as dependeceViews

		if (ServerType == Util.DatabaseType.SQL)
		{
			DataView DV_DependeceView = new DataView(QueryExecute(_ConnectitonStringNew, _QDependenceView(NewDatabaseName)));
			foreach (DataRowView item in DV_DependeceView)
			{

        			ProcedureStruct ProcItemN = (from p in DBStructNew.Procedures // A proc foi encontrada vamos marcar para não dropar 
                                     where p.Name == item[0].ToString()
                                     select p).SingleOrDefault();

				if (ProcItemN != null)
				{
					ProcItemN.DependenceView.Add(item[1].ToString());
				}
			}
		}
        }

        /**************
       * Compara FKs
       ***************/
        private bool CompareFks()
        {
            bool Erro = false;
            foreach (TableStruct Table in DBStructNew.Tables)
            {

                string TableReference = Table.TableName;
                if (Table.OldTableName != null)
                {
                    TableReference = Table.OldTableName;
                }

                List<FKStruct> FkOLength = (from f in DBStructOld.Tables
                                            where f.TableName == TableReference
                                            from pk in f.ForeignKey
                                            select pk).ToList<FKStruct>();

                if (Table.ForeignKey.Count != FkOLength.Count)
                {
                    Erro = true;
                }

                foreach (FKStruct FkN in Table.ForeignKey)
                {

                    FKStruct FkO = (from f in DBStructOld.Tables
                                    where f.TableName == TableReference
                                    from pk in f.ForeignKey
                                    where pk.Name == FkN.Name
                                    select pk).SingleOrDefault();

                    TableStruct TableRelation = (from t in DBStructOld.Tables
                                                 where t.TableName == FkN.ReferencedTable
                                         select t).SingleOrDefault();

                    if (TableRelation != null)
                    {
                        foreach (IndexStruct PK in TableRelation.PrimaryKey)
                        {
                            if (PK.RunDrop)
                            {
                                foreach (KeyValuePair<string, string> ColRef in FkN.ColReferencedColReferencing)
                                {
                                    foreach (KeyValuePair<string,bool> item in PK.ColumnName)
                                    {
                                        if (ColRef.Key == item.Key)
                                        {
                                            FkN.RunCreate = true;
                                            FkN.RunDrop = true;
                                        }
                                    }
                                }
                            }
                        }
                    
                    }

                    if (FkO != null)
                    {
                        FkO.RunDrop = false;
                        bool _Erro = CompareFKStruct(FkN, FkO);

                        if (_Erro)
                        {
                            Erro = _Erro;
                            FkN.RunDrop = true;
                            FkN.RunCreate = true;
                            AddDiff(Table.TableName, "ForeignKey alterada: " + FkN.Name);
                        }
                    }
                    else
                    {
                        FkN.RunCreate = true;
                        Erro = true;
                        AddDiff(Table.TableName, "ForeignKey criada: " + FkN.Name);
                    }
                }
            }
            return Erro;
        }

        /* *******************************************************************************************
        * Cria o Script de inclusão e exclusão de ForeignKey
        *********************************************************************************************/

        public string CreateFKCommand(string TableName, FKStruct FkStruct, bool IsCreate)
        {

            string Update = "";
            string Delete = "";
            string ColumnsName = "";
            string ColumnRefName = "";
            string Comma = "";

            switch ((int)ServerType)
            {
                case 0: //SQL

                    Update = (FkStruct.UpdateCascate) ? "\r\nON UPDATE CASCADE" : "";
                    Delete = (FkStruct.DeleteCascate) ? "\r\nON DELETE CASCADE" : "";
                    ColumnsName = "";
                    ColumnRefName = "";
                    Comma = "";
                    foreach (KeyValuePair<string, string> PkCName in FkStruct.ColReferencedColReferencing)
                    {
                        ColumnsName += Comma + "[" + PkCName.Value + "]";
                        ColumnRefName += Comma + "[" + PkCName.Key + "]";
                        Comma = ",";
                    }

                    if (IsCreate)
                    {
                        return ("ALTER TABLE [" + TableName + "] \r\n" +
                                         "ADD CONSTRAINT [" + FkStruct.Name + "] \r\n" +
                                         "FOREIGN KEY \r\n(" + ColumnsName + ")\r\n" +
                                         "REFERENCES [" + FkStruct.ReferencedTable + "] \r\n(" + ColumnRefName + ")" +
                                         Update.ToString() + Delete.ToString());

                    }
                    else
                    {
                        return ("IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_SCHEMA='dbo' AND CONSTRAINT_NAME='" + FkStruct.Name + "' AND TABLE_NAME='" + TableName + "') \r\n" +
                                         "BEGIN \r\n" +
                                         "ALTER TABLE [" + TableName + "] DROP CONSTRAINT [" + FkStruct.Name + "]\r\nEND");

                    }
                case 1://MYSQL
                    Update = (FkStruct.UpdateCascate) ? "\r\nON UPDATE CASCADE" : "";
                    Delete = (FkStruct.DeleteCascate) ? "\r\nON DELETE CASCADE" : "";
                    ColumnsName = "";
                    ColumnRefName = "";
                    Comma = "";
                    foreach (KeyValuePair<string, string> PkCName in FkStruct.ColReferencedColReferencing)
                    {
                        ColumnsName += Comma + "`" + PkCName.Value + "`";
                        ColumnRefName += Comma + "`" + PkCName.Key + "`";
                        Comma = ",";
                    }

                    if (IsCreate)
                    {
                        return ("ALTER TABLE `" + TableName + "` \r\n" +
                                         "ADD CONSTRAINT `" + FkStruct.Name + "` \r\n" +
                                         "FOREIGN KEY \r\n(" + ColumnsName + ")\r\n" +
                                         "REFERENCES `" + FkStruct.ReferencedTable + "` \r\n(" + ColumnRefName + ")" +
                                         Update.ToString() + Delete.ToString());

                    }
                    else
                    {
                        return ("ALTER TABLE '" + TableName + "' DROP FOREIGN KEY '" + FkStruct.Name + "';");
                    }

                    break;
            }
            return "";
        }

        /* *******************************************************************************************
        * Cria o Script de inclusão e exclusão de PrimaryKey
        *********************************************************************************************/

        public string CreatePKCommand(string TableName, IndexStruct PkStruct, bool IsCreate)
        {
            string IsCluster = "";
            string ColumnName = "";
            string FFactor = "";
            string Comma = "";

            switch ((int)ServerType)
            {
                case 0: //SQL

                    IsCluster = (PkStruct.IsCluster.ToString() == "CLUSTERED") ? "CLUSTERED " : "NONCLUSTERED ";
                    FFactor = (PkStruct.FillFactor > 0) ? "\r\n\tWITH FILLFACTOR = " + PkStruct.FillFactor.ToString() : "";
                    string ConstrantName = PkStruct.Name.Split('.')[1];
                    foreach (KeyValuePair<string, bool> PkCName in PkStruct.ColumnName)
                    {
                        ColumnName += Comma + "[" + PkCName.Key.ToString() + "]" + ((bool)PkCName.Value ? " DESC" : " ASC");
                        Comma = ",";
                    }

                    if (IsCreate)
                    {
                        return ("IF not EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_SCHEMA='dbo' AND CONSTRAINT_NAME='" + PkStruct.Name + "' AND TABLE_NAME='" + TableName + "') \r\n" +
                                                                    "BEGIN \r\n" +
                                                                    "\tALTER TABLE [" + TableName + "] ADD  CONSTRAINT [" + ConstrantName + "] PRIMARY KEY " + IsCluster + "\r\n\t(" + ColumnName + ")" + FFactor + "\r\nEND");
                    }
                    else
                    {
                        return ("IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_SCHEMA='dbo' AND CONSTRAINT_NAME='" + ConstrantName + "' AND TABLE_NAME='" + TableName + "') \r\n" +
                                 "BEGIN \r\n" +
                                 "ALTER TABLE [" + TableName + "] DROP CONSTRAINT [" + ConstrantName + "]\r\nEND");
                    }
                case 1://MYSQL

                    FFactor = (PkStruct.FillFactor > 0) ? "\r\n\tWITH FILLFACTOR = " + PkStruct.FillFactor.ToString() : "";

                    foreach (KeyValuePair<string, bool> PkCName in PkStruct.ColumnName)
                    {
                        ColumnName += Comma + "`" + PkCName.Key.ToString() + "`" + ((bool)PkCName.Value ? " DESC" : " ASC");
                        Comma = ",";
                    }

                    if (IsCreate)
                    {
                        return ("\tALTER TABLE `" + TableName + "` ADD PRIMARY KEY \r\n\t(" + ColumnName + ")");
                    }
                    else
                    {
                        return ("ALTER TABLE `" + TableName + "` DROP PRIMARY KEY");
                    }
            }
            return "";
        }

        /* *******************************************************************************************
         * Cria o Script de inclusão e exclusão dos índices
         *********************************************************************************************/

        public string CreateIndexCommand(string TableName, IndexStruct IndexStruct, bool IsCreate)
        {

            string IsCluster = "";
            string ColumnName = "";
            string FFactor = "";
            string IsUnique = "";
            string Comma = "";
            string IndexName = IndexStruct.Name.Split('.')[1];
            switch ((int)ServerType)
            {

                case 0: //SQL
                    IsCluster = (IndexStruct.IsCluster.ToString() == "CLUSTERED") ? "CLUSTERED " : "NONCLUSTERED ";
                    FFactor = (IndexStruct.FillFactor > 0) ? "\r\n\tWITH FILLFACTOR = " + IndexStruct.FillFactor.ToString() : "";
                    IsUnique = ((bool)IndexStruct.IsUnique) ? " UNIQUE " : "";

                    foreach (KeyValuePair<string, bool> PkCName in IndexStruct.ColumnName)
                    {
                        ColumnName += Comma + "[" + PkCName.Key.ToString() + "]" + ((bool)PkCName.Value ? " DESC" : " ASC");
                        Comma = ",";
                    }

                    if (IsCreate)
                    {
                        return ("CREATE " + IsUnique + " " + IsCluster + " INDEX [" + IndexName + "] ON [" + TableName + "]" +
                        "(" + ColumnName + ")" + FFactor);
                    }
                    else
                    {
                        return ("IF EXISTS (SELECT name FROM sysindexes WHERE name = '" + IndexName + "')DROP INDEX [" + TableName + "].[" + IndexName + "]");
                    }
                case 1://MYSQL

                    FFactor = (IndexStruct.FillFactor > 0) ? "\r\n\tWITH FILLFACTOR = " + IndexStruct.FillFactor.ToString() : "";
                    IsUnique = ((bool)IndexStruct.IsUnique) ? " UNIQUE " : "";

                    foreach (KeyValuePair<string, bool> PkCName in IndexStruct.ColumnName)
                    {
                        ColumnName += Comma + "`" + PkCName.Key.ToString() + "`" + ((bool)PkCName.Value ? " DESC" : " ASC");
                        Comma = ",";
                    }

                    if (IsCreate)
                    {
                        return ("CREATE " + IsUnique + " INDEX `" + IndexName + "` ON `" + TableName + "` (" + ColumnName + ")");
                    }
                    else
                    {
                        return ("DROP INDEX `" + IndexName + "` ON `" + TableName + "`");
                    }
                    break;
            }
            return "";
        }
        /*********************
        * Compara FKStruct
        **********************/

        private bool CompareFKStruct(FKStruct FKN, FKStruct FKO)
        {
            bool _Erro = false;

            if (FKN.ColReferencedColReferencing.Count == FKO.ColReferencedColReferencing.Count)
            {
                foreach (KeyValuePair<string, string> FkCols in FKN.ColReferencedColReferencing) // Vamos Procurar se as colunas existem e se a ordenação esta correta
                {
                    if (FKO.ColReferencedColReferencing.ContainsKey(FkCols.Key.ToString()))
                    {

                        if (FKO.ColReferencedColReferencing[FkCols.Key.ToString()] != FkCols.Value)
                        {
                            _Erro = true;                                //Ordenação foi alterada
                        }
                    }
                    else
                    {
                        _Erro = true;                                     //Campo não existe
                    }
                }

                if (FKN.DeleteCascate != FKO.DeleteCascate || FKN.ReferencedTable != FKO.ReferencedTable || FKN.UpdateCascate != FKO.UpdateCascate) // Vamos verificar se a PK é igual.
                {
                    _Erro = true;
                }
            }
            else
            {
                _Erro = true; //Quantidade de campos diferentes vamos criar novamente a PK
            }

            return _Erro;
        }

        /*********************
        * Compara índicesStruct
        **********************/

        private bool CompareIndexStruct(IndexStruct IndexN, IndexStruct IndexO)
        {
            bool _Erro = false;

            if (IndexN.ColumnName.Count == IndexO.ColumnName.Count)
            {
                foreach (KeyValuePair<string, bool> IndexName in IndexN.ColumnName) // Vamos Procurar se as colunas existem e se a ordenação esta correta
                {
                    if (IndexO.ColumnName.ContainsKey(IndexName.Key.ToString()))
                    {

                        if (IndexO.ColumnName[IndexName.Key.ToString()] != IndexName.Value)
                        {
                            _Erro = true;                                //Ordenação foi alterada
                        }
                    }
                    else
                    {
                        _Erro = true;                                     //Campo não existe
                    }
                }

                if (IndexN.IsCluster != IndexO.IsCluster || IndexN.IsUnique != IndexO.IsUnique) // Vamos verificar se a PK é igual.
                {
                    _Erro = true;
                }
            }
            else
            {
                _Erro = true; //Quantidade de campos diferentes vamos criar novamente a PK
            }
            return _Erro;
        }

        /*********************
        * Compara índices
        **********************/

        private bool CompareIndex()
        {
            bool Erro = false;
            foreach (TableStruct Table in DBStructNew.Tables)
            {

                string TableReference = Table.TableName;
                if (Table.OldTableName != null)
                {
                    TableReference = Table.OldTableName;
                }

                List<IndexStruct> PkOLength = (from f in DBStructOld.Tables
                                               where f.TableName == TableReference
                                               from pk in f.Index
                                               select pk).ToList<IndexStruct>();

                if (Table.Index.Count != PkOLength.Count)
                {
                    Erro = true;
                }

                foreach (IndexStruct IndexN in Table.Index)
                {
                    IndexStruct IndexO = (from f in DBStructOld.Tables
                                          where f.TableName == TableReference
                                          from pk in f.Index
                                          where pk.Name == IndexN.Name
                                          select pk).SingleOrDefault();

                    if (IndexO != null)
                    {
                        IndexO.RunDrop = false;
                        bool _Erro = CompareIndexStruct(IndexN, IndexO);

                        if (_Erro)
                        {
                            Erro = _Erro;
                            IndexN.RunDrop = true;
                            IndexN.RunCreate = true;
                            AddDiff(Table.TableName, "Index alterado: " + IndexN.Name);
                        }
                    }
                    else
                    {
                        IndexN.RunCreate = true;
                        Erro = true;
                        AddDiff(Table.TableName, "Index Criado: " + IndexN.Name);
                    }
                }
            }
            return Erro;
        }

        //***************
        //* Compara PKs
        //****************/

        private bool ComparePks()
        {
            bool Erro = false;
            foreach (TableStruct Table in DBStructNew.Tables)
            {

                string TableReference = Table.TableName;
                if (Table.OldTableName != null)
                {
                    TableReference = Table.OldTableName;
                }

                List<IndexStruct> PkOLength = (from f in DBStructOld.Tables
                                               where f.TableName == TableReference
                                               from pk in f.PrimaryKey
                                               select pk).ToList<IndexStruct>();

                if (Table.PrimaryKey.Count != PkOLength.Count)
                {
                    Erro = true;

                }

                foreach (IndexStruct PkN in Table.PrimaryKey)
                {
                    IndexStruct PkO = (from f in DBStructOld.Tables
                                       where f.TableName == TableReference
                                       from pk in f.PrimaryKey
                                       where pk.Name == PkN.Name
                                       select pk).SingleOrDefault();

                    if (PkO != null)
                    {
                        PkO.RunDrop = false;
                        bool _Erro = CompareIndexStruct(PkN, PkO);

                        if (_Erro)
                        {
                            Erro = _Erro;
                            PkN.RunDrop = true;
                            PkN.RunCreate = true;
                            AddDiff(Table.TableName, "PrimaryKey alterada: " + PkN.Name);
                        }
                    }
                    else
                    {
                        PkN.RunCreate = true;
                        Erro = true;
                        AddDiff(Table.TableName, "PrimaryKey criada: " + PkN.Name);
                    }
                }
            }
            return Erro;
        }

        /*****************
        * Compara Triggers
        ******************/
        private bool CompareTriggers()
        {
            bool Erro = false;

            foreach (TableStruct Table in DBStructNew.Tables)
            {
                foreach (TriggerStruct Trigger in Table.Triggers)
                {
                    string TableReference = Table.TableName;
                    if (Table.OldTableName != null)
                    {
                        TableReference = Table.OldTableName;
                    }

                    TriggerStruct FindTrigger = (from f in DBStructOld.Tables
                                                 where f.TableName == TableReference
                                                 from Tr in f.Triggers
                                                 where Tr.Name == Trigger.Name
                                                 select Tr).SingleOrDefault();

                    if (FindTrigger != null)        // Encontrou a Trigger no DB velho
                    {
                        FindTrigger.RunDrop = false;

                        if (!NormalizeText(Trigger.CreateCommand.ToString()).Equals(NormalizeText(FindTrigger.CreateCommand.ToString())))
                        {
                            Trigger.RunDrop = true;
                            Trigger.RunCreate = true;
                            Erro = true;
                            AddDiff(Table.TableName, "Trigger alterada:" + Trigger.Name);
                        }
                        else
                        {
                            Trigger.RunCreate = false;
                        }
                    }
                    else
                    {
                        Trigger.RunCreate = true;
                        Erro = true;
                        AddDiff(Table.TableName, "Trigger criada:" + Trigger.Name);
                    }
                }
            }
            return Erro;
        }

        /*****************
        * Compara Procedures
        ******************/

        private bool CompareProcedures()
        {
            bool Erro = false;
            foreach (ProcedureStruct NewProc in DBStructNew.Procedures)
            {
                ProcedureStruct ProcItemO = (from p in DBStructOld.Procedures // A proc foi encontrada vamos marcar para não dropar 
                                             where p.Name == NewProc.Name
                                             select p).SingleOrDefault();

                if (ProcItemO != null)        // Encontrou a proc no DB velho
                {
                    ProcItemO.RunDrop = false;

                    if (!NormalizeText(NewProc.CreateCommand.ToString()).Equals(NormalizeText(ProcItemO.CreateCommand)))
                    {
                        NewProc.RunDrop = true;
                        NewProc.RunCreate = true;
                        Erro = true;
                        AddDiff("Procedures", "Procedure alterada: " + NewProc.Name);
                    }
                    else
                    {
                        NewProc.RunCreate = false;
                    }
                }
                else
                {
                    NewProc.RunCreate = true;
                    Erro = true;
                    AddDiff("Procedures", "Procedure criada: " + NewProc.Name);
                }
            }

            return Erro;

        }

        /******************************************
        * Carrega no Script de adaptação ordenado 
        ******************************************/

        public string GetScriptAdapter()
        {
            StringBuilder Script = new StringBuilder();

            ProgrammabilityCommand = (from p in ProgrammabilityCommand orderby GetCommandValuePS(p.Type), ProgrammabilityCommand.IndexOf(p as ProgrammabilityCommandsStruct) select p).ToList();

            char[] trim = new char[] { ' ', '\r', '\n', ';' };
            string separator = "";
            string CommandEnd = "";

            if (ServerType == Util.DatabaseType.SQL)
            {
                separator = "GO\r\n";
            }
            else if (ServerType == Util.DatabaseType.MYSQL)
            {
                CommandEnd = ";\r\n";
            }

            foreach (ProgrammabilityCommandsStruct item in ProgrammabilityCommand)
            {
                Script.Append(item.Command.Trim(trim) + CommandEnd + "\r\n" + separator + "\r\n");
            }
            return Script.ToString();
        }

        /************************************************
        * Normaliza o texto Removendo duas ocorrências
        * de \r\n , espaços duplos , Tabs , Comentários
        ************************************************/
        public string NormalizeText(string Text)
        {

            Dictionary<string, string> TypesComments = new Dictionary<string, string>();
            TypesComments.Add("/*", "*/");
            TypesComments.Add("--", "\r\n");

            foreach (KeyValuePair<string, string> Comments in TypesComments)
            {
                int OldSize = -1;
                while (OldSize != Text.Length)  //Remove os comentários SQL
                {
                    OldSize = Text.Length;
                    int StartComment = Text.IndexOf(Comments.Key);
                    int FinishComment = Text.IndexOf(Comments.Value);
                    if (StartComment >= 0 && FinishComment > StartComment)
                    {
                        Text = Text.Remove(StartComment, FinishComment - StartComment + Comments.Value.Length);
                    }
                }
            }

            Text = Text.Replace("\r", " ");
            Text = Text.Replace("\n", " ");
            Text = Text.Replace("\t", " ");
            Text = Text.Trim();
            Text = Text.Trim(';');

            int TLenght = 0;
            while (Text.Length != TLenght)
            {
                TLenght = Text.Length;
                Text = Text.Replace("  ", " ");
            }

            return Text;
        }

        /* *******************************************************************************************
       * Cria o Script de Update de campo
       *********************************************************************************************/

        public void ScriptUpdateField(string TableName, string ColumnName, string UpdateColumn)
        {

            switch ((int)ServerType)
            {
                case 0: //SQL

                    AddCommandStruct(CommandType.UpdateField, "UPDATE [" + TableName + "] SET [" + ColumnName + "] = [" + UpdateColumn + "]");
                    break;

                case 1://Oracle
                    break;
            }
        }

        /* *******************************************************************************************
         * Cria o Script de remoção de tabela
         *********************************************************************************************/

        public void ScriptDropField(string TableName, FieldStruct Field, bool OldTable)
        {
            string Command = "";
            switch ((int)ServerType)
            {
                case 0: //SQL
                    Command = ("ALTER TABLE [" + TableName + "] DROP COLUMN [" + Field.FieldName + "]");

                    break;

                case 1://MYSQL
                    Command = ("ALTER TABLE `" + TableName + "` DROP COLUMN `" + Field.FieldName + "`");
                    break;
            }

            if (OldTable)
            {
                AddCommandStruct(Field.UsedToUpdate ? CommandType.DropUpdateField : CommandType.DropFieldO, Command);
            }
            else
            {
                AddCommandStruct(Field.UsedToUpdate ? CommandType.DropUpdateField : CommandType.DropField, Command);
            }

        }

        /* *******************************************************************************************
         * Cria o Script de remoção de tabela
         *********************************************************************************************/

        public void ScriptDropTable(string TableName)
        {
            AddDiff(TableName, "Tabela Deletada");
            switch ((int)ServerType)
            {
                case 0: //SQL
                    AddCommandStruct(CommandType.DropTable, "Drop Table [" + TableName + "]");
                    break;

                case 1://MYSQL
                    AddCommandStruct(CommandType.DropTable, "Drop Table IF EXISTS `" + TableName + "`");
                    break;
            }
        }

        /* *******************************************************************************************
         * Cria o Script de Deleção Procedure e View
         *********************************************************************************************/
        private string ScriptsViewAndProcDrop(string xtype, string Name)
        {
            if (xtype.ToUpper() == "PROCEDURE")
            {
                switch (ServerType)
                {
                    case Util.DatabaseType.SQL:
                        return ("IF EXISTS (SELECT * FROM   sysobjects where name = '" + Name + "' AND type = 'P')" + "\r\nDROP  PROCEDURE [" + Name + "]");

                    case Util.DatabaseType.MYSQL:
                        return ("DROP PROCEDURE IF EXISTS `" + Name + "`");
                }
            }
			else if (xtype.ToUpper() == "FUNCTION")
			{
				switch (ServerType)
				{
					case Util.DatabaseType.SQL:
						return ("IF EXISTS (SELECT * FROM   sysobjects where name = '" + Name + "' AND type = 'FN')" + "\r\nDROP  FUNCTION [" + Name + "]");
					case Util.DatabaseType.MYSQL:
						return ("DROP FUNCTION IF EXISTS `" + Name + "`");
				}
			}
            else if (xtype.ToUpper() == "VIEW")
            {

                switch (ServerType)
                {
                    case Util.DatabaseType.SQL:
                        return ("IF EXISTS (SELECT * FROM   sysobjects where name = '" + Name + "' AND type = 'V')" + "\r\nDROP  VIEW [" + Name + "]");
                    case Util.DatabaseType.MYSQL:
                        return ("DROP VIEW IF EXISTS `" + Name + "`");
                }
            }
            return "";
        }

        /* *******************************************************************************************
         * Cria o Script de criação Procedure e View
         *********************************************************************************************/
        private string ScriptsViewAndProcCreate(string xtype, string ViewName, string Text)
        {
            if (xtype.ToUpper() == "PROCEDURE")
            {
                switch (ServerType)
                {
                    case Util.DatabaseType.SQL:
                        return Text;
                    case Util.DatabaseType.MYSQL:
                        return Text;
                }
            }
            else if (xtype.ToUpper() == "VIEW")
            {
                switch (ServerType)
                {
                    case Util.DatabaseType.SQL:
                        return Text;
                    case Util.DatabaseType.MYSQL:
                        return "CREATE VIEW " + ViewName + " AS " + Text;
                }
            }
            return "";
        }

        /* *******************************************************************************************
         * Cria o Script de Rename de tabela
         *********************************************************************************************/

        public void ScriptRenameTable(string NewName, string OldName)
        {
            AddDiff(NewName, "Tabela Renomeada de " + OldName + " para " + NewName);
            switch ((int)ServerType)
            {
                case 0: //SQL
                    AddCommandStruct(CommandType.RenameTable, "EXEC sp_rename '" + OldName + "', '" + NewName + "'");
                    break;

                case 1://MYSQL
                    AddCommandStruct(CommandType.RenameTable, "ALTER TABLE '" + OldName + "' RENAME TO '" + NewName + "'");
                    break;
            }
        }

        /* *******************************************************************************************
         * Cria o Script de Rename dos campos
         *********************************************************************************************/

        public void ScriptRenameFieldTMP(string TableName, string NewFieldName, string OldFieldName, FieldStruct Field)
        {

            ScriptRenameField(TableName, NewFieldName, NewFieldName + "TMP", Field);
            switch ((int)ServerType)
            {
                case 0: //SQL

                    AddCommandStruct(CommandType.RenameTemp, "EXECUTE sp_rename '" + TableName + "." + OldFieldName + "', '" + NewFieldName + "TMP" + "', 'COLUMN'");
                    break;

                case 1://MYSQL
                    AddCommandStruct(CommandType.RenameTemp, "'MYSQL4'EXECUTE sp_rename '" + TableName + "." + OldFieldName + "', '" + NewFieldName + "TMP" + "', 'COLUMN'");
                    break;
            }
        }

        /* *******************************************************************************************
         * Cria o Script de Rename dos campos
         *********************************************************************************************/

        public void ScriptRenameField(string TableName, string NewFieldName, string OldFieldName, FieldStruct Field)
        {

            switch ((int)ServerType)
            {
                case 0: //SQL
                    AddCommandStruct(CommandType.RenameField, "EXECUTE sp_rename '" + TableName + "." + OldFieldName + "', '" + NewFieldName + "', 'COLUMN'");
                    break;

                case 1://MYSQL

                    string FieldSize = GetFieldSize(Field);
                    string IsNullable = (!Field.IsNullable) ? " NOT NULL" : " NULL";
                    AddCommandStruct(CommandType.AlterField, "ALTER TABLE `" + TableName + "` change `" + OldFieldName + "`  `" + NewFieldName + "` " + Field.FieldType + FieldSize + IsNullable);
                    break;

                    break;

            }
        }

        /* *******************************************************************************************
        * Cria o Script de alteração dos campos
        *********************************************************************************************/

        public void ScriptAlterField(string TableName, FieldStruct Field, TableStruct TableOld)
        {

            bool DoAlterField = PrepareAlterField(TableName, Field, TableOld);
            string FieldSize = "";
            string IsNullable = "";
            string IsIdentity = (Field.IsIdentity) ? "  AUTO_INCREMENT" : "";

            if (DoAlterField)
            {
                switch ((int)ServerType)
                {
                    case 0: //SQL
                        FieldSize = GetFieldSize(Field);
                        IsNullable = (!Field.IsNullable) ? " NOT NULL" : " NULL";
                        AddCommandStruct(CommandType.AlterField, "ALTER TABLE [" + TableName + "] ALTER COLUMN [" + Field.FieldName + "]  " + Field.FieldType + FieldSize + IsNullable);
                        break;

                    case 1://MYSQL

                        FieldSize = GetFieldSize(Field);
                        IsNullable = (!Field.IsNullable) ? " NOT NULL" : " NULL";
                        AddCommandStruct(CommandType.AlterField, "ALTER TABLE `" + TableName + "` MODIFY `" + Field.FieldName + "`  " + Field.FieldType + FieldSize + IsNullable + IsIdentity);
                        break;
                }
            }
        }

        public void ScriptChangeIdentityField(TableStruct Table, FieldStruct Field)
        {

            FieldStruct OldAssocieted = (from T in DBStructOld.Tables //Vamos Procurar a query de drop DefaultValue
                                         where T.TableName == Table.TableName
                                         from OldField in T.Fields
                                         where OldField.FieldName == Field.FieldName
                                         select OldField).FirstOrDefault();

            TableStruct OldTableAssocieted = (from T in DBStructOld.Tables //Vamos Procurar a query de drop DefaultValue
                                              where T.TableName == Table.TableName
                                              select T).FirstOrDefault();

            if (OldAssocieted != null && OldTableAssocieted != null)
            {

                switch (ServerType)
                {
                    case Util.DatabaseType.SQL:

                        Field.TmpFieldName = Field.FieldName + "TMP";

                        ScriptRenameField(Table.TableName, Field.FieldName + "TMP", Field.FieldName, Field);
                        ScriptCreateField(Table.TableName, Field);
                        ScriptUpdateField(Table.TableName, Field.FieldName, Field.TmpFieldName);

                        Field.ChangesFound = 0;
                        OldAssocieted.FieldName = Field.TmpFieldName;
                        OldAssocieted.Drop = true;
                        OldAssocieted.UsedToUpdate = true;

                        PrepareAlterField(Table.TableName, Field, OldTableAssocieted);

                        break;
                    case Util.DatabaseType.MYSQL:
                        ScriptAlterField(Table.TableName, Field, OldTableAssocieted);
                        break;
                    case Util.DatabaseType.ORACLE:
                        break;
                    default:
                        break;
                }
            }

        }

        /* *******************************************************************************************
        * Cria o Script de ADD dos campos
        *********************************************************************************************/
        public void ScriptCreateField(string TableName, FieldStruct Field)
        {

            string FieldSize = "";
            string IsNullable = "";
            string DefaultValue = "";
            string IsIdentity = "";
            string FieldName = "";

            switch ((int)ServerType)
            {
                case 0: //SQL
                    FieldSize = GetFieldSize(Field);
                    DefaultValue = "";
                    IsIdentity = (Field.IsIdentity) ? "  IDENTITY (" + Field.SeedValue + ",  " + Field.IncrementValue + " ) " + (Field.IsNotForReplication ? "NOT FOR REPLICATION" : "") : "";
                    FieldName = (Field.Renamed && Field.TmpFieldName.Length > 0) ? Field.TmpFieldName : Field.FieldName; //TmpFieldName será usado no caso de campos identity com alteração de estrutura

                    if (Field.DefaultValue.Length > 0 && !Field.IsIdentity)
                    {
                        if (Field.FieldType.ToUpper() != "TIMESTAMP")
                        {
                            DefaultValue = " DEFAULT " + Field.DefaultValue.ToString();
                        }
                    }
                    if (!Field.IsNullable)
                    {
                        IsNullable = " NOT NULL";
                    }
                    else
                    {
                        IsNullable = " NULL";
                    }

                    if (Field.DefaultValue.ToString().Length == 0 && !Field.IsNullable) //Vamos Tratar o caso do usuário criar um campo not null que não tem valor default
                    {
                        string DropDV = "";
                        string CreatepDV = "";

                        if (!Field.IsIdentity)
                        {
                        AddCommandStruct(CommandType.CreateField, "ALTER TABLE [" + TableName + "] ADD [" + FieldName + "] " + Field.FieldType.ToString() + FieldSize + " DEFAULT " + GetDefaultValueByType(Field.FieldType.ToString()) + IsNullable + IsIdentity);
                        }
                        else
                        {
                            AddCommandStruct(CommandType.CreateField, "ALTER TABLE [" + TableName + "] ADD [" + FieldName + "] " + Field.FieldType.ToString() + FieldSize + IsNullable + IsIdentity);
                        }
                        ScriptDefaultValue(TableName, Field.FieldName, "", ref CreatepDV, ref DropDV);
                        AddCommandStruct(CommandType.DropDefaultValueTMP, DropDV);
                    }
                    else if (Field.IsIdentity)
                    {
                        AddCommandStruct(CommandType.CreateField, "ALTER TABLE [" + TableName + "] ADD [" + FieldName + "] " + Field.FieldType.ToString() + FieldSize + IsNullable + IsIdentity);
                    }
                    else
                    {
                        AddCommandStruct(CommandType.CreateField, "ALTER TABLE [" + TableName + "] ADD [" + FieldName + "] " + Field.FieldType.ToString() + FieldSize + DefaultValue + IsNullable + IsIdentity);
                    }

                    break;

                case 1://MYSQL

                    FieldSize = GetFieldSize(Field);
                    DefaultValue = "";
                    IsIdentity = (Field.IsIdentity) ? "  AUTO_INCREMENT" : "";
                    FieldName = (Field.Renamed && Field.TmpFieldName.Length > 0) ? Field.TmpFieldName : Field.FieldName; //TmpFieldName será usado no caso de campos identity com alteração de estrutura

                    if (Field.DefaultValue.Length > 0)
                    {
                        if (Field.FieldType.ToUpper() != "TIMESTAMP")
                        {
                            DefaultValue = " DEFAULT " + Field.DefaultValue.ToString();
                        }
                    }
                    if (!Field.IsNullable)
                    {
                        IsNullable = " NOT NULL";
                    }
                    else
                    {
                        IsNullable = " NULL";
                    }

                    if (Field.DefaultValue.ToString().Length == 0 && !Field.IsNullable) //Vamos Tratar o caso do usuário criar um campo not null que não tem valor default
                    {
                        string DropDV = "";
                        string CreatepDV = "";
                        AddCommandStruct(CommandType.CreateField, "ALTER TABLE `" + TableName + "` ADD `" + FieldName + "` " + Field.FieldType.ToString() + FieldSize + " DEFAULT " + GetDefaultValueByType(Field.FieldType.ToString()) + IsNullable + IsIdentity);
                        ScriptDefaultValue(TableName, Field.FieldName, "", ref CreatepDV, ref DropDV);
                        AddCommandStruct(CommandType.DropDefaultValueTMP, DropDV);
                    }
                    else if (Field.IsIdentity)
                    {
                        AddCommandStruct(CommandType.CreateField, "ALTER TABLE `" + TableName + "` ADD `" + FieldName + "` " + Field.FieldType.ToString() + FieldSize + IsNullable + IsIdentity);
                    }
                    else
                    {
                        AddCommandStruct(CommandType.CreateField, "ALTER TABLE `" + TableName + "` ADD `" + FieldName + "` " + Field.FieldType.ToString() + FieldSize + DefaultValue + IsNullable + IsIdentity);
                    }

                    break;
            }
        }

        public string GetDefaultValueByType(string FieldType)
        {

            switch (FieldType.ToUpper())
            {
                case "BIGINT":
                case "DECIMAL":
                case "FLOAT":
                case "INT":
                case "MONEY":
                case "NCHAR":
                case "NUMERIC":
                case "REAL":
                case "SMALLINT":
                case "SMALLMONEY":
                case "TINYINT":
                case "UNIQUEIDENTIFIER":
                    return "0";

                case "BIT":
                    return "0";

                case "DATETIME":
                case "SMALLDATETIME":
                case "TIMESTAMP":
                case "DATE":
                    return "'01-01-1900'";

                case "XML":
                case "NTEXT":
                case "TEXT":
                case "SQL_VARIANT":
                case "CHAR":
                case "NVARCHAR":
                case "VARCHAR":
                default:
                    return "''";

                case "IMAGE":
                    return "''";
                case "BINARY":
                    return "CONVERT(BINARY(1),'')";
                case "VARBINARY":
                    return "CONVERT(VARBINARY(1),'')";

            }

        }

        /* *******************************************************************************************
        * Cria o Script de DefaultValue
        *********************************************************************************************/

        public void ScriptDefaultValue(string Table, string Field, string DefaultValue, ref string CommandCreate, ref string CommandDrop)
        {

            switch ((int)ServerType)
            {
                case 0: //SQL
                    CommandCreate = ("ALTER TABLE [" + Table + "] ADD CONSTRAINT "
                                                + "[DF__" + Table.Replace(" ", "_") + "_" + Field.Replace(" ", "_") + "_DefaultValueByAdapter] DEFAULT "
                                                + DefaultValue
                                                + " FOR [" + Field + "]");

                    CommandDrop = ("DECLARE @ConstraintName VARCHAR(150) \r\n" +
                                    "set @ConstraintName = \r\n" +
                                    "(SELECT name FROM (SELECT  t.id , c.cdefault from sysobjects t \r\n" +
                                    "INNER JOIN syscolumns c on t.id = c.id \r\n" +
                                    "where t.name = '" + Table + "' AND c.name = '" + Field + "')  as tc \r\n" +
                                    "INNER JOIN \r\n" +
                                    "(SELECT name, id, parent_obj FROM sysobjects WHERE xtype = 'D') as DF \r\n" +
                                    "on DF.parent_obj = tc.id and DF.id = tc.cdefault ) \r\n" +
                                    "if (@ConstraintName is not null) \r\nbegin \r\n" +
                                    "\texec ('Alter Table [" + Table + "] Drop Constraint \"' + @ConstraintName + '\"')" +
                                    "\r\nend");
                    break;

                case 1://MYSQL
                    CommandCreate = String.Format("ALTER TABLE `{0}` ALTER `{1}` SET DEFAULT {2}", Table, Field, DefaultValue);
                    CommandDrop = String.Format("ALTER TABLE `{0}` ALTER `{1}` DROP DEFAULT", Table, Field);
                    break;
            }
        }

        private int GetCommandValuePS(CommandType Type)
        {
            switch (Type)
            {
                case CommandType.CreateFunctions:
                    return 1;
                case CommandType.DropFKO:
                    return 2;
                case CommandType.DropFK:
                    return 3;
                case CommandType.DropPKO:
                    return 4;
                case CommandType.DropPK:
                    return 5;
                case CommandType.DropTriggerO:
                    return 6;
                case CommandType.DropTrigger:
                    return 7;
                case CommandType.DropDefaultValueO:
                    return 8;
                case CommandType.DropDefaultValue:
                    return 9;
                case CommandType.DropFieldO:
                    return 10;
                case CommandType.DropField:
                    return 11;
                case CommandType.DropProcedure:
                    return 12;
                case CommandType.DropTable:
                    return 13;
                case CommandType.RenameTable:
                    return 14;
                case CommandType.CreateTable:
                    return 15;
                case CommandType.RenameTemp:
                    return 16;
                case CommandType.RenameField:
                    return 17;
                case CommandType.CreateField:
                    return 18;
                case CommandType.AlterField:
                    return 19;
                case CommandType.UpdateField:
                    return 20;
                case CommandType.RenameIdentityField:
                    return 21;
                case CommandType.DropDefaultValueTMP:
                    return 22;
                case CommandType.DropUpdateField:
                    return 23;
                case CommandType.CreateDefaultValue:
                    return 24;
                case CommandType.CreateIndex:
                    return 25;
                case CommandType.CreatePK:
                    return 26;
                case CommandType.CreateFK:
                    return 27;
                case CommandType.CreateTrigger:
                    return 28;
                case CommandType.CreateProcedure:
                    return 29;
                case CommandType.DropFunctions:
                    return 30;

            }
            return -1;
        }

        /* *******************************************************************************************
         * Add Command Struct
         *********************************************************************************************/
        public void AddCommandStruct(CommandType Type, string Command)
        {
            ProgrammabilityCommandsStruct PCS = new ProgrammabilityCommandsStruct();
            PCS.Command = Command;
            PCS.Type = Type;
            ProgrammabilityCommand.Add(PCS);
        }

        /* *******************************************************************************************
        * Muda o estado atual do campo old para exist ou não existe
        *********************************************************************************************/
        public void ChangeFieldOldLife(string TableName, string ColumnName, bool State)
        {

            FieldStruct FindAssocieted = (from f in DBStructOld.Tables
                                          where f.TableName == TableName
                                          from fi in f.Fields
                                          where fi.FieldName == ColumnName
                                          select fi).SingleOrDefault();

            if (FindAssocieted != null)
            {
                FindAssocieted.Exist = State;
            }

        }

        /* *******************************************************************************************
        * Faz o Rename dos campos 
        *********************************************************************************************/
        public void RenameFieldStruct(TableStruct TableN, TableStruct TableO)
        {

            int QR = 1; // Quantidade de Rename
            int IR = 0; // Impossible to Rename

            while (QR > 0) //Renomeia os campos
            {
                List<FieldStruct> FoundRename = (from f //Todos os campos que precisam ser renomeados.
                                                 in TableN.Fields
                                                 where f.Renamed == true
                                                 select f).ToList<FieldStruct>();
                QR = FoundRename.Count;

                if (QR > 0 && QR == IR) //Renomeia para temp pois travou o rename
                {
                    ScriptRenameFieldTMP(TableN.TableName, FoundRename[0].FieldName, FoundRename[0].OldFieldName, FoundRename[0]);
                    ChangeFieldOldLife(TableN.TableName, FoundRename[0].OldFieldName, false);
                    FoundRename[0].OldFieldName = FoundRename[0].OldFieldName + "TMP";
                    FoundRename[0].Renamed = false;
                    continue;
                }
                IR = 0;
                foreach (FieldStruct Field in FoundRename)
                {

                    FieldStruct FoundAssocied = (from f in TableN.Fields //Vamos verificar se o ( NewField ) esta associado com algum old  
                                                 where f.OldFieldName == Field.FieldName
                                                 select f).FirstOrDefault();

                    FieldStruct NewExistInOldTable = (from f in TableO.Fields //Vamos verificar se o (NewField) Exist na tabela antiga
                                                      where f.FieldName == Field.FieldName
                                                      select f).FirstOrDefault();

                    FieldStruct OldField = (from f in TableO.Fields //Vamos verificar se o (OldField) Exist na tabela antiga
                                            where f.FieldName == Field.OldFieldName
                                            select f).FirstOrDefault();

                    if (FoundAssocied != null)
                    {
                        if (NewExistInOldTable != null && NewExistInOldTable.Exist)
                        {

                            IR++;
                            continue;
                        }
                        else
                        {
                            Field.Renamed = true;
                        }
                    }
                    else
                    {

                        if (NewExistInOldTable != null && NewExistInOldTable.Exist)
                        {
                            Field.Drop = true;
                            NewExistInOldTable.Exist = false;
                            Field.Exist = false;
                        }

                        List<FieldStruct> FindAssocieted = (from f   //Vamos Procurar outro campo associado ao old
                                                            in TableN.Fields
                                                            where f.OldFieldName == Field.OldFieldName
                                                            && f.FieldName != Field.FieldName
                                                            select f).ToList<FieldStruct>();

                        if (FindAssocieted.Count > 0)
                        {
                            string ColumnUpdate = "";

                            foreach (FieldStruct item in FindAssocieted) // Vamos procurar uma associação perfeita
                            {
                                ColumnUpdate = item.FieldName;

                                if (item.FieldName == item.OldFieldName) // Encontrei o par Perfeito
                                {
                                    ColumnUpdate = item.FieldName;
                                    Field.Create = true;
                                    Field.Change = false;
                                    Field.Renamed = false;
                                    ScriptUpdateField(TableN.TableName, Field.FieldName, ColumnUpdate);
                                    break;
                                }
                            }

                            if (NewExistInOldTable != null)
                            {
                                if (NewExistInOldTable.Exist)
                                {
                                    Field.Renamed = true;
                                    OldField.Exist = false;
                                }
                                else
                                {
                                    Field.Create = true;
                                    Field.Change = false;
                                }
                            }
                            if (!OldField.Exist)
                            {
                                Field.Create = true;
                                Field.Renamed = false;
                                ScriptUpdateField(TableN.TableName, Field.FieldName, ColumnUpdate);
                            }

                        }
                        else
                        {
                            Field.Renamed = true;
                            OldField.Exist = false;
                        }
                    }

                    if (Field.Renamed)
                    {
                        ScriptRenameField(TableN.TableName, Field.FieldName, Field.OldFieldName, Field);
                        Field.Renamed = false;
                        Field.Exist = true;
                        OldField.Exist = false;
                    }
                }
            }
        }

        private bool PrepareAlterField(string TableName, FieldStruct Field, TableStruct _OldTable)
        {
            bool DoAlterField = true;
            RecreatePkIndexFK(TableName, Field.FieldName, true, true); //se teve alteração nos valores ( Length, precision, type ) devemos recriar as chaves

            FieldStruct FindAssocieted = (from f in _OldTable.Fields
                                          where f.FieldName == Field.OldFieldName
                                          select f).SingleOrDefault();

            if (FindAssocieted != null)
            {
                string Ret = SqlConverter.GetConverterType(FindAssocieted.FieldType, Field.FieldType);

                if (Ret == "N") // Conversão de dados não permitida, o campo deve ser removido e recriado.
                {
                    FindAssocieted.Drop = true;
                    ScriptCreateField(TableName, Field);
                    DoAlterField = false;
                    WarningList.Add(String.Format("Não é possível alterar o tipo do campo {0}  da tabela {1} esse campo será removido e recriado e todos os dados serão perdidos!!!", Field, TableName));
                }
            }

            if (Field.DefaultValue.ToString() != "") // Vou alterar o campo, tenho que matar o default pois na nova estrutura existe default.
            {
                if (!Field.DefaultValueCommand.RunDrop && FindAssocieted != null && FindAssocieted.DefaultValue.Length > 0)// Vamos deletar o Default caso exista.
                {
                    Field.DefaultValueCommand.RunDrop = true;
                }
                Field.DefaultValueCommand.RunCreate = true;
            }

            return DoAlterField;
        }

        /* *******************************************************************************************
        * Monta todos os Scripts de adaptação do banco.
        *********************************************************************************************/
        public string CreateScript(DataBaseStruct DB_Old, DataBaseStruct DB_New)
        {
            TableStruct _OldTable = new TableStruct();

            foreach (TableStruct TableStListN in DB_New.Tables)
            {
                if (TableStListN.Create)
                {
                    CreateTable(TableStListN);
                }
                else if (TableStListN.Change)                                            //Foi encontrada modificação na tabela
                {
                    string TableReference = TableStListN.TableName;

                    if (TableStListN.TableName != TableStListN.OldTableName && TableStListN.OldTableName != null) //Renomeia a tabela
                    {
                        ScriptRenameTable(TableStListN.TableName, TableStListN.OldTableName); // Cria o Script de rename da tabela
                        TableReference = TableStListN.OldTableName;
                    }

                    foreach (TableStruct OldTable in DB_Old.Tables)                     //Tabela foi renomeada, vamos marcar a old para não deletar
                    {
                        if (OldTable.TableName == TableReference)
                        {
                            OldTable.Drop = false;
                            _OldTable = OldTable;                                       //Carrega a tabela correspondente.
                            break;
                        }
                    }

                    RenameFieldStruct(TableStListN, _OldTable);                         //Verifica Rename de campos

                    foreach (FieldStruct FieldStListN in TableStListN.Fields)           //Vamos verificar a crição dos camos
                    {
                        if (FieldStListN.Create)                                        //O campo não existe, vai criar o campo
                        {
                            ScriptCreateField(TableStListN.TableName, FieldStListN);
                            FieldStListN.Create = true;
                        }
                    }

                    foreach (FieldStruct FieldStListN in TableStListN.Fields)           //Vamos verificar a crição dos camos
                    {
                        if (FieldStListN.Drop)                                        //O campo não existe, vai criar o campo
                        {
                            ScriptDropField(TableStListN.TableName, FieldStListN, false);
                            RecreatePkIndexFK(TableStListN.TableName, FieldStListN.FieldName, true, true);
                            FieldStListN.DefaultValueCommand.RunDrop = true;
                            FieldStListN.DefaultValueCommand.RunCreate = true;
                            FieldStListN.Drop = false;
                        }
                    }

                    foreach (FieldStruct FieldStListN in TableStListN.Fields)           //Verifica Modificações dos campos
                    {
                        if (FieldStListN.Change)                                     //Foi encontrada modificação no campos, Cria o script
                        {
                            //1 = IsNullable alterado
                            //2 = Size alterado
                            //4 = type alterado
                            //8 = Numeric Precision Alterado
                            //16 = Criar Constraint de Default Value
                            //32 = Dropa constraint de Default Value
                            //64 = Identity alterado
                            //128 = SeedValue Alterado do campo Identity
                            //254 = increment_value Alterado do campo Identity
                            //512 = is_not_for_replication Alterado do campo Identity								

                            //Adapta não foi ajustado para alterar um campo para identity 

                            if (FieldStListN.ChangesFound >= 512)
                            {
                                FieldStListN.ChangesFound -= 512;
                            }
                            if (FieldStListN.ChangesFound >= 254)
                            {
                                FieldStListN.ChangesFound -= 254;
                            }
                            if (FieldStListN.ChangesFound >= 128)
                            {
                                FieldStListN.ChangesFound -= 128;

                            }
                            if (FieldStListN.ChangesFound >= 64)
                            {
                                FieldStListN.ChangesFound -= 64;
                                ScriptChangeIdentityField(TableStListN, FieldStListN);
                            }

                            if (FieldStListN.ChangesFound >= 32)
                            {
                                FieldStListN.ChangesFound -= 32;
                                FieldStListN.DefaultValueCommand.RunDrop = true;
                            }

                            if (FieldStListN.ChangesFound >= 16)
                            {
                                FieldStListN.ChangesFound -= 16;
                                FieldStListN.DefaultValueCommand.RunCreate = true;
                            }

                            if (FieldStListN.ChangesFound < 16 && FieldStListN.ChangesFound > 0) // Todas as alterações abaixo de 16 implicam em alteração de estrutura do campo.
                            {
                                FieldStListN.ChangesFound = 0;
                                ScriptAlterField(TableStListN.TableName, FieldStListN, _OldTable);
                            }
                        }
                    }
                }

            }

            foreach (TableStruct TableStListN in DB_New.Tables)
            {

                foreach (FieldStruct FieldStN in TableStListN.Fields)                      //Cria os DefaultValues
                {
                    if (FieldStN.DefaultValueCommand.RunCreate)
                    {
                        AddCommandStruct(CommandType.CreateDefaultValue, FieldStN.DefaultValueCommand.CreateCommand);
                    }

                    if (FieldStN.DefaultValueCommand.RunDrop)
                    {
                        string TableName = TableStListN.OldTableName != null ? TableStListN.OldTableName : TableStListN.TableName;
                        FieldStruct FindAssocieted = (from f in DB_Old.Tables //Vamos Procurar a query de drop DefaultValue
                                                      where f.TableName == TableName
                                                      from OldField in f.Fields
                                                      where OldField.FieldName == FieldStN.FieldName
                                                      select OldField).FirstOrDefault();
                        if (FindAssocieted != null)
                        {
                            AddCommandStruct(CommandType.DropDefaultValue, FindAssocieted.DefaultValueCommand.DropCommand);
                        }
                    }
                }

                foreach (FKStruct FKStN in TableStListN.ForeignKey)                      //Recria as FKs Alteradas
                {
                    if (FKStN.RunCreate == true)
                    {
                        AddCommandStruct(CommandType.CreateFK, FKStN.CreateCommand);
                    }
                    if (FKStN.RunDrop == true)
                    {

                        CreateFunctions = true;

                        string TableName = TableStListN.OldTableName != null ? TableStListN.OldTableName : TableStListN.TableName;
                        FKStruct FKAssocieted = (from f in DB_Old.Tables //Vamos Procurar a query de drop FKs
                                                 where f.TableName == TableName
                                                 from OldFK in f.ForeignKey
                                                 where OldFK.Name == FKStN.Name
                                                 select OldFK).FirstOrDefault();

                        if (FKAssocieted != null)
                        {
                            AddCommandStruct(CommandType.DropFK, FKAssocieted.DropCommand);
                        }
                    }

                }

                foreach (IndexStruct PKStN in TableStListN.PrimaryKey)                      //Recria as PKs Alteradas
                {
                    if (PKStN.RunCreate == true)
                    {
                        AddCommandStruct(CommandType.CreatePK, PKStN.CreateCommand);
                    }
                    if (PKStN.RunDrop == true)
                    {
                        string TableName = TableStListN.OldTableName != null ? TableStListN.OldTableName : TableStListN.TableName;
                        IndexStruct IndexAssocieted = (from f in DB_Old.Tables //Vamos Procurar a query de drop PKs
                                                       where f.TableName == TableName
                                                       from OldFK in f.PrimaryKey
                                                       where OldFK.Name == PKStN.Name
                                                       select OldFK).FirstOrDefault();

                        if (IndexAssocieted != null)
                        {
                            AddCommandStruct(CommandType.DropPK, IndexAssocieted.DropCommand);
                        }
                    }

                }

                foreach (IndexStruct IndexStN in TableStListN.Index)                        //Recria os indices alterados
                {
                    if (IndexStN.RunCreate)
                    {
                        AddCommandStruct(CommandType.CreatePK, IndexStN.CreateCommand);
                    }
                    if (IndexStN.RunDrop)
                    {
                        string TableName = TableStListN.OldTableName != null ? TableStListN.OldTableName : TableStListN.TableName;
                        IndexStruct IndexAssocieted = (from f in DB_Old.Tables //Vamos Procurar a query de drop Index
                                                       where f.TableName == TableName
                                                       from OldFK in f.Index
                                                       where OldFK.Name == IndexStN.Name
                                                       select OldFK).FirstOrDefault();

                        if (IndexAssocieted != null)
                        {
                            AddCommandStruct(CommandType.DropPK, IndexAssocieted.DropCommand);
                        }
                    }
                }

                foreach (TriggerStruct Trigger in TableStListN.Triggers)                    //Cria as triggers
                {
                    if (Trigger.RunCreate)
                    {
                        AddCommandStruct(CommandType.CreateTrigger, Trigger.CreateCommand);
                    }

                    if (Trigger.RunDrop)
                    {
                        string TableName = TableStListN.OldTableName != null ? TableStListN.OldTableName : TableStListN.TableName;
                        TriggerStruct TriggerAssocieted = (from f in DB_Old.Tables //Vamos Procurar a query de drop Trigger
                                                           where f.TableName == TableName
                                                           from OldTrigger in f.Triggers
                                                           where OldTrigger.Name == Trigger.Name
                                                           select OldTrigger).FirstOrDefault();

                        if (TriggerAssocieted != null)
                        {
                            AddCommandStruct(CommandType.DropTrigger, TriggerAssocieted.DropCommand);
                        }
                    }
                }
            }

            //foreach (ProcedureStruct NewProc in DBStructNew.Procedures)                     //Vamos Criar as procedures pendentes
            //{
            //    if (NewProc.RunCreate)
            //    {
            //        AddCommandStruct(CommandType.CreateProcedure, NewProc.CreateCommand);
            //    }

            //    if (NewProc.RunDrop)
            //    {
            //        ProcedureStruct ProcAssocieted = (from f in DB_Old.Procedures //Vamos Procurar a query de drop Index
            //                                          where f.Name == NewProc.Name
            //                                          select f).SingleOrDefault();

            //        if (ProcAssocieted != null)
            //        {
            //            AddCommandStruct(CommandType.DropProcedure, ProcAssocieted.DropCommand);
            //        }
            //    }
            //}

            foreach (ProcedureStruct NewProc in DBStructNew.Procedures.Where(P => P.RunDrop))                     //Vamos deletar as procedures pendentes
            {
                if (NewProc.RunDrop)
                {
                    ProcedureStruct ProcAssocieted = (from f in DB_Old.Procedures //Vamos Procurar a query de drop Index
                                                      where f.Name == NewProc.Name
                                                      select f).SingleOrDefault();

                    if (ProcAssocieted != null)
                    {
                        AddCommandStruct(CommandType.DropProcedure, ProcAssocieted.DropCommand);
                    }
                }
            }

            List<ProcedureStruct> OrderedProcedure = new List<ProcedureStruct>();
            List<ProcedureStruct> ALLProcedureCreating = DBStructNew.Procedures.Where(P => P.RunCreate).ToList();
            int check = 0;
            while (OrderedProcedure.Count() != ALLProcedureCreating.Count()) // enquando não tiver ordenado todas as procedures vamos continuar
            {
                foreach (ProcedureStruct NewProc in ALLProcedureCreating.Where(P => !OrderedProcedure.Contains(P)))                   //Vamos Criar as procedures pendentes
                {
                    if (NewProc.DependenceView.Count == 0)
                    {
                        OrderedProcedure.Add(NewProc);
                    }
                    else
                    {
                        bool Wait = false;
                        foreach (string Dependecing in NewProc.DependenceView)
                        {
                            if (ALLProcedureCreating.Where(Apc => Apc.Name == Dependecing).Count() > 0 && OrderedProcedure.Where(O => O.Name == Dependecing).Count() == 0)
                            {
                                Wait = true;
                            }
                        }
                        if (!Wait)
                        {
                            check = 0;
                            OrderedProcedure.Add(NewProc);
                        }

                    }
                }

                if (check > ALLProcedureCreating.Count())//impedir travamento da comparação em caso de recursividade 
                {
                    break;
                }
                check++;
            }

            foreach (var NewProc in OrderedProcedure) // Já esta ordenado, vamos criar as views
            {
                AddCommandStruct(CommandType.CreateProcedure, NewProc.CreateCommand);
            }

            foreach (TableStruct TableStListO in DB_Old.Tables)                             //Vamos Verificar na struct antiga o que restou
            {
                if (TableStListO.Drop == true)                                              // Vamos Deletar as tabelas que não foram associadas
                {
                    ScriptDropTable(TableStListO.TableName);
                    AddDiff(TableStListO.TableName, "Tabela Removida");
                }
                else
                {

                    foreach (FKStruct FKStO in TableStListO.ForeignKey)                  //Remove as FKs que não existem mais.
                    {
                        if (FKStO.RunDrop == true)
                        {
                            AddCommandStruct(CommandType.DropFKO, FKStO.DropCommand);
                            AddDiff(TableStListO.TableName, "ForeignKey deletada: " + FKStO.Name);
                        }
                    }

                    foreach (IndexStruct PKStO in TableStListO.PrimaryKey)                  //Remove as PKs que não existem mais.
                    {
                        if (PKStO.RunDrop == true)
                        {
                            AddCommandStruct(CommandType.DropPKO, PKStO.DropCommand);
                            AddDiff(TableStListO.TableName, "PrimaryKey deletada: " + PKStO.Name);
                        }
                    }

                    foreach (IndexStruct IndexStO in TableStListO.Index)                    //Remove os indices que não existem mais.
                    {
                        if (IndexStO.RunDrop == true)
                        {
                            AddCommandStruct(CommandType.DropPK, IndexStO.DropCommand);
                            AddDiff(TableStListO.TableName, "Index deletado: " + IndexStO.Name);
                        }
                    }
                    foreach (FieldStruct FieldStListO in TableStListO.Fields)               //Remove os campos que não existem mais
                    {
                        if (FieldStListO.Drop == true)
                        {
                            AddDiff(TableStListO.TableName, "Campo deletado: " + FieldStListO.FieldName);
                            if (FieldStListO.DefaultValue.ToString() != "")                 // se esse campo tiver um defaultvalue devemos remover antes de apagar o campo
                            {
                                AddCommandStruct(CommandType.DropDefaultValueO, FieldStListO.DefaultValueCommand.DropCommand);// Remove as Constraints de DefaultValue
                                AddDiff(TableStListO.TableName, "Trigger de DefaultValue Deletada, campo: " + FieldStListO.FieldName);
                            }
                            ScriptDropField(TableStListO.TableName, FieldStListO, true);
                        }
                    }

                }
                foreach (TriggerStruct Trigger in TableStListO.Triggers)                        //Vamos Dropar as procedures que sobraram 
                {

                    if (Trigger.RunDrop)
                    {
                        AddCommandStruct(CommandType.DropTriggerO, Trigger.DropCommand);
                        AddDiff(TableStListO.TableName, "Trigger deletada: " + Trigger.Name);
                    }
                }
            }

            foreach (ProcedureStruct OldProc in DBStructOld.Procedures)                         //Vamos Dropar as procedures que sobraram 
            {

                if (OldProc.RunDrop)
                {
                    AddCommandStruct(CommandType.DropProcedure, OldProc.DropCommand);
                    AddDiff("Procedure", "Procedure deletada: " + OldProc.Name);
                }
            }

            //Vamos Criar as funções de deleção de FK para MySQL
            if (CreateFunctions && ServerType == Util.DatabaseType.MYSQL)
            {
                AddCommandStruct(CommandType.CreateFunctions, _FunctionDropFKCreate);
                AddCommandStruct(CommandType.DropFunctions, _FunctionDropFKDrop);
            }

            return GetScriptAdapter();
        }

        bool _HasDiff;
        public bool HasDiff
        {
            get
            {
                return _HasDiff;
            }
            set
            {
                _HasDiff = value;
            }
        }

        /// <summary>   Comparação de DataBaseStruct
        /// Compara duas estruturas de tabelas e seta as tabelas e campos que tiveram mudanças
        /// </summary>
        /// <param name="DB_Old"> Recebe o DataBaseStruct do banco antigo</param>
        /// <param name="DB_New">Recebe o DataBaseStruct do banco novo</param>
        /// <returns> Retorno o bool informando qual foi o resultado da comparação, em caso de true os DataBaseStruct são diferentes.
        /// </returns>

        public bool CompareDbStruct(DataBaseStruct DB_Old, DataBaseStruct DB_New)
        {
            HasDiff = false;

            CommandProgrammability.Clear(); //Vamos carregar todos os comandos de criação de chaves,procedures, triggers
            LoadProcedure(DB_New.DatabaseName, DB_Old.DatabaseName);
            LoadTrigger();
            LoadKeys();

            if (DB_Old.Tables.Count != DB_New.Tables.Count) { HasDiff = true; }
            if (DB_Old.Functions.Count != DB_New.Functions.Count) { HasDiff = true; }
            if (DB_Old.Procedures.Count != DB_New.Procedures.Count) { HasDiff = true; }

            foreach (TableStruct TableStNew in DB_New.Tables)
            {
                TableStNew.Drop = false;

                TableStruct TableSTOld = (from t in DB_Old.Tables
                                          where t.TableName == TableStNew.TableName.ToString()
                                          select t).SingleOrDefault();

                if (TableSTOld == null)                                                       //Tabela não existe vamos criar
                {
                    TableStNew.Create = true;
                    HasDiff = true;
                }
                else
                {
                    TableSTOld.Drop = false;                                                   //se achou a tabela correspondente então não precisamos deletar.

                    if (TableSTOld.Fields.Count != TableStNew.Fields.Count) { HasDiff = true; }
                    if (TableSTOld.Triggers.Count != TableStNew.Triggers.Count) { HasDiff = true; }
                    if (TableSTOld.ForeignKey.Count != TableStNew.ForeignKey.Count) { HasDiff = true; }
                    if (TableSTOld.PrimaryKey.Count != TableStNew.PrimaryKey.Count) { HasDiff = true; }
                    if (TableSTOld.Index.Count != TableStNew.Index.Count) { HasDiff = true; }

                    if (CompareFieldStruct(TableSTOld, TableStNew))                            //Vamos comparar os campos.
                    {
                        HasDiff = true;

                        List<FieldStruct> FieldStNew = (from t in TableStNew.Fields
                                                        where t.Create == true
                                                        select t).ToList<FieldStruct>();

                        if (FieldStNew.Count > 0)                                               //Vamos verificar se existe rename de campos para essa tabela e pedir ação do usuário
                        {
                            List<FieldStruct> OldFieldDroped = (from t in TableSTOld.Fields
                                                                where t.Drop == true
                                                                select t).ToList<FieldStruct>();

                            if (OldFieldDroped.Count > 0)
                            {
                                AdaptingTable = TableStNew;
                                if (!CheckingAdapter)
                                {
                                    OnTableChangedEventArgs Args = new OnTableChangedEventArgs();
                                    Args.Table = AdaptingTable;
                                    Args.OldTable = new TableStruct();
                                    Args.OldTable.Fields = TableSTOld.Fields;
                                    RaiseOnTableChangedEvent(Args);

                                    while (!ShouldRunNextStep && !IsCanceling)                  // aguarda solicitação para continuar ou cancelar
                                    {
                                        Thread.Sleep(1);		
                                    }
                                }
                                ShouldRunNextStep = false;

                                if (!IsCanceling && !CheckingAdapter)
                                {
                                    foreach (FieldStruct Field in AdaptingTable.Fields)     //vamos carregar os novos valores no OldFieldName
                                    {
                                        OnFieldTableChangedEventArgs Argsf = new OnFieldTableChangedEventArgs();
                                        Argsf.Field = Field;
                                        RaiseOnFieldTableChangedEvent(Argsf);
                                    }
                                }
                                CompareFieldStruct(TableSTOld, TableStNew);                 //Achou rename de campos então vamos comparar todos os campos novamente.
                            }
                        }
                    }
                }
            }

            List<TableStruct> NewTableCreated = (from t in DB_New.Tables
                                                 where t.Create == true
                                                 select t).ToList<TableStruct>();

            foreach (TableStruct NewTable in NewTableCreated)                             //Inicia a verificação para descobrir se tem tabela renomeada
            {
                List<TableStruct> OldTableDroped = (from t in DB_Old.Tables
                                                    where t.Drop == true
                                                    select t).ToList<TableStruct>();

                if (OldTableDroped.Count > 0)                                            //Existe tabelas deletadas e incluidas vamos solicitar associação para o usuário.
                {
                    AdaptingTable = NewTable;

                    if (!CheckingAdapter)
                    {
                        OnTableCreatedOrRenamedEventArgs Args = new OnTableCreatedOrRenamedEventArgs();
                        Args.Table = AdaptingTable;
                        Args.AvaibleTables = OldTableDroped;
                        RaiseOnTableCreatedOrRenamedEvent(Args);

                        while (!ShouldRunNextStep && !IsCanceling)                            // aguarda solicitação para continuar ou cancelar
                        {
                            Thread.Sleep(1);
                        }
                    }
                    if (IsCanceling)
                    {
                        return false;
                    }

                    ShouldRunNextStep = false;

                    if (!IsCanceling)
                    {
                        if (OldTable != null && OldTable != "")                                            // tem rename de tabela (velha para nova, vamos solicitar dados de cada campo)
                        {
                            foreach (FieldStruct Field in AdaptingTable.Fields)
                            {
                                OnFieldTableChangedEventArgs FieldArgs = new OnFieldTableChangedEventArgs();
                                FieldArgs.Field = Field;
                                RaiseOnFieldTableChangedEvent(FieldArgs);
                            }

                            AdaptingTable.OldTableName = OldTable;                      // Guarda o nome  antigo da tabela         
                            AdaptingTable.Create = false;
                            AdaptingTable.Drop = false;
                            AdaptingTable.Change = true;

                            TableStruct TableSTOld = (from t in DB_Old.Tables           //já que encontrou a tabela vamos comparar os campos
                                                      where t.TableName == AdaptingTable.OldTableName.ToString()
                                                      select t).SingleOrDefault();

                            if (TableSTOld != null)
                            {
                                TableSTOld.Drop = false;

                                if (CompareFieldStruct(TableSTOld, AdaptingTable))
                                {
                                    HasDiff = true;
                                }
                            }
                        }
                    }
                }
            }
            //Caso necessite inverter a ordem para CompareFks() | ComparePks()  será necessário modificar 
            //a validação da FK onde procuramos campos referenciados  que fazem parte de um indice que sofrerá drop
            if (ComparePks() |CompareFks() | CompareTriggers() | CompareProcedures() | CompareIndex())
            {
                HasDiff = true;
            }

            return HasDiff;
        }

        /* *******************************************************************************************
        * Compara duas estruturas de Tabelas e seta as tabelas e campos que tiveram mudanças
        *********************************************************************************************/

        public bool CompareFieldStruct(TableStruct TableStructOld, TableStruct TableStructNew)
        {
            bool Erro = false;
            foreach (FieldStruct FieldStructNew in TableStructNew.Fields)
            {
                FieldStructNew.Drop = false;                                        //Novo campo nunca será dropado
                FieldStructNew.Change = false;

                string RelatedField = FieldStructNew.OldFieldName;                  //Os nomes serão igual,a não ser que o usuário tenha alterado

                if (FieldStructNew.OldFieldName != "")
                {
                    if (FieldStructNew.FieldName != FieldStructNew.OldFieldName)    //Verifica se o nome do campo foi alterado
                    {
                        FieldStructNew.Renamed = true;
                        FieldStructNew.Create = false;
                        Erro = true;
                        AddDiff(TableStructNew.TableName, "Campo Renomeado: " + FieldStructNew.OldFieldName + " Para: " + FieldStructNew.FieldName);
                    }
                }
                else
                {
                    FieldStructNew.Create = true;
                    Erro = true;
                }

                FieldStruct FieldSTOld = (from f in TableStructOld.Fields
                                          where f.FieldName.ToString() == RelatedField
                                          select f).SingleOrDefault();

                if (FieldSTOld != null)                                               //Campo não existe vamos criar
                {
                    if (!FieldStructNew.Create)                                      //é um rename ou é o mesmo campo então não vamos deletar o campo antigo
                    {
                        FieldSTOld.Drop = false;
                    }

                    FieldStructNew.ChangesFound = 0;
                    if (!FieldStructNew.IsNullable.Equals(FieldSTOld.IsNullable)) { FieldStructNew.ChangesFound += 1; FieldStructNew.Change = true; }
                    if (!FieldStructNew.FieldSize.Equals(FieldSTOld.FieldSize)) { FieldStructNew.ChangesFound += 2; FieldStructNew.Change = true; }
                    if (!FieldStructNew.FieldType.Equals(FieldSTOld.FieldType)) { FieldStructNew.ChangesFound += 4; FieldStructNew.Change = true; }
                    if (!FieldStructNew.NumericPrecision.Equals(FieldSTOld.NumericPrecision) || !FieldStructNew.NumericScale.Equals(FieldSTOld.NumericScale)) { FieldStructNew.ChangesFound += 8; FieldStructNew.Change = true; }
                    if (!FieldStructNew.DefaultValue.Equals(FieldSTOld.DefaultValue))
                    {
                        FieldStructNew.Change = true;
                        if (FieldStructNew.DefaultValue.ToString() != "") { FieldStructNew.ChangesFound += 16; }
                        if (FieldSTOld.DefaultValue != "") { FieldStructNew.ChangesFound += 32; }
                    }
                    if (!FieldStructNew.IsIdentity.Equals(FieldSTOld.IsIdentity)) { FieldStructNew.ChangesFound += 64; FieldStructNew.Change = true; }
                    if (!FieldStructNew.SeedValue.Equals(FieldSTOld.SeedValue)) { FieldStructNew.ChangesFound += 128; FieldStructNew.Change = true; }
                    if (!FieldStructNew.IncrementValue.Equals(FieldSTOld.IncrementValue)) { FieldStructNew.ChangesFound += 256; FieldStructNew.Change = true; }
                    if (!FieldStructNew.IsNotForReplication.Equals(FieldSTOld.IsNotForReplication)) { FieldStructNew.ChangesFound += 512; FieldStructNew.Change = true; }

                    if (FieldStructNew.Change)                                    //Se tiver encontrado modificações no campo seta erro e TableST.Change = true.
                    {
                        TableStructNew.Change = true;                             //Considera que a tabela tem alteração
                        Erro = true;
                        AddDiff(TableStructNew.TableName, "Campo Alterado: " + FieldStructNew.FieldName);
                    }
                }
                else
                {                                                                     //Campo Criado
                    FieldStructNew.Create = true;
                    Erro = true;
                }
            }
            if (Erro)
            {
                TableStructNew.Change = true;
            }
            return Erro;
        }

        /********************************************************************************************
        *  Seta a Recria de FKs e índices
        *********************************************************************************************/

        private void RecreateFK(string TableName, Dictionary<string, bool> ColumnName, bool Create, bool Drop)
        {

            List<FKStruct> Fk = (from t in DBStructNew.Tables
                                 from fk in t.ForeignKey
                                 where fk.ReferencedTable == TableName
                                 from c in fk.ColReferencedColReferencing.Keys
                                 where ColumnName.ContainsKey(c)
                                 select fk).ToList<FKStruct>();
            if (Fk.Count > 0)
            {
                foreach (FKStruct FKitem in Fk)
                {
                    FKitem.RunCreate = Create;
                    FKitem.RunDrop = Drop;

                }
            }
        }

        /********************************************************************************************
        *  Seta a Recria de PKs e índices
        *********************************************************************************************/

        private void RecreatePkIndexFK(string TableName, string ColumnName, bool Create, bool Drop)
        {

            List<IndexStruct> Index = (from t in DBStructNew.Tables
                                       where t.TableName == TableName
                                       from pk in t.Index
                                       where pk.ColumnName.Keys.Contains(ColumnName)
                                       select pk).ToList<IndexStruct>();

            if (Index.Count > 0)
            {
                foreach (IndexStruct indexitem in Index)
                {
                    indexitem.RunCreate = Create;
                    indexitem.RunDrop = Drop;
                    RecreateFK(TableName, indexitem.ColumnName, Create, Drop);
                }
            }

            IndexStruct Pk = (from t in DBStructNew.Tables
                              where t.TableName == TableName
                              from pk in t.PrimaryKey
                              where pk.ColumnName.Keys.Contains(ColumnName)
                              select pk).SingleOrDefault();

            if (Pk != null)
            {
                Pk.RunCreate = Create;
                Pk.RunDrop = Drop;
                RecreateFK(TableName, Pk.ColumnName, Create, Drop);
            }
        }

        /* *******************************************************************************************
        * Executa uma determinada query no SQL retornando um DataTable com o resultado da Query.
        *********************************************************************************************/
        public DataTable QueryExecute(string _ConnectionString, string Query)
        {
            bool erro = false;
            return QueryExecute(_ConnectionString, Query, ref erro);
        }

        public DataTable QueryExecute(string _ConnectionString, string Query, ref bool Erro)
        {

            DataTable DTResult = new DataTable();

            switch (ServerType)
            {
                case Util.DatabaseType.SQL:
                    SqlConnection SQLconnection = new SqlConnection();

                    try
                    {
                        SQLconnection.ConnectionString = _ConnectionString;
                        SQLconnection.Open();
                        SqlDataAdapter SQLDataAdapter = new SqlDataAdapter(Query, SQLconnection);
                        SQLDataAdapter.Fill(DTResult);
                        SQLDataAdapter.Dispose();
                    }
                    catch (Exception e)
                    {
                        if (SQLconnection != null)
                        {
                            _ErroList.Add(e.Message.ToString());
                            Erro = true;
                            SQLconnection.Close();
                            SQLconnection.Dispose();
                        }
                    }

                    finally
                    {
                        SQLconnection.Close(); //Close de Connection
                        SQLconnection.Dispose();
                    }

                    break;
                case Util.DatabaseType.ORACLE:
                    break;
                default:
                    break;
            }

            return DTResult;
        }
    }
}
