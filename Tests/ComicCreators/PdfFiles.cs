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
        public void FilesAreNotImages()
		{
			var images = Directory.GetFiles(Samples.IMAGESDIR);
			images = [.. images, Samples.FAKEIMAGE];

            Assert.ThrowsAny<Exception>(() => ComicBuilder.CreatePdf(images, "pdftest"));
        }
    }
}