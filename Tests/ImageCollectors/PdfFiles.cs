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
            ImageExtractors.ExtractPdfImages(Samples.PDFPNGPATH, "PdfPNGExtractedImages");

            var files = Directory.GetFiles("PdfExtractedImages").Length;
            var filesPNG = Directory.GetFiles("PdfPNGExtractedImages").Length;

            Directory.Delete("PdfExtractedImages", true);
            Directory.Delete("PdfPNGExtractedImages", true);

            Assert.Equal(3, files);
            Assert.Equal(3, filesPNG);

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