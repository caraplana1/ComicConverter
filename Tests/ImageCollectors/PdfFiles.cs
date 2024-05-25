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
            ImageExporter.ExportPdfImages(Samples.PDFPATH, "PdfExtractedImages");
            ImageExporter.ExportPdfImages(Samples.PDFPNGPATH, "PdfPNGExtractedImages");

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
            Assert.Throws<FormatException>(() => ImageExporter.ExportPdfImages(Samples.CBRPATH));
        }

        [Fact]
        public void FileNotFound()
        {
            Assert.Throws<FileNotFoundException>(() => ImageExporter.ExportPdfImages("FakeFile.pdf"));
        }

        [Fact]
        public void EmptyAttrubuteDirectory()
        {
            ImageExporter.ExportPdfImages(Samples.PDFPATH, "PdfExtracted");

            var files = Directory.GetFiles("PdfExtracted");

            Assert.Equal(3, files.Length);

            Directory.Delete("PdfExtracted", true);
        }

        [Fact]
        public void EmptyStringDirectory()
        {
            Assert.Throws<FormatException>(() => ImageExporter.ExportPdfImages(Samples.PDFPATH, ""));
        }
    }
}