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

            ImageExporter.ExportPdfImages(Samples.Pdfpath, pdfDir);
            ImageExporter.ExportPdfImages(Samples.Pdfpngpath, pdfPngDir);

            var files = Directory.GetFiles(pdfDir).Length;
            var filesPng = Directory.GetFiles(pdfPngDir).Length;

            Directory.Delete(pdfDir, true);
            Directory.Delete(pdfPngDir, true);

            Assert.Equal(3, files);
            Assert.Equal(3, filesPng);

        }

        [Fact]
        public void FileIsNotPdf()
        {
            Assert.Throws<FormatException>(() => ImageExporter.ExportPdfImages(Samples.Cbrpath));
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

            ImageExporter.ExportPdfImages(Samples.Pdfpath, pdfExport);

            var files = Directory.GetFiles(pdfExport);
            Directory.Delete(pdfExport, true);

            Assert.Equal(3, files.Length);
        }

        [Fact]
        public void EmptyStringDirectory()
        {
            Assert.Throws<FormatException>(() => ImageExporter.ExportPdfImages(Samples.Pdfpath, ""));
        }
    }
}