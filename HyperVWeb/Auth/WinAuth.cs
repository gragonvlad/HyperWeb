using System;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace HyperVWeb.Auth
{
	public class WinAuth : AuthorizeAttribute
	{
		public WinAuth()
		{
		}

		protected override bool IsAuthorized(HttpActionContext actionContext)
		{
			return base.IsAuthorized(actionContext);
		}

		public override void OnAuthorization(HttpActionContext actionContext)
		{
			base.OnAuthorization(actionContext);
		}
	}
}