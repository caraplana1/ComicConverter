using System;
using System.IO;
using ComicConverter;
using Xunit;

namespace Test.ImageCollector
{
    public class PdfFiles
    {
        [Fact]
        public void ExtractImagesPdf()
        {
            string pdfDir = "PdfExtractedImages";
            string pdfPngDir = "PdfPNGExtractedImages";

            ImageExporter.ExportPdfImages(Samples.PDFPATH, pdfDir);
            ImageExporter.ExportPdfImages(Samples.PDFPNGPATH, pdfPngDir);

            var files = Directory.GetFiles(pdfDir).Length;
            var filesPNG = Directory.GetFiles(pdfPngDir).Length;

            Directory.Delete(pdfDir, true);
            Directory.Delete(pdfPngDir, true);

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
            string pdfExport = "PdfExported";

            ImageExporter.ExportPdfImages(Samples.PDFPATH, pdfExport);

            var files = Directory.GetFiles(pdfExport);
            Directory.Delete(pdfExport, true);

            Assert.Equal(3, files.Length);
        }

        [Fact]
        public void EmptyStringDirectory()
        {
            Assert.Throws<FormatException>(() => ImageExporter.ExportPdfImages(Samples.PDFPATH, ""));
        }
    }
}