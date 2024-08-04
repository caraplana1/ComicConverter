using Xunit;
using System;
using System.IO;
using ComicConverter;
using ComicConverter.Enums;

namespace Test
{
    public class ComicConverter
    {
        private const string ComicName = "Test";

        [Fact]
        public void ConvertCbr2Cbz()
        {
            Comic comic = new(Samples.Cbrpath);

            comic.Convert(ComicName, ComicFormat.Cbz);

            Assert.True(File.Exists($"{ComicName}.cbz"));

            File.Delete($"{ComicName}.cbz");
        }

        [Fact]
        public void ConvertCbz2Cbt()
        {
            Comic comic = new(Samples.Cbzpath);

            comic.Convert(ComicName, ComicFormat.Cbt);

            Assert.True(File.Exists($"{ComicName}.cbt"));

            File.Delete($"{ComicName}.cbt");
        }

        [Fact]
        public void ConvertCbr2Pdf()
        {
            Comic comic = new(Samples.Cbrpath);

            comic.Convert("Pdf", ComicFormat.Pdf);

            Assert.True(File.Exists("Pdf.pdf"));

            File.Delete("Pdf.pdf");
        }

        [Fact]
        public void ConvertPdf2Cbz()
        {
            Comic comic = new(Samples.Pdfpath);

            comic.Convert("pdf2cbz", ComicFormat.Cbz);

            Assert.True(File.Exists("pdf2cbz.cbz"));

            File.Delete("pdf2cbz.cbz");
        }

        [Fact]
        public void OutInvalidFormat()
        {
            Comic comic = new(Samples.Cbzpath);

            Assert.Throws<FormatException>(() => comic.Convert(ComicName, ComicFormat.Cbr));
        }

        [Fact]
        public void InInvalidFormat()
        {
            Comic comic;

            Assert.Throws<FormatException>(() => comic = new(Samples.Testpath));
        }
    }
}