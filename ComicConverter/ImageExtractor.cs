using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using GroupDocs.Parser;
using GroupDocs.Parser.Exceptions;
using GroupDocs.Parser.Data;

namespace ComicConverter
{
	public static class ImageExtractor
	{
		public static void ExtractPdfImages(string filePath, string outputDir = ".")
		{
			if (!File.Exists(filePath))
				throw new FileNotFoundException();

			if (filePath.Substring(filePath.Length - 3) != "pdf")
				throw new FormatException("The file is not pdf file");

			if (String.IsNullOrEmpty(outputDir))
                throw new System.FormatException("The directoty cannot be null or empty");

			Directory.CreateDirectory(outputDir);

			using Parser parser = new (filePath);

			var fileImages = parser.GetImages();
			int counter = 0;
			
			if (fileImages == null)
				return;


			foreach (var image in fileImages)
			{
				Image.FromStream(image.GetImageStream()).Save($"{outputDir}/{counter}.png", ImageFormat.Png);
				counter++;
			}
		}

		public static void ExtractEPUBImages(string filePath, string outputDir = ".") => throw new NotImplementedException();

	}
}