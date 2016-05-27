using NLog;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HyperVWeb.Auth
{
	internal class AuthHandler : DelegatingHandler
	{
		protected readonly Logger logger = LogManager.GetCurrentClassLogger();

		public AuthHandler()
		{
		}

		protected Task<HttpResponseMessage> UnAuth(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			return Task<HttpResponseMessage>.Factory.StartNew(() => {
				HttpResponseMessage stringContent = request.CreateResponse();
				stringContent.Content = new StringContent(string.Format("You are not authorized to use this service. Your windows login name should be in config file ({0})", string.Concat(Process.GetCurrentProcess().MainModule.FileName, ".config")), Encoding.UTF8);
				return stringContent;
			});
		}
	}
}