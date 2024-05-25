using Xunit;
using System.IO;
using ComicConverter;
using System.Collections.Generic;

namespace Test.ImageCollector
{
    public class CbrFiles
    {
        private const string outputDir = "ImagesFromRarDir";

        [Fact]
        public void Unrar()
        {
            string[] extractedFiles;

            ImageExporter.UnRar(Samples.CBRPATH, outputDir);

            extractedFiles = Directory.GetFiles(outputDir);
            Directory.Delete(outputDir, true);
            Assert.True(extractedFiles.Length > 0);
        }

        [Fact]
        public void FileIsNotRar()
        {
            Assert.Throws<System.FormatException>(() => ImageExporter.UnRar(Samples.TESTPATH, outputDir));
        }

        [Fact]
        public void FileNotFound()
        {
            Assert.Throws<FileNotFoundException>(() => ImageExporter.UnRar(Samples.FAKEFILE, outputDir));
        }

        [Fact]
        public void EmptyAttributeDirectory()
        {
            ImageExporter.UnRar(Samples.CBRPATH, "Folder");
            ImageExporter.UnRar(Samples.CBRPATH);

            List<string> filesExtracted = [.. Directory.GetFiles(".")];

            foreach (var file in Directory.GetFiles("Folder"))
            {
                if (!filesExtracted.Contains(file.Replace("Folder", ".")))
                    Assert.True(false);
                else
                    File.Delete(file.Replace("Folder", "."));
            }

            Directory.Delete("Folder", true);
        }

        [Fact]
        public void EmptyStringDirectory()
        {
            Assert.Throws<System.FormatException>(() => ImageExporter.UnRar(Samples.CBRPATH, ""));
        }
    }
}
