using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using PROJETO;
using COMPONENTS;
using COMPONENTS.Data;
using COMPONENTS.Security;
using COMPONENTS.Configuration;
using System.IO;
using System.Web;
using System.Web.UI;
using PROJETO.DataProviders;
using PROJETO.DataPages;

namespace PROJETO.DataProviders
{
	public partial class _22326MAKOTO_TB_LOGIN_GROUPItem : GeneralDataProviderItem
	{
		private string DataBaseName;
				
		private DataAccessObject _Dao;
		public DataAccessObject Dao
		{
			get 
			{ 
				if (_Dao == null) _Dao = Settings.GetDataAccessObject(((Databases)HttpContext.Current.Application["Databases"])[DataBaseName]);
				return _Dao;
			}
		}

		public _22326MAKOTO_TB_LOGIN_GROUPItem(string DataBaseName) : this(DataBaseName, true)
		{
		}

		public _22326MAKOTO_TB_LOGIN_GROUPItem(string DataBaseName, params string[] FieldNames) : this(DataBaseName, false, FieldNames)
		{
		}
		
		/// <summary>
		/// Construtor da Página
		/// </summary>
		private _22326MAKOTO_TB_LOGIN_GROUPItem(string DataBaseName, bool AllFields, params string[] FieldNames)
		{
			this.DataBaseName = DataBaseName;	
			Fields = CreateItemFields(AllFields, FieldNames);
		}
		
		public static Dictionary<string, FieldBase> CreateItemFields(bool AllFields, params string[] FieldNames)
		{
			Dictionary<string, FieldBase> NewFields = new Dictionary<string, FieldBase>();
			if (AllFields || Contains(FieldNames, "LOGIN_GROUP_NAME")) NewFields.Add("LOGIN_GROUP_NAME", new TextField("LOGIN_GROUP_NAME", "", null, true));
			if (AllFields || Contains(FieldNames, "LOGIN_GROUP_IS_ADMIN")) NewFields.Add("LOGIN_GROUP_IS_ADMIN", new BooleanField("LOGIN_GROUP_IS_ADMIN", "", null, true));
			
			if (!AllFields)
			{
				Dictionary<string, FieldBase> NewFieldsOrder = new Dictionary<string, FieldBase>();
				foreach (string Field in FieldNames)
				{
					NewFieldsOrder.Add(Field, NewFields[Field]);
				}
				NewFields = NewFieldsOrder; 
			}
			
			return NewFields;
		}
		
		/// <summary>
		/// Valida se todos os campos foram preenchidos corretamente
		/// </summary>
		/// <param name="provider">Provider que vai ser usado para inserir o registro na tabela</param>
		public override void Validate(GeneralDataProvider provider)
		{
		}
	}
	
	/// <summary>
	/// Classe de provider usada para acessar a tabela de produtos
	/// </summary>
	public class _22326MAKOTO_TB_LOGIN_GROUPDataProvider : GeneralDataProvider
	{
		public FieldBase this[string ColumnName]
		{
			get
			{
				return Item[ColumnName];
			}
		}

		public override Dictionary<string, FieldBase> CreateItemFields()
		{
			return _22326MAKOTO_TB_LOGIN_GROUPItem.CreateItemFields(true); 
		}
	
		public _22326MAKOTO_TB_LOGIN_GROUPDataProvider(IGeneralDataProvider BasePage, string TableName, string DataBaseName, string IndexName, string Name) : base(BasePage, TableName, DataBaseName, IndexName, Name, "")
		{
		}

		public override void CreateUniqueParameter()
		{
			Parameters.Clear();
			switch (IndexName)
			{
				case "LOGIN_GROUP_PK":
					CreateParameter("LOGIN_GROUP_NAME");
					break;
			}
		}
				
		public override void CreateParameters()
		{
			Parameters.Clear();
			switch (IndexName)
			{
				case "LOGIN_GROUP_PK":
					CreateParameter("LOGIN_GROUP_NAME");
					break;
			}
			base.CreateParameters();
		}

		public override string ProviderFilterExpression()
		{
			return this.GetFilterExpression( _22326MAKOTO_TB_LOGIN_GROUPItem.CreateItemFields(false, GetUniqueKeyFields()));
		}

		public override string[] GetUniqueKeyFields()
		{
			return new string[] { "LOGIN_GROUP_NAME" };
		}			
	}
}
