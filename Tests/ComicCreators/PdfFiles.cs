using Test;
using Xunit;
using System;
using System.IO;
using ComicConverter;

namespace Tests.ComicCreators
{
    public class PdfFiles
    {
        private const string Path = "pdftestBuild";

        [Fact]
        public void CreatePdf()
        {
            var images = Directory.GetFiles(Samples.Imagesdir);

            ComicBuilder.CreatePdf(images, Path);

            Assert.True(File.Exists($"{Path}.pdf"));

            File.Delete($"{Path}.pdf");
        }

        [Fact]
        public void FilesAreNotImages()
        {
            var images = Directory.GetFiles(Samples.Imagesdir);
            images = [.. images, Samples.Fakeimage];

            Assert.ThrowsAny<Exception>(() => ComicBuilder.CreatePdf(images, Path));
        }
        
        [Fact]
        public void ImagesNotFound()
        {
            string[] files = ["Fakename", "fakename2", "fakename3"];

            Assert.Throws<IOException>(() => ComicBuilder.CreatePdf(files, Path));
        }
    }
}