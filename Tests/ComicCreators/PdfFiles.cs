using Xunit;
using System;
using System.IO;
using ComicConverter;

namespace Test.ComicCreators
{
	public class PdfFiles
	{
		[Fact]
		public void CreatePdf()
		{
			var images = Directory.GetFiles(Samples.IMAGESDIR);

			ComicBuilder.CreatePdf(images, "pdftest");

			Assert.True(File.Exists("pdftest.pdf"));

			File.Delete("pdftest.pdf");
		}

		[Fact]
<<<<<<< HEAD
<<<<<<< HEAD
=======
		public void ImagesNotFound()
		{
			string[] images = {"Fake1.jpeg", "fake2.jpg", "fake3.jpg"};

			Assert.Throws<FileNotFoundException>(() => ComicBuilder.CreatePdf(images, "pdftest"));
		}

		[Fact]
>>>>>>> b8e9672 (Pdf creator tests)
=======
>>>>>>> 01585f3 (Imagesnotfound test deleted)
		public void FilesAreNotImages()
		{
			var images = Directory.GetFiles(Samples.IMAGESDIR);

			Assert.Throws<FormatException>(() => ComicBuilder.CreatePdf(images, "pdftest"));
		}
	}
}