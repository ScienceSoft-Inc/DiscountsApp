namespace SCNDISC.Server.Infrastructure.Imaging
{
    public class ImageOptions
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public ImageType ImageType { get; set; }

        public static ImageOptions Image => new ImageOptions {Height = 288, Width = 512, ImageType = ImageType.Png};

        public static ImageOptions Icon => new ImageOptions {Width = 128, Height = 128, ImageType = ImageType.Png};
    }
}