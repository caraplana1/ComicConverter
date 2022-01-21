using System;
using System.IO;
using ComicConverter;
using Xunit;

namespace Test.ImageCollector
{
	public class PdfFiles
	{
		[Fact]
		public void  ExtractImagesPdf()
		{
			ImageExtractors.ExtractPDf(Samples.PDFPATH, "PdfExtractedImages");

			var files = Directory.GetFiles("PdfExtractedImages");

			Assert.Equal(49, files.Length);

			Directory.Delete("PdfExtractedImages", true);
		}

		[Fact]
		public void FileIsNotPdf()
		{
			Assert.Throws<FormatException>(() => ImageExtractors.ExtractPDf(Samples.CBRPATH));
		}

		[Fact]
		public void FileNotFound()
		{
			Assert.Throws<FileNotFoundException>(() => ImageExtractors.ExtractPDf("FakeFile.pdf"));
		}

		[Fact]
		public void EmptyAttrubuteDirectory()
		{
			ImageExtractors.ExtractPDf(Samples.PDFPATH, "PdfExtracted");

			var files = Directory.GetFiles("PdfExtracted");

			Assert.Equal(49, files.Length);

			Directory.Delete("PdfExtracted", true);
		}

		[Fact]
		public void EmptyStringDirectory()
		{
			Assert.Throws<FormatException>(() => ImageExtractors.ExtractPDf( Samples.PDFPATH, ""));
		}
	}
}