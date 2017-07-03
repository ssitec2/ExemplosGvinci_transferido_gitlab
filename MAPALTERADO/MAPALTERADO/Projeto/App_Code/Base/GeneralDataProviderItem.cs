using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Resources;
using COMPONENTS;
using COMPONENTS.Data;
using COMPONENTS.Security;
using COMPONENTS.Configuration;
using PROJETO;
using System.Collections.Generic;
using COMPONENTS;

namespace PROJETO.DataProviders
{
	public class GeneralDataProviderItem
	{

		/// <summary>
		/// Propriedade usada para saber se é um novo registro
		/// </summary>
		private bool _IsNew;
		public bool IsNew
		{
			get { return _IsNew; }
			set { _IsNew = value; }
		}

		/// <summary>
		/// Propriedade usada para saber se o registro foi deletado
		/// </summary>
		private bool _IsDeleted;

		public bool IsDeleted
		{
			get { return _IsDeleted; }
			set { _IsDeleted = value; }
		}

		private ResourceManager _RM;
		public ResourceManager RM
		{
			get
			{
				return _RM;
			}
			set
			{
				_RM = value;
			}
		}
		
		private string _CodLan;
		public string CodLan
		{
			get
			{
				return _CodLan;
			}
		set
			{
				_CodLan = value;
			}
		}
		
		private string _IntLan;
        public string IntLan
        {
            get
            {
                return _IntLan;
            }
            set
            {
                _IntLan = value;
            }
        }

		private NameValueCollection _Errors;
		public NameValueCollection Errors
		{
			get
			{
				return _Errors;
			}
			set
			{
				_Errors = value;
			}
		}

		private DataRow _RawData;
		public DataRow RawData 
		{
			get
			{
				return _RawData;
			}
			set
			{
				_RawData = value;
			}
		}

		private Dictionary<string, FieldBase> _Fields;
		public Dictionary<string, FieldBase> Fields
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

		public virtual void Validate(GeneralDataProvider provider)
		{
		}

		public virtual string GetComboText(string FieldName)
		{
			return "";
		}

		public GeneralDataProviderItem()
		{
			_RM = (ResourceManager)HttpContext.Current.Application["rm"];
			_Errors = new NameValueCollection();
			_RawData = null;
			_Fields = new Dictionary<string, FieldBase>();
		}

		/// <summary>
		/// Propriedade para pegar os valores dos campos de acordo com seus nomes
		/// </summary>
		/// <param name="fieldName">Nome do campo</param>
		/// <returns>Valor do campo</returns>
		public FieldBase this[string fieldName]
		{
			get
			{
				try
				{
					return Fields[fieldName];
				}
				catch
				{
					throw (new ArgumentOutOfRangeException());
				}
			}
			set
			{
				try
				{
					Fields[fieldName] = value;
				}
				catch
				{
					throw (new ArgumentOutOfRangeException());
				}
			}
		}

		public void SetFieldValue(FieldBase Field, object FieldValue, bool allowempty)
		{
			try
			{
				Field.AllowEmpty = allowempty;
				Field.SetValue(FieldValue);
			}
			catch (ArgumentException)
			{
			}
		}

		public void SetFieldValue(FieldBase Field, object FieldValue)
		{
			SetFieldValue(Field, FieldValue, false);
		}

		public string GetFieldAliasByFieldName(string FieldName)
		{
			foreach (KeyValuePair<string, FieldBase> f in Fields)
			{
				if (f.Value.Name == FieldName)
				{
					return f.Key;
				}
			}
			return "";
		}

		public static bool Contains(string[] FieldNames, string Field)
		{
			foreach (string f in FieldNames)
			{
				if (f.Equals(Field))
				{
					return true;
				}
			}
			return false;
		}
		
	}
}
