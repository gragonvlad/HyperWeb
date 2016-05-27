using HyperVWeb.Properties;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Threading;
using System.Threading.Tasks;

namespace HyperVWeb.Auth
{
	internal class WindowsAuthHandler : AuthHandler
	{
		public WindowsAuthHandler()
		{
		}

		[WinAuth]
		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			string name = request.GetSecurityMessageProperty().ServiceSecurityContext.PrimaryIdentity.Name;
			IEnumerable<string> strs = Settings.Default.AllowedWindowsUsers.Cast<string>();
			bool flag = strs.Any<string>((string t) => string.Equals(name, t, StringComparison.InvariantCultureIgnoreCase));
			if (Settings.Default.EnableAuthLogging)
			{
				this.logger.Info((flag ? "Authentificated user: {0}" : "Not authentificated user: {0}"), name);
			}
			if (flag)
			{
				return base.SendAsync(request, cancellationToken);
			}
			return base.UnAuth(request, cancellationToken);
		}
	}
}