using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HyperVWeb
{
	public class FileHandler : DelegatingHandler
	{
		private Logger logger = LogManager.GetCurrentClassLogger();

		public FileHandler()
		{
		}

		private Stream GenerateStreamFromString(string value)
		{
			return new MemoryStream(Encoding.Default.GetBytes(value ?? ""));
		}

		private MediaTypeHeaderValue GuessMediaTypeFromExtension(string path)
		{
			string extension = Path.GetExtension(path);
			string str = extension;
			if (extension != null)
			{
				switch (str)
				{
					case ".htm":
					case ".html":
					{
						return new MediaTypeHeaderValue("text/html");
					}
					case ".js":
					{
						return new MediaTypeHeaderValue("text/javascript");
					}
					case ".css":
					{
						return new MediaTypeHeaderValue("text/css");
					}
					case ".jpg":
					case ".jpeg":
					{
						return new MediaTypeHeaderValue("image/jpeg");
					}
					case ".png":
					{
						return new MediaTypeHeaderValue("image/png");
					}
				}
			}
			return new MediaTypeHeaderValue("text/plain");
		}

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			if (!request.RequestUri.OriginalString.Contains("/api/vm/"))
			{
				this.logger.Info(request.RequestUri.OriginalString);
			}
			if (request.RequestUri.AbsolutePath.StartsWith("/api/"))
			{
				return base.SendAsync(request, cancellationToken);
			}
			return Task<HttpResponseMessage>.Factory.StartNew(() => {
				HttpResponseMessage byteArrayContent = request.CreateResponse();
				string str = (request.RequestUri.AbsolutePath == "" || request.RequestUri.AbsolutePath == "/" ? "Default.html" : request.RequestUri.AbsolutePath.Substring(1));
				if ((new string[] { "vmlist", "main", "details", "newvm", "switch", "about" }).Any<string>((string t) => t.StartsWith(str)))
				{
					str = "default.html";
				}
				if (byteArrayContent.Content == null)
				{
					string str1 = string.Concat("WebContent/", str);
					//Dictionary<string, byte[]> zipped = ZipHelper.Zipped;
					//if (zipped.ContainsKey(str1))
					//{
					//	byteArrayContent.Content = new ByteArrayContent(zipped[str1]);
					//}
				    var file = File.OpenRead(str1);
                    var reader = new StreamReader(file);
				    byteArrayContent.Content = new ByteArrayContent(reader.CurrentEncoding.GetBytes(reader.ReadToEnd()));
                    file.Close();
				}
				if (byteArrayContent.Content != null)
				{
					byteArrayContent.Content.Headers.ContentType = this.GuessMediaTypeFromExtension(str);
				}
				return byteArrayContent;
			});
		}
	}
}