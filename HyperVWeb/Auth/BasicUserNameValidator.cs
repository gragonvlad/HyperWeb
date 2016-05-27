using HyperVWeb.Properties;
using System;
using System.Collections;
using System.IdentityModel.Selectors;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.ServiceModel.Web;

namespace HyperVWeb.Auth
{
	internal class BasicUserNameValidator : UserNamePasswordValidator
	{
		public BasicUserNameValidator()
		{
		}

		public override void Validate(string userName, string password)
		{
			if (userName == null || password == null)
			{
				throw new ArgumentNullException();
			}
			string str = string.Concat(userName, ":");
			string str1 = Settings.Default.BasicAuthUserCredentials.Cast<string>().FirstOrDefault<string>((string t) => t.StartsWith(str));
			if (string.IsNullOrWhiteSpace(str1) || !string.Equals(str1.Substring(str.Length), password, StringComparison.InvariantCulture))
			{
				WebFaultException webFaultException = new WebFaultException(HttpStatusCode.Unauthorized);
				webFaultException.Data.Add("HttpStatusCode", webFaultException.StatusCode);
				throw webFaultException;
			}
		}
	}
}