using Test;
using Xunit;
using System;
using System.IO;
using ComicConverter;

namespace Tests.ImageCollectors
{
    public class PdfFiles
    {
        [Fact]
        public void ExtractImagesJpgPdf()
        {
            string pdfDir = "PdfExtractedImages";
            ImageExporter.ExportPdfImages(Samples.Pdfpath, pdfDir);
            var files = Directory.GetFiles(pdfDir).Length;

            Directory.Delete(pdfDir, true);

            Assert.Equal(3, files);

        }

        [Fact]
        public void ExtractImagesPngPdf()
        {
            string pdfPngDir = "PdfPNGExtractedImages";
            ImageExporter.ExportPdfImages(Samples.Pdfpngpath, pdfPngDir);
            var filesPng = Directory.GetFiles(pdfPngDir).Length;
            
            Directory.Delete(pdfPngDir, true);
            
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