using System.IO;
using System.IO.Compression;

namespace SCNDISC.Server.Core.Infrastructure.Compression
{
	public class DeflateCompressor : ICompressor
	{
		/// <summary>
		/// The deflate compression.
		/// </summary>
		private const string DeflateCompression = "deflate";

		/// <summary>
		/// Gets the type of the encoding.
		/// </summary>
		/// <value>
		/// The type of the encoding.
		/// </value>
		public string EncodingType
		{
			get
			{
				return DeflateCompression;
			}
		}

		/// <summary>
		/// Creates the compression stream.
		/// </summary>
		/// <param name="output">The output.</param>
		/// <returns>
		/// A Compression Stream.
		/// </returns>
		public Stream CreateCompressionStream(Stream output)
		{
			return new DeflateStream(output, CompressionMode.Compress, true);
		}

		/// <summary>
		/// Creates the decompression stream.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <returns>
		/// A Decompression Stream.
		/// </returns>
		public Stream CreateDecompressionStream(Stream input)
		{
			return new DeflateStream(input, CompressionMode.Decompress, true);
		}
	}
}