namespace SCNDISC.Server.Infrastructure.Imaging
{
	public class ImageOptions
	{
		public int Width { get; set; }
		public int Height { get; set; }
		public ImageType ImageType { get; set; }

		public static ImageOptions Image
		{
			get { return new ImageOptions{Height = 180, Width = 340, ImageType = ImageType.Png};}
		}

		public static ImageOptions Icon
		{
			get { return new ImageOptions{Width = 48, Height = 48, ImageType = ImageType.Png };}
		}
	}
}