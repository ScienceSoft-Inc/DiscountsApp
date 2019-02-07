namespace SCNDISC.Server.Infrastructure.Imaging
{
	public interface IImageConverter
	{
		byte[] Convert(byte[] image, ImageOptions options);
	}
}