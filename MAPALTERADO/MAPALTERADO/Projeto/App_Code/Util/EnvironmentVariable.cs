using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.IO;
using System.Text;
using System.Web.SessionState;
using System.Resources;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using COMPONENTS;
using COMPONENTS.Data;
using COMPONENTS.Configuration;
using System.Web.Security;
using System.Configuration;
using System.Web.UI;
using System.Data;
using PROJETO.DataProviders;
using System.Collections.Generic;

namespace PROJETO
{
	public static class EnvironmentVariable
    {

        public static string LoggedIdUser
        {
            get
            {
				try
				{
					return Crypt.Decripta(HttpContext.Current.Session[GMembershipProvider.Default.UserIdSessionVariable].ToString());
				}
				catch
				{
					return "";
				}
            }
        }

        public static string LoggedLoginUser
        {
            get
            {
                try
				{

					return Crypt.Decripta(HttpContext.Current.Session[GMembershipProvider.Default.UserLoginSessionVariable].ToString());
				}
				catch
				{
					return "";
				}
            }
        }

        public static string LoggedNameUser
        {
            get
            {
                try
				{
					return Crypt.Decripta(HttpContext.Current.Session[GMembershipProvider.Default.UserNameSessionVariable].ToString());
				}
				catch
				{
					return "";
				}
            }
        }

        public static string LoggedObsUser
        {
            get
            {
                try
				{
					return Crypt.Decripta(HttpContext.Current.Session[GMembershipProvider.Default.UserObsSessionVariable].ToString());
				}
				catch
				{
					return "";
				}
            }
        }

        public static string LoggedIdGroup
        {
            get
            {
                try
				{
					return Crypt.Decripta(HttpContext.Current.Session[GMembershipProvider.Default.GroupIdSessionVariable].ToString());
				}
				catch
				{
					return "";
				}
            }
        }

        public static string LoggedNameGroup
        {
            get
            {
                try
				{
					return Crypt.Decripta(HttpContext.Current.Session[GMembershipProvider.Default.GroupNameSessionVariable].ToString());
				}
				catch
				{
					return "";
				}
            }
        }
        
        public static string LoggedIsAdminGroup
        {
            get
            {
                try
				{
					return HttpContext.Current.Session[GMembershipProvider.Default.GroupIsAdminSessionVariable].ToString();
				}
				catch
				{
					return "";
				}
            }
        }
        public static string CompanyName
        {
            get
            {
                return ConfigurationManager.AppSettings["CompanyName"];
            }
        }

        public static string DeveloperName
        {
            get
            {
                return ConfigurationManager.AppSettings["DeveloperName"];
            }
        }

        public static string ProjectVersion
        {
            get
            {
                return ConfigurationManager.AppSettings["ProjectVersion"];
            }
        }

        public static string ProjectCopyright
        {
            get
            {
                return ConfigurationManager.AppSettings["ProjectCopyright"];
            }
        }

		public static DateTime ActualDate
        {
            get
            {
                return System.DateTime.Today;
            }
        }

		public static DateTime ActualDateTime
        {
            get
            {
                return System.DateTime.Now;
            }
        }

		public static int ActualDay
		{
			get
			{
				return System.DateTime.Now.Day;
			}
		}

		public static int ActualMonth
		{
			get
			{
				return System.DateTime.Now.Month;
			}
		}

		public static int ActualYear
		{
			get
			{
				return System.DateTime.Now.Year;
			}
		}

		public static string ActualTime
        {
            get
            {
                return System.DateTime.Now.ToLongTimeString();
            }
        }
		

    }
}
