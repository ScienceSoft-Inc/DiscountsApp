using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

namespace SCNDISC.Server.Core.Infrastructure.Http.Results
{
	public class FileResult : ActionResult
	{
		private readonly byte[] _buffer;

		public FileResult(byte[] buffer)
		{
			_buffer = buffer ?? new byte[0];
		}

		public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
		{
			var response = new HttpResponseMessage(HttpStatusCode.OK)
			{
				Content = new ByteArrayContent(_buffer)
			};

			response.Content.Headers.ContentType =
				new MediaTypeHeaderValue("application/octet-stream");

			return Task.FromResult(response);
		}
	}
}