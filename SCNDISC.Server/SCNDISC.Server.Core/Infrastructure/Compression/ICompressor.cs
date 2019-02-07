using System.IO;

namespace SCNDISC.Server.Core.Infrastructure.Compression
{
	public interface ICompressor
	{
		string EncodingType { get; }
		Stream CreateCompressionStream(Stream output);
		Stream CreateDecompressionStream(Stream input);
	}
}