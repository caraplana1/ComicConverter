using Xunit;
using System;
using System.Linq;
using System.Collections.Generic;
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
            images = images.ToList().Append(Samples.FAKEIMAGE).ToArray();

            Assert.ThrowsAny<Exception>(() => ComicBuilder.CreatePdf(images, "pdftest"));
        }
    }
}