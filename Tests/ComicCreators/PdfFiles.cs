using Xunit;
using System;
using System.IO;
using ComicConverter;

namespace Test.ComicCreators
{
    public class PdfFiles
    {
        private const string path = "pdftestBuild";

        [Fact]
        public void CreatePdf()
        {
            var images = Directory.GetFiles(Samples.IMAGESDIR);

            ComicBuilder.CreatePdf(images, path);

            Assert.True(File.Exists($"{path}.pdf"));

            File.Delete($"{path}.pdf");
        }

        [Fact]
        public void FilesAreNotImages()
        {
            var images = Directory.GetFiles(Samples.IMAGESDIR);
            images = [.. images, Samples.FAKEIMAGE];

            Assert.ThrowsAny<Exception>(() => ComicBuilder.CreatePdf(images, path));
        }
    }
}