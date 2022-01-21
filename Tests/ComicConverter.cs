using Xunit;
using System;
using System.IO;
using ComicConverter;

namespace Test
{
	public class ComicConverter
	{
		[Fact]
		public void ConvertCbr2Cbz()
		{
			Comic comic = new(Samples.CBRPATH);

			comic.Convert("Test", ComicFormat.CBZ);

			Assert.True(File.Exists("Test.cbz"));

			File.Delete("Test.cbz");
		}

		[Fact]
		public void ConvertCbz2Cbt()
		{
			Comic comic = new(Samples.CBZPATH);

			comic.Convert("Test", ComicFormat.CBT);

			Assert.True(File.Exists("Test.cbt"));

			File.Delete("Test.cbt");
		}

		[Fact]
		public void ConvertCbr2Pdf()
		{
			Comic comic = new(Samples.CBRPATH);

			comic.Convert("Pdf", ComicFormat.PDF);

			Assert.True(File.Exists("Pdf.pdf"));

			File.Delete("Pdf.pdf");
		}

		[Fact]
		public void ConvertPdf2Cbz()
		{
			Comic comic = new (Samples.PDFPATH);

			comic.Convert("pdf2cbz", ComicFormat.CBZ);

			Assert.True(File.Exists("pdf2cbz.cbz"));

			File.Delete("pdf2cbz.cbz");
		}

		[Fact]
		public void OutInvalidFormat()
		{
			Comic comic = new(Samples.CBZPATH);

			Assert.Throws<FormatException>(() => comic.Convert("Test", ComicFormat.CBR));
		}

		[Fact]
		public void InInvalidFormat()
		{
			Comic comic;

			Assert.Throws<FormatException>(() => comic = new(Samples.TESTPATH));
		}
	}
}