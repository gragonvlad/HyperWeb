using HyperVWeb.Auth;
using HyperVWeb.Properties;
using NLog;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.ServiceModel;
using System.Web.Http;
using System.Web.Http.SelfHost;
using Newtonsoft.Json;

namespace HyperVWeb
{
	internal class WindowsService
	{
		private readonly HttpSelfHostServer server;

		private readonly Logger _logger = LogManager.GetCurrentClassLogger();

		public WindowsService()
		{

			string listeningUrl = Settings.Default.ListeningUrl;
			HttpSelfHostConfiguration httpSelfHostConfiguration = new HttpSelfHostConfiguration(listeningUrl);
			httpSelfHostConfiguration.Routes.MapHttpRoute("API Default", "api/{controller}/{id}", new { id = RouteParameter.Optional });
            httpSelfHostConfiguration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
			string upperInvariant = Settings.Default.Auth.ToUpperInvariant();
			string str = upperInvariant;
			if (upperInvariant != null)
			{
				if (str == "WINDOWS")
				{
					httpSelfHostConfiguration.ClientCredentialType = HttpClientCredentialType.Windows;
					httpSelfHostConfiguration.MessageHandlers.Add(new WindowsAuthHandler());
				}
				else if (str == "BASIC")
				{
					httpSelfHostConfiguration.ClientCredentialType = HttpClientCredentialType.Basic;
					httpSelfHostConfiguration.UserNamePasswordValidator = new BasicUserNameValidator();
					httpSelfHostConfiguration.MessageHandlers.Add(new BasicAuthHandler());
				}
				else if (str != "OFF")
				{
				}
			}
			httpSelfHostConfiguration.MessageHandlers.Add(new FileHandler());
			httpSelfHostConfiguration.MaxConcurrentRequests = 200;
			this.server = new HttpSelfHostServer(httpSelfHostConfiguration);
			this._logger.Info("Webinterface adress: {0}", listeningUrl);
		} 
        public void Start()
		{
			this.server.OpenAsync();
		}

		public void Stop()
		{
			this.server.CloseAsync();
			this.server.Dispose();
		}
	}
}