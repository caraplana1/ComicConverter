using Xunit;
using System;
using System.IO;
using ComicConverter;

namespace Test
{
    public class ComicConverter
    {
        private const string ComicName = "Test";

        [Fact]
        public void ConvertCbr2Cbz()
        {
            Comic comic = new(Samples.CBRPATH);

            comic.Convert(ComicName, ComicFormat.CBZ);

            Assert.True(File.Exists($"{ComicName}.cbz"));

            File.Delete($"{ComicName}.cbz");
        }

        [Fact]
        public void ConvertCbz2Cbt()
        {
            Comic comic = new(Samples.CBZPATH);

            comic.Convert(ComicName, ComicFormat.CBT);

            Assert.True(File.Exists($"{ComicName}.cbt"));

            File.Delete($"{ComicName}.cbt");
        }

        [Fact]
        public void ConvertCbr2Pdf()
        {
            Comic comic = new(Samples.CBRPATH);

            comic.Convert("Pdf", ComicFormat.PDF);

            Assert.True(File.Exists("Pdf.pdf"));

            File.Delete("Pdf.pdf");
        }

        [Fact]
        public void ConvertPdf2Cbz()
        {
            Comic comic = new(Samples.PDFPATH);

            comic.Convert("pdf2cbz", ComicFormat.CBZ);

            Assert.True(File.Exists("pdf2cbz.cbz"));

            File.Delete("pdf2cbz.cbz");
        }

        [Fact]
        public void OutInvalidFormat()
        {
            Comic comic = new(Samples.CBZPATH);

            Assert.Throws<FormatException>(() => comic.Convert(ComicName, ComicFormat.CBR));
        }

        [Fact]
        public void InInvalidFormat()
        {
            Comic comic;

            Assert.Throws<FormatException>(() => comic = new(Samples.TESTPATH));
        }
    }
}