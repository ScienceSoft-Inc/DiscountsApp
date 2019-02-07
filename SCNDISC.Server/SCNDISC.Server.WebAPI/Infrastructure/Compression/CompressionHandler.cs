using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SCNDISC.Server.WebAPI.Infrastructure.Compression
{
	public class CompressionHandler : DelegatingHandler
	{
		/// <summary>
		/// Gets a collection of <see cref="ICompressor"/> that are registered.
		/// </summary>
		/// <value>
		/// The registered compressors.
		/// </value>
		private readonly Collection<ICompressor> _compressors = new Collection<ICompressor> {new GZipCompressor(), new DeflateCompressor()};

		/// <summary>
		/// Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.
		/// </summary>
		/// <param name="request">The HTTP request message to send to the server.</param>
		/// <param name="cancellationToken">A cancellation token to cancel operation.</param>
		/// <returns>
		/// Returns <see cref="T:System.Threading.Tasks.Task`1" />. The task object representing the asynchronous operation.
		/// </returns>
		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
		{
			if (request.Content.Headers.ContentEncoding != null
			    && request.Content.Headers.ContentEncoding.Any())
			{
				//// request content is compressed, decompress it.
				var encoding = request.Content.Headers.ContentEncoding.First();
				var compressor = this._compressors.FirstOrDefault(c => c.EncodingType.Equals(encoding, StringComparison.InvariantCultureIgnoreCase));
				if (compressor != null)
				{
					request.Content = new DecompressedHttpContent(request.Content, compressor);
				}
			}

			if (request.Headers.AcceptEncoding != null && request.Headers.AcceptEncoding.Any())
			{
				//// response needs to be compressed
				var response = await base.SendAsync(request, cancellationToken);

				if (response.Content == null)
				{   //// no content to be encoded.
					return response;
				}

				var encoding = request.Headers.AcceptEncoding.First();
				var compressor = _compressors.FirstOrDefault(c => c.EncodingType.Equals(encoding.Value, StringComparison.InvariantCultureIgnoreCase));
				if (compressor != null)
				{
					response.Content = new CompressedHttpContent(response.Content, compressor);
				}

				return response;
			}

			return await base.SendAsync(request, cancellationToken);
		}
	}
}