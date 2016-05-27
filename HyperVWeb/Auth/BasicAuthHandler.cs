using HyperVWeb.Properties;
using NLog;
using System;
using System.Net.Http;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Threading;
using System.Threading.Tasks;

namespace HyperVWeb.Auth
{
	internal class BasicAuthHandler : AuthHandler
	{
		public BasicAuthHandler()
		{
		}

		[WinAuth]
		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			IIdentity primaryIdentity = request.GetSecurityMessageProperty().ServiceSecurityContext.PrimaryIdentity;
			string name = primaryIdentity.Name;
			bool isAuthenticated = primaryIdentity.IsAuthenticated;
			if (Settings.Default.EnableAuthLogging)
			{
				this.logger.Info((isAuthenticated ? "Authentificated user: {0}" : "Not authentificated user: {0}"), name);
			}
			if (isAuthenticated)
			{
				return base.SendAsync(request, cancellationToken);
			}
			return base.UnAuth(request, cancellationToken);
		}
	}
}