using System;
using System.IO;
using ComicConverter;
using Xunit;

namespace Test.ImageCollector
{
    public class PdfFiles
    {
        [Fact]
        public void ExtractImagesPdfJpg()
        {
            ImageExtractors.ExtractPdfImages(Samples.PDFPATH, "PdfExtractedImages");

            var files = Directory.GetFiles("PdfExtractedImages");

            Assert.Equal(3, files.Length);

            Directory.Delete("PdfExtractedImages", true);
        }

        [Fact]
        public void FileIsNotPdf()
        {
            Assert.Throws<FormatException>(() => ImageExtractors.ExtractPdfImages(Samples.CBRPATH));
        }

        [Fact]
        public void FileNotFound()
        {
            Assert.Throws<FileNotFoundException>(() => ImageExtractors.ExtractPdfImages("FakeFile.pdf"));
        }

        [Fact]
        public void EmptyAttrubuteDirectory()
        {
            ImageExtractors.ExtractPdfImages(Samples.PDFPATH, "PdfExtracted");

            var files = Directory.GetFiles("PdfExtracted");

            Assert.Equal(3, files.Length);

            Directory.Delete("PdfExtracted", true);
        }

        [Fact]
        public void EmptyStringDirectory()
        {
            Assert.Throws<FormatException>(() => ImageExtractors.ExtractPdfImages(Samples.PDFPATH, ""));
        }
    }
}