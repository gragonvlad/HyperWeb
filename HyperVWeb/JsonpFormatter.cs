using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;

namespace HyperVWeb
{
	public class JsonpFormatter : JsonMediaTypeFormatter
	{
		private string JsonpCallbackFunction;

		public string JsonpParameterName
		{
			get;
			set;
		}

		public JsonpFormatter()
		{
			base.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
			base.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/javascript"));
			this.JsonpParameterName = "callback";
		}

		public override bool CanWriteType(Type type)
		{
			return true;
		}

		private string GetJsonCallbackFunction(HttpRequestMessage request)
		{
			if (request.Method != HttpMethod.Get)
			{
				return null;
			}
			NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(request.RequestUri.Query);
			string item = nameValueCollection[this.JsonpParameterName];
			if (string.IsNullOrEmpty(item))
			{
				return null;
			}
			return item;
		}

		public override MediaTypeFormatter GetPerRequestFormatterInstance(Type type, HttpRequestMessage request, MediaTypeHeaderValue mediaType)
		{
			JsonpFormatter jsonpFormatter = new JsonpFormatter()
			{
				JsonpCallbackFunction = this.GetJsonCallbackFunction(request)
			};
			JsonpFormatter jsonpFormatter1 = jsonpFormatter;
			jsonpFormatter1.SerializerSettings.Converters.Add(new StringEnumConverter());
			jsonpFormatter1.SerializerSettings.Formatting = Formatting.Indented;
			return jsonpFormatter1;
		}

		public override Task WriteToStreamAsync(Type type, object value, Stream stream, HttpContent content, TransportContext transportContext)
		{
			Task task;
			if (string.IsNullOrEmpty(this.JsonpCallbackFunction))
			{
				return base.WriteToStreamAsync(type, value, stream, content, transportContext);
			}
			StreamWriter streamWriter = null;
			try
			{
				streamWriter = new StreamWriter(stream);
				streamWriter.Write(string.Concat(this.JsonpCallbackFunction, "("));
				streamWriter.Flush();
				return base.WriteToStreamAsync(type, value, stream, content, transportContext).ContinueWith((Task innerTask) => {
					if (innerTask.Status == TaskStatus.RanToCompletion)
					{
						streamWriter.Write(")");
						streamWriter.Flush();
					}
				}, TaskContinuationOptions.ExecuteSynchronously).ContinueWith<Task>((Task innerTask) => {
					streamWriter.Dispose();
					return innerTask;
				}, TaskContinuationOptions.ExecuteSynchronously).Unwrap();
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				LogManager.GetCurrentClassLogger().ErrorException("", exception);
				try
				{
					if (streamWriter != null)
					{
						streamWriter.Dispose();
					}
				}
				catch
				{
				}
				TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>();
				taskCompletionSource.SetException(exception);
				task = taskCompletionSource.Task;
			}
			return task;
		}
	}
}