using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using COMPONENTS;

namespace PROJETO
{
    public enum Operators
    {
        Equals,
        Like,
        Diferent,
        BiggerThan,
        LowerThan
    }

    [Serializable]
    public class InnerJoin
    {
        private Table _Table;
        public Table Table 
        { 
            get
            {
                return _Table;
            }
            set
            {
                _Table = value;
            }
        }
        private FieldItem _InnerJoinField;
        public FieldItem InnerJoinField
        { 
            get
            {
                return _InnerJoinField;
            }
            set
            {
                _InnerJoinField = value;
            }
        }
        private Operators _Operator;
        public Operators Operator
        { 
            get
            {
                return _Operator;
            }
            set
            {
                _Operator = value;
            }
        }
        private FieldItem _BaseField;
        public FieldItem BaseField
        { 
            get
            {
                return _BaseField;
            }
            set
            {
                _BaseField = value;
            }
        }
        private string _Alias;
        public string Alias
        { 
            get
            {
                return _Alias;
            }
            set
            {
                _Alias = value;
            }
        }
        private string _JoinType;
        public string JoinType
        { 
            get
            {
                return _JoinType;
            }
            set
            {
                _JoinType = value;
            }
        }

        public InnerJoin()
        {
            Table = new Table();

        }
    }

    [Serializable]
    public class Filter
    {
        private FieldItem _FirstField;
        public FieldItem FirstField 
        {
            get
            {
                return _FirstField;
            }
            set
            {
                _FirstField = value;
            }
        }
        private Operators _Operator;
        public Operators Operator
        {
            get
            {
                return _Operator;
            }
            set
            {
                _Operator = value;
            }
        }
        private FieldItem _SeccondField;
        public FieldItem SeccondField
        {
            get
            {
                return _SeccondField;
            }
            set
            {
                _SeccondField = value;
            }
        }
        private string _LogicalOperator;
        public string LogicalOperator
        {
            get
            {
                return _LogicalOperator;
            }
            set
            {
                _LogicalOperator = value;
            }
        }

        public Filter()
        {

        }

        public string getFilterExpression()
        {
            string retVal = "";
            retVal = FirstField.ToString();
            switch (Operator)
            {
                case Operators.Equals:
                    retVal += " = ";
                    break;
                case Operators.Like:
                    retVal +=" LIKE ";
                    break;
                case Operators.Diferent:
                    retVal +=" != ";
                    break;
                case Operators.BiggerThan:
                    retVal +=" > ";
                    break;
                case Operators.LowerThan:
                    retVal +=" < ";
                    break;
                default:
                    retVal +=" = ";
                    break;
            }
            retVal += SeccondField.ToString();
            return retVal;
        }
    }

    [Serializable]
    public class FieldItem
    {
        private string _TableName;
        public string TableName
        {
            get
            {
                return _TableName;
            }
            set
            {
                _TableName = value;
            }
        }

        private string _Title;
		public string Title
		{
			get
			{
				return _Title;
			}
			set
			{
				_Title = value;
			}
		}

        private string _FieldName;
        public string FieldName
                {
            get
            {
                return _FieldName;
            }
            set
            {
                _FieldName = value;
            }
        }
        private string _CustomValue;
        public string CustomValue
        {
            get
            {
                return _CustomValue;
            }
            set
            {
                _CustomValue = value;
            }
        }
        private bool _IsNewColumn;
        public bool IsNewColumn
        {
            get
            {
                return _IsNewColumn;
            }
            set
            {
                _IsNewColumn = value;
            }
        }
        
        public string FullName
		{
			get
			{
				if (IsNewColumn)
				{
					return CustomValue;
				}
				else
				{
					return "[" + TableName + "].[" + FieldName + "]";
				}
			}
		}
		
        public FieldItem()
        {
            IsNewColumn = false;
        }
        public override string ToString()
        {
            if (CustomValue != null)
            {
                return CustomValue;
            }
            return FullName;
        }
    }

    [Serializable]
    public class NewColumn
    {
        private string _Fucntion;
        public string Fucntion
        {
            get
            {
                return _Fucntion;
            }
            set
            {
                _Fucntion = value;
            }
        }
        private FieldItem _Content;
        public FieldItem Content
        {
            get
            {
                return _Content;
            }
            set
            {
                _Content = value;
            }
        }
        private string _Title;
        public string Title
        {
            get
            {
                return _Title;
            }
            set
            {
                _Title = value;
            }
        }
        private bool _HasTitle;
        public bool HasTitle
        {
            get
            {
                return _HasTitle;
            }
            set
            {
                _HasTitle = value;
            }
        }
        public NewColumn()
        {

        }
    }

    [Serializable]
    public class OrderBy
    {
        private FieldItem _Field;
        public FieldItem Field
        {
            get 
            {
                return _Field;
            }
            set 
            {
                _Field = value;
            }
        }
        private string _Type;
        public string Type
        {
            get 
            {
                return _Type;
            }
            set 
            {
                _Type = value;
            }
        }
        public OrderBy()
        {

        }
    }

    [Serializable]
    public class ViewSettings
    {

        public List<string> SelectedTables = new List<string>();
        public List<FieldItem> SelectedFields = new List<FieldItem>();
        public List<FieldItem> GroupBy = new List<FieldItem>();
        public List<OrderBy> OrderBy = new List<OrderBy>();
        public string DataBase;
        public List<InnerJoin> InnerJoins = new List<InnerJoin>();
        public List<Filter> Filters = new List<Filter>();
        public List<NewColumn> NewColumn = new List<NewColumn>();
        public string Top;
        public string ViewName;
        public string CustomText;

        public ViewSettings()
        {

        }

        public FieldItem GetFieldByName(string FieldName)
        {
            foreach (FieldItem fi in SelectedFields)
            {
                if (fi.FullName == FieldName)
                {
                    return fi;
                }
            }
            return null;
        }

        public string GenerateSqlQuery()
        {
            if (SelectedFields.Count > 0)
            {
                StringBuilder Sb = new StringBuilder();
                BuildSelect(ref Sb);
                BuildFrom(ref Sb);
                BuildJoins(ref Sb);
                BuildWhere(ref Sb);
                BuildGroupBy(ref Sb);
                BuildOrderBy(ref Sb);
                return Sb.ToString();
            }
            else
            {
                return CustomText;
            }
        }

        private void BuildOrderBy(ref StringBuilder Sb)
        {
            if (OrderBy.Count > 0)
            {
                Sb.Append(System.Environment.NewLine);
                Sb.Append("order by ");
                int Count = 0;
                foreach (OrderBy ob in OrderBy)
                {
                    Count++;
                    if (Count > 1)
                    {
                        Sb.Append(", ");
                    }
                    Sb.Append(ob.Field.FullName + " " + ob.Type);
                }
            }
        }

        private void BuildGroupBy(ref StringBuilder Sb)
        {
            if(GroupBy.Count > 0)
            {
                Sb.Append(System.Environment.NewLine);
                Sb.Append("group by ");
                int Count = 0;
                foreach (FieldItem fi in GroupBy)
                {
                    Count++;
                    if (Count > 1)
                    {
                        Sb.Append(", ");
                    }
                    Sb.Append(fi.FullName);
                }
            }
        }

        private void BuildWhere(ref StringBuilder Sb)
        {
            if (Filters.Count > 0)
            {
                Sb.Append(System.Environment.NewLine);
                Sb.Append("where ");
                int Count = 0;
                string FilterExpression = "";
                foreach (Filter f in Filters)
                {
                    Count++;
                    if (Count == 1)
                    {
                        FilterExpression = "(" + f.getFilterExpression() +")";
                    }
                    else
                    {
                        FilterExpression = "(" + FilterExpression + " " + f.LogicalOperator + " " + f.getFilterExpression() + ")";
                    }
                }
                Sb.Append(FilterExpression);
            }
        }

        private void BuildJoins(ref StringBuilder Sb)
        {
            if (InnerJoins.Count > 0)
            {
                Sb.Append(System.Environment.NewLine);
                foreach (InnerJoin ij in InnerJoins)
                {
                    Sb.Append(ij.JoinType + " join [" + ij.Table.Name + "] " + ij.Alias);
                    Sb.Append(Environment.NewLine);
                    Sb.Append("on " + ij.InnerJoinField.ToString());
                    switch (ij.Operator)
                    {
                        case Operators.Equals:
                            Sb.Append(" = ");
                            break;
                        case Operators.Like:
                            Sb.Append(" LIKE ");
                            break;
                        case Operators.Diferent:
                            Sb.Append(" != ");
                            break;
                        case Operators.BiggerThan:
                            Sb.Append(" > ");
                            break;
                        case Operators.LowerThan:
                            Sb.Append(" < ");
                            break;
                        default:
                            Sb.Append(" = ");
                            break;
                    }
                    Sb.Append(ij.BaseField.ToString());
                }
            }
        }

        private void BuildFrom(ref StringBuilder Sb)
        {
            Sb.Append(" from ");
            int Count = 0;
            foreach (string Table in SelectedTables)
            {
                Count++;
                if (Count > 1)
                {
                    Sb.Append(", ");
                }
                Sb.Append("[" + Table + "]");
            }
        }

        private void BuildSelect(ref StringBuilder Sb)
        {
            Sb.Append("select ");
            int i = 0;
            if (Top != "" && int.TryParse(Top, out i))
            {
                Sb.Append("top " + Top + " ");
            }
            int Count = 0;
            foreach (FieldItem field in SelectedFields)
            {
                Count++;
                if (Count > 1)
                {
                    Sb.Append(", ");
                    if (((Count - 1) % 2) == 0)
                    {
                        Sb.Append(System.Environment.NewLine);
                    }
                }
                if (field.IsNewColumn)
                {
                    NewColumn nc = getNewColumnByName(field.CustomValue);
                    switch (nc.Fucntion)
                    {
                        case "Soma":
                            Sb.Append("Sum(" + nc.Content + ")");
                            break;
                        case "Media":
                            Sb.Append("Avg(" + nc.Content + ")");
                            break;
                        case "Menor":
                            Sb.Append("Min(" + nc.Content + ")");
                            break;
                        case "Maior":
                            Sb.Append("Max(" + nc.Content + ")");
                            break;
                        case "Dia":
                            Sb.Append("DAY(" + nc.Content + ")");
                            break;
                        case "Mes":
                            Sb.Append("MONTH(" + nc.Content + ")");
                            break;
                        case "Ano":
                            Sb.Append("YEAR(" + nc.Content + ")");
                            break;
                        case "Contar":
                            Sb.Append("Count(" + nc.Content + ")");
                            break;
                        case "MesAno":
                            Sb.Append("CONVERT(VARCHAR, DAY(" + nc.Content + ")) + '-'+  CONVERT(VARCHAR,MONTH(" + nc.Content + "))");
                            break;
                        default:
                            break;
                    }
                    if (nc.HasTitle)
                    {
						Sb.Append(" as " + nc.Title);
					}
                }
                else
                {
                    Sb.Append(field.FullName);
                }
            }
        }

        public Table GetTableByJoinAlias(string JoinName)
        {
            foreach (InnerJoin ij in InnerJoins)
            {
                if (JoinName == ij.Alias)
                {
                    return ij.Table;
                }
            }
            return null;
        }

        public InnerJoin getJoinByAlias(string JoinName)
        {
            foreach (InnerJoin ij in InnerJoins)
            {
                if (JoinName == ij.Alias)
                {
                    return ij;
                }
            }
            return null;
        }

        public NewColumn getNewColumnByName(string ColumnAlias)
        {
            foreach (NewColumn nc in NewColumn)
            {
                if (ColumnAlias == nc.Title)
                {
                    return nc;
                }
            }
            return null;
        }
    }
    [Serializable]
    public class DatabaseConfig
    {
        #region XmlFilesPath
        private string _xmlfilespath;
        public string XmlFilesPath
        {
            get { return _xmlfilespath; }
            set { _xmlfilespath = value; }
        }
        #endregion

        public DatabaseCollection DataBases = new DatabaseCollection();

        public DatabaseConfig()
        {
        }

        public DatabaseConfig(string DataBasesPath)
        {
            XmlFilesPath = DataBasesPath;
        }

        public Field GetFieldByName(string FieldName)
        {
            string TbNome = (FieldName.Split('.') as string[])[0];
            string CampoNome = (FieldName.Split('.') as string[])[1];

            foreach (Table Tb in DataBases[0].Tables)
            {
                if (("[" + Tb.Name + "]") == TbNome)
                {
                    foreach (Field CampoTb in Tb.Fields)
                    {
                        if (("[" + CampoTb.Name + "]") == CampoNome)
                        {
                            return CampoTb;
                        }
                    }
                }
            }

            return null;
        }

        public void LoadXmlFile()
        {
            foreach (DatabaseInfo vgDbi in ((Databases)HttpContext.Current.Application["Databases"]).DataBaseList.Values)
            {
                LoadXmlFile(vgDbi.DataBaseAlias + ".xml");
            }
        }
        
        public Dictionary<string, string> Indexes = new Dictionary<string, string>();

        private void LoadXmlFile(string XmlFileName)
        {
            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(XmlFilesPath + XmlFileName);
            XmlNodeList RootElement = XmlDoc.GetElementsByTagName("DATABASE");
            foreach (XmlNode XmlDatabase in RootElement)
            {
                Database db = new Database();
                db.Name = XmlDatabase.Attributes["NAME"].Value;
                db.Title = XmlDatabase.Attributes["TITLE"].Value;
                foreach (XmlNode Node in XmlDatabase.ChildNodes)
                {
                    switch (Node.Name.ToUpper())
                    {
                        case "TABLE":
                            db.Tables.Add(ParseTable(Node));
                            break;

                        case "RELATION":
                            db.Integrities.Add(ParseIntegrity(Node));
                            break;
                    }
                }
                DataBases.Add(db);
            }
        }

        private Integrity ParseIntegrity(XmlNode Node)
        {
            Integrity NewIntegrity = new Integrity();
            NewIntegrity.Name = ReadAttribute(Node, "NAME");
            NewIntegrity.Base = ReadAttribute(Node, "BASE");
            NewIntegrity.Foreign = ReadAttribute(Node, "FOREIGN");
            NewIntegrity.Refresh = ReadAttribute(Node, "UPDATE");
            NewIntegrity.Exclude = ReadAttribute(Node, "DELETE");
            NewIntegrity.System = ReadAttribute(Node, "SYSTEM");
            NewIntegrity.Trigger = ReadAttribute(Node, "TRIGGER");
            NewIntegrity.Type = ReadAttribute(Node, "TYPE");
            NewIntegrity.Message = ReadAttribute(Node, "MSG");

            foreach (XmlNode ChildNode in Node.ChildNodes)
            {
                switch (ChildNode.Name.ToUpper())
                {
                    case "FIELD":
                        NewIntegrity.Fields.Add(ParseIntegrityField(ChildNode));
                        break;
                }
            }
            return NewIntegrity;
        }

		private Integrity ParseIntegrityRelation(XmlNode ChildNode)
		{
			Integrity NewIntegrity = new Integrity();
			NewIntegrity.Name = ReadAttribute(ChildNode, "NAME");
			NewIntegrity.Base = ReadAttribute(ChildNode, "BASETABLE");
			NewIntegrity.Foreign = ReadAttribute(ChildNode, "FOREIGNTABLE");

			foreach (XmlNode Node in ChildNode.ChildNodes)
			{
				switch (Node.Name.ToUpper())
				{
					case "RELATIONOF":
						NewIntegrity.Fields.Add(ParseIntegrityField(Node));
						break;
				}
			}

			return NewIntegrity;
		}

        private IntegrityField ParseIntegrityField(XmlNode ChildNode)
        {
            IntegrityField NewIntegrityField = new IntegrityField();
            NewIntegrityField.Name = ReadAttribute(ChildNode, "NAME");
            NewIntegrityField.System = ReadAttribute(ChildNode, "SYSTEM");
            NewIntegrityField.Base = ReadAttribute(ChildNode, "BASEFIELD");
            NewIntegrityField.Foreign = ReadAttribute(ChildNode, "FOREIGNFIELD");
            return NewIntegrityField;
        }

        private Table ParseTable(XmlNode Node)
        {
            Table NewTable = new Table();
            NewTable.Name = ReadAttribute(Node, "NAME");
            NewTable.Title = ReadAttribute(Node, "TITLE");
            NewTable.System = ReadAttribute(Node, "SYSTEM");
            foreach (XmlNode ChildNode in Node.ChildNodes)
            {
                switch (ChildNode.Name.ToUpper())
                {
                    case "FIELD":
                        NewTable.Fields.Add(ParseTableField(ChildNode));
                        break;
                    case "INDEX":
                        NewTable.Index.Add(ParseTableIndex(ChildNode));
                        break;
					case "RELATION":
						NewTable.Integrities.Add(ParseIntegrityRelation(ChildNode));
						break;                        
                }
            }
            Indexes.Clear();
            return NewTable;
        }

        private Index ParseTableIndex(XmlNode ChildNode)
        {
            Index NewIndex = new Index();
            NewIndex.Name = ReadAttribute(ChildNode, "NAME");
            NewIndex.Title = ReadAttribute(ChildNode, "TITLE");
            NewIndex.Type = ReadAttribute(ChildNode, "TYPE");
            NewIndex.Clustered = ReadAttribute(ChildNode, "CLUSTERIZED");
            NewIndex.FillFactor = ReadAttribute(ChildNode, "FILLFACTOR");
            NewIndex.System = ReadAttribute(ChildNode, "SYSTEM");

            foreach (XmlNode Node in ChildNode.ChildNodes)
            {
                switch (Node.Name.ToUpper())
                {
                    case "FIELD":
                        NewIndex.Fields.Add(ParseIndexField(Node));
                        break;
                }
            }
            return NewIndex;
        }

        private IndexField ParseIndexField(XmlNode ChildNode)
        {
            IndexField NewIndexField = new IndexField();
            NewIndexField.Name = ReadAttribute(ChildNode, "NAME");
            NewIndexField.Title = (Indexes[NewIndexField.Name] != null) ? Indexes[NewIndexField.Name] : ReadAttribute(ChildNode, "TITLE");
            NewIndexField.System = ReadAttribute(ChildNode, "SYSTEM");
            NewIndexField.Desc = ReadAttribute(ChildNode, "DESC");
            return NewIndexField;
        }

        private Field ParseTableField(XmlNode ChildNode)
        {
            Field NewField = new Field();
            NewField.Name = ReadAttribute(ChildNode, "NAME");
            NewField.Title = ReadAttribute(ChildNode, "TITLE");
            Indexes.Add(NewField.Name, NewField.Title);
            string Type = "";
			switch (ReadAttribute(ChildNode, "TYPE"))
			{
				case "TEXT":
					Type = "Caracter";
					break;
				case "BOOLEAN":
					Type = "Logico";
					break;
				case "MEMO":
					Type = "Memo";
					break;
				case "IMAGE":
					Type = "Imagem";
					break;
				case "NUMBER":
				    Type = "Numerico";
				    break;
				case "BINARY":
				    Type = "Binario";
				    break;
				case "DATETIME":
				    Type = "Data/Hora";
				    break;
				case "DATE":
					Type = "Data";
					break;
				default:
					Type = ReadAttribute(ChildNode, "TYPE");
					break;
			}
			NewField.Type = Type;
            NewField.Size = Convert.ToInt32(ReadAttribute(ChildNode, "SIZE"));
            NewField.Masc = ReadAttribute(ChildNode, "MASK");
            NewField.Sequence = ReadAttribute(ChildNode, "SEQUENCE");
            NewField.Null = ReadAttribute(ChildNode, "NULL");
            NewField.System = ReadAttribute(ChildNode, "SYSTEM");
            return NewField;
        }

        private static string ReadAttribute(XmlNode Node, string NomeAtributo)
        {
            if (Node.Attributes[NomeAtributo] != null)
            {
                return Node.Attributes[NomeAtributo].Value;
            }
            else
            {
                return null;
            }
        }

        public Database GetDatabaseByName(string DbName)
        {
            foreach (Database db in DataBases)
            {
                if (DbName == db.Name)
                {
                    return db;
                }
            }
            return null;
        }

        public Table getTableByName(string TbName, string DbName)
        {
            Database db = GetDatabaseByName(DbName);
            foreach (Table Tb in db.Tables)
            {
                if (TbName == Tb.Name)
                {
                    return Tb;
                }
            }
            return null;
        }
    }

    #region Items

    [Serializable]
    public class Table
    {
        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }
		private string _Title;
		public string Title
		{
			get
			{
				return _Title;
			}
			set
			{
				_Title = value;
			}
		}

        private string _System;
        public string System
        {
            get
            {
                return _System;
            }
            set
            {
                _System = value;
            }
        }
        private FieldCollection _Fields;
        public FieldCollection Fields
        {
            get {
                return _Fields;
            }
            set 
            {
                _Fields = value;
            }
        }
        private IndexCollection _Index;
        public IndexCollection Index
        {
            get
            {
                return _Index;
            }
            set
            {
                _Index = value;
            }
        }
        private IntegrityCollection _Integrities;
		public IntegrityCollection Integrities
		{
			get
			{
				return _Integrities;
			}
			set
			{
				_Integrities = value;
			}
		}

        public Table()
        {
            Name = "";
            System = "";
			Title = "";
            Fields = new FieldCollection();
            Index = new IndexCollection();
            Integrities = new IntegrityCollection();
        }
    }
    [Serializable]
    public class Field
    {
        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }
        
		private string _Title;
		public string Title
		{
			get
			{
				return _Title;
			}
			set
			{
				_Title = value;
			}
		}

        private string _Type;
        public string Type
        {
            get
            {
                return _Type;
            }
            set
            {
                _Type = value;
            }
        }
        private int _Size;
        public int Size
        {
            get
            {
                return _Size;
            }
            set
            {
                _Size = value;
            }
        }
        private string _Masc;
        public string Masc
        {
            get
            {
                return _Masc;
            }
            set
            {
                _Masc = value;
            }
        }
        private string _Sequence;
        public string Sequence
        {
            get
            {
                return _Sequence;
            }
            set
            {
                _Sequence = value;
            }
        }
        private string _Null;
        public string Null
        {
            get
            {
                return _Null;
            }
            set
            {
                _Null = value;
            }
        }
        private string _System;
        public string System
        {
            get
            {
                return _System;
            }
            set
            {
                _System = value;
            }
        }

        public Field()
        {
            Name = "";
            Title = "";
            Type = "";
            Masc = "";
            Size = 0;
            Sequence = "";
            Null = "";
            System = "";
        }
    }
    [Serializable]
    public class Index
    {
        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }
        private string _Title;
		public string Title
		{
			get
			{
				return _Title;
			}
			set
			{
				_Title = value;
			}
		}
        private string _Type;
        public string Type
        {
            get
            {
                return _Type;
            }
            set
            {
                _Type = value;
            }
        }
        private string _Clustered;
        public string Clustered
        {
            get
            {
                return _Clustered;
            }
            set
            {
                _Clustered = value;
            }
        }
        private string _FillFactor;
        public string FillFactor
        {
            get
            {
                return _FillFactor;
            }
            set
            {
                _FillFactor = value;
            }
        }
        private string _System;
        public string System
        {
            get
            {
                return _System;
            }
            set
            {
                _System = value;
            }
        }
        private IndexFieldCollection _Fields;
        public IndexFieldCollection Fields
        {
            get
            {
                return _Fields;
            }
            set
            {
                _Fields = value;
            }
        }
        public Index()
        {
            Name = "";
            Title = "";
            Type = "";
            Clustered = "";
            FillFactor = "";
            System = "";
            Fields = new IndexFieldCollection();
        }
    }
    [Serializable]
    public class Integrity
    {
        public string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }
        private string _Base;
        public string Base
        {
            get
            {
                return _Base;
            }            
            set
            {
                _Base = value;
            }
        }
        private string _Foreign;
        public string Foreign
        {
            get
            {
                return _Foreign;
            }
            set
            {
                _Foreign = value;
            }
        }
        private string _Refresh;
        public string Refresh
        {
            get
            {
                return _Refresh;
            }
            set
            {
                _Refresh = value;
            }
        }
        private string _Exclude;
        public string Exclude
        {
            get
            {
                return _Exclude;
            }
            set
            {
                _Exclude = value;
            }
        }
        private string _System;
        public string System
        {
            get
            {
                return _System;
            }
            set
            {
                _System = value;
            }
        }
        private string _Trigger;
        public string Trigger
        {
            get
            {
                return _Trigger;
            }
            set
            {
                _Trigger = value;
            }
        }
        private string _Type;
        public string Type
        {
            get
            {
                return _Type;
            }
            set
            {
                _Type = value;
            }
        }
        private string _Message;
        public string Message
        {
            get
            {
                return _Message;
            }
            set
            {
                _Message = value;
            }
        }
        private IntegrityFieldCollection _Fields;
        public IntegrityFieldCollection Fields
        {
            get
            {
                return _Fields;
            }
            set
            {
                _Fields = value;
            }
        }

        public Integrity()
        {
            Fields = new IntegrityFieldCollection();
            Name = "";
            System = "";
            Foreign = "";
            Refresh = "";
            Exclude = "";
            Base = "";
            Trigger = "";
            Type = "";
            Message = "";
        }

    }
    [Serializable]
    public class IndexField
    {
        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }
        private string _Title;
		public string Title
		{
			get
			{
				return _Title;
			}
			set
			{
				_Title = value;
			}
		}
        private string _System;
        public string System
        {
            get
            {
                return _System;
            }
            set
            {
                _System = value;
            }
        }
        private string _Desc;
        public string Desc
        {
            get
            {
                return _Desc;
            }
            set
            {
                _Desc = value;
            }
        }

        public IndexField()
        {
            Name = "";
            Title = "";
            System = "";
            Desc = "";
        }
    }
    [Serializable]
    public class IntegrityField
    {
        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }
        private string _System;
        public string System
        {
            get
            {
                return _System;
            }
            set
            {
                _System = value;
            }
        }
        private string _Base;
        public string Base
        {
            get
            {
                return _Base;
            }
            set
            {
                _Base = value;
            }
        }
        private string _Foreign;
        public string Foreign
        {
            get
            {
                return _Foreign;
            }
            set
            {
                _Foreign = value;
            }
        }

        public IntegrityField()
        {
            Name = "";
            System = "";
            Base = "";
            Foreign = "";
        }
    }
    [Serializable]
    public class Database
    {
        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }
        private string _Title;
		public string Title
		{
			get
			{
				return _Title;
			}
			set
			{
				_Title = value;
			}
		}
        private TableCollection _Tables;
        public TableCollection Tables
        {
            get
            {
                return _Tables;
            }
            set
            {
                _Tables = value;
            }
        }
        private IntegrityCollection _Integrities;
        public IntegrityCollection Integrities
        {
            get
            {
                return _Integrities;
            }
            set
            {
                _Integrities = value;
            }
        }

        public Database()
        {
            Tables = new TableCollection();
            Integrities = new IntegrityCollection();
            Name = "";
            Title = "";
        }
    }

    #endregion

    #region Collection properties
    [Serializable]
    public class IndexFieldCollection : CollectionBase
    {
        public virtual void Add(IndexField NewIndexField)
        {
            this.List.Add(NewIndexField);
        }

        public virtual IndexField this[int index]
        {
            get
            {
                return (IndexField)this.List[index];
            }
        }
    }
    [Serializable]
    public class IntegrityFieldCollection : CollectionBase
    {
        public virtual void Add(IntegrityField NewIntegrityField)
        {
            this.List.Add(NewIntegrityField);
        }

        public virtual IntegrityField this[int Index]
        {
            get
            {
                return (IntegrityField)this.List[Index];
            }
        }
    }
    [Serializable]
    public class TableCollection : CollectionBase
    {
        public virtual void Add(Table NewTable)
        {
            this.List.Add(NewTable);

        }

        public virtual Table this[int Index]
        {
            get
            {
                if (Index >= 0)
                {
                    return (Table)this.List[Index];
                }
                else
                {
                    return null;
                }
            }
        }
		
		public virtual Table this[string Name]
		{
			get
			{
				return this.List.OfType<Table>().First(d => d.Name.Equals(Name, StringComparison.InvariantCultureIgnoreCase));
			}
		}
    }
    [Serializable]
    public class FieldCollection : CollectionBase
    {
        public virtual void Add(Field NewField)
        {
            this.List.Add(NewField);

        }

        public virtual Field this[int Index]
        {
            get
            {
                return (Field)this.List[Index];
            }
        }
		
		public virtual Field this[string Name]
		{
			get
			{
				return this.List.OfType<Field>().First(d => d.Name.Equals(Name, StringComparison.InvariantCultureIgnoreCase));
			}
		}
    }
    [Serializable]
    public class DatabaseCollection : CollectionBase
    {
        public virtual void Add(Database NewDatabase)
        {
            this.List.Add(NewDatabase);

        }

        public virtual Database this[int Index]
        {
            get
            {
                return (Database)this.List[Index];
            }
        }

		public virtual Database this[string Name]
		{
			get
			{
				return this.List.OfType<Database>().First(d => d.Name.Equals(Name, StringComparison.InvariantCultureIgnoreCase));
			}
		}
    }
    [Serializable]
    public class IndexCollection : CollectionBase
    {
        public virtual void Add(Index NewIndex)
        {
            this.List.Add(NewIndex);

        }

        public virtual Index this[int Index]
        {
            get
            {
                return (Index)this.List[Index];
            }
        }

    }
    [Serializable]
    public class IntegrityCollection : CollectionBase
    {
        public virtual void Add(Integrity NewInregrity)
        {
            this.List.Add(NewInregrity);
        }

        public virtual Integrity this[int Index]
        {
            get
            {
                return (Integrity)this.List[Index];
            }
        }
    }
    #endregion
}
