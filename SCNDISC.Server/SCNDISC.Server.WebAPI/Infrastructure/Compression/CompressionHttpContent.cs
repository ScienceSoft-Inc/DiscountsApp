using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace SCNDISC.Server.WebAPI.Infrastructure.Compression
{
	public abstract class CompressionHttpContent : HttpContent
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CompressionHttpContent"/> class.
		/// </summary>
		/// <param name="content">The original HttpContent object.</param>
		/// <param name="compressor">The compressor.</param>
		protected CompressionHttpContent(HttpContent content, ICompressor compressor)
		{
			OriginalContent = content;
			Compressor = compressor;

			SetContentHeaders();
		}

		/// <summary>
		/// Gets the <see cref="ICompressor"/>.
		/// </summary>
		/// <value>
		/// The compressor.
		/// </value>
		protected ICompressor Compressor { get; }

		/// <summary>
		/// Gets the original <see cref="HttpContent"/>.
		/// </summary>
		/// <value>
		/// The original <see cref="HttpContent"/>.
		/// </value>
		protected HttpContent OriginalContent { get; }

		/// <summary>
		/// Determines whether the HTTP content has a valid length in bytes.
		/// </summary>
		/// <param name="length">The length in bytes of the HHTP content.</param>
		/// <returns>
		/// Returns <see cref="T:System.Boolean" />.true if <paramref name="length" /> is a valid length; otherwise, false.
		/// </returns>
		protected override bool TryComputeLength(out long length)
		{
			length = -1;
			return false;
		}

		/// <summary>
		/// The set content headers.
		/// </summary>
		private void SetContentHeaders()
		{
			//// copy headers from original content
			foreach (var header in OriginalContent.Headers)
			{
				Headers.TryAddWithoutValidation(header.Key, header.Value);
			}

			//// add the content encoding header
			Headers.ContentEncoding.Add(Compressor.EncodingType);
		}
	}

	public class DecompressedHttpContent : CompressionHttpContent
	{
		/// <summary>
		    /// Initializes a new instance of the <see cref="DecompressedHttpContent"/> class.
		    /// </summary>
		    /// <param name="content">The original HttpContent object.</param>
		    /// <param name="compressor">The compressor.</param>
		public DecompressedHttpContent(HttpContent content, ICompressor compressor) : base(content, compressor)
		{
		}

		/// <summary>
		    /// Serialize the HTTP content to a stream as an asynchronous operation.
		    /// </summary>
		    /// <param name="stream">The target stream.</param>
		    /// <param name="context">Information about the transport (channel binding token, for example). This parameter may be null.</param>
		    /// <returns>
		    /// Returns <see cref="T:System.Threading.Tasks.Task" />.The task object representing the asynchronous operation.
		    /// </returns>
		protected override Task SerializeToStreamAsync(System.IO.Stream stream, System.Net.TransportContext context)
		{
			Stream compressionStream = this.Compressor.CreateDecompressionStream(this.OriginalContent.ReadAsStreamAsync().Result);

			return compressionStream.CopyToAsync(stream).ContinueWith(task =>
			{
				if (compressionStream != null)
				{
					compressionStream.Dispose();
				}
			});
		}
	}
}