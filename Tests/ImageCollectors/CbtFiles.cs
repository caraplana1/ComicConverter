using System.IO;
using System;
using System.Linq;
using ComicConverter;
using Xunit;

namespace Test.ImageCollector
{
    public class CbtFiles
    {
        [Fact]
        public void ExtractImages()
        {
            ImageExporter.UnTar(Samples.CBTPATH, "CbtTest1");
            Assert.Equal(3, Directory.GetFiles("CbtTest1").Length);
            Directory.Delete("CbtTest1", true);
        }

        [Fact]
        public void FilesIsNotTar()
        {
            Assert.Throws<FormatException>(() => ImageExporter.UnTar(Samples.CBRPATH));
        }

        [Fact]
        public void FileNotFound()
        {
            Assert.Throws<FileNotFoundException>(() => ImageExporter.UnTar(Samples.FAKEFILE));
        }

        [Fact]
        public void EmptyAttributeDirectory()
        {
            ImageExporter.UnTar(Samples.CBTPATH, "cbtTest2");
            ImageExporter.UnTar(Samples.CBTPATH);

            var files = Directory.GetFiles("cbtTest2").ToList();
            var filesCurrentDir = Directory.GetFiles(".").ToList();

            foreach (var file in files)
            {
                Assert.Contains(file.Replace("cbtTest2", "."), filesCurrentDir);
                File.Delete(file.Replace("cbtTest2", "."));
            }

            Directory.Delete("cbtTest2", true);
        }

        [Fact]
        public void EmptyStringDirectory()
        {
            Assert.Throws<FormatException>(() => ImageExporter.UnTar(Samples.CBTPATH, ""));
        }
    }
}