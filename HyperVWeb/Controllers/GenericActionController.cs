using HyperVWeb;
using HyperVWeb.Models;
using System;
using System.Web.Http;
using WebAPI.OutputCache;

namespace HyperVWeb.Controllers
{
	public class GenericActionController : ApiController
	{
		internal virtual string ScriptPattern
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public GenericActionController()
		{
		}

		[CacheOutput(ClientTimeSpan=0, ServerTimeSpan=0, MustRevalidate=true)]
		public string Post(VMInfo Id)
		{
			string str = string.Format(this.ScriptPattern, Id.HyperVHost, Id.Id);
			return PSHelper.RunScript(str);
		}
	}
}