using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace SCNDISC.Server.Infrastructure.Imaging
{
	public class ImageConverter : IImageConverter
	{
		private static readonly EncoderParameters EncoderParameters = new EncoderParameters(1)
		{
			Param =
			{
				[0] = new EncoderParameter(Encoder.Quality, 1L)
			}
		};

		public byte[] Convert(byte[] image, ImageOptions options)
		{
			if (image == null || image.Length == 0)
			{
				return image;
			}

			using (var stream = new MemoryStream(image))
			{
				using (var fromImage = Image.FromStream(stream))
				{
					var imageHeight = fromImage.Height < options.Height ? fromImage.Height : options.Height;
					var imageWidth = fromImage.Width < options.Width ? fromImage.Width : options.Width;

					if (imageWidth == fromImage.Width && imageHeight == fromImage.Height)
					{
						return image;
					}

					var resizedImage = ResizeImage(imageHeight, imageWidth, fromImage);

					using (var cropedImage = CropImage(resizedImage, imageWidth, imageHeight))
					{
						using (var output = new MemoryStream())
						{
							cropedImage.Save(output, GetEncoder(options.ImageType), EncoderParameters);
							return output.ToArray();
						}
					}
				}
			}
		}

		private static Bitmap ResizeImage(int imageHeight, int imageWidth, Image fromImage)
		{
			float ratio = (float) imageHeight / imageWidth;
			float originalRatio = (float) fromImage.Height / fromImage.Width;

			float scale = ratio > originalRatio
				? (float) imageHeight / fromImage.Height
				: (float) imageWidth / fromImage.Width;

			var width = fromImage.Width * scale;
			if (width < imageWidth)
			{
				width = imageWidth;
			}

			var height = fromImage.Height * scale;
			if (height < imageHeight)
			{
				height = imageHeight;
			}

			var resizedImage = ResizeImage(fromImage, (int) width, (int) height);
			return resizedImage;
		}

		private static Bitmap CropImage(Bitmap resizedImage, int imageWidth, int imageHeight)
		{
			var point = new Point();
			if (resizedImage.Width > resizedImage.Height)
			{
				point.X = Math.Abs(resizedImage.Width - imageWidth) / 2;
				point.Y = 0;
			}
			else
			{
				point.X = 0;
				point.Y = Math.Abs(resizedImage.Height - imageHeight) / 2;
			}

			var size = new Size(imageWidth, imageHeight);
			var rectangle = new Rectangle(point, size);
			var cutedImage = CropImage(resizedImage, rectangle);
			return cutedImage;
		}

		private static Bitmap CropImage(Bitmap source, Rectangle section)
		{
			return source.Clone(section, source.PixelFormat);
		}

		private static ImageCodecInfo GetEncoder(ImageType imageType)
		{
			var format = ToFormat(imageType);
			var codecs = ImageCodecInfo.GetImageDecoders();
			foreach (var codec in codecs)
			{
				if (codec.FormatID == format.Guid)
				{
					return codec;
				}
			}

			return null;
		}

		private static ImageFormat ToFormat(ImageType imageType)
		{
			switch (imageType)
			{
				case ImageType.Png:
				{
					return ImageFormat.Png;
				}
			}

			throw new NotImplementedException();
		}

		public static Bitmap ResizeImage(Image image, int width, int height)
		{
			var destRect = new Rectangle(0, 0, width, height);
			var destImage = new Bitmap(width, height);

			destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

			using (var graphics = Graphics.FromImage(destImage))
			{
				graphics.CompositingMode = CompositingMode.SourceCopy;
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				using (var wrapMode = new ImageAttributes())
				{
					wrapMode.SetWrapMode(WrapMode.TileFlipXY);
					graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
				}
			}

			return destImage;
		}
	}
}